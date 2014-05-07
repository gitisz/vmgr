<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowUploadPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowUploadPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowUploadPackage() {
            return $find("<%=windowUploadPackage.ClientID %>");
        }

        function OnShowUploadPackage(s, e) {
            var windowUploadPackageUrl = siteUrl + "/Plugins/WindowUploadPackage.aspx";
            var windowUploadPackage = $find("<%= this.windowUploadPackage.ClientID %>");
            windowUploadPackage.setUrl(windowUploadPackageUrl);
            windowUploadPackage.show();
        }

        function OnClientBeforeShowWindowUploadPackage(s, e) {
            OnPopupResizeUploadPackage();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowUploadPackageContent";
            var ajaxLoadingPanelWindowUploadPackage = $find("<%=ajaxLoadingPanelWindowUploadPackage.ClientID %>");
            ajaxLoadingPanelWindowUploadPackage.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowUploadPackage(s, e) {
        }

        function OnClientBeforeCloseWindowUploadPackage(s, e) {
        }

        function OnPopupResizeUploadPackage() {
            var windowUploadPackage = $find("<%=windowUploadPackage.ClientID %>");
            windowUploadPackage.set_width($(window).width() * .5);
            windowUploadPackage.set_height($(window).height() * .5);
        }

        addEvent(window, "resize", OnPopupResizeUploadPackage);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowUploadPackage" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowUploadPackage"
    Modal="true"
    Title="Upload Plugin Package"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowUploadPackage.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowUploadPackage"
    OnClientShow="OnClientShowWindowUploadPackage"
    OnClientBeforeClose="OnClientBeforeCloseWindowUploadPackage"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
