using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using System.Data;

namespace GameHost
{
    public class Room
    {
        public List<ClientPerson> people;
        public List<NPC> m_npcs;
        public string m_name;
        public int m_dbid;
        public string description;
        public int x;
        public int y;
        public GameServer parent;
        public List<Item> items;
        public int? NKey = 0;
        private int? m_nexit;
        public bool m_fighting;
        public Attack m_fight;
        public string m_longDescription;
        public List<Spawner> m_spawners;
        Object locker;

      //0 is room //1 open door without lock
      //2 is open door with lock
      //3 is closed door without lock //4 closed door with lock  //5 closed locked door
      //6 is soft boundry  //7 wall
        //internal int?[] Exits;


        public Room(string name, string desc, string longDescription, int xco, int yco, GameServer p, int dbid,int? nexit,int? nkey, List<npc> myNPCs, List<item> myItems, List<ItemSpawner> mySpawners)
        {
            NKey = nkey;
            m_name = name;
            description = desc;
            x = xco;
            y = yco;
            people = new List<ClientPerson>();
            m_npcs = new List<NPC>();
            parent = p;
            m_dbid = dbid;
            items = new List<Item>();
            m_nexit = nexit;
            m_fighting = false;
            m_fight = null;
            m_longDescription = longDescription;
            m_spawners = new List<Spawner>();
            locker = new object();
            


            //load my NPCs
            foreach (npc annpc in myNPCs)
            {
                if (annpc.health > 0)
                    m_npcs.Add(new NPC(
                        annpc.id,
                        annpc.name, 
                        annpc.description,
                        annpc.aggressiveness,
                        annpc.movement_timer,
                        annpc.health_timer,
                        this,
                        parent,
                        annpc.type,
                        (int)annpc.health,
                        (int)annpc.movement_type,
                        (int)annpc.respawn_timer,
                        (int)annpc.start_room_id));

            }
  

            //load Inventory
            foreach (var itm in myItems)
            {
                items.Add(new Item(
                    itm.id,
                    itm.name,
                    itm.description,
                    itm.can_be_held,
                    (bool)itm.container,
                    itm.size,
                    (bool)itm.can_be_seen_when_held,
                    (int)itm.type,
                    itm.location_desc,
                    (int)itm.key_code,
                    (int)itm.health,
                    itm.generic_name,
                    (int)itm.spawner_id));
            }

            //need to add my item spawners
            foreach (var spn in mySpawners)
            {
                m_spawners.Add(new Spawner(
                    spn.id,
                    spn.item_name,
                    spn.item_description,
                    spn.item_can_be_held,
                    spn.item_container,
                    spn.limit,
                    (bool)spn.limit_world,
                    spn.item_size,
                    spn.item_can_be_seen_when_held,
                    spn.item_type,
                    spn.item_key_code,
                    spn.spawn_timer,
                    (int)spn.room_id,
                    spn.location_description,
                    spn.image,
                    spn.item_type_text,
                    (bool)spn.item_stackable,
                    (int)spn.damage,
                    spn.special_text,
                    (int)spn.special,
                    this));
                parent.AppendToRichEditControl(string.Format("\r\nRoom, {0} is monitoring a spawner called {1} every {2} minutes.\r\n", name, spn.item_name, spn.spawn_timer / 60000));
            }
            
        }


        public int? Nexit
        {
            get
            {
                return m_nexit;
            }
            set
            {
                try
                {
                    //make this write back to the database
                    using (GameDatabaseDataContext db = new GameDatabaseDataContext())
                    {
                        if (db.DatabaseExists())
                        {
                            room roomQuery = (from roomt in db.rooms
                                              where roomt.id == m_dbid
                                              select roomt).Single();
                            roomQuery.nexit = value;
                            // roomQuery.visualTypeS = roomQuery.visualTypeS.Replace(m_nexit.ToString(), value.ToString()); //  this needs to be fixed.
                            roomQuery.doorVisualType = roomQuery.doorVisualType.Replace(m_nexit.ToString(), value.ToString());
                            db.SubmitChanges();
                            m_nexit = value;
                        }
                        else
                        {
                            parent.AppendToRichEditControl("\r\nError connecting to database!\r\n");
                            m_nexit = value;
                        }
                    }
                }
                catch(Exception e)
                {
                    parent.AppendToRichEditControl("\r\nError saving nexit value in Room code. Error:\r\n" + e.Message);
                    m_nexit = value;  //set the value in memory anyway...

                }

            }
        }



        internal List<Item> IsItemInHere(string name)
        {

            //do i have this item or many of them?
            List<Item> things = new List<Item>();
            foreach (Item item in items)
            {
                if (item.m_name == name)
                {
                    things.Add(item);
                }
            }
            if (things.Count > 0)
                return things;
            return null;

        }

        internal Item IsItemInHere(string name, string modifier)
        {
            foreach (Item thing in items)
            {
                if (thing.m_name == name && Item.GetHealthDescription(thing.m_health) == modifier)
                {
                    return thing;
                }
            }
            return null;
        }

        internal NPC IsNPCInHere(string name)
        {
            foreach (NPC npc in m_npcs)
            {
                if (npc.m_name == name)
                {
                    return npc;
                }
            }
            return null;
        }

        internal bool findPerson(ClientPerson person)
        {
            if (people.Contains(person))
                return true;
            else
                return false;
        }

