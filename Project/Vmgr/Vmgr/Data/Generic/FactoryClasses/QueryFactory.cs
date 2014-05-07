///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 4.1
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
////////////////////////////////////////////////////////////// 
using System;
using System.Linq;
using Vmgr.Data.Generic.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

namespace Vmgr.Data.Generic.FactoryClasses
{
	/// <summary>Factory class to produce DynamicQuery instances and EntityQuery instances</summary>
	public partial class QueryFactory
	{
		private int _aliasCounter = 0;

		/// <summary>Creates a new DynamicQuery instance with no alias set.</summary>
		/// <returns>Ready to use DynamicQuery instance</returns>
		public DynamicQuery Create()
		{
			return Create(string.Empty);
		}

		/// <summary>Creates a new DynamicQuery instance with the alias specified as the alias set.</summary>
		/// <param name="alias">The alias.</param>
		/// <returns>Ready to use DynamicQuery instance</returns>
		public DynamicQuery Create(string alias)
		{
			return new DynamicQuery(new ElementCreator(), alias, this.GetNextAliasCounterValue());
		}

		/// <summary>Creates a new DynamicQuery which wraps the specified TableValuedFunction call</summary>
		/// <param name="toWrap">The table valued function call to wrap.</param>
		/// <returns>toWrap wrapped in a DynamicQuery.</returns>
		public DynamicQuery Create(TableValuedFunctionCall toWrap)
		{
			return this.Create().From(new TvfCallWrapper(toWrap)).Select(toWrap.GetFieldsAsArray().Select(f => this.Field(toWrap.Alias, f.Alias)).ToArray());
		}

		/// <summary>Creates a new EntityQuery for the entity of the type specified with no alias set.</summary>
		/// <typeparam name="TEntity">The type of the entity to produce the query for.</typeparam>
		/// <returns>ready to use EntityQuery instance</returns>
		public EntityQuery<TEntity> Create<TEntity>()
			where TEntity : IEntityCore
		{
			return Create<TEntity>(string.Empty);
		}

		/// <summary>Creates a new EntityQuery for the entity of the type specified with the alias specified as the alias set.</summary>
		/// <typeparam name="TEntity">The type of the entity to produce the query for.</typeparam>
		/// <param name="alias">The alias.</param>
		/// <returns>ready to use EntityQuery instance</returns>
		public EntityQuery<TEntity> Create<TEntity>(string alias)
			where TEntity : IEntityCore
		{
			return new EntityQuery<TEntity>(new ElementCreator(), alias, this.GetNextAliasCounterValue());
		}
				
		/// <summary>Creates a new field object with the name specified and of resulttype 'object'. Used for referring to aliased fields in another projection.</summary>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField Field(string fieldName)
		{
			return Field<object>(string.Empty, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'object'. Used for referring to aliased fields in another projection.</summary>
		/// <param name="targetAlias">The alias of the table/query to target.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField Field(string targetAlias, string fieldName)
		{
			return Field<object>(targetAlias, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'TValue'. Used for referring to aliased fields in another projection.</summary>
		/// <typeparam name="TValue">The type of the value represented by the field.</typeparam>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField Field<TValue>(string fieldName)
		{
			return Field<TValue>(string.Empty, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'TValue'. Used for referring to aliased fields in another projection.</summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="targetAlias">The alias of the table/query to target.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField Field<TValue>(string targetAlias, string fieldName)
		{
			return new EntityField(fieldName, targetAlias, typeof(TValue));
		}
						
		/// <summary>Gets the next alias counter value to produce artifical aliases with</summary>
		private int GetNextAliasCounterValue()
		{
			_aliasCounter++;
			return _aliasCounter;
		}
		

		/// <summary>Creates and returns a new EntityQuery for the Filter entity</summary>
		public EntityQuery<FilterEntity> Filter
		{
			get { return Create<FilterEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Job entity</summary>
		public EntityQuery<JobEntity> Job
		{
			get { return Create<JobEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Log entity</summary>
		public EntityQuery<LogEntity> Log
		{
			get { return Create<LogEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Monitor entity</summary>
		public EntityQuery<MonitorEntity> Monitor
		{
			get { return Create<MonitorEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Package entity</summary>
		public EntityQuery<PackageEntity> Package
		{
			get { return Create<PackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Plugin entity</summary>
		public EntityQuery<PluginEntity> Plugin
		{
			get { return Create<PluginEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Schedule entity</summary>
		public EntityQuery<ScheduleEntity> Schedule
		{
			get { return Create<ScheduleEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SecurityMembership entity</summary>
		public EntityQuery<SecurityMembershipEntity> SecurityMembership
		{
			get { return Create<SecurityMembershipEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SecurityPermission entity</summary>
		public EntityQuery<SecurityPermissionEntity> SecurityPermission
		{
			get { return Create<SecurityPermissionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SecurityRole entity</summary>
		public EntityQuery<SecurityRoleEntity> SecurityRole
		{
			get { return Create<SecurityRoleEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SecurityRolePermission entity</summary>
		public EntityQuery<SecurityRolePermissionEntity> SecurityRolePermission
		{
			get { return Create<SecurityRolePermissionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SecuritySiteMap entity</summary>
		public EntityQuery<SecuritySiteMapEntity> SecuritySiteMap
		{
			get { return Create<SecuritySiteMapEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Server entity</summary>
		public EntityQuery<ServerEntity> Server
		{
			get { return Create<ServerEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Setting entity</summary>
		public EntityQuery<SettingEntity> Setting
		{
			get { return Create<SettingEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Trigger entity</summary>
		public EntityQuery<TriggerEntity> Trigger
		{
			get { return Create<TriggerEntity>(); }
		}

 

	}
}