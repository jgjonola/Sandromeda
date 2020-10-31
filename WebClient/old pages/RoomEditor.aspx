<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomEditor.aspx.cs" Inherits="WebClient.RoomEditor" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
body {
	margin:0 auto;
	background-color:#363636;
}

table, th, td {
	/* text-align: center; */
	/* vertical-align: middle; */
	border-collapse: separate;
        border-spacing: 0px 0px;
	padding:0px;
	table-layout:fixed;
	font-size: 14px;
            text-align: right;
        }


</style>
    </head>
<body >
    <form id="form1" runat="server">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <table>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="LinkButtonNorth" runat="server" Font-Size="XX-Large" ForeColor="White" OnClick="ButtonScrollSouth_Click">^</asp:LinkButton>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="LinkButtonWest" runat="server" Font-Size="XX-Large" ForeColor="White" OnClick="ButtonWest_Click">&lt;</asp:LinkButton>
                </td>
                <td>    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate> 
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="id" 
        DataSourceID="SqlDataSource1" GroupItemCount="7" 
        onitemdatabound="ListView1_ItemDataBound">
        <EditItemTemplate>
            <td runat="server" style="">name:
                <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                <br />
                id:
                <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                <br />
                description:
                <asp:TextBox ID="descriptionTextBox" runat="server" Text='<%# Bind("description") %>' />
                <br />
                x:
                <asp:TextBox ID="xTextBox" runat="server" Text='<%# Bind("x") %>' />
                <br />
                y:
                <asp:TextBox ID="yTextBox" runat="server" Text='<%# Bind("y") %>' />
                <br />
                nexit:
                <asp:TextBox ID="nexitTextBox" runat="server" Text='<%# Bind("nexit") %>' />
                <br />
                nkey:
                <asp:TextBox ID="nkeyTextBox" runat="server" Text='<%# Bind("nkey") %>' />
                <br />
                visualType:
                <asp:TextBox ID="visualTypeTextBox" runat="server" Text='<%# Bind("visualType") %>' />
                <br />
                type:
                <asp:TextBox ID="typeTextBox" runat="server" Text='<%# Bind("type") %>' />
                <br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                <br />
            </td>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <EmptyItemTemplate>
<td runat="server" />
        </EmptyItemTemplate>
        <GroupTemplate>
            <tr ID="itemPlaceholderContainer" runat="server">
                <td ID="itemPlaceholder" runat="server">
                </td>
            </tr>
        </GroupTemplate>

        <InsertItemTemplate>
            <td runat="server" style="">name:
                <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                <br />
                description:
                <asp:TextBox ID="descriptionTextBox" runat="server" Text='<%# Bind("description") %>' />
                <br />
                x:
                <asp:TextBox ID="xTextBox" runat="server" Text='<%# Bind("x") %>' />
                <br />
                y:
                <asp:TextBox ID="yTextBox" runat="server" Text='<%# Bind("y") %>' />
                <br />
                nexit:
                <asp:TextBox ID="nexitTextBox" runat="server" Text='<%# Bind("nexit") %>' />
                <br />
                nkey:
                <asp:TextBox ID="nkeyTextBox" runat="server" Text='<%# Bind("nkey") %>' />
                <br />
                visualType:
                <asp:TextBox ID="visualTypeTextBox" runat="server" Text='<%# Bind("visualType") %>' />
                <br />
                type:
                <asp:TextBox ID="typeTextBox" runat="server" Text='<%# Bind("type") %>' />
                <br />
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                <br />
            </td>
        </InsertItemTemplate>

        <ItemTemplate>
            <td runat="server">
            <asp:Panel ID="Panel1" runat="server" Width="100px" Height="100px" HorizontalAlign="Center">
                <br />
                <asp:Label ID="nameLabel" runat="server" Text='<%# Eval("name") %>' BackColor="White" />
                <br />
                <asp:Label ID="nexitLabel" runat="server" Text='<%# Eval("nexit") %>' Visible ="false"/>
                <asp:Label ID="labelId" runat="server" Text='<%# Eval("id") %>' BackColor="White" />
                </asp:Panel>
                
            </td>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="groupPlaceholderContainer" runat="server" border="0" style="">
                            <tr ID="groupPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <td runat="server" style="">
                name:
                <asp:Label ID="nameLabel" runat="server" Text='<%# Eval("name") %>' />
                <br />id:
                <asp:Label ID="idLabel" runat="server" Text='<%# Eval("id") %>' />
                <br />description:
                <asp:Label ID="descriptionLabel" runat="server" 
                    Text='<%# Eval("description") %>' />
                <br />x:
                <asp:Label ID="xLabel" runat="server" Text='<%# Eval("x") %>' />
                <br />y:
                <asp:Label ID="yLabel" runat="server" Text='<%# Eval("y") %>' />
                <br />nexit:
                <asp:Label ID="nexitLabel" runat="server" Text='<%# Eval("nexit") %>' />
                <br />nkey:
                <asp:Label ID="nkeyLabel" runat="server" Text='<%# Eval("nkey") %>' />
                <br />visualType:
                <asp:Label ID="visualTypeLabel" runat="server" 
                    Text='<%# Eval("visualType") %>' />
                <br />type:
                <asp:Label ID="typeLabel" runat="server" Text='<%# Eval("type") %>' />
                <br />
            </td>
        </SelectedItemTemplate>
    </asp:ListView>
        
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:LinkButton ID="LinkButtonEast" runat="server" Font-Size="XX-Large" ForeColor="White" OnClick="ButtonScrollRight_Click">&gt;</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:LinkButton ID="LinkButtonSouth" runat="server" Font-Size="XX-Large" ForeColor="White" OnClick="ButtonScrollNorth_Click">V</asp:LinkButton>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>

            
    <asp:Timer ID="Timer1" runat="server" Interval="5000" ontick="Timer1_Tick">
    </asp:Timer>
    <p>
        <asp:Button ID="ButtonBigger" runat="server" onclick="ButtonBigger_Click" 
            Text="Make it bigger" />
    </p>
    </form>
</body>
</html>
