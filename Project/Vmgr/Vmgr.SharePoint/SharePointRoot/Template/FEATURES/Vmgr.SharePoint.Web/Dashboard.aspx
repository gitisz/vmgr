<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="Dashboard" 
    AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" 
    Inherits="Vmgr.SharePoint.Dashboard, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="Vmgr" TagName="MonitoringControl" Src="~/_CONTROLTEMPLATES/Vmgr/MonitoringControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowMonitorPackageControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowMonitorPackageControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
      <ul style="list-style-type: none; margin: 0px 20px; padding: 0px;"><li style="text-align: justify;"> - Monitor performance counters for packages you select.</li></ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
        </script>
    </telerik:RadCodeBlock>
    <div style="padding: 5px;">
        Welcome to V-Manager!
        <div style="font-size: 8pt;">
            <p style="margin: 0px; padding: 2px;">V-Manager is a pluggable service that makes it easy to develop applications requiring schedulable jobs or on-demand processes.</p>
        </div>
        <asp:Panel ID="PanelWeHateIe8" Visible="false" runat="server">
            <div style="color: orange; font-size: 8pt;">
                Due to limitations in IE8, this application will not fully support this browser.  To take full advantage of V-Manager, please use a modern browser.
            </div>
        </asp:Panel>
        <br />
        <Vmgr:MonitoringControl 
            runat="server"
            ID="monitoringControl" 
            >
        </Vmgr:MonitoringControl>
        <Vmgr:WindowMonitorPackageControl 
            runat="server"
            ID="windowMonitorPackageControl" 
            >
        </Vmgr:WindowMonitorPackageControl>
    </div>
</asp:Content>  
