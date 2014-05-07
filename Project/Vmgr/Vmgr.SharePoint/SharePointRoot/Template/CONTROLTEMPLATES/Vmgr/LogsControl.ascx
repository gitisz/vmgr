<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="LogsControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.LogsControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="Vmgr" %>
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        function OnLogSelected(s, e) {
        }

        
        function OnAppDomainSelected(s, e) {
            var row = s.get_selectedItems()[0];
            var appDomainName = row.getDataKeyValue('Name');

            $.ajax({
                type: "POST",
                url: "<%= getAppDomainPluginsServiceUrl %>",
                cache: false,
                contentType: "application/json; charset=utf-8",
                data: "{ 'serverId':'<%= serverId %>', 'domainName': '" + appDomainName + "'}",
                dataType: "json",
                success: function (data, status) {
                    if (data.d) {
                        var gridData = data.d;
                        var tableView = $find("<%= gridAppDomainPlugins.ClientID %>").get_masterTableView();
                        tableView.set_dataSource(gridData);
                        tableView.dataBind();
                    }
                },
                error: function (xmlRequest) {
                }
            });

        }

        function BindAppDomains() {
            $.ajax({
                type: "POST",
                url: "<%= getAppDomainsServiceUrl %>",
                cache: false,
                contentType: "application/json; charset=utf-8",
                data: "{ 'serverId':'<%= serverId %>' }",
                dataType: "json",
                success: function (data, status) {
                    if (data.d) {
                        var gridData = data.d;
                        var tableView = $find("<%= gridAppDomains.ClientID %>").get_masterTableView();
                                tableView.set_dataSource(gridData);
                                tableView.dataBind();
                            }
                        },
                error: function (xmlRequest) {
                }
            });
        }

        function OnBindAppDomains() { BindAppDomains(); Sys.Application.remove_load(OnBindAppDomains); }

        Sys.Application.add_load(OnBindAppDomains);

    </script>
    <style type="text/css">
        .linkButton .rbText
        {
            text-decoration: underline !important;
            color: #3966bf !important;
            cursor: hand;
        }
    </style>
