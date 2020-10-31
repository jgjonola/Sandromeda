using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameHost
{
    public class Item
    {
        public int m_dbid;
        public string m_name;
        public string m_description;
        public bool m_canBeHeld;
        public bool m_container;
        public int m_size;
        public bool m_canBeSeenWhenHeld;
        public int m_type;
        public string m_location_desc;
        public int m_code;
        public int m_itemid;
        public int m_health;
        public string m_genericName;
        public static List<string> m_descriptors = new List<string>{ "dead", "feeble", "poor", "typical", "good", "perfect" };
        public int m_spawnerID;

        //public string m_healthDesc;

        public Item(int dbid,string name,string description,bool canBeHeld, bool container, int size, bool canBeSeenWhenHeld, int type, string location_desc, int code, int health,string genericName, int spawnerID)
        {
            m_dbid=dbid;
            m_name=name;
            m_description=description;
            m_canBeHeld=canBeHeld;
            m_container=container;
            m_size=size;
            m_canBeSeenWhenHeld=canBeSeenWhenHeld;
            m_type=type;
            m_location_desc = location_desc;
            m_code = code;
            m_health = health;
            m_genericName = genericName;
            m_spawnerID = spawnerID;
        }

        

        public string HealthDesc
        {
            get
            {
                return GetHealthDescription(m_health);
            }
            set { }
        }


        public static string GetHealthDescription(int number)
        {
            if (number <= 0)
                return "dead";
            if (number > 0 && number < 10)
                return "*";
            if (number < 20 && number >= 10)
                return "**";
            if (number < 30 && number >= 20)
                return "***";
            if (number < 40 && number >= 30)
                return "****";
            if (number <= 50 && number >= 40)
                return "*****";
            return "dead";
        }

        public static int GetHealthFromDescription(string desc)
        {
            if (desc == "dead")
                return 0;
            if (desc == "*")
                return 10;
            if (desc == "**")
                return 20;
            if (desc == "***")
                return 30;
            if (desc == "****")
                return 40;
            if (desc == "*****")
                return 50;
            return 0;

        }
    }
}
