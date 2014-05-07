<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="DirectorySearchControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.DirectorySearchControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="vmgr" %>

<telerik:RadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">

        function getParentWindow() {
            var w = null;
            if (window.radWindow)
                w = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow)
                w = window.frameElement.radWindow;
            return w;
        }

        function OnGridCreatedDirectorySearch(s, e) {

            var parentWindow = getParentWindow();
            var height = parentWindow.BrowserWindow.directorySearchWindowHeight;
            var gridDataDiv = s.GridDataDiv;

            if (gridDataDiv != null && height != undefined)
                gridDataDiv.style.height = (height - 225) + "px";

            $("body").css("overflow", "hidden");
        }

        function OnRowSelectedDirectorySearch(s, e) {
            var buttonDirectorySearch = $find("<%= this.buttonDirectorySearch.ClientID %>");
            var row = s.get_selectedItems()[0];

            OnDisableButtonOk(true);
        }

        function OnOk(s, e) {
            var gridDirectorySearch = $find("<%= this.gridDirectorySearch.ClientID %>");
            var row = gridDirectorySearch.get_selectedItems()[0];

            if (row != null) {
                var parentWindow = getParentWindow();

                if (parentWindow != null) {
                    <%= this.parentCallbackJs %>
                }
            }
        }

        function OnDirectorySearch(s, e) {
            OnDisableButtonOk(false);
        }

        function OnClose(args) {
            if (args == true) {
                var parentWindow = getParentWindow();
                if (parentWindow != null) {
                    parentWindow.close(null);
                }
            }
        }

        function OnPopupResized() {
            var gridDirectorySearch = $find("<%= this.gridDirectorySearch.ClientID %>");

            if (gridDirectorySearch != null) {
                var gridDataDiv = gridDirectorySearch.GridDataDiv;
                var parentWindow = getParentWindow();
                var height = parentWindow.BrowserWindow.directorySearchWindowHeight;

                if (gridDataDiv != null && height != undefined)
                    gridDataDiv.style.height = (height - 225) + "px";
            }
        }

        addEvent(window, "resize", OnPopupResized);


    </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelDirectorySearch" 
    >
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager
    runat="Server"
    ID="ajaxManagerDirectorySearch"
    DefaultLoadingPanelID="ajaxLoadingPanelDirectorySearch"
    >
    <AjaxSettings>
        <teleriK:AjaxSetting 
            AjaxControlID="buttonDirectorySearch"
            >
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="gridDirectorySearch"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </teleriK:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<asp:Panel
    runat="server"
    ID="panelDirectorySearch"
    DefaultButton="buttonDirectorySearch"
    Width="100%"
    >            
    <div style="padding: 5px;">
        <table border="0" cellpadding="3px" cellspacing="0" style="table-layout: fixed;">
            <tr>
                <td style="width: 50px;">
                    <span style="font-size: 9pt;">Search: </span>
                </td>
                <td style="width: 200px;">
                    <telerik:RadTextBox
                        runat="server"
                        ID="textBoxDirectorySearch"
                        EmptyMessage="Enter a search term."
						Width="200px"
                        ></telerik:RadTextBox>
                    <asp:RequiredFieldValidator 
                        runat="server" 
                        ID="requiredFieldValidatorExternalEmail" 
                        ControlToValidate="textBoxDirectorySearch"
                        ValidationGroup="DirectorySearch" 
                        ErrorMessage="Please enter a search term."
                        Display="None" 
                        />
                </td>
                <td>
                    <telerik:RadButton 
                        runat="server" 
                        ID="buttonDirectorySearch"
                        Text="&nbsp;&nbsp; Search"
                        Width="100px"
                        CausesValidation="true"
                        ValidationGroup="DirectorySearch" 
                        ToolTip="Click to search."
                        OnClick="buttonDirectorySearch_Click"
                        >
                        <Icon 
                            PrimaryIconWidth="16" 
                            PrimaryIconHeight="16" 
                            PrimaryIconUrl="~/Images/search-icon-16.png" />
                    </telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:LinqDataSource 
        runat="server" 
        ID="linqDataSourceDirectorySearch" 
        OnSelecting="linqDataSourceDirectorySearch_Selecting" 
        />
    <telerik:RadGrid 
        runat="server" 
        ID="gridDirectorySearch" 
        DataSourceID="linqDataSourceDirectorySearch"
        AllowPaging="True" 
        AllowSorting="true"
        AllowFilteringByColumn="false"
        PageSize="15" 
        Border="0"
        Width="100%"
        AutoGenerateColumns="false"
        AllowMultiRowSelection="false"
        EnableLinqExpressions="false"
        >
        <ClientSettings>
            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
            <ClientEvents 
                OnRowSelected="OnRowSelectedDirectorySearch" 
                OnGridCreated="OnGridCreatedDirectorySearch" 
                />
        </ClientSettings>
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" BorderStyle="None" />
        <GroupingSettings CaseSensitive="false" />                                                     
        <MasterTableView 
            DataKeyNames="DisplayName,Eid,DomainAccount" 
            ClientDataKeyNames="DisplayName,Eid,DomainAccount" 
            TableLayout="Auto"
            >
            <NoRecordsTemplate>
                Type into the search box above then press "Enter" to start your search.
            </NoRecordsTemplate>
            <Columns>
                <telerik:GridBoundColumn 
                    DataField="DisplayName" 
                    HeaderText="Name"
                    ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="true" 
                    />
                <telerik:GridBoundColumn 
                    DataField="EID" 
                    HeaderText="Eid"
                    ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="true" 
                    />
                <telerik:GridBoundColumn 
                    HeaderText="Email"
                    DataField="Email" 
                    Visible="false"
                    />
            </Columns>
        </MasterTableView>
        <ClientSettings AllowDragToGroup="false" AllowColumnsReorder="false" />
    </telerik:RadGrid>
</asp:Panel>
