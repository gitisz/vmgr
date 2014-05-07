using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using Vmgr;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.Linq;
using Vmgr.Data.SqlServer;
using Vmgr.Data.Biz.MetaData;
using System.Data;

namespace Vmgr.Data.Biz
{
    public partial class AppService
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public IList<LogMetaData> GetLogs(LogQueryParams logQueryParams, out int rowCount)
        {
            DataSet ds = RetrievalProcedures.GetLogs(
                  logQueryParams.PageNumber
                , logQueryParams.PageSize
                , logQueryParams.SortBy
                , logQueryParams.SortMode.ToString()
                )
                ;

            /*  RESULTSET
             * 
				  Id
			    , Thread
			    , ThreadId
			    , ApplicationName
			    , Level
			    , Logger
			    , Message
			    , Exception
			    , CreateDate
			    , CreateUser
             * 
             */

            IList<LogMetaData> list = ds.Tables[0].Rows.Cast<DataRow>()
                .Select(r => new LogMetaData
                {
                    Id = r["Id"].ToNullable<int>() ?? 0,
                    Thread = r["Thread"] as string,
                    ThreadId = r["ThreadId"] as string,
                    ApplicationName = r["ApplicationName"] as string,
                    Level = r["Level"] as string,
                    Logger = r["Logger"] as string,
                    Message = r["Message"] as string,
                    Exception = r["Exception"] as string,
                    CreateDate = (DateTime)r["CreateDate"],
                    CreateUser = r["CreateUser"] as string,
                }
                )
                .ToList()
                ;

            rowCount = ds.Tables[1].Rows[0][0] as int? ?? 0;

            return list;
        }

        public void Delete(int days)
        {
            ActionProcedures.DeleteLogs(days, this.Adapter);
            this.Adapter.Commit();
        }

        #endregion

        #region EVENTS

        #endregion
    }

    public class LogQueryParams : BaseQueryParams
    {
        public int? TotalNumberOfRows { get; set; }
    }

    public abstract class BaseQueryParams
    {
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SortModeType SortMode { get; set; }
        public string Filter { get; set; }
    }

    public enum SortModeType
    {
        Asc,
        Desc
    }
}
