<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="FilterPackageControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.FilterPackageControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
    %>
<%@ Register 
    Tagprefix="SharePoint" 
    Namespace="Microsoft.SharePoint.WebControls" 
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" 
    %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.3.1015.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe"  Namespace="Vmgr.SharePoint" TagPrefix="vmgr" %>

<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">

        function OnFilterApplied(filter) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnApplyFilter(filter);
            }
        }

        function OnApplyFilter(s, e) {

            //  Update the preview string.
            var filter = $find('<%= filter.ClientID %>');
            filter.applyExpressions();

            //  Set the cookie.
            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('APPLY,');
        }

        function OnNewFilter() {
            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('NEW,');
        }

        function OnSaveFilterChanges() {
            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('SAVE,');
        }

        function OnDeleteComplete() {

            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnFilterDeleted();
            }

            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('REFRESH,' + 0);
        }

        function OnSaveComplete(filterId) {

            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnFilterSaved();
            }

            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('REFRESH,' + filterId);
        }

        function OnUndoFilterChanges() {
            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('UNDO,');
        }

        function OnEditName() {
            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('EDIT,');
        }

        function OnDeleteFilter(s, e) {

            var ajaxPanelFilter = $find('<%= ajaxPanelFilter.ClientID %>');
            ajaxPanelFilter.ajaxRequest('DELETE,' + e.get_commandArgument());
        }

        function MoveExpression() {

            $("#<%= filter.ClientID %>").children(".rfPreview")
                .appendTo("#expressionDiv");
        }

        function OnMoveExpression() { MoveExpression(); Sys.Application.remove_load(OnMoveExpression); }

        Sys.Application.add_load(OnMoveExpression);


        function LimitFilters() {

            var filter = $find("<%= filter.ClientID %>");
            var menu = filter.get_contextMenu();
            menu.add_showing(OnFilterMenuShowing);
        }

        function pageLoad(s, e) {
            LimitFilters();
        }

        function OnFilterMenuShowing(sender, args) {
            var filter = $find("<%=filter.ClientID %>");
            var currentExpandedItem = sender.get_attributes()._data.ItemHierarchyIndex;
            var fieldName = filter._expressionItems[currentExpandedItem];
            var allFields = filter._dataFields;
            var dataType = null;
            for (var i = 0, j = allFields.length; i < j; i++) {
                if (allFields[i].FieldName == fieldName) {
                    dataType = allFields[i].DataType;
                    break;
                }
            }
            if (fieldName == "UniqueId") {
                sender.findItemByValue("IsNull").set_visible(false);
                sender.findItemByValue("NotIsNull").set_visible(false);
                sender.findItemByValue("Contains").set_visible(false);
                sender.findItemByValue("DoesNotContain").set_visible(false);
                sender.findItemByValue("StartsWith").set_visible(false);
                sender.findItemByValue("EndsWith").set_visible(false);
                sender.findItemByValue("GreaterThan").set_visible(false);
                sender.findItemByValue("LessThan").set_visible(false);
                sender.findItemByValue("GreaterThanOrEqualTo").set_visible(false);
                sender.findItemByValue("LessThanOrEqualTo").set_visible(false);
                sender.findItemByValue("Between").set_visible(false);
                sender.findItemByValue("NotBetween").set_visible(false);
                sender.findItemByValue("IsEmpty").set_visible(false);
                sender.findItemByValue("NotIsEmpty").set_visible(false);
            }
        }

    </script>
</telerik:RadCodeBlock>
<style type="text/css">
    .KillButton
    {
        background-image: none !important;
        background-color: transparent !important;
        border: none !important;
    }

    .KillButton .rbPrimaryIcon {
        width: 24px;
        height: 24px;
    }

    .KillButton .rbText {
        margin-top: 5px;
        margin-left: 5px;
        color: #666;
        font-weight: bold;
    }

    .EditButton,
    .EditButton .rbPrimaryIcon
    {
        height: 16px !important;
    }

