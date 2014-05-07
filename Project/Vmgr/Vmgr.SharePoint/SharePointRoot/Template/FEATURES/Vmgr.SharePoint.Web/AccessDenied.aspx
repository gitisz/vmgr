<%@ Page 
    Language="C#" 
    MasterPageFile="~site/_catalogs/MasterPage/Base.Master" 
    Title="Access Denied" 
    AutoEventWireup="true" 
    CodeBehind="AccessDenied.aspx.cs" 
    Inherits="Vmgr.SharePoint.AccessDenied, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Access Denied
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Access Denied
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="padding: 5px;">
        Access to this site is not permitted!
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>  
</asp:Content>
