<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="SelectServerControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.ServerControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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
        function OnServerRowSelected(s, e) {
            var row = s.get_selectedItems()[0];
            var uniqueId = row.getDataKeyValue('UniqueId');
            var ajaxPanelServer = $find('<%= ajaxPanelServer.ClientID %>');
            ajaxPanelServer.ajaxRequest('SELECT_SERVER,' + uniqueId);
        }

        function OnSelectServerComplete() {
            var masterTable = $find("<%= gridServer.ClientID %>").get_masterTableView();
            masterTable.rebind();
            <%= javascriptRedirectUrl %>
        }

        function OnPollingServiceSuccess(id, data, status) {
            var imageServerStatus = $("#" + id);

            if (data.d) {
                imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-green-icon-32.png");
            } else {
                imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-red-icon-32.png");
            }
        }

        function OnPollingServiceFail(xmlRequest) {
        }

    </script>
</telerik:RadCodeBlock>
<div id="pageWrapper" class="pageWrapper" style="height: 100%;">
    <telerik:RadAjaxLoadingPanel 
        runat="server"
        ID="ajaxLoadingPanelServer" 
        Skin="Default" 
        ZIndex="2999"
        ></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel
        runat="server"
        ID="ajaxPanelServer"
        Width="100%"
        LoadingPanelID="ajaxLoadingPanelServer"
        OnAjaxRequest="ajaxPanelServer_AjaxRequest"
        >
        <div style="padding: 5px; padding-bottom: 0px;">
            <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
                <div style="position: absolute; top: 5px; width: 100%;">
                    <div style="position: absolute; top: -3; right: 0; float:right; text-align: left;">
                    </div>
                    <telerik:RadTabStrip 
                        runat="server" 
                        ID="tabStripServer" 
                        MultiPageID="multipageServer" 
                        SelectedIndex="0"
                        CausesValidation="false"
                        >
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Servers" PageViewID="pageViewServers">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                </div>
            </div>
        </div>
        <div style="padding: 5px; padding-top: 0px;">
            <div style="border: solid 1px gray; border-top: none; background: white; font-size: 8pt;">
                <telerik:RadMultiPage 
                    runat="server" 
                    ID="multipageServer" 
                    SelectedIndex="0" 
                    RenderSelectedPageOnly="false"
                    >
                    <telerik:RadPageView runat="server" ID="pageViewServers">   
                        <asp:LinqDataSource
                            runat="server"
                            ID="linqDataSourceServer"
                            OnSelecting="linqDataSourceServer_Selecting"
                            ></asp:LinqDataSource>
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
                            <ClientSettings 
                                EnableRowHoverStyle="true">
                                <Selecting 
                                    AllowRowSelect="True"
                                    ></Selecting>
                                <ClientEvents
                                    OnRowSelected="OnServerRowSelected"
                                    />
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
                                            <div style="padding: 5px;">
                                                <asp:Image
                                                    runat="server"
                                                    ID="imageServerStatus"
                                                    style="float:left; margin: 5px; width: 32px; height: 32px; vertical-align:middle"
                                                    ImageAlign="AbsBottom"
                                                    ImageUrl="/_layouts/images/Vmgr/trafficlight-red-icon-32.png"
                                                    />
                                                <div style="margin-top: 11px;margin-left: 5px;">
                                                    <span style="font-size: 12pt; text-decoration: underline;"><%# Eval("Name") %></span><span style=" margin: 5px;">{<%# Eval("UniqueId") %>}</span>   
                                                </div> 
                                                <br />
                                                <span style="font-style: italic; margin: 5px;"><%# Eval("Description") %>&nbsp;</span>
                                            </div>
                                            <asp:Literal
                                                ID="literalScript"
                                                runat="server"
                                                ></asp:Literal>

                                            <script>


                                                function OnIsStarted<%# Eval("ServerId") %>() {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: "<%# pollingServiceUrl %>",
                                                            cache: false,
                                                            contentType: "application/json; charset=utf-8",
                                                            data: "{ 'serverId':'<%# Eval("UniqueId") %>' }",
                                                            dataType: "json",
                                                            success: OnPollingServiceSuccess<%# Eval("ServerId") %>,
                                                            error: OnPollingServiceFail
                                                        });
                                                    }

                                                OnIsStarted<%# Eval("ServerId") %>();

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
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</div>

