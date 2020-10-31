<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CharacterEditor.aspx.cs" Inherits="WebClient.CharacterEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 338px;
        }
        .style2
        {
            width: 442px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width:100%;">
        <tr>
            <td class="style1">
                Select a character:</td>
            <td class="style2">
                Edit Character Stats:</td>
            <td>
                Items Held</td>
        </tr>
        <tr>
            <td class="style1" valign="top">
                <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" 
                    DataSourceID="SqlDataSourceClients" DataTextField="name" DataValueField="id" 
                    Rows="35" Width="238px" 
                    onselectedindexchanged="ListBox1_SelectedIndexChanged"></asp:ListBox>
            </td>
            <td class="style2" valign="top">
                <asp:FormView ID="FormView1" runat="server" DataKeyNames="id" 
                    DataSourceID="SqlDataSourceSingleClient" DefaultMode="Edit" 
                    onitemupdated="FormView1_ItemUpdated">
                    <EditItemTemplate>
                        id:
                        <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                        <br />
                        name:
                        <br />
                        <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                        <br />
                        description:<br />
                        <asp:TextBox ID="descriptionTextBox" runat="server" 
                            Text='<%# Bind("description") %>' Height="88px" TextMode="MultiLine" 
                            Width="278px" />
                        <br />
                        health:
                        <br />
                        <asp:TextBox ID="healthTextBox" runat="server" Text='<%# Bind("health") %>' />
                        <br />
                        phone:<br />
                        <asp:TextBox ID="phoneTextBox" runat="server" Text='<%# Bind("phone") %>' />
                        <br />
                        <br />
                        Login:<br />first_connect:
                        <br />
                        <asp:TextBox ID="first_connectTextBox" runat="server" 
                            Text='<%# Bind("first_connect") %>' ReadOnly="True" />
                        <br />
                        last_connect:
                        <br />
                        <asp:TextBox ID="last_connectTextBox" runat="server" 
                            Text='<%# Bind("last_connect") %>' ReadOnly="True" />
                        <br />
                        last_touch:
                        <br />
                        <asp:TextBox ID="last_touchTextBox" runat="server" 
                            Text='<%# Bind("last_touch") %>' ReadOnly="True" />
                        <br />
                        <br />
                        Location:<br />room_x:
                        <asp:TextBox ID="room_xTextBox" runat="server" Text='<%# Bind("room_x") %>' />
                        <br />
                        room_y:
                        <asp:TextBox ID="room_yTextBox" runat="server" Text='<%# Bind("room_y") %>' />
                        <br />
                        <br />
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                            CommandName="Update" Text="Update" />
                        &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                            CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        phone:
                        <asp:TextBox ID="phoneTextBox" runat="server" Text='<%# Bind("phone") %>' />
                        <br />
                        first_connect:
                        <asp:TextBox ID="first_connectTextBox" runat="server" 
                            Text='<%# Bind("first_connect") %>' />
                        <br />
                        last_connect:
                        <asp:TextBox ID="last_connectTextBox" runat="server" 
                            Text='<%# Bind("last_connect") %>' />
                        <br />
                        last_touch:
                        <asp:TextBox ID="last_touchTextBox" runat="server" 
                            Text='<%# Bind("last_touch") %>' />
                        <br />
                        name:
                        <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                        <br />
                        description:
                        <asp:TextBox ID="descriptionTextBox" runat="server" 
                            Text='<%# Bind("description") %>' />
                        <br />
                        room_x:
                        <asp:TextBox ID="room_xTextBox" runat="server" Text='<%# Bind("room_x") %>' />
                        <br />
                        room_y:
                        <asp:TextBox ID="room_yTextBox" runat="server" Text='<%# Bind("room_y") %>' />
                        <br />
                        health:
                        <asp:TextBox ID="healthTextBox" runat="server" Text='<%# Bind("health") %>' />
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
                        phone:
                        <asp:Label ID="phoneLabel" runat="server" Text='<%# Bind("phone") %>' />
                        <br />
                        first_connect:
                        <asp:Label ID="first_connectLabel" runat="server" 
                            Text='<%# Bind("first_connect") %>' />
                        <br />
                        last_connect:
                        <asp:Label ID="last_connectLabel" runat="server" 
                            Text='<%# Bind("last_connect") %>' />
                        <br />
                        last_touch:
                        <asp:Label ID="last_touchLabel" runat="server" 
                            Text='<%# Bind("last_touch") %>' />
                        <br />
                        name:
                        <asp:Label ID="nameLabel" runat="server" Text='<%# Bind("name") %>' />
                        <br />
                        description:
                        <asp:Label ID="descriptionLabel" runat="server" 
                            Text='<%# Bind("description") %>' />
                        <br />

                        room_x:
                        <asp:Label ID="room_xLabel" runat="server" Text='<%# Bind("room_x") %>' />
                        <br />
                        room_y:
                        <asp:Label ID="room_yLabel" runat="server" Text='<%# Bind("room_y") %>' />
                        <br />
                        health:
                        <asp:Label ID="healthLabel" runat="server" Text='<%# Bind("health") %>' />
                        <br />
                        <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="Edit" />
                        &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" />
                        &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" 
                            CommandName="New" Text="New" />
                    </ItemTemplate>
                </asp:FormView>
            </td>
            <td valign="top">
                <br />
                <br />
                <br />
                <asp:Panel ID="Panel1" runat="server" Visible="False">
                    <asp:ListBox ID="ListBoxItemsHeld" runat="server" Rows="10" Width="230px" 
                    DataSourceID="SqlDataSourceClientsItems" DataTextField="namehealth" 
                    DataValueField="id"></asp:ListBox>
                    <asp:Button ID="ButtonDeleteItem" runat="server" 
                        onclick="ButtonDeleteItem_Click" Text="Delete Item" />
                    <br />
                    <asp:DropDownList ID="DropDownListItems" runat="server" 
                        DataSourceID="SqlDataSourceItemTypes" DataTextField="name" DataValueField="id">
                    </asp:DropDownList>
                    <asp:Button ID="ButtonGiveItem" runat="server" onclick="ButtonGiveItem_Click" 
                        Text="Give Item" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSourceClients" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        SelectCommand="SELECT [id], [name], [phone] FROM [clients] ORDER BY [id]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceSingleClient" runat="server" 
        ConflictDetection="CompareAllValues" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        DeleteCommand="DELETE FROM [clients] WHERE [id] = @original_id AND (([phone] = @original_phone) OR ([phone] IS NULL AND @original_phone IS NULL)) AND (([first_connect] = @original_first_connect) OR ([first_connect] IS NULL AND @original_first_connect IS NULL)) AND (([last_connect] = @original_last_connect) OR ([last_connect] IS NULL AND @original_last_connect IS NULL)) AND (([last_touch] = @original_last_touch) OR ([last_touch] IS NULL AND @original_last_touch IS NULL)) AND (([name] = @original_name) OR ([name] IS NULL AND @original_name IS NULL)) AND (([description] = @original_description) OR ([description] IS NULL AND @original_description IS NULL)) AND (([room_x] = @original_room_x) OR ([room_x] IS NULL AND @original_room_x IS NULL)) AND (([room_y] = @original_room_y) OR ([room_y] IS NULL AND @original_room_y IS NULL)) AND (([health] = @original_health) OR ([health] IS NULL AND @original_health IS NULL))" 
        InsertCommand="INSERT INTO [clients] ([phone], [first_connect], [last_connect], [last_touch], [name], [description], [room_x], [room_y], [health]) VALUES (@phone, @first_connect, @last_connect, @last_touch, @name, @description, @room_x, @room_y, @health)" 
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT [id], [phone], [first_connect], [last_connect], [last_touch], [name], [description], [room_x], [room_y], [health] FROM [clients] WHERE ([id] = @id)" 
        UpdateCommand="UPDATE [clients] SET [phone] = @phone, [first_connect] = @first_connect, [last_connect] = @last_connect, [last_touch] = @last_touch, [name] = @name, [description] = @description, [room_x] = @room_x, [room_y] = @room_y, [health] = @health WHERE [id] = @original_id AND (([phone] = @original_phone) OR ([phone] IS NULL AND @original_phone IS NULL)) AND (([first_connect] = @original_first_connect) OR ([first_connect] IS NULL AND @original_first_connect IS NULL)) AND (([last_connect] = @original_last_connect) OR ([last_connect] IS NULL AND @original_last_connect IS NULL)) AND (([last_touch] = @original_last_touch) OR ([last_touch] IS NULL AND @original_last_touch IS NULL)) AND (([name] = @original_name) OR ([name] IS NULL AND @original_name IS NULL)) AND (([description] = @original_description) OR ([description] IS NULL AND @original_description IS NULL)) AND (([room_x] = @original_room_x) OR ([room_x] IS NULL AND @original_room_x IS NULL)) AND (([room_y] = @original_room_y) OR ([room_y] IS NULL AND @original_room_y IS NULL)) AND (([health] = @original_health) OR ([health] IS NULL AND @original_health IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_phone" Type="String" />
            <asp:Parameter Name="original_first_connect" Type="DateTime" />
            <asp:Parameter Name="original_last_connect" Type="DateTime" />
            <asp:Parameter Name="original_last_touch" Type="DateTime" />
            <asp:Parameter Name="original_name" Type="String" />
            <asp:Parameter Name="original_description" Type="String" />
            <asp:Parameter Name="original_room_x" Type="Int32" />
            <asp:Parameter Name="original_room_y" Type="Int32" />
            <asp:Parameter Name="original_health" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="phone" Type="String" />
            <asp:Parameter Name="first_connect" Type="DateTime" />
            <asp:Parameter Name="last_connect" Type="DateTime" />
            <asp:Parameter Name="last_touch" Type="DateTime" />
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="room_x" Type="Int32" />
            <asp:Parameter Name="room_y" Type="Int32" />
            <asp:Parameter Name="health" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="ListBox1" Name="id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="phone" Type="String" />
            <asp:Parameter Name="first_connect" Type="DateTime" />
            <asp:Parameter Name="last_connect" Type="DateTime" />
            <asp:Parameter Name="last_touch" Type="DateTime" />
            <asp:Parameter Name="name" Type="String" />
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="room_x" Type="Int32" />
            <asp:Parameter Name="room_y" Type="Int32" />
            <asp:Parameter Name="health" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_phone" Type="String" />
            <asp:Parameter Name="original_first_connect" Type="DateTime" />
            <asp:Parameter Name="original_last_connect" Type="DateTime" />
            <asp:Parameter Name="original_last_touch" Type="DateTime" />
            <asp:Parameter Name="original_name" Type="String" />
            <asp:Parameter Name="original_description" Type="String" />
            <asp:Parameter Name="original_room_x" Type="Int32" />
            <asp:Parameter Name="original_room_y" Type="Int32" />
            <asp:Parameter Name="original_health" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceItemTypes" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        SelectCommand="SELECT * FROM [items]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceClientsItems" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        
        SelectCommand="SELECT inventory.id, inventory.itemid, inventory.health, items.name, inventory.clientid, items.name + ' &lt;' + CAST(inventory.health AS VARCHAR(10)) + '%&gt;' AS namehealth FROM inventory INNER JOIN items ON inventory.itemid = items.id WHERE (inventory.clientid = @clientid)">
        <SelectParameters>
            <asp:ControlParameter ControlID="ListBox1" Name="clientid" 
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
