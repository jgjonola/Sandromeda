using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

namespace WebClient
{
    public partial class mapsedit : System.Web.UI.Page
    {
        string myid;
        int gridsize = 28;
        List<npc> npclist;
        List<ItemSpawner> it; 
        bool updatebricks = true;
        List<Tile_Visual_Lookup> tvl;
        room copyRoom;


        protected override PageStatePersister PageStatePersister
        {
            get
            {
                return new SessionPageStatePersister(Page);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            


            //LinkButtonUndo.Enabled = (Session["undelete"] == null) ? false : true;
            DataClasses1DataContext db = new DataClasses1DataContext();
            npclist = (from np in db.npcs
                       select np).ToList();

            //load the tile lookup table into the list
            tvl = (from tv in db.Tile_Visual_Lookups
                   select tv).ToList();
            if (it == null)
            {
                //if we don't have a master items list then get it
                it = (from someinv in db.ItemSpawners
                      select someinv).ToList();
            }

            var npcQuery = from n in npclist
                           where n.start_room_id == Convert.ToInt32(Convert.ToInt32(Session["selected"]))
                           select n;
            var myinv = from i in it
                        where i.room_id == Convert.ToInt32(Convert.ToInt32(Session["selected"]))
                        select i;

            RepeaterNPC.DataSource = npcQuery;
            RepeaterNPC.DataBind();

            RepeaterItems.DataSource = myinv;
            RepeaterItems.DataBind();

            myid = Request.UserHostAddress;
            MaintainScrollPositionOnPostBack = true;
            string path = Server.MapPath(@"images\tiles");

            //set the grid positioning
            SetGridPosition();

            if (!this.IsPostBack)
            {
                //get max x and y for map
                int maxx = (from x in db.rooms
                            orderby x.x descending
                            select x.x).First();
                int maxy = (from y in db.rooms
                            orderby y.y descending
                            select y.y).First();
                Session["maxx"] = maxx;
                Session["maxy"] = maxy;
                Session["currentx"] = 0;
                Session["currenty"] = 0;
                Session["gridMinX"] = 0;
                Session["gridMinY"] = 0;
                Session["gridMaxX"] = gridsize;
                Session["gridMaxY"] = gridsize;
                Session["action"] = "edit";
                Session["selected"] = "1";

                //load items into the tiles dropdown

                string[] TileFiles = Directory.EnumerateFiles(path, "*.*")
                    .Select(p => Path.GetFileName(p).Replace(".png", "")).ToArray();

                //string[] TileFiles = Directory.GetFiles(path);
                ListBoxTiles.DataSource = TileFiles;
                ListBoxTiles.DataBind();
                ListBoxTiles.SelectedIndex = 0;
                tileImage.Attributes["class"] = "item_contain_center " + ListBoxTiles.SelectedValue;
                tileName.Text = ListBoxTiles.SelectedValue;

                //since this is the first time (not a postback) populate the left side of the map editor with the selected room of room one
                PopulateLeftMapEditor(Session["selected"].ToString());
            }

            ListView1.GroupItemCount = (int)gridsize;
        }

        private void SetGridPosition()
        {
            Session["gridMinX"] = Convert.ToInt32(Session["currentx"]);
            if (Convert.ToInt32(Session["gridMinX"]) < 0)
                Session["gridMinX"] = 0;
            Session["gridMinY"] = Convert.ToInt32(Session["currenty"]);
            if (Convert.ToInt32(Session["gridMinY"]) < 0)
                Session["gridMinY"] = 0;
            Session["gridMaxX"] = Convert.ToInt32(Session["gridMinX"]) + gridsize;
            Session["gridMaxY"] = Convert.ToInt32(Session["gridMinY"]) + gridsize;
        }

        private void PopulateLeftMapEditor(string v)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var roomQuery = (from rm in db.rooms
                            where rm.id == Convert.ToInt32(v)
                            select rm).Single();
            CellIdLit.Text = String.Format("Id {0}", roomQuery.id);
            PositionLit.Text = String.Format("Position X:{0} Y:{1}", roomQuery.x, roomQuery.y);
            Session["lastDoor"] = roomQuery.doorVisualType;
            Session["lastTile"] = roomQuery.visualTypeS;
            if (updatebricks)
            {
                //if we are updating bricks turn on or off features
                if (roomQuery.visualTypeS != "none")
                {
                    tileName.Text = roomQuery.visualTypeS;
                    tileImage.Attributes["class"] = "item_contain_center " + roomQuery.visualTypeS;
                    tileImage.Visible = true;
                    PanelOtherFeatures.Visible = true;
                    PanelDoor.Visible = true;
                    //LinkButtonOK.Visible = false;
                    ListBoxTiles.SelectedValue = roomQuery.visualTypeS;
                }
                else
                {
                    tileImage.Attributes["class"] = "item_contain_center";
                    tileImage.Visible = false;
                    PanelOtherFeatures.Visible = false;
                    PanelDoor.Visible = false;
                    LinkButtonOK.Visible = false;
                    ListBoxTiles.SelectedIndex = -1;
                    tileName.Text = roomQuery.visualTypeS;
                }
            }

            //door and key codes stuff
                                  
            int? key = (from tv in tvl
                       where tv.tilename == roomQuery.doorVisualType
                        select tv.tileType).SingleOrDefault();
            CheckBoxDoor.Checked = (key > 0 && key < 6);  //set the door checkbox
            if(CheckBoxDoor.Checked)
            {
                tileImage.Attributes["class"] = "item_contain_center " + roomQuery.doorVisualType;
            }

            DoorFeatures.Visible = CheckBoxDoor.Checked;


            if (key==2 || key ==4 || key == 5)
            {
                TextBoxKeyCode.Visible = true;
                LabelKeyCode.Visible = true;
                ruleKeyCode.Visible = true;
                TextBoxKeyCode.Text = roomQuery.nkey.ToString();
                CheckBoxDoorLockable.Checked = true;
            }
            else
            {
                TextBoxKeyCode.Visible = false;
                LabelKeyCode.Visible = false;
                ruleKeyCode.Visible = false;
                TextBoxKeyCode.Text = "";
                CheckBoxDoorLockable.Checked = false;
            }
            CheckBoxDoorInvisible.Enabled = CheckBoxDoor.Checked;
            CheckBoxDoorLockable.Enabled = CheckBoxDoor.Checked;
            CheckBoxDoorTrapped.Enabled = CheckBoxDoor.Checked;
            if(roomQuery.trapdoor !=null)
                CheckBoxDoorTrapped.Checked = (bool)roomQuery.trapdoor;
            if (roomQuery.secretdoor != null)
                CheckBoxDoorInvisible.Checked = (bool)roomQuery.secretdoor;


            NameTextBox.Text = roomQuery.name;
            ShortTextBox.Text = roomQuery.description;
            LongTextBox.Text = roomQuery.longDescription;
            string terText = roomQuery.terrain.Replace("Open ", "");
            terText = terText.Replace("Closed ", "");
            LiteralTerrain.Text = terText;
            RadioButtonIndoor.Checked = (bool)roomQuery.indoor;
            RadioButtonOutdoor.Checked = !(bool)roomQuery.indoor;
            CheckBoxCombat.Checked = (bool)roomQuery.combatAllowed;
            CheckBoxSleep.Checked = (bool)roomQuery.sleepAllowed;
            CheckBoxSound.Checked = (bool)roomQuery.soundAllowed;
            CheckBoxWandering.Checked = (bool)roomQuery.wanderingAllowed;

            if(roomQuery.nexit==7 & roomQuery.visualTypeS!="none")
            {
                //hide features if its a wall...
                DoorFeatures.Visible = false;
                PanelOtherFeatures.Visible = false;
                PanelDoor.Visible = false;
                
            }
            else
            {
                if (tileName.Text != "none")
                {
                    //show stuff if it isn't a blank tile
                    PanelDoor.Visible = true;
                    DoorFeatures.Visible = true;
                    PanelOtherFeatures.Visible = true;
                    
                }
            }
            if (CheckBoxDoor.Checked)
            {
                //hide if its a door
                PanelDoor.Visible = false;
                PanelOtherFeatures.Visible = false;
            }

            if (tileName.Text == "none")
                DoorFeatures.Visible = false;


            npclist = (from np in db.npcs
                       select np).ToList();

            it = (from someinv in db.ItemSpawners
                  select someinv).ToList();
            var myinv = from i in it
                        where i.room_id == Convert.ToInt32(Convert.ToInt32(Session["selected"]))
                        select i;
            var npcQuery = from n in npclist
                           where n.start_room_id == Convert.ToInt32(Convert.ToInt32(Session["selected"]))
                           select n;
            RepeaterItems.DataSource = myinv;
            RepeaterNPC.DataSource = npcQuery;
            RepeaterNPC.DataBind();
            RepeaterItems.DataBind();
        }



        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Panel ip;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string visualTypeS = DataBinder.Eval(e.Item.DataItem, "visualTypeS").ToString();
                string doorVisualType = DataBinder.Eval(e.Item.DataItem, "doorVisualType").ToString();
                string nexit = DataBinder.Eval(e.Item.DataItem, "nexit").ToString();
                string name = DataBinder.Eval(e.Item.DataItem, "name").ToString();
                string description = DataBinder.Eval(e.Item.DataItem, "description").ToString();
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("thecell");
                ip = (Panel)e.Item.FindControl("Panel1");

