using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class npcs : System.Web.UI.Page
    {
        List<npc> npcList;
        List<NPC_Template> npcTemplateList;
        List<item> itemList;
        List<item_template> newItemList;
        List<NPCPhrase> NPCPhrases;


        protected void Page_Load(object sender, EventArgs e)
        {


        }



        protected void LinkButtonShowInventory_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            itemList = (from i in db.items
                        where i.clientid == Convert.ToInt32(Session["SelectedNPC"])
                        select i).ToList();

            PanelInventory.Visible = true;
            RepeaterInventory.DataSource = itemList;
            RepeaterInventory.DataBind();
        }



        protected void RepeaterInventory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                item deleteItem = (from di in db.items
                                   where di.id == Convert.ToInt32(e.CommandArgument)
                                   select di).SingleOrDefault();
                db.items.DeleteOnSubmit(deleteItem);
                db.SubmitChanges();
                itemList = (from i in db.items
                            where i.clientid == Convert.ToInt32(Session["SelectedNPC"])
                            select i).ToList();

                PanelInventory.Visible = true;
                RepeaterInventory.DataSource = itemList;
                RepeaterInventory.DataBind();

            }
        }

        protected void LinkButtonAddInventory_Click(object sender, EventArgs e)
        {
            if (Session["SelectedNPC"] != null)
            {
                //we are adding to an existing NPCs inventory
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
                        clientid = Convert.ToInt32(Session["SelectedNPC"]),
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
                                where i.clientid == Convert.ToInt32(Session["SelectedNPC"])
                                select i).ToList();

                    PanelInventory.Visible = true;
                    RepeaterInventory.DataSource = itemList;
                    RepeaterInventory.DataBind();
                }
            }
            if(Session["NewTemplate"]!=null)
            {
                //we are adding to a new templates starting inventory
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
                    //add the selected item to the NPC_Initial_Inventory table
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    NPC_Initial_Inventory insertInitial = new NPC_Initial_Inventory
                    {
                        NPCID = Convert.ToInt32(Session["NewTemplate"]),
                        itemid = Convert.ToInt32(DropDownListAdd.SelectedValue),
                        groupid = 0
                    };

                    db.NPC_Initial_Inventories.InsertOnSubmit(insertInitial);
                    db.SubmitChanges();
                    DropDownListAdd.Visible = false;
                    var templateList = (from i in db.NPC_Initial_Inventories
                                        join it in db.item_templates
                                        on i.itemid equals it.id
                                where i.NPCID == Convert.ToInt32(Session["NewTemplate"])
                                select it).ToList();

                    PanelInventory.Visible = true;
                    RepeaterInventory.DataSource = templateList;
                    RepeaterInventory.DataBind();
                }
            }

            if (Session["SelectedNPCTemplate"] != null)
            {
                //we are editing a templates starting inventory
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
                    //add the selected item to the NPC_Initial_Inventory table
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    NPC_Initial_Inventory insertInitial = new NPC_Initial_Inventory
                    {
                        NPCID = Convert.ToInt32(Session["SelectedNPCTemplate"]),
                        itemid = Convert.ToInt32(DropDownListAdd.SelectedValue),
                        groupid = 0
                    };

                    db.NPC_Initial_Inventories.InsertOnSubmit(insertInitial);
                    db.SubmitChanges();
                    DropDownListAdd.Visible = false;
                    var templateList = (from i in db.NPC_Initial_Inventories
                                        join it in db.item_templates
                                        on i.itemid equals it.id
                                        where i.NPCID == Convert.ToInt32(Session["SelectedNPCTemplate"])
                                        select it).ToList();

                    PanelInventory.Visible = true;
                    RepeaterInventory.DataSource = templateList;
                    RepeaterInventory.DataBind();
                }
            }

        }

        protected void LinkButtonExitInventory_Click(object sender, EventArgs e)
        {
            PanelInventory.Visible = false;
        }

        protected void LinkButtonCreate_Click(object sender, EventArgs e)
        {
            //creat a new template of a NPC
            Session["NewTemplate"] = null;
            Session["SelectedNPC"] = null;
            Session["SelectedNPCTemplate"] = null;
            DataClasses1DataContext db = new DataClasses1DataContext();
            npcTemplateList = (from np in db.NPC_Templates
                               select np).ToList();
            ListBoxNPC.DataSource = npcTemplateList;
            ListBoxNPC.DataValueField = "id";
            ListBoxNPC.DataTextField = "Name";
            ListBoxNPC.DataBind();
            PanelList.Visible = true;
            PanelNPC.Visible = false;
            LabelTitle.Text = "Create New Template";
        }


        protected void LinkButtonEditNPC_Click(object sender, EventArgs e)
        {
            //edit the list of in game npcs
            LabelTitle.Text = "NPC Editor";
            DataClasses1DataContext db = new DataClasses1DataContext();
            npcList = (from np in db.npcs
                       select np).ToList();
            ListBoxNPC.DataSource = npcList;
            ListBoxNPC.DataValueField = "id";
            ListBoxNPC.DataTextField = "Name";
            ListBoxNPC.DataBind();

            PanelNPC.Visible = true;
            PanelList.Visible = true;
            Session["NewTemplate"] = null;
            Session["SelectedNPC"] = null;
            Session["SelectedNPCTemplate"] = null;
        }


        protected void LinkButtonSaveNew_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (LabelTitle.Text== "Create New Template")
            {
                //we are adding a new one
                NPC_Template newNpc = new NPC_Template
                {
                    name = TextBoxNewName.Text,
                    generic_name = TextBoxNewName.Text,
                    description = TextBoxNewDescription.Text,
                    starting_health = Convert.ToInt32(TextBoxStartingHealth.Text),
                    aggressiveness = Convert.ToInt32(TextBoxNewAggressiveness.Text),
                    movement_timer = Convert.ToInt32(TextBoxNewMovementTimer.Text),
                    movement_radius = Convert.ToInt32(TextBoxNewMovementRadius.Text),
                    movement_type = Convert.ToInt32(TextBoxNewMovementType.Text),
                    health_timer = Convert.ToInt32(TextBoxNewHealthTimer.Text),
                    respawn_timer = Convert.ToInt32(TextBoxNewRespawnTimer.Text),
                    attack = Convert.ToInt32(TextBoxNewAttack.Text),
                    carry = Convert.ToInt32(TextBoxNewCarry.Text),
                    has_dialog = true,
                    is_food = CheckBoxNewIsFood.Checked
                };
                db.NPC_Templates.InsertOnSubmit(newNpc);
                db.SubmitChanges();

                //Switch to editing mode since we added already
                Session["SelectedNPCTemplate"] = newNpc.id;
                LabelTitle.Text = "Edit Existing Template";
                //reload the list to include the added one
                npcTemplateList = (from np in db.NPC_Templates
                           select np).ToList();

                ListBoxNPC.DataSource = npcTemplateList;
                ListBoxNPC.DataValueField = "id";
                ListBoxNPC.DataTextField = "Name";
                ListBoxNPC.DataBind();
                ListBoxNPC.SelectedValue = newNpc.id.ToString();

            }
            else
            if(LabelTitle.Text=="Edit Existing Template")
            {
                //we are editing an existing template

                NPC_Template editedNPC = (from np in db.NPC_Templates
                                 where np.id == Convert.ToInt32(Session["SelectedNPCTemplate"].ToString())
                                 select np).SingleOrDefault();
                editedNPC.name = TextBoxNewName.Text;
                editedNPC.generic_name = TextBoxNewName.Text;
                editedNPC.description = TextBoxNewDescription.Text;
                editedNPC.starting_health = Convert.ToInt32(TextBoxStartingHealth.Text);
                editedNPC.aggressiveness = Convert.ToInt32(TextBoxNewAggressiveness.Text);
                editedNPC.movement_timer = Convert.ToInt32(TextBoxNewMovementTimer.Text);
                editedNPC.movement_radius = Convert.ToInt32(TextBoxNewMovementRadius.Text);
                editedNPC.movement_type = Convert.ToInt32(TextBoxNewMovementType.Text);
                editedNPC.health_timer = Convert.ToInt32(TextBoxNewHealthTimer.Text);
                editedNPC.respawn_timer = Convert.ToInt32(TextBoxNewRespawnTimer.Text);
                editedNPC.attack = Convert.ToInt32(TextBoxNewAttack.Text);
                editedNPC.carry = Convert.ToInt32(TextBoxNewCarry.Text);
                editedNPC.is_food = CheckBoxNewIsFood.Checked;
                db.SubmitChanges();

                //reload the list to include the edited Changes
                npcTemplateList = (from np in db.NPC_Templates
                                   select np).ToList();

                ListBoxNPC.DataSource = npcTemplateList;
                ListBoxNPC.DataValueField = "id";
                ListBoxNPC.DataTextField = "Name";
                ListBoxNPC.DataBind();
                ListBoxNPC.SelectedValue = editedNPC.id.ToString();

            }
            else
                if(LabelTitle.Text=="NPC Editor")
            {
                //We are editing an existing NPC
                npc editedNPC = (from np in db.npcs
                                 where np.id == Convert.ToInt32(Session["SelectedNPC"].ToString())
                                 select np).SingleOrDefault();
                editedNPC.name = TextBoxNewName.Text;
                editedNPC.description = TextBoxNewDescription.Text;
                editedNPC.health = Convert.ToInt32(TextBoxStartingHealth.Text);
                editedNPC.aggressiveness = Convert.ToInt32(TextBoxNewAggressiveness.Text);
                editedNPC.movement_timer = Convert.ToInt32(TextBoxNewMovementTimer.Text);
                editedNPC.roomid = Convert.ToInt32(TextBoxRoomID.Text);
                editedNPC.movement_radius = Convert.ToInt32(TextBoxNewMovementRadius.Text);
                editedNPC.movement_type = Convert.ToInt32(TextBoxNewMovementType.Text);
                editedNPC.health_timer = Convert.ToInt32(TextBoxNewHealthTimer.Text);
                editedNPC.type = Convert.ToInt32(TextBoxType.Text);
                editedNPC.generic_name = TextBoxGTN.Text;
                editedNPC.respawn_timer = Convert.ToInt32(TextBoxNewRespawnTimer.Text);
                editedNPC.attack = Convert.ToInt32(TextBoxNewAttack.Text);
                editedNPC.carry = Convert.ToInt32(TextBoxNewCarry.Text);
                editedNPC.is_food = CheckBoxNewIsFood.Checked;
                editedNPC.id = Convert.ToInt32(Session["SelectedNPC"]);
                db.SubmitChanges();


                //reload the main list of NPCs as there could have been a name change
                npcList = (from np in db.npcs
                           select np).ToList();

                ListBoxNPC.DataSource = npcList;
                ListBoxNPC.DataValueField = "id";
                ListBoxNPC.DataTextField = "Name";
                ListBoxNPC.DataBind();
                ListBoxNPC.SelectedValue = editedNPC.id.ToString();

            }
        }

        protected void LinkButtonStartingInventory_Click(object sender, EventArgs e)
        {
            //find out if we are editing or adding
            if (Session["SelectedNPCTemplate"] == null)
            {
                //adding 
                if (Session["NewTemplate"] == null)
                {
                    //we havent saved the new template yet so cant add inventory  
                    //we should let them know

                }
                else
                {
                    //ok we have a new template so open the inventory panel
                    RepeaterInventory.DataSource = null;
                    PanelInventory.Visible = true;
                    Session["SelectedNPC"] = null;  //so we know that we are adding to a template and not an npc
                }
            }
            else
            {

                //we are editing
                DataClasses1DataContext db = new DataClasses1DataContext();
                var templateList = (from i in db.NPC_Initial_Inventories
                                    join it in db.item_templates
                                    on i.itemid equals it.id
                                    where i.NPCID == Convert.ToInt32(Session["SelectedNPCTemplate"])
                                    select it).ToList();

                PanelInventory.Visible = true;
                RepeaterInventory.DataSource = templateList;
                RepeaterInventory.DataBind();
            }
        }

        protected void LinkButtonEditTemplate_Click(object sender, EventArgs e)
        {
            Session["NewTemplate"] = null;
            Session["SelectedNPC"] = null;
            Session["SelectedNPCTemplate"] = null;
            DataClasses1DataContext db = new DataClasses1DataContext();
            npcTemplateList = (from np in db.NPC_Templates
                       select np).ToList();
            ListBoxNPC.DataSource = npcTemplateList;
            ListBoxNPC.DataValueField = "id";
            ListBoxNPC.DataTextField = "Name";
            ListBoxNPC.DataBind();
            PanelList.Visible = true;
            PanelNPC.Visible = false;
            LabelTitle.Text = "Edit Existing Template";
        }

        protected void LinkButtonStartingDialog_Click(object sender, EventArgs e)
        {
            //we are showing the starting dialog for a new or edited NPC Template
            //find out if we are editing or adding

            if (Session["SelectedNPCTemplate"] == null)
            {
                //adding 
                if (Session["NewTemplate"] == null)
                {
                    //we havent saved the new template yet so cant add Dialog  
                    //we should let them know

                }
                else
                {
                    //ok we have a new template so open the dialog panel
                    RepeaterDialog.DataSource = null;
                    PanelDialog.Visible = true;
                    Session["SelectedNPC"] = null;  //so we know that we are adding to a template and not an npc
                }
            }
            else
            {

                //we are editing
                DataClasses1DataContext db = new DataClasses1DataContext();
                NPCPhrases = (from p in db.NPCPhrases
                                    where p.npctype== Convert.ToInt32(Session["SelectedNPCTemplate"])
                                    select p).ToList();

                PanelDialog.Visible = true;
                RepeaterDialog.DataSource = NPCPhrases;
                RepeaterDialog.DataBind();
            }
        }

        protected void RepeaterDialog_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void LinkButtonExitDialog_Click(object sender, EventArgs e)
        {
            PanelDialog.Visible = false;
        }

        protected void LinkButtonAddDialog_Click(object sender, EventArgs e)
        {
            if (Session["SelectedNPCTemplate"] != null)
            {
                //we are editing a templates phrases
                if (!PanelNewDialogAdd.Visible)
                {
                    //show the items available to add
                    DropDownListSayWhen.Items.Add("random");
                    DropDownListSayWhen.Items.Add("attack");
                    DropDownListSayWhen.Items.Add("flee");
                    DropDownListSayWhen.Items.Add("contact");
                    DropDownListSayWhen.Items.Add("ending");
                    PanelNewDialogAdd.Visible = true;
                }
                else
                {
                    //add the selected item to the NPC_Phrases table
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    NPCPhrase insertInitial = new NPCPhrase
                    {
                        sayphrase = DropDownListSayWhen.SelectedValue,
                        npctype = Convert.ToInt32(Session["SelectedNPCTemplate"]),
                        response = TextBoxPhrase.Text
                    };

                    db.NPCPhrases.InsertOnSubmit(insertInitial);
                    db.SubmitChanges();
                    DropDownListAdd.Visible = false;
                    var templateList = (from i in db.NPCPhrases
                                        join it in db.item_templates
                                        on i.npctype equals it.id
                                        where i.npctype == Convert.ToInt32(Session["SelectedNPCTemplate"])
                                        select it).ToList();

                    PanelDialog.Visible = true;
                    RepeaterDialog.DataSource = templateList;
                    RepeaterDialog.DataBind();
                    TextBoxPhrase.Text = "";   
                    NPCPhrases = (from p in db.NPCPhrases
                              where p.npctype == Convert.ToInt32(Session["SelectedNPCTemplate"])
                              select p).ToList();

                PanelDialog.Visible = true;
                RepeaterDialog.DataSource = NPCPhrases;
                RepeaterDialog.DataBind();
                }
                

            }
        }

        protected void ListBoxNPC_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataClasses1DataContext db = new DataClasses1DataContext();
            if (LabelTitle.Text == "NPC Editor")
            {
                //we are editing an in game NPC
                npc selectedNpc = (from np in db.npcs
                                    where np.id == Convert.ToInt32(ListBoxNPC.SelectedValue)
                                    select np).SingleOrDefault();
                TextBoxNewName.Text = selectedNpc.name;
                TextBoxNewDescription.Text = selectedNpc.description;
                TextBoxStartingHealth.Text = selectedNpc.health.ToString();
                TextBoxNewAggressiveness.Text = selectedNpc.aggressiveness.ToString();
                TextBoxNewMovementTimer.Text = selectedNpc.movement_timer.ToString();
                TextBoxRoomID.Text = selectedNpc.roomid.ToString();
                TextBoxNewMovementRadius.Text = selectedNpc.movement_radius.ToString();
                TextBoxNewMovementType.Text = selectedNpc.movement_type.ToString();
                TextBoxNewHealthTimer.Text = selectedNpc.health_timer.ToString();
                TextBoxType.Text = selectedNpc.type.ToString();
                TextBoxGTN.Text = selectedNpc.generic_name;
                TextBoxNewRespawnTimer.Text = selectedNpc.respawn_timer.ToString();
                TextBoxNewAttack.Text = selectedNpc.attack.ToString();
                TextBoxNewCarry.Text = selectedNpc.carry.ToString();
                CheckBoxNewIsFood.Checked = (bool)selectedNpc.is_food;
                Session["SelectedNPC"] = ListBoxNPC.SelectedValue;
                ImageNPC.ImageUrl = "images/npcs/"+selectedNpc.name+".png";

            }
            else
            {
                //We are editing or Creating an NPC Template

                    NPC_Template selectedNpc = (from np in db.NPC_Templates
                                                where np.id == Convert.ToInt32(ListBoxNPC.SelectedValue)
                                                select np).SingleOrDefault();
                TextBoxNewName.Text = selectedNpc.name;
                TextBoxNewDescription.Text = selectedNpc.description;
                TextBoxStartingHealth.Text = selectedNpc.starting_health.ToString();
                TextBoxNewAggressiveness.Text = selectedNpc.aggressiveness.ToString();
                TextBoxNewMovementTimer.Text = selectedNpc.movement_timer.ToString();
                TextBoxNewMovementRadius.Text = selectedNpc.movement_radius.ToString();
                TextBoxNewMovementType.Text = selectedNpc.movement_type.ToString();
                TextBoxNewHealthTimer.Text = selectedNpc.health_timer.ToString();
                TextBoxNewRespawnTimer.Text = selectedNpc.respawn_timer.ToString();
                TextBoxNewAttack.Text = selectedNpc.attack.ToString();
                TextBoxNewCarry.Text = selectedNpc.carry.ToString();
                CheckBoxNewIsFood.Checked = (bool)selectedNpc.is_food;
                if (LabelTitle.Text == "Edit Existing Template")
                {
                    //we only want to set this if it we are editing
                    Session["SelectedNPCTemplate"] = ListBoxNPC.SelectedValue;
                }
                ImageNPC.ImageUrl = "images/npcs/" + selectedNpc.name + ".png";
            }
        }
    }
}
