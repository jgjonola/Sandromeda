<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="game.aspx.cs" Inherits="WebClient.game" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Game Mode</title>
    <link href="capricorn_styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:capricornislandConnectionString %>" 
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT name, id, description, longDescription, x, y, nexit, nkey, visualType, type, visualTypeS, terrain, indoor, wanderingAllowed, sleepAllowed, combatAllowed, soundAllowed, doorVisualType FROM rooms WHERE (x &gt;= @x3) AND (x &lt; @x) AND (y &gt;= @y3) AND (y &lt; @y) ORDER BY y, x">
        <SelectParameters>
            <asp:SessionParameter Name="x3" SessionField="gridMinX" Type="Int32" />
            <asp:SessionParameter Name="x" SessionField="gridMaxX" Type="Int32" />
            <asp:SessionParameter Name="y3" SessionField="gridMinY" Type="Int32" />
            <asp:SessionParameter Name="y" SessionField="gridMaxY" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
<div class="god">
  <div class="tabs_god">
    <div class="icon_contain">
    	<a href="#"><div class="icon_button"><div class="icon_load"></div></div></a>
        <a href="#"><div class="icon_button"><div class="icon_new"></div></div></a>
        <a href="#"><div class="icon_button"><div class="icon_save"></div></div></a>
        <a href="#"><div class="icon_button"><div class="icon_delete"></div></div></a>
        <div class="icon_project_title" style="padding-left: 5px;">capricorn_game</div>	
    </div>
    <div class="tabs_contain">
      <asp:HyperLink ID="HyperLinkMaps" runat="server" NavigateUrl="maps.aspx"><div class="tabs">Maps</div></asp:HyperLink>
      <asp:HyperLink ID="HyperLinkItems" runat="server" NavigateUrl="items.aspx"><div class="tabs">Items</div></asp:HyperLink>
      <asp:HyperLink ID="HyperLinkNPCs" runat="server" NavigateUrl="npcs.aspx"><div class="tabs">NPCs</div></asp:HyperLink>
      <asp:HyperLink ID="HyperLinkCharacters" runat="server" NavigateUrl="characters.aspx"><div class="tabs">Characters</div></asp:HyperLink>
      <asp:HyperLink ID="HyperLinkActions" runat="server" NavigateUrl="actions.aspx"><div class="tabs">Actions</div></asp:HyperLink>
      <asp:HyperLink ID="HyperLinkGame" runat="server" NavigateUrl="game.aspx"><div class="tabs">Game</div></asp:HyperLink>
    </div>
<div class="row">
<div class="outliner">
      
<!--      <div class="row">
        <div class="message_bar">
        </div>
      </div>-->
      
      

<div class="left_panel_area">
<div class="outliner_tight">
  <div class="editor_title"><div class="roundcorner"></div>Game Mode</div>
      <div class="editor_area">
        <div class="editor_content">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:TextBox ID="TextBoxServer" CssClass="editor_textarea" runat="server" style="height:500px; resize: none;" TextMode="MultiLine"></asp:TextBox>
                </ContentTemplate>
                        <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
            </asp:UpdatePanel>
                    <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="5000">
    </asp:Timer>
         	
			<div class="editor_row"></div> 
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="TextBoxSend" CssClass="editor_field" runat="server"></asp:TextBox>
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
      </div>
</div>
        <div class="right_panel_area">    
    <div class="outliner_tight_margin">
          <div class="map_area">

<asp:UpdatePanel ID="UpdatePanel3" runat="server">
    <ContentTemplate> 
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="id" 
        DataSourceID="SqlDataSource1" GroupItemCount="7" 
        onitemdatabound="ListView1_ItemDataBound">
       
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
            <tr ID="itemPlaceholderContainer" runat="server" >
                <td ID="itemPlaceholder" runat="server">
                </td>
            </tr>
        </GroupTemplate>

        <ItemTemplate>
            <td runat="server" id="thecell">
            <asp:Panel ID="Panel1" runat="server" >
                <asp:Label ID="nameLabel" runat="server" Text='<%# Eval("name") %>' BackColor="White" Visible ="false" />
                <asp:Label ID="visualType" runat="server" Text='<%# Eval("visualTypeS") %>' BackColor="White" Visible ="false" />
                <asp:Label ID="nexitLabel" runat="server" Text='<%# Eval("nexit") %>' Visible ="false"/>
                <asp:Label ID="labelId" runat="server" Text='<%# Eval("id") %>' BackColor="White" Visible ="false" />
                </asp:Panel>
            </td>
        </ItemTemplate>

        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="groupPlaceholderContainer" runat="server" border="0" class="table_grid">
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

    </asp:ListView>
        
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>

</div>
        </div>
            </div>
</div>
</div>
</div>
</div>
    </form>
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
