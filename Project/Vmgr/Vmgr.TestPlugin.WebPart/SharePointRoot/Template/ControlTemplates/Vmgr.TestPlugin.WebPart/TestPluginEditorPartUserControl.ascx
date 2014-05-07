<%@ Assembly Name="Vmgr.TestPlugin.WebPart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=05645e6641d2565d" %>

<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint" %>
<%@ Control Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="TestPluginEditorPartUserControl.ascx.cs" 
    Inherits="Vmgr.TestPlugin.WebPart.UI.TestPluginEditorPartUserControl, Vmgr.TestPlugin.WebPart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=05645e6641d2565d" %>

<asp:Panel 
    ID="Panel"   
    runat="server" 
    >
    <div class="user-section-title" style="padding: 2px;">Available Lists</div>
    <div style="padding: 2px;">
        <asp:ListBox
            ID="listBoxServers"
            DataSourceID="linqDataSourceServers"
            DataTextField="Name"
            DataValueField="UniqueId"
            Height="150px"
            Width="100%"
            SelectionMode="Single"
            AutoPostBack="true"
            runat="server"
            >
        </asp:ListBox>
    </div> 
</asp:Panel>

<asp:LinqDataSource 
    ID="linqDataSourceServers" 
    runat="server"
    OnSelecting="linqDataSourceServers_Selecting"
    />


