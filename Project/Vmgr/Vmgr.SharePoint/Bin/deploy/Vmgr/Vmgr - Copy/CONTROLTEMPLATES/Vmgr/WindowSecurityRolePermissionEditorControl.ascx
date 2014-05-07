<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowSecurityRolePermissionEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowSecurityRolePermissionEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">

        function getWindowSecurityRolePermissionEditor() {
            return $find("<%=windowSecurityRolePermissionEditor.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowSecurityRolePermissionEditorUrl = siteUrl + "/Administration/WindowSecurityRolePermissionEditor.aspx?";
            var windowSecurityRolePermissionEditor = $find("<%= this.windowSecurityRolePermissionEditor.ClientID %>");
            windowSecurityRolePermissionEditor.setUrl(windowSecurityRolePermissionEditorUrl);
            windowSecurityRolePermissionEditor.show();
        }

        function OnClientBeforeShowWindowSecurityRolePermissionEditor(s, e) {
            OnPopupResizeFilterWindowSecurityRolePermissionEditor();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowSecurityRolePermissionEditorContent";
            var ajaxLoadingPanelWindowSecurityRolePermissionEditor = $find("<%=ajaxLoadingPanelWindowSecurityRolePermissionEditor.ClientID %>");
            ajaxLoadingPanelWindowSecurityRolePermissionEditor.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowSecurityRolePermissionEditor(s, e) {
        }

        function OnClientBeforeCloseWindowSecurityRolePermissionEditor(s, e) {
        }

        function OnPopupResizeFilterWindowSecurityRolePermissionEditor() {
            var windowSecurityRolePermissionEditor = $find("<%=windowSecurityRolePermissionEditor.ClientID %>");

            var minHeight = 500;
            var calcHeight = $(window).height() * .3;
            
            windowSecurityRolePermissionEditor.set_width($(window).width() * .5);

            if(calcHeight < minHeight)
                windowSecurityRolePermissionEditor.set_height(minHeight);
            else
                windowSecurityRolePermissionEditor.set_height(calcHeight);
        }

        addEvent(window, "resize", OnPopupResizeFilterWindowSecurityRolePermissionEditor);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowSecurityRolePermissionEditor" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowSecurityRolePermissionEditor"
    Modal="true"
    Title="Security Role Permission Editor"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowSecurityRolePermissionEditor.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowSecurityRolePermissionEditor"
    OnClientShow="OnClientShowWindowSecurityRolePermissionEditor"
    OnClientBeforeClose="OnClientBeforeCloseWindowSecurityRolePermissionEditor"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
