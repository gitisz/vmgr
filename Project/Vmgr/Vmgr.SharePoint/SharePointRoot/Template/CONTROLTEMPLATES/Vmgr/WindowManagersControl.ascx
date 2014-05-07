<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="WindowManagersControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.WindowManagersControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<style type="text/css">

    div.rwWindowContent, div.radalert, div.radalert td
    {  
        background-image: none !important; 
        font-size: 10pt !important;
    } 

    div.rwWindowContent, div.radconfirm, div.radconfirm td
    {  
        background-image: none !important; 
        font-size: 10pt !important;
    } 

</style>
<telerik:RadScriptBlock ID="radScriptBlock" runat="server">
    <script type="text/javascript">
        var confirmWindowComment;
        var confirmWindowSendEmail;

        function OnClientShow(s, e) {
        
        }

        function OnClientBeforeClose(s, e) {
           
        }

        function OnCloseComment() {
            confirmWindowComment = $get('textBoxComment').value;
        }

        function OnCloseCommentEmail() {
            confirmWindowComment = $get('textBoxComment').value;
            confirmWindowSendEmail = $get('checkBoxSendEmail').checked;
        }
        
        function OnProvideCommentVisible(visible) {
            if (visible) {
                $('#tableConfirmComment').attr('style', 'visibility: visible');
            }
            else {
                $('#tableConfirmComment').attr('style', 'visibility: collapse');
            }
        }

        function getWindowManagerAlertCheck() {
            return $find("<%=windowManagerAlertCheck.ClientID %>");
        }

        function getWindowManagerAlertError() {
            return $find("<%=windowManagerAlertError.ClientID %>");
        }

    </script>
</telerik:RadScriptBlock>
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerAlertCheck" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <AlertTemplate>
		<div class="rwDialogPopup radalert" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/green-checkmark-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
				            <a  onclick="$find('{0}').close(true);"
				            class="rwPopupButton" href="javascript:void(0);">
					            <span class="rwOuterSpan">
						            <span class="rwInnerSpan">##LOC[OK]##</span>
					            </span>
				            </a>				
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </AlertTemplate>
</telerik:RadWindowManager>    
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerAlertError" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <AlertTemplate>
		<div class="rwDialogPopup radalert" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/warning-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
				            <a  onclick="$find('{0}').close(true);"
				            class="rwPopupButton" href="javascript:void(0);">
					            <span class="rwOuterSpan">
						            <span class="rwInnerSpan">##LOC[OK]##</span>
					            </span>
				            </a>				
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </AlertTemplate>
</telerik:RadWindowManager>
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerAlertInfo" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <AlertTemplate>
		<div class="rwDialogPopup radalert" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/info-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
				            <a  onclick="$find('{0}').close(true);"
				            class="rwPopupButton" href="javascript:void(0);">
					            <span class="rwOuterSpan">
						            <span class="rwInnerSpan">##LOC[OK]##</span>
					            </span>
				            </a>				
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </AlertTemplate>
</telerik:RadWindowManager>
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerConfirm" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <ConfirmTemplate>
		<div class="rwDialogPopup radconfirm" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/info-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="padding-top: 5px;">
                            <a onclick="$find('{0}').close(true);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[Yes]##</span></span></a>
                            <a onclick="$find('{0}').close(false);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[No]##</span></span></a>
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </ConfirmTemplate>
</telerik:RadWindowManager>    
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerConfirmComment" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <ConfirmTemplate>
		<div class="rwDialogPopup radconfirm" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/info-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <table id="tableConfirmComment" style="visibility: visible; width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td>
                                        Provide a comment: <span style="color: Gray;">(Optional)</span> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <textarea
                                            id="textBoxComment"
                                            style="width: 300px; border: solid 1px #CCC;"
                                            rows="5"
                                            cols="1"
                                            ></textarea>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="padding-top: 5px;">
                            <a onclick="OnCloseComment();$find('{0}').close(true);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[Yes]##</span></span></a>
                            <a onclick="$find('{0}').close(false);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[No]##</span></span></a>
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </ConfirmTemplate>
</telerik:RadWindowManager>    
<telerik:RadWindowManager 
    runat="server" 
    ID="windowManagerConfirmCommentEmail" 
    EnableShadow="true"
    OnClientShow="OnClientShow"
    OnClientBeforeClose="OnClientBeforeClose"
    >
    <ConfirmTemplate>
		<div class="rwDialogPopup radconfirm" style="padding: 10px;">			
			<div class="rwDialogText">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td style="padding: 10px 15px 0px 5px; vertical-align: top; width: 60px;">
                            <asp:Image
                                ID="imageIcon"
                                ImageUrl="/_layouts/Images/Vmgr/info-icon-48.png"
                                style="vertical-align: baseline;"
                                runat="server"
                                />
                        </td>
                        <td>
                            {1}  
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input 
                                id="checkBoxSendEmail"
                                onclick="OnProvideCommentVisible(this.checked);"
                                type="checkbox"
                                />
                            <span> Yes, I would also like to send an email.</span>
                            <br />
                            <br />
                            <table id="tableConfirmComment" style="visibility: collapse; width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td>
                                        Provide a comment: <span style="color: Gray;">(Optional)</span> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <textarea
                                            id="textBoxComment"
                                            style="width: 300px; border: solid 1px #CCC;"
                                            rows="5"
                                            cols="1"
                                            ></textarea>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="padding-top: 5px;">
                            <a onclick="OnCloseCommentEmail();$find('{0}').close(true);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[Yes]##</span></span></a>
                            <a onclick="$find('{0}').close(false);" class="rwPopupButton" href="javascript:void(0);">
                                <span class="rwOuterSpan"><span class="rwInnerSpan">##LOC[No]##</span></span></a>
                        </td>
                    </tr>
                </table>
			</div>
		</div>
    </ConfirmTemplate>
</telerik:RadWindowManager>    

