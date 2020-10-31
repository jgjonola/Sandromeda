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
    public partial class CharacterEditor : System.Web.UI.Page
    {
        SqlConnection con;
        protected void Page_Load(object sender, EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionStringLocal"].ConnectionString;
            con = new SqlConnection(connStr);
        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            ListBox1.DataBind();
        }

        protected void ButtonGiveItem_Click(object sender, EventArgs e)
        {
            string itemToGive = DropDownListItems.SelectedValue;
            string whoToGive = ListBox1.SelectedValue;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = String.Format("INSERT INTO inventory (clientid, itemid, health) VALUES ({0},{1},{2})", whoToGive,itemToGive,50);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            ListBoxItemsHeld.DataBind();
        }

        protected void ButtonDeleteItem_Click(object sender, EventArgs e)
        {
            string itemToTake = ListBoxItemsHeld.SelectedValue;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = String.Format("DELETE FROM inventory WHERE id = {0}", itemToTake);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            ListBoxItemsHeld.DataBind();
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ListBox1.SelectedValue != null)
                Panel1.Visible = true;
        }
    }
}