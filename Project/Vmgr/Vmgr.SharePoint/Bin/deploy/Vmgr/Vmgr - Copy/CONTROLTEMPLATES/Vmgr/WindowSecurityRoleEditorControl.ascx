<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowSecurityRoleEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowSecurityRoleEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">

        function getWindowSecurityRoleEditor() {
            return $find("<%=windowSecurityRoleEditor.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowSecurityRoleEditorUrl = siteUrl + "/Administration/WindowSecurityRoleEditor.aspx?";
            var windowSecurityRoleEditor = $find("<%= this.windowSecurityRoleEditor.ClientID %>");
            windowSecurityRoleEditor.setUrl(windowSecurityRoleEditorUrl);
            windowSecurityRoleEditor.show();
        }

        function OnClientBeforeShowWindowSecurityRoleEditor(s, e) {
            OnPopupResizeFilterWindowSecurityRoleEditor();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowSecurityRoleEditorContent";
            var ajaxLoadingPanelWindowSecurityRoleEditor = $find("<%=ajaxLoadingPanelWindowSecurityRoleEditor.ClientID %>");
            ajaxLoadingPanelWindowSecurityRoleEditor.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowSecurityRoleEditor(s, e) {
        }

        function OnClientBeforeCloseWindowSecurityRoleEditor(s, e) {
        }

        function OnPopupResizeFilterWindowSecurityRoleEditor() {
            var windowSecurityRoleEditor = $find("<%=windowSecurityRoleEditor.ClientID %>");

            var minHeight = 500;
            var calcHeight = $(window).height() * .3;
            
            windowSecurityRoleEditor.set_width($(window).width() * .5);

            if(calcHeight < minHeight)
                windowSecurityRoleEditor.set_height(minHeight);
            else
                windowSecurityRoleEditor.set_height(calcHeight);
        }

        addEvent(window, "resize", OnPopupResizeFilterWindowSecurityRoleEditor);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowSecurityRoleEditor" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowSecurityRoleEditor"
    Modal="true"
    Title="Security Role Editor"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowSecurityRoleEditor.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowSecurityRoleEditor"
    OnClientShow="OnClientShowWindowSecurityRoleEditor"
    OnClientBeforeClose="OnClientBeforeCloseWindowSecurityRoleEditor"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