                HiddenField HFid = (HiddenField)e.Item.FindControl("HFId");

                //hover span code
                PlaceHolder spanPlacer = (PlaceHolder)e.Item.FindControl("SpanPlacer");
                Label hoverSpan = new Label();
                LiteralControl lit = new LiteralControl(string.Format("Id {0} | Position X:{1} Y:{2}<br /><hr class='style-one' />", HFid.Value, DataBinder.Eval(e.Item.DataItem, "x").ToString(), DataBinder.Eval(e.Item.DataItem, "y").ToString()));
                Panel title = new Panel();
                title.CssClass = "item_title";
                title.Controls.Add(new LiteralControl(name.ToUpper()));
                hoverSpan.Controls.Add(lit);
                hoverSpan.Controls.Add(title);
                     hoverSpan.Controls.Add(new LiteralControl(description + "<hr class='style-one' />"));


                ////we have to find out if we have npcs here first
                DataClasses1DataContext db = new DataClasses1DataContext();

                if (npclist == null)
                {
                    npclist = (from np in db.npcs
                               select np).ToList();
                }
                var npcQuery = from n in npclist
                                where n.start_room_id == Convert.ToInt32(HFid.Value)
                                select n;


                if (npcQuery.Count() > 0)
                {
                    //we have npcs so add the fieldset section
                    hoverSpan.Controls.Add(new LiteralControl("<fieldset class='group'><legend><label for='item_select'>NPCs</label></legend>"));
                    string movement = "";
                    foreach (npc n in npcQuery)
                    {
                        switch(n.movement_type)
                        {
                            case 0:
                                movement = "Fixed";
                                break;
                            case 1:
                                movement = "Patrol";
                                break;
                            case 2:
                                movement = "Wander";
                                break;
                        }
                        hoverSpan.Controls.Add(new LiteralControl(string.Format("<p class='item_contain_list'><sub class='item_64'><img src = 'images/npcs/{0}.png' /> </sub>{1} <br/> {2}</p> ", n.generic_name,n.name,movement)));
                    }
                    hoverSpan.Controls.Add(new LiteralControl("</fieldset> <br/> "));
                }



