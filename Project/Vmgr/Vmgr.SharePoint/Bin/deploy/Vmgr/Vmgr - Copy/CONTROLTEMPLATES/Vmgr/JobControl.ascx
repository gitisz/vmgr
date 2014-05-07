<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="JobControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.JobControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        var signalrFunctionHubRegistry = new FunctionArray();
        var scheduleFunctionArray = new FunctionArray();
        var jobFunctionArray = new FunctionArray();
        var signalrSchedulerScriptLoaded = false;
   
        signalrFunctionHubRegistry.add('JOB_HISTORY_SCRIPT', function () {

            if (!signalrSchedulerScriptLoaded) {

                $.getScript('<%= (this.Page as BasePage).GetHubConnectionUrl() + "/signalr/hubs" %>')
                    .done(function (script, status) {
                        initializeScheduleHub();
                        initializeJobHistoryHub();
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

        function initializeScheduleHub() {
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

        function initializeJobHistoryHub() {

            var connectionJob = $.hubConnection("<%= (this.Page as BasePage).GetHubConnectionUrl() %>" + "/signalr");
            var vmgrHubJob = connectionJob.createHubProxy('vmgrHub');

            vmgrHubJob.on('addJobHistory', function (key, data) {
                jobFunctionArray.execute(key, data);
            });

            var startJob = function () {
                connectionJob.start({ })
                    .then(function () {
                        // Change the client somehow.
                    });
            };

            startJob();
        }
  

        function OnRefreshHistory() {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest('REFRESH,');
        }

        function OnOpenOptions(s, e) {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest('OPEN_OPTIONS,');
        }

        function OnCloseOptions() {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest('CLOSE_OPTIONS,');
        }

        function OnApplyFilter(filter) {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest(filter);
        }

        function OnFilterSaved() {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnFilterDeleted() {
            var ajaxPanelJob = $find('<%= ajaxPanelJob.ClientID %>');
            ajaxPanelJob.ajaxRequest('REFRESH_FILTERS,');
        }

        function OnJobGridCreated(s, e) {

            var height = s.get_masterTableView().get_element().clientHeight;

            var optionsDiv = $get('optionsDiv');

            if (optionsDiv != undefined)
                optionsDiv.style.minHeight = getViewportHeight() - 170 + 'px';
        }

        function OnOptionSelected(s, e) {

            var itemValue = s.get_selectedNode().get_value();

            if (itemValue == 'REFRESH') {

                var node = s.get_selectedNode();
                node.unselect();

                OnRefreshHistory();
            }

            if (itemValue == 'MANAGE_FILTERS') {
                OnShowFilter();
            }

            if (itemValue.indexOf("APPLY_FILTER") >= 0) {
                OnApplyFilter(itemValue);
            }
        }

        function OnJobKeySelected(s, e) {
            OnShowTriggerHistory(s, e);
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
   
            signalrFunctionHubRegistry.execute('JOB_HISTORY_SCRIPT', null);
        }

        function OnUpdateFilterExpDiv() { updateFilterExpDiv(); Sys.Application.remove_load(OnUpdateFilterExpDiv); }


        Sys.Application.add_load(OnUpdateFilterExpDiv);

        $(document).ready(function () {
            updateFilterEllipses();
        }
        )
        ;

        function updateFilterEllipses() {
            $("#ellipsesDiv").dotdotdot({
                ellipsis: '... ',
                watch: true
            });
        }

        function OnJobResposeEnd(s, e) {
            updateFilterExpDiv();
            updateFilterEllipses();
        }

        addEvent(window, "resize", updateFilterExpDiv);

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
    ID="ajaxLoadingPanelJob" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelJob"
    Width="100%"
    LoadingPanelID="ajaxLoadingPanelJob"
    OnAjaxRequest="ajaxPanelJob_AjaxRequest"
    ClientEvents-OnResponseEnd="OnJobResposeEnd"
    >
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourceJob"
        OnSelecting="linqDataSourceJob_Selecting"
        ></asp:LinqDataSource>
    <div style="padding: 5px; padding-bottom: 0px; z-index:1000; position:relative;">
        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
            <div style="position: absolute; top: 5px; width: 100%;">
                <telerik:RadTabStrip 
                    runat="server" 
                    ID="tabStripJob" 
                    MultiPageID="multipageJob" 
                    SelectedIndex="0"
                    CausesValidation="false"
                    >
                    <Tabs>
                        <telerik:RadTab runat="server" Text="History" PageViewID="pageViewHistory">
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
                                    ID="literalJobFilterExpression"
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
                    Text="Job Options"
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
                ID="multipageJobr" 
                SelectedIndex="0" 
                RenderSelectedPageOnly="false"
                >
                <telerik:RadPageView runat="server" ID="pageViewJobs">   
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td style="border-collapse: collapse; vertical-align: top; padding: 0px;">
                                <telerik:RadGrid 
                                    runat="server" 
                                    ID="gridJob" 
                                    DataSourceID="linqDataSourceJob"
                                    GridLines="None" 
                                    BorderStyle="None"
                                    AutoGenerateColumns="false"
                                    Width="100%" 
                                    AllowSorting="True" 
                                    AllowPaging="True" 
                                    AllowCustomPaging="true"
                                    PageSize="25" 
                                    OnItemDataBound="gridJob_ItemDataBound" 
                                    OnItemCreated="gridJob_ItemCreated"
                                    >
                                    <MasterTableView 
                                        AllowPaging="true" 
                                        AllowCustomPaging="true"
                                        AutoGenerateColumns="false" 
                                        HierarchyLoadMode="ServerOnDemand" 
			                            DataKeyNames="JobId" 
                                        >
                                        <Columns>
                                            <telerik:GridTemplateColumn
                                                UniqueName="ScheduleName" 
                                                DataField="ScheduleName" 
                                                HeaderText="Schedule"
                                                SortExpression="ScheduleName"
                                                ItemStyle-VerticalAlign="Top"
                                                HeaderStyle-Width="30%"
                                                >
                                                <ItemTemplate>
                                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                        <script type="text/javascript">

                                                            scheduleFunctionArray.add('<%# Eval("ScheduleUniqueId") %>', function (data) {
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
                                                                        <%# Eval("ScheduleName") %>
                                                                        </asp:Label>
                                                                    </span>
                                                                <br />
                                                                <span style="">
                                                                    <asp:Label
                                                                        runat="server"
                                                                        ID="labelUniqueId"
                                                                        >
                                                                        {<%# Eval("ScheduleUniqueId") %>}
                                                                        </asp:Label>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn
                                                UniqueName="JobKey" 
                                                DataField="JobKey" 
                                                HeaderText="Job"
                                                SortExpression="Name"
                                                ItemStyle-VerticalAlign="Top"
                                                >
                                                <ItemTemplate>
                                                    <telerik:RadButton
                                                        runat="server"
                                                        ID="buttonJobKey"
                                                        CausesValidation="false"
                                                        ButtonType="ToggleButton"
                                                        AutoPostBack="false"
                                                        CssClass="linkButton"
                                                        ToolTip="Click to view history for this job."
                                                        OnClientClicked="OnJobKeySelected"
                                                        ></telerik:RadButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn
                                                UniqueName="JobStatusTypeId" 
                                                DataField="JobStatusTypeId" 
                                                HeaderText="Status"
                                                SortExpression="JobStatusTypeId"
                                                HeaderStyle-Width="16px" 
                                                ItemStyle-VerticalAlign="Top"
                                                ItemStyle-HorizontalAlign="Center"
                                                >
                                                <ItemTemplate>
                                                    <telerik:RadCodeBlock runat="server">
                                                        <script type="text/javascript">

                                                            jobFunctionArray.add('<%# Eval("JobKey") %>', function (data) {
                                                                data = $telerik.$.parseJSON(JSON.stringify(data));

                                                                var imageJobStatusType = $('#<%# Container.FindControl("imageJobStatusType").ClientID %>');

                                                                if (data.JobStatusType == 1) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/actions-view-calendar-day-icon-16.png");
                                                                    imageJobStatusType.attr("alt", "Jobd");
                                                                    imageJobStatusType.attr("title", "Jobd");
                                                                }
                                                                else if (data.JobStatusType == 2) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/recurrence-icon-16.png");
                                                                    imageJobStatusType.attr("alt", "Recurrence");
                                                                    imageJobStatusType.attr("title", "Recurrence");
                                                                }
                                                                else if (data.JobStatusType == 3) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/hourglass-select-icon-16.png");
                                                                    imageJobStatusType.attr("alt", "Total elapsed: " + data.ElapsedTime);
                                                                    imageJobStatusType.attr("title", "Total elapsed: " + data.ElapsedTime);
                                                                }
                                                                else if (data.JobStatusType == 4) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/green-check-16.png");
                                                                    imageJobStatusType.attr("alt", "Succeeded");
                                                                    imageJobStatusType.attr("title", "Succeeded");
                                                                }
                                                                else if (data.JobStatusType == 5) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/actions-view-calendar-day-icon-misfire-16.png");
                                                                    imageJobStatusType.attr("alt", "Failed");
                                                                    imageJobStatusType.attr("title", "Failed");
                                                                }
                                                                else if (data.JobStatusType == 6) {
                                                                    imageJobStatusType.attr("src", "/_layouts/images/Vmgr/stop-icon-16.png");
                                                                    imageJobStatusType.attr("alt", "Removed");
                                                                    imageJobStatusType.attr("title", "Removed");
                                                                }
                                                            }
                                                            )
                                                            ;
                                                        </script>
                                                    </telerik:RadCodeBlock>
                                                    <asp:Image
                                                        runat="server"
                                                        ID="imageJobStatusType"
                                                        />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <NoRecordsTemplate>
                                            <div style="padding: 5px;">
                                                No jobs found.
                                            </div>
                                        </NoRecordsTemplate>
                                    </MasterTableView>
                                    <ClientSettings 
                                        AllowDragToGroup="false" 
                                        AllowColumnsReorder="false" 
                                        >
                                        <ClientEvents 
                                            OnGridCreated="OnJobGridCreated"
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
                                            style="overflow-x: hidden;"
                                            >
                                            <Nodes>
                                                <telerik:RadTreeNode
                                                    ImageUrl="/_layouts/images/Vmgr/refresh-icon-24.png"
                                                    Text="Refresh history"
                                                    style="padding: 3px;"
                                                    Value="REFRESH"
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