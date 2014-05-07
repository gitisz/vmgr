<%@ Page 
    Language="C#" 
    Title="Server Selection" 
    AutoEventWireup="true" 
    CodeBehind="Server.aspx.cs" 
    Inherits="Vmgr.SharePoint.Server, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" TagName="SelectServerControl" Src="~/_CONTROLTEMPLATES/Vmgr/SelectServerControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Server Selection
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Server Selection
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:SelectServerControl 
        ID="selectServerControl" 
        runat="server">
        </Vmgr:SelectServerControl>
</asp:Content>
