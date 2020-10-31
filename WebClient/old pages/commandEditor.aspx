<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="commandEditor.aspx.cs" Inherits="WebClient.commandEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 333px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    Action</td>
                <td>
                    Reaction:</td>
                <td>
                    Secondary Commands:</td>
            </tr>
            <tr>
                <td class="style1" valign="top">
                    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" 
                        DataSourceID="SqlDataSourceCommands" DataTextField="action" DataValueField="id" 
                        Height="562px" Width="245px"></asp:ListBox>
                    <br />
                    <asp:Button ID="ButtonAdd" runat="server" onclick="ButtonAdd_Click" 
                        Text="Add Action" />
                </td>
                <td valign="top">
                    <asp:FormView ID="FormViewAction" runat="server" DataKeyNames="id" 
                        DataSourceID="SqlDataSourceCommand" DefaultMode="Edit" 
                        onitemcommand="FormViewAction_ItemCommand" 
                        onitemdeleted="FormViewAction_ItemDeleted" 
                        oniteminserted="FormViewAction_ItemInserted" 
                        onitemupdated="FormViewAction_ItemUpdated">
                        <EditItemTemplate>
                            id:
                            <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                            <br />
                            action:
                            <br />
                            <asp:TextBox ID="actionTextBox" runat="server" 
                                ontextchanged="actionTextBox_TextChanged" Text='<%# Bind("action") %>' />
                            <br />
                            text:
                            <asp:CheckBox ID="textCheckBox" runat="server" Checked='<%# Bind("text") %>' />
                            <br />
                            response:
                            <br />
                            <asp:TextBox ID="responseTextBox" runat="server" Height="59px" 
                                Text='<%# Bind("response") %>' TextMode="MultiLine" Width="217px" />
                            <br />
                            requires_more:
                            <asp:CheckBox ID="requires_moreCheckBox" runat="server" 
                                Checked='<%# Bind("requires_more") %>' />
                            <br />
                            requires_item:
                            <asp:CheckBox ID="requires_itemCheckBox" runat="server" 
                                Checked='<%# Bind("requires_item") %>' />
                            <br />
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                                CommandName="Update" Text="Update" />
                            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                                CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            action:
                            <asp:TextBox ID="actionTextBox" runat="server" Text='<%# Bind("action") %>' />
                            <br />
                            text:
                            <asp:CheckBox ID="textCheckBox" runat="server" Checked='<%# Bind("text") %>' />
                            <br />
                            response:
                            <asp:TextBox ID="responseTextBox" runat="server" 
                                Text='<%# Bind("response") %>' />
                            <br />
                            requires_more:
                            <asp:CheckBox ID="requires_moreCheckBox" runat="server" 
                                Checked='<%# Bind("requires_more") %>' />
                            <br />
                            requires_item:
                            <asp:CheckBox ID="requires_itemCheckBox" runat="server" 
                                Checked='<%# Bind("requires_item") %>' />
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
                            action:
                            <asp:Label ID="actionLabel" runat="server" Text='<%# Bind("action") %>' />
                            <br />
                            text:
                            <asp:CheckBox ID="textCheckBox" runat="server" Checked='<%# Bind("text") %>' 
                                Enabled="false" />
                            <br />
                            response:
                            <asp:Label ID="responseLabel" runat="server" Text='<%# Bind("response") %>' />
                            <br />
                            requires_more:
                            <asp:CheckBox ID="requires_moreCheckBox" runat="server" 
                                Checked='<%# Bind("requires_more") %>' Enabled="false" />
                            <br />
                            requires_item:
                            <asp:CheckBox ID="requires_itemCheckBox" runat="server" 
                                Checked='<%# Bind("requires_item") %>' Enabled="false" />
                            <br />

                        </ItemTemplate>
                    </asp:FormView>
                    <asp:SqlDataSource ID="SqlDataSourceCommand" runat="server" 
                        ConflictDetection="CompareAllValues" 
                        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
                        DeleteCommand="DELETE FROM [actions] WHERE [id] = @original_id AND [action] = @original_action AND [text] = @original_text AND (([response] = @original_response) OR ([response] IS NULL AND @original_response IS NULL)) AND (([requires_more] = @original_requires_more) OR ([requires_more] IS NULL AND @original_requires_more IS NULL)) AND (([requires_item] = @original_requires_item) OR ([requires_item] IS NULL AND @original_requires_item IS NULL))" 
                        InsertCommand="INSERT INTO [actions] ([action], [text], [response], [requires_more], [requires_item]) VALUES (@action, @text, @response, @requires_more, @requires_item)" 
                        OldValuesParameterFormatString="original_{0}" 
                        SelectCommand="SELECT * FROM [actions] WHERE ([id] = @id)" 
                        UpdateCommand="UPDATE [actions] SET [action] = @action, [text] = @text, [response] = @response, [requires_more] = @requires_more, [requires_item] = @requires_item WHERE [id] = @original_id AND [action] = @original_action AND [text] = @original_text AND (([response] = @original_response) OR ([response] IS NULL AND @original_response IS NULL)) AND (([requires_more] = @original_requires_more) OR ([requires_more] IS NULL AND @original_requires_more IS NULL)) AND (([requires_item] = @original_requires_item) OR ([requires_item] IS NULL AND @original_requires_item IS NULL))">
                        <DeleteParameters>
                            <asp:Parameter Name="original_id" Type="Int32" />
                            <asp:Parameter Name="original_action" Type="String" />
                            <asp:Parameter Name="original_text" Type="Boolean" />
                            <asp:Parameter Name="original_response" Type="String" />
                            <asp:Parameter Name="original_requires_more" Type="Boolean" />
                            <asp:Parameter Name="original_requires_item" Type="Boolean" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="action" Type="String" />
                            <asp:Parameter Name="text" Type="Boolean" />
                            <asp:Parameter Name="response" Type="String" />
                            <asp:Parameter Name="requires_more" Type="Boolean" />
                            <asp:Parameter Name="requires_item" Type="Boolean" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ListBox1" Name="id" 
                                PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="action" Type="String" />
                            <asp:Parameter Name="text" Type="Boolean" />
                            <asp:Parameter Name="response" Type="String" />
                            <asp:Parameter Name="requires_more" Type="Boolean" />
                            <asp:Parameter Name="requires_item" Type="Boolean" />
                            <asp:Parameter Name="original_id" Type="Int32" />
                            <asp:Parameter Name="original_action" Type="String" />
                            <asp:Parameter Name="original_text" Type="Boolean" />
                            <asp:Parameter Name="original_response" Type="String" />
                            <asp:Parameter Name="original_requires_more" Type="Boolean" />
                            <asp:Parameter Name="original_requires_item" Type="Boolean" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    <asp:SqlDataSource ID="SqlDataSourceCommands" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
        SelectCommand="SELECT * FROM [actions]"></asp:SqlDataSource>
    </form>
</body>
</html>
