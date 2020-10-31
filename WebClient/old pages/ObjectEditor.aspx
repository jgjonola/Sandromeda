<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObjectEditor.aspx.cs" Inherits="WebClient.ObjectEditor1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" 
            DataSourceID="SqlDataSourceObjects" DataTextField="id" DataValueField="id" 
            Height="774px" Width="315px"></asp:ListBox>
        <asp:SqlDataSource ID="SqlDataSourceObjects" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionStringLocal %>" 
            SelectCommand="SELECT * FROM [inventory]"></asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
