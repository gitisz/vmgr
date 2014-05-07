<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowMovePackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowMovePackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowMovePackage() {
            return $find("<%=windowMovePackage.ClientID %>");
        }

        function OnShowMovePackage(s, e) {
            var windowMovePackageUrl = siteUrl + "/Plugins/WindowMovePackage.aspx"
                + "?UniqueId="
                + s.get_commandArgument()
                ;
            var windowMovePackage = $find("<%= this.windowMovePackage.ClientID %>");
            windowMovePackage.setUrl(windowMovePackageUrl);
            windowMovePackage.show();
        }

        function OnClientBeforeShowWindowMovePackage(s, e) {
            OnPopupResizeMovePackage();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowMovePackageContent";
            var ajaxLoadingPanelWindowMovePackage = $find("<%=ajaxLoadingPanelWindowMovePackage.ClientID %>");
            ajaxLoadingPanelWindowMovePackage.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowMovePackage(s, e) {
        }

        function OnClientBeforeCloseWindowMovePackage(s, e) {
        }

        function OnPopupResizeMovePackage() {
            var windowMovePackage = $find("<%=windowMovePackage.ClientID %>");
            windowMovePackage.set_width($(window).width() * .5);
            windowMovePackage.set_height($(window).height());
        }

        addEvent(window, "resize", OnPopupResizeMovePackage);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowMovePackage" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowMovePackage"
    Modal="true"
    Title="Move Plugin Package"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowMovePackage.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowMovePackage"
    OnClientShow="OnClientShowWindowMovePackage"
    OnClientBeforeClose="OnClientBeforeCloseWindowMovePackage"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
