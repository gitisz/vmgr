<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="Logs" 
    AutoEventWireup="true" 
    CodeBehind="Security.aspx.cs" 
    Inherits="Vmgr.SharePoint.Logs, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register TagPrefix="Vmgr" TagName="SecurityControl" Src="~/_CONTROLTEMPLATES/Vmgr/SecurityControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowSecurityMembershipEditorControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowSecurityMembershipEditorControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowSecurityRoleEditorControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowSecurityRoleEditorControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowSecurityRolePermissionEditorControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowSecurityRolePermissionEditorControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Security
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Security
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:SecurityControl 
        ID="securityControl" 
        runat="server">
        </Vmgr:SecurityControl>
    <Vmgr:WindowSecurityMembershipEditorControl 
        runat="server"
        ID="windowSecurityMembershipEditorControl" 
        >
    </Vmgr:WindowSecurityMembershipEditorControl>
    <Vmgr:WindowSecurityRoleEditorControl 
        runat="server"
        ID="windowSecurityRoleEditorControl" 
        >
    </Vmgr:WindowSecurityRoleEditorControl>
    <Vmgr:WindowSecurityRolePermissionEditorControl 
        runat="server"
        ID="windowSecurityRolePermissionEditorControl" 
        >
    </Vmgr:WindowSecurityRolePermissionEditorControl>
</asp:Content>
