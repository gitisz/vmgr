<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Assembly Name="Vmgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MonitoringControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.MonitoringControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<%@ Register TagPrefix="Vmgr" TagName="PerformanceChartControl" Src="~/_CONTROLTEMPLATES/Vmgr/PerformanceChartControl.ascx" %>

<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        var signalrFunctionHubRegistry = new FunctionArray();
        var packageFunctionArray = new FunctionArray();
        var scheduleFunctionArray = new FunctionArray();
        var chartFunctionArray = new FunctionArray();
        var lastSize = null;

        var signalrMonitoringScriptLoaded = false;

        signalrFunctionHubRegistry.add('MONITORING_SCRIPT', function () {

            if (!signalrMonitoringScriptLoaded) {

                $.getScript('<%= (this.Page as BasePage).GetHubConnectionUrl() + "/signalr/hubs" %>')
                    .done(function (script, status) {

                        var connectionMonitoring = $.hubConnection("<%= (this.Page as BasePage).GetHubConnectionUrl() %>" + "/signalr");
                        var vmgrHubMonitoring = connectionMonitoring.createHubProxy('vmgrHub');

                        vmgrHubMonitoring.on('addMonitor', function (key, data) {
                            packageFunctionArray.execute(key, data);
                        });

                        vmgrHubMonitoring.on('addSchedule', function (key, data) {
                            scheduleFunctionArray.execute(key, data);
                        });

                        var startMonitoring = function () {
                            connectionMonitoring.start({ })
                                .then(function () {
                                    // Change the client somehow.
                                });
                        };

                        startMonitoring();

                        lastSize = getViewportWidth();
                    }
                    )
                ;
            }
        }
        )
        ;

        signalrFunctionHubRegistry.execute('MONITORING_SCRIPT', null);
        
        function OnMonitorPackage(s, e) {
            OnShowMonitorPackage(s, e);
        }

        function OnRefreshMonitors() {
            <%= this.javascriptRedirectUrl %>;
        }

        function OnDeleteMonitor(s, e) {
            selectedMonitorId = e.get_commandArgument();
            var ajaxPanelMonitor = $find('<%= ajaxPanelMonitor.ClientID %>');
            ajaxPanelMonitor.ajaxRequest('DELETE_MONITOR,' + selectedMonitorId);
        }

        function OnDeletePackageComplete(s, e) {
            <%= this.javascriptRedirectUrl %>;
        }

        function OnRepaintChart() {

            var less = getViewportWidth() < lastSize;

            chartFunctionArray.execute("REPAINT", less);
            lastSize = getViewportWidth();
        }

        addEvent(window, "resize", OnRepaintChart);

    </script>
</telerik:RadCodeBlock>

