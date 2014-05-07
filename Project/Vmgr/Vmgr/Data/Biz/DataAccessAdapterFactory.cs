using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr.Configuration;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.SqlServer;

namespace Vmgr.Data.Biz
{
    /// <summary>
    /// Controls access to the LLBLGen Database specific Adapter objects
    /// </summary>
    /// <remarks>You should wrap these Adapter methods inside of a using statement so that dispose is always called.</remarks>
    public static class DataAccessAdapterFactory
    {
        /// <summary>
        /// Standard Adapter. Read Committed Isolation Level.
        /// </summary>
        /// <returns></returns>
        public static IDataAccessAdapter CreateStandardAdapter()
        {
            IDataAccessAdapter adapter = new ImpersonatingDataAccessAdapter(false);
            adapter.ConnectionString = ConnectionStringManager.ConnectionString;

            return adapter;
        }

        public static IDataAccessAdapter CreateStandardAdapter(string connectionString)
        {
            IDataAccessAdapter adapter = DataAccessAdapterFactory.CreateStandardAdapter();
            adapter.ConnectionString = connectionString;

            return adapter;
        }

        /// <summary>
        /// ReadUncommited specifies that statements can read rows that have been modified by other transactions but not yet committed.  No locks are held during query.
        /// </summary>
        /// <returns>IDataAccessAdapter</returns>
        /// <remarks>
        /// A transaction is started in order to specify the isolation level.  Be sure to call dispose once done with the adapter.
        /// </remarks>
        public static IDataAccessAdapter CreateNoLockAdapter()
        {
            string stack = string.Empty;

#if DEBUG
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            stack = string.Join(", ", stackFrames.Select(s => s.GetMethod().Name).ToArray());
#endif
            string transactionName = Guid.NewGuid().ToUniqueName();
           
			IDataAccessAdapter adapter = CreateStandardAdapter();
            adapter.ConnectionString = ConnectionStringManager.ConnectionString;
           
			Logger.Logs.Log(string.Format("Starting transaction with name '{0}'. Stack: {1}", transactionName, stack), LogType.Info);
            
			adapter.StartTransaction(System.Data.IsolationLevel.ReadUncommitted, Guid.NewGuid().ToUniqueName());
           
			return adapter;
        }

        /// <summary>
        /// ReadCommitted specifies that statements cannot read data that has been modified but not committed by other transactions. 
        /// </summary>
        /// <returns>IDataAccessAdapter</returns>
        /// <remarks>
        /// A transaction is started in order to specify the isolation level.  Be sure to call dispose once done with the adapter.
        /// </remarks>
        public static IDataAccessAdapter CreateXActAbortAdapter()
        {
            XActAbortDataAccessAdapter.SetXActAbortFlag(true);

            IDataAccessAdapter adapter = new XActAbortDataAccessAdapter();
            adapter.ConnectionString = ConnectionStringManager.ConnectionString;
            adapter.StartTransaction(System.Data.IsolationLevel.ReadUncommitted, "DISTRIBUTED_READUNCOMMITED_ADAPTER");

            return adapter;
        }
    }

	
    public partial class ImpersonatingDataAccessAdapter : DataAccessAdapter
    {
        public ImpersonatingDataAccessAdapter(bool keepConnectionOpen)
            : base(keepConnectionOpen)
        {
        }

        public override void ExecuteSingleRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFields2 fieldsToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo)
        {
            Impersonation.Impersonate(delegate
            {
                base.ExecuteSingleRowRetrievalQuery(queryToExecute, fieldsToFill, fieldsPersistenceInfo);
            }
            );
        }

        public override void ExecuteMultiRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFactory2 entityFactory, IEntityCollection2 collectionToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, bool allowDuplicates, IEntityFields2 fieldsUsedForQuery)
        {
            Impersonation.Impersonate(delegate
            {
                base.ExecuteMultiRowRetrievalQuery(queryToExecute, entityFactory, collectionToFill, fieldsPersistenceInfo, allowDuplicates, fieldsUsedForQuery);
            }
            );
        }

        public override bool ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, System.Data.Common.DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo)
        {
            bool result = false;

            Impersonation.Impersonate(delegate
            {
                result = base.ExecuteMultiRowDataTableRetrievalQuery(queryToExecute, dataAdapterToUse, tableToFill, fieldsPersistenceInfo);
            }
            );

            return result;
        }

        public override int ExecuteActionQuery(IActionQuery queryToExecute)
        {
            int result = 0;

            Impersonation.Impersonate(delegate
            {
                result = base.ExecuteActionQuery(queryToExecute);
            }
            );

            return result;
        }

        public override object ExecuteScalarQuery(IRetrievalQuery queryToExecute)
        {
            object result = null;

            Impersonation.Impersonate(delegate
            {
                result = base.ExecuteScalarQuery(queryToExecute);
            }
            );

            return result;
        }

        public override IDataReader FetchDataReader(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior)
        {
            IDataReader reader = null;

            Impersonation.Impersonate(delegate
            {
                reader = base.FetchDataReader(queryToExecute, readerBehavior);
            }
            );

            return reader;
        }

        public override void StartTransaction(IsolationLevel isolationLevelToUse, string name)
        {
            Impersonation.Impersonate(delegate
            {
                base.StartTransaction(isolationLevelToUse, name);
            }
            );
        }
		
        public override int CallActionStoredProcedure(string storedProcedureToCall, System.Data.Common.DbParameter[] parameters)
        {
            int result = 0;

            Impersonation.Impersonate(delegate
            {
                result = base.CallActionStoredProcedure(storedProcedureToCall, parameters);
            }
            );
            
            return result;
        }
		
        public override bool CallRetrievalStoredProcedure(string storedProcedureToCall, System.Data.Common.DbParameter[] parameters, DataSet dataSetToFill)
        {

            bool result = false;

            Impersonation.Impersonate(delegate
            {
                result = base.CallRetrievalStoredProcedure(storedProcedureToCall, parameters, dataSetToFill);
            }
            );
            return result;
        }

        public override bool CallRetrievalStoredProcedure(string storedProcedureToCall, System.Data.Common.DbParameter[] parameters, DataTable tableToFill)
        {
            bool result = false;

            Impersonation.Impersonate(delegate
            {
                result = base.CallRetrievalStoredProcedure(storedProcedureToCall, parameters, tableToFill);
            }
            );
            return result;
        }
    }
}