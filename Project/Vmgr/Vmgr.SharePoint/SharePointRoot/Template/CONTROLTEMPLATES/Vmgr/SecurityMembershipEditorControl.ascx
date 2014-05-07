<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SecurityMembershipEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.SecurityMembershipEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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

        var selectedItemHierarchicalIndex = 0;
    
        var selectedSecurityRolId = <%= selectedSecurityRolId %>;


        /*
        * Entity
        */

        function OnCheckEntityClicked(s, e) {
            var primaryIconElement = s.get_primaryIconElement();
            $telerik.$(primaryIconElement).removeClass("rbCheck");
            $telerik.$(primaryIconElement).addClass("rbLoading");

            var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
            ajaxPanelEntity.ajaxRequest('CHECK');
        }

        function OnSave(s, e) {

            if(Page_IsValid)
            {
                var hiddenFieldIsValidEntity = $get('<%= hiddenFieldIsValidEntity.ClientID %>');
                hiddenFieldIsValidEntity.value = "false";

                var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
                ajaxPanelEntity.ajaxRequest('SAVE');
            }
        }

        function OnSaveSuccess(msg){
           
            var parentWindow = getParentWindow();

            var windowManagerAlertCheck = parentWindow.BrowserWindow.getWindowManagerAlertCheck();

            if(windowManagerAlertCheck != null)
            {
                windowManagerAlertCheck.radalert(msg, 350, 230, 'Save Security Membership Success', OnCloseSecurityMembershipEditor, null);
            }
        }

        function OnSaveFail(msg){
           
            var parentWindow = getParentWindow();

            var windowManagerAlertError = parentWindow.BrowserWindow.getWindowManagerAlertError();

            if(windowManagerAlertError != null)
            {
                windowManagerAlertError.radalert(msg, 350, 230, 'Failed to Save Security Membership', null, null);
            }
        }

        function OnCloseSecurityMembershipEditor(s, e) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshSecuritytMemberships();
            }
            this.OnClose(s, e);
        }

        function OnEntityNameChanged(s, e) {
            var hiddenFieldEntity = $get('<%= hiddenFieldEntity.ClientID %>');
            hiddenFieldEntity.value = "";

            var hiddenFieldIsValidEntity = $get('<%= hiddenFieldIsValidEntity.ClientID %>');
            hiddenFieldIsValidEntity.value = false;

            var lblEntityResult = $get('<%= lblEntityResult.ClientID %>');
            lblEntityResult.innerHTML = 'Search directory for Entity names.';
            lblEntityResult.style.color = 'black';
        }

        function OnShowEntitySearchWindow() {
            var textBoxEntityName = $find('<%= textBoxEntityName.ClientID %>');
            var parentWindow = getParentWindow();

            parentWindow.BrowserWindow.OnShowDirectorySearch(textBoxEntityName.get_value(), 'User', 'CallbackEntityResult');
        }

        function CallbackEntityResult(result) {
            var textBoxEntityName = $find('<%= textBoxEntityName.ClientID %>');
            textBoxEntityName.set_value(result.DisplayName);

            var hiddenFieldEntity = $get('<%= hiddenFieldEntity.ClientID %>');
            hiddenFieldEntity.value = result.Eid;

            var hiddenFieldIsValidEntity = $get('<%= hiddenFieldIsValidEntity.ClientID %>');
            hiddenFieldIsValidEntity.value = result.IsValid;

            var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
            ajaxPanelEntity.ajaxRequest('CHECK');
        }

        function OnEntityRequestStart(s, e) {
            var buttonCheckEntity = $find('<%= buttonCheckEntity.ClientID %>');
            buttonCheckEntity.set_enabled(false);
        }

        function OnEntityResponseEnd(s, e) {
            var buttonCheckEntity = $find('<%= buttonCheckEntity.ClientID %>');
            buttonCheckEntity.set_enabled(true);

            var primaryIconElementCheck = buttonCheckEntity.get_primaryIconElement();

            if ($telerik.$(primaryIconElementCheck).hasClass("rbLoading")) {
                $telerik.$(primaryIconElementCheck).removeClass("rbLoading");
                $telerik.$(primaryIconElementCheck).addClass("rbCheck");
            }
        }

        function OnConfirmDeleteEntity(s, e) {
            selectedItemHierarchicalIndex = s.get_commandArgument();
            var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
            ajaxPanelEntity.ajaxRequest('CONFIRM_DELETE_Entity,' + selectedItemHierarchicalIndex);
        }

        function OnConfirmDeleteEntityHandler(args) {
            var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
            if (args == true) {
                ajaxPanelEntity.ajaxRequest('DELETE_Entity,' + selectedItemHierarchicalIndex);
            }
        }

        function OnDeleteEntityComplete(s, e) {
            var ajaxPanelEntity = $find('<%= ajaxPanelEntity.ClientID %>');
            ajaxPanelEntity.ajaxRequest('DELETE_Entity_COMPLETE');
        }

        /*
        * GLOBAL
        */

        function OnEntityRequestStart(s, e) {
        }

        function OnEntityResponseEnd(s, e) {
        }

        /*
        *   Role
        */

        function OnSecurityRoleRowSelected(s, e) {
            var row = s.get_selectedItems()[0];
            selectedSecurityRolId = row.getDataKeyValue('SecurityRoleId');
        }

        function OnValidateSecurityRole(s, e) {

            if (selectedSecurityRolId == null)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        function OnValidateSecurityEntity(s, e) {

            var hiddenFieldIsValidEntity = $get('<%= hiddenFieldIsValidEntity.ClientID %>');
            e.IsValid = (hiddenFieldIsValidEntity.value == "true");
        }



    </script>
</telerik:RadCodeBlock>
    <telerik:RadAjaxPanel 
        runat="server"
        ID="ajaxPanelEntity" 
        OnAjaxRequest="ajaxPanelEntity_AjaxRequest"
        ClientEvents-OnRequestStart="OnEntityRequestStart"
        ClientEvents-OnResponseEnd="OnEntityResponseEnd"
        >

        <div style="padding: 10px;">
            <h3>Select a security role: <asp:Label
                runat="server"
                ID="labelPackage"
                ></asp:Label>
                </h3>
            Please select a security role for this membership.
            <asp:ValidationSummary
                runat="server"
                ID="validationSummarySchedule"
                DisplayMode="BulletList"
                style="margin: 5px;"
                />
        </div>
        <asp:LinqDataSource
            runat="server"
            ID="linqDataSourceSecurityRole"
            OnSelecting="linqDataSourceSecurityRole_Selecting"
            ></asp:LinqDataSource>
        <telerik:RadGrid 
            runat="server" 
            ID="gridSecurityRole" 
            DataSourceID="linqDataSourceSecurityRole"
            GridLines="None" 
            BorderStyle="None"
            AutoGenerateColumns="false"
            Width="100%" 
            AllowSorting="True" 
            AllowPaging="True" 
            AllowCustomPaging="true"
            PageSize="5" 
            OnItemDataBound="gridSecurityRole_ItemDataBound" 
            >
            <ClientSettings 
                EnableRowHoverStyle="true">
                <Selecting 
                    AllowRowSelect="True"
                    ></Selecting>
                <ClientEvents
                    OnRowSelected="OnSecurityRoleRowSelected"
                    />
            </ClientSettings>                            
            <MasterTableView 
                AllowPaging="true" 
                AllowCustomPaging="true"
                AutoGenerateColumns="false" 
                HierarchyLoadMode="ServerOnDemand" 
			    DataKeyNames="SecurityRoleId" 
			    ClientDataKeyNames="SecurityRoleId,Name" 
                >
                <Columns>
                    <telerik:GridBoundColumn
                        UniqueName="Name" 
                        DataField="Name" 
                        HeaderText="Role Name"
                        HeaderStyle-Width="100%" 
                        SortExpression="Name"
                        >
                    </telerik:GridBoundColumn>
                </Columns>
                <NoRecordsTemplate>
                    <div style="padding: 5px;">
                        No security roles found.
                    </div>
                </NoRecordsTemplate>
            </MasterTableView>
            <ClientSettings 
                AllowDragToGroup="false" 
                AllowColumnsReorder="false" 
                />
            <PagerStyle 
                Mode="NextPrevAndNumeric" 
                HorizontalAlign="Center" 
                PageSizeLabelText="Results Per Page"
                AlwaysVisible="true" 
                />
            <FilterMenu 
                EnableImageSprites="False" 
                />
            <HeaderContextMenu 
                CssClass="GridContextMenu GridContextMenu_Default" 
                />
        </telerik:RadGrid>
        <asp:CustomValidator
            runat="server"
            ID="customValidatorSelectedSecurityRole"
            Display="None"
            Text=""
            ErrorMessage="A security role selection is required."
            ClientValidationFunction="OnValidateSecurityRole"
            ></asp:CustomValidator>
        <asp:CustomValidator
            runat="server"
            ID="customValidatorEntity"
            Display="None"
            Text=""
            ErrorMessage="A valid directory entity is required."
            ClientValidationFunction="OnValidateSecurityEntity"
            ></asp:CustomValidator>

    <div style="padding: 10px;">
        <table style="margin: auto; width: 100%; border-collapse: collapse;">
            <tr>
                <td style="padding-left: 4px;">
                    <asp:Label 
                        runat="server" 
                        ID="lblEntityResult" 
                        EnableViewState="false" 
                        Text="Search directory for Entity names."
                        Font-Size="10pt"
                        Font-Names="Verdana"
                        />
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td class="label" style="white-space: nowrap; width: 140px;">
                    Name / EID / Group:<span style="color: red;">*</span>
                </td>
                <td style="width: 200px;">
                    <telerik:RadTextBox 
                        runat="server" 
                        ID="textBoxEntityName" 
                        ToolTip="Search directory for Entity names."
                        Width="200px"
                        >
                        <ClientEvents
                            OnValueChanged="OnEntityNameChanged" 
                            OnKeyPress="OnEntityNameChanged"
                            />
                        </telerik:RadTextBox>
                    <asp:HiddenField 
                        ID="hiddenFieldEntity" 
                        runat="server" 
                        />
                    <asp:HiddenField 
                        ID="hiddenFieldIsValidEntity" 
                        runat="server" 
                        />
                </td>
                <td style="padding-top: 3px;">
                    <telerik:RadButton 
                        runat="server" 
                        ID="buttonCheckEntity" 
                        ToolTip="Click to check availability of Entity."
                        CausesValidation="false"
                        ValidationGroup="SearchEntity" 
                        AutoPostBack="false"
                        OnClientClicked="OnCheckEntityClicked" 
                        >
                        <Icon 
                            PrimaryIconCssClass="rbCheck" 
                            PrimaryIconLeft="10" 
                            PrimaryIconTop="4" 
                            />
                        </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
</telerik:RadAjaxPanel>
