<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemTypeEditor.aspx.cs" Inherits="WebClient.ObjectEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 295px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        objects:<table style="width: 100%;">
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1" valign="top">
                    <asp:ListBox ID="ListBoxItems" runat="server" AutoPostBack="True" 
                        DataSourceID="SqlDataSource1" DataTextField="name" DataValueField="id" 
                        Rows="35" Width="194px"></asp:ListBox>
                    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                        Text="Add a new Item" />
                </td>
                <td valign="top">
                    <asp:FormView ID="FormViewItem" runat="server" DataKeyNames="id" 
                        DataSourceID="SqlDataSourceItem" DefaultMode="Edit" 
                        onitemdeleted="FormViewItem_ItemDeleted" 
                        oniteminserted="FormViewItem_ItemInserted" 
                        onitemupdated="FormViewItem_ItemUpdated">
                        <EditItemTemplate>
                            id:
                            <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                            <br />
                            name:
                            <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                            <br />
                            description:<br />
                            <asp:TextBox ID="descriptionTextBox" runat="server" Height="60px" 
                                Text='<%# Bind("description") %>' TextMode="MultiLine" Width="220px" />
                            <br />
                            is_random:
                            <asp:CheckBox ID="is_randomCheckBox" runat="server" 
                                Checked='<%# Bind("is_random") %>' />
                            <br />
                            can_be_held:
                            <asp:CheckBox ID="can_be_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_held") %>' />
                            <br />
                            container:
                            <asp:CheckBox ID="containerCheckBox" runat="server" 
                                Checked='<%# Bind("container") %>' />
                            <br />
                            room_limit:
                            <asp:TextBox ID="room_limitTextBox" runat="server" 
                                Text='<%# Bind("room_limit") %>' />
                            <br />
                            size:
                            <asp:TextBox ID="sizeTextBox" runat="server" Text='<%# Bind("size") %>' />
                            <br />
                            can_be_seen_when_held:
                            <asp:CheckBox ID="can_be_seen_when_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_seen_when_held") %>' />
                            <br />
                            type:
                            <asp:TextBox ID="typeTextBox" runat="server" Text='<%# Bind("type") %>' />
                            <br />
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                                CommandName="Update" Text="Update" />
                            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                                CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            name:
                            <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                            <br />
                            description:<br />
                            <br />
