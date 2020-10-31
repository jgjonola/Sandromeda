using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace WebClient
{
    public partial class default2 : System.Web.UI.Page
    {
        string myid;
        protected void Page_Load(object sender, EventArgs e)
        {
            myid = Request.UserHostAddress;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (WebMessages message in SiteVariables.WM)
            {
                if (message.m_id == myid & !message.processed)
                {
                    //message.m_message = message.m_message.Replace("\r", "'r");
                    //message.m_message = message.m_message.Replace("\n", "'n");
                    message.m_message = message.m_message.Replace("\0", "");
                    TextBoxServer.Text += message.m_message;
                    message.processed = true;
                }
            }
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {

        }

        protected void ImageButtonSend_Click(object sender, ImageClickEventArgs e)
        {
            string sURL;
            try
            {
                //sURL = String.Format("http://174.47.7.35/?From=web&Body={0}",TextBoxSend.Text);
                sURL = String.Format("http://174.47.7.35/?From={0}&Body={1}", myid, TextBoxSend.Text);  //fix this ip should be myid
                //sURL = String.Format("http://localhost/?From={0}&Body={1}", myid, TextBoxSend.Text);

                WebRequest myWebRequest = WebRequest.Create(sURL);
                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    TextBoxServer.Text += "\r\n\r\n>" + TextBoxSend.Text + "\r\n\r\n";
                }

                TextBoxSend.Text = "";
                SetFocus(TextBoxSend);
            }
            catch (Exception t)
            {
                // TextBoxServer.Text += t.Message + " " + t.InnerException.Message;
                Response.Write(t.Message);
            }
        }
    }
}