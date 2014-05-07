<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowFilterScheduleControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowFilterScheduleControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowFilterSchedule() {
            return $find("<%=windowFilterSchedule.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowFilterScheduleUrl = siteUrl + "/Scheduler/WindowFilterSchedule.aspx?FilterType=<%= this.FilterType %>";
            var windowFilterSchedule = $find("<%= this.windowFilterSchedule.ClientID %>");
            windowFilterSchedule.setUrl(windowFilterScheduleUrl);
            windowFilterSchedule.show();
        }

        function OnClientBeforeShowWindowFilterSchedule(s, e) {
            OnPopupResizeFilter();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowFilterScheduleContent";
            var ajaxLoadingPanelWindowFilterSchedule = $find("<%=ajaxLoadingPanelWindowFilterSchedule.ClientID %>");
            ajaxLoadingPanelWindowFilterSchedule.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowFilterSchedule(s, e) {
        }

        function OnClientBeforeCloseWindowFilterSchedule(s, e) {
        }

        function OnPopupResizeFilter() {
            var windowFilterSchedule = $find("<%=windowFilterSchedule.ClientID %>");
            windowFilterSchedule.set_width($(window).width() * .5);
            windowFilterSchedule.set_height($(window).height() * .5);
        }

        addEvent(window, "resize", OnPopupResizeFilter);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowFilterSchedule" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowFilterSchedule"
    Modal="true"
    Title="Filter Schedules"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowFilterSchedule.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowFilterSchedule"
    OnClientShow="OnClientShowWindowFilterSchedule"
    OnClientBeforeClose="OnClientBeforeCloseWindowFilterSchedule"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
