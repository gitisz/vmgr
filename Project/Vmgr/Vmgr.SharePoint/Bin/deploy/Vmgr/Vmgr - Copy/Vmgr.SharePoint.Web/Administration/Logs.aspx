﻿<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="Logs" 
    AutoEventWireup="true" 
    CodeBehind="Logs.aspx.cs" 
    Inherits="Vmgr.SharePoint.Logs, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register TagPrefix="Vmgr" TagName="LogsControl" Src="~/_CONTROLTEMPLATES/Vmgr/LogsControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Logs
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Logs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
    <ul style="list-style-type: none; margin: 0px 20px; padding: 0px;"><li style="text-align: justify;"> - Review all logs generated by the V-Manager system.</li></ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:LogsControl 
        ID="logsControl" 
        runat="server">
        </Vmgr:LogsControl>
</asp:Content>