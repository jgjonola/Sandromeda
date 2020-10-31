using System;
using System.Linq;


namespace WebClient
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var user = (from usr in db.clients
                        where usr.username == username.Text &&
                              usr.password == Password.Text
                        select usr).SingleOrDefault();
            if (user != null)
            {
                Session["userid"] = user.id.ToString();
                user.logged_in = true;
                db.SubmitChanges();
                Response.Redirect("game.aspx");
            }
            else
            {
                //we don't have a successful login
                LabelError.ForeColor = System.Drawing.Color.Red;
                LabelError.Text = "Password Incorrect, Try Again Loser";

            }
            
        }

        protected void LinkButtonSignup_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var user = (from usr in db.clients
                        where usr.username == username.Text &&
                              usr.password == Password.Text
                        select usr).SingleOrDefault();
            if (user != null)
            {
                Session["userid"] = user.id.ToString();
                user.logged_in = true;
                db.SubmitChanges();
                Response.Redirect("game.aspx");
            }
            else
            {
                //we don't have a successful login so create a new user
                var userCheck = (from usr in db.clients
                            where usr.username == username.Text 
                            select usr).SingleOrDefault();
                if (userCheck == null)
                {
                    client newclient = new client
                    {
                        name = username.Text,
                        password = Password.Text,
                        first_connect = DateTime.Now,
                        username = username.Text,
                        ip_add = "0",
                        phone = "",
                        last_connect = DateTime.Now,
                        last_touch = DateTime.Now,
                        description = "TEST USER",
                        room_x = 4,
                        room_y = 4,
                        roomid = 37,
                        health = 50,
                        is_playing = true,
                        logged_in = true,
                        is_admin = false
                    };
                    if (Password.Text != "")
                    {

                        db.clients.InsertOnSubmit(newclient);
                        db.SubmitChanges();
                        Session["userid"] = newclient.id.ToString();
                        Response.Redirect("game.aspx");
                    }
                    else
                    {
                        LabelError.ForeColor = System.Drawing.Color.Red;
                        LabelError.Text = "Please put in a password, idiot.";
                    }
                }
                else
                {
                    LabelError.ForeColor = System.Drawing.Color.Red;
                    LabelError.Text = "There's already a Sandroid with that name.";

                }
            }
        }
    }
}