<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SchedulerControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.SchedulerControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">
        var selectedScheduleUniqueId = null;
        var scheduleFunctionArray = new FunctionArray();
        var signalrFunctionHubRegistry = new FunctionArray();
        var signalrSchedulerScriptLoaded = false;

        signalrFunctionHubRegistry.add('SCHEDULER_SCRIPT', function () {

            if (!signalrSchedulerScriptLoaded) {

                $.getScript('<%= (this.Page as BasePage).GetHubConnectionUrl() + "/signalr/hubs" %>')
                    .done(function (script, status) {

                        signalrSchedulerScriptLoaded = true;

                        var connectionSchedule = $.hubConnection("<%= (this.Page as BasePage).GetHubConnectionUrl() %>" + "/signalr");
                        var vmgrHubSchedule = connectionSchedule.createHubProxy('vmgrHub');

                        vmgrHubSchedule.on('addSchedule', function (key, data) {
                            scheduleFunctionArray.execute(key, data);
                        });

                        var start = function () {
                            connectionSchedule.start({ })
                                .then(function () {
                                    // Change the client somehow.
                                });
                            };

                        start();

                    }
                    )
                    .fail(function (jqxhr, settings, exception) {
                    }
                )
                ;
            }
        }
        )
        ;

        function OnEditSchedule(s, e) {
            OnShowScheduleEditor(s.get_commandArgument());
        }

        function OnPauseResumeSchedule(s, e) {
            selectedScheduleUniqueId = e.get_commandArgument();
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('CONFIRM_PAUSE_RESUME_SCHEDULE,' + selectedScheduleUniqueId);
        }

        function OnConfirmPauseResumeScheduleHandler(args) {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            if (args == true) {
                ajaxPanelSchedule.ajaxRequest('PAUSE_RESUME_SCHEDULE,' + selectedScheduleUniqueId);
            }
        }

        function OnPauseResumeScheduleComplete(s, e) {
            <%= this.javascriptRedirectUrl %>; 
        }

        function OnOpenOptions(s, e) {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('OPEN_OPTIONS,');
        }


        function OnCloseOptions() {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('CLOSE_OPTIONS,');
        }

        function OnApplyFilter(filter) {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest(filter);
        }

        function OnFilterSaved() {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnFilterDeleted() {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnDeleteSchedule(s, e) {
            selectedScheduleUniqueId = e.get_commandArgument();
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('CONFIRM_DELETE_SCHEDULE,' + selectedScheduleUniqueId);
        }

        function OnConfirmDeleteScheduleHandler(args) {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            if (args == true) {
                ajaxPanelSchedule.ajaxRequest('DELETE_SCHEDULE,' + selectedScheduleUniqueId);
            }
        }

        function OnDeleteScheduleComplete(s, e) {
            <%= this.javascriptRedirectUrl %>; 
        }

        function OnRefreshSchedules() {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('REFRESH,');
        }

        function OnScheduleGridCreated(s, e) {

            var height = s.get_masterTableView().get_element().clientHeight;

            var optionsDiv = $get('optionsDiv');

            if (optionsDiv != undefined)
                optionsDiv.style.minHeight = getViewportHeight() - 170 + 'px';
        }

        function OnOptionSelected(s, e) {

            var itemValue = s.get_selectedNode().get_value();

            if (itemValue == 'EDIT_SCHEDULE') {
                OnShowScheduleEditor('00000000-0000-0000-0000-000000000000');

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

        function updateFilterExpDiv() {
            var windowWidth = getViewportWidth();
            $("#fiterExpressionDiv").width(windowWidth - 450);
            $("#ellipsesDiv").width(windowWidth - 465);
            $("#ellipsesDiv").height(16);

            signalrFunctionHubRegistry.execute('SCHEDULER_SCRIPT', null);
        }

        function OnUpdateFilterExpDiv() { updateFilterExpDiv(); Sys.Application.remove_load(OnUpdateFilterExpDiv); }

        Sys.Application.add_load(OnUpdateFilterExpDiv);

        $(document).ready(function () {
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

        function OnScheduleResposeEnd(s, e) {
            updateFilterExpDiv();
            UpdateFilterEllipses();
        }

        addEvent(window, "resize", updateFilterExpDiv);

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
    ID="ajaxLoadingPanelSchedule" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelSchedule"
    Width="100%"
    LoadingPanelID="ajaxLoadingPanelSchedule"
    OnAjaxRequest="ajaxPanelSchedule_AjaxRequest"
    ClientEvents-OnResponseEnd="OnScheduleResposeEnd"
    >
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourceSchedule"
        OnSelecting="linqDataSourceSchedule_Selecting"
        >
    </asp:LinqDataSource>
    <div style="padding: 5px; padding-bottom: 0px; z-index:1000; position:relative;">
        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
            <div style="position: absolute; top: 5px; width: 100%;">
                <telerik:RadTabStrip 
                    runat="server" 
                    ID="tabStripScheduler" 
                    MultiPageID="multipageScheduler" 
                    SelectedIndex="0"
                    CausesValidation="false"
                    >
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Schedules" PageViewID="pageViewSchedules">
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
                                    ID="literalScheduleFilterExpression"
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
                    Text="Schedule Options"
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
                ID="multipageScheduler" 
                SelectedIndex="0" 
                RenderSelectedPageOnly="false"
                >
                <telerik:RadPageView runat="server" ID="pageViewSchedules">   
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td style="border-collapse: collapse; vertical-align: top; padding: 0px;">
                                    <telerik:RadGrid 
                                        runat="server" 
                                        ID="gridSchedule" 
                                        DataSourceID="linqDataSourceSchedule"
                                        GridLines="None" 
                                        BorderStyle="None"
                                        AutoGenerateColumns="false"
                                        Width="100%" 
                                        AllowSorting="True" 
                                        AllowPaging="True" 
                                        AllowCustomPaging="true"
                                        PageSize="5" 
                                        OnItemDataBound="gridSchedule_ItemDataBound" 
                                        >
                                        <MasterTableView 
                                            AllowPaging="true" 
                                            AllowCustomPaging="true"
                                            AutoGenerateColumns="false" 
                                            HierarchyLoadMode="ServerOnDemand" 
			                                DataKeyNames="ScheduleId,UniqueId" 
			                                ClientDataKeyNames="ScheduleId,UniqueId" 
                                            TableLayout="Auto"
                                            Width="100%"
                                            >
                                            <CommandItemTemplate>

                                            </CommandItemTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn
                                                    UniqueName="Name" 
                                                    DataField="Name" 
                                                    HeaderText="Details"
                                                    HeaderStyle-Width="25%"
                                                    ItemStyle-Width="25%"
                                                    SortExpression="Name"
                                                    ItemStyle-VerticalAlign="Top"
                                                    >
                                                    <ItemTemplate>
                                                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                            <script type="text/javascript">

                                                                scheduleFunctionArray.add('<%# Eval("UniqueId") %>', function (data) {
                                                                    data = $telerik.$.parseJSON(JSON.stringify(data));

                                                                    var imageCalendar = $('#<%# Container.FindControl("imageCalendar").ClientID %>');

                                                                    imageCalendar.attr("src", "/_layouts/images/Vmgr/nav-schedule-enabled-32.png");

                                                                    if (data.IsRunning) {
                                                                        imageCalendar.attr("src", "/_layouts/images/Vmgr/nav-schedule-active-32.png");
                                                                    }
                                                                    else {
                                                                    }
                                                                }
                                                                )
                                                                ;
                                                            </script>
                                                        </telerik:RadCodeBlock>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td rowspan="2" style="vertical-align: top; width: 32px;">
                                                                    <asp:Image
                                                                        runat="server"
                                                                        ID="imageCalendar"
                                                                        ImageUrl="/_layouts/images/Vmgr/nav-schedule-enabled-32.png"
                                                                        ToolTip="Scheduled resumed"
                                                                        />
                                                                </td>
                                                                <td>
                                                                    <span style="font-weight: bold;">
                                                                        <asp:Label
                                                                            runat="server"
                                                                            ID="labelName"
                                                                            >
                                                                            <%# Eval("Name") %>
                                                                            </asp:Label>
                                                                        </span>
                                                                    <br />
                                                                    <span style="">
                                                                        <asp:Label
                                                                            runat="server"
                                                                            ID="labelUniqueId"
                                                                            >
                                                                            {<%# Eval("UniqueId") %>}
                                                                            </asp:Label>
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <span style="font-style: italic;">
                                                                        <asp:Label
                                                                            runat="server"
                                                                            ID="labelDescription"
                                                                            >
                                                                            <%# Eval("Description")%>
                                                                            </asp:Label>
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn
                                                    HeaderText="Schedule"
                                                    HeaderStyle-Width="25%"
                                                    ItemStyle-Width="25%"
                                                    ItemStyle-VerticalAlign="Top"
                                                    >
                                                    <ItemTemplate>
                                                        <telerik:RadCodeBlock runat="server">
                                                            <script type="text/javascript">

                                                                scheduleFunctionArray.add('<%# Eval("UniqueId") %>', function (data) {
                                                                    data = $telerik.$.parseJSON(JSON.stringify(data));

                                                                    var panelSchedule = $('#<%# Container.FindControl("panelSchedule").ClientID %>');

                                                                    if (data.IsRunning) {
                                                                    }
                                                                    else {
                                                                        panelSchedule.html(data.PrimaryScheduleText + data.AnticipatedScheduleText);
                                                                    }
                                                                }
                                                                )
                                                                ;
                                                            </script>
                                                        </telerik:RadCodeBlock>
                                                        <asp:Panel
                                                            runat="server"
                                                            ID="panelSchedule"
                                                            >
                                                            <asp:Literal
                                                                runat="server"
                                                                ID="literalSchedule"
                                                                ></asp:Literal>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn
                                                    HeaderStyle-Width="3%"
                                                    ItemStyle-Width="3%"
                                                    ItemStyle-VerticalAlign="Top"
                                                    >
                                                    <ItemTemplate>
                                                        <table border="0" cellpadding="3px" cellspacing="3px">
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadButton 
                                                                        runat="server" 
                                                                        ID="buttonEdit"
                                                                        AutoPostBack="false"
                                                                        ToolTip="Edit schedule"
                                                                        Visible="true"
                                                                        Width="16px"
                                                                        Height="16px"
                                                                        OnClientClicked="OnEditSchedule"
                                                                        >
                                                                        <Image 
                                                                            ImageUrl="/_layouts/Images/Vmgr/edit-icon-16.png" />
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
                                                                        OnClientClicked="OnPauseResumeSchedule"
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
                                                                        ToolTip="Delete schedule"
                                                                        Visible="true"
                                                                        Width="16px"
                                                                        Height="16px"
                                                                        OnClientClicked="OnDeleteSchedule"
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
                                            <NoRecordsTemplate>
                                                <div style="padding: 5px;">
                                                    No schedules found.
                                                </div>
                                            </NoRecordsTemplate>
                                        </MasterTableView>
                                        <ClientSettings 
                                            AllowDragToGroup="false" 
                                            AllowColumnsReorder="false" 
                                            />
                                        <ClientSettings>
                                            <ClientEvents 
                                                OnGridCreated="OnScheduleGridCreated"
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
                                                    Text="Create a new schedule"
                                                    style="padding: 3px;"
                                                    Value="EDIT_SCHEDULE"
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