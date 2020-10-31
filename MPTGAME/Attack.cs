using System;
using System.Collections.Generic;
using System.Threading;


namespace GameHost
{
    public class Attack : IDisposable
    {
        GenericCharacter m_initiator;
        private List<GenericCharacter> m_Fighters = new List<GenericCharacter>();
        GenericCharacter m_attackee;
        GenericCharacter m_myTurn;
        private List<MenuChoice> m_Menu;
        Random rnd = new Random((int)DateTime.Now.Ticks);
        Room m_location;
        Timer m_AnswerTimer;
        bool m_Ended = false;
        int m_answerTimerDuration = 3000000;
        //string answer = "";

        public Attack(GenericCharacter initiator,Room rm, string[] words)
        {
            m_location = rm;
            m_AnswerTimer = new Timer(new TimerCallback(AnswerTimeExpired));

            //find out who we are attacking and what it is
            m_initiator = initiator;
            m_Fighters.Add(m_initiator);
            if (words.Length == 2)
            {
                if (rm.findPerson(words[1]) != null)
                {
                    m_attackee = rm.findPerson(words[1]);

                }
                else
                {
                    if (rm.IsNPCInHere(words[1]) != null)
                    {
                        m_attackee = rm.IsNPCInHere(words[1]);
                    }
                }
            }
            else
            {
                string attackeeString = "";
                for (int i = 1; i < words.Length; i++)
                {
                    attackeeString += words[i] + "_";
                }
                attackeeString = attackeeString.TrimEnd('_');
                if (rm.findPerson(attackeeString) != null)
                {
                    m_attackee = rm.findPerson(attackeeString);

                }
                else
                {
                    if (rm.IsNPCInHere(attackeeString) != null)
                    {
                        m_attackee = rm.IsNPCInHere(attackeeString);
                    }
                }
            }


            if (m_attackee != null)
            {
                m_Fighters.Add(m_attackee);
                //get everyone else in the room and put them in attack mode and let them know
                foreach (GenericCharacter fighter in m_Fighters)
                {
                    fighter.StartAttack(this);
                    if (fighter == m_initiator)
                    {
                        fighter.sM("You enter combat!\r\n\0");
                        m_myTurn = fighter;
                        //construct a menu object and find out what you want to do...
                        m_Menu = new List<MenuChoice>();
                        foreach (GenericCharacter possibleAttackee in m_Fighters)
                        {
                            if (possibleAttackee != fighter)
                                m_Menu.Add(new MenuChoice(String.Format("Attack {0} ({1})", possibleAttackee.m_name.Replace('_',' '),possibleAttackee.GetHealthString()), "attack", possibleAttackee));
                        }
                        m_Menu.Add(new MenuChoice("Dodge", "dodge"));
                        m_Menu.Add(new MenuChoice("Flee", "flee"));
                        MenuChoice special;
                        if ((special = fighter.GetSpecialAttack()) != null)
                            m_Menu.Add(special);
                        for (int j = 1; j < m_Menu.Count + 1; j++)
                        {
                            fighter.sM(String.Format("{0}. {1}\r\n", j, m_Menu[j - 1].m_name));
                        }
                        fighter.sM("Your Health: " + fighter.GetHealthString());
                        fighter.sM("\0");
                    }
                    else
                    {
                        fighter.sM(string.Format("{0} starts a fight with you!\r\n\0", m_initiator.m_name));
                    }
                }
            }
            else
            {
                m_initiator.sM("You can't attack that.\r\n\0");
                EndAttack();
            }
        }

        private void AnswerTimeExpired(Object stateInfo)
        {
            MessageEveryoneButMe(m_myTurn,String.Format("{0} took too long.\r\n\0",m_myTurn.m_name));
            PickNextUp(m_myTurn);
            MessageEveryone(String.Format("It's {0}'s turn.\r\n\0", m_myTurn.m_name));
            if(m_AnswerTimer!=null)
                m_AnswerTimer.Dispose();
            m_AnswerTimer = null;
        }

        private void MessageEveryone(string v)
        {
            foreach (GenericCharacter c in m_Fighters)
            {
                c.sM(v + "\r\n\0");
            }
        }

        internal void ProcessAttackMsg(ClientPerson sender, string msg, string[] words)
        {
            //process an attack message
            if (sender == m_myTurn)
            {
                int choice = 9999;
                //its my turn so do something
                try
                {
                   choice = Convert.ToInt32(words[0]);
                }
                catch (Exception)
                {
                    
                }
                if(m_AnswerTimer!=null)
                    m_AnswerTimer.Dispose();
                m_AnswerTimer = null;
                if (choice >= 0 && choice != 9999)
                {
                    switch (m_Menu[choice - 1].m_action)
                    {
                        case "attack":
                            DoAttack(1, sender, m_Menu[choice - 1].m_target);
                            break;
                        case "dodge":
                            sender.sM(string.Format("You {0}.\r\n\0", m_Menu[choice - 1].m_name));
                            break;
                        case "flee":
                            sender.sM(string.Format("You {0}.\r\n", m_Menu[choice - 1].m_name));
                            sender.EndAttack();
                            sender.sM("What do you want to do now?\r\n\0");
                            m_Fighters.Remove(sender);
                            if (m_Fighters.Count < 2)
                            {
                                EndAttack();
                                //the fight has ended.                     
                            }
                            break;
                        case "groinkick":
                            sender.sM(string.Format("You {0}.\r\n\0", m_Menu[choice - 1].m_name));
                            break;
                        default:
                            sender.sM("Not a valid choice.\r\n\0");
                            if (m_AnswerTimer != null)
                                m_AnswerTimer.Change(m_answerTimerDuration, m_answerTimerDuration);
                            else
                                m_AnswerTimer = new Timer(AnswerTimeExpired,null,m_answerTimerDuration, m_answerTimerDuration);
                            break;
                    }
                    if (!m_Ended)
                        PickNextUp(sender);
                }
                else

                {
                    sender.sM("Not a valid choice.\r\n\0");
                    if (m_AnswerTimer != null)
                        m_AnswerTimer.Change(m_answerTimerDuration, m_answerTimerDuration);
                    else
                        m_AnswerTimer = new Timer(AnswerTimeExpired, null, m_answerTimerDuration, m_answerTimerDuration);
                }
                
            }
            else
            {
                //its not my turn!
                sender.sM("Sorry, it's not your turn.\r\n\0");
            }
        }

