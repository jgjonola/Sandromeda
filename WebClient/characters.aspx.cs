using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class characters : System.Web.UI.Page
    {
        List<client> clientList;
        List<item> itemList;
        List<item_template> newItemList;
        


        protected void Page_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            clientList = (from cl in db.clients
                       select cl).ToList();

            RepeaterClientList.DataSource = clientList;
            RepeaterClientList.DataBind();
        }

        protected void RepeaterClientList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (e.CommandName == "Select")
            {
                PanelClient.Visible = true;
                LinkButton manager = e.Item.FindControl("LBSelect") as LinkButton;
                manager.ForeColor = System.Drawing.Color.Red;
                client selectedClient = (from cl in clientList
                                     where cl.id == Convert.ToInt32(e.CommandArgument)
                                     select cl).SingleOrDefault();
                TextBoxName.Text = selectedClient.name;
                TextBoxDescription.Text = selectedClient.description;
                TextBoxHeath.Text = selectedClient.health.ToString();
                TextBoxPassword.Text = selectedClient.password;
                TextBoxPhone.Text = selectedClient.phone;
                TextBoxRoomID.Text = selectedClient.roomid.ToString();
                TextBoxUsername.Text = selectedClient.username;
                CheckBoxAdmin.Checked = (bool)selectedClient.is_admin;
                LabelLastConnected.Text = "Last Connected:" + selectedClient.last_connect.ToString();
                Session["SelectedUser"] = e.CommandArgument.ToString();
                PanelInventory.Visible = false;
               
            }
        }

        protected void LinkButtonSave_Click(object sender, EventArgs e)
        {
            if(Session["SelectedUser"]!=null)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                client editedClient = (from ecl in db.clients
                                       where ecl.id == Convert.ToInt32(Session["SelectedUser"].ToString())
                                       select ecl).SingleOrDefault();
                editedClient.name=TextBoxName.Text;
                editedClient.description=TextBoxDescription.Text;
                editedClient.health=Convert.ToInt32(TextBoxHeath.Text);
                editedClient.password=TextBoxPassword.Text;
                editedClient.phone=TextBoxPhone.Text;
                editedClient.roomid= Convert.ToInt32(TextBoxRoomID.Text);
                editedClient.username=TextBoxUsername.Text;
                editedClient.is_admin=CheckBoxAdmin.Checked;
                db.SubmitChanges();
                TextBoxName.Text = "";
                TextBoxDescription.Text = "";
                TextBoxHeath.Text= "";
                TextBoxPassword.Text = "";
                TextBoxPhone.Text = "";
                TextBoxRoomID.Text = "";
                TextBoxUsername.Text = "";
                CheckBoxAdmin.Checked = false;
                LabelLastConnected.Text = "";
                Session["SelectedUser"] = null;
                PanelClient.Visible = false;
            } 
        }

        protected void LinkButtonCancel_Click(object sender, EventArgs e)
        {
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
            TextBoxHeath.Text = "";
            TextBoxPassword.Text = "";
            TextBoxPhone.Text = "";
            TextBoxRoomID.Text = "";
            TextBoxUsername.Text = "";
            CheckBoxAdmin.Checked = false;
            LabelLastConnected.Text = "";
            Session["SelectedUser"] = null;
            PanelClient.Visible = false;
        }

        protected void LinkButtonShowInventory_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            itemList = (from i in db.items
                        where i.clientid==Convert.ToInt32(Session["SelectedUser"])
                          select i).ToList();

            PanelInventory.Visible = true;
            RepeaterInventory.DataSource = itemList;
            RepeaterInventory.DataBind(); 
        }

        protected void RepeaterInventory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if(e.CommandName=="Delete")
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                item deleteItem = (from di in db.items
                                         where di.id == Convert.ToInt32(e.CommandArgument)
                                         select di).SingleOrDefault();
                db.items.DeleteOnSubmit(deleteItem);
                db.SubmitChanges();
                itemList = (from i in db.items
                            where i.clientid == Convert.ToInt32(Session["SelectedUser"])
                            select i).ToList();

                PanelInventory.Visible = true;
                RepeaterInventory.DataSource = itemList;
                RepeaterInventory.DataBind();

            }
        }

        protected void LinkButtonAddInventory_Click(object sender, EventArgs e)
        {
            if (!DropDownListAdd.Visible)
            {
                //show the items available to add
                DataClasses1DataContext db = new DataClasses1DataContext();
                newItemList = (from ni in db.item_templates
                               select ni).ToList();
                DropDownListAdd.DataSource = newItemList;
                DropDownListAdd.DataTextField = "name";
                DropDownListAdd.DataValueField = "id";
                DropDownListAdd.DataBind();
                DropDownListAdd.Visible = true;
            }
            else
            {
                //add the selected item
                DataClasses1DataContext db = new DataClasses1DataContext();
                item_template newItem = (from ni in db.item_templates
                                       where ni.id == Convert.ToInt32(DropDownListAdd.SelectedValue)
                                       select ni).SingleOrDefault();
                item insertItem = new item
                {
                    type = newItem.type,
                    generic_name = newItem.item_type_text,
                    description = newItem.description,
                    location_desc = newItem.location_description,
                    image = newItem.image,
                    item_stackable = newItem.item_stackable,
                    can_be_held = newItem.can_be_held,
                    clientid = Convert.ToInt32(Session["SelectedUser"]),
                    damage = 0,
                    special = newItem.special,
                    spawner_id = newItem.spawner_id,
                    size = newItem.size,
                    name = newItem.name,
                    container = newItem.container,
                    room_limit = newItem.room_limit,
                    can_be_seen_when_held = newItem.can_be_seen_when_held,
                    key_code = newItem.key_code,
                    health = 50     
                };
                db.items.InsertOnSubmit(insertItem);
                db.SubmitChanges();
                DropDownListAdd.Visible = false;
                itemList = (from i in db.items
                            where i.clientid == Convert.ToInt32(Session["SelectedUser"])
                            select i).ToList();

                PanelInventory.Visible = true;
                RepeaterInventory.DataSource = itemList;
                RepeaterInventory.DataBind();
            }
        }

        protected void LinkButtonExitInventory_Click(object sender, EventArgs e)
        {
            PanelInventory.Visible = false;
        }
    }
}