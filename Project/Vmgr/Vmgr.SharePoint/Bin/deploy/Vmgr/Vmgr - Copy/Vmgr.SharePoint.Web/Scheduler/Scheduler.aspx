<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="Scheduler" 
    AutoEventWireup="true" 
    CodeBehind="Scheduler.aspx.cs" 
    Inherits="Vmgr.SharePoint.Scheduler, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register TagPrefix="Vmgr" TagName="SchedulerControl" Src="~/_CONTROLTEMPLATES/Vmgr/SchedulerControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowScheduleEditorControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowScheduleEditorControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowFilterScheduleControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowFilterScheduleControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Scheduler
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Scheduler
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
    <ul style="list-style-type: none; margin: 0px 20px; padding: 0px;"><li style="text-align: justify;"> - Create schedules for your plugin so jobs can be triggered at regular intervals.</li></ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:WindowScheduleEditorControl 
        runat="server"
        ID="windowScheduleEditorControl" 
        >
    </Vmgr:WindowScheduleEditorControl>
    <Vmgr:WindowFilterScheduleControl 
        runat="server"
        ID="windowFilterScheduleControl" 
        FilterType="ScheduleMetaData"
        >
    </Vmgr:WindowFilterScheduleControl>
    <Vmgr:SchedulerControl 
        ID="SchedulerControl" 
        runat="server">
        </Vmgr:SchedulerControl>
</asp:Content>
