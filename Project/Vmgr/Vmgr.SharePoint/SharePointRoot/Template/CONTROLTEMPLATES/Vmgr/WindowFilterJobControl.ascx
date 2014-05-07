<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowFilterJobControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowFilterJobControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">

        var siteUrl = "<%= this.siteUrl %>";

        function getWindowFilterJob() {
            return $find("<%=windowFilterJob.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowFilterJobUrl = siteUrl + "/Scheduler/WindowFilterJob.aspx?FilterType=<%= this.FilterType %>";
            var windowFilterJob = $find("<%= this.windowFilterJob.ClientID %>");
            windowFilterJob.setUrl(windowFilterJobUrl);
            windowFilterJob.show();
        }

        function OnClientBeforeShowWindowFilterJob(s, e) {
            OnPopupResizeFilter();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowFilterJobContent";
            var ajaxLoadingPanelWindowFilterJob = $find("<%=ajaxLoadingPanelWindowFilterJob.ClientID %>");
            ajaxLoadingPanelWindowFilterJob.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowFilterJob(s, e) {
        }

        function OnClientBeforeCloseWindowFilterJob(s, e) {
        }

        function OnPopupResizeFilter() {
            var windowFilterJob = $find("<%=windowFilterJob.ClientID %>");
            windowFilterJob.set_width($(window).width() * .5);
            windowFilterJob.set_height($(window).height() * .5);
        }

        addEvent(window, "resize", OnPopupResizeFilter);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowFilterJob" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowFilterJob"
    Modal="true"
    Title="Filter Jobs"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowFilterJob.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowFilterJob"
    OnClientShow="OnClientShowWindowFilterJob"
    OnClientBeforeClose="OnClientBeforeCloseWindowFilterJob"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
