using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WebClient
{
    public partial class RoomEditor : System.Web.UI.Page
    {
        //0 is room 
        //1 open door without lock
        //2 is open door with lock
        //3 is closed door without lock 
        //4 closed door with lock  
        //5 closed locked door
        //6 is soft boundry  
        //7 wall

        int gridsize = 10;
        int scrollSize=1;


        protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
            if (!this.IsPostBack)
            {
                Session["gridMinX"] = 0;
                Session["gridMinY"] = 0;
                Session["gridMaxX"] = gridsize;
                Session["gridMaxY"] = gridsize;
            }

            ListView1.GroupItemCount = (int)gridsize;

        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Panel ip;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                // Display the e-mail address in italics.
                ip = (Panel)e.Item.FindControl("Panel1");
                Label nexitLabel = (Label)e.Item.FindControl("nexitLabel");
                Label nameLabel = (Label)e.Item.FindControl("nameLabel");
                Label idLabel = (Label)e.Item.FindControl("labelId");


                DataClasses1DataContext db = new DataClasses1DataContext();
                var query = from np in db.npcs
                            where np.roomid == Convert.ToInt32(idLabel.Text)
                            select np;

                foreach (npc n in query)
                {
                    if (n.health > 0)
                    {
                        Image chicken = new Image();
                        chicken.ToolTip = string.Format("Name:{0}\r\nHealth:{1}",n.name,n.health.ToString());
                        if (n.id == 1)
                            chicken.ImageUrl = "~/images/chicken.png";
                        if (n.id == 2)
                            chicken.ImageUrl = "~/images/dirty_chicken.png";
                        chicken.Height = Unit.Pixel(32);
                        chicken.Width = Unit.Pixel(32);
                        Panel cpan = new Panel();
                        Label chlab = new Label();
                        LiteralControl br = new LiteralControl("<br />");
                        chlab.Text = n.generic_name;
                        chlab.Font.Size = FontUnit.XXSmall;
                        cpan.Style.Add("float", "left");
                        cpan.Controls.Add(chicken);
                        cpan.Controls.Add(br);
                        cpan.Controls.Add(chlab);
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
                        person.ImageUrl = "~/images/player1.png";
                        person.Height = Unit.Pixel(20);
                        person.Width = Unit.Pixel(9);
                        person.ToolTip = String.Format("Health:{0}", guy.health);
                        Panel cpan = new Panel();
                        Label chlab = new Label();
                        LiteralControl br = new LiteralControl("<br />");
                        chlab.Text = guy.name;
                        chlab.Font.Size = FontUnit.XXSmall;
                        cpan.Controls.Add(person);
                        cpan.Style.Add("float", "left");
                        cpan.Controls.Add(br);
                        cpan.Controls.Add(chlab);
                        ip.Controls.Add(cpan);
                        ip.HorizontalAlign = HorizontalAlign.Center;
                    }
                    
                }




                if (ip != null)
                switch (nexitLabel.Text)
                {
                    case "0":
                        ip.BackImageUrl = "~/images/med.png";
                        break;
                    case "1":
                        ip.BackImageUrl = "~/images/opendoor.jpg";
                            nameLabel.Visible = false;
                            break;
                    case "2":
                        ip.BackImageUrl = "~/images/opendoor.jpg";
                            nameLabel.Visible = false;
                            break;
                    case "3":
                        ip.BackImageUrl = "~/images/closeddoor.jpg";
                            nameLabel.Visible = false;
                            break;
                    case "4":
                        ip.BackImageUrl = "~/images/closeddoor.jpg";
                            nameLabel.Visible = false;
                            break;
                    case "5":
                        ip.BackImageUrl = "~/images/locked.jpg";
                            nameLabel.Visible = false;
                            break;
                    case "6":
                        ip.BackImageUrl = "~/images/clouds.jpg";
                            //nameLabel.Visible = false;
                            break;
                    case "7":
                            if (nameLabel.Text != "NEW")
                            {
                                ip.BackImageUrl = "~/images/Dungeon_wall2.jpg";
                                nameLabel.Visible = false;
                            }
                            else
                            {
                                ip.BackImageUrl = "";
                                nameLabel.Visible = false;
                                //idLabel.Visible = false;
                            }
                        break;
                }
            }
            
        }

        

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void ButtonBigger_Click(object sender, EventArgs e)
        {
            int oldx;
            int oldy;
            DataClasses1DataContext db = new DataClasses1DataContext();
            var query = (from rm in db.rooms
                         orderby rm.x descending
                         select rm).First();

            oldx = query.x;

            var query2 = (from rm in db.rooms
                         orderby rm.y descending
                         select rm).First();
            oldy = query2.y;

            //we have y+1 * x+1 total rows
            //we have to add y+1 + x+1 + 1 rows

            //calc the (x,y)

            //insert rows with a constant x and increasing y 
            //insert rows with a constant y and increasing x
            for (int i = 0; i < oldy + 1; i++)
            {
                room newRoom = new room
                {
                    //set x and all
                    x = i,
                    y=oldy+1,
                    description = "Inaccessable",
                    name = "NEW",
                    nexit = 7,
                    nkey=0,
                    type=0,
                    visualType=0
                };
                db.rooms.InsertOnSubmit(newRoom);
            }
            for (int j = 0; j < oldx + 2; j++)
            {
                room newRoom2 = new room
                {
                    //set x and all
                    x = oldx+1,
                    y = j,
                    description = "Inaccessable",
                    name = "NEW",
                    nexit = 7,
                    nkey = 0,
                    type = 0,
                    visualType = 0
                };
                db.rooms.InsertOnSubmit(newRoom2);
            }
            db.SubmitChanges();

            DataClasses1DataContext db2 = new DataClasses1DataContext();
            var query3 = (from rm in db.rooms
                         orderby rm.x descending
                         select rm).First();
            ListView1.GroupItemCount = query3.x + 1;
            DataBind();
        }

        protected void ButtonScrollRight_Click(object sender, EventArgs e)
        {
            int tempmix = (int)Session["gridMinX"];
            int tempmax = (int)Session["gridMaxX"];
            tempmix += scrollSize;
            tempmax += scrollSize;
            Session["gridMinX"]=tempmix;
            Session["gridMaxX"]=tempmax;

        }

        protected void ButtonWest_Click(object sender, EventArgs e)
        {
            
            int tempmix = (int)Session["gridMinX"];
            int tempmax = (int)Session["gridMaxX"];
            if (tempmix != 0)
            {
                tempmix -= scrollSize;
                tempmax -= scrollSize;
                Session["gridMinX"] = tempmix;
                Session["gridMaxX"] = tempmax;
            }
        }

        protected void ButtonScrollNorth_Click(object sender, EventArgs e)
        {
            int tempmiy = (int)Session["gridMinY"];
            int tempmay = (int)Session["gridMaxY"];
            tempmiy += scrollSize;
            tempmay += scrollSize;
            Session["gridMinY"] = tempmiy;
            Session["gridMaxY"] = tempmay;
        }

        protected void ButtonScrollSouth_Click(object sender, EventArgs e)
        {
            int tempmiy = (int)Session["gridMinY"];
            int tempmay = (int)Session["gridMaxY"];
            if (tempmiy != 0)
            {
                tempmiy -= scrollSize;
                tempmay -= scrollSize;
                Session["gridMinY"] = tempmiy;
                Session["gridMaxY"] = tempmay;
            }
        }
    }
}