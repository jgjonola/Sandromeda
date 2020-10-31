using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameHost
{
    class MenuChoice
    {
        internal string m_action;
        internal string m_name;
        internal GenericCharacter m_target;
        internal string m_item;
        internal string m_modifier;
        public MenuChoice(string name, string action)
        {
            m_name = name;
            m_action = action;
        }

        public MenuChoice(string name, string action,GenericCharacter target)
        {
            m_name = name;
            m_action = action;
            m_target = target;

        }

        public MenuChoice(string name, string item, string action, string modifier )
        {
            m_name = name;
            m_item = item;
            m_action = action;
            m_modifier = modifier;
        }


    }
}
