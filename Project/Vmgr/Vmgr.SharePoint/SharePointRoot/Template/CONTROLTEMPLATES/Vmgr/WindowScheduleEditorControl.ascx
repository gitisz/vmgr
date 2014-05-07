<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowScheduleEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowScheduleEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        function getWindowScheduleEditor() {
            return $find("<%=windowScheduleEditor.ClientID %>");
        }

        function OnShowScheduleEditor(uniqueId) {
            var windowScheduleEditorUrl = siteUrl + "/Scheduler/WindowScheduleEditor.aspx"
                + "?UniqueId="
                + uniqueId
                ;
            var windowScheduleEditor = $find("<%= this.windowScheduleEditor.ClientID %>");
            windowScheduleEditor.setUrl(windowScheduleEditorUrl);
            windowScheduleEditor.show();
        }

        function OnClientBeforeShowWindowScheduleEditor(s, e) {
            OnPopupResizeScheduleEditor();
            s.get_contentFrame().parentNode.id = "ajaxLoadingPanelWindowScheduleEditorContent";
            var ajaxLoadingPanelWindowScheduleEditor = $find("<%=ajaxLoadingPanelWindowScheduleEditor.ClientID %>");
            ajaxLoadingPanelWindowScheduleEditor.show(s.get_contentFrame().parentNode.id);
        }

        function OnClientShowWindowScheduleEditor(s, e) {
        }

        function OnClientBeforeCloseWindowScheduleEditor(s, e) {
        }

        function OnPopupResizeScheduleEditor() {
            var windowScheduleEditor = $find("<%=windowScheduleEditor.ClientID %>");
            windowScheduleEditor.set_width($(window).width() * .8);
            windowScheduleEditor.set_height($(window).height());
        }

        addEvent(window, "resize", OnPopupResizeScheduleEditor);
        
    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel 
    ID="ajaxLoadingPanelWindowScheduleEditor" 
    Skin="Default" 
    runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadWindow 
    runat="server"
    ID="windowScheduleEditor"
    Modal="true"
    Title="Edit Schedule"
    BackColor="#ECE9D8"
    Behaviors="Close"
    VisibleStatusbar="false"
    EnableShadow="true"
    NavigateUrl="~site/Plugins/WindowScheduleEditor.aspx"
    IconUrl="/_layouts/images/Vmgr/Vmgr-16.png"
    OnClientBeforeShow="OnClientBeforeShowWindowScheduleEditor"
    OnClientShow="OnClientShowWindowScheduleEditor"
    OnClientBeforeClose="OnClientBeforeCloseWindowScheduleEditor"
    ShowContentDuringLoad="false"
    >
</telerik:RadWindow>
