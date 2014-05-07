<%@ Assembly Name="Vmgr.TestPlugin.WebPart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=05645e6641d2565d" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint" %>
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="TestPluginWebPartUserControl.ascx.cs" 
    Inherits="Vmgr.TestPlugin.WebPart.UI.TestPluginWebPartUserControl, Vmgr.TestPlugin.WebPart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=05645e6641d2565d" 
    %>

<script src="/_layouts/Vmgr/jquery.1.7.2.js" type="text/javascript"></script>
<script src="/_layouts/Vmgr/json2.min.js" type="text/javascript"></script>
<script src="../../signalr/hubs" type="text/javascript"></script>
<script src="/_layouts/Vmgr/jquery.signalR-2.0.0-beta2.min.js" type="text/javascript"></script>

<script type="text/javascript">

    function FunctionArray() {
        this.functionArray = new Array();
    }

    FunctionArray.prototype.add = function (k, f) {
        if (typeof f != "function") {
            f = new Function(f);
        }
        this.functionArray[this.functionArray.length] = { key: k, func: f };
    }

    FunctionArray.prototype.getLength = function () {
        return this.functionArray.length;
    }

    FunctionArray.prototype.execute = function (k, d) {
        for (var i = 0; i < this.functionArray.length; i++) {
            if (this.functionArray[i].key == k)
                this.functionArray[i].func(d);
        }
    }

    var packageFunctionArray = new FunctionArray();

    var connectionState;

    $(function () {
        configureMessaging();
    });

    function configureMessaging() {
        var hubConnectionMonitoring = $.hubConnection('http://<%= this.serverName  %>:8080/signalr');
        hubConnectionMonitoring.stateChanged(connectionStateChanged);
        hubConnectionMonitoring.start({ waitForPageLoad: false, xdomain: true, jsonp: true });

        var vmgrHub = hubConnectionMonitoring.createHubProxy("vmgrHub");

        vmgrHub.on('addMonitor', function (key, data) {
            packageFunctionArray.execute(key, data);
        });

    }

    function connectionStateChanged(state) {
        connectionState = state.newState;
    }

    packageFunctionArray.add('e179b68c-199a-4ea3-b767-54333be9ddd4', function (data) {

        var labelAvgMonitoringTotalProcessorTimeRt = $('#<%= this.FindControl("labelAvgMonitoringTotalProcessorTimeRt").ClientID %>');
        labelAvgMonitoringTotalProcessorTimeRt.html(data[data.length - 1].AvgMonitoringTotalProcessorTime + ' %');

        var labelCpuUtilizationRt = $('#<%= this.FindControl("labelCpuUtilizationRt").ClientID %>');
        labelCpuUtilizationRt.html(data[data.length - 1].CpuUtilization + ' %');

        var labelMonitoringSurvivedMemorySizeRt = $('#<%= this.FindControl("labelMonitoringSurvivedMemorySizeRt").ClientID %>');
        labelMonitoringSurvivedMemorySizeRt.html(data[data.length - 1].MonitoringSurvivedMemorySize + ' MB');

        var labelMemoryUtilizationRt = $('#<%= this.FindControl("labelMemoryUtilizationRt").ClientID %>');
        labelMemoryUtilizationRt.html(data[data.length - 1].MemoryUtilization + ' %');
    }
    )
    ;

</script>

<br />
<h2>WCF Call Test Plugin Result:</h2>

<asp:Label
    runat="server"
    ID="labelMessage"
    ></asp:Label>

<h2>Monitor data (on-demand):</h2>
<table>
    <tr>
        <td>
            CPU Std Deviation:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelAvgMonitoringTotalProcessorTime"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            CPU Utilization:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelCpuUtilization"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Memory Survived:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelMonitoringSurvivedMemorySize"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Memory Utilization:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelMemoryUtilization"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:Button
                runat="server"
                ID="buttonReload"
                Text="Reload"
                ></asp:Button>
        </td>
    </tr>
</table>

<h2>Monitor data (real-time):</h2>
<table>
    <tr>
        <td>
            CPU Std Deviation:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelAvgMonitoringTotalProcessorTimeRt"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            CPU Utilization:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelCpuUtilizationRt"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Memory Survived:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelMonitoringSurvivedMemorySizeRt"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Memory Utilization:
        </td>
        <td>
            <asp:Label
                runat="server"
                ID="labelMemoryUtilizationRt"
                ForeColor="Green"
                ></asp:Label>
        </td>
    </tr>
</table>
