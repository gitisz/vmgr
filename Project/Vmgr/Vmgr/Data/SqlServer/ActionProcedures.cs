///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 4.1
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.SqlServer
{
	/// <summary>Class which contains the static logic to execute action stored procedures in the database.</summary>
	public static partial class ActionProcedures
	{

		/// <summary>Delegate definition for stored procedure 'DeleteLogs' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int DeleteLogsCallBack(System.Int32 pDays, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'GetTriggersByPlugin' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int GetTriggersByPluginCallBack(System.Guid uniqueId, System.Int32 days, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'RemoveFilter' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int RemoveFilterCallBack(System.Int32 filterId, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'RemoveMonitor' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int RemoveMonitorCallBack(System.Int32 monitorId, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'RemovePackage' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int RemovePackageCallBack(System.Int32 packageId, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'RemovePlugin' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int RemovePluginCallBack(System.Int32 pluginId, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'RemoveSchedule' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int RemoveScheduleCallBack(System.Int32 scheduleId, IDataAccessCore dataAccessProvider);


		/// <summary>Calls stored procedure 'DeleteLogs'.<br/><br/></summary>
		/// <param name="pDays">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int DeleteLogs(System.Int32 pDays)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return DeleteLogs(pDays, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'DeleteLogs'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pDays">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int DeleteLogs(System.Int32 pDays, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateDeleteLogsCall(dataAccessProvider, pDays))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'GetTriggersByPlugin'.<br/><br/></summary>
		/// <param name="uniqueId">Input parameter. </param>
		/// <param name="days">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int GetTriggersByPlugin(System.Guid uniqueId, System.Int32 days)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return GetTriggersByPlugin(uniqueId, days, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'GetTriggersByPlugin'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="uniqueId">Input parameter. </param>
		/// <param name="days">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int GetTriggersByPlugin(System.Guid uniqueId, System.Int32 days, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateGetTriggersByPluginCall(dataAccessProvider, uniqueId, days))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'RemoveFilter'.<br/><br/></summary>
		/// <param name="filterId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveFilter(System.Int32 filterId)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return RemoveFilter(filterId, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'RemoveFilter'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="filterId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveFilter(System.Int32 filterId, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateRemoveFilterCall(dataAccessProvider, filterId))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'RemoveMonitor'.<br/><br/></summary>
		/// <param name="monitorId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveMonitor(System.Int32 monitorId)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return RemoveMonitor(monitorId, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'RemoveMonitor'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="monitorId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveMonitor(System.Int32 monitorId, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateRemoveMonitorCall(dataAccessProvider, monitorId))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'RemovePackage'.<br/><br/></summary>
		/// <param name="packageId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemovePackage(System.Int32 packageId)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return RemovePackage(packageId, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'RemovePackage'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="packageId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemovePackage(System.Int32 packageId, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateRemovePackageCall(dataAccessProvider, packageId))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'RemovePlugin'.<br/><br/></summary>
		/// <param name="pluginId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemovePlugin(System.Int32 pluginId)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return RemovePlugin(pluginId, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'RemovePlugin'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pluginId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemovePlugin(System.Int32 pluginId, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateRemovePluginCall(dataAccessProvider, pluginId))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'RemoveSchedule'.<br/><br/></summary>
		/// <param name="scheduleId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveSchedule(System.Int32 scheduleId)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return RemoveSchedule(scheduleId, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'RemoveSchedule'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="scheduleId">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int RemoveSchedule(System.Int32 scheduleId, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateRemoveScheduleCall(dataAccessProvider, scheduleId))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}
		
		/// <summary>Creates the call object for the call 'DeleteLogs' to stored procedure 'DeleteLogs'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pDays">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateDeleteLogsCall(IDataAccessCore dataAccessProvider, System.Int32 pDays)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[DeleteLogs]", "DeleteLogs")
							.AddParameter("@pDays", "Int", 0, ParameterDirection.Input, true, 10, 0, pDays);
		}

		/// <summary>Creates the call object for the call 'GetTriggersByPlugin' to stored procedure 'GetTriggersByPlugin'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="uniqueId">Input parameter</param>
		/// <param name="days">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateGetTriggersByPluginCall(IDataAccessCore dataAccessProvider, System.Guid uniqueId, System.Int32 days)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[GetTriggersByPlugin]", "GetTriggersByPlugin")
							.AddParameter("@uniqueId", "UniqueIdentifier", 0, ParameterDirection.Input, true, 0, 0, uniqueId)
							.AddParameter("@days", "Int", 0, ParameterDirection.Input, true, 10, 0, days);
		}

		/// <summary>Creates the call object for the call 'RemoveFilter' to stored procedure 'RemoveFilter'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="filterId">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateRemoveFilterCall(IDataAccessCore dataAccessProvider, System.Int32 filterId)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[RemoveFilter]", "RemoveFilter")
							.AddParameter("@FilterId", "Int", 0, ParameterDirection.Input, true, 10, 0, filterId);
		}

		/// <summary>Creates the call object for the call 'RemoveMonitor' to stored procedure 'RemoveMonitor'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="monitorId">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateRemoveMonitorCall(IDataAccessCore dataAccessProvider, System.Int32 monitorId)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[RemoveMonitor]", "RemoveMonitor")
							.AddParameter("@MonitorId", "Int", 0, ParameterDirection.Input, true, 10, 0, monitorId);
		}

		/// <summary>Creates the call object for the call 'RemovePackage' to stored procedure 'RemovePackage'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="packageId">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateRemovePackageCall(IDataAccessCore dataAccessProvider, System.Int32 packageId)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[RemovePackage]", "RemovePackage")
							.AddParameter("@packageId", "Int", 0, ParameterDirection.Input, true, 10, 0, packageId);
		}

		/// <summary>Creates the call object for the call 'RemovePlugin' to stored procedure 'RemovePlugin'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pluginId">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateRemovePluginCall(IDataAccessCore dataAccessProvider, System.Int32 pluginId)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[RemovePlugin]", "RemovePlugin")
							.AddParameter("@pluginId", "Int", 0, ParameterDirection.Input, true, 10, 0, pluginId);
		}

		/// <summary>Creates the call object for the call 'RemoveSchedule' to stored procedure 'RemoveSchedule'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="scheduleId">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateRemoveScheduleCall(IDataAccessCore dataAccessProvider, System.Int32 scheduleId)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[RemoveSchedule]", "RemoveSchedule")
							.AddParameter("@scheduleId", "Int", 0, ParameterDirection.Input, true, 10, 0, scheduleId);
		}


		#region Included Code

		#endregion
	}
}