<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelMonitor" 
    Skin="Default" 
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelMonitor"
    OnAjaxRequest="ajaxPanelMonitor_AjaxRequest"
    >
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourceMonitor"
        OnSelecting="linqDataSourceMonitor_Selecting"
        ></asp:LinqDataSource>
    <asp:Repeater
        runat="server"
        ID="repeaterMonitor"
        DataSourceID="linqDataSourceMonitor"
        OnItemDataBound="repeaterMonitor_ItemDataBound"
        >
        <HeaderTemplate>
            <table style="width: 100%; padding: 0px; border-spacing:0; border-collapse: collapse;">
                <tr>
                    <td></td>
                    <td style="text-align: right; vertical-align: top; width: 24px;">
                        <asp:Image
                            runat="server"
                            ID="imageAddMonitor"
                            ToolTip="Monitor a package"
                            ImageUrl="/_layouts/images/Vmgr/add-icon-24.png"
                            />
                    </td>
                    <td style="text-align: right; vertical-align: top; width: 120px; white-space: nowrap; font-size: 9pt; padding-top: 3px; padding-right: 5px;">
                        <telerik:RadButton
                            runat="server"
                            ID="buttonAdd" 
                            AutoPostBack="false"
                            OnClientClicked="OnMonitorPackage"
                            Text="Monitor a package"
                            ButtonType="ToggleButton"
                            Font-Size="10pt"
                            style="cursor: hand; cursor: pointer;"
                            ></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <div style="margin-bottom: 4px;"></div>
        </HeaderTemplate>
        <ItemTemplate>
            <table style="width: 100%; padding: 0px; border-spacing:0; border-collapse: collapse;">
                <tr>
                    <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/monitor-top-left.png'); background-repeat: no-repeat;"></td>
                    <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/monitor-top-center.png'); background-repeat: repeat-x;"></td>
                    <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/monitor-top-right.png'); background-repeat: no-repeat;"></td>
                </tr>
                <tr>
                    <td style="padding: 0px; width: 12px; background-image: url('/_layouts/images/Vmgr/monitor-mid-left.png'); background-repeat: repeat-y;"></td>
                    <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/monitor-mid-center.png'); background-repeat: repeat;">
                        <telerik:RadCodeBlock runat="server">
                            <script type="text/javascript">

                                packageFunctionArray.add('<%# Eval("PackageUniqueId") %>', function (data) {
                                    data = $telerik.$.parseJSON(JSON.stringify(data));

                                    var windowWidth = getViewportWidth();

                                    var crop = 11;

                                    if (windowWidth < 1300) {
                                        crop = 8;
                                    }
                                    if (windowWidth < 1200) {
                                        crop = 7;
                                    }
                                    if (windowWidth < 1100) {
                                        crop = 6;
                                    }
                                    if (windowWidth < 1000) {
                                        crop = 5;
                                    }
                                    if (windowWidth < 900) {
                                        crop = 4;
                                    }

                                    var dArray = new Array();

                                    $.each(data, function (i, obj) {
                                        if (i > data.length - crop) {
                                            dArray.push(obj);
                                        }
                                    }
                                    );

                                    data = dArray;


                                    var labelChartCaptionAvgMonitoringTotalProcessorTimeValue = $get('<%# Container.FindControl("labelChartCaptionAvgMonitoringTotalProcessorTimeValue").ClientID %>');
                                    labelChartCaptionAvgMonitoringTotalProcessorTimeValue.innerHTML = data[data.length - 1].AvgMonitoringTotalProcessorTime + '%';

                                    var htmlChartAvgMonitoringTotalProcessorTimeValue = $find('<%# Container.FindControl("performanceChartControlAvgMonitoringTotalProcessorTimeValue").FindControl("htmlChartAvgMonitoringTotalProcessorTimeValue").ClientID %>');
                                    htmlChartAvgMonitoringTotalProcessorTimeValue.set_dataSource(data);
                                    htmlChartAvgMonitoringTotalProcessorTimeValue.repaint();

                                    var labelChartCaptionCpuUtilizationValue = $get('<%# Container.FindControl("labelChartCaptionCpuUtilizationValue").ClientID %>');
                                    labelChartCaptionCpuUtilizationValue.innerHTML = data[data.length - 1].CpuUtilization + '%';

                                    var htmlChartCpuUtilization = $find('<%# Container.FindControl("performanceChartControlCpuUtilization").FindControl("htmlChartCpuUtilization").ClientID %>');
                                    htmlChartCpuUtilization.set_dataSource(data);
                                    htmlChartCpuUtilization.repaint();

                                    var labelChartCaptionMemorySurvivedValue = $get('<%# Container.FindControl("labelChartCaptionMemorySurvivedValue").ClientID %>');
                                    labelChartCaptionMemorySurvivedValue.innerHTML = data[data.length - 1].MonitoringSurvivedMemorySize + 'MB';

                                    var htmlChartMemorySurvived = $find('<%# Container.FindControl("performanceChartControlMemorySurvived").FindControl("htmlChartMemorySurvived").ClientID %>');
                                    htmlChartMemorySurvived.set_dataSource(data);
                                    htmlChartMemorySurvived.repaint();

                                    var labelChartCaptionMemoryUtilizationValue = $get('<%# Container.FindControl("labelChartCaptionMemoryUtilizationValue").ClientID %>');
                                    labelChartCaptionMemoryUtilizationValue.innerHTML = data[data.length - 1].MemoryUtilization + '%';

                                    var htmlChartMemoryUtilization = $find('<%# Container.FindControl("performanceChartControlMemoryUtilization").FindControl("htmlChartMemoryUtilization").ClientID %>');
                                    htmlChartMemoryUtilization.set_dataSource(data);
                                    htmlChartMemoryUtilization.repaint();
                                }
                                )
                                ;

                                chartFunctionArray.add('REPAINT', function (less) {
                                    var dArray = new Array();

                                    var htmlChartAvgMonitoringTotalProcessorTimeValue = $find('<%# Container.FindControl("performanceChartControlAvgMonitoringTotalProcessorTimeValue").FindControl("htmlChartAvgMonitoringTotalProcessorTimeValue").ClientID %>');
                                    if(less)
                                        htmlChartAvgMonitoringTotalProcessorTimeValue.set_dataSource(dArray);
                                    htmlChartAvgMonitoringTotalProcessorTimeValue.repaint();

                                    var htmlChartCpuUtilization = $find('<%# Container.FindControl("performanceChartControlCpuUtilization").FindControl("htmlChartCpuUtilization").ClientID %>');
                                    if (less)
                                        htmlChartCpuUtilization.set_dataSource(dArray);
                                    htmlChartCpuUtilization.repaint();

                                    var htmlChartMemorySurvived = $find('<%# Container.FindControl("performanceChartControlMemorySurvived").FindControl("htmlChartMemorySurvived").ClientID %>');
                                    if (less)
                                        htmlChartMemorySurvived.set_dataSource(dArray);
                                    htmlChartMemorySurvived.repaint();

                                    var htmlChartMemoryUtilization = $find('<%# Container.FindControl("performanceChartControlMemoryUtilization").FindControl("htmlChartMemoryUtilization").ClientID %>');
                                    if (less)
                                        htmlChartMemoryUtilization.set_dataSource(dArray);
                                    htmlChartMemoryUtilization.repaint();
                                }
                                )
                                ;

                            </script>
                        </telerik:RadCodeBlock>
                        <div style="font-size: 8pt;">
                            <table>
                                <tr>
                                    <td style="vertical-align: top; width: 320px;">
                                        <asp:Label
                                            runat="server"
                                            ID="labelPackageName"
                                            Text='<%# Eval("PackageName") %>'
                                            Font-Size="12pt"
                                            ></asp:Label>
                                        <br />
                                        <asp:Label
                                            runat="server"
                                            ID="labelPackageUniqueId"
                                            Font-Italic="true"
                                            Text='<%# string.Format("{{{0}}}", (Guid)Eval("PackageUniqueId")) %>'
                                            ></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label 
                                            runat="server" 
                                            ID="labelPackageId" 
                                            Visible="false"
                                            Text='<%# Eval("PackageId") %>'></asp:Label> 
                                        <asp:LinqDataSource 
                                            runat="server" 
                                            ID="linqDataSourceSchedule" 
                                            OnSelecting="linqDataSourceSchedule_Selecting"
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
                                        <asp:Repeater
                                            runat="server"
                                            ID="repeaterSchedule"
                                            DataSourceID="linqDataSourceSchedule"
                                            >
                                            <ItemTemplate>
                                                <telerik:RadCodeBlock runat="server">
                                                    <script type="text/javascript">
                                                        scheduleFunctionArray.add('<%# Eval("ScheduleUniqueId") %>', function (data) {
                                                            data = $telerik.$.parseJSON(JSON.stringify(data));

                                                            var imageSchedule = $('#<%# Container.FindControl("imageSchedule").ClientID %>');
                                                            var labelSchedule = $('#<%# Container.FindControl("labelSchedule").ClientID %>');

                                                            imageSchedule.attr("src", "/_layouts/images/Vmgr/alarm-icon-32.png");

                                                            if (data.IsRunning) {
                                                                imageSchedule.attr("src", "/_layouts/images/Vmgr/alarm-on-icon-32.png");
                                                                labelSchedule.html("Total elapsed: " + data.ElapsedTime);
                                                            }
                                                            else {
                                                                labelSchedule.html(data.SecondaryScheduleText);
                                                            }
                                                        }
                                                        )
                                                        ;
                                                    </script>
                                                </telerik:RadCodeBlock>
                                                <table>
                                                    <tr>
                                                        <td style="width: 32px;">
                                                            <asp:Image
                                                                runat="server"
                                                                ID="imageSchedule"
                                                                ToolTip='<%# Eval("Name") %>'
                                                                ImageUrl="/_layouts/images/Vmgr/alarm-icon-32.png"
                                                                />
                                                        </td>
                                                        <td>
                                                            <asp:Label
                                                                runat="server"
                                                                ID="labelSchedule"
                                                                Text='<%# Eval("NextAnticipated") %>'
                                                                ></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <br />
                                        <br />
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div>
                                            <div style="padding-left: 15px; padding-bottom: 4px;">
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionCpuUtilization"
                                                    Font-Size="12pt"
                                                    Text="CPU Utilization:"
                                                    ></asp:Label>
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionCpuUtilizationValue"
                                                    Font-Size="12pt"
                                                    ForeColor="#5AB7DE"
                                                    Text="0%"
                                                    ></asp:Label>
                                            </div>
                                            <Vmgr:PerformanceChartControl 
                                                runat="server"
                                                ID="performanceChartControlCpuUtilization" 
                                                >
                                                <PerformanceChartTemplate>
                                                    <telerik:RadHtmlChart 
                                                        runat="server"
                                                        ID="htmlChartCpuUtilization" 
                                                        Height="100px"
                                                        >
                                                        <PlotArea>
                                                            <XAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickType="Outside" 
                                                                MinorTickType="Outside"
                                                                Reversed="false"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                            </XAxis>
                                                            <YAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickSize="1" 
                                                                MajorTickType="Outside"
                                                                MaxValue="100" 
                                                                MinorTickSize="1" 
                                                                MinorTickType="Outside" 
                                                                MinValue="0" 
                                                                Reversed="false"
                                                                Step="25"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                                <TitleAppearance Position="Center" RotationAngle="0"></TitleAppearance>
                                                                </YAxis>                                                            
                                                            <Series>
                                                                <telerik:LineSeries 
                                                                    MissingValues="Interpolate" 
                                                                    DataFieldY="CpuUtilization"
                                                                    >
                                                                    <Appearance>
                                                                        <FillStyle BackgroundColor="#5AB7DE"></FillStyle>
                                                                    </Appearance>
                                                                    <LabelsAppearance DataFormatString="{0}%" Position="Above"></LabelsAppearance>
                                                                    <MarkersAppearance 
                                                                        MarkersType="Circle" 
                                                                        BackgroundColor="White" 
                                                                        ></MarkersAppearance>
                                                                    <TooltipsAppearance DataFormatString="{0}%"></TooltipsAppearance>
                                                                </telerik:LineSeries> 
                                                            </Series>
                                                        </PlotArea>
                                                    </telerik:RadHtmlChart>
                                                </PerformanceChartTemplate>
                                            </Vmgr:PerformanceChartControl>
                                        </div>
                                        <div>
                                            <div style="padding-left: 15px; padding-bottom: 4px;">
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionAvgMonitoringTotalProcessorTime"
                                                    Font-Size="12pt"
                                                    Text="CPU Std. Deviation:"
                                                    ></asp:Label>
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionAvgMonitoringTotalProcessorTimeValue"
                                                    Font-Size="12pt"
                                                    ForeColor="Green"
                                                    Text="0%"
                                                    ></asp:Label>
                                            </div>
                                            <Vmgr:PerformanceChartControl 
                                                runat="server"
                                                ID="performanceChartControlAvgMonitoringTotalProcessorTimeValue" 
                                                >
                                                <PerformanceChartTemplate>
                                                    <telerik:RadHtmlChart 
                                                        runat="server"
                                                        ID="htmlChartAvgMonitoringTotalProcessorTimeValue" 
                                                        Height="100px"
                                                        >
                                                        <PlotArea>
                                                            <XAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickType="Outside" 
                                                                MinorTickType="Outside"
                                                                Reversed="false"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                            </XAxis>
                                                            <YAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickSize="1" 
                                                                MajorTickType="Outside"
                                                                MaxValue="100" 
                                                                MinorTickSize="1" 
                                                                MinorTickType="Outside" 
                                                                MinValue="0" 
                                                                Reversed="false"
                                                                Step="25"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                                <TitleAppearance Position="Center" RotationAngle="0"></TitleAppearance>
                                                                </YAxis>                                                            
                                                            <Series>
															    <telerik:LineSeries 
                                                                    MissingValues="Interpolate" 
                                                                    DataFieldY="AvgMonitoringTotalProcessorTime"
                                                                    >
                                                                    <Appearance>
                                                                        <FillStyle BackgroundColor="#66B445"></FillStyle>
                                                                    </Appearance>
                                                                    <LabelsAppearance DataFormatString="{0}%" Position="Above"></LabelsAppearance>
                                                                    <MarkersAppearance 
                                                                        MarkersType="Circle" 
                                                                        BackgroundColor="White" 
                                                                        ></MarkersAppearance>
                                                                    <TooltipsAppearance DataFormatString="{0}%"></TooltipsAppearance>
                                                                </telerik:LineSeries> 
                                                            </Series>
                                                        </PlotArea>
                                                    </telerik:RadHtmlChart>
                                                </PerformanceChartTemplate>
                                            </Vmgr:PerformanceChartControl>
                                        </div>
                                        <div>
                                            <div style="padding-left: 15px; padding-bottom: 4px;">
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionMemoryUtilization"
                                                    Font-Size="12pt"
                                                    Text="Memory Utilization:"
                                                    ></asp:Label>
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionMemoryUtilizationValue"
                                                    Font-Size="12pt"
                                                    ForeColor="#5AB7DE"
                                                    Text="0%"
                                                    ></asp:Label>
                                            </div>
                                            <Vmgr:PerformanceChartControl 
                                                runat="server"
                                                ID="performanceChartControlMemoryUtilization" 
                                                >
                                                <PerformanceChartTemplate>
                                                    <telerik:RadHtmlChart 
                                                        runat="server"
                                                        ID="htmlChartMemoryUtilization" 
                                                        Height="100px"
                                                        >
                                                        <PlotArea>
                                                            <XAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickType="Outside" 
                                                                MinorTickType="Outside"
                                                                Reversed="false"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                            </XAxis>
                                                            <YAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickSize="1" 
                                                                MajorTickType="Outside"
                                                                MaxValue="100" 
                                                                MinorTickSize="1" 
                                                                MinorTickType="Outside" 
                                                                MinValue="0" 
                                                                Reversed="false"
                                                                Step="25"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                                <TitleAppearance Position="Center" RotationAngle="0"></TitleAppearance>
                                                                </YAxis>                                                            
                                                            <Series>
                                                                <telerik:LineSeries 
                                                                    MissingValues="Interpolate" 
                                                                    DataFieldY="MemoryUtilization"
                                                                    >
                                                                    <Appearance>
                                                                        <FillStyle BackgroundColor="#5AB7DE"></FillStyle>
                                                                    </Appearance>
                                                                    <LabelsAppearance DataFormatString="{0}%" Position="Above"></LabelsAppearance>
                                                                    <MarkersAppearance 
                                                                        MarkersType="Circle" 
                                                                        BackgroundColor="White" 
                                                                        ></MarkersAppearance>
                                                                    <TooltipsAppearance DataFormatString="{0}%"></TooltipsAppearance>
                                                                </telerik:LineSeries> 
                                                            </Series>
                                                        </PlotArea>
                                                    </telerik:RadHtmlChart>
                                                </PerformanceChartTemplate>
                                            </Vmgr:PerformanceChartControl>
                                        </div>
                                        <div>
                                            <div style="padding-left: 15px; padding-bottom: 4px;">
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionMemorySurvived"
                                                    Font-Size="12pt"
                                                    Text="Memory Survived:"
                                                    ></asp:Label>
                                                <asp:Label
                                                    runat="server"
                                                    ID="labelChartCaptionMemorySurvivedValue"
                                                    Font-Size="12pt"
                                                    ForeColor="Green"
                                                    Text="0MB"
                                                    ></asp:Label>
                                            </div>
                                            <Vmgr:PerformanceChartControl 
                                                runat="server"
                                                ID="performanceChartControlMemorySurvived" 
                                                >
                                                <PerformanceChartTemplate>
                                                    <telerik:RadHtmlChart 
                                                        runat="server"
                                                        ID="htmlChartMemorySurvived" 
                                                        Height="100px"
                                                        >
                                                        <PlotArea>
                                                            <XAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickType="Outside" 
                                                                MinorTickType="Outside"
                                                                Reversed="false"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                            </XAxis>
                                                            <YAxis 
                                                                AxisCrossingValue="0" 
                                                                Color="#B3B3B3" 
                                                                MajorTickSize="1" 
                                                                MajorTickType="Outside"
                                                                MaxValue="100" 
                                                                MinorTickSize="1" 
                                                                MinorTickType="Outside" 
                                                                MinValue="0" 
                                                                Reversed="false"
                                                                Step="25"
                                                                >
                                                                <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                                                                <MajorGridLines Visible="false"></MajorGridLines>
                                                                <MinorGridLines Visible="false"></MinorGridLines>
                                                                <TitleAppearance Position="Center" RotationAngle="0"></TitleAppearance>
                                                                </YAxis>                                                            
                                                            <Series>
                                                                <telerik:LineSeries 
                                                                    MissingValues="Interpolate" 
                                                                    DataFieldY="MonitoringSurvivedMemorySize"
                                                                    >
                                                                    <Appearance>
                                                                        <FillStyle BackgroundColor="#66B445"></FillStyle>
                                                                    </Appearance>
                                                                    <LabelsAppearance DataFormatString="{0}MB" Position="Above"></LabelsAppearance>
                                                                    <MarkersAppearance 
                                                                        MarkersType="Circle" 
                                                                        BackgroundColor="White" 
                                                                        ></MarkersAppearance>
                                                                    <TooltipsAppearance DataFormatString="{0}MB"></TooltipsAppearance>
                                                                </telerik:LineSeries> 
                                                            </Series>
                                                        </PlotArea>
                                                    </telerik:RadHtmlChart>
                                                </PerformanceChartTemplate>
                                            </Vmgr:PerformanceChartControl>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: bottom;"> 
                                        <telerik:RadButton
                                            runat="server"
                                            ID="buttonDelete" 
                                            OnClientClicked="OnDeleteMonitor"
                                            AutoPostBack="false"
                                            ToolTip="Delete monitor"
                                            Font-Size="10pt"
                                            style="cursor: hand; cursor: pointer;"
                                            Width="32px"
                                            Height="32px"
                                            >
                                            <Image ImageUrl="/_layouts/images/Vmgr/trash-can-icon-32.png" />
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td style="padding: 0px; width: 12px; background-image: url('/_layouts/images/Vmgr/monitor-mid-right.png'); background-repeat: repeat-y;"></td>
                </tr>
                <tr>
                    <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/monitor-bottom-left.png'); background-repeat: no-repeat;"></td>
                    <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/monitor-bottom-center.png'); background-repeat: repeat-x;"></td>
                    <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/monitor-bottom-right.png'); background-repeat: no-repeat;"></td>
                </tr>
            </table>
            <div style="height: 6px;"></div>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
</telerik:RadAjaxPanel>