        internal ClientPerson findPerson(string name)
        {
            foreach (ClientPerson client in people)
            {
                if (client.m_name.ToLower() == name.ToLower())
                    return client;
            }
            return null;
        }

        internal void addPerson(ClientPerson person,string entranceString)
        {
            //add a person to the array
            people.Add(person);
            foreach (ClientPerson others in people)
            {
                if (others != person)
                {
                    others.sM(String.Format("\r\n{0} arrives from the {1}.\0", person.m_name, entranceString));
                }
            }

        }

        internal void removePerson(ClientPerson person, string exitstring)
        {
            //remove a person from the array
            people.Remove(person);
            foreach (ClientPerson others in people)
            {
                if (others != person)
                {
                    others.sM(String.Format("\r\nA {0} leaves {1}.\r\n\0", person.m_name, exitstring));                    
                }

            }
        }

        internal void addNPC(NPC npc, string entranceString)
        {
            //add a NPC to the array
            m_npcs.Add(npc);
            foreach (ClientPerson client in people.ToList().AsReadOnly())
            {
                if (!string.IsNullOrEmpty(entranceString))
                {
                    client.sM(string.Format("\r\nA {0} arrives from the {1}.\r\n\0", npc.m_name, entranceString));
                }
                else
                {
                    client.sM(string.Format("\r\nA {0} enters.\r\n\0", npc.m_name));
                }
            }

        }

        internal void removeNPC(NPC npc, string exitstring)
        {
            //remove a NPC from the array
            lock(locker)
            {
                if (m_npcs.Contains(npc))
                {
                    m_npcs.Remove(npc);
                    if (exitstring != "")
                    {
                        foreach (ClientPerson client in people.ToList().AsReadOnly())
                        {
                            client.sM(String.Format("\r\n{0} leaves {1}.\r\n\0", npc.m_name, exitstring));
                        }
                    }
                }
            }
        }

        
        internal void othersMsg(ClientPerson person)
        {
            //are there other people, npcs or things here? return a string to that effect.
            foreach (ClientPerson others in people)
            {
                if (others == person)
                {
                    switch (people.Count)
                    {
                        case 1:
                            break;
                        default:
                            person.sM("--------------------\r\n");
                            foreach (ClientPerson notMe in people)
                            {
                                if (notMe != person)
                                {
                                    person.sM(String.Format("{0} is here.\r\n", notMe.m_name));
                                }
                            }
                            person.sM("--------------------\r\n");
                            break;
                    }

                    //items
                    var groups = from i in items
                                 group i by new { i.m_type, i.m_name, i.m_location_desc, i.HealthDesc};

                    foreach (var group in groups)
                    {
                        if (group.Count() == 1)
                        {
                            person.sM(String.Format("There is a {0}({1}) {2}.\r\n",group.Key.m_name.Replace("_", " "), group.Key.HealthDesc, group.Key.m_location_desc));
                        }
                        else
                        {
                            person.sM(String.Format("There are {1} {2}s({0}) {3}.\r\n", group.Key.HealthDesc,group.Count(), group.Key.m_name.Replace("_", " "), group.Key.m_location_desc));
                        }                     
                    }
                    
                    //npcs
                    foreach (NPC npc in m_npcs)
                        person.sM(String.Format("There is a {0}({1}) here.\r\n", npc.m_name.Replace("_", " "),npc.GetHealthString()));
                    if(m_npcs.Count!=0)
                        person.sM("--------------------\r\n");
                    person.sM("\r\n");
                }
              }
          }

        ///<summary>
        ///for sending a message to others in the game (chatting)
        ///</summary>
        internal void MessageOthers(ClientPerson person, string message)
        {
            foreach (ClientPerson others in people)
            {
                if (people.Count == 1)
                {
                    person.sM("You say it, but nobody can hear you.\r\n\0");
                }
                else
                {
                    if (others != person)
                    {
                        others.sM(String.Format("{0} says, '{1}'\r\n\0", person.m_name, message));
                    }
                    else
                    {
                        person.sM(String.Format("You say, '{0}'\r\n\0", message));
                    }
                }

            }
        }

        ///<summary>
        ///for sending a message to everyone in the room from someone
        ///</summary>
        internal void MessageEveryone(string from, string message)
        {

            
            foreach (ClientPerson person in people)
            {
                person.sM(String.Format("\r\n{0} says, '{1}'\r\n\0", from, message));
            }
        }
        ///<summary>
        ///for sending a message to everyone in the room 
        ///</summary>
        internal void MessageEveryone(string message)
        {
            foreach (ClientPerson person in people)
            {
                person.sM(String.Format("\r\n{0}\r\n\0", message));
            }
        }


        internal void RemoveItem(Item getThis)
        {
            //remove an item from the room and from the room database
            items.Remove(getThis);
        }

        internal bool AddItem(Item addThis, string locationDescription)
        {
            //add an item from to room
                addThis.m_location_desc = locationDescription;
                items.Add(addThis);
                return true;
        }

        ///<summary>
        /// sends a status to everyone but the client sent to it
        ///</summary>
        internal void SendOthersStatus(ClientPerson clientPerson, string p) // to send a status message to everyone else but you
        {
            foreach (ClientPerson others in people)
            {
                if (others != clientPerson)
                {
                    others.sM(p);
                }
            }
        }
    }
}