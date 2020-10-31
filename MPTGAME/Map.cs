using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using System.Data;
using System.Threading.Tasks; 

namespace GameHost
{
    public class Map
    {
        public Room[,] map;
        public GameServer parent;
        internal List<Attack> m_fights;
        public List<NPC> m_heaven;  //used to hold dead NPCs until they respawn

        public Map(GameServer p)
        {
            m_fights = new List<GameHost.Attack>();
            parent = p;
            m_heaven = new List<NPC>();
            //first we have to load in the rooms, so we have to find the highest x and y
            //in the database and that will be the array size -1
            LoadRoomsFromDatabase();
        }

        private void LoadRoomsFromDatabase()
        {
            var t = Task.Run(() =>
            {
                //loads all the rooms in the map array from the database
                int x;
                int y;
                try
                {
                    using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
                    {
                        var query = (from rm in gdb.rooms
                                     orderby rm.x descending
                                     select new
                                     {
                                         rm.x
                                     }).First();
                        x = query.x;
                        var query2 = (from rm in gdb.rooms
                                      orderby rm.y descending
                                      select new
                                      {
                                          rm.y
                                      }).First();
                        y = query2.y;

                        map = new Room[x + 1, y + 1];  //creates the array


                        //load all items and npcs into a list then make a separate list and send it to the rooms
                        List<item> allItems = (from someinv in gdb.items
                                               select someinv).ToList();
                        List<npc> allNPC = (from someNPC in gdb.npcs
                                            select someNPC).ToList();
                        List<ItemSpawner> allSpawners = (from someSpawners in gdb.ItemSpawners
                                                         select someSpawners).ToList();
                        parent.AppendToRichEditControl(string.Format("\r\nWorld contains {0} total items, {1} of which are held by clients.\r\n",allItems.Count(),allItems.Where(a=>a.clientid!=null).Count()));
                        parent.AppendToRichEditControl(string.Format("\r\nWorld contains {0} Non-Player Characters.\r\n", allNPC.Count()));
                        parent.AppendToRichEditControl(string.Format("\r\nWorld contains {0} spawners.\r\n", allSpawners.Count()));
                        //now loop through the x and y loading all the rooms to the array size and add them to the array.
                        var q3 = (from rm in gdb.rooms
                                  select rm).ToList();
                        parent.AppendToRichEditControl(string.Format("\r\nMap contains {0} rooms, {1} of which are active.\r\n", q3.Count(), q3.Where(b=>b.visualTypeS !="none").Count()));
                        parent.loadingForm.SetStatusLength(q3.Count());
                        foreach (room r in q3)
                        {
                            List<item> myItems = (from mi in allItems
                                                  where mi.roomid == r.id
                                                  select mi).ToList();
                            List<npc> myNPC = (from NP in allNPC
                                               where NP.roomid == r.id
                                               select NP).ToList();
                            List<ItemSpawner> myIS = (from IS in allSpawners
                                                      where IS.room_id == r.id
                                                      select IS).ToList();
                            map[r.x, r.y] = new Room(r.name, r.description, r.longDescription, r.x, r.y, parent, r.id, r.nexit, r.nkey, myNPC, myItems, myIS);
                            parent.loadingForm.Tick();
                        }

                        //find all the NPCs without a room, load them into heaven and 
                        //set respawn timers for them
                        var heaven = (from np in allNPC
                                      where np.roomid == 0
                                      select np).ToList();

                        foreach (npc n in heaven)
                        {
                            //create them as a real in game npc and add them to heaven
                            m_heaven.Add(new NPC(n.id, n.name, n.description, n.aggressiveness, n.movement_timer, n.health_timer, null, parent, n.type, (int)n.health, (int)n.movement_type, (int)n.respawn_timer, (int)n.start_room_id));
                            parent.AppendToRichEditControl(string.Format("\r\n{0} is in heaven. Minutes to respawn:{1}\r\n", n.name, n.respawn_timer / 60000));
                        }

                        //we need to load all of the item spawners into a list and set timers for them
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
            });
            t.ContinueWith(EndLoading);
        }

        private void EndLoading(Task obj)
        {
            parent.loadingForm.Invoke(new Action(() => parent.loadingForm.Hide()));
            parent.loadingForm.Invoke(new Action(() => parent.loadingForm.Dispose()));
        }


        internal void Attack(Room arena, GenericCharacter initiator, string[] words)
        {
            //create an attack object and add it to a list control of all the fights
            Attack fight = new GameHost.Attack(initiator, arena, words);
            arena.m_fighting = true;
            arena.m_fight = fight;
            m_fights.Add(fight);
        }

        internal void EndAttack(Attack fight)
        {
            m_fights.Remove(fight);
        }


        public Room newPlayer(ClientPerson person)
        {
            //add a new player to the main room
            map[8, 13].addPerson(person,"ether");
            return map[8, 13];
        }
        internal string getExitString(Room room)
        {
            //return a string that lists the exits from that room
            string text = "[";

            if (room.y > map.GetLowerBound(1) && moveToLocation("north", room).Nexit < 7)
            {

                //is it closed?
                if (moveToLocation("north", room).Nexit > 2 && moveToLocation("north", room).Nexit < 6)
                {
                    text += "(N) | ";
                }
                else
                {
                    text += "N | ";
                }

            }
            if (room.x < map.GetUpperBound(1) && moveToLocation("east", room).Nexit < 7)
            {
                if (moveToLocation("east", room).Nexit > 2 && moveToLocation("east", room).Nexit < 6)
                {
                    text += "(E) | ";
                }
                else
                {
                    text += "E | ";
                }

            }

            if (room.y < map.GetUpperBound(0) && moveToLocation("south", room).Nexit < 7)
            {
                
                if (moveToLocation("south", room).Nexit > 2 && moveToLocation("south", room).Nexit < 6)
                {
                    text += "(S) | ";
                }
                else
                {
                    text += "S | ";
                }
            }

            if (room.x > map.GetLowerBound(1) && moveToLocation("west", room).Nexit < 7)
            {
                    if (moveToLocation("west", room).Nexit > 2 && moveToLocation("west", room).Nexit < 6)
                    { 
                        text += "(W) | ";
                    }
                    else
                    {
                        text += "W | ";
                    }
            }
            text = text.Trim();
            text = text.TrimEnd('|');
            text = text.Trim();
            text += "]";
            

            return text;
        }

        internal Room dropInLocation(int x, int y, ClientPerson client)
        {
            map[x, y].addPerson(client,"ether");
            return map[x,y];
        }
        internal Room dropInLocation(int roomDbId, NPC nonPlayerCharacter)
        {
            //find the room with that dbid
            Room rm = FindRoomByID(roomDbId);
            if(rm !=null)
            {
                rm.addNPC(nonPlayerCharacter,"");
            }
            return rm;
        }

        private Room FindRoomByID(int roomDbId)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if(map[i, j].m_dbid==roomDbId)
                    {
                        return map[i, j];
                    }
                }
            }
            return null;
        }

        internal Room moveToLocation(string p, Room room)
        {
            if (room != null)
            {
                if (p == "west" || p == "4")
                {
                    return map[room.x - 1, room.y];
                }

                if (p == "east" || p == "2")
                {
                    return map[room.x + 1, room.y];
                }


                if (p == "south" || p == "3")
                {
                    return map[room.x, room.y + 1];
                }


                if (p == "north" || p == "1")
                {
                    return map[room.x, room.y - 1];
                }
                return null;
            }
            else
            {
                return null;
            }
            
        }
    }
}

