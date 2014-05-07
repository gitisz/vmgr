<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SecurityRolePermissionEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.SecurityRolePermissionEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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
        
           
    </style>
    <script  type="text/javascript">
        window.scrollTo = function () { }
    </script>
    <script  type="text/javascript">
        var selectedPermissionsArray = [];

        function getSelectedPermissionsArrayIds() {
            return selectedPermissionsArray.join(",");
        }

        function OnAddPermission(id) {
            if ($.inArray(id, selectedPermissionsArray) == -1) {
                selectedPermissionsArray.push(id);
            }
        }

        function OnRemovePermission(id) {
            selectedPermissionsArray = $.grep(selectedPermissionsArray, function (val) { return val != id; });
        }

        function OnPermissionSelect(select) {
            var gridPermission = $find('<%= gridPermission.ClientID %>');
            var masterTableView = gridPermission.get_masterTableView();
            var dataItems = masterTableView.get_dataItems();
            for (var i = 0; i < dataItems.length; i++) {
                var buttonCheckboxSelect = dataItems[i].findControl("buttonCheckboxSelect");

                if (buttonCheckboxSelect != null)
                    buttonCheckboxSelect.set_checked(select);
            }
        }

        function OnMasterTableViewCreatedPermission(s, e) {
            var gridPermission = $find('<%= gridPermission.ClientID %>');
            var masterTableView = gridPermission.get_masterTableView();
            var dataItems = masterTableView.get_dataItems();
            for (var i = 0; i < dataItems.length; i++) {
                var buttonCheckboxSelect = dataItems[i].findControl("buttonCheckboxSelect");

                if (buttonCheckboxSelect != null)
                    if (buttonCheckboxSelect.get_checked())
                        OnAddPermission(dataItems[i].getDataKeyValue("SecurityPermissionId"))
            }
        }

        function getParentWindow() {
            var w = null;
            if (window.radWindow)
                w = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow)
                w = window.frameElement.radWindow;
            return w;
        }

        function OnSave(s, e) {
            Page_ClientValidate('');

            if (Page_IsValid) {
                var ajaxPanelSecurityRolePermissionEditor = $find("<%= this.ajaxPanelSecurityRolePermissionEditor.ClientID %>");
                if (ajaxPanelSecurityRolePermissionEditor != null)
                    ajaxPanelSecurityRolePermissionEditor.ajaxRequest("SAVE," + getSelectedPermissionsArrayIds());
            }
        }

        function OnSaveSuccess(msg) {

            var parentWindow = getParentWindow();

            var windowManagerAlertCheck = parentWindow.BrowserWindow.getWindowManagerAlertCheck();

            if (windowManagerAlertCheck != null) {
                windowManagerAlertCheck.radalert(msg, 350, 230, 'Permission Set Save Success', OnClose, null);
            }
        }

        function OnSaveFail(msg) {

            var parentWindow = getParentWindow();

            var windowManagerAlertError = parentWindow.BrowserWindow.getWindowManagerAlertError();

            if (windowManagerAlertError != null) {
                windowManagerAlertError.radalert(msg, 350, 230, 'Failed to Save Security Role Permissions', null, null);
            }
        }

        function OnClose(args) {
            if (args == true) {
                var parentWindow = getParentWindow();
                if (parentWindow != null) {
                    parentWindow.BrowserWindow.refreshSecurityRolePermissionList();
                    parentWindow.close(null);
                }
            }
        }


        function OnSecurityRoleChanged(s, e) {
            var ajaxPanelSecurityRolePermissionEditor = $find("<%= ajaxPanelSecurityRolePermissionEditor.ClientID %>");
            ajaxPanelSecurityRolePermissionEditor.ajaxRequest("RESET,");
        }
    </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxPanel 
    runat="server"
    ID="ajaxPanelSecurityRolePermissionEditor" 
    OnAjaxRequest="ajaxPanelSecurityRolePermissionEditor_AjaxRequest"
    >
    <asp:Panel
        ID="panelSecurityRolePermissionEditor"
        style="padding: 10px;"
        runat="server"
        >            
        <table style="border-collapse: collapse; font-size: 8pt; width: 98%;">
            <tr>
                <td style="vertical-align: top; width: 50%; padding: 5px;">
                    <fieldset  style="font-size: 9pt; padding: 10px;">
                        <legend style="margin-bottom: 10px;">Permission Set:</legend>
                        <table style="border-collapse: collapse; font-size: 8pt; width: 100%;">
                            <tr>
                                <td colspan="2">
                                    Security Role:
                                    <asp:RequiredFieldValidator ID="requiredFieldValidatorSecurityRole" 
                                        runat="server" 
                                        Display="Dynamic"
                                        ErrorMessage="(Security role is a required field.)"
                                        ControlToValidate="comboBoxSecurityRole"
                                        InitialValue="Please select..."
                                        >
                                        </asp:RequiredFieldValidator>        
                                    <asp:Label 
                                        runat="server" 
                                        ID="labelDeactivatedRole"
                                        ForeColor="Red"
                                        Visible="false"
                                        Text="The role for this record has been deactivated.  Please choose another." 
                                        />    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadComboBox 
                                        runat="server"
                                        ID="comboBoxSecurityRole" 
                                        DataSourceID="linqDataSourceSecurityRole"
                                        DataTextField="Name"
                                        DataValueField="SecurityRoleId"
                                        AutoPostBack="false"
                                        OnClientSelectedIndexChanged="function(s, e){  OnSecurityRoleChanged(s, e); }"
                                        >
                                    </telerik:RadComboBox> 
                                    <asp:LinqDataSource 
                                        runat="server" 
                                        ID="linqDataSourceSecurityRole" 
                                        OnSelecting="linqDataSourceSecurityRole_Selecting"
                                        />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Permissions:
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinqDataSource 
                                        runat="server" 
                                        ID="linqDataSourcePermission" 
                                        OnSelecting="linqDataSourcePermission_Selecting"
                                        >
                                    </asp:LinqDataSource>		
                                    <div style="border-left: solid 1px gray; border-top: solid 1px gray; border-right: solid 1px gray; ">	
                                        <telerik:RadGrid 
						                    runat="server" 
						                    ID="gridPermission"
                                            DataSourceID="linqDataSourcePermission"
						                    AllowSorting="true"
						                    AllowPaging="false" 
						                    AllowFilteringByColumn="false" 
                                            AllowMultiRowSelection="true"
						                    AutoGenerateColumns="false" 
						                    ShowStatusBar="true"
						                    Border="0"
						                    OnItemCreated="gridPermission_ItemCreated"
						                    >
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="False"></Selecting>
                                                <ClientEvents OnMasterTableViewCreated="OnMasterTableViewCreatedPermission"  />
                                            </ClientSettings>
                                            <GroupingSettings CaseSensitive="false" />       
						                    <MasterTableView 
							                    DataKeyNames="SecurityPermissionId" 
                                                ClientDataKeyNames="SecurityPermissionId" 
							                    ShowHeader="true" 
							                    AutoGenerateColumns="False" 
							                    AllowPaging="false"
							                    HierarchyLoadMode="ServerOnDemand"
							                    >
							                    <Columns>
                                                    <telerik:GridTemplateColumn  AutoPostBackOnFilter="true"  DataField="Hidden" UniqueName="Hidden" HeaderStyle-Width="16px" ItemStyle-Width="16px" ItemStyle-Height="16px" AllowFiltering="false">
                                                        <ItemTemplate>
                                                            <asp:Image
                                                                runat="server"
                                                                ID="imageAssetStatus"
                                                                style="padding-left: 3px;"
                                                                Visible="false"
                                                                />
                                                            <telerik:RadButton 
                                                                runat="server"
                                                                ID="buttonCheckboxSelect" 
                                                                ButtonType="ToggleButton" 
                                                                ToggleType="CheckBox"
                                                                BorderWidth="0" 
                                                                Height="16px"
                                                                Width="16px"
                                                                BackColor="transparent" 
                                                                AutoPostBack="false"
                                                                ToolTip="Select"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState HoveredCssClass="rbHoverCheckbox" PrimaryIconCssClass="rbToggleCheckboxChecked" />
                                                                    <telerik:RadButtonToggleState PrimaryIconCssClass="rbToggleCheckbox"  />
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
								                    <telerik:GridBoundColumn 
                                                        CurrentFilterFunction="Contains" 
                                                        AutoPostBackOnFilter="true"  
                                                        DataField="SecurityPermissionId" 
                                                        HeaderText="ID" 
                                                        >
								                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn 
                                                        CurrentFilterFunction="Contains" 
                                                        AutoPostBackOnFilter="true"  
                                                        DataField="Name" 
                                                        HeaderText="Permission" 
                                                        >
                                                        <ItemTemplate>
                                                            <asp:Label
                                                                runat="server"
                                                                ID="labelSecurityPermissionName"
                                                                ></asp:Label>
                                                        </ItemTemplate>
			                                        </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn 
                                                        CurrentFilterFunction="Contains" 
                                                        AutoPostBackOnFilter="true"  
                                                        DataField="Description" 
                                                        HeaderText="Description" 
                                                        >
                                                    </telerik:GridBoundColumn>
							                    </Columns>
						                    </MasterTableView>
					                    </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </asp:Panel>
</telerik:RadAjaxPanel>
