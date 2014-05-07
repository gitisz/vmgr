<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SecurityRoleEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.SecurityRoleEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>

<telerik:RadCodeBlock ID="radcbMaintainMeetings" runat="server">
    <style type="text/css">
        
        .rbAdd
        {
            background: url('/_layouts/Images/Vmgr/add-icon-16.png') no-repeat 0 0 !important;
        }
            
        .rbCheck
        {
            background: url('/_layouts/Images/Vmgr/search-icon-16.png') no-repeat 0 0 !important;
        }
            
        .rbLoading
        {
            background: url('/_layouts/Images/Vmgr/loading2.gif') no-repeat 0 0 !important;
        }
            
    </style>
    <script  type="text/javascript">
        window.scrollTo = function () { }
    </script>
    <script  type="text/javascript">

   
        var selectedSecurityRoleId = <%= selectedSecurityRoleId %>;

        function OnSave(s, e) {

            if(Page_IsValid)
            {
                var ajaxPanelRole = $find('<%= ajaxPanelRole.ClientID %>');
                ajaxPanelRole.ajaxRequest('SAVE');
            }
        }

        function OnSaveSuccess(msg){
           
            var parentWindow = getParentWindow();

            var windowManagerAlertCheck = parentWindow.BrowserWindow.getWindowManagerAlertCheck();

            if(windowManagerAlertCheck != null)
            {
                windowManagerAlertCheck.radalert(msg, 350, 230, 'Save Security Role Success', OnCloseSecurityRoleEditor, null);
            }
        }

        function OnSaveFail(msg){
           
            var parentWindow = getParentWindow();

            var windowManagerAlertError = parentWindow.BrowserWindow.getWindowManagerAlertError();

            if(windowManagerAlertError != null)
            {
                windowManagerAlertError.radalert(msg, 350, 230, 'Failed to Save Security Role', null, null);
            }
        }

        function OnCloseSecurityRoleEditor(s, e) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshSecuritytRoles();
            }
            this.OnClose(s, e);
        }

    </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxPanel 
    runat="server"
    ID="ajaxPanelRole" 
    OnAjaxRequest="ajaxPanelRole_AjaxRequest"
    >
        <telerik:RadInputManager ID="radInputManagerSiteRole" runat="server">
        <telerik:TextBoxSetting BehaviorID="textBoxName" EmptyMessage="Type a role name..." Validation-IsRequired="true">
            <TargetControls>
                <telerik:TargetInput ControlID="textBoxName" />
            </TargetControls>
        </telerik:TextBoxSetting>        
        <telerik:TextBoxSetting BehaviorID="textBoxDescription" EmptyMessage="Optional..." Validation-IsRequired="false">
            <TargetControls>
                <telerik:TargetInput ControlID="textBoxDescription" />
            </TargetControls>
        </telerik:TextBoxSetting>
    </telerik:RadInputManager>
    <asp:Panel
        ID="panelSiteRoleEditor"
        style="padding: 10px;"
        runat="server"
        >            
        <table style="border-collapse: collapse; font-size: 8pt; width: 98%;">
            <tr>
                <td style="vertical-align: top; width: 50%; padding: 5px;">
                    <fieldset  style="font-size: 9pt; padding: 10px;">
                        <legend style="margin-bottom: 10px;">Site Role:</legend>
                        <table style="border-collapse: collapse; font-size: 8pt; width: 100%;">
                            <tr>
                                <td colspan="2">
                                    Role name:
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox 
                                        runat="server"
                                        ID="textBoxName"
                                        Width="99.5%"
                                        >
                                    </asp:TextBox>                    
                                </td>
                            </tr>                            
                            <tr>
                                <td colspan="2">
                                    Description:
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox 
                                        runat="server"
                                        ID="textBoxDescription"
                                        Width="99.5%"
                                        >
                                    </asp:TextBox>                    
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </asp:Panel> 
</telerik:RadAjaxPanel>
