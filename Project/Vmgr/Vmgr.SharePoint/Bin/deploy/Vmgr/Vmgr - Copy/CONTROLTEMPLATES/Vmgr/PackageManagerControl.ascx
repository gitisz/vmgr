<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="PackageManagerControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.PackageManagerControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        var assemblyTabFunctionArray = new FunctionArray();
        var fixColumnFunctionArray = new FunctionArray();
        
        var selectedPackageUniqueId = "";

        function OnConfirmDeletePackage(s, e) {
            selectedPackageUniqueId = e.get_commandArgument();
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('CONFIRM_DELETE_PACKAGE,' + selectedPackageUniqueId);
        }

        function OnConfirmDeletePackageHandler(args) {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            if (args == true) {
                ajaxPanelPackageManager.ajaxRequest('DELETE_PACKAGE,' + selectedPackageUniqueId);
            }
        }

        function OnDeletePackageComplete(s, e) {
            <%= this.javascriptRedirectUrl %>; 
        }

        function OnRefreshPackages() {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('REFRESH');
        }

        function OnConfirmPauseResumePackage(s, e) {
            selectedPackageUniqueId = e.get_commandArgument();
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('CONFIRM_PAUSE_RESUME_PACKAGE,' + selectedPackageUniqueId);
        }

        function OnConfirmPauseResumePackageHandler(args) {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            if (args == true) {
                ajaxPanelPackageManager.ajaxRequest('PAUSE_RESUME_PACKAGE,' + selectedPackageUniqueId);
            }
        }

        function OnPauseResumePackageComplete(s, e) {
        }

        function OnOpenOptions(s, e) {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('OPEN_OPTIONS,');
        }

        function OnCloseOptions() {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('CLOSE_OPTIONS,');
        }

        function OnApplyFilter(filter) {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest(filter);
        }

        function OnFilterSaved() {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnFilterDeleted() {
            var ajaxPanelPackageManager = $find('<%= ajaxPanelPackageManager.ClientID %>');
            ajaxPanelPackageManager.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnPackageGridCreated(s, e) {

            var height = s.get_masterTableView().get_element().clientHeight;

            var optionsDiv = $get('optionsDiv');

            if (optionsDiv != undefined)
                optionsDiv.style.minHeight = getViewportHeight() - 170 + 'px';
        }

        function OnOptionSelected(s, e) {

            var itemValue = s.get_selectedNode().get_value();

            if (itemValue == 'UPLOAD_PACKAGE') {
                OnShowUploadPackage();

                var node = s.get_selectedNode();
                node.unselect();
            }

            if (itemValue == 'MANAGE_FILTERS') {
                OnShowFilter();
            }

            if (itemValue.indexOf("APPLY_FILTER") >= 0) {
                OnApplyFilter(itemValue);
            }
        }

        function OptionsOnLoad(s, e) {
            var nodes = s.get_allNodes();
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].expand();
                }
            }
        }

        function UpdateFilterExpDiv() {
            var windowWidth = getViewportWidth();
            $("#fiterExpressionDiv").width(windowWidth - 450);
            $("#ellipsesDiv").width(windowWidth - 465);
            $("#ellipsesDiv").height(16);
        }

        function OnUpdateFilterExpDiv() { UpdateFilterExpDiv(); Sys.Application.remove_load(OnUpdateFilterExpDiv); }

        Sys.Application.add_load(OnUpdateFilterExpDiv);

        $(document).ready(function() {
            UpdateFilterEllipses();
        }
        )
        ;

        function UpdateFilterEllipses() {
            $("#ellipsesDiv").dotdotdot({
                ellipsis: '... ',
                watch: true
            });
        }

        function OnPackageManagerResposeEnd(s, e) {

            fixColumnFunctionArray.execute('COLFIX_SCRIPT', null);

            UpdateFilterExpDiv();
            UpdateFilterEllipses();
        }

        addEvent(window, "resize", UpdateFilterExpDiv);

     </script>
</telerik:RadCodeBlock>

<style type="text/css">

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
</style>
<!--[if IE]>
    <style type="text/css">
        .optionsDivRt {
            right: 25px;
        }
    </style>
