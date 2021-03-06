﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;
using Vmgr;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.Linq;
using Vmgr.Data.SqlServer;
/*
* THIS CODE WAS AUTO-GENERATED.  DO NOT MAKE MODIFICATIONS.
*/

namespace Vmgr.Data.Biz
{
    public class XActAbortDynamicQueryEngine : DynamicQueryEngine
    {
        private SqlServerCompatibilityLevel _compatibilityLevel = _defaultCompatibilityLevel;
        private static SqlServerCompatibilityLevel _defaultCompatibilityLevel = SqlServerCompatibilityLevel.SqlServer2005;

        public static bool XActAbortOn;

        private void AppendArithAbortIfRequired(QueryFragments fragments, bool startOfQuery)
        {
            if (!this.IsCe() && ArithAbortOn)
            {
                if (startOfQuery)
                {
                    fragments.AddFragment("SET ARITHABORT ON;");
                }
                else
                {
                    fragments.AddFragment(";SET ARITHABORT OFF");
                }
            }
        }

        private void AppendXActAbortIfRequired(QueryFragments fragments, bool startOfQuery)
        {
            if (!this.IsCe() && XActAbortOn)
            {
                if (startOfQuery)
                {
                    fragments.AddFragment("SET XACT_ABORT ON;");
                }
                else
                {
                    fragments.AddFragment(";SET XACT_ABORT OFF");
                }
            }
        }

        private void WrapQueryInXActAbortOnOff(IActionQuery query)
        {
            if (!string.IsNullOrEmpty(query.Command.CommandText))
            {
                QueryFragments fragments = new QueryFragments();
                this.AppendXActAbortIfRequired(fragments, true);
                fragments.AddFragment(query.Command.CommandText);
                this.AppendXActAbortIfRequired(fragments, false);
                query.SetCommandText(fragments.ToString());
            }
        }

        private bool IsCe()
        {
            return IsCe(this._compatibilityLevel);
        }

        internal static bool IsCe(SqlServerCompatibilityLevel compatibilityLevel)
        {
            switch (compatibilityLevel)
            {
                case SqlServerCompatibilityLevel.SqlServerCE3x:
                case SqlServerCompatibilityLevel.SqlServerCE35:
                case SqlServerCompatibilityLevel.SqlServerCE40:
                    return true;
            }
            return false;
        }

        private static ParameterDirection GetOutputParameterDirection(SqlServerCompatibilityLevel compatibilityLevel)
        {
            if (!IsCe(compatibilityLevel))
            {
                return ParameterDirection.Output;
            }
            return ParameterDirection.Input;
        }

        protected override void CreateSingleTargetUpdateDQ(IEntityFieldCore[] fields
            , IFieldPersistenceInfo[] fieldsPersistenceInfo
            , IActionQuery query
            , IPredicate updateFilter
            , IRelationCollection relationsToWalk)
        {
            base.CreateSingleTargetUpdateDQ(fields, fieldsPersistenceInfo, query, updateFilter, relationsToWalk);
            
            this.WrapQueryInXActAbortOnOff(query);
        }

