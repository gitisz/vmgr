<%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ScheduleEditorControl.ascx.cs" 
    Inherits="Vmgr.SharePoint.ScheduleEditorControl, Vmgr.SharePoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d83f0fbbbc9139fe" 
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
        var selectedPluginUniqueId = <%= selectedPluginUniqueId %>;

        function OnPluginRowSelected(s, e) {
            var row = s.get_selectedItems()[0];
            selectedPluginUniqueId = row.getDataKeyValue('UniqueId');

            var textBoxName = $find('<%= textBoxName.ClientID %>');
            textBoxName.set_value(row.getDataKeyValue('Name') + ' Schedule');
        }

        function OnSave(s, e) {
            var ajaxPanelSchedule = $find('<%= ajaxPanelSchedule.ClientID %>');
            ajaxPanelSchedule.ajaxRequest('SAVE_SCHEDULE,' + selectedPluginUniqueId);
        }

        function OnCloseSchedulerEditor(s, e) {
            var parentWindow = getParentWindow();
            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshSchedules();
            }
            this.OnClose(s, e);
        }

        function OnValidateSelectedPlugin(s, e) {

            if (selectedPluginUniqueId == null)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        function OnValidateEnd(s, e) {
            
            var dateTimePickerStart = $find('<%= dateTimePickerStart.ClientID %>');
            var dateTimePickerEnd = $find('<%= dateTimePickerEnd.ClientID %>');

            if (dateTimePickerEnd.get_selectedDate() != null){
                if(dateTimePickerEnd.get_selectedDate() <= dateTimePickerStart.get_selectedDate()){
                    e.IsValid = false;
                }
            }
            else
                e.IsValid = true;
        }

        function OnValidateMinutely(s, e) {
            var numericTextBoxMinutely = $find('<%= numericTextBoxMinutely.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');

            if(tabStripRecurrence.get_selectedTab().get_value() == 0)
            {
                if(numericTextBoxMinutely.get_value() < 1) {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }

        function OnValidateHourlyInterval(s, e) {
            var numericTextBoxHourly = $find('<%= numericTextBoxHourly.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var buttonToggleHourly1 = $find('<%= buttonToggleHourly1.ClientID %>');
            var timePickerHourlyStartTime = $find('<%= timePickerHourlyStartTime.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 1)
            {
                if(buttonToggleHourly1.get_checked())
                {
                    if(numericTextBoxHourly.get_value() < 1) {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }

        function OnValidateHourlyTime(s, e) {
            var numericTextBoxHourly = $find('<%= numericTextBoxHourly.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var buttonToggleHourly2 = $find('<%= buttonToggleHourly2.ClientID %>');
            var timePickerHourlyStartTime = $find('<%= timePickerHourlyStartTime.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 1)
            {
                if(buttonToggleHourly2.get_checked())
                {
                    if(timePickerHourlyStartTime.get_selectedDate() == null) {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }

        function OnValidateDailyDays(s, e) {
            var numericTextBoxDaily = $find('<%= numericTextBoxDaily.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var buttonToggleDaily1 = $find('<%= buttonToggleDaily1.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 2)
            {
                if(buttonToggleDaily1.get_checked())
                {
                    if(numericTextBoxDaily.get_value() < 1) {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }
        
        function OnValidateDailyTime(s, e) {
            var timePickerDailyStartTime = $find('<%= timePickerDailyStartTime.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 2)
            {
                if(timePickerDailyStartTime.get_selectedDate() == null) {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }

        function OnValidateWeeklyDays(s, e) {
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var buttonWeeklyMonday = $find('<%= buttonWeeklyMonday.ClientID %>');
            var buttonWeeklyTuesday = $find('<%= buttonWeeklyTuesday.ClientID %>');
            var buttonWeeklyWednesday = $find('<%= buttonWeeklyWednesday.ClientID %>');
            var buttonWeeklyThursday = $find('<%= buttonWeeklyThursday.ClientID %>');
            var buttonWeeklyFriday = $find('<%= buttonWeeklyFriday.ClientID %>');
            var buttonWeeklySaturday = $find('<%= buttonWeeklySaturday.ClientID %>');
            var buttonWeeklySunday = $find('<%= buttonWeeklySunday.ClientID %>');
            
            var count = 0;

            if(buttonWeeklyMonday.get_checked())
                count ++;
            if(buttonWeeklyTuesday.get_checked())
                count ++;
            if(buttonWeeklyWednesday.get_checked())
                count ++;
            if(buttonWeeklyThursday.get_checked())
                count ++;
            if(buttonWeeklyFriday.get_checked())
                count ++;
            if(buttonWeeklySaturday.get_checked())
                count ++;
            if(buttonWeeklySunday.get_checked())
                count ++;

            if(tabStripRecurrence.get_selectedTab().get_value() == 3)
            {
                if(count == 0)
                {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }
        
        function OnValidateWeeklyTime(s, e) {
            var timePickerWeeklyStartTime = $find('<%= timePickerWeeklyStartTime.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 3)
            {
                if(timePickerWeeklyStartTime.get_selectedDate() == null) {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }
        
        function OnValidateMonthlyDay(s, e) {
            var buttonToggleMonthly1 = $find('<%= buttonToggleMonthly1.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var numericTextBoxMonthlyDay = $find('<%= numericTextBoxMonthlyDay.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 4)
            {
                if(buttonToggleMonthly1.get_checked()) {
                    if(numericTextBoxMonthlyDay.get_value() < 1)
                    {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }
        
        function OnValidateMonthlyMonths(s, e) {
            var buttonToggleMonthly1 = $find('<%= buttonToggleMonthly1.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var numericTextBoxMonthlyMonths = $find('<%= numericTextBoxMonthlyMonths.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 4)
            {
                if(buttonToggleMonthly1.get_checked()) {
                    if(numericTextBoxMonthlyMonths.get_value() < 1)
                    {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }
        
        function OnValidateMonthlyOccurrenceMonth(s, e) {
            var buttonToggleMonthly2 = $find('<%= buttonToggleMonthly2.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var numericTextBoxOccurrenceMonth = $find('<%= numericTextBoxOccurrenceMonth.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 4)
            {
                if(buttonToggleMonthly2.get_checked()) {
                    if(numericTextBoxOccurrenceMonth.get_value() < 1)
                    {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;
        }
        
        
        function OnValidateMonthlyTime(s, e) {
            var timePickerMonthlyStartTime = $find('<%= timePickerMonthlyStartTime.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 4)
            {
                if(timePickerMonthlyStartTime.get_selectedDate() == null) {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }
    
        function OnValidateYearlyOccurrenceDay(s, e) {
            var buttonToggleYearly1 = $find('<%= buttonToggleYearly1.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            var numericTextBoxYearlyOccurrenceDay = $find('<%= numericTextBoxYearlyOccurrenceDay.ClientID %>');
           
            if(tabStripRecurrence.get_selectedTab().get_value() == 5)
            {
                if(buttonToggleYearly1.get_checked()) {
                    if(numericTextBoxYearlyOccurrenceDay.get_value() < 1)
                    {
                        e.IsValid = false;
                    }
                }
            }else
                e.IsValid = true;

        }     
   
        function OnValidateYearlyTime(s, e) {
            var timePickerYearlyStartTime = $find('<%= timePickerYearlyStartTime.ClientID %>');
            var tabStripRecurrence = $find('<%= tabStripRecurrence.ClientID %>');
            
            if(tabStripRecurrence.get_selectedTab().get_value() == 4)
            {
                if(timePickerYearlyStartTime.get_selectedDate() == null) {
                    e.IsValid = false;
                }
            }else
                e.IsValid = true;
        }

        function OptionsOnLoad(s, e) {
            var nodes = s.get_allNodes();
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_nodes() != null) {
                    nodes[i].expand();
                }
            }
        }
        
        function OnOptionSelected(s, e) {

            var itemValue = s.get_selectedNode().get_value();
            var ajaxPanelExclusions = $find('<%= ajaxPanelExclusions.ClientID %>');

            if (itemValue == 'UNDO_EXCLUSIONS') {
                ajaxPanelExclusions.ajaxRequest('UNDO_EXCLUSIONS,');
            }

            if (itemValue == 'REMOVE_EXCLUSIONS') {
                ajaxPanelExclusions.ajaxRequest('REMOVE_EXCLUSIONS,');
            }
        }

        function OnOpenOptions(s, e) {
            var ajaxPanelExclusions = $find('<%= ajaxPanelExclusions.ClientID %>');
            ajaxPanelExclusions.ajaxRequest('OPEN_OPTIONS,');
        }


        function OnCloseOptions() {
            var ajaxPanelExclusions = $find('<%= ajaxPanelExclusions.ClientID %>');
            ajaxPanelExclusions.ajaxRequest('CLOSE_OPTIONS,');
        }

    </script>
    <style type="text/css">

        .optionsDivRt {
            right: 5px;
        }

        .CloseButton
        {
            background-image: none !important;
            background-color: transparent !important;
            border: none !important;
        }

        .CloseButton .rbPrimaryIcon {
            width: 24px;
            height: 24px;
            top: 6px;
            left: 10px;
        }

        .CloseButton .rbText {
            margin-top: 0px;
            margin-left: -3px;
            color: #666;
            font-weight: bold;
        }
    </style>
    <!--[if IE]>
        <style type="text/css">
            .optionsDivRt {
                right: 25px;
            }
        </style>
    <![endif]-->
</telerik:RadCodeBlock>
<telerik:RadAjaxLoadingPanel 
    runat="server"
    ID="ajaxLoadingPanelSchedule" 
    Skin="Default" 
    ZIndex="2999"
    ></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel 
    runat="server"
    ID="ajaxPanelSchedule" 
    LoadingPanelID="ajaxLoadingPanelSchedule"
    OnAjaxRequest="ajaxPanelSchedule_AjaxRequest"
    >
    <div style="padding: 10px;">
        <h3>Select a plugin: <asp:Label
            runat="server"
            ID="labelPackage"
            ></asp:Label>
            </h3>
        Please select a plugin for this schedule.
        <asp:ValidationSummary
            runat="server"
            ID="validationSummarySchedule"
            DisplayMode="BulletList"
            style="margin: 5px;"
            />
    </div>
    <asp:LinqDataSource
        runat="server"
        ID="linqDataSourcePlugin"
        OnSelecting="linqDataSourcePlugin_Selecting"
        ></asp:LinqDataSource>
    <telerik:RadGrid 
        runat="server" 
        ID="gridPlugin" 
        DataSourceID="linqDataSourcePlugin"
        GridLines="None" 
        BorderStyle="None"
        AutoGenerateColumns="false"
        Width="100%" 
        AllowSorting="True" 
        AllowPaging="True" 
        AllowCustomPaging="true"
        PageSize="5" 
        OnItemDataBound="gridPlugin_ItemDataBound" 
        >
        <ClientSettings 
            EnableRowHoverStyle="true">
            <Selecting 
                AllowRowSelect="True"
                ></Selecting>
            <ClientEvents
                OnRowSelected="OnPluginRowSelected"
                />
        </ClientSettings>                            
        <MasterTableView 
            AllowPaging="true" 
            AllowCustomPaging="true"
            AutoGenerateColumns="false" 
            HierarchyLoadMode="ServerOnDemand" 
			DataKeyNames="PluginId,UniqueId" 
			ClientDataKeyNames="PluginId,UniqueId,Name" 
            >
            <Columns>
                <telerik:GridBoundColumn
                    UniqueName="Name" 
                    DataField="Name" 
                    HeaderText="Plugin Name"
                    HeaderStyle-Width="100%" 
                    SortExpression="Name"
                    >
                </telerik:GridBoundColumn>
            </Columns>
            <NoRecordsTemplate>
                <div style="padding: 5px;">
                    No plugins found.
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
    <asp:CustomValidator
        runat="server"
        ID="customValidatorSelectedPlugin"
        Display="None"
        Text=""
        ErrorMessage="A plugin selection is a required."
        ClientValidationFunction="OnValidateSelectedPlugin"
        ></asp:CustomValidator>
    <div style="padding: 10px;">
        <div style="border-bottom: solid 1px gray; position: relative; height: 30px;">
            <div style="position: absolute; top: 5px;">
                <telerik:RadTabStrip 
                    runat="server" 
                    ID="tabStripSchedule" 
                    MultiPageID="multiPageSchedule" 
                    SelectedIndex="0"
                    CausesValidation="false"
                    Orientation="HorizontalTop"
                    >
                    <Tabs>
                        <telerik:RadTab 
                            runat="server" 
                            Text="Details" 
                            PageViewID="pageViewDetails"
                            >
                        </telerik:RadTab>
                        <telerik:RadTab 
                            runat="server" 
                            Text="Recurrence" 
                            PageViewID="pageViewRecurrence"
                            >
                        </telerik:RadTab>
                        <telerik:RadTab 
                            runat="server" 
                            Text="Exclusions" 
                            PageViewID="pageViewExclusions"
                            >
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
        </div>
        <div style="border: solid 1px gray; border-top: none; background: white; font-size: 8pt;">
            <div style="padding: 10px;">
                <telerik:RadMultiPage 
                    runat="server" 
                    ID="multiPageSchedule" 
                    SelectedIndex="0" 
                    RenderSelectedPageOnly="false"
                    >
                    <telerik:RadPageView runat="server" ID="pageViewDetails">
                        <h3>Schedule Name: <asp:Label
                            runat="server"
                            ID="label2"
                            ></asp:Label>
                            </h3>
                        Please specify schedule name, description, start date, and if desired, end date.
                        <br />
                        <br />
                        <table border="0" cellpadding="3" cellspacing="0" style="">
                            <tr>
                                <td style="vertical-align: top;">
                                    Name <span style="color: Red;">*</span>
                                </td>
                                <td style="vertical-align: top;">
                                    <telerik:RadTextBox
                                        runat="server"
                                        ID="textBoxName"
                                        Width="600px"
                                        ></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ID="requiredFieldValidatorName"
                                        Display="None"
                                        Text=""
                                        ErrorMessage="Name is a required field."
                                        ControlToValidate="textBoxName"
                                        ></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    Description 
                                </td>
                                <td style="vertical-align: top;">
                                    <telerik:RadTextBox
                                        runat="server"
                                        ID="textBoxDescription"
                                        TextMode="MultiLine"
                                        Width="600px"
                                        ></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    Start date & time <span style="color: Red;">*</span>
                                </td>
                                <td style="vertical-align: top;">
                                    <telerik:RadDateTimePicker
                                        runat="server"
                                        ID="dateTimePickerStart"
                                        Width="180px"
                                        ></telerik:RadDateTimePicker>
                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ID="requiredFieldValidatorStart"
                                        Display="None"
                                        Text=""
                                        ErrorMessage="Start date &amp; time is a required field."
                                        ControlToValidate="dateTimePickerStart"
                                        ></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    End date & time 
                                </td>
                                <td style="vertical-align: top;">
                                    <telerik:RadDateTimePicker
                                        runat="server"
                                        ID="dateTimePickerEnd"
                                        Width="180px"
                                        ></telerik:RadDateTimePicker>
                                    <asp:CustomValidator
                                        runat="server"
                                        ID="customValidatorEnd"
                                        Display="None"
                                        Text=""
                                        ErrorMessage="End date &amp; time must be greater than the start date &amp; time."
                                        ClientValidationFunction="OnValidateEnd"
                                        ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="pageViewRecurrence">
                        <h3>Define Recurrence: <asp:Label
                            runat="server"
                            ID="label1"
                            ></asp:Label>
                            </h3>
                        Please specify the recurrence schedule.
                        <br />
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td style="width: 85px; vertical-align: top;">
                                    <telerik:RadTabStrip 
                                        runat="server" 
                                        ID="tabStripRecurrence" 
                                        MultiPageID="multiPageRecurrence" 
                                        SelectedIndex="0"
                                        CausesValidation="false"
                                        Orientation="VerticalLeft"
                                        >
                                        <Tabs>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Minutely" 
                                                Value="0"
                                                PageViewID="pageViewMinutely"
                                                >
                                            </telerik:RadTab>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Hourly" 
                                                Value="1"
                                                PageViewID="pageViewHourly"
                                                >
                                            </telerik:RadTab>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Daily" 
                                                Value="2"
                                                PageViewID="pageViewDaily"
                                                >
                                            </telerik:RadTab>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Weekly" 
                                                Value="3"
                                                PageViewID="pageViewWeekly"
                                                >
                                            </telerik:RadTab>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Monthly" 
                                                Value="4"
                                                PageViewID="pageViewMonthly"
                                                >
                                            </telerik:RadTab>
                                            <telerik:RadTab 
                                                runat="server" 
                                                Text="Yearly" 
                                                Value="5"
                                                PageViewID="pageViewYearly"
                                                >
                                            </telerik:RadTab>
                                        </Tabs>
                                    </telerik:RadTabStrip>
                                </td>
                                <td style="vertical-align: top; border: solid 1px gray;">
                                    <div style="padding: 5px;">
                                        <telerik:RadMultiPage 
                                            runat="server" 
                                            ID="multiPageRecurrence" 
                                            SelectedIndex="0" 
                                            RenderSelectedPageOnly="false"
                                            >
                                            <telerik:RadPageView runat="server" ID="pageViewMinutely">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorMinutely"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="The number of minutes is required."
                                                    ClientValidationFunction="OnValidateMinutely"
                                                    ></asp:CustomValidator>
                                                Every 
                                                <telerik:RadNumericTextBox
                                                    runat="server"
                                                    ID="numericTextBoxMinutely"
                                                    MinValue="1"
                                                    AutoPostBack="false"
                                                    NumberFormat-DecimalDigits="0"
                                                    Width="55px"
                                                    ></telerik:RadNumericTextBox>
                                                minute(s)
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="pageViewHourly">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorHourlyInterval"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="The number of hours is required."
                                                    ClientValidationFunction="OnValidateHourlyInterval"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorHourlyTime"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="An hourly time selection is required."
                                                    ClientValidationFunction="OnValidateHourlyTime"
                                                    ></asp:CustomValidator>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleHourly1" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Hourly"
                                                                Checked="true"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            Every 
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxHourly"
                                                                MinValue="1"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                ></telerik:RadNumericTextBox>
                                                            hour(s)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleHourly2" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Hourly"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            At <telerik:RadTimePicker
                                                                runat="server"
                                                                ID="timePickerHourlyStartTime"
                                                                Width="100px"
                                                                >
                                                                <DateInput
                                                                    DateFormat="HH:mm"
                                                                    ></DateInput>
                                                                <TimeView
                                                                    TimeFormat="HH:mm"
                                                                ></TimeView>
                                                                </telerik:RadTimePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="pageViewDaily">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorDailyDays"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A number of days is required."
                                                    ClientValidationFunction="OnValidateDailyDays"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorDailyTime"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A daily time selection is required."
                                                    ClientValidationFunction="OnValidateDailyTime"
                                                    ></asp:CustomValidator>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleDaily1" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Daily"
                                                                Checked="true"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            Every 
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxDaily"
                                                                MinValue="1"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                ></telerik:RadNumericTextBox>
                                                            day(s)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleDaily2" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Daily"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            Every week day
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Start time 
                                                            <telerik:RadTimePicker
                                                                runat="server"
                                                                ID="timePickerDailyStartTime"
                                                                Width="100px"
                                                                SelectedDate="12:00"
                                                                >
                                                                <DateInput
                                                                    DateFormat="HH:mm"
                                                                    ></DateInput>
                                                                <TimeView
                                                                    TimeFormat="HH:mm"
                                                                ></TimeView>
                                                            </telerik:RadTimePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="pageViewWeekly">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorWeeklyDays"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="At least one week day selection is required."
                                                    ClientValidationFunction="OnValidateWeeklyDays"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorWeeklyTime"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A weekly time selection is required."
                                                    ClientValidationFunction="OnValidateWeeklyTime"
                                                    ></asp:CustomValidator>
                                                <table border="0" cellpadding="3px" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklyMonday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Monday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Monday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklyTuesday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Tuesday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Tuesday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklyWednesday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Wednesday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Wednesday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklyThursday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Thursday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Thursday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklyFriday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Friday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Friday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklySaturday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Saturday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Saturday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton 
                                                                ID="buttonWeeklySunday" 
                                                                runat="server" 
                                                                ToggleType="CheckBox" 
                                                                ButtonType="ToggleButton"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Sunday" 
                                                                        PrimaryIconCssClass="rbToggleCheckboxChecked"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                    <telerik:RadButtonToggleState 
                                                                        Text="Sunday" 
                                                                        PrimaryIconCssClass="rbToggleCheckbox"
                                                                        >
                                                                    </telerik:RadButtonToggleState>
                                                                </ToggleStates>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            Start time 
                                                            <telerik:RadTimePicker
                                                                runat="server"
                                                                ID="timePickerWeeklyStartTime"
                                                                Width="100px"
                                                                SelectedDate="12:00"
                                                                >
                                                                <DateInput
                                                                    DateFormat="HH:mm"
                                                                    ></DateInput>
                                                                <TimeView
                                                                    TimeFormat="HH:mm"
                                                                ></TimeView>
                                                            </telerik:RadTimePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="pageViewMonthly">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorMonthlyDay"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A day of the month is required."
                                                    ClientValidationFunction="OnValidateMonthlyDay"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorMonthlyMonths"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A number of months is required."
                                                    ClientValidationFunction="OnValidateMonthlyMonths"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorMonthlyOccurrenceMonth"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A number of months is required."
                                                    ClientValidationFunction="OnValidateMonthlyOccurrenceMonth"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorMonthlyTime"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A monthly time selection is required."
                                                    ClientValidationFunction="OnValidateMonthlyTime"
                                                    ></asp:CustomValidator>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleMonthly1" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Monthly"
                                                                Checked="true"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            Day 
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxMonthlyDay"
                                                                MinValue="1"
                                                                MaxValue="31"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                ></telerik:RadNumericTextBox>
                                                            of every
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxMonthlyMonths"
                                                                MinValue="1"
                                                                MaxValue="12"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                ></telerik:RadNumericTextBox>
                                                            month(s)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleMonthly2" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Monthly"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            The
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxMonthlyOccurrenceIndex"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="First" Value="1" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="Second" Value="2" />    
                                                                    <telerik:RadComboBoxItem Text="Third" Value="3" />    
                                                                    <telerik:RadComboBoxItem Text="Fourth" Value="4" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxMonthlyOccurrenceWeekDay"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Monday" Value="MON" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="Tuesday" Value="TUE" />    
                                                                    <telerik:RadComboBoxItem Text="Wednesday" Value="WED" />    
                                                                    <telerik:RadComboBoxItem Text="Thursday" Value="THU" />    
                                                                    <telerik:RadComboBoxItem Text="Friday" Value="FRI" />    
                                                                    <telerik:RadComboBoxItem Text="Saturday" Value="SAT" />    
                                                                    <telerik:RadComboBoxItem Text="Sunday" Value="SUN" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            of every
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxOccurrenceMonth"
                                                                MinValue="1"
                                                                MaxValue="12"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                Value="1"
                                                                ></telerik:RadNumericTextBox>
                                                            month(s)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Start time 
                                                            <telerik:RadTimePicker
                                                                runat="server"
                                                                ID="timePickerMonthlyStartTime"
                                                                Width="100px"
                                                                SelectedDate="12:00"
                                                                >
                                                                <DateInput
                                                                    DateFormat="HH:mm"
                                                                    ></DateInput>
                                                                <TimeView
                                                                    TimeFormat="HH:mm"
                                                                ></TimeView>
                                                            </telerik:RadTimePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="pageViewYearly">
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorYearlyOccurrenceDay"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A day of the month is required."
                                                    ClientValidationFunction="OnValidateYearlyOccurrenceDay"
                                                    ></asp:CustomValidator>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="customValidatorYearly"
                                                    Display="None"
                                                    Text=""
                                                    ErrorMessage="A yearly time selection is required."
                                                    ClientValidationFunction="OnValidateYearlyTime"
                                                    ></asp:CustomValidator>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleYearly1" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Yearly"
                                                                Checked="true"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            Every 
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxYearlyOccurrenceMonth"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="January" Value="1" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="February" Value="2" />    
                                                                    <telerik:RadComboBoxItem Text="March" Value="3" />    
                                                                    <telerik:RadComboBoxItem Text="April" Value="4" />    
                                                                    <telerik:RadComboBoxItem Text="May" Value="5" />    
                                                                    <telerik:RadComboBoxItem Text="June" Value="6" />    
                                                                    <telerik:RadComboBoxItem Text="July" Value="7" />    
                                                                    <telerik:RadComboBoxItem Text="August" Value="8" />    
                                                                    <telerik:RadComboBoxItem Text="September" Value="9" />    
                                                                    <telerik:RadComboBoxItem Text="October" Value="10" />    
                                                                    <telerik:RadComboBoxItem Text="November" Value="11" />    
                                                                    <telerik:RadComboBoxItem Text="December" Value="12" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            <telerik:RadNumericTextBox
                                                                runat="server"
                                                                ID="numericTextBoxYearlyOccurrenceDay"
                                                                MinValue="1"
                                                                MaxValue="31"
                                                                AutoPostBack="false"
                                                                NumberFormat-DecimalDigits="0"
                                                                Width="55px"
                                                                ></telerik:RadNumericTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom: 5px;">
                                                            <telerik:RadButton 
                                                                ID="buttonToggleYearly2" 
                                                                runat="server" 
                                                                ToggleType="Radio" 
                                                                ButtonType="ToggleButton" 
                                                                GroupName="Yearly"
                                                                AutoPostBack="false"
                                                                CausesValidation="false"
                                                                >
                                                                <ToggleStates>
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadioChecked" 
                                                                        />
                                                                    <telerik:RadButtonToggleState 
                                                                        PrimaryIconCssClass="rbToggleRadio" 
                                                                        />
                                                                </ToggleStates>
                                                            </telerik:RadButton>                                        
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            The
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxYearlyOccurrenceIndex"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="First" Value="1" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="Second" Value="2" />    
                                                                    <telerik:RadComboBoxItem Text="Third" Value="3" />    
                                                                    <telerik:RadComboBoxItem Text="Fourth" Value="4" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxYearlyOccurrenceWeekDay"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Monday" Value="MON" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="Tuesday" Value="TUE" />    
                                                                    <telerik:RadComboBoxItem Text="Wednesday" Value="WED" />    
                                                                    <telerik:RadComboBoxItem Text="Thursday" Value="THU" />    
                                                                    <telerik:RadComboBoxItem Text="Friday" Value="FRI" />    
                                                                    <telerik:RadComboBoxItem Text="Saturday" Value="SAT" />    
                                                                    <telerik:RadComboBoxItem Text="Sunday" Value="SUN" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                            of 
                                                            <telerik:RadComboBox
                                                                runat="server"
                                                                ID="comboBoxYearlyOccurrenceWeekMonth"
                                                                Width="100px"
                                                                >
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="January" Value="1" Checked="true" />    
                                                                    <telerik:RadComboBoxItem Text="February" Value="2" />    
                                                                    <telerik:RadComboBoxItem Text="March" Value="3" />    
                                                                    <telerik:RadComboBoxItem Text="April" Value="4" />    
                                                                    <telerik:RadComboBoxItem Text="May" Value="5" />    
                                                                    <telerik:RadComboBoxItem Text="June" Value="6" />    
                                                                    <telerik:RadComboBoxItem Text="July" Value="7" />    
                                                                    <telerik:RadComboBoxItem Text="August" Value="8" />    
                                                                    <telerik:RadComboBoxItem Text="September" Value="9" />    
                                                                    <telerik:RadComboBoxItem Text="October" Value="10" />    
                                                                    <telerik:RadComboBoxItem Text="November" Value="11" />    
                                                                    <telerik:RadComboBoxItem Text="December" Value="12" />    
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Start time 
                                                            <telerik:RadTimePicker
                                                                runat="server"
                                                                ID="timePickerYearlyStartTime"
                                                                Width="100px"
                                                                SelectedDate="12:00"
                                                                >
                                                                <DateInput
                                                                    DateFormat="HH:mm"
                                                                    ></DateInput>
                                                                <TimeView
                                                                    TimeFormat="HH:mm"
                                                                ></TimeView>
                                                            </telerik:RadTimePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="pageViewExclusions">
                        <telerik:RadAjaxPanel 
                            runat="server"
                            ID="ajaxPanelExclusions" 
                            LoadingPanelID="ajaxLoadingPanelSchedule"
                            OnAjaxRequest="ajaxPanelExclusions_AjaxRequest"
                            >                                
                            <div style="padding: 0px; padding-top: 0px; z-index:10; position:relative;">
                                <div style="border: none; background: white; font-size: 8pt;">
                                    <table style="width: 100%; padding: 0px; border-collapse: collapse;">
                                        <tr>
                                            <td>
                                                <h3>Exclusions:</h3>
                                                Select any dates to be excluded from this schedule.
                                            </td>
                                            <td style="text-align: right; width: 150px;">
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
                                                </style>			                        
                                                <telerik:RadButton 
                                                    ID="buttonOpenOptions"
                                                    runat="server"
                                                    Text="Exclusion Options"
                                                    ButtonType="LinkButton"
                                                    AutoPostBack="false"
                                                    CssClass="KillButton"
                                                    OnClientClicked="OnOpenOptions"
                                                    >
                                                    <Icon PrimaryIconUrl="/_layouts/images/Vmgr/options-icon-24.png" />
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <style>
                                        .RadCalendarMultiView .rcCalendar {
                                            border-style: none !important;
                                        }
                                    </style>    
                                    <table style="border-collapse: collapse; width: 100%; height: 380px;">
                                        <tr>
                                            <td style="border-collapse: collapse; vertical-align: top; padding: 0px;">
                                                <telerik:RadCalendar 
                                                    runat="server" 
                                                    ID="calendarExclusions" 
                                                    AutoPostBack="true"
                                                    MultiViewColumns="3"
                                                    MultiViewRows="2" 
                                                    EnableMultiSelect="true" 
                                                    Width="100%"
                                                    OnSelectionChanged="calendarExclusions_SelectionChanged"
                                                    >
                                                    </telerik:RadCalendar>
                                            </td>
                                            <td 
                                                id="optionsTd" 
                                                runat="server" 
                                                style="
                                                    width: 250px; 
                                                    vertical-align: top; 
                                                    border: solid 1px gray; 
                                                    overflow: hidden; 
                                                    padding: 0px;
                                                    background-color: #FAFAFA;
                                                    "
                                                    >
                                                <div id="optionsDiv" style="overflow-y: hidden; overflow-x: hidden; border-collapse: collapse; position: relative; height: 100%;">
                                                    <div class="optionsDivRt" style="position: absolute; top: 3px; height: 100%;">
			                                            <telerik:RadButton 
                                                            ID="buttonCloseOptions"
                                                            runat="server"
                                                            Text="Close"
                                                            ButtonType="LinkButton"
                                                            AutoPostBack="false"
                                                            CssClass="CloseButton"
                                                            OnClientClicked="OnCloseOptions"
                                                            >
                                                            <Icon PrimaryIconUrl="/_layouts/images/Vmgr/cancel-icon-8.png" />
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div style="padding: 0px; position: absolute; top: 30px; left: 0px;">
                                                        <style type="text/css">
                                                            .RadTreeView .rtLI
                                                            {
                                                                padding-bottom: 8px;
                                                            }
                                                            .RadTreeView .rtUL .rtUL
                                                            {
                                                                margin-top: 8px;
                                                            }
                                                            .RadTreeView .rtLast
                                                            {
                                                                padding-bottom: 0; 
                                                            }

                                                            .RadTreeView .rtPlus, 
                                                            .RadTreeView .rtMinus
                                                            {
                                                                display: none !important;
                                                            }                                            

                                                        </style>
                                                        <telerik:RadTreeView 
                                                            ID="treeViewOptions" 
                                                            runat="server" 
                                                            Width="100%" 
                                                            OnClientLoad="OptionsOnLoad"
                                                            OnClientNodeClicked="OnOptionSelected"
                                                            style="overflow-x: hidden;"
                                                            >
                                                            <Nodes>
                                                                <telerik:RadTreeNode
                                                                    ImageUrl="/_layouts/images/Vmgr/undo-icon-24.png"
                                                                    Text="Undo exclusion changes"
                                                                    style="padding: 3px;"
                                                                    Value="UNDO_EXCLUSIONS"
                                                                    >
                                                                </telerik:RadTreeNode>
                                                                <telerik:RadTreeNode
                                                                    ImageUrl="/_layouts/images/Vmgr/trash-can-icon-24.png"
                                                                    Text="Remove all exclusions"
                                                                    style="padding: 3px;"
                                                                    Value="REMOVE_EXCLUSIONS"
                                                                    >
                                                                </telerik:RadTreeNode>
                                                            </Nodes>
                                                        </telerik:RadTreeView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel
                                        runat="server"
                                        ID="panelExclusions"
                                        Visible="false"
                                        >
                                        <br />
                                        <p>Jobs will be excluded from being triggered on the following days:</p>
                                        <asp:Label
                                            runat="server"
                                            ID="labelExclusions"
                                            style="color: orange;"
                                            ></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                        </telerik:RadAjaxPanel>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
        </div>
    </div>
</telerik:RadAjaxPanel>