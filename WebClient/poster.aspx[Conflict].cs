using System;


namespace WebClient
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                string to = Request["To"];
                string body = Request["Body"];
            string dbid = Request["dbid"];
                //we have a get from the server
                //create a webmessage and exit
                SiteVariables.WM.Add(new WebMessages(to, body, dbid));
        }
    }
}