        protected override void CreateSingleTargetInsertDQ(IEntityFieldCore[] fields, IFieldPersistenceInfo[] fieldsPersistenceInfo, IActionQuery query, Dictionary<IEntityFieldCore, DbParameter> fieldToParameter)
        {
            TraceHelper.WriteLineIf(DynamicQueryEngineBase.Switch.TraceInfo, "CreateSingleTargetInsertDQ", "Method Enter");
            QueryFragments fragments = new QueryFragments();
            this.AppendXActAbortIfRequired(fragments, true);
            this.AppendArithAbortIfRequired(fragments, true);
            DelimitedStringList list = fragments.AddSemiColonFragmentList(false, true);
            fragments.AddFormatted("INSERT INTO {0}", new object[] { base.Creator.CreateObjectName(fieldsPersistenceInfo[0]) });
            DelimitedStringList list2 = fragments.AddCommaFragmentList(true);
            StringPlaceHolder holder = fragments.AddPlaceHolder();
            fragments.AddFragment("VALUES");
            DelimitedStringList list3 = fragments.AddCommaFragmentList(true);
            QueryFragments fragments2 = fragments.AddQueryFragments();
            this.AppendArithAbortIfRequired(fragments, false);
            this.AppendXActAbortIfRequired(fragments, false);
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < fields.Length; i++)
            {
                DbParameter parameter;
                IEntityFieldCore field = fields[i];
                IFieldPersistenceInfo persistenceInfo = fieldsPersistenceInfo[i];
                if (persistenceInfo.IsIdentity)
                {
                    parameter = base.Creator.CreateParameter(field, persistenceInfo, GetOutputParameterDirection(this._compatibilityLevel));
                    query.AddParameterFieldRelation(field, parameter, persistenceInfo.TypeConverterToUse);
                    if (this.IsCe())
                    {
                        parameter.Value = 0;
                        query.AddSequenceRetrievalQuery(base.CreateCommand("SELECT @@IDENTITY", query.Connection), false).AddSequenceParameter(parameter);
                    }
                    else
                    {
                        query.AddParameter(parameter);
                        fragments2.AddFormatted(";SELECT {0}={1}", new object[] { parameter.ParameterName, (DefaultCompatibilityLevel == SqlServerCompatibilityLevel.SqlServer7) ? "@@IDENTITY" : persistenceInfo.IdentityValueSequenceName });
                        flag = true;
                    }
                    fieldToParameter.Add(field, parameter);
                }
                else if (((!this.IsCe() && (persistenceInfo.SourceColumnDbType == "UniqueIdentifier")) && (field.IsPrimaryKey && !field.IsChanged)) && (((DefaultCompatibilityLevel == SqlServerCompatibilityLevel.SqlServer2005) && !flag2) && (field.LinkedSuperTypeField == null)))
                {
                    list.Add("DECLARE @__idTmp table(id uniqueidentifier)");
                    holder.SetFormatted("OUTPUT INSERTED.{0} INTO @__idTmp", new object[] { base.Creator.CreateFieldNameSimple(persistenceInfo, field.Name) });
                    parameter = base.Creator.CreateParameter(field, persistenceInfo, ParameterDirection.Output);
                    query.AddParameter(parameter);
                    query.AddParameterFieldRelation(field, parameter, persistenceInfo.TypeConverterToUse);
                    fragments2.AddFormatted(";SELECT top 1 {0}=id FROM @__idTmp", new object[] { parameter.ParameterName });
                    fieldToParameter.Add(field, parameter);
                    flag2 = true;
                }
                else if (this.CheckIfFieldNeedsInsertAction(field))
                {
                    list2.Add(base.Creator.CreateFieldNameSimple(persistenceInfo, field.Name));
                    parameter = base.Creator.CreateParameter(field, persistenceInfo, ParameterDirection.Input);
                    query.AddParameter(parameter);
                    list3.Add(parameter.ParameterName);
                    fieldToParameter.Add(field, parameter);
                }
            }
            if (list2.Count <= 0)
            {
                if ((!flag || this.IsCe()) || flag2)
                {
                    throw new ORMQueryConstructionException("The insert query doesn't contain any fields.");
                }
                holder.Value = "DEFAULT";
            }
            query.SetCommandText(fragments.ToString());

            XActAbortOn = false;

            TraceHelper.WriteIf(DynamicQueryEngineBase.Switch.TraceVerbose, query, "Generated Sql query");
            TraceHelper.WriteLineIf(DynamicQueryEngineBase.Switch.TraceInfo, "CreateSingleTargetInsertDQ", "Method Exit");
        }


        protected override void CreateSingleTargetDeleteDQ(IFieldPersistenceInfo[] fieldsPersistenceInfo
            , IActionQuery query
            , IPredicate deleteFilter
            , IRelationCollection relationsToWalk)
        {
            base.CreateSingleTargetDeleteDQ(fieldsPersistenceInfo, query, deleteFilter, relationsToWalk);

            this.WrapQueryInXActAbortOnOff(query);
        }
    }
}
