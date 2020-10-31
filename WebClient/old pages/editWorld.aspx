<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editWorld.aspx.cs" Inherits="WebClient.editWorld" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Capricorn Island World Editor<br />
        <br />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/RoomEditor.aspx">Room Editor</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink6" runat="server" 
            NavigateUrl="~/ItemTypeEditor.aspx">Item Type Editor</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ObjectEditor.aspx">Object Editor</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" 
            NavigateUrl="~/CharacterEditor.aspx">Character Editor</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink4" runat="server" 
            NavigateUrl="~/MonsterEditor.aspx">Monster Editor</asp:HyperLink>
    
        <br />
        <asp:HyperLink ID="HyperLink5" runat="server" 
            NavigateUrl="~/commandEditor.aspx">Command Editor</asp:HyperLink>
    
    </div>
    </form>
</body>
</html>
