<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MonitorPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.MonitorPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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
        var selectedPackageUniqueId = null;
        
        function OnSave(s, e) {
            var gridPackage = $find("<%=gridPackage.ClientID %>");
            var selectedItem = gridPackage.get_masterTableView().get_selectedItems()[0];
            selectedPackageUniqueId = selectedItem.getDataKeyValue('UniqueId');
            var ajaxPanelMonitorPackage = $find('<%= ajaxPanelMonitorPackage.ClientID %>');
            ajaxPanelMonitorPackage.ajaxRequest('CONFIRM_MONITOR_PACKAGE,' + selectedPackageUniqueId);
        }

        function OnConfirmMonitorPackageHandler(args) {
            var ajaxPanelMonitorPackage = $find('<%= ajaxPanelMonitorPackage.ClientID %>');
            if (args == true) {
                ajaxPanelMonitorPackage.ajaxRequest('MONITOR_PACKAGE,' + selectedPackageUniqueId);
            }
        }

        function OnCloseMonitorPackage(s, e) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshMonitors();
            }
            this.OnClose(s, e);
        }
   
   </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelMonitorPackage" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel 
    runat="server"
    ID="ajaxPanelMonitorPackage" 
    LoadingPanelID="ajaxLoadingPanelMonitorPackage"
    OnAjaxRequest="ajaxPanelMonitorPackage_AjaxRequest"
    >
    <div style="padding: 10px;">
        <h3>
            Monitor Package: 
        </h3>
    Please select a package you wish to monitor.
    </div>
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourcePackage"
        OnSelecting="linqDataSourcePackage_Selecting"
        ></asp:LinqDataSource>
    <telerik:RadGrid 
        runat="server" 
        ID="gridPackage" 
        DataSourceID="linqDataSourcePackage"
        GridLines="None" 
        BorderStyle="None"
        AutoGenerateColumns="false"
        Width="100%" 
        AllowSorting="True" 
        AllowPaging="True" 
        AllowCustomPaging="true"
        PageSize="5" 
        OnItemDataBound="gridPackage_ItemDataBound" 
        >
        <ClientSettings 
            EnableRowHoverStyle="true">
            <Selecting 
                AllowRowSelect="True"
                ></Selecting>
        </ClientSettings>                            
        <MasterTableView 
            AllowPaging="true" 
            AllowCustomPaging="true"
            AutoGenerateColumns="false" 
            HierarchyLoadMode="ServerOnDemand" 
			DataKeyNames="PackageId,UniqueId" 
			ClientDataKeyNames="PackageId,UniqueId" 
            >
            <Columns>
                <telerik:GridTemplateColumn
                    UniqueName="Name" 
                    HeaderText="Package Name"
                    HeaderStyle-Width="100%" 
                    SortExpression="Name"
                    >
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Image
                                runat="server"
                                ID="imagePackage"
                                style="float:left; margin: 5px;"
                                ImageUrl="/_layouts/Images/Vmgr/nav-packagemgr-enabled-32.png"
                                />
                            <span style="font-weight: bold; margin: 5px;"><%# Eval("Name") %> </span><span style=" margin: 5px;">{<%# Eval("UniqueId") %>}</span>    
                            <br />
                            <span style="font-style: italic; margin: 5px;"><%# Eval("Description") %>&nbsp;</span>
                        </div>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <NoRecordsTemplate>
                <div style="padding: 5px;">
                    No packages found.
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
</telerik:RadAjaxPanel>