        private void EndAttack()
        {
            m_Fighters[0].EndAttack();
            if(m_AnswerTimer!=null)
                m_AnswerTimer.Dispose();
            m_AnswerTimer = null;
            m_Ended = true;
            m_location.parent.map.EndAttack(this);
        }

        private void PickNextUp(GenericCharacter currentPlayer)
        {
            //pick the next player randomly in the room
            if(m_Fighters.Count>=2)
            { 
            int n = rnd.Next(0, m_Fighters.Count);
            while (currentPlayer == m_Fighters[n])
            {
                n = rnd.Next(0, m_Fighters.Count);
            }
            m_myTurn = m_Fighters[n];

                //if this is a players turn
                //send the new player a menu

                if (m_myTurn.m_charType == 1)
                {
                    //its a player
                    m_Menu = new List<MenuChoice>();
                    foreach (GenericCharacter possibleAttackee in m_Fighters)
                    {
                        if (possibleAttackee != m_myTurn)
                            m_Menu.Add(new MenuChoice(String.Format("Attack {0} ({1})", possibleAttackee.m_name.Replace('_', ' '), possibleAttackee.GetHealthString()), "attack", possibleAttackee));
                    }
                    m_Menu.Add(new MenuChoice("Dodge", "dodge"));
                    m_Menu.Add(new MenuChoice("Flee", "flee"));
                    MenuChoice special;
                    if ((special = m_myTurn.GetSpecialAttack()) != null)
                        m_Menu.Add(special);
                    for (int j = 1; j < m_Menu.Count + 1; j++)
                    {
                        m_myTurn.sM(String.Format("{0}. {1}\r\n", j, m_Menu[j - 1].m_name));
                    }
                    m_myTurn.sM("Your Health: " + m_myTurn.GetHealthString());
                    m_myTurn.sM("\0");
                    //set a timer to kill skip the player if he waits too long
                    if (m_AnswerTimer != null)
                        m_AnswerTimer.Change(m_answerTimerDuration, m_answerTimerDuration);
                    else
                        m_AnswerTimer = new Timer(AnswerTimeExpired, null, m_answerTimerDuration, m_answerTimerDuration);
                }
                else
                {
                    //its an NPC

                    //MessageFighters(String.Format("{0} attacks!\r\n\0", m_myTurn.m_name));
                    //chose a person to attack at random...
                    int p = rnd.Next(0, m_Fighters.Count);
                    while (m_myTurn == m_Fighters[p] || m_Fighters[p].m_charType == 2)
                    {
                        p = rnd.Next(0, m_Fighters.Count);
                    }
                    DoAttack(m_myTurn.ChooseAttack(m_Fighters[p]), m_myTurn, m_Fighters[p]);
                    PickNextUp(m_myTurn);
                }
            }
            else
            {
                //the fight is over
                MessageEveryone("The fight has ended.");
            }

        }

        private void DoAttack(int v, GenericCharacter m_myTurn, GenericCharacter attackee)
        {
            //do some attack here
            m_myTurn.sM(String.Format("You attack {0}.\r\n\r\n\0", attackee.m_name));
            

            //let them know
            attackee.sM(String.Format("You were attacked by {0}.\r\n\r\n\0",m_myTurn.m_name));
            int damage;
            if(m_myTurn.m_charType==2)
            {
                damage = 1;
            }
            else
            {
                damage = 10;
            }
                attackee.SubtractHealth(damage);
            //MessageOtherFighters(String.Format("{0} attacked {1}.\r\n\0",m_myTurn.m_name,attackee.m_name));
        }

        private void MessageOtherFighters(string v)
        {
            foreach(GenericCharacter c in m_Fighters)
            {
                if (c != m_myTurn || c != m_attackee)
                {
                    c.sM(v + "\r\n\0");
                }
            }
        }

        internal void ImDead(GenericCharacter clientPerson)
        {
            m_Fighters.Remove(clientPerson);
            MessageEveryoneButMe(clientPerson, string.Format("{0} died.", clientPerson.m_name));
            if (m_Fighters.Count <= 1)
                EndAttack();
        }

        private void MessageEveryoneButMe(GenericCharacter clientPerson, string v)
        {
            foreach (GenericCharacter c in m_Fighters)
            {
                if (c != clientPerson)
                {
                    c.sM(v + "\r\n\0");
                }
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                if(m_AnswerTimer!=null)
                    m_AnswerTimer.Dispose();
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
