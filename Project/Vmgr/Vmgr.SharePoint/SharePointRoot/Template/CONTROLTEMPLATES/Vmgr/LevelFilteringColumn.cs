using Telerik.Web.UI;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using Vmgr.Data.Biz.MetaData;
using System.Collections.Generic;

namespace Vmgr.SharePoint
{
    public class LevelFilteringColumn : GridTemplateColumn
    {

        protected override void SetupFilterControls(TableCell cell)
        {
            var collection = new List<LogMetaData> { }; ;
            collection.Add(new LogMetaData { Level = null });
            collection.Add(new LogMetaData { Level = "INFO" });
            collection.Add(new LogMetaData { Level = "WARN" });
            collection.Add(new LogMetaData { Level = "ERROR" });

            RadComboBox rcBox = new RadComboBox();
            rcBox.ID = "dropDownListLevel";
            rcBox.AutoPostBack = true;
            rcBox.DataTextField = this.DataField;
            rcBox.DataValueField = this.DataField;
            rcBox.SelectedIndexChanged += rcBox_SelectedIndexChanged;
            rcBox.DataSource = collection;
            rcBox.Width = 65;
            rcBox.Attributes.Add("style", "padding-top: 5px;");
            cell.Controls.Add(rcBox);
        }

        protected override void SetCurrentFilterValueToControl(TableCell cell)
        {
            if (!(this.CurrentFilterValue == ""))
            {
                ((RadComboBox)cell.Controls[0]).Items.FindItemByText(this.CurrentFilterValue).Selected = true;
            }
        }

        protected override string GetCurrentFilterValueFromControl(TableCell cell)
        {
            string currentValue = ((RadComboBox)cell.Controls[0]).SelectedItem.Value;
            this.CurrentFilterFunction = (currentValue != "") ? GridKnownFunction.EqualTo : GridKnownFunction.NoFilter;
            return currentValue;
        }

        private void rcBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ((GridFilteringItem)(((RadComboBox)sender).Parent.Parent)).FireCommandEvent("Filter", new Pair());
        }
    }
}