&nbsp;<asp:TextBox ID="descriptionTextBox" runat="server" Height="89px" Text='<%# Bind("description") %>' 
                                Width="191px" />
                            <br />
                            is_random:
                            <asp:CheckBox ID="is_randomCheckBox" runat="server" 
                                Checked='<%# Bind("is_random") %>' />
                            <br />
                            can_be_held:
                            <asp:CheckBox ID="can_be_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_held") %>' />
                            <br />
                            container:
                            <asp:CheckBox ID="containerCheckBox" runat="server" 
                                Checked='<%# Bind("container") %>' />
                            <br />
                            room_limit:
                            <asp:TextBox ID="room_limitTextBox" runat="server" 
                                Text='<%# Bind("room_limit") %>' />
                            <br />
                            size:
                            <asp:TextBox ID="sizeTextBox" runat="server" Text='<%# Bind("size") %>' />
                            <br />
                            can_be_seen_when_held:
                            <asp:CheckBox ID="can_be_seen_when_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_seen_when_held") %>' />
                            <br />
                            type:
                            <asp:TextBox ID="typeTextBox" runat="server" Text='<%# Bind("type") %>' />
                            <br />
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                                CommandName="Insert" Text="Insert" />
                            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                                CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            id:
                            <asp:Label ID="idLabel" runat="server" Text='<%# Eval("id") %>' />
                            <br />
                            name:
                            <asp:Label ID="nameLabel" runat="server" Text='<%# Bind("name") %>' />
                            <br />
                            description:
                            <asp:Label ID="descriptionLabel" runat="server" 
                                Text='<%# Bind("description") %>' />
                            <br />
                            is_random:
                            <asp:CheckBox ID="is_randomCheckBox" runat="server" 
                                Checked='<%# Bind("is_random") %>' Enabled="false" />
                            <br />
                            can_be_held:
                            <asp:CheckBox ID="can_be_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_held") %>' Enabled="false" />
                            <br />
                            container:
                            <asp:CheckBox ID="containerCheckBox" runat="server" 
                                Checked='<%# Bind("container") %>' Enabled="false" />
                            <br />
                            room_limit:
                            <asp:Label ID="room_limitLabel" runat="server" 
                                Text='<%# Bind("room_limit") %>' />
                            <br />
                            size:
                            <asp:Label ID="sizeLabel" runat="server" Text='<%# Bind("size") %>' />
                            <br />
                            can_be_seen_when_held:
                            <asp:CheckBox ID="can_be_seen_when_heldCheckBox" runat="server" 
                                Checked='<%# Bind("can_be_seen_when_held") %>' Enabled="false" />
                            <br />
                            type:
                            <asp:Label ID="typeLabel" runat="server" Text='<%# Bind("type") %>' />
                            <br />

                        </ItemTemplate>
                    </asp:FormView>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        SelectCommand="SELECT * FROM [items]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceItem" runat="server" 
        ConflictDetection="CompareAllValues" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        DeleteCommand="DELETE FROM [items] WHERE [id] = @original_id AND [name] = @original_name AND [description] = @original_description AND [is_random] = @original_is_random AND [can_be_held] = @original_can_be_held AND [container] = @original_container AND [room_limit] = @original_room_limit AND [size] = @original_size AND (([can_be_seen_when_held] = @original_can_be_seen_when_held) OR ([can_be_seen_when_held] IS NULL AND @original_can_be_seen_when_held IS NULL)) AND (([type] = @original_type) OR ([type] IS NULL AND @original_type IS NULL))" 
        InsertCommand="INSERT INTO [items] ([name], [description], [is_random], [can_be_held], [container], [room_limit], [size], [can_be_seen_when_held], [type]) VALUES (@name, @description, @is_random, @can_be_held, @container, @room_limit, @size, @can_be_seen_when_held, @type)" 
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT * FROM [items] WHERE ([id] = @id)" 
        UpdateCommand="UPDATE [items] SET [name] = @name, [description] = @description, [is_random] = @is_random, [can_be_held] = @can_be_held, [container] = @container, [room_limit] = @room_limit, [size] = @size, [can_be_seen_when_held] = @can_be_seen_when_held, [type] = @type WHERE [id] = @original_id AND [name] = @original_name AND [description] = @original_description AND [is_random] = @original_is_random AND [can_be_held] = @original_can_be_held AND [container] = @original_container AND [room_limit] = @original_room_limit AND [size] = @original_size AND (([can_be_seen_when_held] = @original_can_be_seen_when_held) OR ([can_be_seen_when_held] IS NULL AND @original_can_be_seen_when_held IS NULL)) AND (([type] = @original_type) OR ([type] IS NULL AND @original_type IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_name" Type="String" />
            <asp:Parameter Name="original_description" Type="String" />
            <asp:Parameter Name="original_is_random" Type="Boolean" />
            <asp:Parameter Name="original_can_be_held" Type="Boolean" />
            <asp:Parameter Name="original_container" Type="Boolean" />
            <asp:Parameter Name="original_room_limit" Type="Int32" />
            <asp:Parameter Name="original_size" Type="Int32" />
            <asp:Parameter Name="original_can_be_seen_when_held" Type="Boolean" />
            <asp:Parameter Name="original_type" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="is_random" Type="Boolean" />
            <asp:Parameter Name="can_be_held" Type="Boolean" />
            <asp:Parameter Name="container" Type="Boolean" />
            <asp:Parameter Name="room_limit" Type="Int32" />
            <asp:Parameter Name="size" Type="Int32" />
            <asp:Parameter Name="can_be_seen_when_held" Type="Boolean" />
            <asp:Parameter Name="type" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ListBoxItems" Name="id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="is_random" Type="Boolean" />
            <asp:Parameter Name="can_be_held" Type="Boolean" />
            <asp:Parameter Name="container" Type="Boolean" />
            <asp:Parameter Name="room_limit" Type="Int32" />
            <asp:Parameter Name="size" Type="Int32" />
            <asp:Parameter Name="can_be_seen_when_held" Type="Boolean" />
            <asp:Parameter Name="type" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_name" Type="String" />
            <asp:Parameter Name="original_description" Type="String" />
            <asp:Parameter Name="original_is_random" Type="Boolean" />
            <asp:Parameter Name="original_can_be_held" Type="Boolean" />
            <asp:Parameter Name="original_container" Type="Boolean" />
            <asp:Parameter Name="original_room_limit" Type="Int32" />
            <asp:Parameter Name="original_size" Type="Int32" />
            <asp:Parameter Name="original_can_be_seen_when_held" Type="Boolean" />
            <asp:Parameter Name="original_type" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
