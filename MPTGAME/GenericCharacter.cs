using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHost
{
    abstract public class GenericCharacter
    {
        public string m_name;
        public string m_description;
        public int m_charType;
        protected int? m_health;

        //type 1 is client
        //type 2 is NPC
        abstract internal MenuChoice GetSpecialAttack();
        abstract public void sM(string msg);

        


        abstract public string GetHealthString();
        abstract internal void EndAttack();
        internal abstract int ChooseAttack(GenericCharacter genericCharacter);
        internal abstract void SubtractHealth(int v);
        internal abstract void StartAttack(Attack attack);
    }
}
