<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="maps.aspx.cs" Inherits="WebClient.mapsedit" smartNavigation="true" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Editor</title>
    <link href="capricorn_styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script type="text/javascript">
            /*script makes the tile scrollbar keep its position*/
            var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
            function BeginRequestHandler(sender, args)
            {
                xPos = $get('ListBoxTiles').scrollLeft;
                yPos = $get('ListBoxTiles').scrollTop; 
            }
            function EndRequestHandler(sender, args)
            {
                $get('ListBoxTiles').scrollLeft = xPos;
                $get('ListBoxTiles').scrollTop = yPos;
            }
        </script>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:capricornislandConnectionString %>" 
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT * FROM [rooms] WHERE (([x] &gt;= @x3) AND ([x] &lt; @x) AND ([y] &gt;= @y3) AND ([y] &lt; @y)) ORDER BY [y], [x]">
        <SelectParameters>
            <asp:SessionParameter Name="x3" SessionField="gridMinX" Type="Int32" />
            <asp:SessionParameter Name="x" SessionField="gridMaxX" Type="Int32" />
            <asp:SessionParameter Name="y3" SessionField="gridMinY" Type="Int32" />
            <asp:SessionParameter Name="y" SessionField="gridMaxY" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

        <div class="god">
			<div class="tabs_god">
                <!--<div class="icon_contain">
                    <a href="#"><div class="icon_button"><div class="icon_load"></div></div></a>
                    <a href="#"><div class="icon_button"><div class="icon_new"></div></div></a>
                    <a href="#"><div class="icon_button"><div class="icon_save"></div></div></a>
                    <a href="#"><div class="icon_button"><div class="icon_delete"></div></div></a>
                    <div class="icon_project_title" style="padding-left: 5px;">capricorn_game</div>	
                </div>-->
			<div class="icon_project_title">projeckt kaprikorn</div>
                <div class="tabs_contain">
                    <a href="maps.aspx"><div class="tabs_selected">Maps</div></a>
                    <a href="items.aspx"><div class="tabs">Items</div></a>
                    <a href="npcs.aspx"><div class="tabs">NPCs</div></a>
                    <a href="characters.aspx"><div class="tabs">Characters</div></a>
                    <a href="actions.aspx"><div class="tabs">Actions</div></a>
                    <a href="game.aspx"><div class="tabs">Game</div></a>
                </div>
                <div class="row">
                    <div class="outliner">


                        <div class="left_panel_area">

                            <div class="outliner_tight">

                                <asp:UpdatePanel ID="UpdatePanelLeftScroll" runat="server">
                                    <ContentTemplate>
                                        <div class="scrollbar" id="style-8">
                                            <div class="force-overflow">
                                                <div class="editor_title">
                                                    <div class="roundcorner"></div>
                                                    Map Editor
												</div>
                                                <div class="editor_area">
                                                    <div class="editor_content">
                                                        <div class="editor_row">
                                                            <div class="rule"></div>
                                                        </div>
                                                        <asp:Literal ID="CellIdLit" runat="server" Text="Id 0"></asp:Literal><span class="pipe">|</span><asp:Literal ID="PositionLit" runat="server" Text="Position X:0 Y:0"></asp:Literal><br />
                                                        <div class="editor_row">
                                                            <div class="rule"></div>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanelLeft" runat="server">
                                                            <ContentTemplate>
                                                                <fieldset class="group">
                                                                    <legend>
                                                                        <label for="npc_select">Tiles</label></legend>

                                                                    <asp:ListBox ID="ListBoxTiles" CssClass="editor_dropdown" runat="server" Rows="8" OnSelectedIndexChanged="ListBoxTiles_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                                                    <div class="editor_row"></div>
                                                                    <div id="tileImage" runat="server" class="item_contain_center floor_brick01a">
                                                                        <div class="table_grid_cell128">
                                                                        </div>
                                                                    </div>

                                                                    <div class="item_contain_center">
                                                                        Tile:&nbsp;<div class="item_title"><asp:Literal ID="tileName" runat="server" Text="Tile Name"></asp:Literal>
                                                                        </div>
                                                                    </div>

                                                                    <div class="item_contain_center">
                                                                        Terrain:&nbsp;<span class="item_title"><asp:Literal ID="LiteralTerrain" runat="server" Text="Normal"></asp:Literal></span>
                                                                    </div>

                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <div class="editor_row"></div>
                                                         <asp:Panel ID="DoorFeatures" runat="server">
                                                          <fieldset class="group" visible="false">
                                                            <legend>Doorways</legend>
                                                            <ul class="checkbox">
                                                                <li><asp:CheckBox ID="CheckBoxDoor" runat="server" Text=" YES" Checked="false" AutoPostBack="True" OnCheckedChanged="CheckBoxDoor_CheckedChanged" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxDoorLockable" runat="server" Text=" Lock" Checked="false" AutoPostBack="True" OnCheckedChanged="CheckBoxDoorLockable_CheckedChanged" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxDoorTrapped" runat="server" Text=" Trap" Checked="false" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxDoorInvisible" runat="server" Text=" Secret" Checked="false" /></li>                                                  
                                                            </ul>
                                                             &nbsp;
                                                             <div id="ruleKeyCode" runat="server" class="rule"></div>
                                                             <asp:Label ID="LabelKeyCode" runat="server" Text="Key Code" ></asp:Label>
                                                             
                                                        <asp:TextBox ID="TextBoxKeyCode" runat="server" CssClass="editor_field" AutoComplete="off" ></asp:TextBox>
                                                        </fieldset>
                                                        </asp:Panel>
														<div class="editor_row"></div>
														
                                                        <asp:Panel ID="PanelDoor" runat="server">
															<label for="text">Name</label><br />
                                                            <asp:TextBox ID="NameTextBox" runat="server" CssClass="editor_field" AutoComplete="off"></asp:TextBox>
                                                            <div class="editor_row"></div>


                                                            <label for="textarea">Short Description</label><br />
                                                            <asp:TextBox ID="ShortTextBox" runat="server" CssClass="editor_textarea_short" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                            <div class="editor_row"></div>


                                                            <label for="textarea">Long Description</label><br />
                                                            <asp:TextBox ID="LongTextBox" runat="server" CssClass="editor_textarea" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                        </asp:Panel>

                                                        <div class="editor_row"></div>
                                                        <asp:Panel ID="PanelOtherFeatures" runat="server">
                                                        <fieldset class="group">
                                                            <legend>Locale</legend>
                                                            <ul class="radio">
                                                                <li>
                                                                    <asp:RadioButton ID="RadioButtonIndoor" runat="server" GroupName="locale" Text=" Indoor" /></li>
                                                                <li>
                                                                    <asp:RadioButton ID="RadioButtonOutdoor" runat="server" GroupName="locale" Text=" Outdoor" /></li>
                                                            </ul>
                                                        </fieldset>
                                                        <div class="editor_row"></div>

                                                        <fieldset class="group">
                                                            <legend>Allowed Events</legend>
                                                            <ul class="checkbox">
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxWandering" runat="server" Text=" Wandering" Checked="true" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxCombat" runat="server" Text=" Combat" Checked="true" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxSleep" runat="server" Text=" Sleep" Checked="true" /></li>
                                                                <li>
                                                                    <asp:CheckBox ID="CheckBoxSound" runat="server" Text=" Sound" Checked="true" /></li>
                                                            </ul>
                                                        </fieldset>
                                                        <div class="editor_row"></div>


                                                        <fieldset class="group">
                                                            <legend>
                                                                <label for="item_select">NPCs</label></legend>
                                                            <asp:Repeater ID="RepeaterNPC" runat="server" OnItemCommand="RepeaterNPC_ItemCommand" OnItemDataBound="RepeaterNPC_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="Panel2" runat="server" CssClass="item_contain_list">
                                                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" CssClass="item_corner_ul" CommandName="Delete" CommandArgument='<%# Eval("id") %>'><div class='icon_delete'></div></asp:LinkButton>
                                                                        <asp:LinkButton ID="LBSelect" runat="server" CommandName="Select" CommandArgument='<%# Eval("id") %>'><div class='item_64'>
                                                                            <asp:Image ID="LBNPCImage" runat="server" ImageUrl='<%# "images/npcs/"+Eval("generic_name")+".png" %> '/></div></asp:LinkButton>
                                                                        <div class='item_info'><span class='item_title'><%# Eval("name") %></span><br />
                                                                            <asp:Literal ID="LiteralSideMovement" runat="server" ></asp:Literal></div>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <div runat="server" id="divNPC2" class="editor_row">
                                                                        <div runat="server" id="divNPC" class="rule"></div>
                                                                    </div>
                                                                    <asp:LinkButton ID="openModal01bb" runat="server" CommandName="Add">
                                                                <div class="btn02">Add NPC</div>
                                                                    </asp:LinkButton>
                                                                    </fieldset>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        
                                                        <div class="editor_row"></div>

                                                        <fieldset class="group">
                                                            <legend>
                                                                <label for="item_select">Items</label></legend>
                                                                <asp:Repeater ID="RepeaterItems" runat="server" OnItemCommand="RepeaterItems_ItemCommand" OnItemDataBound="RepeaterItems_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="Panel2" runat="server" CssClass="item_contain_list">
                                                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" CssClass="item_corner_ul" CommandArgument='<%# Eval("id") %>' CommandName="Delete"><div class='icon_delete'></div></asp:LinkButton>
                                                                        <asp:LinkButton ID="LBSelect" runat="server" CommandName="Select" CommandArgument='<%# Eval("id") %>'><div class='item_64'><img src='images/items/crate.png' /></div></asp:LinkButton>
                                                                        <div class='item_info'><span class='item_title'><%# Eval("item_name") %></span><br />MAX:<%# Eval("limit") %><br />REGEN:<%# Eval("spawn_timer") %>Seconds</div>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                                  <FooterTemplate>
                                                                    <div id="divITEMS2" runat="server" class="editor_row">
                                                                        <div id="divITEMS" runat="server" class="rule"></div>
                                                                    </div>
                                                                    <asp:LinkButton ID="openModal02bb" runat="server" CommandName="Add">
                                                                <div class="btn02">Add Item</div>
                                                                    </asp:LinkButton>
                                                                    </fieldset>
                                                                </FooterTemplate>

                                                            </asp:Repeater>
                                                            </asp:Panel>
                                                            <div class="editor_row">
                                                            </div>
                                                        </fieldset>
                                                        <div class="editor_row">
                                                        </div>

                                                        
                                                            
                                                        <asp:LinkButton ID="LinkButtonOK" runat="server" OnClick="LinkButtonOK_Click" ViewStateMode="Enabled"><div class="btn">Save</div>
                                                        </asp:LinkButton>
                                                        

                                                    </div>
                                                </div>
                                            </div> <!-- End Overflow -->
                                        </div> <!-- End Scroll Bar -->
									</div> <!-- End Outliner-Tight -->
								</div> <!-- End Left Panel Area -->

											
											
											
                                        <!--NPC MODAL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!-->

                                        <div id="openModal" runat="server" class="modalDialog" visible="false">
                                            <div>
                                                <asp:LinkButton ID="LinkButtonCloseNPC" runat="server" CssClass="close" OnClick="LinkButtonCloseNPC_Click">X</asp:LinkButton>
                                                <div class="left_panel_area">
                                                    <div class="outliner_tight">
                                                        <div class="editor_title">
                                                            <div class="roundcorner"></div>
                                                            <asp:Literal ID="LiteralNPCTitle" runat="server" Text="Add NPC"></asp:Literal>
                                                        </div>
                                                        <div class="editor_area">
                                                            <div class="editor_content">
                                                                <fieldset class="group">
                                                                    <legend>
                                                                        <label for="npc_select">NPCs</label></legend>
                                                                    
                                                                    <asp:ListBox ID="ListBoxNPCSelect" CssClass="editor_dropdown" runat="server" Rows="8" AutoPostBack="True" OnSelectedIndexChanged="ListBoxNPCSelect_SelectedIndexChanged"></asp:ListBox>

                                                                    <div class="editor_row"></div>

                                                                    <div class="item_contain_left">
                                                                        <a href="monsters.aspx" class="item_corner_ur">
                                                                            <div class="icon_edit"></div>
                                                                        </a>
                                                                        <div class="item_128">
                                                                            <asp:Image ID="ImageNPCModal" runat="server" ImageUrl="images/npcs/monster.png" />
                                                                        </div>
                                                                        <div class="item_info">
                                                                            ID: <span class="item_title">
                                                                                <asp:Literal ID="LiteralNPCId" runat="server"></asp:Literal></span>
                                                                            <div class="editor_row"><span class="rule"></span></div>
                                                                            AV: <span class="item_title">
                                                                                <asp:Literal ID="LiteralAV" runat="server"></asp:Literal></span>
                                                                            <div class="editor_row"><span class="rule"></span></div>
                                                                            Attack: <span class="item_title">
                                                                                <asp:Literal ID="LiteralAttack" runat="server"></asp:Literal></span><br />
                                                                            Aggro: <span class="item_title">
                                                                                <asp:Literal ID="LiteralAggro" runat="server"></asp:Literal></span><br />
                                                                            Carry: <span class="item_title">
                                                                                <asp:Literal ID="LiteralCarry" runat="server"></asp:Literal></span><br />
                                                                            <div class="editor_row"><span class="rule"></span></div>
                                                                            Dialog: <span class="item_title">
                                                                                <asp:Literal ID="LiteralDialog" runat="server"></asp:Literal></span><br />
                                                                        </div>
                                                                    </div>

                                                                    <div class="item_contain_left">
                                                                        <div class="item_title">
                                                                            <asp:Literal ID="LiteralName" runat="server"></asp:Literal>
                                                                        </div>
                                                                    </div>
                                                                    <div class="item_contain_left">
                                                                        <asp:Literal ID="LiteralDesc" runat="server"></asp:Literal>
                                                                    </div>
                                                                    <div class="item_contain_left">
                                                                    <asp:Label ID="NPCModalName" runat="server" Text="Label" class="item_title" Visible="false"></asp:Label>
                                                                        </div>
                                                                    <div class="item_info">
                                                                    <asp:Label ID="NPCModalDesc" runat="server" Text="Label" Visible="false"></asp:Label>
                                                                        </div>
                                                                </fieldset>

                                                                <div class="editor_row"></div>

                                                                <fieldset class="group" runat="server" id="fsQuantity">
                                                                    <legend>Quantity</legend>
                                                                    <asp:TextBox ID="item_qty_npc" runat="server" class="editor_dropdown" Text="1" AutoComplete="off" Width="252px"></asp:TextBox>
                                                                    </ul>
                                                                </fieldset>

                                                                <div class="editor_row"></div>

                                                                <fieldset class="group">
                                                                    <legend>Movement</legend>
                                                                    <ul class="radio">
                                                                        <li>
                                                                            <asp:RadioButton ID="RadioButtonFixed" runat="server" GroupName="movement" value="fixed" Checked="true" Text="Fixed" /></li>
                                                                        <li>
                                                                            <asp:RadioButton ID="RadioButtonPatrol" runat="server" GroupName="movement" value="patrol" Text="Patrol" /></li>
                                                                        <li>
                                                                            <asp:RadioButton ID="RadioButtonWander" runat="server" GroupName="movement" value="wander" Text="Wander" /></li>
                                                                    </ul>
                                                                </fieldset>



                                                                <div class="editor_row">
                                                                    <div class="rule"></div>
                                                                </div>



                                                                <div class="btn">
                                                                    <asp:LinkButton ID="LinkButtonNPCCancel" runat="server" OnClick="LinkButtonNPCCancel_Click">Cancel</asp:LinkButton>
                                                                </div>
                                                                <div class="btn">
                                                                    <asp:LinkButton ID="LinkButtonNPCAdd" runat="server" OnClick="LinkButtonNPCAdd_Click" Text="Add"></asp:LinkButton>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <!--ITEM MODAL-->

                                        <div runat="server" id="openModal02" class="modalDialog" visible="false">
                                            <div>
                                                <asp:LinkButton ID="LinkButtonItemClose" runat="server" CssClass="close" OnClick="LinkButtonItemClose_Click">X</asp:LinkButton>
                                                <div class="left_panel_area">
                                                    <div class="outliner_tight">
                                                        <div class="editor_title">
                                                            <div class="roundcorner"></div>
                                                            <asp:Literal ID="LiteralAddItemTitle" runat="server" Text="Add Item"></asp:Literal>
                                                        </div>
                                                        <div class="editor_area">
                                                            <div class="editor_content">
                                                                <fieldset class="group">
                                                                    <legend>
                                                                        <label for="npc_select">Items</label></legend>
                                                                    <asp:ListBox ID="ItemSelectModal" runat="server" CssClass="editor_dropdown" Rows="8" AutoPostBack="True" OnSelectedIndexChanged="ItemSelectModal_SelectedIndexChanged"></asp:ListBox>
                                                                    
                                                                    <div class="editor_row"></div>

                                                                    <div class="item_contain_left">
                                                                        <a href="#" class="item_corner_ur">
                                                                            <div class="icon_edit"></div>
                                                                        </a>
                                                                        <div class="item_128">
                                                                            <asp:Image ID="ImageSelectedItem" runat="server" />
                                                                        </div>
                                                                        <div class="item_info">
                                                                            ID: <span class="item_title"><asp:Literal ID="LiteralID" runat="server"></asp:Literal></span><br />
                                                                            Type: <span class="item_title"><asp:Literal ID="LiteralType" runat="server"></asp:Literal></span><br />
                                                                            Weight/Slots: <span class="item_title"><asp:Literal ID="LiteralWeight" runat="server"></asp:Literal></span><br />
                                                                            Stackable: <span class="item_title"><asp:Literal ID="LiteralStackable" runat="server"></asp:Literal></span><br />
                                                                            Handed: <span class="item_title"><asp:Literal ID="LiteralCanCarry" runat="server"></asp:Literal></span><br />
                                                                            Damage: <span class="item_title"><asp:Literal ID="LiteralDamage" runat="server"></asp:Literal></span><br />
                                                                            Spcl: <span class="item_title"><asp:Literal ID="LiteralSpecial" runat="server"></asp:Literal></span>
                                                                        </div>
                                                                    </div>

                                                                    <div class="item_contain_left">
                                                                        <div class="item_title">
                                                                            <asp:Literal ID="LiteralItemTitle" runat="server"></asp:Literal></div>
                                                                    </div>



                                                                    <div class="item_contain_left">
                                                                        <asp:Literal ID="LiteralDescription" runat="server"></asp:Literal>
                                                                    </div>

                                                                </fieldset>

                                                                

                                                                <fieldset class="group">
                                                                    <legend>
                                                                        <asp:Label ID="QuanitiyLabel" runat="server" Text="Max Quantity"></asp:Label>
                                                                    </legend>

                                                                    <asp:TextBox ID="ItemQty" runat="server" class="editor_dropdown" Width="250px" AutoComplete="off"></asp:TextBox>
                                                                    <div class="editor_row"></div>
                                                                    <asp:CheckBox ID="CheckBoxWorldLimit" class="checkbox" runat="server" text="World Limit"/>
                                                                </fieldset>
                                                                <div class="editor_row"></div>
                                                                 <fieldset class="group">
                                                                    <legend>
                                                                        <asp:Label ID="LabelCreation" runat="server" Text="Creation Interval"></asp:Label>
                                                                    </legend>

                                                                    <asp:TextBox ID="TextBoxCreationInt" runat="server" class="editor_dropdown" Width="250px" AutoComplete="off"></asp:TextBox>
                                                                </fieldset>

                                                                <div class="editor_row">
                                                                    <div class="rule"></div>
                                                                </div>
                                                                
                                                                <div class="btn">
                                                                    <asp:LinkButton ID="LinkButtonItemsCancel" runat="server" OnClick="LinkButtonItemsCancel_Click">Cancel</asp:LinkButton>
                                                                </div>
                                                                <div class="btn">
                                                                    <asp:LinkButton ID="LinkButtonItemsAdd" runat="server" OnClick="LinkButtonItemsAdd_Click" Text="Add"></asp:LinkButton>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <!--END ITEM MODAL-->
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="LinkButtonOK" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <!--END NPC MODAL-->


                                <!--Tool Bar-->

                                <div class="left_panel_area">

                                            <div class="outliner_tight_margin" style="margin-top: 5px;">
                                                <div class="editor_area_skinny">
                                                    <div class="editor_content_skinny">
                                                        <div class="icon_contain_skinny">
                                                            <asp:LinkButton ID="LinkButtonUndo" runat="server" OnClick="LinkButtonUndo_Click"><div class="icon_button"><div class="icon_undo"></div></div></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="outliner_tight_margin" style="margin-top: 5px;">
                                                <div class="editor_area_skinny">
                                                    <div class="editor_content_skinny">
                                                        <div class="icon_contain_skinny">
                                                            <asp:LinkButton ID="LinkButtonEdit" runat="server" OnClick="LinkButtonEdit_Click">
                                                                <div id="divEdit" class="icon_button_selected" runat="server">
                                                                    <div class="icon_edit">
                                                                    </div>
                                                                </div>
                                                            </asp:LinkButton>
                                                            <div class="editor_row"></div>
                                                            <asp:LinkButton ID="LinkButtonPaste" runat="server" OnClick="LinkButtonPaste_Click">
                                                                <div id="divPaste" runat="server" class="icon_button">
                                                                    <div class="icon_paste"></div>
                                                                </div>
                                                            </asp:LinkButton>
                                                            <div class="editor_row"></div>
                                                            <asp:LinkButton ID="LinkButtonDelete" runat="server" OnClick="LinkButtonDelete_Click">
                                                                <div id="divDelete" runat="server" class="icon_button">
                                                                    <div class="icon_delete"></div>
                                                                </div>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="outliner_tight_margin" style="margin-top: 5px;">
                                                <div class="editor_area_skinny">
                                                    <div class="editor_content_skinny">
                                                        <div class="icon_contain_skinny">
                                                            <asp:LinkButton ID="LinkButtonUp" runat="server" OnClick="LinkButtonUp_Click"><div class="icon_button"><div class="icon_arrow_u"></div></div></asp:LinkButton>
                                                            <div class="editor_row"></div>
                                                            <asp:LinkButton ID="LinkButtonDown" runat="server" OnClick="LinkButtonDown_Click"><div class="icon_button"><div class="icon_arrow_d"></div></div></asp:LinkButton>
                                                            <div class="editor_row"></div>
                                                            <asp:LinkButton ID="LinkButtonLeft" runat="server" OnClick="LinkButtonLeft_Click"><div class="icon_button"><div class="icon_arrow_l"></div></div></asp:LinkButton>
                                                            <div class="editor_row"></div>
                                                            <asp:LinkButton ID="LinkButtonRight" runat="server" OnClick="LinkButtonRight_Click"><div class="icon_button"><div class="icon_arrow_r"></div></div></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                                <div class="outliner_tight_margin" style="margin-top: 5px;">
                                                <div class="editor_area_skinny">
                                                    <div class="editor_content_skinny">
                                                        <div class="icon_contain_skinny">
                                                            <asp:LinkButton ID="LinkButtonEmbiggen" runat="server" OnClick="LinkButtonEmbiggen_Click"><div class="icon_button"><div class="icon_big"></div></div></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                </div>

                                <%# Eval("name") %>
                                <div class="right_panel_area">
                                    <div class="outliner_tight_margin">
                                        <div class="map_area">

                                            <asp:ListView ID="ListView1" runat="server" DataKeyNames="id"
                                                DataSourceID="SqlDataSource1" GroupItemCount="28"
                                                OnItemDataBound="ListView1_ItemDataBound" OnItemCommand="ListView1_ItemCommand">

                                                <EmptyDataTemplate>
                                                    <table runat="server" class="table_grid">
                                                        <tr>
                                                            <td>No data was returned.</td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>

                                                <EmptyItemTemplate>
                                                    <td runat="server" />
                                                </EmptyItemTemplate>

                                                <GroupTemplate>
                                                    <tr id="itemPlaceholderContainer" runat="server">
                                                        <td id="itemPlaceholder" runat="server"></td>
                                                    </tr>
                                                </GroupTemplate>

                                                <ItemTemplate>
                                                    <td runat="server" id="thecell">
                                                        <asp:LinkButton ID="thelink" runat="server" CssClass="tooltips">
                                                            <asp:PlaceHolder ID="SpanPlacer" runat="server"></asp:PlaceHolder>
                                                            <asp:Panel ID="Panel1" runat="server" >
                                                                <asp:HiddenField ID="HFId" Value='<%# Eval("id") %>' runat="server" />
                                                            </asp:Panel>
                                                        </asp:LinkButton>
                                                    </td>
                                                </ItemTemplate>

                                                <LayoutTemplate>
                                                    <table runat="server">
                                                        <tr runat="server">
                                                            <td runat="server">
                                                                <table id="groupPlaceholderContainer" runat="server" border="0" class="table_grid">
                                                                    <tr id="groupPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server">
                                                            <td runat="server" style=""></td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </div>
                                <%# Eval("item.name") %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
test