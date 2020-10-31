<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default_game.aspx.cs" Inherits="WebClient.default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="capricorn_styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
	<div class="god">
		<div class="logo"></div>
		<div class="row">
    <form id="form1" runat="server">
                	<div class="booker">    
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:TextBox ID="TextBoxServer" class="fielder" runat="server" Height="490px" 
                    TextMode="MultiLine" Width="260px"></asp:TextBox>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="5000">
    </asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

                <asp:TextBox ID="TextBoxSend" class="fielder" runat="server" Width="260px" 
                    AutoCompleteType="Disabled"></asp:TextBox>
                <asp:ImageButton ID="ImageButtonSend" runat="server" Height="1px" 
                    ImageUrl="~/images/midway_atoll_us_island_flag.png" 
                    onclick="ImageButtonSend_Click" Width="1px" />
                    </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ImageButtonSend" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </div>
                <div class="tag">The World's First Massively Multiplayer Text Messaging Game<br />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/editWorld.aspx">Editor</asp:HyperLink>
                    </div>
    </form>
		</div> 
      </div>
</body>
<script type="text/javascript">


    function afterAsyncPostBack() {
        var objDiv = document.getElementById("TextBoxServer");
        objDiv.scrollTop = objDiv.scrollHeight;
        //document.getElementById("TextBoxSend").focus();
    }

    Sys.Application.add_load(appl_init);

    function appl_init() {
        var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        pgRegMgr.add_endRequest(EndHandler);
    }

    function EndHandler() {
        afterAsyncPostBack();
    }
 
       
    </script>

</html>
