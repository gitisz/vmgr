<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowFilterPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowFilterPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowFilterPackage() {
            return $find("<%=windowFilterPackage.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowFilterPackageUrl = siteUrl + "/Plugins/WindowFilterPackage.aspx?FilterType=<%= this.FilterType %>";
            var windowFilterPackage = $find("<%= this.windowFilterPackage.ClientID %>");
            windowFilterPackage.setUrl(windowFilterPackageUrl);
            windowFilterPackage.show();
        }

        function OnClientBeforeShowWindowFilterPackage(s, e) {
            OnPopupResizeFilter();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowFilterPackageContent";
            var ajaxLoadingPanelWindowFilterPackage = $find("<%=ajaxLoadingPanelWindowFilterPackage.ClientID %>");
            ajaxLoadingPanelWindowFilterPackage.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowFilterPackage(s, e) {
        }

        function OnClientBeforeCloseWindowFilterPackage(s, e) {
        }

        function OnPopupResizeFilter() {
            var windowFilterPackage = $find("<%=windowFilterPackage.ClientID %>");
            windowFilterPackage.set_width($(window).width() * .5);
            windowFilterPackage.set_height($(window).height() * .5);
        }

        addEvent(window, "resize", OnPopupResizeFilter);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowFilterPackage" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowFilterPackage"
    Modal="true"
    Title="Filter Packages"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowFilterPackage.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowFilterPackage"
    OnClientShow="OnClientShowWindowFilterPackage"
    OnClientBeforeClose="OnClientBeforeCloseWindowFilterPackage"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
