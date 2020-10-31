<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="characters.aspx.cs" Inherits="WebClient.characters" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Character Editor</title>
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
                    <a href="npcs.aspx"><div class="tabs">NPCs</div></a>
                    <a href="characters.aspx"><div class="tabs_selected">Characters</div></a>
                    <a href="actions.aspx"><div class="tabs">Actions</div></a>
                    <a href="game.aspx"><div class="tabs">Game</div></a>
                </div>

                <div class="row">
                <div class="outliner">


                        <div class="left_panel_area">
                            <div class="outliner_tight">
                                <div class="editor_title">
                                    <div class="roundcorner"></div>
                                    Character Editor
                                </div>
                                <div class="editor_area">
                                    <div class="editor_content">


                                        <div class="editor_row">
                                            <div class="rule"></div>
                                        </div>


                                        <asp:Repeater ID="RepeaterClientList" runat="server" OnItemCommand="RepeaterClientList_ItemCommand">
                                            <ItemTemplate>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="item_contain_list">
                                                    <asp:LinkButton ID="LBSelect" runat="server" CommandName="Select" CommandArgument='<%# Eval("id") %>'> <%# Eval("id") %> - <%# Eval("name") %></asp:LinkButton>
                                                </asp:Panel>
                                            </ItemTemplate>

                                        </asp:Repeater>
                                        &nbsp;<div class="editor_row">
                                            <div class="rule"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="PanelClient" runat="server" Visible="false">
                            <div class="right_panel_area">
                                <div class="outliner_tight_margin">
                                    <div class="editor_area">
                                        <div class="editor_content">

                                            <asp:Label ID="LabelName" runat="server" Text="Name:"></asp:Label>
                                            <asp:TextBox ID="TextBoxName" CssClass="editor_field" runat="server"></asp:TextBox>
                                            <asp:Label ID="LabelDescription" runat="server" Text="Description:"></asp:Label>
                                            <asp:TextBox ID="TextBoxDescription" CssClass="editor_field" runat="server"></asp:TextBox>
                                            <asp:Label ID="LabelRoomID" runat="server" Text="Location: (room id)"></asp:Label>
                                            <asp:TextBox ID="TextBoxRoomID" runat="server" CssClass="editor_field"></asp:TextBox>
                                            <asp:Label ID="LabelHealth" runat="server" Text="Health:"></asp:Label>
                                            <asp:TextBox ID="TextBoxHeath" CssClass="editor_field" runat="server"></asp:TextBox>
                                            <asp:Label ID="LabelUsername" runat="server" Text="Username:"></asp:Label>
                                            <asp:TextBox ID="TextBoxUsername" runat="server" CssClass="editor_field"></asp:TextBox>
                                            <asp:Label ID="LabelPassword" runat="server" Text="Password:"></asp:Label>
                                            <asp:TextBox ID="TextBoxPassword" CssClass="editor_field" runat="server"></asp:TextBox>
                                            <asp:Label ID="LabelPhone" runat="server" Text="Phone:"></asp:Label>
                                            <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="editor_field"></asp:TextBox>
                                            <asp:Label ID="LabelLastConnected" runat="server" Text="Last Connected:"></asp:Label><br />
                                            <asp:CheckBox ID="CheckBoxAdmin" runat="server" CssClass="checkbox" Text="Administrator" />
                                            <br />

                                            <div class="btn">
                                                <asp:LinkButton ID="LinkButtonCancel" runat="server" OnClick="LinkButtonCancel_Click">Cancel</asp:LinkButton>
                                            </div>
                                            <div class="btn">
                                                <asp:LinkButton ID="LinkButtonSave" runat="server" OnClick="LinkButtonSave_Click">Save</asp:LinkButton>
                                            </div>
                                            <div class="btn">
                                                <asp:LinkButton ID="LinkButtonShowInventory" runat="server" OnClick="LinkButtonShowInventory_Click">Load Inventory</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="right_panel_area">
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
                                                    <div class="item_info"><asp:Image ID="Image1" runat="server" ImageUrl='<%# "images/items/"+Eval("generic_name")+".png" %>' /></div>
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
                    </div>
                </div>
                </div>
            </div>
    </form>
</body>
</html>
