<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="PerformanceChartControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.PerformanceChartControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<table style="width: 100%; padding: 0px; border-collapse: collapse;">
    <tr>
        <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/chart-top-left.png'); background-repeat: no-repeat;"></td>
        <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/chart-top-center.png'); background-repeat: repeat-x;"></td>
        <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/chart-top-right.png'); background-repeat: no-repeat;"></td>
    </tr>
    <tr>
        <td style="padding: 0px; width: 12px; background-image: url('/_layouts/images/Vmgr/chart-mid-left.png'); background-repeat: repeat-y;"></td>
        <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/chart-mid-center.png'); background-repeat: repeat;">
            <asp:PlaceHolder ID="PlaceHolderPerformanceChart" runat="server"></asp:PlaceHolder> 
        </td>
        <td style="padding: 0px; width: 12px; background-image: url('/_layouts/images/Vmgr/chart-mid-right.png'); background-repeat: repeat-y;"></td>
    </tr>
    <tr>
        <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/chart-bottom-left.png'); background-repeat: no-repeat;"></td>
        <td style="padding: 0px; background-image: url('/_layouts/images/Vmgr/chart-bottom-center.png'); background-repeat: repeat-x;"></td>
        <td style="padding: 0px; width: 12px; height: 12px; background-image: url('/_layouts/images/Vmgr/chart-bottom-right.png'); background-repeat: no-repeat;"></td>
    </tr>
</table>
