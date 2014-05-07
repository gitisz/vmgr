<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SecurityControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.SecuritiesControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="Vmgr" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowDirectorySearchControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowDirectorySearchControl.ascx" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        var siteUrl = "<%= this.siteUrl %>";

        function OnClientTabSelected(s, e) {

            var tab = e.get_tab();
        }

        function OnRefreshSecurity(s, e) {
            var ajaxPanelSecurities = $find('<%= ajaxPanelSecurities.ClientID %>');
            ajaxPanelSecurities.ajaxRequest('REFRESH,');
        }


        function OptionsOnLoad(s, e) {
            var nodes = s.get_allNodes();
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].expand();
                }
            }
        }

        function OnOptionSelected(s, e) {

            var itemValue = s.get_selectedNode().get_value();

            if (itemValue == 'REFRESH') {

                var node = s.get_selectedNode();
                node.unselect();

                OnRefreshSecurity();
            }

            if (itemValue == 'ADD_MEMBERSHIP') {
                OnAddSecurityMembership();
            }

            if (itemValue == 'ADD_ROLE') {
                OnAddSecurityRole();
            }

            if (itemValue == 'MANAGE_PERMISSION_SET') {
                OnAddSecurityRolePermission();
            }
        }

        function OnOpenOptions(s, e) {
            var ajaxPanelSecurities = $find('<%= ajaxPanelSecurities.ClientID %>');
            ajaxPanelSecurities.ajaxRequest('OPEN_OPTIONS,');
        }

        function OnCloseOptions() {
            var ajaxPanelSecurities = $find('<%= ajaxPanelSecurities.ClientID %>');
            ajaxPanelSecurities.ajaxRequest('CLOSE_OPTIONS,');
        }

        function OnAddSecurityMembership() {
            var windowSecurityMembershipEditor = getWindowSecurityMembershipEditor();
            var windowSecurityMembershipEditorUrl = siteUrl + "/Administration/WindowSecurityMembershipEditor.aspx?SecurityMembershipId=";
            if (windowSecurityMembershipEditor != null) {
                windowSecurityMembershipEditor.setUrl(windowSecurityMembershipEditorUrl);
                windowSecurityMembershipEditor.show();
            }
        }

        function OnAddSecurityRole() {
            var windowSecurityRoleEditor = getWindowSecurityRoleEditor();
            var windowSecurityRoleEditorUrl = siteUrl + "/Administration/WindowSecurityRoleEditor.aspx?SecurityRoleId=";
            if (windowSecurityRoleEditor != null) {
                windowSecurityRoleEditor.setUrl(windowSecurityRoleEditorUrl);
                windowSecurityRoleEditor.show();
            }
        }

        function OnAddSecurityRolePermission() {
            var windowSecurityRolePermissionEditor = getWindowSecurityRolePermissionEditor();
            var windowSecurityRolePermissionEditorUrl = siteUrl + "/Administration/WindowSecurityRolePermissionEditor.aspx?SecurityRolePermissionId=";
            if (windowSecurityRolePermissionEditor != null) {
                windowSecurityRolePermissionEditor.setUrl(windowSecurityRolePermissionEditorUrl);
                windowSecurityRolePermissionEditor.show();
            }
        }

        function OnToolBarClientButtonClicked(s, e) {

            var callbackPanel = $find("<%= this.ajaxPanelSecurities.ClientID %>");
            var commandName = e.get_item().get_commandName();
            var commandArgument = e.get_item().get_commandArgument();
            var arguments = commandName + ',' + commandArgument;

            function OnDeactivateMembershipHandler(args) {
                if (args) {
                    callbackPanel.ajaxRequest("DEACTIVATE_MEMBERSHIP," + commandArgument);
                }
            }

            function OnDeactivateRoleHandler(args) {
                if (args) {
                    callbackPanel.ajaxRequest("DEACTIVATE_ROLE," + commandArgument);
                }
            }

            switch (commandName) {
                case 'EDIT_MEMBERSHIP':
                    var windowSecurityMembershipEditor = getWindowSecurityMembershipEditor();
                    var windowSecurityMembershipEditorUrl = siteUrl + "/Administration/WindowSecurityMembershipEditor.aspx?SecurityMembershipId=" + commandArgument;
                    if (windowSecurityMembershipEditor != null) {
                        windowSecurityMembershipEditor.setUrl(windowSecurityMembershipEditorUrl);
                        windowSecurityMembershipEditor.show();
                    }
                    break;
                case 'DEACTIVATE_MEMBERSHIP':
                    var message = "Are you sure you want to deactivate the site membership <span style='font-weight: bold;'>" + e.accountName + "</span>?<br /><br />";
                    radconfirm(message, OnDeactivateMembershipHandler, 500, 200, null, 'Deactivate Membership Confirmation');
                    break;
                case 'EDIT_ROLE':
                    var windowSecurityRoleEditor = getWindowSecurityRoleEditor();
                    var windowSecurityRoleEditorUrl = siteUrl + "/Administration/WindowSecurityRoleEditor.aspx?SecurityRoleId=" + commandArgument;
                    if (windowSecurityRoleEditor != null) {
                        windowSecurityRoleEditor.setUrl(windowSecurityRoleEditorUrl);
                        windowSecurityRoleEditor.show();
                    }
                    break;
                case 'DEACTIVATE_ROLE':
                    var message = "Are you sure you want to deactivate the site role <span style='font-weight: bold;'>" + e.name + "</span>?<br /><br />";
                    radconfirm(message, OnDeactivateRoleHandler, 500, 200, null, 'Deactivate Role Confirmation');
                    break;
                default:
            }
        }

        function OnRefreshSecuritytMemberships() {
            refreshSecurityMembershipList();
        }

        function OnRefreshSecuritytRoles() {
            refreshSecurityRoleList();
        }

        function refreshSecurityMembershipList() {
            var ajaxPanelSecurities = $find("<%= this.ajaxPanelSecurities.ClientID %>");
            ajaxPanelSecurities.ajaxRequest('REFRESH,');
        }

        function refreshSecurityRoleList() {
            var ajaxPanelSecurities = $find("<%= this.ajaxPanelSecurities.ClientID %>");
            ajaxPanelSecurities.ajaxRequest('REFRESH,');
        }

        function refreshSecurityRolePermissionList() {
            var ajaxPanelSecurities = $find("<%= this.ajaxPanelSecurities.ClientID %>");
            ajaxPanelSecurities.ajaxRequest('REFRESH,');
        }

    </script>
