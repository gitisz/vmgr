<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="UploadPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.UploadPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">
        function OnClientFilesUploadedPackage(s, e) {
            var ajaxPanelPackage = $find('<%= ajaxPanelPackage.ClientID %>');
            ajaxPanelPackage.ajaxRequest('PLUGIN_PACKAGES_UPLOADED');
        }

        function OnClientBeforeShowWindowUploadProgress(s, e) {
            s.set_width($(window).width() * .8);
        }

        function OnClientShowWindowUploadProgress(s, e) {
            var popupControlDiv = document.getElementById(s.get_id() + '_C');
            popupControlDiv.style.overflow = 'hidden';
        }

        function OnClientBeforeCloseWindowUploadProgress(s, e) {
        }

        function OnClientFilesSelected(s, e) {

            $("#progressUpdateBar").progressbar({
                value: 0
            });

            $("#progressTitle")
                .empty()
            ;

            $("#progressTitle")
                .append('')
            ;
        }

        var signalrUploadScriptLoaded = false;
        var reconnectAttempts = 0;

        signalrFunctionHubRegistry.add('UPLOAD_PACKAGE_SCRIPT', function () {

            if (!signalrUploadScriptLoaded) {

                $.getScript('<%= (this.Page as BasePage).GetHubConnectionUrl() + "/signalr/hubs" %>')
                    .done(function (script, status) {

                        signalrUploadScriptLoaded = true;

                        var connectionUploadProgress = $.hubConnection("<%= (this.Page as BasePage).GetHubConnectionUrl() %>");
                        var uploadProgressHub = connectionUploadProgress.createHubProxy('VmgrHub');

                        connectionUploadProgress.reconnected(function () {
                            reconnectAttempts = 0;
                            updateProgressMessage("Connection re-established: " + new Date().toTimeString() + "");
                            enableUpload();
                        });

                        connectionUploadProgress.error(function (err) {

                            if (err)
                                updateProgressMessage("Error: " + err.message);
                            else
                                updateProgressMessage("An unknown error occurred.");
                        });

                        connectionUploadProgress.disconnected(function () {
                            if (reconnectAttempts < 5) {
                                reconnectAttempts++;
                                setTimeout(function () {
                                    connectionUploadProgress.start({})
                                        .then(function () {
                                            uploadProgressHub.invoke("addToGroup", "<%= this.groupKey %>");
                                            enableUpload();
                                        });
                                    ;
                                }, 1000);
                            }

                            updateProgressMessage("Disconnected.");

                            disableUpload();
                        }
                        )
                        ;

                        connectionUploadProgress.reconnecting(function () {
                            disableUpload();
                        }
                        )
                        ;

                        connectionUploadProgress.stateChanged(function (change) {

                            var oldState = null,
                                newState = null;

                            for (var p in $.signalR.connectionState) {
                                if ($.signalR.connectionState[p] === change.oldState) {
                                    oldState = p;
                                }

                                if ($.signalR.connectionState[p] === change.newState) {
                                    newState = p;
                                }
                            }

                            connectionState = newState;
                            connectionStateMsg = "Current state: " + newState + ".";

                            var changedStateMsg = 'Connection state changed from: ' + oldState + ' to: ' + newState + ".";

                            updateProgressMessage(changedStateMsg);

                        });

                        uploadProgressHub.on('uploadProgress', function (obj) {
                            updateProgressMessage(obj.Message);

                            var progressBar = $("#progressUpdateBar");
                            var progressbarValue = progressBar.find(".ui-progressbar-value");

                            progressBar.progressbar({
                                value: obj.PrimaryValue / obj.PrimaryTotal * 100
                            });

                            if (obj.IsFaulted) {
                                progressbarValue.css({
                                    "background": 'url(/_layouts/images/vmgr/red-gradient-background.png) repeat-x scroll 0px'
                                });
                            } else {
                                if (obj.PrimaryValue == 0) {
                                    progressBar.progressbar("option", "value", false);
                                    var progressbarOverlay = progressBar.find(".ui-progressbar-overlay");
                                    progressbarOverlay.css({
                                        "background": 'url(/_layouts/images/vmgr/progress-indeterminate.gif) repeat-x scroll 0px'
                                    });
                                    messageFunctionArray.execute('<%# Eval("UniqueId") %>', 'Attempting to contact server.');
                                    } else {
                                        progressbarValue.css({
                                            "background": 'url(/_layouts/images/vmgr/green-gradient-background.png) repeat-x scroll 0px'
                                        });
                                    }
                                }

                            if (obj.PrimaryValue == obj.PrimaryTotal) {
                                var parentWindow = getParentWindow();

                                if (parentWindow != null) {
                                    parentWindow.BrowserWindow.OnRefreshPackages();

                                }
                            }
                            ;

                        }
                        )
                        ;

                        var start = function () {
                            connectionUploadProgress.start({})
                                .then(function () {
                                    uploadProgressHub.invoke("addToGroup", "<%= this.groupKey %>");
                                        enableUpload();
                                    });
                            };

                        start();
                    }
                    )
                    .fail(function (jqxhr, settings, exception) {
                    }
                    )
                    ;
            }
        }
        )
        ;


        function enableUpload() {

            var asycUploadPackage = $find('<%= asycUploadPackage.ClientID %>');
            asycUploadPackage.set_enabled(true);

            var buttonCheckboxOverwrite = $find('<%= buttonCheckboxOverwrite.ClientID %>');
            buttonCheckboxOverwrite.set_enabled(true);

            var labelProgressMessage = $get('<%= labelProgressMessage.ClientID %>');
            labelProgressMessage.style.color = "green";

            $(labelProgressMessage)
                .empty()
            ;

            $(labelProgressMessage)
                .append("Upload plugin package <span style='font-style: italic;'>(*.vmgx)</span>")
            ;
        }

        function disableUpload() {
            var asycUploadPackage = $find('<%= asycUploadPackage.ClientID %>');

            if (asycUploadPackage != null) {
                asycUploadPackage.set_enabled(false);
            }

            var buttonCheckboxOverwrite = $find('<%= buttonCheckboxOverwrite.ClientID %>');
            if (buttonCheckboxOverwrite != null) {
                buttonCheckboxOverwrite.set_enabled(false);
            }

            var labelProgressMessage = $get('<%= labelProgressMessage.ClientID %>');

            if (labelProgressMessage != null) {
                labelProgressMessage.style.color = "orange";

                $(labelProgressMessage)
                    .empty()
                ;

                $(labelProgressMessage)
                    .append("Please wait for a connection to be established...")
                ;
            }
        }

        function updateProgressMessage(msg) {

            $('#progressUpdateMessage').prop('title', msg);

            $("#progressUpdateMessage")
                .empty()
            ;

            $("#progressUpdateMessage")
                .append(msg)
            ;
        }

        function loadUploadPackage() {

            signalrFunctionHubRegistry.execute('UPLOAD_PACKAGE_SCRIPT', null);

            var asycUploadPackage = $find('<%= asycUploadPackage.ClientID %>');
            asycUploadPackage.set_enabled(false);

            var buttonCheckboxOverwrite = $find('<%= buttonCheckboxOverwrite.ClientID %>');
            buttonCheckboxOverwrite.set_enabled(false);
            
            $("#progressUpdateBar").progressbar({
                value: 0
            });

            $("#progressTitle")
                .empty()
            ;

            $("#progressTitle")
                .append('')
            ;

        }

        function OnloadUploadPackage() { loadUploadPackage(); Sys.Application.remove_load(OnloadUploadPackage); }

        Sys.Application.add_load(OnloadUploadPackage);


   </script>
   <style type="text/css">
        DIV.ProgressRw TD.rwTopLeft,
        DIV.ProgressRw TD.rwTopRight,
        DIV.ProgressRw TD.rwFooterLeft,
        DIV.ProgressRw TD.rwFooterRight,
        DIV.ProgressRw TD.rwFooterCenter,
        DIV.ProgressRw TD.rwBodyLeft,
        DIV.ProgressRw TD.rwBodyRight,
        DIV.ProgressRw TD.rwTitlebar,
        DIV.ProgressRw TD.rwTopResize,
        DIV.ProgressRw TD.rwWindowContent,
        DIV.ProgressRw TD.rwWindowContent
        {  
            background-image: none !important; 
            background-color: transparent !important;
        }
    
        div.rwWindowContent, div.radconfirm
        {  
            background-image: url(/_layouts/images/Vmgr/warning-icon-48.png) !important; 
            font-size: 10pt !important;
        } 

        div.rwDialogPopup
        {
            padding-left: 60px !important;
        }

        .RadUploadProgressArea_Default 
            ul.ruProgress  { background: none !important; background-color: white !important; }

        .RadUploadProgressArea_Default 
            ul.ruProgress  .ruBar { background: none !important; background-color: #CCC !important; }

        .ruButton .ruRemove, .RadUpload .ruRemove,
        .ruButton .ruCancel, .RadUpload .ruCancel
        {
            display:none !important;
        }
   </style>
</telerik:RadCodeBlock>
            
<div style="width: 100%;">
    <div style="padding: 10px;">
        <table border="0" style="width: 100%; border-collapse: collapse; padding: 0px;">
            <tr>
                <td>
                    <asp:Label
                        runat="server"
                        ID="labelProgressMessage"
                        ForeColor="Orange"
                        >Please wait for a connection to be established...</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <telerik:RadAjaxPanel 
        runat="server"
        ID="ajaxPanelPackage" 
        OnAjaxRequest="ajaxPanelPackage_AjaxRequest"
        >
        <div style="padding: 10px;">
            <table border="0" style="width: 100%; border-collapse: collapse; padding: 0px;">
                <tr>
                    <td>
                        <telerik:RadAsyncUpload
                            runat="server"
                            ID="asycUploadPackage"
                            MultipleFileSelection="Automatic"
                            AllowedFileExtensions="vmgx"
                            MaxFileInputsCount="10"
                            UseApplicationPoolImpersonation="true"
                            OnClientFilesSelected="OnClientFilesSelected"
                            OnClientFilesUploaded="OnClientFilesUploadedPackage"
                            OnFileUploaded="asycUploadPackage_FileUploaded"
                            >
                        </telerik:RadAsyncUpload>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadButton 
                            runat="server"
                            ID="buttonCheckboxOverwrite" 
                            ButtonType="ToggleButton" 
                            ToggleType="CheckBox"
                            BorderWidth="0" 
                            BackColor="transparent" 
                            AutoPostBack="false"
                            ToolTip="&nbsp;Overwrite package if already present."
                            Text="&nbsp;Overwrite package if already present."
                            >
                            <ToggleStates>
                                <telerik:RadButtonToggleState />
                                <telerik:RadButtonToggleState />
                            </ToggleStates>
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
    </telerik:RadAjaxPanel>
</div>
<div style="width: 100%;">
    <div style="padding: 10px;">
        <table border="0" style="width: 100%; border-collapse: collapse; padding: 0px;">
            <tr>
                <td>
                    <p id="progressTitle"></p>
                    <span id="progressUpdateMessage"></span>
                    <div id="progressUpdateBar" style="margin-top: 3px;"></div>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <p style="padding: 3px;"><span style="font-style: italic;">Note: Once a package is selected, the upload will begin automatically.</span></p>
                </td>
            </tr>
        </table>
    </div>
</div>
