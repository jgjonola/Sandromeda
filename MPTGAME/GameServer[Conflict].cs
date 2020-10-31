
using System;
using System.Net;
using GameHost;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DefaultNamespace
{
    /// <summary>
    /// Description of SocketServer.	
    /// </summary>
    public class GameServer : System.Windows.Forms.Form
    {
        private System.Windows.Forms.RichTextBox richTextBoxReceivedMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBoxSendMsg;
        private System.Windows.Forms.Button buttonSendMsg;
        private System.Windows.Forms.Button buttonClose;
        public List<ClientPerson> clientPeople;
        public Map map;
        public delegate void UpdateRichEditCallback(string text);
        public delegate void UpdateClientListCallback();

        //public AsyncCallback pfnWorkerCallBack;
        private System.Windows.Forms.ListBox listBoxClientList;
        private System.Windows.Forms.Label label6;
        //private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnClear;
        Object locker;
        public LoadingForm loadingForm;
        mCore.SMS m_objSMS;
        mCore.Inbox objInbox;
        public List<action> m_actions;

        public GameServer()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            loadingForm = new LoadingForm();
            locker = new object();
            clientPeople = new List<ClientPerson>();
            loadingForm.Show(this);
            map = new Map(this); //loads the map from the database
            LoadCustomActions();//we need to load all the custom actions into the program from the database
            SetupModem();  //gets the GSM MODEM READY TO SEND texts
            CheckText();   //Starts checking web texts

        }

        private void LoadCustomActions()
        {
            GameDatabaseDataContext gdb = new GameDatabaseDataContext();
            m_actions = (from acts in gdb.actions
                         select acts).ToList();
        }

        private void SetupModem()
        {
            m_objSMS = new mCore.SMS();
            m_objSMS.Port = "COM6";
            m_objSMS.BaudRate = mCore.BaudRate.BaudRate_115200;
            m_objSMS.DataBits = mCore.DataBits.Eight;
            m_objSMS.Parity = mCore.Parity.None;
            m_objSMS.StopBits = mCore.StopBits.One;
            m_objSMS.FlowControl = mCore.FlowControl.None;
            m_objSMS.DisableCheckPIN = true;
            m_objSMS.LongMessage = mCore.LongMessage.Concatenate;
            m_objSMS.NewMessageReceived += new mCore.SMS.NewMessageReceivedEventHandler(objSMS_NewMessageReceived);
            m_objSMS.NewMessageIndication = true;
            m_objSMS.NewMessageConcatenate = true;
            m_objSMS.Connect();

            //get inbox
            objInbox = m_objSMS.Inbox();

        }

        private void objSMS_NewMessageReceived(object sender, mCore.NewMessageReceivedEventArgs e)
        {
            //New Message Indication Event
            //Display the new message

            objInbox.Refresh();
            foreach (mCore.Message msg in objInbox)
            {
                //for each message recieved, check if it is a new user and process it.
                bool notFound = true;
                foreach (ClientPerson client in clientPeople)
                {
                    if (client.phone == msg.Phone)
                    {
                        AppendToRichEditControl("\r\nFrom: " + client.m_name + "\r\n" + msg.Text);
                        client.processMessage(msg.Text);
                        notFound = false;
                        break;
                    }
                }
                if (notFound)
                {
                    clientPeople.Add(new ClientPerson(this, msg.Phone,""));
                }
                //MessageBox.Show("FROM: " + Msg.Phone + "\r\n\r\nDATE/TIME: " + Msg.TimeStampRFC + "\r\n\r\nMESSAGE:\r\n" + Msg.Text, "NEW MESSAGE RECEIVED...", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            objInbox.Clear();

        }

        [STAThread]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.Run(new GameServer());
        }



        internal void RemoveClient(ClientPerson client)
        {
            //removes someone from the game temporarily

            clientPeople.Remove(client);
            UpdateClientListControl();


        }
        #region Windows Forms Designer generated code
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSendMsg = new System.Windows.Forms.Button();
            this.richTextBoxSendMsg = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBoxReceivedMsg = new System.Windows.Forms.RichTextBox();
            this.listBoxClientList = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(508, 488);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 24);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.ButtonCloseClick);
            // 
            // buttonSendMsg
            // 
            this.buttonSendMsg.Location = new System.Drawing.Point(16, 99);
            this.buttonSendMsg.Name = "buttonSendMsg";
            this.buttonSendMsg.Size = new System.Drawing.Size(192, 24);
            this.buttonSendMsg.TabIndex = 7;
            this.buttonSendMsg.Text = "Send Message";
            this.buttonSendMsg.Click += new System.EventHandler(this.ButtonSendMsgClick);
            // 
            // richTextBoxSendMsg
            // 
            this.richTextBoxSendMsg.Location = new System.Drawing.Point(16, 42);
            this.richTextBoxSendMsg.Name = "richTextBoxSendMsg";
            this.richTextBoxSendMsg.Size = new System.Drawing.Size(192, 57);
            this.richTextBoxSendMsg.TabIndex = 6;
            this.richTextBoxSendMsg.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Broadcast Message To Clients";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(217, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Message Received From Clients";
            // 
            // richTextBoxReceivedMsg
            // 
            this.richTextBoxReceivedMsg.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.richTextBoxReceivedMsg.Location = new System.Drawing.Point(217, 43);
            this.richTextBoxReceivedMsg.Name = "richTextBoxReceivedMsg";
            this.richTextBoxReceivedMsg.ReadOnly = true;
            this.richTextBoxReceivedMsg.Size = new System.Drawing.Size(419, 439);
            this.richTextBoxReceivedMsg.TabIndex = 9;
            this.richTextBoxReceivedMsg.Text = "";
            // 
            // listBoxClientList
            // 
            this.listBoxClientList.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxClientList.Location = new System.Drawing.Point(16, 150);
            this.listBoxClientList.Name = "listBoxClientList";
            this.listBoxClientList.Size = new System.Drawing.Size(192, 329);
            this.listBoxClientList.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Connected Clients";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(404, 488);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 24);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // GameServer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(648, 524);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBoxClientList);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBoxReceivedMsg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonSendMsg);
            this.Controls.Add(this.richTextBoxSendMsg);
            this.Name = "GameServer";
            this.Text = "Capricorn Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameServer_FormClosed);
            this.ResumeLayout(false);

        }
        #endregion




        
        // This method could be called by either the main thread or any of the
        // worker threads
        private void AppendToRichEditControl(string msg)
        {

            if (InvokeRequired)
            {
                object[] pList = { msg };
                richTextBoxReceivedMsg.BeginInvoke(new UpdateRichEditCallback(OnUpdateRichEdit), pList);
            }
            else
            {
                // This is the main thread which created this control, hence update it
                // directly 
                OnUpdateRichEdit(msg);
            }
        }


        private void OnUpdateRichEdit(string msg)
        {
            richTextBoxReceivedMsg.AppendText(msg);
            richTextBoxReceivedMsg.SelectionStart = richTextBoxReceivedMsg.Text.Length;
            richTextBoxReceivedMsg.ScrollToCaret();
        }


        internal void UpdateClientListControl()
        {
            if (InvokeRequired) // Is this called from a thread other than the one created
            // the control
            {
                listBoxClientList.BeginInvoke(new UpdateClientListCallback(UpdateClientList), null);
            }
            else
            {
                // This is the main thread which created this control, hence update it
                // directly 
                UpdateClientList();
            }
        }
        void ButtonSendMsgClick(object sender, System.EventArgs e)
        {
            foreach (ClientPerson cp in clientPeople)
            {
                string msg = richTextBoxSendMsg.Text;
                cp.sM("From server support:\r\n" +msg +"\0");
                
            }
            richTextBoxSendMsg.Clear();
        }

        void ButtonCloseClick(object sender, System.EventArgs e)
        {

            Close();
        }

        // Update the list of clients that is displayed
        void UpdateClientList()
        {
            listBoxClientList.Items.Clear();
            for (int i = 0; i < clientPeople.Count; i++)
            {
                listBoxClientList.Items.Add(clientPeople[i].m_name + " = " + clientPeople[i].phone);
            }
        }


        private void btnClear_Click(object sender, System.EventArgs e)
        {
            richTextBoxReceivedMsg.Clear();

        }

        internal void SendText(string recip, string fullMessage)
        {

            lock (locker)
            {
                try
                {
                    if (fullMessage != "\0")
                    {
                        var t = Task.Run(() =>
                        {
                            m_objSMS.SendSMS(recip, fullMessage);
                        });
                        AppendToRichEditControl("\r\nTO: " + recip + "\r\n" + fullMessage);
                    }
                }
                catch (Exception e)
                {
                    AppendToRichEditControl(e.Message + " - " + e.InnerException.Message);
                }
            }
        }

        internal void CheckText()
        {
            //check for messages 

            WebServer ws = new WebServer(GetTextWeb, "http://*:7080/");
            ws.Run();
        }

        public string GetTextWeb(HttpListenerRequest request)
        {
            if (request.QueryString.GetValues("dbid") != null)
            {
               string body = request.QueryString.GetValues("Body")[0];
                string dbid = request.QueryString.GetValues("dbid")[0];
            
                    bool notFound = true;
                    //check and see if they exist in the game already...
                    
                    foreach (ClientPerson client in clientPeople)
                    {
                        if (client.databaseId == dbid)
                        {
                            AppendToRichEditControl("\r\nFrom: " + client.m_name + "\r\n" + body);
                            client.processMessage(body);
                            notFound = false;
                            break;
                        }
                    }
                if (notFound)
                {
                    clientPeople.Add(new ClientPerson(this,"", dbid));
                    //web should always be found so this is a problem
                    //Console.WriteLine("Web User Not Found... " + from + body + dbid);
                }
            }
            return "<HTML><BODY>B</BODY></HTML>";
        }

        internal void SendWeb(string dbid, string fullMessage)
        {
            lock (locker)
            {
                try
                {
                    string sURL;
                    if (fullMessage != "\0")
                    {
                        sURL = String.Format("http://gjonola.com/capricorn/poster.aspx?To={0}&Body={1}&dbid={2}", "web", fullMessage,dbid);
                        WebRequest myWebRequest = WebRequest.Create(sURL);
                        using (WebResponse myWebResponse = myWebRequest.GetResponse())
                        {
                            AppendToRichEditControl("\r\nTO: " + dbid + "\r\n" + fullMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    AppendToRichEditControl(e.Message + " - " + e.InnerException.Message);
                }
            }
        }

        private void GameServer_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            m_objSMS.Disconnect();
        }
    }
}

