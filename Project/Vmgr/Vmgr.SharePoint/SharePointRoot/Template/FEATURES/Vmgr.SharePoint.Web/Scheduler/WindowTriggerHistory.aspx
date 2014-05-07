<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page    
    Title="" 
    Language="C#" 
    MasterPageFile="~site/_catalogs/MasterPage/Window.Master" 
    AutoEventWireup="true" 
    CodeBehind="WindowTriggerHistory.aspx.cs" 
    Inherits="Vmgr.SharePoint.WindowTriggerHistory, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" TagName="TriggerHistoryControl" Src="~/_CONTROLTEMPLATES/Vmgr/TriggerHistoryControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <Vmgr:TriggerHistoryControl 
        runat="server"
        ID="scheduleEditorControl" 
        >
    </Vmgr:TriggerHistoryControl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooterContent" runat="server" Visible="true">
    <div style="text-align: center; width: 100%;">
        <table border="0" cellpadding="0" cellspacing="0" style="margin: 0px auto;">
            <tr>
                <td>
                    <telerik:RadButton 
                        runat="server" 
                        ID="buttonRefresh"
                        Text="Refresh"
                        Width="120px"
                        Height="65px" 
                        Skin="Default"
                        AutoPostBack="false"
                        CausesValidation="false"
                        OnClientClicked="function(s, e){ OnRefreshTriggers(); }"
                        >
                        <Icon 
                            PrimaryIconWidth="24" 
                            PrimaryIconHeight="24" 
                            PrimaryIconUrl="/_layouts/images/Vmgr/refresh-icon-24.png" />
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
