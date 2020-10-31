using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameHost;
using System.Net;
using System.IO;
using System.Web;

namespace GameClient
{
    public partial class Form1 : Form
    {

        public delegate void UpdateRichEditCallback(string text);
        string ip = "";

        public Form1()
        {
            InitializeComponent();

            string strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ad in addr)
            {
                if (ad.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Console.WriteLine("Local Machine's ip: " + ad.ToString());
                    ip = ad.ToString();
                    break;
                }
            }

            NATUPNPLib.UPnPNAT upnpnat = new NATUPNPLib.UPnPNAT();
            NATUPNPLib.IStaticPortMappingCollection mappings = upnpnat.StaticPortMappingCollection;

            mappings.Add(9000, "TCP", 9000, ip, true, "Local Web Server");
            CheckText();
            
        }

        internal void CheckText()
        {
            //check for messages 

            WebServer ws = new WebServer(GetText, "http://*:9000/");
            ws.Run();
        }

        public string GetText(HttpListenerRequest request)
        {
            if (request.QueryString.GetValues("From") != null)
            {
                string from = request.QueryString.GetValues("From")[0];
                string body = request.QueryString.GetValues("Body")[0];
                AppendToRichEditControl(body + "\r\n\r\n");
                return string.Format("<HTML><BODY>From: {0} Message: {1}<br></BODY></HTML>", from,body);
            }
            return string.Format("<HTML><BODY>Test Page 2.<br>{0}</BODY></HTML>", DateTime.Now);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string sURL;
            try
            {
                sURL = String.Format("http://174.47.7.35/?From=web&Body={0}", textBoxMessage.Text);
                //sURL = String.Format("http://10.10.3.195/?From=web&Body={0}", textBoxMessage.Text);

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);

                var response = wrGETURL.GetResponse();
                int start = richTextBoxServer.TextLength;
                richTextBoxServer.AppendText("CLIENT:" + "\r\n");
                richTextBoxServer.AppendText(textBoxMessage.Text + "\r\n\r\n");
                int end = richTextBoxServer.TextLength;
                richTextBoxServer.Select(start, end - start);
                richTextBoxServer.SelectionColor = Color.Red;
                richTextBoxServer.SelectionLength = 0;
                //richTextBoxServer
                textBoxMessage.Clear();
            }
            catch (Exception t)
            {
                Console.WriteLine(t.Message);
            }
        }

        private void AppendToRichEditControl(string msg)
        {

            if (InvokeRequired)
            {
                object[] pList = { msg };
                richTextBoxServer.BeginInvoke(new UpdateRichEditCallback(OnUpdateRichEdit), pList);
                
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
            msg = msg.Replace("\0", "");
            richTextBoxServer.AppendText(msg+"\r\n");
        }

        private void textBoxMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                buttonSend.PerformClick();
                e.Handled = true;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxServer.Clear();
        }
    }
}
