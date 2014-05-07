///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 4.1
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;

namespace Vmgr.Data.Generic.Linq
{
	/// <summary>Meta-data class for the construction of Linq queries which are to be executed using LLBLGen Pro code.</summary>
	public partial class LinqMetaData: ILinqMetaData
	{
		#region Class Member Declarations
		private IDataAccessAdapter _adapterToUse;
		private FunctionMappingStore _customFunctionMappings;
		private Context _contextToUse;
		#endregion
		
		/// <summary>CTor. Using this ctor will leave the IDataAccessAdapter object to use empty. To be able to execute the query, an IDataAccessAdapter instance
		/// is required, and has to be set on the LLBLGenProProvider2 object in the query to execute. </summary>
		public LinqMetaData() : this(null, null)
		{
		}
		
		/// <summary>CTor which accepts an IDataAccessAdapter implementing object, which will be used to execute queries created with this metadata class.</summary>
		/// <param name="adapterToUse">the IDataAccessAdapter to use in queries created with this meta data</param>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(IDataAccessAdapter adapterToUse) : this (adapterToUse, null)
		{
		}

		/// <summary>CTor which accepts an IDataAccessAdapter implementing object, which will be used to execute queries created with this metadata class.</summary>
		/// <param name="adapterToUse">the IDataAccessAdapter to use in queries created with this meta data</param>
		/// <param name="customFunctionMappings">The custom function mappings to use. These take higher precedence than the ones in the DQE to use.</param>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(IDataAccessAdapter adapterToUse, FunctionMappingStore customFunctionMappings)
		{
			_adapterToUse = adapterToUse;
			_customFunctionMappings = customFunctionMappings;
		}
	
		/// <summary>returns the datasource to use in a Linq query for the entity type specified</summary>
		/// <param name="typeOfEntity">the type of the entity to get the datasource for</param>
		/// <returns>the requested datasource</returns>
		public IDataSource GetQueryableForEntity(int typeOfEntity)
		{
			IDataSource toReturn = null;
			switch((Vmgr.Data.Generic.EntityType)typeOfEntity)
			{
				case Vmgr.Data.Generic.EntityType.FilterEntity:
					toReturn = this.Filter;
					break;
				case Vmgr.Data.Generic.EntityType.JobEntity:
					toReturn = this.Job;
					break;
				case Vmgr.Data.Generic.EntityType.LogEntity:
					toReturn = this.Log;
					break;
				case Vmgr.Data.Generic.EntityType.MonitorEntity:
					toReturn = this.Monitor;
					break;
				case Vmgr.Data.Generic.EntityType.PackageEntity:
					toReturn = this.Package;
					break;
				case Vmgr.Data.Generic.EntityType.PluginEntity:
					toReturn = this.Plugin;
					break;
				case Vmgr.Data.Generic.EntityType.ScheduleEntity:
					toReturn = this.Schedule;
					break;
				case Vmgr.Data.Generic.EntityType.SecurityMembershipEntity:
					toReturn = this.SecurityMembership;
					break;
				case Vmgr.Data.Generic.EntityType.SecurityPermissionEntity:
					toReturn = this.SecurityPermission;
					break;
				case Vmgr.Data.Generic.EntityType.SecurityRoleEntity:
					toReturn = this.SecurityRole;
					break;
				case Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity:
					toReturn = this.SecurityRolePermission;
					break;
				case Vmgr.Data.Generic.EntityType.SecuritySiteMapEntity:
					toReturn = this.SecuritySiteMap;
					break;
				case Vmgr.Data.Generic.EntityType.ServerEntity:
					toReturn = this.Server;
					break;
				case Vmgr.Data.Generic.EntityType.SettingEntity:
					toReturn = this.Setting;
					break;
				case Vmgr.Data.Generic.EntityType.TriggerEntity:
					toReturn = this.Trigger;
					break;
				default:
					toReturn = null;
					break;
			}
			return toReturn;
		}

		/// <summary>returns the datasource to use in a Linq query for the entity type specified</summary>
		/// <typeparam name="TEntity">the type of the entity to get the datasource for</typeparam>
		/// <returns>the requested datasource</returns>
		public DataSource2<TEntity> GetQueryableForEntity<TEntity>()
			    where TEntity : class
		{
    		return new DataSource2<TEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse);
		}

		/// <summary>returns the datasource to use in a Linq query when targeting FilterEntity instances in the database.</summary>
		public DataSource2<FilterEntity> Filter
		{
			get { return new DataSource2<FilterEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting JobEntity instances in the database.</summary>
		public DataSource2<JobEntity> Job
		{
			get { return new DataSource2<JobEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting LogEntity instances in the database.</summary>
		public DataSource2<LogEntity> Log
		{
			get { return new DataSource2<LogEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MonitorEntity instances in the database.</summary>
		public DataSource2<MonitorEntity> Monitor
		{
			get { return new DataSource2<MonitorEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PackageEntity instances in the database.</summary>
		public DataSource2<PackageEntity> Package
		{
			get { return new DataSource2<PackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PluginEntity instances in the database.</summary>
		public DataSource2<PluginEntity> Plugin
		{
			get { return new DataSource2<PluginEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ScheduleEntity instances in the database.</summary>
		public DataSource2<ScheduleEntity> Schedule
		{
			get { return new DataSource2<ScheduleEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SecurityMembershipEntity instances in the database.</summary>
		public DataSource2<SecurityMembershipEntity> SecurityMembership
		{
			get { return new DataSource2<SecurityMembershipEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SecurityPermissionEntity instances in the database.</summary>
		public DataSource2<SecurityPermissionEntity> SecurityPermission
		{
			get { return new DataSource2<SecurityPermissionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SecurityRoleEntity instances in the database.</summary>
		public DataSource2<SecurityRoleEntity> SecurityRole
		{
			get { return new DataSource2<SecurityRoleEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SecurityRolePermissionEntity instances in the database.</summary>
		public DataSource2<SecurityRolePermissionEntity> SecurityRolePermission
		{
			get { return new DataSource2<SecurityRolePermissionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SecuritySiteMapEntity instances in the database.</summary>
		public DataSource2<SecuritySiteMapEntity> SecuritySiteMap
		{
			get { return new DataSource2<SecuritySiteMapEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ServerEntity instances in the database.</summary>
		public DataSource2<ServerEntity> Server
		{
			get { return new DataSource2<ServerEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SettingEntity instances in the database.</summary>
		public DataSource2<SettingEntity> Setting
		{
			get { return new DataSource2<SettingEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TriggerEntity instances in the database.</summary>
		public DataSource2<TriggerEntity> Trigger
		{
			get { return new DataSource2<TriggerEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
 
		#region Class Property Declarations
		/// <summary> Gets / sets the IDataAccessAdapter to use for the queries created with this meta data object.</summary>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public IDataAccessAdapter AdapterToUse
		{
			get { return _adapterToUse;}
			set { _adapterToUse = value;}
		}

		/// <summary>Gets or sets the custom function mappings to use. These take higher precedence than the ones in the DQE to use</summary>
		public FunctionMappingStore CustomFunctionMappings
		{
			get { return _customFunctionMappings; }
			set { _customFunctionMappings = value; }
		}
		
		/// <summary>Gets or sets the Context instance to use for entity fetches.</summary>
		public Context ContextToUse
		{
			get { return _contextToUse;}
			set { _contextToUse = value;}
		}
		#endregion
	}
}