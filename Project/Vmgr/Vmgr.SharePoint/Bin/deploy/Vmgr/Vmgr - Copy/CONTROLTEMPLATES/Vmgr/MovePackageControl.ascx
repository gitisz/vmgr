<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MovePackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.MovePackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="cc" %>
<telerik:RadAjaxLoadingPanel 
    ID="loadingPanelMovePackage" 
    runat="server" 
    Transparency="100" 
    BackColor="Gray" 
    IsSticky="true" 
    style="position: absolute; top: 0; left: 0" 
    Width="100%" 
    Height="100%" 
    />
<telerik:RadAjaxPanel 
    runat="server"
    ID="ajaxPanelMovePackage" 
    OnAjaxRequest="ajaxPanelMovePackage_AjaxRequest"
    >
    <asp:HiddenField
        runat="server"
        ID="hiddenFieldSelectedServer"
        />
    <div style="padding: 10px;">
        <h3>Moving Package: <asp:Label
            runat="server"
            ID="labelPackage"
            ></asp:Label>
            </h3>
        <h3>From Server: <asp:Label
            runat="server"
            ID="labelServer"
            ></asp:Label>
            </h3>
        Please select a new server for this package. <span style="font-style: italic">Note: Ensure the new server supports all dependencies required by the package!</span>
    </div>
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourceServer"
        OnSelecting="linqDataSourceServer_Selecting"
        ></asp:LinqDataSource>
</telerik:RadAjaxPanel>
<telerik:RadCodeBlock ID="codeBlockMovePackage" runat="server">
    <script type="text/javascript">

        var selectedServerUniqueId = $get('<%= hiddenFieldSelectedServer.ClientID %>').value;
        var previousServerUniqueId = null;
        var statusFunctionArray = new FunctionArray();
        var messageFunctionArray = new FunctionArray();
        var progressFunctionArray = new FunctionArray();

        function OnRowSelected(s, e) {

            var selectedItem = s.get_masterTableView().get_selectedItems()[0];

            previousServerUniqueId = selectedServerUniqueId;
            selectedServerUniqueId = selectedItem.getDataKeyValue('UniqueId');

            if (selectedServerUniqueId != $get('<%= hiddenFieldSelectedServer.ClientID %>').value) {

                var ajaxPanelMovePackage = $find('<%= ajaxPanelMovePackage.ClientID %>');
                ajaxPanelMovePackage.ajaxRequest('CONFIRM_MOVE_PACKAGE,' + selectedServerUniqueId);

                var loadingPanelMovePackage = $find('<%= loadingPanelMovePackage.ClientID %>');
                loadingPanelMovePackage.show("<%= this.Page.Form.ClientID %>");


                for (var p in messageFunctionArray.functionArray) {
                    messageFunctionArray.execute(messageFunctionArray.functionArray[p].key, null);
                }

                for (var p in progressFunctionArray.functionArray) {
                    progressFunctionArray.execute(progressFunctionArray.functionArray[p].key, null);
                }
            }
        }

        function OnConfirmMovePackageHandler(args) {

            if (args == true) {

                var data = new Object;
                data.Message = ' ';
                data.IsFaulted = false;
                data.PrimaryValue = 0;
                data.PrimaryTotal = 5;
                progressFunctionArray.execute(selectedServerUniqueId, data);

                var ajaxPanelMovePackage = $find('<%= ajaxPanelMovePackage.ClientID %>');
                ajaxPanelMovePackage.ajaxRequest('MOVE_PACKAGE,' + selectedServerUniqueId);

            } else {

                resetPackageSelection();
            }
        }

        function OnConfirmAssignPackageHandler(args) {

            if (args == true) {
                var ajaxPanelMovePackage = $find('<%= ajaxPanelMovePackage.ClientID %>');
                ajaxPanelMovePackage.ajaxRequest('LOAD_PACKAGE_DESTINATION,' + selectedServerUniqueId);
            } else {

                resetPackageSelection();

                for (var p in messageFunctionArray.functionArray) {
                    messageFunctionArray.execute(messageFunctionArray.functionArray[p].key, null);
                }

                for (var p in progressFunctionArray.functionArray) {
                    progressFunctionArray.execute(progressFunctionArray.functionArray[p].key, null);
                }
            }
        }

        function OnMovePackageFailHandler() {

            resetPackageSelection();

            for (var p in messageFunctionArray.functionArray) {
                messageFunctionArray.execute(messageFunctionArray.functionArray[p].key, null);
            }

            for (var p in progressFunctionArray.functionArray) {
                progressFunctionArray.execute(progressFunctionArray.functionArray[p].key, null);
            }
        }

        function OnPackageAssignedHandler(args) {

            previousServerUniqueId = args;

            $get('<%= hiddenFieldSelectedServer.ClientID %>').value = previousServerUniqueId;

            var ajaxPanelMovePackage = $find('<%= ajaxPanelMovePackage.ClientID %>');
            ajaxPanelMovePackage.ajaxRequest('REFRESH,');

            for (var p in messageFunctionArray.functionArray) {
                messageFunctionArray.execute(messageFunctionArray.functionArray[p].key, null);
            }

            for (var p in progressFunctionArray.functionArray) {
                progressFunctionArray.execute(progressFunctionArray.functionArray[p].key, null);
            }

            resetPackageSelection();
        }

        function OnPackageAssignedFailHandler() {

            resetPackageSelection();

            for (var p in messageFunctionArray.functionArray) {
                messageFunctionArray.execute(messageFunctionArray.functionArray[p].key, null);
            }

            for (var p in progressFunctionArray.functionArray) {
                progressFunctionArray.execute(progressFunctionArray.functionArray[p].key, null);
            }
        }

        function OnCloseMovePackage(s, e) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshPackages();
            }
            this.OnClose(s, e);
        }

        function resetPackageSelection() {

            var gridServer = $find('<%= gridServer.ClientID %>');
            var masterTable = gridServer.get_masterTableView();
            gridServer.clearSelectedItems();

            var rows = gridServer.get_masterTableView().get_dataItems();

            for (var i = 0; i < rows.length; i++) {
                if (rows[i].getDataKeyValue('UniqueId') == previousServerUniqueId) {
                    masterTable.selectItem(rows[i].get_element());
                    break;
                }
            }

            var loadingPanelMovePackage = $find('<%= loadingPanelMovePackage.ClientID %>');
            loadingPanelMovePackage.hide("<%= this.Page.Form.ClientID %>");
        }

    </script>
