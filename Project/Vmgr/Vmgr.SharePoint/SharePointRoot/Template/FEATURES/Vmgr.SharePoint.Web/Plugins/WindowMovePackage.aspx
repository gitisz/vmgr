<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page    
    Title="" 
    Language="C#" 
    MasterPageFile="~site/_catalogs/MasterPage/Window.Master" 
    AutoEventWireup="true" 
    CodeBehind="WindowMovePackage.aspx.cs" 
    Inherits="Vmgr.SharePoint.Plugins.WindowMovePackage, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" TagName="MovePackageControl" Src="~/_CONTROLTEMPLATES/Vmgr/MovePackageControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <Vmgr:MovePackageControl 
        runat="server"
        ID="uploadPackageControl" 
        >
    </Vmgr:MovePackageControl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooterContent" runat="server">
</asp:Content>
