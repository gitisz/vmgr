<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowMonitorPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowMonitorPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowMonitorPackage() {
            return $find("<%=windowMonitorPackage.ClientID %>");
        }

        function OnShowMonitorPackage(s, e) {
            var windowMonitorPackageUrl = siteUrl + "/WindowMonitorPackage.aspx";
            var windowMonitorPackage = $find("<%= this.windowMonitorPackage.ClientID %>");
            windowMonitorPackage.setUrl(windowMonitorPackageUrl);
            windowMonitorPackage.show();
        }

        function OnClientBeforeShowWindowMonitorPackage(s, e) {
            OnPopupResizeMonitorPackage();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowMonitorPackageContent";
            var ajaxLoadingPanelWindowMonitorPackage = $find("<%=ajaxLoadingPanelWindowMonitorPackage.ClientID %>");
            ajaxLoadingPanelWindowMonitorPackage.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowMonitorPackage(s, e) {
        }

        function OnClientBeforeCloseWindowMonitorPackage(s, e) {
        }

        function OnPopupResizeMonitorPackage() {
            var windowMonitorPackage = $find("<%=windowMonitorPackage.ClientID %>");
            windowMonitorPackage.set_width($(window).width() * .5);
            windowMonitorPackage.set_height($(window).height());
        }

        addEvent(window, "resize", OnPopupResizeMonitorPackage);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowMonitorPackage" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowMonitorPackage"
    Modal="true"
    Title="Monitor Package"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/WindowMonitorPackage.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowMonitorPackage"
    OnClientShow="OnClientShowWindowMonitorPackage"
    OnClientBeforeClose="OnClientBeforeCloseWindowMonitorPackage"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