</telerik:RadCodeBlock>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelLogs" 
    Skin="Default" 
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelLogs"
    LoadingPanelID="ajaxLoadingPanelLogs"
    OnAjaxRequest="ajaxPanelLogs_AjaxRequest"
    >
    <div style="padding: 5px; padding-bottom: 0px;">
        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
            <div style="position: absolute; top: 5px; width: 100%;">
                <telerik:RadTabStrip 
                    runat="server" 
                    ID="tabStripLogs" 
                    MultiPageID="multipageLogs" 
                    SelectedIndex="0"
                    CausesValidation="false"
                    >
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Logs" PageViewID="pageViewLogs">
                        </telerik:RadTab>
                        <telerik:RadTab runat="server" Text="App Domains" PageViewID="pageViewAppDomains">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
            <table style="width: 100%; padding: 0px; border-spacing:0; border-collapse: collapse; float: right;">
                <tr>
                    <td></td>
                    <td style="text-align: right; vertical-align: top; width: 24px;">
                    </td>
                    <td style="text-align: right; vertical-align: top; width: 120px; white-space: nowrap; font-size: 9pt; padding-top: 3px; padding-right: 5px;">
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="padding: 5px; padding-top: 0px;">
        <div style="border: solid 1px gray; border-top: none; background: white; font-size: 8pt;">
            <telerik:RadMultiPage 
                runat="server" 
                ID="multipageLogs" 
                SelectedIndex="0" 
                RenderSelectedPageOnly="false"
                >
                <telerik:RadPageView
                    runat="server"
                    ID="pageViewLogs"
                    >
                    <telerik:RadGrid 
                        runat="server" 
                        ID="gridLogs" 
                        GridLines="None" 
                        BorderStyle="None"
                        AutoGenerateColumns="false"
                        Width="100%" 
                        AllowSorting="True" 
                        AllowFilteringByColumn="true"
                        AllowPaging="True" 
                        AllowCustomPaging="true"
                        PageSize="15" 
                        OnItemDataBound="gridLogs_ItemDataBound" 
                        OnNeedDataSource="gridLogs_NeedDataSource"
                        >
                        <MasterTableView 
                            AllowPaging="true" 
                            AllowCustomPaging="true"
                            AutoGenerateColumns="false" 
                            HierarchyLoadMode="ServerOnDemand" 
			                DataKeyNames="Id" 
                            >
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="ID" SortOrder="Descending" />
                            </SortExpressions>
                            <Columns>
                                <telerik:GridMaskedColumn
                                    UniqueName="Id" 
                                    DataField="Id" 
                                    HeaderText="ID"
                                    HeaderStyle-Width="5%" 
                                    SortExpression="Id"
                                    ShowFilterIcon="false" 
                                    AutoPostBackOnFilter="true" 
                                    FilterDelay="2000" 
                                    FilterControlWidth="50px" 
                                    CurrentFilterFunction="EqualTo"
                                    Mask="#######"
                                    >
                                </telerik:GridMaskedColumn>
                                <Vmgr:LevelFilteringColumn 
                                    DataField="Level" 
                                    FilterControlWidth="180px" 
                                    HeaderText="Level"
                                    >
                                    <ItemTemplate>
                                        <div style="width: 100%; text-align: center;"></div>
                                        <asp:Image
                                            runat="server"
                                            ID="imageLevel"
                                            ToolTip='<%# Eval("Level") %>'
                                            style="display: block; margin: 0 auto;"
                                            />
                                        </div>
                                    </ItemTemplate>
                                </Vmgr:LevelFilteringColumn>
                                <telerik:GridBoundColumn
                                    UniqueName="Message" 
                                    DataField="Message" 
                                    HeaderText="Message"
                                    SortExpression="Message"
                                    HeaderStyle-Width="85%" 
                                    ItemStyle-VerticalAlign="Top"
                                    ShowFilterIcon="false" 
                                    AutoPostBackOnFilter="true" 
                                    FilterDelay="2000" 
                                    FilterControlWidth="200px" 
                                    CurrentFilterFunction="Contains"
                                    >
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn
                                    UniqueName="CreateDate" 
                                    DataField="CreateDate" 
                                    HeaderText="Created"
                                    SortExpression="CreateDate"
                                    HeaderStyle-Width="5%" 
                                    ItemStyle-VerticalAlign="Top"
                                    ItemStyle-HorizontalAlign="Right"
                                    >
                                    <FilterTemplate>
                                        <telerik:RadDatePicker
                                            runat="server" 
                                            ID="datePickerCreateDate"
                                            AutoPostBack="true"
                                            OnSelectedDateChanged="datePickerCreateDate_SelectedDateChanged"
                                            SelectedDate='<%# this.filterCreateDate %>'
                                            >
                                            </telerik:RadDatePicker>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                <div style="padding: 5px;">
                                    No logs found.
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
                </telerik:RadPageView>
                <telerik:RadPageView
                    runat="server"
                    ID="pageViewAppDomains"
                    >
                    <div style="padding:5px;">
                        The following is a list of application domains that are currently running in the V-Manager service on: <%= (this.Page as BasePage).server.Name %>.
                    </div>
                    <telerik:RadGrid 
                        runat="server" 
                        ID="gridAppDomains" 
                        GridLines="None" 
                        BorderStyle="None"
                        AutoGenerateColumns="false"
                        Width="100%" 
                        AllowSorting="True" 
                        AllowPaging="True" 
                        AllowCustomPaging="true"
                        PageSize="5" 
                        >
                        <ClientSettings 
                            EnableRowHoverStyle="true"
                            >
                            <ClientEvents OnRowSelected="OnAppDomainSelected" />
                            <Selecting 
                                AllowRowSelect="false"
                                ></Selecting>
                        </ClientSettings>    
                        <MasterTableView 
                            AllowPaging="true" 
                            AllowCustomPaging="true"
                            AutoGenerateColumns="false" 
                            HierarchyLoadMode="Client" 
							DataKeyNames="Name" 
							ClientDataKeyNames="Name" 
                            >
                            <Columns>
                                <telerik:GridTemplateColumn
                                    UniqueName="Name" 
                                    DataField="Name" 
                                    HeaderText="Application Domain Name"
                                    HeaderStyle-Width="100%" 
                                    SortExpression="Name"
                                    >
                                    <ClientItemTemplate>
                                        #=Name#
                                    </ClientItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                <div style="padding: 5px;">
                                    No application domains were found.
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
                    <div style="padding:5px;">
                        Select a domain to view curently loaded plugins.
                    </div>
                    <telerik:RadGrid 
                        runat="server" 
                        ID="gridAppDomainPlugins" 
                        GridLines="None" 
                        BorderStyle="None"
                        AutoGenerateColumns="false"
                        Width="100%" 
                        AllowSorting="True" 
                        AllowPaging="True" 
                        AllowCustomPaging="true"
                        PageSize="5" 
                        >
                        <ClientSettings 
                            EnableRowHoverStyle="true"
                            >
                            <Selecting 
                                AllowRowSelect="True"
                                ></Selecting>
                        </ClientSettings>    
                        <MasterTableView 
                            AllowPaging="true" 
                            AllowCustomPaging="true"
                            AutoGenerateColumns="false" 
                            HierarchyLoadMode="Client" 
							DataKeyNames="Name" 
							ClientDataKeyNames="Name" 
                            >
                            <Columns>
                                <telerik:GridBoundColumn
                                    UniqueName="Name" 
                                    DataField="Name" 
                                    HeaderText="Plugin Name"
                                    HeaderStyle-Width="100%" 
                                    SortExpression="Name"
                                    >
                                </telerik:GridBoundColumn>
                            </Columns>
                            <NoRecordsTemplate>
                                <div style="padding: 5px;">
                                    No plugins found.
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
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
    </div>
</telerik:RadAjaxPanel>