<![endif]-->
    <telerik:RadAjaxLoadingPanel 
        runat="server"
        ID="ajaxLoadingPanelPackageManager" 
        Skin="Default" 
        ZIndex="2999"
        ></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel
        runat="server"
        ID="ajaxPanelPackageManager"
        Width="100%"
        LoadingPanelID="ajaxLoadingPanelPackageManager"
        OnAjaxRequest="ajaxPanelPackageManager_AjaxRequest"
        ClientEvents-OnResponseEnd="OnPackageManagerResposeEnd"
        >
        <div style="padding: 5px; padding-bottom: 0px; z-index:1000; position:relative;">
            <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
                <div style="position: absolute; top: 5px; width: 100%;">
                    <telerik:RadTabStrip 
                        runat="server" 
                        ID="tabStripPackageManager" 
                        MultiPageID="multipagePackageManager" 
                        SelectedIndex="0"
                        CausesValidation="false"
                        >
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Packages" PageViewID="pageViewPackages">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                </div>
                <style type="text/css">
                    .rfPreview {
                        padding-top: 0px !important;
                        min-height: 18px;
                        font-size: 8pt;
                    }
                    
                    .RadFilter:after{
                        visibility: hidden;
                        visibility: collapse !important;
                    }

                    .RadFilter_Default .rfPreview STRONG {
	                    COLOR: #25800b
                    }

                    .RadFilter_Default .rfPreview EM {
	                    COLOR: #0043dc
                    }

                    .RadFilter_Default .rfPreview .rfBr {
	                    COLOR: #0043dc
                    }

                </style>
                <div id="fiterExpressionDiv" style="position: absolute; top: 10px; left: 100px; height: 16px;">
                    <table style="width: 100%; border-collapse: collapse; padding: 0px;">
                        <tr>
                            <td style="padding: 0px; width: 20px;">
                                <telerik:RadButton 
                                    runat="server" 
                                    ID="buttonClearFilter"
                                    AutoPostBack="false"
                                    ToolTip="Clear filter"
                                    Visible="false"
                                    Width="16px"
                                    Height="16px"
                                    OnClientClicked="function(s, e){ OnApplyFilter('APPLY_FILTER,0'); }"
                                    >
                                    <Image 
                                        ImageUrl="/_layouts/Images/Vmgr/filter-remove-icon-16.png" />
                                </telerik:RadButton>
                            </td>
                            <td id="fiterExpressionTd">
                                <div id="ellipsesDiv" class="RadFilter RadFilter_Default rfPreview">
                                    <asp:Literal
                                        runat="server"
                                        ID="literalPackageFilterExpression"
                                        ></asp:Literal>
                                </div>
                            </td>
                        </tr>
                    </table>
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
                        Text="Package Options"
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
                <telerik:RadMultiPage 
                    runat="server" 
                    ID="multipagePackageManager" 
                    SelectedIndex="0" 
                    RenderSelectedPageOnly="false"
                    >
                    <telerik:RadPageView runat="server" ID="pageViewPackages">   
                        <table style="border-collapse: collapse; width: 100%;">
                            <tr>
                                <td style="border-collapse: collapse; vertical-align: top; padding: 0px;">
                                    <asp:LinqDataSource
                                        runat="server"
                                        ID="linqDataSourcePackage"
                                        OnSelecting="linqDataSourcePackage_Selecting"
                                        ></asp:LinqDataSource>
                                    <telerik:RadGrid 
                                        runat="server" 
                                        ID="gridPackage" 
                                        DataSourceID="linqDataSourcePackage"
                                        GridLines="None" 
                                        BorderStyle="None"
                                        AutoGenerateColumns="false"
                                        Width="100%" 
                                        AllowSorting="True" 
                                        AllowPaging="True" 
                                        AllowCustomPaging="true"
                                        PageSize="5" 
                                        OnItemDataBound="gridPackage_ItemDataBound" 
                                        style="border-collapse: collapse;"
                                        >
                                        <MasterTableView 
                                            AllowPaging="true" 
                                            AllowCustomPaging="true"
                                            AutoGenerateColumns="false" 
                                            HierarchyLoadMode="ServerOnDemand" 
						                    DataKeyNames="PackageId" 
                                            >
                                            <Columns>
                                                <telerik:GridTemplateColumn
                                                    UniqueName="Name" 
                                                    HeaderText="Package Name"
                                                    HeaderStyle-Width="90%" 
                                                    SortExpression="Name"
                                                    >
                                                    <ItemTemplate>
                                                        <script type="text/javascript">
                                                            fixColumnFunctionArray.add("COLFIX_SCRIPT", function () {
                                                                var tablecol = $get('<%# this.gridPackage.ClientID + "_ctl00" %>');
                                                                $(tablecol).find('col:eq(0)').css('width', '30px');
                                                            }
                                                            )
                                                            ;

                                                            fixColumnFunctionArray.execute('COLFIX_SCRIPT', null);
                                                        </script>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; border-collapse: collapse;">
                                                            <tr>
                                                                <td rowspan="2" style="width: 32px;">
                                                                    <asp:Image
                                                                        runat="server"
                                                                        ID="imagePackageIcon"
                                                                        ImageUrl="/_layouts/images/Vmgr/nav-packagemgr-enabled-32.png"
                                                                        />
                                                                </td>
                                                                <td>
                                                                    <asp:HyperLink
                                                                        runat="server"
                                                                        ID="hyperLinkPackageDownload"
                                                                        ToolTip="Download this package."
                                                                        >
                                                                        <span style="font-weight: bold;"><%# Eval("Name") %></span></asp:HyperLink> 
                                                                        &nbsp;<asp:Label
                                                                            runat="server"
                                                                            ID="labelUniqueId"
                                                                            >{<%# Eval("UniqueId") %>}</asp:Label></span>  
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span style="font-style: italic;">
                                                                    <asp:Label
                                                                            runat="server"
                                                                            ID="labelDescription"
                                                                            ><%# Eval("Description") %></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn
                                                    HeaderStyle-Width="80px" 
                                                    ItemStyle-VerticalAlign="Top"
                                                    ItemStyle-HorizontalAlign="Right"
                                                    >
                                                    <ItemTemplate>
                                                        <table border="0" cellpadding="3px" cellspacing="3px" style="width: 80px;">
                                                            <tr>
                                                                <td valign="top">
                                                                    <telerik:RadButton 
                                                                        runat="server" 
                                                                        ID="buttonMovePackage"
                                                                        AutoPostBack="false"
                                                                        ToolTip="Move package"
                                                                        Visible="true"
                                                                        Width="16px"
                                                                        Height="16px"
                                                                        OnClientClicked="OnShowMovePackage"
                                                                        >
                                                                        <Image 
                                                                            ImageUrl="/_layouts/Images/Vmgr/page-swap-icon-16.png" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadButton 
                                                                        runat="server" 
                                                                        ID="buttonPauseResume"
                                                                        AutoPostBack="false"
                                                                        ToolTip="Pause package"
                                                                        Visible="true"
                                                                        Width="16px"
                                                                        Height="16px"
                                                                        OnClientClicked="OnConfirmPauseResumePackage"
                                                                        >
                                                                        <Image 
                                                                            ImageUrl="/_layouts/Images/Vmgr/pause-icon-16.png" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadButton 
                                                                        runat="server" 
                                                                        ID="buttonDelete"
                                                                        AutoPostBack="false"
                                                                        ToolTip="Delete package"
                                                                        Visible="true"
                                                                        Width="16px"
                                                                        Height="16px"
                                                                        OnClientClicked="OnConfirmDeletePackage"
                                                                        >
                                                                        <Image 
                                                                            ImageUrl="/_layouts/Images/Vmgr/trash-can-icon-16.png" />
                                                                    </telerik:RadButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <NestedViewTemplate>
                                                <telerik:RadAjaxLoadingPanel 
                                                    runat="server"
                                                    ID="ajaxLoadingPanelAssembly" 
                                                    Transparency="100"
                                                    ZIndex="2999"
                                                    IsSticky="true"
                                                    ></telerik:RadAjaxLoadingPanel>
                                                <telerik:RadAjaxPanel
                                                    runat="server"
                                                    ID="ajaxPanelAssembly"
                                                    Width="100%"
                                                    LoadingPanelID="ajaxLoadingPanelAssembly"
                                                    OnAjaxRequest="ajaxPanelAssembly_AjaxRequest"
                                                    >
                                                    <div style="padding: 5px; padding-bottom: 0px;">
                                                        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
                                                            <div style="position: absolute; top: 5px;">
                                                                <telerik:RadTabStrip 
                                                                    runat="server" 
                                                                    ID="tabStripPackageResource" 
                                                                    MultiPageID="multipagePackageResource" 
                                                                    SelectedIndex="0"
                                                                    >
                                                                    <Tabs>
                                                                        <telerik:RadTab runat="server" Text="Plugins" PageViewID="pageViewPlugin">
                                                                        </telerik:RadTab>
                                                                        <telerik:RadTab runat="server" Text="Assemblies" PageViewID="pageViewAssembly" >
                                                                        </telerik:RadTab>
                                                                    </Tabs>
                                                                </telerik:RadTabStrip>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div style="padding: 5px; padding-top: 0px;">
                                                        <div style="border: solid 1px gray; border-top: none; background: white; font-size: 8pt;">
                                                            <telerik:RadMultiPage 
                                                                runat="server" 
                                                                ID="multipagePackageResource" 
                                                                RenderSelectedPageOnly="false"
                                                                SelectedIndex="0" 
                                                                >    
                                                                <telerik:RadPageView 
                                                                    runat="server" 
                                                                    ID="pageViewPlugin"
                                                                    >
                                                                    <asp:Label 
                                                                        runat="server" 
                                                                        ID="labelPackageId" 
                                                                        Visible="false"
                                                                        Text='<%# Eval("PackageId") %>'></asp:Label> 
                                                                    <asp:LinqDataSource 
                                                                        runat="server" 
                                                                        ID="linqDataSourcePlugin" 
                                                                        OnSelecting="linqDataSourcePlugin_Selecting"
                                                                        Where="PackageId == Int32(@PackageId)"
                                                                        >
                                                                        <WhereParameters>
                                                                            <asp:ControlParameter 
                                                                                ControlID="labelPackageId" 
                                                                                Name="PackageId" 
                                                                                PropertyName="Text" 
                                                                                Type="Int32"
                                                                                /> 
                                                                        </WhereParameters>
                                                                    </asp:LinqDataSource>
					                                                <telerik:RadGrid 
						                                                runat="server" 
						                                                ID="gridPlugin"
                                                                        DataSourceID="linqDataSourcePlugin"
						                                                AllowSorting="true"
						                                                AllowPaging="false" 
						                                                AllowFilteringByColumn="false" 
                                                                        AllowMultiRowSelectolumns="false" 
						                                                ShowStatusBar="true"
                                                                        ShowFooter="true"
                                                                        BorderStyle="None"
                                                                        OnItemDataBound="gridPlugin_ItemDataBound"
						                                                >
                                                                        <ClientSettings>
                                                                            <Selecting AllowRowSelect="False"></Selecting>
                                                                        </ClientSettings>        
						                                                <MasterTableView 
							                                                DataKeyNames="PluginId" 
							                                                ShowHeader="true" 
							                                                AutoGenerateColumns="False" 
							                                                AllowPaging="true"
							                                                PageSize="10" 
							                                                HierarchyLoadMode="ServerOnDemand"
							                                                >
							                                                <Columns>
                                                                                <telerik:GridTemplateColumn
                                                                                    UniqueName="Name" 
                                                                                    HeaderText="Plugin Name"
                                                                                    HeaderStyle-Width="90%" 
                                                                                    SortExpression="Name"
                                                                                    >
                                                                                    <ItemTemplate>
                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td rowspan="2" style="width: 32px;">
                                                                                                    <asp:Image
                                                                                                        runat="server"
                                                                                                        ID="imagePluginIcon"
                                                                                                        ImageUrl="/_layouts/images/Vmgr/package-plugin-enabled-32.png"
                                                                                                        />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span style="font-weight: bold;"><%# Eval("Name") %></span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span style="">{<%# Eval("UniqueId") %>}</span>                                                    
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <span style="font-style: italic;"><%# Eval("Description") %></span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridBoundColumn
                                                                                    UniqueName="Schedulable"
                                                                                    DataField="Schedulable"
                                                                                    HeaderText="Schedulable"
                                                                                    ItemStyle-VerticalAlign="Top"
                                                                                    ItemStyle-HorizontalAlign="Center"
                                                                                    ></telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn
                                                                                    UniqueName="UpdateDate"
                                                                                    DataField="UpdateDate"
                                                                                    HeaderText="Updated"
                                                                                    ItemStyle-VerticalAlign="Top"
                                                                                    ItemStyle-HorizontalAlign="Right"
                                                                                    ></telerik:GridBoundColumn>
							                                                </Columns>
                                                                            <NoRecordsTemplate>
                                                                                <p style="padding: 2px;">No dependencies found for this package.</p>
                                                                            </NoRecordsTemplate>
						                                                </MasterTableView>
					                                                </telerik:RadGrid>
                                                                </telerik:RadPageView>
                                                                <telerik:RadPageView 
                                                                    runat="server" 
                                                                    ID="pageViewAssembly"
                                                                    >
                                                                    <asp:Label 
                                                                        runat="server" 
                                                                        ID="labelPackageUniqueId" 
                                                                        Visible="false"
                                                                        Text='<%# Eval("UniqueId") %>'></asp:Label> 
                                                                    <asp:LinqDataSource 
                                                                        runat="server" 
                                                                        ID="linqDataSourceAssembly" 
                                                                        OnSelecting="linqDataSourceAssembly_Selecting"
                                                                        Where="Id == String(@Id)"
                                                                        >
                                                                        <WhereParameters>
                                                                            <asp:ControlParameter 
                                                                                ControlID="labelPackageUniqueId" 
                                                                                Name="Id" 
                                                                                PropertyName="Text" 
                                                                                Type="String"
                                                                                /> 
                                                                        </WhereParameters>
                                                                    </asp:LinqDataSource>
					                                                <telerik:RadGrid 
						                                                runat="server" 
						                                                ID="gridAssembly"
						                                                AllowSorting="true"
						                                                AllowFilteringByColumn="false" 
                                                                        AllowMultiRowSelectolumns="false" 
						                                                ShowStatusBar="true"
                                                                        ShowFooter="true"
                                                                        BorderStyle="None"
						                                                >
                                                                        <ClientSettings>
                                                                            <Selecting AllowRowSelect="False"></Selecting>
                                                                        </ClientSettings>        
						                                                <MasterTableView 
							                                                DataKeyNames="Name" 
							                                                ShowHeader="true" 
							                                                AutoGenerateColumns="False" 
							                                                AllowPaging="true"
							                                                PageSize="10" 
							                                                HierarchyLoadMode="ServerOnDemand"
                                                                            TableLayout="Auto"
							                                                >
							                                                <Columns>
                                                                                <telerik:GridBoundColumn
                                                                                    DataField="FullName" 
                                                                                    UniqueName="FullName" 
                                                                                    HeaderText="Assembly Name"
                                                                                    HeaderStyle-Width="70%" 
                                                                                    SortExpression="Name"
                                                                                    >
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn
                                                                                    DataField="CompanyName" 
                                                                                    UniqueName="CompanyName" 
                                                                                    HeaderText="Company Name"
                                                                                    SortExpression="CompanyName"
                                                                                    >
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn
                                                                                    DataField="AssemblyVersion" 
                                                                                    UniqueName="AssemblyVersion" 
                                                                                    HeaderText="Assembly Version"
                                                                                    SortExpression="AssemblyVersion"
                                                                                    >
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn
                                                                                    DataField="AssemblyFileVersion" 
                                                                                    UniqueName="AssemblyFileVersion" 
                                                                                    HeaderText="File Version"
                                                                                    SortExpression="AssemblyFileVersion"
                                                                                    >
                                                                                </telerik:GridBoundColumn>
							                                                </Columns>
                                                                            <NoRecordsTemplate>
                                                                                <p style="padding: 2px;">No assemblies found for this package.</p>
                                                                            </NoRecordsTemplate>
						                                                </MasterTableView>
					                                                </telerik:RadGrid>
                                                                </telerik:RadPageView>
                                                            </telerik:RadMultiPage>
                                                        </telerik:RadAjaxPanel>
                                                    </div>
                                                </div>
                                            </NestedViewTemplate>
                                            <NoRecordsTemplate>
                                                <div style="padding: 5px;">
                                                    No packages found.
                                                </div>
                                            </NoRecordsTemplate>
                                        </MasterTableView>
                                        <ClientSettings 
                                            AllowDragToGroup="false" 
                                            AllowColumnsReorder="false" 
                                            />
                                        <ClientSettings>
                                            <ClientEvents 
                                                OnGridCreated="OnPackageGridCreated"
                                                />
                                        </ClientSettings>
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
                                </td>
                                <td 
                                    id="optionsTd" 
                                    runat="server" 
                                    style="
                                        width: 250px; 
                                        vertical-align: top; 
                                        border-left: solid 1px gray; 
                                        overflow: hidden; 
                                        padding: 0px;
                                        background-color: #FAFAFA;
                                        "
                                    >
                                    <div id="optionsDiv" style="overflow-y: auto; overflow-x: hidden; border-collapse: collapse; position: relative;">
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
                                                OnLoad="treeViewOptions_Load"
                                                style="overflow-x: hidden;"
                                                >
                                                <Nodes>
                                                    <telerik:RadTreeNode
                                                        ImageUrl="/_layouts/images/Vmgr/add-icon-24.png"
                                                        Text="Upload a package"
                                                        style="padding: 3px;"
                                                        Value="UPLOAD_PACKAGE"
                                                        >
                                                    </telerik:RadTreeNode>
                                                    <telerik:RadTreeNode
                                                        ImageUrl="/_layouts/images/Vmgr/filter-icon-24.png"
                                                        Text="Manage my filters"
                                                        style="padding: 3px;"
                                                        Value="MANAGE_FILTERS"
                                                        >
                                                        <Nodes>
                                                            <telerik:RadTreeNode
                                                                Text="Clear filter"
                                                                ImageUrl="/_layouts/images/Vmgr/filter-remove-icon-16.png"
                                                                Value="APPLY_FILTER,0"
                                                                >
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                            </telerik:RadTreeView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
    </div>
</telerik:RadAjaxPanel>

