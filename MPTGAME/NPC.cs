using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DefaultNamespace;
using System.Data;

namespace GameHost
{
    public class NPC:GenericCharacter, IDisposable
    {
        internal int m_aggressiveness;
        internal int m_movementTimer;
        internal int m_healthTimer;
        internal int m_movement_type;

        private GameServer m_parent;
        Room m_location;
        public string m_databaseId;
        public List<Item> inventory;
        //internal List<MenuChoice> Menu;
        internal int m_type;
        Timer mTimer;
        Random r;
        Attack m_MyFight;
        public bool inFight;
        int m_spawnTimerTime;
        int m_startRoom;
        Timer m_spawnTimer;



        public NPC(int databaseId, string name, string description, int aggressiveness, int movementTimer, int healthTimer, Room location, GameServer parent, int type, int health,int movement_type, int spawnTimer, int startRoom)
        {
            m_databaseId = databaseId.ToString();
            m_name = name;
            m_description = description;
            m_aggressiveness = aggressiveness;
            m_movementTimer = movementTimer;
            m_healthTimer = healthTimer;
            m_location = location;
            m_parent = parent;
            m_type = type;
            m_health = health;
            m_movement_type = movement_type;
            inFight = false;
            m_charType = 2;
            m_spawnTimerTime = spawnTimer;
            m_startRoom = startRoom;

            //TODO: get my inventory from the database...   
            
            //this is a line to randomize numbers for this guy   
            r = new Random((int)DateTime.Now.Ticks+Convert.ToInt32(m_databaseId));

            if (m_movement_type>0)
            {
                //this NPC moves, so make it move with a movement timer
                mTimer = new Timer(new TimerCallback(DoSomething), null, m_movementTimer, m_movementTimer);
                
            }
            if(m_location==null)
            {
                //we are starting dead so set a respawn timer
                m_spawnTimer = new Timer(new TimerCallback(Respawn), null, m_spawnTimerTime, m_spawnTimerTime);

            }


            //add a saysomething timer for random phrases.

        }

        override public void sM(string msg)
        {
            //not used
        }
        internal override void EndAttack()
        {
            m_MyFight = null;
            inFight = false;
            //if there is a movement timer, restart it so i can move
            if(mTimer!=null)
                mTimer.Change(m_movementTimer, m_movementTimer);
        }
        override internal void StartAttack(Attack myFight)
        {
            //stop the movment timers if there is one
            if(mTimer!=null)
                mTimer.Change(Timeout.Infinite,Timeout.Infinite);
            m_MyFight = myFight;
            inFight = true;
        }

        public void DoSomething(Object stateInfo)
        {
            //do something
            //m_location.MessageEveryone(m_name, "bawk");
            MoveRandom();
            //m_location.MessageEveryone(m_name, "bawk");
        }

        public void MoveRandom()
        {
            //calculate a direction
            //Random r = new Random((int)DateTime.Now.Ticks);
            Move(r.Next(0, 5));
            
        }
        private void Move(int direction)
        {
            switch (direction)
            {
                case 1:
                    Move("north");
                    break;
                case 2:
                    Move("east");
                    break;
                case 3:
                    Move("south");
                    break;
                case 4:
                    Move("west");
                    break;
            }
        }

        override internal MenuChoice GetSpecialAttack()
        {
            return null;
        }

        private void Move(string direction)
        {
            int? exit=7;
            if (m_location != null)
            {
                exit = m_parent.map.moveToLocation(direction, m_location).Nexit;
                if (exit < 3)
                {
                    //we can go that way!
                    //do it
                    m_location.removeNPC(this, String.Format("to the {0}", direction));
                    m_location = m_parent.map.moveToLocation(direction, m_location);
                    if(exit>0)
                        m_location = m_parent.map.moveToLocation(direction, m_location);
                    m_location.addNPC(this, direction);
                    UpdateDatabase();
                }
                else
                {
                    bool possibleExit = false;
                    for (int i = 1; i < 5; i++)
                    {
                        //check all directions to see if im stuck and can't move
                        if (m_parent.map.moveToLocation(i.ToString(), m_location).Nexit < 3)
                        {
                            possibleExit = true;
                        }
                    }
                    if (possibleExit)  //only move if i have possible exit
                        MoveRandom();
                }
            }
        }


        override public string GetHealthString()
        {
            if (m_health > 0 && m_health <= 10)
                return "*";
            if (m_health > 10 && m_health <= 20)
                return "**";
            if (m_health > 20 && m_health <= 30)
                return "***";
            if (m_health > 30 && m_health <= 40)
                return "****";
            if (m_health > 40 && m_health <= 50)
                return "*****";
            return "dead";

        }

        private void UpdateDatabase()
        {
            try
            {
                using (GameDatabaseDataContext dbc = new GameDatabaseDataContext())
                {
                    if (dbc.DatabaseExists())
                    {
                        npc theone = (from npcdata in dbc.npcs
                                      where npcdata.id == Convert.ToInt32(m_databaseId)
                                      select npcdata).SingleOrDefault();

                        theone.health = m_health;
                        if (m_location != null)  //if we are dead the location is null!
                            theone.roomid = m_location.m_dbid;
                        else
                            theone.roomid = 0;
                        dbc.SubmitChanges();
                    }
                    else
                    {
                        //no database connection
                        m_parent.AppendToRichEditControl("\r\nCouldn't update the database in the updatedatabase() in NPC\r\n");
                    }
                }
            }catch(Exception e)
            {
                m_parent.AppendToRichEditControl(string.Format("\r\nThere was an error connecting to the database in UpdateDatabase() for NPC.  Error:{0}\r\n", e.Message));
            }
        }

        internal override int ChooseAttack(GenericCharacter genericCharacter)
        {
            //this should return an attack methode. 
            return 1;
        }

        internal override void SubtractHealth(int v)
        {
            m_health -= v;
            if(m_health <=0)
            {
                //its dead, Jim!
                m_MyFight.ImDead(this);
                m_location.removeNPC(this, "");
                m_parent.map.m_heaven.Add(this);
                m_location = null;
                if(mTimer!= null)
                    mTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_spawnTimer = new Timer(new TimerCallback(Respawn), null, m_spawnTimerTime, m_spawnTimerTime);
                m_parent.AppendToRichEditControl(string.Format("\r\n{0} has been sent to heaven.  Minutes to respawn: {1}",m_name,m_spawnTimerTime/60000));
            }
            UpdateDatabase();
            //check if im dead now and do something.
            
        }

        private void Respawn(object state)
        {
            m_spawnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //you've just come back to life by the will of the timer, what do you want to do now?
            //set my health to full
            m_health = 50;
            //put me in my starting room
            m_parent.map.m_heaven.Remove(this);
            m_location= m_parent.map.dropInLocation(m_startRoom, this);
            //start my movement timer again
            if(mTimer!=null)
                mTimer.Change(m_movementTimer, m_movementTimer);
            //tell the database
            UpdateDatabase();
            m_parent.AppendToRichEditControl(string.Format("\r\n{0} is respawning in room {1}.\r\n", m_name, m_startRoom));


        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                mTimer.Dispose();
                m_spawnTimer.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
