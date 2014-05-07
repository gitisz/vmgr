<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="History" 
    AutoEventWireup="true" 
    CodeBehind="Job.aspx.cs" 
    Inherits="Vmgr.SharePoint.Job, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register TagPrefix="Vmgr" TagName="JobControl" Src="~/_CONTROLTEMPLATES/Vmgr/JobControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowTriggerHistoryControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowTriggerHistoryControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowFilterJobControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowFilterJobControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    History
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    History
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
    <ul style="list-style-type: none; margin: 0px 20px; padding: 0px;"><li style="text-align: justify;"> - Review the history of previously run jobs.  Also, current jobs will show its real-time status.  Click on any key to see a list of all tiggers that were invoked for that job.</li></ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:WindowTriggerHistoryControl 
        runat="server"
        ID="windowTriggerHistoryControl" 
        >
    </Vmgr:WindowTriggerHistoryControl>
    <Vmgr:WindowFilterJobControl 
        runat="server"
        ID="windowFilterJobControl" 
        FilterType="JobMetaData"
        >
    </Vmgr:WindowFilterJobControl>
    <Vmgr:JobControl 
        ID="jobControl" 
        runat="server">
        </Vmgr:JobControl>
</asp:Content>
