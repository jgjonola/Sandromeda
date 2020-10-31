using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DefaultNamespace;

namespace GameHost
{
    public class Spawner:IDisposable
    {
        GameServer m_parent;
        Timer m_spawnTimer;
        int m_id;
        string m_name;
        string m_description;
        bool m_canBeHeld;
        bool m_container;
        int m_limit;
        bool m_limitWorld;
        int m_itemSize;
        bool m_canBeSeenWhenHeld;
        int m_itemType;
        int m_keyCode;
        int m_spawnTimerDuration;
        int m_roomID;
        string m_locationDescription;
        string m_image;
        string m_typeText;
        bool m_stackable;
        int m_damage;
        string m_specialText;
        int m_special;
        Room m_myRoom;



        public Spawner(int id, string name, string description, bool canBeHeld, bool container, int limit, bool limitWorld, int itemSize, bool canBeSeenWhenHeld, int itemType, int keyCode, int spawnTimerDuration, int roomID, string locationDescription, string image, string typeText, bool stackable, int damage, string specialText, int special, Room myRoom)
        {
            //after creation of this object, set the timer to the specified interval, then check my room for the amount of items if we need to create objects on every tick
            m_id=id;
            m_name=name;
            m_description=description;
            m_canBeHeld=canBeHeld;
            m_container=container;
            m_limit=limit;
            m_limitWorld=limitWorld;
            m_itemSize=itemSize;
            m_canBeSeenWhenHeld=canBeSeenWhenHeld;
            m_itemType=itemType;
            m_keyCode=keyCode;
            m_spawnTimerDuration=spawnTimerDuration;
            m_roomID=roomID;
            m_locationDescription=locationDescription;
            m_image=image;
            m_typeText=typeText;
            m_stackable=stackable;
            m_damage=damage;
            m_specialText=specialText;
            m_special=special;
            m_myRoom = myRoom;
            m_parent = myRoom.parent;
            //set a check timer
            m_spawnTimer = new Timer(new TimerCallback(Check), null, spawnTimerDuration, spawnTimerDuration);
        }

        public void Check(Object stateInfo)
        {
            //timer ticked
            //check the item list of the room to see how many items that i created are left in the room
            int totalItems = 0;
            if (m_limitWorld)
            {
                //check all rooms for my items and see if its at the limit
                int worldTotal = 0;

                for (int i = 0; i < m_myRoom.parent.map.map.GetLength(0); i++)
                {
                    for (int j = 0; j < m_myRoom.parent.map.map.GetLength(1); j++)
                    {
                        Room checkRoom = m_myRoom.parent.map.map[i, j];
                        worldTotal += (from its in checkRoom.items
                                       where its.m_spawnerID == m_id
                                       select its).Count();
                    }
                }
                totalItems = worldTotal;
            }
            else
            {
                        int myItems = (from it in m_myRoom.items
                                      where it.m_spawnerID == m_id
                                      select it).Count();
                totalItems = myItems; 
            }

            if ((m_limit - totalItems) > 0)
            {
                //we are short an item or more, so make some more items
                //first add it to the database so that we have the dbid to pass to the new item object
                using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
                {
                    item newitem = new item
                    {
                        name = m_name,
                        description = m_description,
                        can_be_held = m_canBeHeld,
                        container = m_container,
                        room_limit = m_limit,
                        world_limit = m_limitWorld,
                        size = m_itemSize,
                        can_be_seen_when_held = m_canBeSeenWhenHeld,
                        type = m_itemType,
                        key_code = m_keyCode,
                        roomid = m_myRoom.m_dbid,
                        location_desc = m_locationDescription,
                        image = m_image,
                        type_text = m_typeText,
                        item_stackable = m_stackable,
                        damage = m_damage,
                        special_text = m_specialText,
                        special = m_special,
                        health = 50,
                        spawner_id = m_id,
                        generic_name = m_typeText
                    };
                    gdb.items.InsertOnSubmit(newitem);
                    gdb.SubmitChanges();


                    m_myRoom.items.Add(new Item(
                        newitem.id,
                        m_name,
                        m_description,
                        m_canBeHeld,
                        m_container,
                        m_itemSize,
                        m_canBeSeenWhenHeld,
                        m_itemType,
                        m_locationDescription,
                        m_keyCode,
                        50,
                        m_typeText,
                        m_id
                        ));
                    m_parent.AppendToRichEditControl(string.Format("\r\nItem {0} spawned in room {1}.", m_name, m_myRoom.m_dbid));
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
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
