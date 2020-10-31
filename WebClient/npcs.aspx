<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="npcs.aspx.cs" Inherits="WebClient.npcs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>NPC Editor</title>
    <link href="capricorn_styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
	
	<form id="form1" runat="server">
		<div class="god">
			<div class="tabs_god">
				<div class="icon_project_title">projeckt kaprikorn</div>
                <div class="tabs_contain">
                    <a href="maps.aspx"><div class="tabs">Maps</div></a>
                    <a href="items.aspx"><div class="tabs">Items</div></a>
                    <a href="npcs.aspx"><div class="tabs_selected">NPCs</div></a>
                    <a href="characters.aspx"><div class="tabs">Characters</div></a>
                    <a href="actions.aspx"><div class="tabs">Actions</div></a>
                    <a href="game.aspx"><div class="tabs">Game</div></a>
                </div>
				
                <div class="row">
                <div class="outliner">
        
<!-- Sub Nav -->
					
					<div class="row">
                        <div class="tabs_contain rule_dark">
							<asp:LinkButton ID="LinkButtonCreate" class="tabs" runat="server" OnClick="LinkButtonCreate_Click">Create</asp:LinkButton>
							
							<asp:LinkButton ID="LinkButtonEditTemplate" class="tabs" runat="server" OnClick="LinkButtonEditTemplate_Click">Edit</asp:LinkButton>
							
							<asp:LinkButton ID="LinkButtonEditNPC" class="tabs" runat="server" OnClick="LinkButtonEditNPC_Click">Current</asp:LinkButton>
                        </div>
					</div>					
						
						<div class="editor_row_sm"></div>
						
						<div>
							<div class="left_panel_area">
								<div class="outliner_tight">
									
									<asp:Panel ID="PanelList" runat="server" Visible="false">

											<div class="scrollbar" id="style-8">
												<div class="force-overflow">
									
									
	                                <div class="editor_title">
                                        <div class="roundcorner"></div>
                                        <asp:Label ID="LabelTitle" runat="server" Text="NPC Editor"></asp:Label>
                                    </div>
													
													
													
									
                                        <div class="editor_area">
                                            <div class="editor_content">
											
												
												<fieldset class="group">
													<legend>
														<label for="npc_select">NPCs</label>
													</legend>
													
													<asp:ListBox ID="ListBoxNPC" CssClass="editor_dropdown" Rows="8" runat="server" OnSelectedIndexChanged="ListBoxNPC_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
												</fieldset>
												
                                                <div class="editor_row"></div>
													
                                                <fieldset class="group">
                                                    <legend>
                                                        <label for="item_select">Image</label>
                                                    </legend>
                                                    <div class="item_contain_list">
                                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" CssClass="item_corner_ul" CommandName="Edit" CommandArgument='<%# Eval("id") %>'><div class='icon_edit'></div></asp:LinkButton>
                                                        <div class="item_64">
                                                            <asp:Image ID="ImageNPC" runat="server" Height="16px" Width="16px"/>
                                                        </div>
                                                    </div>
                                                </fieldset>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewName" runat="server" Text="Name"></asp:Label>
                                                <asp:TextBox ID="TextBoxNewName" CssClass="editor_field" runat="server"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewDescription" runat="server" Text="Description"></asp:Label>
                                                <asp:TextBox ID="TextBoxNewDescription" CssClass="editor_field" runat="server"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelStartingHealth" runat="server" Text="Starting Health"></asp:Label>
                                                <asp:TextBox ID="TextBoxStartingHealth" CssClass="editor_field" runat="server"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewHealthTimer" runat="server" Text="Health Timer"></asp:Label>
                                                <asp:TextBox ID="TextBoxNewHealthTimer" CssClass="editor_field" runat="server"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewRespawnTimer" runat="server" Text="Respawn Timer"></asp:Label>
                                                <asp:TextBox ID="TextBoxNewRespawnTimer" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewMovementRadius" runat="server" Text="Movement Radius"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewMovementRadius" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelnNewMovementTimer" runat="server" Text="Movement Timer"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewMovementTimer" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewMovementType" runat="server" Text="Movement Type"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewMovementType" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewAttack" runat="server" Text="Attack"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewAttack" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewAggressiveness" runat="server" Text="Aggressiveness"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewAggressiveness" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>
												
                                                <asp:Label ID="LabelNewCarry" runat="server" Text="Carry"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxNewCarry" runat="server" CssClass="editor_field"></asp:TextBox>
												<div class="editor_row"></div>

                                                <asp:Panel ID="PanelNPC" runat="server">
                                                <asp:Label ID="LabelGenericTypeName" runat="server" Text="Generic Name:"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxGTN" CssClass="editor_field" runat="server"></asp:TextBox>
                                                <div class="editor_row"></div>

                                                <asp:Label ID="LabelRoomID" runat="server" Text="Location: (room id)"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxRoomID" runat="server" CssClass="editor_field"></asp:TextBox>
                                                <div class="editor_row"></div>

                                                <asp:Label ID="LabelType" runat="server" Text="Type:"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxType" runat="server" CssClass="editor_field"></asp:TextBox>
                                                <div class="editor_row"></div>
                                                </asp:Panel>

                                                
												<div class="checkbox">
                                                        <asp:CheckBox ID="CheckBoxNewIsFood" runat="server" Text="Is Food" />
												</div>
												
                                                <div class="editor_row"></div>


                                                <!--***** ADD DIALOG *****-->
                                                <fieldset class="group">
                                                    <legend>
                                                        <label for="item_select">Items</label>
                                                    </legend>
                                                    <asp:LinkButton class="btn02" ID="LinkButtonStartInventory" runat="server" OnClick="LinkButtonStartingInventory_Click">Add Item</asp:LinkButton>
                                                </fieldset>

                                                <div class="editor_row"></div>

                                                <!--***** ADD ITEM *****-->
                                                <fieldset class="group">
                                                    <legend>
                                                        <label for="item_select">Dialog</label>
                                                    </legend>
                                                    <asp:LinkButton class="btn02" ID="LinkButtonStartingDialog" runat="server" OnClick="LinkButtonStartingDialog_Click">Add Dialog</asp:LinkButton>
                                                </fieldset>
                                                <div class="editor_row"></div>
                                                <div class="editor_row"></div>
                                                <asp:LinkButton class="btn" ID="LinkButtonSaveNew" runat="server" OnClick="LinkButtonSaveNew_Click">Save</asp:LinkButton>
                                            </div>
                                        </div>
									</div> <!-- End Overflow -->
                                </div> <!-- End Scroll Bar -->
									
									<!-- DIALOG MODAL -->	
									<!--END DIALOG MODAL-->
										
									<!-- ITEM MODAL -->	
									<!--END ITEM MODAL-->

                                </asp:Panel>
									
                                </div> <!-- End OutlinerTight -->
                            </div> <!-- End Left Panel Area -->
							</div>

                    <%--Inventory Section--%>
                    
                        <div class="left_panel_area">
                            <asp:Panel ID="PanelInventory" Visible="false" runat="server">
                                <div class="outliner_tight_margin">
                                    <div class="editor_area">
                                        <div class="editor_content">

                                            <asp:Label ID="LabelInventory" runat="server" Text="Inventory:"></asp:Label>
                                            <br />
                                            <br />
                                            <asp:Repeater ID="RepeaterInventory" runat="server" OnItemCommand="RepeaterInventory_ItemCommand">
                                            <ItemTemplate>
                                                <asp:Panel ID="Panel2" runat="server" >
                                                    <div class="item_info">Item Name: <asp:Label ID="LabelName" runat="server" Text='<%# Eval("name") %>'></asp:Label></div>
                                                    <div class="item_info">Description: <asp:Label ID="LabelDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label></div>
                                                    <div class="item_info"><asp:Image ID="Image1" runat="server" ImageUrl='<%# "images/items/"+Eval("name")+".png" %>' /></div>
                                                    <asp:LinkButton ID="LBSelect" runat="server" CommandName="Delete" CommandArgument='<%# Eval("id") %>'> Delete </asp:LinkButton>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            </asp:Repeater>
                                            <br />
                                        </div>
                                        <asp:DropDownList ID="DropDownListAdd" CssClass="editor_dropdown" runat="server" Visible="false"></asp:DropDownList>
                                        <div class="btn">
                                            <asp:LinkButton ID="LinkButtonExitInventory" runat="server" OnClick="LinkButtonExitInventory_Click">Exit Inventory</asp:LinkButton>
                                        </div>
                                        <div class="btn">
                                            <asp:LinkButton ID="LinkButtonAddInventory" runat="server" OnClick="LinkButtonAddInventory_Click">Add Item</asp:LinkButton>
                                        </div>
                                        
                                    </div>
                                </div> 
                            </asp:Panel>
                        </div>


                          <div class="left_panel_area">
                            <asp:Panel ID="PanelDialog" Visible="false" runat="server">
                                <div class="outliner_tight_margin">
                                    <div class="editor_area">
                                        <div class="editor_content">
                                            <asp:Label ID="Label1" runat="server" Text="Dialog:"></asp:Label>
                                            <br />
                                            <br />
                                            <asp:Repeater ID="RepeaterDialog" runat="server" OnItemCommand="RepeaterDialog_ItemCommand">
                                            <ItemTemplate>
                                                <asp:Panel ID="PanelAddDialog" runat="server" >
                                                    <div class="item_info">Phrase: <asp:Label ID="LabelName" runat="server" Text='<%# Eval("response") %>'></asp:Label></div>
                                                    <div class="item_info">Say when trigger: <asp:Label ID="LabelDescription" runat="server" Text='<%# Eval("sayphrase") %>'></asp:Label></div>
                                                    <asp:LinkButton ID="LBSelect" runat="server" CommandName="Delete" CommandArgument='<%# Eval("id") %>'> Delete </asp:LinkButton>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            </asp:Repeater>
                                            <br />
                                        </div>
                                        <asp:Panel ID="PanelNewDialogAdd" runat="server" Visible="false">
                                        <asp:Label ID="LabelSayWhen" runat="server" Text="Say When:" ></asp:Label><asp:DropDownList ID="DropDownListSayWhen" CssClass="editor_dropdown" runat="server" ></asp:DropDownList>  
                                        <asp:Label ID="LabelPhrase" runat="server" Text="Phrase" ></asp:Label> <asp:TextBox ID="TextBoxPhrase" CssClass="editor_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>      
                                            </asp:Panel>    
                                            <asp:LinkButton ID="LinkButtonExitDialog" class="btn" runat="server" OnClick="LinkButtonExitDialog_Click">Exit Dialog</asp:LinkButton>
                                            <asp:LinkButton ID="LinkButtonAddDialog" class="btn" runat="server" OnClick="LinkButtonAddDialog_Click">Add Dialog</asp:LinkButton>
                                        
                                        
                                    </div>
                                </div> 
                            </asp:Panel>
                        </div>
						</div>
                    </div>
            </div>
        </div>
    </form>
</body>
</html>
