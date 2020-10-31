using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameHost
{
    public class ClientPerson : GenericCharacter
    {
        
        public int client;
        private GameServer parent;
        Room location;
        string el = "\r\n";
        public string databaseId;
        public string phone;
        string fullMessage;
        string final = " \0";
        public List<Item> inventory;
        string answer = "";
        internal List<MenuChoice> Menu;
        internal Attack m_MyFight;
        public bool m_loggedin;
        Random specialAttack = new Random((int)DateTime.Now.Ticks);

        public ClientPerson(GameServer p, string ph, string dbid)
        {
            m_name = "";
            m_description = "";
            parent = p;
            phone = ph;
            databaseId = "";
            inventory = new List<Item>();
            //health = 0;
            Menu = new List<MenuChoice>();
            m_charType = 1;
            m_loggedin = false;



            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                client newClient;

                //we have a web player
                newClient = (from cl in gdb.clients
                             where cl.id == Convert.ToInt32(dbid)
                             select cl).SingleOrDefault();
                m_loggedin = (bool)newClient.logged_in;

                if (newClient != null)
                {

                    m_description = newClient.description;
                    m_name = newClient.name;
                    databaseId = newClient.id.ToString();
                    health = (int)newClient.health;
                    sM(String.Format("Connecting... {0}.\r\n", m_name));
                    GetInventory();  //get his stuff
                    location = parent.map.dropInLocation((int)newClient.room_x, (int)newClient.room_y, this);
                    newClient.is_playing = true;
                    gdb.SubmitChanges();
                    sM("What do you want to do?\r\n\0");
                }
                else
                {
                    //new user
                    sM("What is your name, traveler?\0" + el);
                    //creation = true;
                    answer = "creation";
                }
            }
            parent.UpdateClientListControl();
            if (health <=0 )
            {
                health = 50;
                sM("Resurrecting your player...\r\nYou are now at full health, please be more careful next time.\r\n\0");
            }
        }

        public int health
        {
            get
            {
                return (int)m_health;
            }
            set
            {
                using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
                {
                    if (gdb.DatabaseExists())
                    {
                        var q6 = (from clts in gdb.clients
                                  where clts.id == Convert.ToInt32(databaseId)
                                  select clts).SingleOrDefault();
                        q6.health = value;
                        gdb.SubmitChanges();
                    }
                    else
                    {
                        //oops couldn't report that the health changed to the database
                        parent.AppendToRichEditControl("\r\nTried to report health of a client to the database and faild because the database isn't connected.\r\n");
                    }
                }
                m_health = value;
            }
        }
        private void UpdateDatabase()
        {
            ///
            ///Updates the whole user object back to the database except location (health only until updated)
            /// 
            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                if (gdb.DatabaseExists())
                {

                    var q6 = (from clts in gdb.clients
                              where clts.id == Convert.ToInt32(databaseId)
                              select clts).SingleOrDefault();
                    q6.health = m_health;
                    gdb.SubmitChanges();
                }
                else
                {
                    parent.AppendToRichEditControl("\r\nUpdating the client in the database failed.  No connection in Clientperson.UpdateDatabase()\r\n");
                }
            }
        }

        override internal MenuChoice GetSpecialAttack()
        {
            int probablity = specialAttack.Next(1, 3);
            if (probablity == 2)
                return new MenuChoice("Groin Kick", "groinkick");
            else
                return null;
        }

        private void CreateNewUser(string msg)
        {
            //all messages should hit here until the user is created so we have all the info, set creation to false
            if (m_name != "" & m_description == "")
            {
                m_description = msg;
                health = 50;

                sM(String.Format("So, {0}, you look like {1}.\0", m_name, m_description) + el);
                location = parent.map.newPlayer(this);

                //add the info we have so far to the database
                using (GameDatabaseDataContext gd = new GameDatabaseDataContext())
                {
                    GameHost.client cl = new GameHost.client
                    {
                        ip_add = "0",
                        phone = phone,
                        first_connect = DateTime.Now,
                        last_connect = DateTime.Now,
                        last_touch = DateTime.Now,
                        name = m_name,
                        description = m_description,
                        room_x = 4,
                        room_y = 4,
                        roomid = 37,
                        health = 50,
                        is_playing = true
                    };
                    gd.clients.InsertOnSubmit(cl);
                    gd.SubmitChanges();
                    databaseId = cl.id.ToString();
                }
                    

                sM("What do you want to do?\0"+ el);
                if (answer == "creation")
                    answer="";

            }

            if (m_name == "")
            {
                m_name = msg;
                sM("Hello, " + m_name + "!\r\n What do you look like?\r\n\0");
            }

        }


        public void processMessage(string msg)
        {
            if(msg == "reload map")
            {
                Process.Start(Application.ExecutablePath);
                Application.Exit();
            }
            msg = msg.ToLower();
            msg = msg.Replace("'", "");
            if (!msg.StartsWith("say "))
            {
                msg = msg.Replace("and ", "");
                msg = msg.Replace("the ", "");
                msg = msg.Replace("at ", "");

            }

            char[] delimiterChars = {' '};
            string[] words = msg.Split(delimiterChars);

                              
                switch (answer)
                {
                    case "dead":
                    if (msg != "heal")
                    {
                        sM("You are dead.  Type heal to ressurect.\r\n\0");
                    }
                    else
                    {
                        health = 50;
                        answer = "";
                        sM("Your sandroid repaired itself.\r\n\0");
                    }
                    break;
                    case "looking":
                    try
                    {
                        if (Convert.ToInt32(words[0]) <= Menu.Count && Convert.ToInt32(words[0]) != 0)
                        {

                            int choice = Convert.ToInt32(words[0]);
                            Item lookAtThis = location.IsItemInHere(Menu[choice - 1].m_item, Menu[choice - 1].m_modifier);
                            sM(String.Format("{0}\r\n{1}\r\n\0", lookAtThis.m_name.Replace("_", " "), lookAtThis.m_description));
                            Menu.Clear();
                            answer = "";  //since we got our answer
                        }
                        else
                        {
                            sM("Please choose a menu item.\r\n\0");
                        }
                    }
                    catch (Exception)
                    {
                        sM("Please choose a menu item.\r\n\0");
                    }
                    answer = "";
                    break;
                    case "getting":
                    //we are getting an item 
                    try
                    {
                        if (Convert.ToInt32(words[0]) <= Menu.Count && Convert.ToInt32(words[0]) != 0)
                        {

                            int choice = Convert.ToInt32(words[0]);
                            Item getThis = location.IsItemInHere(Menu[choice - 1].m_item, Menu[choice - 1].m_modifier);
                            GetItem(getThis);
                            sM(String.Format("You picked up the {1}({0}).\r\n\0", Item.GetHealthDescription(getThis.m_health), getThis.m_name.Replace("_", " ")));
                            location.SendOthersStatus(this, String.Format("{0} just picked up a {1}.\r\n\0", m_name, getThis.m_name.Replace("_", " ")));
                            Menu.Clear();
                            answer = "";  //since we got our answer
                        }
                        else
                        {
                            sM("Please choose a menu item.\r\n\0");
                        }
                    }
                    catch (Exception)
                    {
                        sM("Please choose a menu item.\r\n\0");
                    }
                    answer = "";
                    break;
                    case "dropping":
                    //we are dropping an item 
                    try
                    {
                        if (Convert.ToInt32(words[0]) <= Menu.Count && Convert.ToInt32(words[0]) != 0)
                        {

                            int choice = Convert.ToInt32(words[0]);
                            Item dropThis = HasThis(Menu[choice - 1].m_item, Menu[choice - 1].m_modifier);
                            DropItem(dropThis,false);
                            sM(String.Format("You dropped the {1}({0}).\r\n\0", Item.GetHealthDescription(dropThis.m_health), dropThis.m_name.Replace("_", " ")));
                            location.SendOthersStatus(this, String.Format("{0} just dropped a {1}.\r\n\0", m_name, dropThis.m_name.Replace("_", " ")));
                            Menu.Clear();
                            answer = "";  //since we got our answer
                        }
                        else
                        {
                            sM("Please choose a menu item.\r\n\0");
                        }
                    }
                    catch (Exception)
                    {
                        sM("Please choose a menu item.\r\n\0");
                    }
                    answer = "";
                    break;  
                    case "attacking":
                    //send the message to the attack object
                    m_MyFight.ProcessAttackMsg(this, msg, words);
                    break;
                    case "creation":   //ignore since we are still creating this character
                        CreateNewUser(msg);
                        break;
                    case "open":
                    case "unlock":
                    case "close":
                    case "lock":
                        //we are trying to operate a door and don't know which one.
                        //This would be the answer the the door menu.
                        //get the current menu
                        try
                        {
                            if (Convert.ToInt32(words[0]) <= Menu.Count && Convert.ToInt32(words[0]) != 0)
                            {

                                int choice = Convert.ToInt32(words[0]);
                                OperateDoor(Menu[choice - 1].m_action, answer);
                                Menu.Clear();
                                answer = "";  //since we got our answer
                            }
                            else
                            {
                                sM("Please choose a menu item.\r\n\0");
                            }
                        }
                        catch (Exception)
                        {
                            sM("Please choose a menu item.\r\n\0");
                        }
                        break;
                    default:
                        switch (words[0])
                        {
                            case "north":
                            case "n":
                            case "south":
                            case "s":
                            case "east":
                            case "e":
                            case "west":
                            case "w":
                                Move(words[0]);
                                break;
                            case "go":
                                switch (words[1])
                                {
                                    case "n":
                                    case "north":
                                    case "e":
                                    case "east":
                                    case "s":
                                    case "south":
                                    case "w":
                                    case "west":
                                        Move(words[0]);
                                        break;
                                }
                                break;
                            case "look":
                                Look(msg, words);
                                //sM(final);
                                break;
                            case "say":
                                string sayMessage = msg.Replace("say ", "");
                                location.MessageOthers(this, sayMessage);
                                //sM(final);
                                break;
                            case "open":
                                Open(msg, words);
                                break;
                            case "close":
                                Close(msg, words);
                                break;
                            case "lock":
                                Lock(msg,words);
                                break;
                            case "unlock":
                                Unlock(msg, words);
                                break;
                            case "exit":
                                Exit();
                                break;
                            default:
                                ProcessOtherAction(msg, words);
                                break;
                        }
                        break;
                }
        }

        private void Lock(string msg, string[] words)
        {
            if (words.Length == 2)
            {
                if (words[1] == "door")
                {
                    //is there only one locked door in the room?
                    int unlockeddoorcount = 0;
                    for (int i = 1; i<5; i++)
                    {
                        if (parent.map.moveToLocation(i.ToString(), location).Nexit == 4)
                            unlockeddoorcount++;
                    }

                    switch (unlockeddoorcount)
                    {
                        case 0:
                            //there arent any closed doors!  Let them know
                            sM("You don't see any unlocked doors here." + el);
                            sM(final);
                            break;
                        case 1:
                            //there is only one open door, so open it
                            for (int i = 1; i < 5; i++)
                            {
                                //find the open door, there is only 1
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit == 4)
                                    OperateDoor(i.ToString(), "lock");
                            }
                            break;
                        default:
                            //present the menu, as we have more than 1 closed door
                            sM("Which door would you like to lock?" + el);
                            for (int i = 1; i < 5; i++)
                            {
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit == 4)
                                {
                                    switch (i)
                                    {
                                        case 1:  //north
                                            Menu.Add(new MenuChoice("North Door", "north"));
                                            break;
                                        case 2:  //east
                                            Menu.Add(new MenuChoice("East Door", "east"));
                                            break;
                                        case 3:  //south
                                            Menu.Add(new MenuChoice("South Door", "south"));
                                            break;
                                        case 4:  //west
                                            Menu.Add(new MenuChoice("West Door", "west"));
                                            break;
                                    }
                                }
                            }
                            int j = 1;
                            foreach (MenuChoice choice in Menu)
                            {
                                sM(String.Format("{0}. Lock the {1}\r\n", j, choice.m_name));
                                j++;
                            }
                            sM(final);
                            answer = "lock";
                            break;
                    }
                }
            }
            else
                sM("Lock what?\r\n\0");
        }

        private void Unlock(string msg, string[] words)
        {
            if (words.Length == 2)
            {
                if (words[1] == "door")
                {
                    //is there only one locked door in the room?
                    int lockeddoorcount = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        if (parent.map.moveToLocation(i.ToString(), location).Nexit == 5)
                            lockeddoorcount++;
                    }

                    switch (lockeddoorcount)
                    {
                        case 0:
                            //there arent any closed doors!  Let them know
                            sM("You don't see any locked doors here." + el);
                            sM(final);
                            break;
                        case 1:
                            //there is only one open door, so open it
                            for (int i = 1; i < 5; i++)
                            {
                                //find the open door, there is only 1
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit == 5)
                                    OperateDoor(i.ToString(), "unlock");
                            }
                            break;
                        default:
                           //fix this !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //present the menu, as we have more than 1 closed door
                            sM("Which door would you like to unlock?" + el);
                            for (int i = 1; i < 5; i++)
                            {
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit == 5)
                                {
                                    switch (i)
                                    {
                                        case 1:  //north
                                            Menu.Add(new MenuChoice("North Door", "north"));
                                            break;
                                        case 2:  //east
                                            Menu.Add(new MenuChoice("East Door", "east"));
                                            break;
                                        case 3:  //south
                                            Menu.Add(new MenuChoice("South Door", "south"));
                                            break;
                                        case 4:  //west
                                            Menu.Add(new MenuChoice("West Door", "west"));
                                            break;
                                    }
                                }
                            }
                            int j = 1;
                            foreach (MenuChoice choice in Menu)
                            {
                                sM(String.Format("{0}. Unlock the {1}\r\n", j, choice.m_name));
                                j++;
                            }
                            sM(final);
                            answer = "unlock";
                            break;
                    }
                }
            }
            else
                sM("Unlock what?\r\n\0");
        }

        private void Close(string msg, string[] words)
        {
            if (words.Length == 2)
            {
                if (words[1] == "door")
                {
                    //is there only one open door in the room?

                    int opendoorcount = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        if (parent.map.moveToLocation(i.ToString(), location).Nexit < 3 & parent.map.moveToLocation(i.ToString(), location).Nexit != 0)
                            opendoorcount++;
                    }

                    switch (opendoorcount)
                    {
                        case 0:
                            //there arent any closed doors!  Let them know
                            sM("You don't see any open doors here." + el);
                            sM(final);
                            break;
                        case 1:
                            //there is only one open door, so open it
                            for (int i = 1; i < 5; i++)
                            {
                                //find the open door, there is only 1
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit < 3 & parent.map.moveToLocation(i.ToString(), location).Nexit != 0)
                                {
                                    //this is the closed door, open it if it isn't locked  //now we have to find the adjacent door and open that as well
                                        switch (i)
                                        {

                                            case 1://north
                                                OperateDoor("north", "close");
                                                break;
                                            case 2://east
                                                OperateDoor("east", "close");
                                                break;
                                            case 3://south
                                                OperateDoor("south", "close");
                                                break;
                                            case 4://west
                                                OperateDoor("west", "close");
                                                break;
                                        }

                                }
                            }
                            break;
                        default:
                            //present the menu, as we have more than 1 closed door
                            sM("Which door would you like to close?" + el);
                            for (int i = 1; i < 5; i++)
                            {
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit != 0 & parent.map.moveToLocation(i.ToString(), location).Nexit < 3)
                                {
                                    switch (i)
                                    {
                                        case 1:  //north
                                            Menu.Add(new MenuChoice("North Door", "north"));
                                            break;
                                        case 2:  //east
                                            Menu.Add(new MenuChoice("East Door", "east"));
                                            break;
                                        case 3:  //south
                                            Menu.Add(new MenuChoice("South Door", "south"));
                                            break;
                                        case 4:  //west
                                            Menu.Add(new MenuChoice("West Door", "west"));
                                            break;
                                    }
                                }
                            }
                            int j = 1;
                            foreach (MenuChoice choice in Menu)
                            {
                                sM(String.Format("{0}. Close the {1}\r\n", j, choice.m_name));
                                j++;
                            }
                            sM(final);
                            answer = "close";
                            break;
                    }
                }
                else
                {
                    sM("I don't understand what you would like me to close... Maybe a door?\r\n\0");
                }
            }
            else
                sM("Close what?\r\n\0");
        }


        private void OperateDoor(string door,string action)
        {
            if (action == "open")
            {
                parent.map.moveToLocation(door, location).Nexit -= 2;
                sM(String.Format("You opened the {0} door.\r\n\0", door));
            }
            if (action == "close")
            {
                parent.map.moveToLocation(door, location).Nexit += 2;
                sM(String.Format("You closed the {0} door.\r\n\0", door));
            }
            if (action == "unlock")
            {
                //try and unlock the door, first check if you have the right key
                if (CheckKey(parent.map.moveToLocation(door, location).NKey))
                {
                    parent.map.moveToLocation(door, location).NKey = 4;
                    sM(String.Format("You unlocked the {0} door.\r\n\0",door));
                }
                else
                    sM("You don't have the key to that door.\r\n\0");
            }

            if (action == "lock")
            {
                //try and unlock the door, first check if you have the right key
                if (CheckKey(parent.map.moveToLocation(door, location).NKey))
                {
                    parent.map.moveToLocation(door, location).NKey = 5;
                    sM(String.Format("You locked the {0} door.\r\n\0", door));
                }
                else
                    sM("You don't have the key to that door.\r\n\0");
            }
        }

        private bool CheckKey(int? p)
        {
            //check the inventory for a matching key
            foreach (Item i in inventory)
            {
                if (i.m_type == 5 && i.m_code == p)
                    return true;
            }
            return false;
        }


        private void Open(string msg, string[] words)
        {
            if (words.Length == 2)
            {
                if (words[1] == "door")
                {
                    //is there only one closed door in the room?
                    int closeddoorcount = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        if (parent.map.moveToLocation(i.ToString(), location).Nexit > 2 & parent.map.moveToLocation(i.ToString(), location).Nexit < 6)
                            closeddoorcount++;
                    }

                    switch (closeddoorcount)
                    {
                        case 0:
                            //there arent any closed doors!  Let them know
                            sM("You don't see any closed doors here." + el);
                            sM(final);
                            break;
                        case 1:
                            //there is only one closed door, so open it
                            for (int i=1; i<5; i++)
                            {
                                //find the closed door, there is only 1
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit > 2 & parent.map.moveToLocation(i.ToString(), location).Nexit < 6)
                                {
                                    //this is the closed door, open it if it isn't locked  
                                    if (parent.map.moveToLocation(i.ToString(), location).Nexit != 5)
                                    {
                                        switch (i)
                                        {
                                            
                                            case 1://north
                                                OperateDoor("north","open");
                                                break;
                                            case 2://east
                                                OperateDoor("east","open");
                                                break;
                                            case 3://south
                                                OperateDoor("south", "open");
                                                break;
                                            case 4://west
                                                OperateDoor("west", "open");
                                                break;
                                        }

                                    }
                                    else
                                    {
                                        //the door is locked dont do anything and tell the people
                                        sM("The door is locked." + el);
                                        sM(final);
                                    }
                                }
                            }
                            break;
                        default:
                            //present the menu, as we have more than 1 closed door
                            sM("Which door would you like to open?" + el);
                            for (int i = 1; i < 5; i++)
                            {
                                if (parent.map.moveToLocation(i.ToString(), location).Nexit > 2 & parent.map.moveToLocation(i.ToString(), location).Nexit < 6)
                                {
                                    switch (i)
                                    {
                                        case 1:  //north
                                            Menu.Add(new MenuChoice("North Door","north"));
                                                break;
                                        case 2:  //east
                                            Menu.Add(new MenuChoice("East Door", "east"));
                                            break;
                                        case 3:  //south
                                            Menu.Add(new MenuChoice("South Door", "south"));
                                            break;
                                        case 4:  //west
                                            Menu.Add(new MenuChoice("West Door", "west"));
                                            break;
                                    }
                                }
                            }
                            int j=1;
                            foreach (MenuChoice choice in Menu)
                            {
                                sM(String.Format("{0}. Open the {1}\r\n",j,choice.m_name));
                                j++;
                            }
                            sM(final);
                            answer = "open";
                            break;
                    }
                }
             }
            else
                sM("Open what?\r\n\0");
        }




        private void ProcessOtherAction(string msg, string[] words)
        {
            try
            {
                //check actions list for primary actions (single line actions that use 1 word or the whole message)
                var actionsQuery = from acts in parent.m_actions
                                   where acts.action1 == msg
                                   select acts;
                if (actionsQuery.Count() > 0) // we found an action that is a single line
                {
                    foreach (action a in actionsQuery)
                    {
                        if (a.text) //its a simple text response so just send it.
                        {
                            sM(a.response + el);
                            sM(final);
                        }
                        else
                        {
                            ProcessDbCommand(a.response, words); // this response needs processing so sending a number and the words
                        }
                    }
                }
                else
                {
                    //first check for commands that use secondary actions/ items if no respond with i don't get it.
                    string response = (from resp in parent.m_actions
                                       where resp.action1 == words[0] &&
                                       resp.requires_item == true
                                       select resp.response).SingleOrDefault();
                    if (response != null)
                    {
                        ProcessDbCommand(response, words);
                    }
                    else
                    {
                        sM("I don't understand. Try something else." + el);
                        sM(final);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Look(string msg, string[] words)
        {
            if (words == null || words.Length == 1)
            {
                sM(location.m_name + el, "caps");
                sM(string.Format("{0} {1}\r\n", location.description,location.m_longDescription));
                sM(parent.map.getExitString(location) + el);
                location.othersMsg(this);
                sM(final);
            }
            else
            {
                
                switch (words[1])
                {
                    case "self":
                    case "me":
                    case "myself":
                        //look at self
                        sM(string.Format("{0} ({1}){2}", m_name, GetHealthString(), el), "caps");

                        //items
                        var groups = from i in inventory
                                     group i by new { i.m_type, i.m_name, i.HealthDesc };

                        foreach (var group in groups)
                        {
                            if (group.Count() == 1)
                            {
                                sM(String.Format("You have a {1}({0}).\r\n", group.Key.HealthDesc, group.Key.m_name.Replace("_", " ")));
                            }
                            else
                            {
                                sM(String.Format("You have {1} {2}s({0}).\r\n", group.Key.HealthDesc, group.Count(), group.Key.m_name.Replace("_", " ")));
                            }
                        }
                        sM("\r\n\0");
                        break;
                    case "room":
                    case "around":
                        //look and list items in room
                        sM(location.m_name + el, "caps");
                        sM(string.Format("{0} {1}\r\n", location.description, location.m_longDescription));
                        sM(parent.map.getExitString(location) + el);
                        location.othersMsg(this);
                        sM("\r\n\0");
                        break;
                    default:
                        //we could be looking at someone else, so see if they exist in the room
                        ClientPerson lookee = location.findPerson(words[1]);
                        if (lookee != null)
                        {
                            //found them!
                            sM(lookee.m_name + el, "caps");
                            sM(lookee.GetHealthString() + el);
                            sM(final);
                            break;
                        }
                        else
                        {
                            //we need to check the room we are in to see if it has an item to look at
                            //gets a bit more complicated as it could be a 2 word item or monster after this
                            //so use the original message minus the look and replace the spaces with underscores.
                            Item lookAtThis = null;
                            string itemsearch = null;
                            if (Item.m_descriptors.Contains(words[1]))
                            {
                                //we have a health descriptor
                                //itemsearch should now contain the rest of the words in the array.
                                for (int x = 2; x < words.Length; x++)
                                {
                                    itemsearch += words[x] + "_";
                                }
                                itemsearch = itemsearch.TrimEnd('_');
                                lookAtThis = location.IsItemInHere(itemsearch, words[1]);
                            }
                            else
                            {
                                //itemsearch should now contain the rest of the words in the array.
                                for (int x = 1; x < words.Length; x++)
                                {
                                    itemsearch += words[x] + "_";
                                }
                                itemsearch = itemsearch.TrimEnd('_');
                                List<Item> list = location.IsItemInHere(itemsearch);
                                if (list != null)
                                {
                                    if (list.Count == 1)
                                    {
                                        lookAtThis = list[0];
                                    }
                                    else
                                    {
                                        //now we have to group by again and find if the items are the same

                                        var lgroups = from i in list
                                                      group i by new { i.m_type, i.m_name, i.m_location_desc, i.HealthDesc };

                                        if (lgroups.Count() == 1)
                                        {
                                            //there is only 1 type of the item so drop it
                                            lookAtThis = list.First();
                                        }
                                        else
                                        {

                                            foreach (var group in lgroups)
                                            {
                                                //oops there is more than one match due to health, create a menu and ask.
                                                Menu.Add(new MenuChoice(group.Key.HealthDesc + " " + group.Key.m_name, group.Key.m_name, "look", group.Key.HealthDesc));
                                            }
                                            sM("Which one?\r\n");
                                            int j = 1;
                                            foreach (MenuChoice choice in Menu)
                                            {
                                                sM(String.Format("{0}. Look at the {1}\r\n", j, choice.m_name.Replace("_", " ")));
                                                j++;
                                            }
                                            sM("\r\n\0");
                                            answer = "looking";
                                        }
                                    }
                                }
                            }
                            if (answer != "looking")
                            {
                                if (lookAtThis != null)
                                {
                                    sM(String.Format("{0}\r\n{1}\r\n\0", lookAtThis.m_name.Replace("_", " "), lookAtThis.m_description));
                                    break;
                                }
                                else
                                {
                                    //we need to check the room we are in to see if it has an item to look at
                                    //gets a bit more complicated as it could be a 2 word item or monster after this
                                    //so use the original message minus the look and replace the spaces with underscores.

                                    NPC npc = location.IsNPCInHere(itemsearch);
                                    if (npc != null)
                                    {
                                        sM(String.Format("{0}\r\n{1}\r\n\0", npc.m_name.Replace("_", " "), npc.m_description));
                                        break;
                                    }
                                    else
                                    {
                                        sM("You don't see that." + el);
                                        sM("\r\n\0");
                                        break;
                                    }
                                }
                            }

                        }
                        break;
                }
            }
        }

        override public string GetHealthString()
        {
            if (health > 0 && health <= 10)
                return "*";
            if (health > 10 && health <= 20)
                return "**";
            if (health > 20 && health <= 30)
                return "***";
            if (health > 30 && health <= 40)
                return "****";
            if (health > 40 && health <= 50)
                return "*****";
            return "dead";

        }

        private void Move(string direction)
        {
            switch (direction)
            {
                case "n":
                    direction = "north";
                    break;
                case "s":
                    direction = "south";
                    break;
                case "e":
                    direction = "east";
                    break;
                case "w":
                    direction = "west";
                    break;
            }
            switch(parent.map.moveToLocation(direction, location).Nexit)
            {
                case 0:
                    //yep you can go!
                    location.removePerson(this, string.Format("to the {0}", direction));
                    location = parent.map.moveToLocation(direction, location);  //gets the location where they would move too
                    location.addPerson(this, direction);                        //this actually adds the person to the new location
                    UpdateDatabaseLocation(location.x, location.y, location.m_dbid);
                    sM(location.m_name + el, "caps");
                    sM(string.Format("{0}\r\n", location.description));
                    sM(parent.map.getExitString(location) + el);
                    location.othersMsg(this);
                    break;
                case 1:
                case 2:
                case 6:
                    //yep you can go, but its a door so you need to move two spaces
                    location.removePerson(this,string.Format("to the {0}",direction));
                    location = parent.map.moveToLocation(direction, location);  //gets the location where they would move too
                    location = parent.map.moveToLocation(direction, location);  //but its a door so you need to move two spaces
                    location.addPerson(this, direction);                        //this actually adds the person to the new location
                    UpdateDatabaseLocation(location.x, location.y,location.m_dbid);
                    sM(location.m_name + el, "caps");
                    sM(string.Format("{0}\r\n", location.description));
                    sM(parent.map.getExitString(location) + el);
                    location.othersMsg(this);
                    break;
                case 3:
                    sM("The door is closed. ");
                    break;
                case 4:
                    sM("The door is closed. ");
                    break;
                case 5:
                    sM("The door is locked. ");
                    break;
                case 7:
                    sM("That way is blocked. ");
                    break;
            }
            sM(final);
        }
        


        private void ProcessDbCommand(string p, string[] words)
        {
            string msg = "";
            string itemsearch="";
            try
            {
                //recompile message
                foreach (string s in words)
                {
                    msg += s + "_";
                }
                msg = msg.Trim('_');
                switch (p)
                {
                    case "100":  //this is a GET command.
                        Item getThis = null;
                        if (Item.m_descriptors.Contains(words[1]))
                        {
                            //we have a health descriptor

                            //itemsearch should now contain the rest of the words in the array.
                            for (int x = 2; x < words.Length; x++)
                            {
                                itemsearch += words[x] + "_";
                            }
                            itemsearch = itemsearch.TrimEnd('_');
                            getThis = location.IsItemInHere(itemsearch, words[1]);

                        }
                        else
                        {
                            //itemsearch should now contain the rest of the words in the array.
                            for (int x = 1; x < words.Length; x++)
                            {
                                itemsearch += words[x] + "_";
                            }
                            itemsearch = itemsearch.TrimEnd('_');
                            List<Item> list = location.IsItemInHere(itemsearch);
                            if (list != null)
                            {
                                if (list.Count == 1)
                                {
                                    getThis = list[0];
                                }
                                else
                                {
                                    //now we have to group by again and find if the items are the same

                                    var groups = from i in list
                                                 group i by new { i.m_type, i.m_name, i.m_location_desc, i.HealthDesc };

                                    if (groups.Count() == 1)
                                    {
                                        //there is only 1 type of the item so drop it
                                        getThis = list.First();
                                    }
                                    else
                                    {

                                        foreach (var group in groups)
                                        {
                                            //oops there is more than one match due to health, create a menu and ask.
                                            Menu.Add(new MenuChoice(string.Format("{0}({1})",group.Key.m_name, group.Key.HealthDesc), group.Key.m_name, "get", group.Key.HealthDesc));
                                        }
                                        sM("Which one?\r\n");
                                        int j = 1;
                                        foreach (MenuChoice choice in Menu)
                                        {
                                            sM(String.Format("{0}. Get the {1}\r\n", j, choice.m_name.Replace("_", " ")));
                                            j++;
                                        }
                                        sM("\r\n\0");
                                        answer = "getting";
                                    }
                                }
                            }

                        }

                        if (getThis != null)
                        {
                            if (getThis.m_canBeHeld)
                            {
                                GetItem(getThis);
                                sM(String.Format("You picked up the {1}({0}).\r\n\0",getThis.HealthDesc, getThis.m_name.Replace("_"," ")));
                                location.SendOthersStatus(this, String.Format("{0} just took the {1}.\r\n\0", m_name, getThis.m_name.Replace("_", " ")));
                            }
                            else
                            {
                                sM(String.Format("You can't carry the {0}.\r\n\0", getThis.m_name.Replace("_"," ")));
                            }
                        }
                        else
                        {
                            if(answer != "getting")
                                sM("You don't see one here.\r\n\0");
                        }
                        break;

                    case "101":  //drop command
                        Item dropThis = null;
                        if (words[1] == null)
                            sM("Drop what?\r\n\0");
                        if(Item.m_descriptors.Contains(words[1]))
                        {
                            //we have a health descriptor
                           
                            //itemsearch should now contain the rest of the words in the array.
                            for(int x = 2; x<words.Length; x++)
                            {
                                itemsearch += words[x] + "_";
                            }
                            itemsearch = itemsearch.TrimEnd('_');
                            dropThis = HasThis(itemsearch, words[1]);

                        }
                        else
                        {
                            //itemsearch should now contain the rest of the words in the array.
                            for (int x = 1; x < words.Length; x++)
                            {
                                itemsearch += words[x] + "_";
                            }
                            itemsearch = itemsearch.TrimEnd('_');
                            List<Item> list = HasThis(itemsearch);
                            if (list != null)
                            {
                                if (list.Count == 1)
                                {
                                    dropThis = list[0];
                                }
                                else
                                {
                                    //now we have to group by again and find if the items are the same

                                    var groups = from i in list
                                                 group i by new { i.m_type, i.m_name, i.m_location_desc, i.HealthDesc};

                                    if (groups.Count() == 1)
                                    {
                                        //there is only 1 type of the item so drop it
                                        dropThis = list.First();
                                    }
                                    else
                                    {
                                        
                                        foreach (var group in groups)
                                        {
                                            //oops there is more than one match due to health, create a menu and ask.
                                            Menu.Add(new MenuChoice(string.Format("{0}({1})", group.Key.m_name,group.Key.HealthDesc), group.Key.m_name,"drop", group.Key.HealthDesc));
                                        }
                                        sM("Which one?\r\n");
                                        int j = 1;
                                        foreach (MenuChoice choice in Menu)
                                        {
                                            sM(String.Format("{0}. Drop the {1}\r\n", j, choice.m_name.Replace("_"," ")));
                                            j++;
                                        }
                                        sM("\r\n\0");
                                        answer = "dropping";
                                    }
                                }
                            }

                        }

                        if (dropThis != null)
                        {
                            DropItem(dropThis,false);
                            sM(String.Format("You dropped the {1}({0}).\r\n\0", Item.GetHealthDescription(dropThis.m_health),dropThis.m_name.Replace("_", " ")));
                            location.SendOthersStatus(this, String.Format("{0} just dropped a {1}.\r\n\0", m_name, dropThis.m_name.Replace("_", " ")));
                        }
                        else
                        {
                            if(answer != "dropping")
                                sM("You don't have one.\r\n\0");
                        }
                        break;
                    case "200":
                        //attack!
                        //tell the map who we are, who we are attacking and what room we are in.
                        parent.map.Attack(location, this, words);
                        break;
                    case "300":
                        //heal myself
                        health = 50;
                        sM("Your sandroid repaired itself.\r\n\0");
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        override internal void StartAttack(Attack myFight)
        {
            answer = "attacking";
            m_MyFight = myFight;
        }

        override internal void EndAttack()
        {
            answer = "";
            m_MyFight = null;
        }

        private void GetItem(Item getThis)
        {
            //remove it from the room
            location.RemoveItem(getThis);
            //add an item from to inventory
            inventory.Add(getThis);
            //now add it to the database
            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                if (gdb.DatabaseExists())
                {
                    try
                    {
                        item itm = (from i in gdb.items
                                    where i.id == getThis.m_dbid
                                    select i).SingleOrDefault();
                        itm.clientid = Convert.ToInt32(databaseId);
                        itm.roomid = null;
                        itm.location_desc = null;
                        itm.objectid = null;
                        gdb.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        parent.AppendToRichEditControl("\r\nError connecting to database in GetItem()" + e.Message + "\r\n");
                    }
                }
                else
                {
                    parent.AppendToRichEditControl("\r\nCouldn't connect to database in ClientPerson.GetItem()\r\n");
                }
            }
        }


        private void DropItem(Item dropThis, bool dead)
        {
            //drop an item
            location.AddItem(dropThis, "on the floor");
            if(!dead)
                inventory.Remove(dropThis);
            //update it in database
            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                if (gdb.DatabaseExists())
                {
                    item itm = (from i in gdb.items
                                where i.id == dropThis.m_dbid
                                select i).SingleOrDefault();
                    itm.clientid = null;
                    itm.roomid = location.m_dbid;
                    itm.location_desc = "on the floor";
                    itm.objectid = null;
                    gdb.SubmitChanges();
                }
                else
                {
                    parent.AppendToRichEditControl("\r\nCouldn't report dropped item to database in ClientPerson.DropItem()\r\n");
                }
            }
        }



        private void Exit()
        {
            //we are leaving the game for a bit
            //save our location
            UpdateDatabaseLocation(location.x, location.y, location.m_dbid);

            using (GameDatabaseDataContext db = new GameDatabaseDataContext())
            {
                var query6 = (from cl in db.clients
                              where cl.id == Convert.ToInt32(databaseId)
                              select cl).First();
                query6.is_playing = false;
                db.SubmitChanges();
            }

            //remove this client
            sM("Goodbye\0");
            location.removePerson(this,"");
            parent.RemoveClient(this);
        }

        private void UpdateDatabaseLocation(int x, int y, int id)
        {
            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                if (gdb.DatabaseExists())
                {
                    var q5 = (from clts in gdb.clients
                              where clts.id == Convert.ToInt32(databaseId)
                              select clts).SingleOrDefault();
                    q5.room_x = x;
                    q5.room_y = y;
                    q5.roomid = id;
                    gdb.SubmitChanges();
                }
                else
                {
                    parent.AppendToRichEditControl("\r\nCouldn't update location in ClientPerson.UpdateDatabaseLocation()\r\n");
                }
            }
        }

        internal List<Item> HasThis(string hasThisName)
        {
            //do i have this item or many of them?
            List<Item> things = new List<Item>(); 
            foreach (Item item in inventory)
            {
                if (item.m_name == hasThisName)
                {
                    things.Add(item);
                }
            }
            if (things.Count > 0)
                return things;
            return null;
        }

        internal Item HasThis(string hasThisName,string health)
        {
            //do i have this item?
            foreach (Item item in inventory)
            {
                if (item.m_name == hasThisName && Item.GetHealthDescription(item.m_health)==health)
                    return item;
            }
            return null;
        }

        private void GetInventory()
        {
            //get inventory from database
            //if there isn't an inventory, randomize it from items
            using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
            {
                var itm = from inv in gdb.items
                          where inv.clientid == Convert.ToInt32(databaseId)
                          select inv;
                foreach (item i in itm)
                {
                    inventory.Add(new Item(
                        i.id,
                        i.name,
                        i.description,
                        i.can_be_held,
                        i.container,
                        i.size,
                        (bool)i.can_be_seen_when_held,
                        (int)i.type,
                        i.location_desc,
                        (int)i.key_code,
                        (int)i.health,
                        i.generic_name,
                        (int)i.spawner_id));
                }
            }
        }

        override public void sM(string msg)
        {
            //Console.WriteLine("temp" +name +msg);

            fullMessage += msg;

            if (fullMessage.Contains("\0"))
            {
                if (fullMessage != "\0")
                {
                    if (m_loggedin)
                    {
                        //sending via web client
                        parent.SendWeb(phone, fullMessage, databaseId);

                    }
                    else
                    {
                        //sending via text
                        //fullMessage = fullMessage.Replace("\0", "");
                        //parent.SendText(m_name, phone, fullMessage);
                    }
                }
                fullMessage = "";
            }
        }

        internal void sM(string message, string format)
        {
            string newMessage = message;
            switch (format)
            {
                case "caps":
                    newMessage = message.ToUpper();
                    break;
                case "dashes":
                    newMessage = newMessage.Insert(0,"--------------------\r\n");
                    newMessage += "--------------------\r\n";
                    break;
                default:

                    break;
            }
            sM(newMessage);
        }


        internal override void SubtractHealth(int v)
        {
            health-= v;
            //check if i'm dead and do something about it.
            if(health <=0)
            {
                //im dead...  
                //was i attacking?  end the attack
                if(answer =="attacking")
                {
                    m_MyFight.ImDead(this);
                    answer = "";
                }
                sM("You have died.\r\nYou will now be removed from the game.\r\nYou have also dropped all of your items on the ground.\r\n\0");
                foreach (Item i in inventory)
                {
                    DropItem(i,true);   
                }
                inventory.Clear();
                //Exit();
                answer = "dead";
            }
        }

        internal override int ChooseAttack(GenericCharacter genericCharacter)
        {
            //choose an attack is not nesessary yet, just return 1
            return 1;
        }
    }
}
