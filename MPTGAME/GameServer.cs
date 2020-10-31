
using System;
using System.Net;
using GameHost;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
//using MessagingToolkit.Core;
//using MessagingToolkit.Core.Mobile;
//using MessagingToolkit.Core.Mobile.Message;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public SqlConnection con;
        //string str = @"Data Source=capricornisland.db.2498781.hostedresource.com; Initial Catalog=capricornisland; MultipleActiveResultSets=true; User ID=capricornisland; Password='Dragon123!'";
        public delegate void UpdateRichEditCallback(string text);
        public delegate void UpdateClientListCallback();
        public AsyncCallback pfnWorkerCallBack;
        private System.Windows.Forms.ListBox listBoxClientList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClear;
        Object locker;
        public LoadingForm loadingForm;
        public List<action> m_actions;
       // MessageGateway<IMobileGateway, MobileGatewayConfiguration> messageGateway;
        //IMobileGateway mobileGateway;
        object textLocker;
        object sendlock;


        public GameServer()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            loadingForm = new LoadingForm();

            locker = new object();
            sendlock = new object();
            textLocker = new object();
            clientPeople = new List<ClientPerson>();
            loadingForm.Show(this);
            map = new Map(this);
            //SetupSMS();
            CheckText();
            try
            {
                using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
                {
                    if (gdb.DatabaseExists())
                    {
                        m_actions = (from acts in gdb.actions
                                     select acts).ToList();
                    }
                    else
                    {
                        //we have a problem and cant connect to the database...  
                        //damn, what do we do.  Try again?
                        AppendToRichEditControl("\r\nThere was an error connecting to the database in the GameServer constructor.  Actions is now null.\r\n");
                        MessageBox.Show("Can't connect to database.");
                        Close();
                    }
                }
            }
            catch(Exception e)
            {
                AppendToRichEditControl("\r\nThere was an error connecting to the database in the GameServer constructor.  Actions is now null.  Error:" + e.Message+"\r\n");
            }
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameServer_FormClosing);
            this.ResumeLayout(false);

        }
        #endregion


        
        // This method could be called by either the main thread or any of the
        // worker threads
        public void AppendToRichEditControl(string msg)
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
            foreach(ClientPerson client in clientPeople)
            {
                listBoxClientList.Items.Add(client.m_name + "=" + client.databaseId + "@" + client.phone);
            }
        }

        private void SetupSMS()
        {
            //try
            //{
            //    mobileGateway = MobileGatewayFactory.Default;
            //    MobileGatewayConfiguration config = MobileGatewayConfiguration.NewInstance();
            //    config.PortName = "COM5";
            //    config.BaudRate = PortBaudRate.BitsPerSecond115200;
            //    config.DataBits = PortDataBits.Eight;
            //    config.Parity = PortParity.None;
            //    config.StopBits = PortStopBits.One;
            //    config.Handshake = PortHandshake.None;
            //    config.DisablePinCheck = true;
            //    config.ConcatenateMessage = true;
            //    config.DtrEnable = true;
            //    config.RtsEnable = true;
            //    config.LicenseKey = "850801-02-6191";


            //    messageGateway = MessageGateway<IMobileGateway, MobileGatewayConfiguration>.NewInstance();
            //    mobileGateway = messageGateway.Find(config);

            //    if (mobileGateway != null)
            //    {
            //        Console.WriteLine("connected to Gateway");
            //        AppendToRichEditControl("\r\nConnected to Modem\r\n");
            //        mobileGateway.EnableNewMessageNotification(MessageNotification.ReceivedMessage);
            //        mobileGateway.MessageReceived += GotSMS;
            //        mobileGateway.DeleteMessage(MessageDeleteOption.AllMessages); //delete any old messages

            //    }
            //    else
            //    {
            //        Console.WriteLine("Error connecting to gateway!");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception connecting to gateway: {0}. Source: {1}.", e.Message, e.Source);
            //    AppendToRichEditControl("Error connecting to modem:" + e.Message);
            //}
        }



        private void btnClear_Click(object sender, System.EventArgs e)
        {
            
        }

        //internal void SendText(string username, string recip, string mes)
        //{
        //    var t = Task.Run(() =>
        //    {
        //        lock (textLocker)
        //        {
        //            try
        //            {
        //                Sms sms = Sms.NewInstance();
        //                sms.LongMessageOption = MessageSplitOption.Concatenate;
        //                sms.DestinationAddress = recip;
        //                sms.Content = mes;
        //                mobileGateway.Send(sms);
        //                Console.WriteLine("Sending Message:{0} To:{1}", mes, recip);
        //                AppendToRichEditControl(string.Format("\r\nTo {0}({1}):\r\n{2}\r\n", username, recip, mes));
        //                Console.WriteLine("Sent it!");
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Exception when sending message: {0} From:{1}", e.Message, e.Source);
        //                AppendToRichEditControl("Error in Send SMS: " + e.Message + mobileGateway.LastError.Message);
        //            }
        //        }
        //    });
        //}

        internal void CheckText()
        {
            //check for messages 
            WebServer ws = new WebServer(GetTextWeb, "http://*:7080/");
            ws.Run();
        }

        //private void GotSMS(object sender, MessagingToolkit.Core.Mobile.Event.MessageReceivedEventArgs e)
        //{ 
        //    //lock(sendlock)
        //    //{
        //        Console.WriteLine(e.Message.PhoneNumber + ":" + e.Message.Content);
        //        bool notFound = true;
        //        foreach (ClientPerson client in clientPeople)
        //        {
        //            if (client.phone == e.Message.PhoneNumber)
        //            {
        //                AppendToRichEditControl("\r\nFrom: " + client.m_name + "\r\n" + e.Message.Content+"\r\n");
        //                client.processMessage(e.Message.Content);
        //                notFound = false;
        //                if (client.m_loggedin)
        //                {
        //                    client.m_loggedin = false;
        //                using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
        //                {
        //                    if (gdb.DatabaseExists())
        //                    {
        //                        var q6 = (from clts in gdb.clients
        //                                  where clts.phone == e.Message.PhoneNumber
        //                                  select clts).SingleOrDefault();
        //                        q6.logged_in = false;
        //                        gdb.SubmitChanges();
        //                    }
        //                    else
        //                    {
        //                        //couldn't fidn the client
        //                        AppendToRichEditControl("\r\nCouldn't tell the database that someone was logged in due to the database not being connected in GetSMS()\r\n");
        //                    }
        //                }
        //                }
        //                //delete message
        //                mobileGateway.DeleteMessage(MessageDeleteOption.ByIndex,e.Message.Index);
        //                break;
        //            }
        //        }
        //    if (notFound)
        //    {

        //        using (GameDatabaseDataContext gdb = new GameDatabaseDataContext())
        //        {
        //            if (gdb.DatabaseExists())
        //            {
        //                var q6 = (from clts in gdb.clients
        //                          where clts.phone == e.Message.PhoneNumber
        //                          select clts).SingleOrDefault();
        //                if (q6.id != 0)
        //                {
        //                    q6.logged_in = false;
        //                    gdb.SubmitChanges();
        //                }
        //                clientPeople.Add(new ClientPerson(this, e.Message.PhoneNumber, q6.id.ToString()));
        //            }
        //            else
        //            {
        //                AppendToRichEditControl("\r\nA new person tried to join, but I couldn't connect to the database in GotSMS() in GameServer.\r\n");
        //            }
        //        }
        //        }
        //}

        public string GetTextWeb(HttpListenerRequest request)
        {
            
            if (request.QueryString.GetValues("userid") != null)
            {
                    string from = request.QueryString.GetValues("From")[0];
                    string body = request.QueryString.GetValues("Body")[0];
                    string userid = request.QueryString.GetValues("userid")[0];
                    bool notFound = true;
                    //check and see if they exist in the game already...

                    foreach (ClientPerson client in clientPeople)
                    {
                    if (client.databaseId == userid)
                    {
                        AppendToRichEditControl("\r\nFrom: " + client.m_name + "\r\n" + body+"\r\n");
                            client.m_loggedin = true;
                            client.processMessage(body);
                            notFound = false;
                            break;
                        }
                    }
                    if (notFound)
                    {
                        clientPeople.Add(new ClientPerson(this, from, userid));
                    }
            }
            return "got it!";
        }

        internal void SendWeb(string p, string fullMessage, string dbid)
        {
            lock (locker)
            {
                try
                {
                    string sURL;
                    if (fullMessage != "\0")
                    {
                        sURL = String.Format("http://gjonola.com/capricorn/poster.aspx?To={0}&Body={1}&dbid={2}", p, fullMessage,dbid);
                        WebRequest myWebRequest = WebRequest.Create(sURL);
                        using (WebResponse myWebResponse = myWebRequest.GetResponse())
                        {
                            AppendToRichEditControl("\r\nTO: " + p + "\r\n" + fullMessage+"\r\n");
                        }
                    }
                }
                catch (Exception e)
                {
                    AppendToRichEditControl(e.Message + " - " + e.InnerException.Message);
                }
            }
        }

        private void GameServer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //mobileGateway.Disconnect();
        }
    }
}