                ////now do the same thing for items in the room
                if (it == null)
                {
                    //if we don't have a master items list, get it
                    it = (from someinv in db.ItemSpawners
                              select someinv).ToList();
                }

                var myinv = from i in it
                            where i.room_id == Convert.ToInt32(HFid.Value)
                            select i;

                if (myinv.Count() > 0)
                {
                    //we have items so add the fieldset section
                    hoverSpan.Controls.Add(new LiteralControl("<fieldset class='group'><legend><label for='item_select'>Items</label></legend>"));
                    foreach (var i in myinv)
                    {
                        hoverSpan.Controls.Add(new LiteralControl(string.Format("<p class='item_contain_list'><sub class='item_64'><img src = 'images/items/crate.png' /> </sub>{0} <br/> MAX:&nbsp{1}<br /> REGEN:&nbsp{2}</p> ", i.item_name,i.limit,i.spawn_timer)));
                    }
                    hoverSpan.Controls.Add(new LiteralControl("</fieldset> <br/> "));
                }

                //this code makes the tile look right
                //including the door
                spanPlacer.Controls.Add(hoverSpan);
                td.Attributes.Add("class", string.Format("tile_cell {0}",visualTypeS));

                if (ip != null && HFid.Value == Session["selected"].ToString())
                {
                    if (Session["action"].ToString() == "edit")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32 table_grid_cell_selected {0}", doorVisualType));
                    if (Session["action"].ToString() == "paste")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32_paste table_grid_cell_selected {0}", doorVisualType));
                    if (Session["action"].ToString() == "delete")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32_delete table_grid_cell_selected {0}", doorVisualType));
                }

