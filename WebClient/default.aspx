<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebClient._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="capricorn_styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">


<div class="god">
  <div class="tabs_god">

<div class="row">
<div class="outliner">
      
<!--      <div class="row">
        <div class="message_bar">
        </div>
      </div>-->
      
      

<div class="left_panel_area">
<div class="outliner_tight">
  <div class="editor_title"><div class="roundcorner"></div>Login</div>
      <div class="editor_area">
        <div class="editor_content">
                      <label for="text1">Username</label><br /> <asp:Label ID="LabelError" runat="server" Text=""></asp:Label>
                  <asp:TextBox ID="username" name="text1" CssClass="editor_field" runat="server"></asp:TextBox>
<div class="editor_row"></div>
                      <label for="text2">Password</label><br />
            
                  <asp:TextBox ID="Password" name="text2" CssClass="editor_field" runat="server" TextMode="Password"></asp:TextBox>

<div class="editor_row"><div class="rule"></div></div>

                    <asp:LinkButton ID="LinkButtonLogin" runat="server" OnClick="LinkButton1_Click"><div class="btn">Log In</div></asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonSignup" runat="server" OnClick="LinkButtonSignup_Click"><div class="btn">Sign Up</div></asp:LinkButton>
        </div>
      </div>
      </div>
</div>







<div class="right_panel_area">    
    <div class="outliner_tight_margin">

<div class="item_128"><asp:Image ID="ImageLogin" runat="server" ImageUrl="~/images/login_ani.gif" /></div>
        
 
</div>  
</div>  
 
 

</div>
</div>
</div>
</div>
    </form>
    <audio src="audio/DesertChipSoundsV0.mp3" autoplay loop/>"
</body>
</html>
