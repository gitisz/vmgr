<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page    
    Title="" 
    Language="C#" 
    MasterPageFile="~site/_catalogs/MasterPage/Window.Master" 
    AutoEventWireup="true" 
    CodeBehind="WindowSecurityRoleEditor.aspx.cs" 
    Inherits="Vmgr.SharePoint.WindowSecurityRoleEditor, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="Vmgr" TagName="SecurityRoleEditorControl" Src="~/_CONTROLTEMPLATES/Vmgr/SecurityRoleEditorControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <Vmgr:SecurityRoleEditorControl 
        runat="server"
        ID="securityEditorControl" 
        >
    </Vmgr:SecurityRoleEditorControl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderFooterContent" runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">

        </script>
    </telerik:RadCodeBlock>
    <div style="text-align: center; width: 100%;">
        <table border="0" cellpadding="0" cellspacing="0" style="margin: 0px auto;">
            <tr>
                <td>
                    <telerik:RadButton 
                        runat="server" 
                        ID="buttonCancel"
                        Text="Cancel"
                        Width="120px"
                        Height="65px" 
                        AutoPostBack="false"
                        CausesValidation="false"
                        OnClientClicked="function(s, e){ OnClose(true); }"
                        >
                        <Icon 
                            PrimaryIconWidth="24" 
                            PrimaryIconHeight="24" 
                            PrimaryIconUrl="/_layouts/Images/Vmgr/cancel-icon-24.png" />
                    </telerik:RadButton>
                    <telerik:RadButton 
                        runat="server" 
                        ID="buttonOk"
                        Text="Save"
                        Width="120px"
                        Height="65px" 
                        AutoPostBack="false"
                        CausesValidation="true"
                        OnClientClicked="function(s, e){ OnSave(s, e); }"
                        >
                        <Icon 
                            PrimaryIconWidth="24" 
                            PrimaryIconHeight="24" 
                            PrimaryIconUrl="/_layouts/Images/Vmgr/hardware-floppy-icon-24.png" />
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div></asp:Content>
