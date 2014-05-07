<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="TriggerHistoryControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.TriggerHistoryControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        var scheduleFunctionArray = new FunctionArray();

        scheduleFunctionArray.add('<%= this.job.ScheduleUniqueId %>', function (data) {
            data = $telerik.$.parseJSON(JSON.stringify(data));

            var imageCalendar = $('#<%= this.imageCalendar.ClientID %>');

            imageCalendar.attr("src", "/_layouts/images/Vmgr/nav-schedule-enabled-32.png");

            if (data.IsRunning) {
                imageCalendar.attr("src", "/_layouts/images/Vmgr/nav-schedule-active-32.png");
            }
            else {
            }

            OnRefreshTriggers();

        }
        )
        ;

        var signalrTriggerHistoryScriptLoaded = false;
        var reconnectAttempts = 0;

        signalrFunctionHubRegistry.add('TRIGGER_HISTORY_SCRIPT', function () {

            if (!signalrTriggerHistoryScriptLoaded) {

                $.getScript('<%= (this.Page as BasePage).GetHubConnectionUrl() + "/signalr/hubs" %>')
                    .done(function (script, status) {
                    
                        var connectionSchedule = $.hubConnection("<%= (this.Page as BasePage).GetHubConnectionUrl() %>");
                        var vmgrHubSchedule = connectionSchedule.createHubProxy('vmgrHub');

                        connectionSchedule.reconnected(function () {
                            reconnectAttempts = 0;
                        }
                        )
                        ;

                        connectionSchedule.disconnected(function () {
                            if (reconnectAttempts < 5) {
                                reconnectAttempts++;
                                setTimeout(function () {
                                    connectionSchedule.start({})
                                        .then(function () {
                                        }
                                        )
                                    ;
                                }
                                , 1000);
                            }
                        }
                        )
                        ;

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
                    ;
            }
        }
        )
        ;

        signalrFunctionHubRegistry.execute('TRIGGER_HISTORY_SCRIPT', null);

        function OnRefreshTriggers() {
            var ajaxPanelTriggerHistory = $find('<%= ajaxPanelTriggerHistory.ClientID %>');
            ajaxPanelTriggerHistory.ajaxRequest('REFRESH,');
        }

        function OnGridCreated(sender, args) {

            adjustGridHeight();
        }

        function adjustGridHeight(){
        
            var gridTriggerHistory = $find('<%= gridTriggerHistory.ClientID %>');
            var scrollArea = gridTriggerHistory.GridDataDiv;
            var parent = $get("<%= this.Page.Form.ClientID %>");
            var gridHeader = gridTriggerHistory.GridHeaderDiv;
            scrollArea.style.height = (parent.clientHeight - gridHeader.clientHeight - 162) + "px";
        }

        addEvent(window, "resize", adjustGridHeight);

    </script>
</telerik:RadCodeBlock>
<div style="width: 100%;">
    <div style="padding: 5px;">
        <table border="0">
            <tr>
                <td rowspan="2" style="vertical-align: top; width: 32px; padding: 3px;">
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
                            ID="labelScheduleName"
                            >
                            </asp:Label>
                        </span>
                    <br />
                    <span style="">
                        <asp:Label
                            runat="server"
                            ID="labelScheduleUniqueId"
                            >
                            </asp:Label>
                    </span>
                </td>
            </tr>
        </table>
    </div>
</div>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelTriggerHistory"
    OnAjaxRequest="ajaxPanelTriggerHistory_AjaxRequest"
    >
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourceTriggerHistory"
        OnSelecting="linqDataSourceTriggerHistory_Selecting"
        ></asp:LinqDataSource>
    <telerik:RadGrid 
        runat="server" 
        ID="gridTriggerHistory" 
        DataSourceID="linqDataSourceTriggerHistory"
        GridLines="None" 
        BorderStyle="None"
        AutoGenerateColumns="false"
        Width="100%" 
        AllowSorting="True" 
        AllowPaging="True" 
        AllowCustomPaging="true"
        PageSize="100" 
        OnItemDataBound="gridTriggerHistory_ItemDataBound" 
        >
        <MasterTableView 
            AllowPaging="true" 
            AllowCustomPaging="true"
            AutoGenerateColumns="false" 
            HierarchyLoadMode="ServerOnDemand" 
			DataKeyNames="TriggerId" 
            >
            <Columns>
                <telerik:GridBoundColumn
                    UniqueName="TriggerId" 
                    DataField="TriggerId" 
                    HeaderText="ID"
                    HeaderStyle-Width="3%" 
                    SortExpression="TriggerId"
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn
                    UniqueName="TriggerKey" 
                    DataField="TriggerKey" 
                    HeaderText="Trigger"
                    SortExpression="TriggerKey"
                    HeaderStyle-Width="20%" 
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn
                    UniqueName="Started" 
                    DataField="Started" 
                    HeaderText="Started"
                    SortExpression="Started"
                    HeaderStyle-Width="5%" 
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn
                    UniqueName="Ended" 
                    DataField="Ended" 
                    HeaderText="Ended"
                    SortExpression="Ended"
                    HeaderStyle-Width="5%" 
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn
                    UniqueName="PreviousFire" 
                    DataField="PreviousFire" 
                    HeaderText="Previous"
                    SortExpression="PreviousFire"
                    HeaderStyle-Width="5%" 
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn
                    UniqueName="NextFire" 
                    DataField="NextFire" 
                    HeaderText="Next"
                    SortExpression="NextFire"
                    HeaderStyle-Width="5%" 
                    ItemStyle-VerticalAlign="Top"
                    >
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn
                    UniqueName="TriggerStatusTypeId" 
                    DataField="TriggerStatusTypeId" 
                    HeaderText="Status"
                    SortExpression="TriggerStatusTypeId"
                    HeaderStyle-Width="3%" 
                    ItemStyle-VerticalAlign="Top"
                    ItemStyle-HorizontalAlign="Center"
                    >
                    <ItemTemplate>
                        <asp:Image
                            runat="server"
                            ID="imageTriggerStatusType"
                            />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <NoRecordsTemplate>
                <div style="padding: 5px;">
                    No tiggers found.
                </div>
            </NoRecordsTemplate>
        </MasterTableView>
        <ClientSettings 
            AllowDragToGroup="false" 
            AllowColumnsReorder="false" 
            >
            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
            <ClientEvents OnGridCreated="OnGridCreated" />
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
</telerik:RadAjaxPanel>