using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.UI.HtmlControls;

namespace WebClient
{
    public partial class game : System.Web.UI.Page
    {
        string myid;
        int gridsize = 7;
        int playerID;
        //int scrollSize = 1;
        string serverip = "161.199.245.198";//"10.40.0.21";//"132.148.82.192";//"68.229.32.118" is home;
        string port = "7080";
        protected void Page_Load(object sender, EventArgs e)
        {
            //remove any processed webmessages
            SiteVariables.WM.RemoveAll(x => x.processed == true);


            if (Session["userid"] != null)
            {
                playerID = Convert.ToInt32(Session["userid"]);
                DataClasses1DataContext db = new DataClasses1DataContext();
                var uid = (from u in db.clients
                           where u.id == playerID
                           select u).SingleOrDefault();
                if(uid.is_admin!=null)
                    if ((bool)uid.is_admin)
                    {
                        HyperLinkMaps.Visible = true;
                        HyperLinkActions.Visible = true;
                        HyperLinkCharacters.Visible = true;
                        HyperLinkNPCs.Visible = true;
                        HyperLinkItems.Visible = true;
                    }
                else
                    {
                        HyperLinkMaps.Visible = false;
                        HyperLinkActions.Visible = false;
                        HyperLinkCharacters.Visible = false;
                        HyperLinkNPCs.Visible = false;
                        HyperLinkItems.Visible = false;
                    }
            }
            else
            {
                //we need a user id so go back to the logon screen
                Response.Redirect("default.aspx");
            }

            myid = Request.UserHostAddress;
            MaintainScrollPositionOnPostBack = true;
            CenterMapOnPlayer();

            Session["gridMinX"] = Convert.ToInt32(Session["currentx"]) - 3;
            if (Convert.ToInt32(Session["gridMinX"]) < 0)
                Session["gridMinX"] = 0;
            Session["gridMinY"] = Convert.ToInt32(Session["currenty"]) - 3;
            if (Convert.ToInt32(Session["gridMinY"]) < 0)
                Session["gridMinY"] = 0;
            Session["gridMaxX"] = Convert.ToInt32(Session["gridMinX"]) + gridsize;
            Session["gridMaxY"] = Convert.ToInt32(Session["gridMinY"]) + gridsize;
            

            ListView1.GroupItemCount = (int)gridsize;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            foreach (WebMessages message in SiteVariables.WM)
            {
                if (message.m_db_id==playerID.ToString() & !message.processed)
                {
                    message.m_message = message.m_message.Replace("\0", "");
                    TextBoxServer.Text += message.m_message;
                    message.processed = true;
                }
            }
            DataBind();
        }

        protected void CenterMapOnPlayer()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var uid = (from u in db.clients
                       where u.id == playerID
                       select u).SingleOrDefault();
            if (uid != null)
            {
                Session["currentx"] = uid.room_x.ToString();
                Session["currenty"] = uid.room_y.ToString();
            }

        }
        protected void ImageButtonSend_Click(object sender, ImageClickEventArgs e)
        {
            if (TextBoxSend.Text.StartsWith("IP="))
                {
                    TextBoxServer.Text += "changing ip";
                    serverip = TextBoxSend.Text.Replace("IP=","");
                }
            if (TextBoxSend.Text == "logout")
            {
                //we want to log out of the game
                DataClasses1DataContext db = new DataClasses1DataContext();
                var uid = (from u in db.clients
                           where u.id == playerID
                           select u).SingleOrDefault();
                uid.logged_in = false;
                db.SubmitChanges();
                Session["userid"] = null;
                playerID = 0;
            }
            else
            {
                string sURL;
                try
                {

                    sURL = String.Format("http://{0}:{1}/?From={2}&Body={3}&userid={4}", serverip, port, myid, TextBoxSend.Text, playerID);  //fix this ip should be myid


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

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Panel ip;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("thecell");

                // Display the e-mail address in italics.
                ip = (Panel)e.Item.FindControl("Panel1");
                string doorVisualType = DataBinder.Eval(e.Item.DataItem, "doorVisualType").ToString();
                Label nexitLabel = (Label)e.Item.FindControl("nexitLabel");
                Label nameLabel = (Label)e.Item.FindControl("nameLabel");
                Label idLabel = (Label)e.Item.FindControl("labelId");
                Label visualType = (Label)e.Item.FindControl("visualType");
                //td.Attributes.Add("class", "table_grid");

                DataClasses1DataContext db = new DataClasses1DataContext();
                var query = from np in db.npcs
                            where np.roomid == Convert.ToInt32(idLabel.Text)
                            select np;

                foreach (npc n in query)
                {
                    if (n.health > 0)
                    {
                        Image chicken = new Image();
                        chicken.ToolTip = string.Format("Name:{0}\r\nHealth:{1}", n.name, n.health.ToString());
                        chicken.ImageUrl = string.Format("~/images/npcs/{0}.png",n.generic_name);
                        Panel cpan = new Panel();
                        LiteralControl br = new LiteralControl("<br />");
                        cpan.Style.Add("float", "center");
                        cpan.Controls.Add(chicken);
                        cpan.Controls.Add(br);
                        cpan.HorizontalAlign = HorizontalAlign.Center;
                        ip.Controls.Add(cpan);
                    }
                }

                var getClients = from cl in db.clients
                                 where cl.roomid == Convert.ToInt32(idLabel.Text)
                                 where cl.is_playing == true
                                 select cl;

                foreach (client guy in getClients)
                {
                    if (guy.health > 0)
                    {
                        Image person = new Image();
                        if(guy.id== playerID)
                            person.ImageUrl = "~/images/npcs/me.png";
                        else
                            person.ImageUrl = "~/images/npcs/player.png";
                        //person.Height = Unit.Pixel(20);
                        //person.Width = Unit.Pixel(9);
                        person.ToolTip = String.Format("Health:{0}", guy.health);
                        Panel cpan = new Panel();
                        //Label chlab = new Label();
                        LiteralControl br = new LiteralControl("<br />");
                        //chlab.Text = guy.name;
                        //chlab.Font.Size = FontUnit.XXSmall;
                        cpan.Controls.Add(person);
                        cpan.Style.Add("float", "center");
                        cpan.Controls.Add(br);
                        //cpan.Controls.Add(chlab);
                        ip.Controls.Add(cpan);
                        ip.HorizontalAlign = HorizontalAlign.Center;
                    }

                }

                if (ip != null)
                {
                    if (visualType.Text !="none")
                        td.Attributes.Add("class", visualType.Text);
                    if (doorVisualType != "none")
                        ip.Attributes.Add("class", string.Format("table_grid_cell128 {0}",doorVisualType));
                    else
                        ip.Attributes.Add("class", "table_grid_cell128");
                }
                   
            }

        }
    }
}