                else
                {
                    if (Session["action"].ToString() == "edit")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32 {0}", doorVisualType));
                    if (Session["action"].ToString() == "paste")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32_paste {0}", doorVisualType));
                    if (Session["action"].ToString() == "delete")
                        ip.Attributes.Add("class", String.Format("table_grid_cell32_delete {0}", doorVisualType));
                }
            }

        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (Session["action"].ToString() == "edit")
            {
                HiddenField HFid = (HiddenField)e.Item.FindControl("HFId");
                Session["selected"] = HFid.Value.ToString();
                PopulateLeftMapEditor(Session["selected"].ToString());
                
            }
            if (Session["action"].ToString() == "paste")
            {
                //copy everything from the left editor window over to the selected cell and save it to the database
                //remember the room inventory as well
                HiddenField HFid = (HiddenField)e.Item.FindControl("HFId");
                Session["selected"] = HFid.Value.ToString();
                SaveTile();

            }
            if (Session["action"].ToString() == "delete")
            {
                //set the room to a wall and the name to NEW and description to inaccessable
                //delete the rooms inventory and NPCs
                HiddenField HFid = (HiddenField)e.Item.FindControl("HFId");
                Session["selected"] = HFid.Value.ToString();

                DataClasses1DataContext db = new DataClasses1DataContext();
                room deleteRoom = (from rm in db.rooms
                                 where rm.id == Convert.ToInt32(Session["selected"])
                                 select rm).Single();
                CopyRoom(deleteRoom);
                
                Session["undelete"] = copyRoom;

                deleteRoom.visualTypeS = "none";
                deleteRoom.name = "";
                deleteRoom.description = "";
                deleteRoom.longDescription = "";
                deleteRoom.terrain = "Impassable";
                deleteRoom.indoor = false;
                deleteRoom.combatAllowed = false;
                deleteRoom.sleepAllowed = false;
                deleteRoom.soundAllowed = false;
                deleteRoom.wanderingAllowed = false;
                deleteRoom.nexit = 7;
                deleteRoom.nkey = 0;
                deleteRoom.doorVisualType = "none";
                db.SubmitChanges();
                PopulateLeftMapEditor(Session["selected"].ToString());
            }
            
        }

        private void CopyRoom(room deleteRoom)
        {
            copyRoom = new room();
            copyRoom.id = deleteRoom.id;
            copyRoom.visualTypeS = deleteRoom.visualTypeS;
            copyRoom.name = deleteRoom.name;
            copyRoom.description = deleteRoom.description;
            copyRoom.longDescription = deleteRoom.longDescription;
            copyRoom.terrain = deleteRoom.terrain;
            copyRoom.indoor = deleteRoom.indoor;
            copyRoom.combatAllowed = deleteRoom.combatAllowed;
            copyRoom.sleepAllowed = deleteRoom.sleepAllowed;
            copyRoom.soundAllowed = deleteRoom.soundAllowed;
            copyRoom.wanderingAllowed = deleteRoom.wanderingAllowed;
            copyRoom.nexit = deleteRoom.nexit;
            copyRoom.nkey = deleteRoom.nkey;
            copyRoom.doorVisualType = deleteRoom.doorVisualType;


        }

        protected void LinkButtonDelete_Click(object sender, EventArgs e)
        {
            divEdit.Attributes["class"] = "icon_button";
            divDelete.Attributes["class"] = "icon_button_selected";
            divPaste.Attributes["class"] = "icon_button";
            Session["action"] = "delete";
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        protected void LinkButtonPaste_Click(object sender, EventArgs e)
        {
            divEdit.Attributes["class"] = "icon_button";
            divDelete.Attributes["class"] = "icon_button";
            divPaste.Attributes["class"] = "icon_button_selected";
            Session["action"] = "paste";
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        protected void LinkButtonEdit_Click(object sender, EventArgs e)
        {
            divEdit.Attributes["class"] = "icon_button_selected";
            divDelete.Attributes["class"] = "icon_button";
            divPaste.Attributes["class"] = "icon_button";
            Session["action"] = "edit";
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        protected void LinkButtonUp_Click(object sender, EventArgs e)
        {
            int y = Convert.ToInt32(Session["currenty"]);
            if (y>0)
                y--;
            Session["currenty"] = y;
            //set the grid positioning
            SetGridPosition();
        }

        protected void LinkButtonLeft_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(Session["currentx"]);
            if(x>0)
                x--;
            Session["currentx"] = x;
            //set the grid positioning
            SetGridPosition();
        }

        protected void LinkButtonDown_Click(object sender, EventArgs e)
        {
            int y = Convert.ToInt32(Session["currenty"]);
            int maxy = Convert.ToInt32(Session["maxy"]);
            if (y + gridsize < maxy + 1)
                y++;
            Session["currenty"] = y;
            //set the grid positioning
            SetGridPosition();
        }

        protected void LinkButtonRight_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(Session["currentx"]);
            int maxx = Convert.ToInt32(Session["maxx"]);
            if (x + gridsize < maxx + 1)
                x++;
            Session["currentx"] = x;
            SetGridPosition();
        }

        protected void ListBoxTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            tileImage.Attributes["class"] = "item_contain_center " + ListBoxTiles.SelectedValue;
            tileName.Text = ListBoxTiles.SelectedValue;
            tileImage.Visible = true;
            PanelOtherFeatures.Visible = true;
            PanelDoor.Visible = true;
            updatebricks = false;
            LinkButtonOK.Visible = true;
            PopulateLeftMapEditor(Session["selected"].ToString());
            
            LiteralTerrain.Text = (from tv in tvl
                                  where tv.tilename == tileName.Text
                                  select tv.terrain).Single();

            int? key = (from tv in tvl
                        where tv.tilename == tileName.Text
                        select tv.tileType).Single();
            if (key == 2 || key == 4 || key == 5)
            {
                TextBoxKeyCode.Visible = true;
                LabelKeyCode.Visible = true;
                ruleKeyCode.Visible = true;
            }
            else
            {
                TextBoxKeyCode.Visible = false;
                LabelKeyCode.Visible = false;
                ruleKeyCode.Visible = false;
            }
        }



        protected void openModal02bb_Click(object sender, EventArgs e)
        {

            PopModalItem();
        }

        protected void PopModalNPC()
        {
            HtmlGenericControl mod = (HtmlGenericControl)FindControl("openModal");
            mod.Visible = true;
            PopulateLeftMapEditor(Session["selected"].ToString());
        }
        protected void PopModalItem()
        {
            HtmlGenericControl mod = (HtmlGenericControl)FindControl("openModal02");
            mod.Visible = true;
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        public void NPCImageClicked(object sender, EventArgs e)
        {
            
            PopModalNPC();

        }

        protected void RepeaterNPC_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (e.CommandName == "Select")
            {
                HtmlControl fs = (HtmlControl)FindControl("fsQuantity");
                fs.Visible = false;
                LinkButtonNPCAdd.Text = "Save";
                LiteralNPCTitle.Text = "Edit NPC";
                npc selectedNPC = (from np in db.npcs
                                   where np.id == Convert.ToInt32(e.CommandArgument)
                                   select np).SingleOrDefault();

                LiteralNPCId.Text = e.CommandArgument.ToString();
                ImageNPCModal.ImageUrl = "images/npcs/" + selectedNPC.generic_name + ".png";
                ListBoxNPCSelect.Visible = false;
                NPCModalName.Text = selectedNPC.name;
                NPCModalName.Visible = true;
                NPCModalDesc.Text = selectedNPC.description;
                NPCModalDesc.Visible = true;
                LiteralAV.Text = selectedNPC.health.ToString();
                LiteralCarry.Text = selectedNPC.carry.ToString();
                LiteralAttack.Text = selectedNPC.attack.ToString();
                LiteralAggro.Text = selectedNPC.aggressiveness.ToString();
                LiteralDialog.Text = selectedNPC.has_dialog.ToString();
                switch (selectedNPC.movement_type)
                {
                    case 0:
                        RadioButtonFixed.Checked = true;
                        break;
                    case 1:
                        RadioButtonPatrol.Checked = true;
                        break;
                    case 2:
                        RadioButtonWander.Checked = true;
                        break;
                }
                PopModalNPC();
            }

            if(e.CommandName=="Delete")
            {
                npc selectedNPC = (from np in db.npcs
                                   where np.id == Convert.ToInt32(e.CommandArgument)
                                   select np).SingleOrDefault();
                db.npcs.DeleteOnSubmit(selectedNPC);
                db.SubmitChanges();
                RepeaterNPC.DataBind();
                PopulateLeftMapEditor(Session["selected"].ToString());

            }

            if(e.CommandName=="Add")
            {
                HtmlControl fs = (HtmlControl)FindControl("fsQuantity");
                fs.Visible = true;
                LiteralNPCTitle.Text = "Add NPC";
                LinkButtonNPCAdd.Text = "Add";
                ListBoxNPCSelect.DataSource = (from np in db.NPC_Templates
                                               select np).ToList();
                ListBoxNPCSelect.DataTextField = "name";
                ListBoxNPCSelect.DataValueField = "id";
                ListBoxNPCSelect.DataBind();
                ListBoxNPCSelect.SelectedValue = "1";
                ListBoxNPCSelect.Visible = true;
                NPCModalName.Visible = true;
                NPCModalDesc.Visible = true;
                SetNPCLBValues();
                ListBoxNPCSelect.DataBind();
                PopModalNPC();
            }
            
        }
        protected void SetNPCLBValues()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            NPC_Template newNPC = (from np in db.NPC_Templates
                          where np.id == Convert.ToInt32(ListBoxNPCSelect.SelectedValue)
                          select np).SingleOrDefault();
            ImageNPCModal.ImageUrl = "images/npcs/" + newNPC.generic_name + ".png";
            LiteralAV.Text = newNPC.starting_health.ToString();
            LiteralCarry.Text = newNPC.carry.ToString();
            LiteralAttack.Text = newNPC.attack.ToString();
            LiteralAggro.Text = newNPC.aggressiveness.ToString();
            LiteralDialog.Text = newNPC.has_dialog.ToString();
            NPCModalName.Text = newNPC.name;
            NPCModalDesc.Text = newNPC.description;
            RadioButtonFixed.Checked = true;
        }

        protected void RepeaterItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (e.CommandName == "Select")
            {
                
                ItemSpawner selectedItemSpawner = (from itsp in db.ItemSpawners
                                   where itsp.id == Convert.ToInt32(e.CommandArgument)
                                   select itsp).SingleOrDefault();
                LiteralID.Text = e.CommandArgument.ToString();
                LiteralAddItemTitle.Text = "Edit Item";
                ItemSelectModal.Visible = false;
                LiteralItemTitle.Text = selectedItemSpawner.item_name;
                ImageSelectedItem.ImageUrl = string.Format("images/items/{0}.png",selectedItemSpawner.image);
                ItemSelectModal.Visible = false;
                LiteralCarry.Text = selectedItemSpawner.item_can_be_held.ToString();
                LiteralType.Text = selectedItemSpawner.item_type_text;
                LiteralDescription.Text = selectedItemSpawner.item_description;
                LiteralWeight.Text = selectedItemSpawner.item_size.ToString();
                LiteralStackable.Text = selectedItemSpawner.item_stackable.ToString();
                LiteralDamage.Text = selectedItemSpawner.damage.ToString();
                LiteralSpecial.Text = selectedItemSpawner.special_text;
                LiteralCanCarry.Text = selectedItemSpawner.item_can_be_held.ToString();
                ItemQty.Text = selectedItemSpawner.limit.ToString();
                TextBoxCreationInt.Text = selectedItemSpawner.spawn_timer.ToString();
                CheckBoxWorldLimit.Checked = (bool)selectedItemSpawner.limit_world;
                LinkButtonItemsAdd.Text = "Save";
                PopModalItem();
            }

            if (e.CommandName == "Delete")
            {
                ItemSpawner selectedItemSpawner = (from itemS in db.ItemSpawners
                                   where itemS.id == Convert.ToInt32(e.CommandArgument)
                                   select itemS).SingleOrDefault();
                db.ItemSpawners.DeleteOnSubmit(selectedItemSpawner);
                db.SubmitChanges();
                RepeaterItems.DataBind();
                PopulateLeftMapEditor(Session["selected"].ToString());

            }

            if (e.CommandName == "Add")
            {

                ItemQty.Text = "";
                TextBoxCreationInt.Text = "";
                LiteralAddItemTitle.Text = "Add Item";
                ItemSelectModal.DataSource = (from itemS in db.item_templates
                                               select itemS).ToList();
                ItemSelectModal.DataTextField = "name";
                ItemSelectModal.DataValueField = "id";
                ItemSelectModal.DataBind();
                ItemSelectModal.SelectedValue = "1";
                ItemSelectModal.Visible = true;
                LinkButtonItemsAdd.Text = "Add";
                SetITEMLBValues();
                PopModalItem(); 
            }
            
        }

        private void SetITEMLBValues()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            item_template newitem = (from itm in db.item_templates
                                   where itm.id == Convert.ToInt32(ItemSelectModal.SelectedValue)
                                   select itm).SingleOrDefault();

            LiteralAV.Text = newitem.name;
            LiteralAttack.Text = newitem.size.ToString();
            LiteralStackable.Text = newitem.item_stackable.ToString();
            LiteralCanCarry.Text = newitem.can_be_held.ToString();
            LiteralDamage.Text = newitem.damage.ToString();
            LiteralSpecial.Text = newitem.special_text;
            LiteralItemTitle.Text = newitem.name;
            LiteralDescription.Text = newitem.description;
            LiteralType.Text = newitem.item_type_text;
            LiteralWeight.Text = newitem.size.ToString();
            ImageSelectedItem.ImageUrl = string.Format("images/items/{0}.png", newitem.image);

        }

        protected void LinkButtonItemClose_Click(object sender, EventArgs e)
        {
            CloseItemDialog();
        }

        protected void CloseItemDialog()
        {
            HtmlGenericControl mod = (HtmlGenericControl)FindControl("openModal02");
            mod.Visible = false;
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        protected void LinkButtonItemsCancel_Click(object sender, EventArgs e)
        {
            CloseItemDialog();
        }

        protected void LinkButtonItemsAdd_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            if (LinkButtonItemsAdd.Text == "Add")
            {
                //we are adding a new item
                item_template template = (from temp in db.item_templates
                                          where temp.id == Convert.ToInt32(ItemSelectModal.SelectedValue)
                                          select temp).SingleOrDefault();
                //creat a new itemspawner
                ItemSpawner newItemSpawner = new ItemSpawner
                {
                    item_name = template.name,
                    item_description = template.description,
                    item_can_be_held = template.can_be_held,
                    item_container = template.container,
                    limit = Convert.ToInt32(ItemQty.Text),
                    limit_world = CheckBoxWorldLimit.Checked,
                    item_size = template.size,
                    item_can_be_seen_when_held = (bool)template.can_be_seen_when_held,
                    item_type = (int)template.type,
                    item_key_code = (int)template.key_code,
                    spawn_timer = Convert.ToInt32(TextBoxCreationInt.Text),
                    room_id = Convert.ToInt32(Session["selected"]),
                    location_description = template.location_description,
                    image = template.image,
                    item_type_text = template.item_type_text,
                    item_stackable = template.item_stackable,
                    damage = template.damage,
                    special_text = template.special_text,
                    special = template.special
                 };
                db.ItemSpawners.InsertOnSubmit(newItemSpawner);
                db.SubmitChanges();

            }
            else
            {
                //we are editing an existing item
                ItemSpawner edititemspawner = (from it in db.ItemSpawners
                                              where it.id == Convert.ToInt32(LiteralID.Text)
                                              select it).SingleOrDefault();
                edititemspawner.limit = Convert.ToInt32(ItemQty.Text);
                edititemspawner.spawn_timer = Convert.ToInt32(TextBoxCreationInt.Text);
                edititemspawner.limit_world = CheckBoxWorldLimit.Checked;
                db.SubmitChanges();

            }
            RepeaterItems.DataBind();
            PopulateLeftMapEditor(Session["selected"].ToString());
            CloseItemDialog();
        }

        protected void LinkButtonNPCAdd_Click(object sender, EventArgs e)
        {

            DataClasses1DataContext db = new DataClasses1DataContext();
            int quantity;
            if (LinkButtonNPCAdd.Text == "Add")
            {
                //we are adding a new NPC
                NPC_Template newNPCTemplate = (from np in db.NPC_Templates
                                               where np.id == Convert.ToInt32(ListBoxNPCSelect.SelectedValue)
                                               select np).SingleOrDefault();
                int movement = 0;
                if (RadioButtonFixed.Checked)
                    movement = 0;
                if (RadioButtonPatrol.Checked)
                    movement = 1;
                if (RadioButtonWander.Checked)
                    movement = 2;
                
                if (item_qty_npc.Text == "")
                    item_qty_npc.Text = "1";
                try {
                    quantity = Convert.ToInt32(item_qty_npc.Text);
                }catch(Exception)
                {
                    item_qty_npc.Text = "1";
                    quantity = 1;
                }
                for (int i = 0; i < quantity; i++)
                {
                    npc newNPC = new npc
                    {
                        name = newNPCTemplate.name,
                        description = newNPCTemplate.description,
                        aggressiveness = newNPCTemplate.aggressiveness,
                        movement_timer = newNPCTemplate.movement_timer,
                        health_timer = newNPCTemplate.health_timer,
                        type = newNPCTemplate.id,
                        health = newNPCTemplate.starting_health,
                        roomid = Convert.ToInt32(Session["selected"].ToString()),
                        generic_name = newNPCTemplate.generic_name,
                        respawn_timer = newNPCTemplate.respawn_timer,
                        movement_radius = newNPCTemplate.movement_radius,
                        start_room_id = Convert.ToInt32(Session["selected"].ToString()),
                        attack = newNPCTemplate.attack,
                        carry = newNPCTemplate.carry,
                        has_dialog = newNPCTemplate.has_dialog,
                        is_food = newNPCTemplate.is_food,
                        movement_type = movement
                    };
                    db.npcs.InsertOnSubmit(newNPC);
                    db.SubmitChanges();
                }
            }
            else
            {
                //we are editing, not adding....  do something with that here
                npc editNPC = (from np in db.npcs
                               where np.id == Convert.ToInt32(LiteralNPCId.Text)
                               select np).SingleOrDefault();
                int move = 0;
                if (RadioButtonFixed.Checked)
                    move = 0;
                if (RadioButtonPatrol.Checked)
                    move = 1;
                if (RadioButtonWander.Checked)
                    move = 2;

                editNPC.movement_type = move;
                db.SubmitChanges();
            }
            CloseNPCDialog();
            RepeaterNPC.DataBind();
            PopulateLeftMapEditor(Session["selected"].ToString());

        }

        protected void LinkButtonNPCCancel_Click(object sender, EventArgs e)
        {
            CloseNPCDialog();
        }
        protected void CloseNPCDialog()
        {
            HtmlGenericControl mod = (HtmlGenericControl)FindControl("openModal");
            mod.Visible = false;
            PopulateLeftMapEditor(Session["selected"].ToString());
        }

        protected void LinkButtonCloseNPC_Click(object sender, EventArgs e)
        {
            CloseNPCDialog();
        }

        protected void ListBoxNPCSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetNPCLBValues();
        }

        protected void LinkButtonOK_Click(object sender, EventArgs e)
        {
            Session["undelete"] = null;
            if (Session["action"].ToString() != "delete")
            {
                //save this sexy mother...
                SaveTile();
            }
        }

        protected void SaveTile()
        {
            //save this sexy mother...
            DataClasses1DataContext db = new DataClasses1DataContext();
            room editRoom = (from rm in db.rooms
                             where rm.id == Convert.ToInt32(Session["selected"])
                             select rm).Single();
            Session["undelete"] = editRoom;
            editRoom.visualTypeS = tileName.Text;
            editRoom.name = NameTextBox.Text;
            editRoom.description = ShortTextBox.Text;
            editRoom.longDescription = LongTextBox.Text;
            editRoom.terrain = LiteralTerrain.Text;
            editRoom.indoor = RadioButtonIndoor.Checked;
            editRoom.combatAllowed = CheckBoxCombat.Checked;
            editRoom.sleepAllowed = CheckBoxSleep.Checked;
            editRoom.soundAllowed = CheckBoxSound.Checked;
            editRoom.wanderingAllowed = CheckBoxWandering.Checked;

            if(CheckBoxDoor.Checked)
            {
                //we have a door, so save the door code
                
                editRoom.nexit = (CheckBoxDoorLockable.Checked) ? 5 : 3;
                editRoom.doorVisualType = FindDoorType(editRoom.nexit,editRoom.x, editRoom.y);
                editRoom.terrain = (from tv in tvl
                                    where tv.tilename == editRoom.doorVisualType
                                    select tv.terrain).SingleOrDefault();
                editRoom.trapdoor = CheckBoxDoorTrapped.Checked;
                editRoom.secretdoor = CheckBoxDoorInvisible.Checked;
            }
            else
            {
                editRoom.nexit = (from tv in tvl
                                  where tv.tilename == tileName.Text
                                  select tv.tileType).SingleOrDefault();
                editRoom.terrain = (from tv in tvl
                                    where tv.tilename == tileName.Text
                                    select tv.terrain).SingleOrDefault();
                editRoom.doorVisualType = "none";
            }


            editRoom.nkey = (TextBoxKeyCode.Text == "") ? 0: Convert.ToInt32(TextBoxKeyCode.Text);
            db.SubmitChanges();
            PopulateLeftMapEditor(Session["selected"].ToString());
            
        }

        private string FindDoorType(int? nexit,int x, int y)
        {
            try {
                //find out the door type by finding whether the north is a wall or not, then return a door type
                string doorVisualType = "";
                DataClasses1DataContext db = new DataClasses1DataContext();
                var northroom = (from rm in db.rooms
                                 where rm.x == x && rm.y == y - 1
                                 select rm).SingleOrDefault();
                if (northroom.nexit < 7 || northroom.visualTypeS == "none")
                {
                    //we have a floor
                    //now we have to find out what the east room has as a visualtype to find out what door type we put down.
                    var eastroom = (from rm in db.rooms
                                    where rm.x == x - 1 && rm.y == y
                                    select rm).SingleOrDefault();

                    string wallType = eastroom.visualTypeS;
                    wallType = wallType.Replace("wall_", "");
                    wallType = wallType.Remove(wallType.Length - 6, 6);

                    doorVisualType = string.Format("door_{0}_0{1}_ns", wallType, nexit);

                }
                else
                {
                    //we have a wall
                    //easy, we now find the wall type to find out the door type.
                    string wallType = northroom.visualTypeS;
                    wallType = wallType.Replace("wall_", "");
                    wallType = wallType.Remove(wallType.Length - 6, 6);

                    doorVisualType = string.Format("door_{0}_0{1}_ew", wallType, nexit);

                }
                return doorVisualType;
            }
            catch(Exception)
            {
                return string.Format("door_metal_0{0}_ew",nexit);
            }
        }

        protected void CheckBoxDoor_CheckedChanged(object sender, EventArgs e)
        {
            PanelDoor.Visible = !CheckBoxDoor.Checked;
            CheckBoxDoorInvisible.Enabled = CheckBoxDoor.Checked;
            CheckBoxDoorLockable.Enabled = CheckBoxDoor.Checked;
            CheckBoxDoorTrapped.Enabled = CheckBoxDoor.Checked;
            PanelOtherFeatures.Visible = !CheckBoxDoor.Checked;
            if (CheckBoxDoor.Checked)
            {
                if (Session["lastDoor"].ToString() == "" || Session["lastDoor"].ToString() == "none")
                {
                    tileImage.Attributes["class"] = "item_contain_center " + "door_metal_01_ns";
                }
                else
                {                
                    tileImage.Attributes["class"] = "item_contain_center " + Session["lastDoor"].ToString();
                }
            }
            else
            {
                if (Session["lastTile"].ToString() == "" || Session["lastTile"].ToString() == "none")
                {
                    tileImage.Attributes["class"] = "item_contain_center " + ListBoxTiles.SelectedValue;
                    
                }
                else
                {
                    tileImage.Attributes["class"] = "item_contain_center " + Session["lastTile"].ToString();
                }

            }


        }

        protected void ItemSelectModal_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetITEMLBValues();
        }

        protected void CheckBoxDoorLockable_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxKeyCode.Visible = CheckBoxDoorLockable.Checked;
            LabelKeyCode.Visible = CheckBoxDoorLockable.Checked;
            ruleKeyCode.Visible = CheckBoxDoorLockable.Checked;
        }

        protected void RepeaterItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (RepeaterItems.Items.Count == 0)
                {
                    e.Item.FindControl("divITEMS").Visible = false;
                    e.Item.FindControl("divITEMS2").Visible = false;
                }
            }
              
        }

        protected void RepeaterNPC_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (RepeaterNPC.Items.Count == 0)
                {
                    e.Item.FindControl("divNPC").Visible = false;
                    e.Item.FindControl("divNPC2").Visible = false;
                }
            }
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal l = (Literal)e.Item.FindControl("LiteralSideMovement");
                switch(DataBinder.Eval(e.Item.DataItem, "movement_type").ToString())
                {
                    case "0":
                        l.Text = "Fixed";
                        break;
                    case "1":
                        l.Text = "Patrol";
                        break;
                    case "2":
                        l.Text = "Wander";
                        break;
                }
                 
            }
        }

        protected void LinkButtonUndo_Click(object sender, EventArgs e)
        {
            if (Session["undelete"] != null)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                room savedRoom = (room)Session["undelete"];

                room deleteRoom = (from rm in db.rooms
                                   where rm.id == savedRoom.id
                                   select rm).Single();

                Session["undelete"] = null;


                deleteRoom.visualTypeS = savedRoom.visualTypeS;
                deleteRoom.name = savedRoom.name;
                deleteRoom.description = savedRoom.description;
                deleteRoom.longDescription = savedRoom.longDescription;
                deleteRoom.terrain = savedRoom.terrain;
                deleteRoom.indoor = savedRoom.indoor;
                deleteRoom.combatAllowed = savedRoom.combatAllowed;
                deleteRoom.sleepAllowed = savedRoom.sleepAllowed;
                deleteRoom.soundAllowed = savedRoom.soundAllowed;
                deleteRoom.wanderingAllowed = savedRoom.wanderingAllowed;
                deleteRoom.nexit = savedRoom.nexit;
                deleteRoom.nkey = savedRoom.nkey;
                deleteRoom.doorVisualType = savedRoom.doorVisualType;
                db.SubmitChanges();
                PopulateLeftMapEditor(Session["selected"].ToString());
            }
        }

        protected void LinkButtonEmbiggen_Click(object sender, EventArgs e)
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
                    y = oldy + 1,
                    description = "",
                    name = "",
                    visualTypeS = "none",
                    longDescription = "",
                    terrain = "Impassable",
                    doorVisualType = "none",
                    trapdoor = false,
                    secretdoor = false,
                    wanderingAllowed = false,
                    sleepAllowed = false,
                    combatAllowed = false,
                    soundAllowed = false,
                    nexit = 7,
                    nkey = 0,
                    type = 1,
                    visualType = 0
                };
                db.rooms.InsertOnSubmit(newRoom);
            }
            for (int j = 0; j < oldx + 2; j++)
            {
                room newRoom2 = new room
                {
                    //set x and all
                    x = oldx + 1,
                    y = j,
                    description = "",
                    longDescription="",
                    name = "",
                    visualTypeS = "none",
                    terrain = "Impassable",
                    doorVisualType = "none",
                    trapdoor = false,
                    secretdoor = false,
                    wanderingAllowed = false,
                    sleepAllowed = false,
                    combatAllowed = false,
                    soundAllowed = false,
                    nexit = 7,
                    nkey = 0,
                    type = 1,
                    visualType = 0
                };
                db.rooms.InsertOnSubmit(newRoom2);
            }
            db.SubmitChanges();

        }
    }
}