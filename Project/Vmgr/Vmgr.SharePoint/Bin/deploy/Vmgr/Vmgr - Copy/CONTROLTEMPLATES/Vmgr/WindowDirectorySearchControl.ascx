<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowDirectorySearchControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowDirectorySearchControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <script type="text/javascript">

        var siteUrl = "<%= this.siteUrl %>";

        var directorySearchWindowWidth;
        var directorySearchWindowHeight;

        function getWindowDirectorySearch() {
            return $find("<%=windowDirectorySearch.ClientID %>");
        }

        function OnShowDirectorySearch(search, searchType, callback) {
            var windowDirectorySearchUrl = siteUrl + "/WindowDirectorySearch.aspx?SearchType="
                + searchType
                + "&Search="
                + search
                + "&Callback="
                + callback
            ;
            var windowDirectorySearch = $find("<%= this.windowDirectorySearch.ClientID %>");
            windowDirectorySearch.setUrl(windowDirectorySearchUrl);
            windowDirectorySearch.show();
        }

        function OnClientBeforeShowWindowDirectorySearch(s, e) {
            OnPopupResizeDirectorySearch();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowDirectorySearchContent";
            var ajaxLoadingPanelWindowDirectorySearch = $find("<%=ajaxLoadingPanelWindowDirectorySearch.ClientID %>");
            ajaxLoadingPanelWindowDirectorySearch.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowDirectorySearch(s, e) {
        }

        function OnClientBeforeCloseWindowDirectorySearch(s, e) {
        }

        function OnPopupResizeDirectorySearch() {
            var windowDirectorySearch = $find("<%=windowDirectorySearch.ClientID %>");
            windowDirectorySearch.set_width($(window).width() * .5);
            windowDirectorySearch.set_height($(window).height() * .5);
        }

        addEvent(window, "resize", OnPopupResizeDirectorySearch);

    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowDirectorySearch" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowDirectorySearch"
    Modal="true"
    Title="Directory Search"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/WindowDirectorySearch.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowDirectorySearch"
    OnClientShow="OnClientShowWindowDirectorySearch"
    OnClientBeforeClose="OnClientBeforeCloseWindowDirectorySearch"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
