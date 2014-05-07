<%@ Page 
    Language="C#" 
    MasterPageFile="~masterurl/default.master" 
    Title="Package Manager" 
    AutoEventWireup="true" 
    CodeBehind="PackageManager.aspx.cs" 
    Inherits="Vmgr.SharePoint.PackageManager, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="Vmgr" Namespace="Vmgr.SharePoint" Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" %>
<%@ Register TagPrefix="Vmgr" TagName="PackageManagerControl" Src="~/_CONTROLTEMPLATES/Vmgr/PackageManagerControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowUploadPackageControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowUploadPackageControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowFilterPackageControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowFilterPackageControl.ascx" %>
<%@ Register TagPrefix="Vmgr" TagName="WindowMovePackageControl" Src="~/_CONTROLTEMPLATES/Vmgr/WindowMovePackageControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Package Manager
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Package Manager
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderTitleDescription" runat="server">
     <ul style="list-style-type: none; margin: 0px 20px; padding: 0px;"><li style="text-align: justify;"> - View details about each package, and expand to view more information such as plugins and assemblies each package contains.  You can also upload new packages, or overwrite existing ones.</li></ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <Vmgr:WindowMovePackageControl 
        runat="server"
        ID="windowMovePackageControl" 
        >
    </Vmgr:WindowMovePackageControl>
    <Vmgr:WindowUploadPackageControl 
        runat="server"
        ID="windowUploadPackageControl1" 
        >
    </Vmgr:WindowUploadPackageControl>
    <Vmgr:WindowFilterPackageControl 
        runat="server"
        ID="windowFilterPackageControl" 
        FilterType="PackageMetaData"
        >
    </Vmgr:WindowFilterPackageControl>
    <Vmgr:PackageManagerControl 
        ID="pluginManagerControl" 
        runat="server">
        </Vmgr:PackageManagerControl>
</asp:Content>
