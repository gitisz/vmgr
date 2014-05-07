<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowSecurityMembershipEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowSecurityMembershipEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">

        function getWindowSecurityMembershipEditor() {
            return $find("<%=windowSecurityMembershipEditor.ClientID %>");
        }

        function OnShowFilter(s, e) {
            var windowSecurityMembershipEditorUrl = siteUrl + "/Administration/WindowSecurityMembershipEditor.aspx?";
            var windowSecurityMembershipEditor = $find("<%= this.windowSecurityMembershipEditor.ClientID %>");
            windowSecurityMembershipEditor.setUrl(windowSecurityMembershipEditorUrl);
            windowSecurityMembershipEditor.show();
        }

        function OnClientBeforeShowWindowSecurityMembershipEditor(s, e) {
            OnPopupResizeFilterWindowSecurityMembershipEditor();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowSecurityMembershipEditorContent";
            var ajaxLoadingPanelWindowSecurityMembershipEditor = $find("<%=ajaxLoadingPanelWindowSecurityMembershipEditor.ClientID %>");
            ajaxLoadingPanelWindowSecurityMembershipEditor.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowSecurityMembershipEditor(s, e) {
        }

        function OnClientBeforeCloseWindowSecurityMembershipEditor(s, e) {
        }

        function OnPopupResizeFilterWindowSecurityMembershipEditor() {
            var windowSecurityMembershipEditor = $find("<%=windowSecurityMembershipEditor.ClientID %>");

            var minHeight = 500;
            var calcHeight = $(window).height() * .3;
            
            windowSecurityMembershipEditor.set_width($(window).width() * .5);

            if(calcHeight < minHeight)
                windowSecurityMembershipEditor.set_height(minHeight);
            else
                windowSecurityMembershipEditor.set_height(calcHeight);
        }

        addEvent(window, "resize", OnPopupResizeFilterWindowSecurityMembershipEditor);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowSecurityMembershipEditor" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowSecurityMembershipEditor"
    Modal="true"
    Title="Security Membership Editor"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowSecurityMembershipEditor.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowSecurityMembershipEditor"
    OnClientShow="OnClientShowWindowSecurityMembershipEditor"
    OnClientBeforeClose="OnClientBeforeCloseWindowSecurityMembershipEditor"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
