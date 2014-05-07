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
	/// <summary>Class which contains the static logic to execute retrieval stored procedures in the database.</summary>
	public static partial class RetrievalProcedures
	{



		/// <summary>Calls stored procedure 'GetLogs'.<br/><br/></summary>
		/// <param name="pPageNumber">Input parameter. </param>
		/// <param name="pPageSize">Input parameter. </param>
		/// <param name="pSortBy">Input parameter. </param>
		/// <param name="pSortMode">Input parameter. </param>
		/// <returns>Filled DataSet with resultset(s) of stored procedure</returns>
		public static DataSet GetLogs(System.Int32 pPageNumber, System.Int32 pPageSize, System.String pSortBy, System.String pSortMode)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return GetLogs(pPageNumber, pPageSize, pSortBy, pSortMode, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'GetLogs'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pPageNumber">Input parameter. </param>
		/// <param name="pPageSize">Input parameter. </param>
		/// <param name="pSortBy">Input parameter. </param>
		/// <param name="pSortMode">Input parameter. </param>
		/// <returns>Filled DataSet with resultset(s) of stored procedure</returns>
		public static DataSet GetLogs(System.Int32 pPageNumber, System.Int32 pPageSize, System.String pSortBy, System.String pSortMode, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateGetLogsCall(dataAccessProvider, pPageNumber, pPageSize, pSortBy, pSortMode))
			{
				DataSet toReturn = call.FillDataSet();
				return toReturn;
			}
		}

		/// <summary>Creates an IRetrievalQuery object for a call to the procedure 'GetLogs'.</summary>
		/// <param name="pPageNumber">Input parameter of stored procedure</param>
		/// <param name="pPageSize">Input parameter of stored procedure</param>
		/// <param name="pSortBy">Input parameter of stored procedure</param>
		/// <param name="pSortMode">Input parameter of stored procedure</param>
		/// <returns>IRetrievalQuery object which is ready to use for datafetching</returns>
		public static IRetrievalQuery GetGetLogsCallAsQuery(System.Int32 pPageNumber, System.Int32 pPageSize, System.String pSortBy, System.String pSortMode)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return CreateGetLogsCall(dataAccessProvider, pPageNumber, pPageSize, pSortBy, pSortMode).ToRetrievalQuery();
			}
		}

		/// <summary>Creates the call object for the call 'GetLogs' to stored procedure 'GetLogs'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="pPageNumber">Input parameter</param>
		/// <param name="pPageSize">Input parameter</param>
		/// <param name="pSortBy">Input parameter</param>
		/// <param name="pSortMode">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateGetLogsCall(IDataAccessCore dataAccessProvider, System.Int32 pPageNumber, System.Int32 pPageSize, System.String pSortBy, System.String pSortMode)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[DOM_Config].[vmg].[GetLogs]", "GetLogs")
							.AddParameter("@pPageNumber", "Int", 0, ParameterDirection.Input, true, 10, 0, pPageNumber)
							.AddParameter("@pPageSize", "Int", 0, ParameterDirection.Input, true, 10, 0, pPageSize)
							.AddParameter("@pSortBy", "VarChar", 50, ParameterDirection.Input, true, 0, 0, pSortBy)
							.AddParameter("@pSortMode", "VarChar", 10, ParameterDirection.Input, true, 0, 0, pSortMode);
		}


		#region Included Code

		#endregion
	}
}
