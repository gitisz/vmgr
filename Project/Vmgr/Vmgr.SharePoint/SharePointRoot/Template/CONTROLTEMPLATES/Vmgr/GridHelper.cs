using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telerik.Web.UI;

namespace Vmgr.SharePoint
{
    public class GridHelper
    {
        public static string GetOrderBy(GridTableView tableView)
        {
            return GetOrderBy(tableView, string.Empty, new string[0]);
        }

        public static string GetOrderBy(GridTableView tableView, string fieldNamePrefix, params string[] skipFieldNames)
        {
            var foundKeys = new List<string>();
            var orderBy = new StringBuilder();

            if (fieldNamePrefix == null)
            {
                fieldNamePrefix = string.Empty;
            }

            if (skipFieldNames == null)
            {
                skipFieldNames = new string[0];
            }

            foreach (GridSortExpression sortExpression in tableView.SortExpressions)
            {
                switch (sortExpression.SortOrder)
                {
                    case GridSortOrder.Ascending:
                        Append(orderBy, skipFieldNames.Contains(sortExpression.FieldName) ? sortExpression.FieldName : fieldNamePrefix + sortExpression.FieldName);
                        if (tableView.DataKeyNames.Any(kn => !foundKeys.Contains(kn)))
                        {
                            foundKeys.Add(sortExpression.FieldName);
                        }
                        break;
                    case GridSortOrder.Descending:
                        Append(orderBy, skipFieldNames.Contains(sortExpression.FieldName) ? sortExpression.FieldName : fieldNamePrefix + sortExpression.FieldName, " desc");
                        if (tableView.DataKeyNames.Any(kn => !foundKeys.Contains(kn)))
                        {
                            foundKeys.Add(sortExpression.FieldName);
                        }
                        break;
                    case GridSortOrder.None:
                        break;
                    default:
                        break;
                }
            }

            foreach (string keyName in tableView.DataKeyNames.Where(kn => !foundKeys.Contains(kn)))
            {
                Append(orderBy, skipFieldNames.Contains(keyName) ? keyName : fieldNamePrefix + keyName);
            }

            return (orderBy.ToString());
        }

        private static void Append(StringBuilder builder, params string[] strings)
        {
            if (builder.Length > 0 && strings.Length > 0)
            {
                builder.Append(",");
            }
            foreach (var s in strings)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(s);
            }
        }

        public static string GetFilterExpression(GridTableView view, string fieldNamePrefix, params string[] skipFieldNames)
        {
            if (string.IsNullOrEmpty(fieldNamePrefix))
            {
                return view.FilterExpression;
            }

            if (skipFieldNames == null)
            {
                skipFieldNames = new string[0];
            }

            string result = view.FilterExpression;

            foreach (var column in view.Columns)
            {
                if (!(column is GridBoundColumn) || string.IsNullOrEmpty((column as GridBoundColumn).DataField))
                {
                    continue;
                }

                string fieldName = (column as GridBoundColumn).DataField;

                if (!skipFieldNames.Contains(fieldName))
                {
                    result = result.Replace(fieldName, fieldNamePrefix + fieldName);
                }
            }

            return result;
        }
    }
}