</telerik:RadCodeBlock>
<telerik:RadGrid 
    runat="server" 
    ID="gridServer" 
    DataSourceID="linqDataSourceServer"
    GridLines="None" 
    BorderStyle="None"
    AutoGenerateColumns="false"
    Width="100%" 
    AllowSorting="True" 
    AllowPaging="True" 
    AllowCustomPaging="true"
    PageSize="5" 
    OnItemDataBound="gridServer_ItemDataBound" 
    >
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="True"></Selecting>
        <ClientEvents OnRowSelected="OnRowSelected" />
    </ClientSettings>                            
    <MasterTableView 
        AllowPaging="true" 
        AllowCustomPaging="true"
        AutoGenerateColumns="false" 
        HierarchyLoadMode="ServerOnDemand" 
		DataKeyNames="ServerId,UniqueId" 
		ClientDataKeyNames="ServerId,UniqueId" 
        >
        <Columns>
            <telerik:GridTemplateColumn
                UniqueName="Name" 
                HeaderText="Server Name"
                HeaderStyle-Width="100%" 
                SortExpression="Name"
                >
                <ItemTemplate>
                    <div style="width: 100%;">
                        <div style="padding: 5px;">
                            <div>
                                <asp:Image
                                    runat="server"
                                    ID="imageServerStatus"
                                    style="float:left; margin: 5px 5px 5px 0px; width: 32px; height: 32px; vertical-align:middle"
                                    ImageAlign="AbsBottom"
                                    ImageUrl="/_layouts/images/Vmgr/trafficlight-red-icon-32.png"
                                    />
                                <div>
                                    <div style="margin-top: 11px; padding: 10px; border: none;">
                                        <span style="font-size: 12pt; text-decoration: none;"><%# Eval("Name") %></span><span style=" margin: 5px;">{<%# Eval("UniqueId") %>}</span>   
                                    </div>
                                </div> 
                                <span style="font-style: italic; margin: 5px;"><%# Eval("Description") %>&nbsp;</span>
                            </div>
                            <br style="margin: -2px;" />
                            <div style="padding:2px;">
                                <asp:Label 
                                    ID="progressUpdateMessage" 
                                    runat="server"></asp:Label>
                            </div>
                            <div id="progressUpdateBar" style="border: none;" runat="server"></div>
                        </div>
                    </div>
                    <script>
                        var signalrScriptLoaded<%# Eval("ServerId") %> = false;

                        signalrFunctionHubRegistry.add('<%# Eval("UniqueId") %>', function () {

                            if (!signalrScriptLoaded<%# Eval("ServerId") %>) {

                                $.getScript('<%# Eval("HubConnectionUrl") + "/signalr/hubs" %>')
                                    .done(function (script, status) {

                                        signalrScriptLoaded<%# Eval("ServerId") %> = true;

                                        var connectionMoveProgress = $.hubConnection("<%# Eval("HubConnectionUrl") %>" + "/signalr");
                                        var moveProgressHub = connectionMoveProgress.createHubProxy('VmgrHub');

                                        moveProgressHub.on('moveProgress', (function (obj) {
                                            messageFunctionArray.execute(selectedServerUniqueId, obj.Message);
                                            progressFunctionArray.execute(selectedServerUniqueId, obj);
                                        }
                                        )
                                        )
                                        ;

                                        var start = function () {
                                            connectionMoveProgress.start({})
                                                .then(function () {
                                                    moveProgressHub.invoke("addToGroup", "<%= this.groupKey %>");
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

                        statusFunctionArray.add('<%# Eval("UniqueId") %>', function () {
                            $.ajax({
                                type: "POST",
                                url: "<%# pollingServiceUrl %>",
                                cache: false,
                                contentType: "application/json; charset=utf-8",
                                data: "{ 'serverId':'<%# Eval("UniqueId") %>' }",
                                dataType: "json",
                                success: function (data) {
                                    var imageServerStatus = $("#<%# Container.FindControl("imageServerStatus").ClientID %>");

                                    signalrFunctionHubRegistry.execute('<%# Eval("UniqueId") %>', null); 

                                    if (data.d) {
                                        imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-green-icon-32.png");
                                    } else {
                                        imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-red-icon-32.png");
                                    }
                                },
                                error: function () {
                                    var imageServerStatus = $("#<%# Container.FindControl("imageServerStatus").ClientID %>");
                                    imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-red-icon-32.png");
                                }
                            });
                        }
                        )
                        ;

                        messageFunctionArray.add('<%# Eval("UniqueId") %>', function (message) {

                            var progressUpdateMessage = "#<%# Container.FindControl("progressUpdateMessage").ClientID %>";

                            $(progressUpdateMessage).empty();
                            $(progressUpdateMessage).append(message);
                        }
                        )
                        ;

                        progressFunctionArray.add('<%# Eval("UniqueId") %>', function (obj) {
                            
                            var progressBar = $("#<%# Container.FindControl("progressUpdateBar").ClientID%>");
                            var progressbarValue = progressBar.find(".ui-progressbar-value");

                            if (obj != null) {

                                progressBar.show();

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

                                    var ajaxPanelMovePackage = $find('<%= ajaxPanelMovePackage.ClientID %>');
                                    ajaxPanelMovePackage.ajaxRequest('REFRESH,');

                                    previousServerUniqueId = obj.ServerUniqueId;
                                    $get('<%= hiddenFieldSelectedServer.ClientID %>').value = obj.ServerUniqueId;

                                    resetPackageSelection();
                                }
                            }
                            else {
                                progressBar.hide();
                            }
                        }
                        )
                        ;

                        statusFunctionArray.execute('<%# Eval("UniqueId") %>', null);
                        messageFunctionArray.execute('<%# Eval("UniqueId") %>', ' ');

                    </script>

                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
        <NoRecordsTemplate>
            <div style="padding: 5px;">
                No servers found.
            </div>
        </NoRecordsTemplate>
    </MasterTableView>
    <ClientSettings 
        AllowDragToGroup="false" 
        AllowColumnsReorder="false" 
        />
    <PagerStyle 
        Mode="NextPrevAndNumeric" 
        HorizontalAlign="Center" 
        PageSizeLabelText="Results Per Page"
        AlwaysVisible="true" 
        />
    <FilterMenu 
        EnableImageSprites="False" 
        />
    <HeaderContextMenu 
        CssClass="GridContextMenu GridContextMenu_Default" 
        />
</telerik:RadGrid>