</style>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelPackageManager" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel
    runat="server"
    ID="ajaxPanelFilter"
    Width="100%"
    LoadingPanelID="ajaxLoadingPanelFilter"
    OnAjaxRequest="ajaxPanelFilter_AjaxRequest"
    >
    <div style="padding: 10px;">
        <table style="border-collapse: collapse; font-size: 8pt; width: 100%; padding: 0px; table-layout: auto;">
            <tr>
                <td colspan="3" style="vertical-align: top; padding: 5px;">
                    <style type="text/css">
                        .rfPreview {
                            padding-top: 0px !important;
                        }
                        .rfPreview {
                            min-height: 18px;
                        }
                    </style>
                    <fieldset  style="font-size: 9pt; padding: 10px;">
                        <legend style="margin-bottom: 0px;">Expression:</legend> 
                        <div id="expressionDiv" class="RadFilter RadFilter_Default rfPreview">
                        </div>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; padding: 5px; width: 50%;">
                    <fieldset  style="font-size: 9pt; padding: 10px;">
                        <legend style="margin-bottom: 0px;">Expression Builder: 
                            <asp:Label 
                                runat="server" 
                                ID="labelSelectedFilter"
                                Font-Bold="true"
                                Visible="false"
                                >
                            </asp:Label>
                            <telerik:RadTextBox
                                runat="server"
                                ID="textBoxName"
                                EmptyMessage="Filter name here..."
                                Visible="false"
                                >
                            </telerik:RadTextBox>
                            <telerik:RadButton 
                                ID="buttonEditName"
                                runat="server"
                                ButtonType="LinkButton"
                                ToolTip="Edit name"
                                CssClass="KillButton EditButton"
                                AutoPostBack="false"
                                CausesValidation="false"
                                OnClientClicked="OnEditName"
                                >
                                <Icon PrimaryIconUrl="/_layouts/images/Vmgr/edit-icon-16.png" />
                            </telerik:RadButton>
                        </legend>
                        <asp:LinqDataSource
                            runat="server"
                            ID="linqDataSourceCustomFilter"
                            OnSelecting="linqDataSourceFilter_Selecting"
                            ></asp:LinqDataSource>
                        <telerik:RadFilter 
                            runat="server" 
                            ID="filter"
                            ExpressionPreviewPosition="Bottom"
                            DefaultGroupOperation="And"
                            ShowApplyButton="false"
                            OnApplyExpressions="filter_ApplyExpressions"
                            >
                            <ClientSettings>
                                <ClientEvents OnFilterCreated="MoveExpression" />
                            </ClientSettings>
                        </telerik:RadFilter>
                        <br />
                        <asp:RequiredFieldValidator
                            ID="requiredFieldValidatorName"
                            Runat="server"
                            Display="Dynamic"
                            ControlToValidate="textBoxName"
                            ErrorMessage="Filter name is a required field." >
                        </asp:RequiredFieldValidator>			                
                        <table style="width: 100%; border-collapse: collapse; padding: 0px;">
                            <tr>
                                <td style="width: 33%; text-align: center;">
			                        <telerik:RadButton 
                                        ID="buttonNewFilter"
                                        runat="server"
                                        ButtonType="LinkButton"
                                        Text="New filter"
                                        ToolTip="New filter"
                                        CssClass="KillButton"
                                        AutoPostBack="false"
                                        CausesValidation="false"
                                        OnClientClicked="OnNewFilter"
                                        >
                                        <Icon PrimaryIconUrl="/_layouts/images/Vmgr/new-filter-icon-24.png" />
                                    </telerik:RadButton>
                                </td>
                                <td style="width: 33%; text-align: center;">
			                        <telerik:RadButton 
                                        ID="buttonUndoChanges"
                                        runat="server"
                                        ButtonType="LinkButton"
                                        Text="Undo changes"
                                        ToolTip="Undo changes"
                                        CssClass="KillButton"
                                        AutoPostBack="false"
                                        CausesValidation="false"
                                        OnClientClicked="OnUndoFilterChanges"
                                        >
                                        <Icon PrimaryIconUrl="/_layouts/images/Vmgr/undo-icon-24.png" />
                                    </telerik:RadButton>
                                </td>
                                <td style="width: 33%; text-align: center;">
			                        <telerik:RadButton 
                                        ID="buttonSaveFilter"
                                        runat="server"
                                        ButtonType="LinkButton"
                                        Text="Save changes"
                                        ToolTip="Save changes"
                                        CssClass="KillButton"
                                        AutoPostBack="false"
                                        CausesValidation="true"
                                        OnClientClicked="OnSaveFilterChanges"
                                        >
                                        <Icon PrimaryIconUrl="/_layouts/images/Vmgr/hardware-floppy-icon-24.png" />
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="vertical-align: middle; width: 5px;">
                    &nbsp;
                </td>
                <td style="vertical-align: top; padding: 5px;">
                    <fieldset style="padding: 10px;">
                        <legend style="font-size: 9pt;">My Package Filters</legend>
                        <asp:LinqDataSource
                            runat="server"
                            ID="linqDataSourceFilter"
                            OnSelecting="linqDataSourceFilter_Selecting"
                            ></asp:LinqDataSource>
                        <telerik:RadTreeView 
                            runat="server" 
                            ID="treeViewOptions" 
                            DataSourceID="linqDataSourceFilter"
                            DataValueField="FilterId"
                            DataTextField="Name"
                            Width="100%" 
                            CausesValidation="false"
                            style="overflow-x: hidden;"
                            OnNodeDataBound="treeViewOptions_NodeDataBound"
                            OnNodeClick="treeViewOptions_NodeClick"
                            >
                            <NodeTemplate>
                                <table style="width: 90%; border-collapse: collapse; padding: 0px;">
                                    <tr>
                                        <td>
                                            <%# Eval("Name") %>
                                        </td>
                                        <td style="text-align: right;">
                                            <telerik:RadButton 
                                                runat="server" 
                                                ID="buttonDeleteFilter"
                                                AutoPostBack="false"
                                                ToolTip="Delete filter"
                                                Visible="true"
                                                Width="16px"
                                                Height="16px"
                                                CausesValidation="false"
                                                OnClientClicked="OnDeleteFilter"
                                                CommandArgument='<%# Eval("FilterId") %>'
                                                >
                                                <Image 
                                                    ImageUrl="/_layouts/Images/Vmgr/trash-can-icon-16.png" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </NodeTemplate>
                        </telerik:RadTreeView>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 5px;">
                    <span style="font-style: italic; color: #666;">Note: Saving filters is recommended, but it is not required.  Just click apply to have your custom filter take immediate affect.</span>
                </td>
            </tr>
        </table>
    </div>
</telerik:RadAjaxPanel>