</telerik:RadCodeBlock>
<style type="text/css">
    
    .linkButton .rbText
    {
        text-decoration: underline !important;
        color: #3966bf !important;
        cursor: hand;
    }

    .optionsDivRt {
        right: 5px;
    }

    .CloseButton
    {
        background-image: none !important;
        background-color: transparent !important;
        border: none !important;
    }

    .CloseButton .rbPrimaryIcon {
        width: 24px;
        height: 24px;
        top: 6px;
        left: 10px;
    }

    .CloseButton .rbText {
        margin-top: 0px;
        margin-left: -3px;
        color: #666;
        font-weight: bold;
    }

    .linkButton .rbText
    {
        text-decoration: underline !important;
        color: #3966bf !important;
        cursor: hand;
    }
</style>
<!--[if IE]>
    <style type="text/css">
        .optionsDivRt {
            right: 25px;
        }
    </style>
<![endif]-->
<Vmgr:WindowDirectorySearchControl
    runat="Server"
    ID="windowDirectorySearchControl"
    ></Vmgr:WindowDirectorySearchControl>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelSecurities" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelSecurities"
    Width="100%"
    LoadingPanelID="ajaxLoadingPanelSecurities"
    OnAjaxRequest="ajaxPanelSecurities_AjaxRequest"
    >
    <div style="padding: 5px; padding-bottom: 0px; z-index:1000; position:relative;">
        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
            <div style="position: absolute; top: 5px; width: 100%;">
                <telerik:RadTabStrip 
                    runat="server" 
                    ID="tabStripSecurities" 
                    MultiPageID="multipageSecurities" 
                    SelectedIndex="0"
                    CausesValidation="false"
                    >
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Memberships" PageViewID="pageViewMemberships">
                        </telerik:RadTab>
                        <telerik:RadTab runat="server" Text="Roles" PageViewID="pageViewSecurityRoles">
                        </telerik:RadTab>
                        <telerik:RadTab runat="server" Text="Permissions" PageViewID="pageViewSecurityPermissions">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
            <div style="position: absolute; top: 0px; right: 0px;">
                <style type="text/css">
                    .KillButton
                    {
                        background-image: none !important;
                        background-color: transparent !important;
                        border: none !important;
                    }

                    .KillButton .rbPrimaryIcon {
                        width: 24px;
                        height: 24px;
                    }

                    .KillButton .rbText {
                        margin-top: 5px;
                        margin-left: 5px;
                        color: #666;
                        font-weight: bold;
                    }
                </style>
			    <telerik:RadButton 
                    ID="buttonOpenOptions"
                    runat="server"
                    Text="Security Options"
                    ButtonType="LinkButton"
                    AutoPostBack="false"
                    CssClass="KillButton"
                    OnClientClicked="OnOpenOptions"
                    >
                    <Icon PrimaryIconUrl="/_layouts/images/Vmgr/options-icon-24.png" />
                </telerik:RadButton>
            </div>
        </div>
    </div>
    <div style="padding: 5px; padding-top: 0px; z-index:10; position:relative;">
        <div style="border: solid 1px gray; border-top: none; background: white; font-size: 8pt;">
            <table style="border-collapse: collapse; width: 100%;">
                <tr>
                    <td style="border-collapse: collapse; vertical-align: top; padding: 0px;">
                        <telerik:RadMultiPage 
                            runat="server" 
                            ID="multipageSecurities" 
                            RenderSelectedPageOnly="false"
                            SelectedIndex="0" 
                            >    
                            <telerik:RadPageView 
                                runat="server" 
                                ID="pageViewMemberships"
                                >
                                <div style="padding: 10px;">
                                    Memberships connect users or groups of users to a role.
                                </div>
                                <telerik:RadGrid 
                                    runat="server" 
                                    ID="gridSecurityMembership" 
                                    DataSourceID="linqDataSourceSecurityMembership"
                                    AllowPaging="True" 
                                    AllowSorting="true"
                                    AllowFilteringByColumn="false"
                                    PageSize="25" 
                                    Width="100%"
                                    Border="0"
                                    AutoGenerateColumns="false"
                                    AllowMultiRowSelection="true"
                                    OnItemCreated="gridSecurityMembership_ItemCreated"
                                    >
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="False"></Selecting>
                                    </ClientSettings>
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" BorderStyle="None" />
                                    <GroupingSettings CaseSensitive="false" />  
                                    <MasterTableView 
                                        DataKeyNames="SecurityMembershipId" 
                                        TableLayout="Auto"
                                        >
                                        <NoRecordsTemplate>
                                            There are no records available
                                        </NoRecordsTemplate>
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Role" FieldName="SecurityRoleName"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="SecurityRoleId" SortOrder="Ascending"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                        <SortExpressions>
                                            <telerik:GridSortExpression FieldName="Account" SortOrder="Ascending" />
                                        </SortExpressions>
                                        <Columns>
                                            <telerik:GridTemplateColumn  AutoPostBackOnFilter="true"  
                                                DataField="Account" 
                                                HeaderText="Account"
                                                UniqueName="Account"
                                                SortExpression="Account"
                                                CurrentFilterFunction="Contains" 
                                                >
                                                <ItemTemplate>
                                                    <telerik:RadToolBar 
                                                        runat="server" 
                                                        ID="toolBar" 
                                                        Width="100%"
                                                        EnableRoundedCorners="false"
                                                        CollapseAnimation-Type="None"
                                                        CollapseAnimation-Duration="0"
                                                        ExpandAnimation-Type="None"
                                                        ExpandAnimation-Duration="0"
                                                        OnClientButtonClicked="OnToolBarClientButtonClicked"
                                                        >
                                                        <Items>
                                                            <telerik:RadToolBarDropDown
                                                                ImagePosition="Left"
                                                                Width="200px"
                                                                >
                                                                <Buttons>
                                                                    <telerik:RadToolBarButton 
                                                                        CommandName="EDIT_MEMBERSHIP"
                                                                        ImageUrl="/_layouts/images/Vmgr/edit-icon-16.png" 
                                                                        Text="Edit membership" 
                                                                        Visible="true"
                                                                        />
                                                                    <telerik:RadToolBarButton 
                                                                        CommandName="DEACTIVATE_MEMBERSHIP"
                                                                        ImageUrl="/_layouts/images/Vmgr/trash-can-icon-16.png" 
                                                                        Text="Deactivate membership" 
                                                                        Visible="true"
                                                                        ></telerik:RadToolBarButton>
                                                                </Buttons>
                                                            </telerik:RadToolBarDropDown>
                                                        </Items>
                                                    </telerik:RadToolBar>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="SecurityRoleName" 
                                                SortExpression="SecurityRoleName"
                                                HeaderText="Role"
                                                >
			                                </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="AccountType" 
                                                SortExpression="AccountType"
                                                HeaderText="Type"
                                                >
                                                <ItemTemplate>
                                                    <asp:Label
                                                        runat="server"
                                                        ID="labelAccountType"
                                                        ></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
			                                <telerik:GridBoundColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="CreateDate" 
                                                HeaderText="Created"
                                                SortExpression="CreateDate"
                                                >
			                                </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <asp:LinqDataSource 
                                    runat="server" 
                                    ID="linqDataSourceSecurityMembership" 
                                    OnSelecting="linqDataSourceSecurityMembership_Selecting"
                                    />    
                            </telerik:RadPageView>
                            <telerik:RadPageView 
                                runat="server" 
                                ID="pageViewSecurityRoles"
                                >
                                <div style="padding: 10px;">
                                    Roles contain a set of permissions that limits what a user can do.
                                </div>
                                <telerik:RadGrid 
                                    runat="server" 
                                    ID="gridSecurityRole" 
                                    DataSourceID="linqDataSourceSecurityRole"
                                    AllowPaging="True" 
                                    AllowSorting="true"
                                    AllowFilteringByColumn="false"
                                    PageSize="25" 
                                    Width="100%"
                                    Border="0"
                                    AutoGenerateColumns="false"
                                    AllowMultiRowSelection="true"
                                    OnItemCreated="gridSecurityRole_ItemCreated"
                                    >
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="False"></Selecting>
                                    </ClientSettings>
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" BorderStyle="None" />
                                    <GroupingSettings CaseSensitive="false" />  
                                    <MasterTableView 
                                        DataKeyNames="SecurityRoleId" 
                                        TableLayout="Auto"
                                        >
                                        <NoRecordsTemplate>
                                            There are no records available
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn  AutoPostBackOnFilter="true"  
                                                DataField="Name" 
                                                HeaderText="Role"
                                                UniqueName="Name"
                                                SortExpression="Name"
                                                CurrentFilterFunction="Contains" 
                                                >
                                                <ItemTemplate>
                                                    <telerik:RadToolBar 
                                                        runat="server" 
                                                        ID="toolBar" 
                                                        Width="100%"
                                                        EnableRoundedCorners="false"
                                                        CollapseAnimation-Type="None"
                                                        CollapseAnimation-Duration="0"
                                                        ExpandAnimation-Type="None"
                                                        ExpandAnimation-Duration="0"
                                                        OnClientButtonClicked="OnToolBarClientButtonClicked"
                                                        >
                                                        <Items>
                                                            <telerik:RadToolBarDropDown
                                                                ImagePosition="Left"
                                                                Width="150px"
                                                                >
                                                                <Buttons>
                                                                    <telerik:RadToolBarButton 
                                                                        CommandName="EDIT_ROLE"
                                                                        ImageUrl="/_layouts/images/Vmgr/edit-icon-16.png" 
                                                                        Text="Edit role" 
                                                                        Visible="true"
                                                                        />
                                                                    <telerik:RadToolBarButton 
                                                                        CommandName="DEACTIVATE_ROLE"
                                                                        ImageUrl="/_layouts/images/Vmgr/trash-can-icon-16.png" 
                                                                        Text="Deactivate role" 
                                                                        Visible="true"
                                                                        ></telerik:RadToolBarButton>
                                                                </Buttons>
                                                            </telerik:RadToolBarDropDown>
                                                        </Items>
                                                    </telerik:RadToolBar>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
			                                <telerik:GridBoundColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="Description" 
                                                HeaderText="Description"
                                                >
			                                </telerik:GridBoundColumn>
			                                <telerik:GridBoundColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="CreateDate" 
                                                HeaderText="Created"
                                                >
			                                </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <asp:LinqDataSource 
                                    runat="server" 
                                    ID="linqDataSourceSecurityRole" 
                                    OnSelecting="linqDataSourceSecurityRole_Selecting"
                                    />       
                            </telerik:RadPageView>
                            <telerik:RadPageView 
                                runat="server" 
                                ID="pageViewSecurityPermissions"
                                >
                                <div style="padding: 10px;">
                                    Permissions for each role.  Select 'Security Options' to manage a permission set.
                                </div>
                                <telerik:RadGrid 
                                    runat="server" 
                                    ID="gridSecurityRolePermission" 
                                    DataSourceID="linqDataSourceSecurityRolePermission"
                                    AllowPaging="True" 
                                    AllowSorting="true"
                                    AllowFilteringByColumn="false"
                                    PageSize="25" 
                                    Width="100%"
                                    Border="0"
                                    AutoGenerateColumns="false"
                                    AllowMultiRowSelection="true"
                                    OnItemCreated="gridSecurityRolePermission_ItemCreated"
                                    >
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="False"></Selecting>
                                    </ClientSettings>
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" BorderStyle="None" />
                                    <GroupingSettings CaseSensitive="false" />  
                                    <MasterTableView 
                                        DataKeyNames="SecurityRolePermissionId" 
                                        TableLayout="Auto"
                                        >
                                        <NoRecordsTemplate>
                                            There are no records available
                                        </NoRecordsTemplate>
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Role" FieldName="SecurityRoleName"></telerik:GridGroupByField>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="SecurityRoleId" SortOrder="Ascending"></telerik:GridGroupByField>
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                        <SortExpressions>
                                            <telerik:GridSortExpression FieldName="SecurityPermissionId" SortOrder="Ascending" /> 
                                        </SortExpressions>
                                        <Columns>
                                            <telerik:GridTemplateColumn  AutoPostBackOnFilter="true" DataField="Active" HeaderText="Active" HeaderStyle-Width="16px">
                                                <ItemTemplate>
                                                    <asp:Image
                                                        runat="server"
                                                        ID="imageActive"
                                                        />
                                                </ItemTemplate>
			                                </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="SecurityPermissionName" 
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
                                                DataField="SecurityPermissionDescription" 
                                                HeaderText="Description" 
                                                >
			                                </telerik:GridBoundColumn>
			                                <telerik:GridBoundColumn 
                                                CurrentFilterFunction="Contains" 
                                                AutoPostBackOnFilter="true"  
                                                DataField="CreateDate" 
                                                HeaderText="Created"
                                                >
			                                </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <asp:LinqDataSource 
                                    runat="server" 
                                    ID="linqDataSourceSecurityRolePermission" 
                                    OnSelecting="linqDataSourceSecurityRolePermission_Selecting"
                                    />       
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </td>
                    <td 
                        id="optionsTd" 
                        runat="server" 
                        style="
                            width: 250px; 
                            vertical-align: top; 
                            border-left: solid 1px gray; 
                            padding: 0px;
                            background-color: #FAFAFA;
                        "
                        >
                        <div id="optionsDiv" style="overflow-y: auto; overflow-x: hidden; border-collapse: collapse; position: relative; width:100%; height: 200px;">
                            <div class="optionsDivRt" style="position: absolute; top: 3px;">
			                    <telerik:RadButton 
                                    ID="buttonCloseOptions"
                                    runat="server"
                                    Text="Close"
                                    ButtonType="LinkButton"
                                    AutoPostBack="false"
                                    CssClass="CloseButton"
                                    OnClientClicked="OnCloseOptions"
                                    >
                                    <Icon PrimaryIconUrl="/_layouts/images/Vmgr/cancel-icon-8.png" />
                                </telerik:RadButton>
                            </div>
                            <div style="padding: 5px; position: absolute; top: 10px; left: 0px;">
                                <style type="text/css">
                                    .RadTreeView .rtLI
                                    {
                                        padding-bottom: 8px;
                                    }
                                    .RadTreeView .rtUL .rtUL
                                    {
                                        margin-top: 8px;
                                    }
                                    .RadTreeView .rtLast
                                    {
                                        padding-bottom: 0; 
                                    }

                                    .RadTreeView .rtPlus, 
                                    .RadTreeView .rtMinus
                                    {
                                        display: none !important;
                                    }                                            

                                </style>
                                <telerik:RadTreeView 
                                    ID="treeViewOptions" 
                                    runat="server" 
                                    Width="100%" 
                                    OnClientLoad="OptionsOnLoad"
                                    OnClientNodeClicked="OnOptionSelected"
                                    style="overflow-x: hidden;"
                                    >
                                    <Nodes>
                                        <telerik:RadTreeNode
                                            ImageUrl="/_layouts/images/Vmgr/refresh-icon-24.png"
                                            Text="Refresh security"
                                            style="padding: 3px;"
                                            Value="REFRESH"
                                            >
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode
                                            ImageUrl="/_layouts/images/Vmgr/add-icon-24.png"
                                            Text="Add a membership"
                                            style="padding: 3px;"
                                            Value="ADD_MEMBERSHIP"
                                            >
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode
                                            ImageUrl="/_layouts/images/Vmgr/add-icon-24.png"
                                            Text="Add a role"
                                            style="padding: 3px;"
                                            Value="ADD_ROLE"
                                            >
                                        </telerik:RadTreeNode>
                                        <telerik:RadTreeNode
                                            ImageUrl="/_layouts/images/Vmgr/unlocked-icon-24.png"
                                            Text="Manage permission set"
                                            style="padding: 3px;"
                                            Value="MANAGE_PERMISSION_SET"
                                            >
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</telerik:RadAjaxPanel>