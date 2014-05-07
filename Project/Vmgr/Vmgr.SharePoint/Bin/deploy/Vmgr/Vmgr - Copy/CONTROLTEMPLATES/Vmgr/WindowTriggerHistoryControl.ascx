<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowTriggerHistoryControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowTriggerHistoryControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowTriggerHistory() {
            return $find("<%=windowTriggerHistory.ClientID %>");
        }

        function OnShowTriggerHistory(s, e) {
            var windowTriggerHistoryUrl = siteUrl + "/Scheduler/WindowTriggerHistory.aspx?JobId="
                + e.get_commandArgument();
            var windowTriggerHistory = $find("<%= this.windowTriggerHistory.ClientID %>");
            windowTriggerHistory.setUrl(windowTriggerHistoryUrl);
            windowTriggerHistory.show();
        }

        function OnClientBeforeShowWindowTriggerHistory(s, e) {
            OnPopupResizeTriggerHistory();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowTriggerHistoryContent";
            var ajaxLoadingPanelWindowTriggerHistory = $find("<%=ajaxLoadingPanelWindowTriggerHistory.ClientID %>");
            ajaxLoadingPanelWindowTriggerHistory.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowTriggerHistory(s, e) {
        }

        function OnClientBeforeCloseWindowTriggerHistory(s, e) {
        }

        function OnPopupResizeTriggerHistory() {
            var windowTriggerHistory = $find("<%=windowTriggerHistory.ClientID %>");
            windowTriggerHistory.set_width($(window).width() * .5);
            windowTriggerHistory.set_height($(window).height());
        }

        addEvent(window, "resize", OnPopupResizeTriggerHistory);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowTriggerHistory" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowTriggerHistory"
    Modal="true"
    Title="Trigger History"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowTriggerHistory.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowTriggerHistory"
    OnClientShow="OnClientShowWindowTriggerHistory"
    OnClientBeforeClose="OnClientBeforeCloseWindowTriggerHistory"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
