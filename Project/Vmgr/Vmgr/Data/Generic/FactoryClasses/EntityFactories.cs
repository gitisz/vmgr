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
using System.Collections.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Generic.FactoryClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>general base class for the generated factories</summary>
	[Serializable]
	public partial class EntityFactoryBase2<TEntity> : EntityFactoryCore2
		where TEntity : EntityBase2, IEntity2
	{
		private readonly Vmgr.Data.Generic.EntityType _typeOfEntity;
		private readonly bool _isInHierarchy;
		
		/// <summary>CTor</summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <param name="isInHierarchy">If true, the entity of this factory is in an inheritance hierarchy, false otherwise</param>
		public EntityFactoryBase2(string entityName, Vmgr.Data.Generic.EntityType typeOfEntity, bool isInHierarchy) : base(entityName)
		{
			_typeOfEntity = typeOfEntity;
			_isInHierarchy = isInHierarchy;
		}
		
		/// <summary>Creates, using the generated EntityFieldsFactory, the IEntityFields2 object for the entity to create.</summary>
		/// <returns>Empty IEntityFields2 object.</returns>
		public override IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(_typeOfEntity);
		}
		
		/// <summary>Creates a new entity instance using the GeneralEntityFactory in the generated code, using the passed in entitytype value</summary>
		/// <param name="entityTypeValue">The entity type value of the entity to create an instance for.</param>
		/// <returns>new IEntity instance</returns>
		public override IEntity2 CreateEntityFromEntityTypeValue(int entityTypeValue)
		{
			return GeneralEntityFactory.Create((Vmgr.Data.Generic.EntityType)entityTypeValue);
		}

		/// <summary>Creates the relations collection to the entity to join all targets so this entity can be fetched. </summary>
		/// <param name="objectAlias">The object alias to use for the elements in the relations.</param>
		/// <returns>null if the entity isn't in a hierarchy of type TargetPerEntity, otherwise the relations collection needed to join all targets together to fetch all subtypes of this entity and this entity itself</returns>
		public override IRelationCollection CreateHierarchyRelations(string objectAlias) 
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetHierarchyRelations(this.ForEntityName, objectAlias);
		}

		/// <summary>This method retrieves, using the InheritanceInfoprovider, the factory for the entity represented by the values passed in.</summary>
		/// <param name="fieldValues">Field values read from the db, to determine which factory to return, based on the field values passed in.</param>
		/// <param name="entityFieldStartIndexesPerEntity">indexes into values where per entity type their own fields start.</param>
		/// <returns>the factory for the entity which is represented by the values passed in.</returns>
		public override IEntityFactory2 GetEntityFactory(object[] fieldValues, Dictionary<string, int> entityFieldStartIndexesPerEntity) 
		{
			IEntityFactory2 toReturn = (IEntityFactory2)InheritanceInfoProviderSingleton.GetInstance().GetEntityFactory(this.ForEntityName, fieldValues, entityFieldStartIndexesPerEntity);
			if(toReturn == null)
			{
				toReturn = this;
			}
			return toReturn;
		}
		
		/// <summary>Gets a predicateexpression which filters on the entity with type belonging to this factory.</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <param name="objectAlias">The object alias to use for the predicate(s).</param>
		/// <returns>ready to use predicateexpression, or an empty predicate expression if the belonging entity isn't a hierarchical type.</returns>
		public override IPredicateExpression GetEntityTypeFilter(bool negate, string objectAlias) 
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter(this.ForEntityName, objectAlias, negate);
		}
						
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity which this factory belongs to.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return _isInHierarchy ? new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields(this.ForEntityName), InheritanceInfoProviderSingleton.GetInstance(), null) : base.CreateHierarchyFields();
		}
	}

	/// <summary>Factory to create new, empty FilterEntity objects.</summary>
	[Serializable]
	public partial class FilterEntityFactory : EntityFactoryBase2<FilterEntity> {
		/// <summary>CTor</summary>
		public FilterEntityFactory() : base("FilterEntity", Vmgr.Data.Generic.EntityType.FilterEntity, false) { }
		
		/// <summary>Creates a new FilterEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty JobEntity objects.</summary>
	[Serializable]
	public partial class JobEntityFactory : EntityFactoryBase2<JobEntity> {
		/// <summary>CTor</summary>
		public JobEntityFactory() : base("JobEntity", Vmgr.Data.Generic.EntityType.JobEntity, false) { }
		
		/// <summary>Creates a new JobEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new JobEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewJobUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LogEntity objects.</summary>
	[Serializable]
	public partial class LogEntityFactory : EntityFactoryBase2<LogEntity> {
		/// <summary>CTor</summary>
		public LogEntityFactory() : base("LogEntity", Vmgr.Data.Generic.EntityType.LogEntity, false) { }
		
		/// <summary>Creates a new LogEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LogEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLogUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MonitorEntity objects.</summary>
	[Serializable]
	public partial class MonitorEntityFactory : EntityFactoryBase2<MonitorEntity> {
		/// <summary>CTor</summary>
		public MonitorEntityFactory() : base("MonitorEntity", Vmgr.Data.Generic.EntityType.MonitorEntity, false) { }
		
		/// <summary>Creates a new MonitorEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MonitorEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMonitorUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PackageEntity objects.</summary>
	[Serializable]
	public partial class PackageEntityFactory : EntityFactoryBase2<PackageEntity> {
		/// <summary>CTor</summary>
		public PackageEntityFactory() : base("PackageEntity", Vmgr.Data.Generic.EntityType.PackageEntity, false) { }
		
		/// <summary>Creates a new PackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PackageEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PluginEntity objects.</summary>
	[Serializable]
	public partial class PluginEntityFactory : EntityFactoryBase2<PluginEntity> {
		/// <summary>CTor</summary>
		public PluginEntityFactory() : base("PluginEntity", Vmgr.Data.Generic.EntityType.PluginEntity, false) { }
		
		/// <summary>Creates a new PluginEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PluginEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPluginUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ScheduleEntity objects.</summary>
	[Serializable]
	public partial class ScheduleEntityFactory : EntityFactoryBase2<ScheduleEntity> {
		/// <summary>CTor</summary>
		public ScheduleEntityFactory() : base("ScheduleEntity", Vmgr.Data.Generic.EntityType.ScheduleEntity, false) { }
		
		/// <summary>Creates a new ScheduleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ScheduleEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewScheduleUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SecurityMembershipEntity objects.</summary>
	[Serializable]
	public partial class SecurityMembershipEntityFactory : EntityFactoryBase2<SecurityMembershipEntity> {
		/// <summary>CTor</summary>
		public SecurityMembershipEntityFactory() : base("SecurityMembershipEntity", Vmgr.Data.Generic.EntityType.SecurityMembershipEntity, false) { }
		
		/// <summary>Creates a new SecurityMembershipEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SecurityMembershipEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSecurityMembershipUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SecurityPermissionEntity objects.</summary>
	[Serializable]
	public partial class SecurityPermissionEntityFactory : EntityFactoryBase2<SecurityPermissionEntity> {
		/// <summary>CTor</summary>
		public SecurityPermissionEntityFactory() : base("SecurityPermissionEntity", Vmgr.Data.Generic.EntityType.SecurityPermissionEntity, false) { }
		
		/// <summary>Creates a new SecurityPermissionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SecurityPermissionEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSecurityPermissionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SecurityRoleEntity objects.</summary>
	[Serializable]
	public partial class SecurityRoleEntityFactory : EntityFactoryBase2<SecurityRoleEntity> {
		/// <summary>CTor</summary>
		public SecurityRoleEntityFactory() : base("SecurityRoleEntity", Vmgr.Data.Generic.EntityType.SecurityRoleEntity, false) { }
		
		/// <summary>Creates a new SecurityRoleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SecurityRoleEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSecurityRoleUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SecurityRolePermissionEntity objects.</summary>
	[Serializable]
	public partial class SecurityRolePermissionEntityFactory : EntityFactoryBase2<SecurityRolePermissionEntity> {
		/// <summary>CTor</summary>
		public SecurityRolePermissionEntityFactory() : base("SecurityRolePermissionEntity", Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity, false) { }
		
		/// <summary>Creates a new SecurityRolePermissionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SecurityRolePermissionEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSecurityRolePermissionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SecuritySiteMapEntity objects.</summary>
	[Serializable]
	public partial class SecuritySiteMapEntityFactory : EntityFactoryBase2<SecuritySiteMapEntity> {
		/// <summary>CTor</summary>
		public SecuritySiteMapEntityFactory() : base("SecuritySiteMapEntity", Vmgr.Data.Generic.EntityType.SecuritySiteMapEntity, false) { }
		
		/// <summary>Creates a new SecuritySiteMapEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SecuritySiteMapEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSecuritySiteMapUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ServerEntity objects.</summary>
	[Serializable]
	public partial class ServerEntityFactory : EntityFactoryBase2<ServerEntity> {
		/// <summary>CTor</summary>
		public ServerEntityFactory() : base("ServerEntity", Vmgr.Data.Generic.EntityType.ServerEntity, false) { }
		
		/// <summary>Creates a new ServerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServerEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SettingEntity objects.</summary>
	[Serializable]
	public partial class SettingEntityFactory : EntityFactoryBase2<SettingEntity> {
		/// <summary>CTor</summary>
		public SettingEntityFactory() : base("SettingEntity", Vmgr.Data.Generic.EntityType.SettingEntity, false) { }
		
		/// <summary>Creates a new SettingEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SettingEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSettingUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TriggerEntity objects.</summary>
	[Serializable]
	public partial class TriggerEntityFactory : EntityFactoryBase2<TriggerEntity> {
		/// <summary>CTor</summary>
		public TriggerEntityFactory() : base("TriggerEntity", Vmgr.Data.Generic.EntityType.TriggerEntity, false) { }
		
		/// <summary>Creates a new TriggerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TriggerEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTriggerUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty Entity objects based on the entity type specified. Uses  entity specific factory objects</summary>
	[Serializable]
	public partial class GeneralEntityFactory
	{
		/// <summary>Creates a new, empty Entity object of the type specified</summary>
		/// <param name="entityTypeToCreate">The entity type to create.</param>
		/// <returns>A new, empty Entity object.</returns>
		public static IEntity2 Create(Vmgr.Data.Generic.EntityType entityTypeToCreate)
		{
			IEntityFactory2 factoryToUse = null;
			switch(entityTypeToCreate)
			{
				case Vmgr.Data.Generic.EntityType.FilterEntity:
					factoryToUse = new FilterEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.JobEntity:
					factoryToUse = new JobEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.LogEntity:
					factoryToUse = new LogEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.MonitorEntity:
					factoryToUse = new MonitorEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.PackageEntity:
					factoryToUse = new PackageEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.PluginEntity:
					factoryToUse = new PluginEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.ScheduleEntity:
					factoryToUse = new ScheduleEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SecurityMembershipEntity:
					factoryToUse = new SecurityMembershipEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SecurityPermissionEntity:
					factoryToUse = new SecurityPermissionEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SecurityRoleEntity:
					factoryToUse = new SecurityRoleEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity:
					factoryToUse = new SecurityRolePermissionEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SecuritySiteMapEntity:
					factoryToUse = new SecuritySiteMapEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.ServerEntity:
					factoryToUse = new ServerEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.SettingEntity:
					factoryToUse = new SettingEntityFactory();
					break;
				case Vmgr.Data.Generic.EntityType.TriggerEntity:
					factoryToUse = new TriggerEntityFactory();
					break;
			}
			IEntity2 toReturn = null;
			if(factoryToUse != null)
			{
				toReturn = factoryToUse.Create();
			}
			return toReturn;
		}		
	}
		
	/// <summary>Class which is used to obtain the entity factory based on the .NET type of the entity. </summary>
	[Serializable]
	public static class EntityFactoryFactory
	{
		private static Dictionary<Type, IEntityFactory2> _factoryPerType = new Dictionary<Type, IEntityFactory2>();

		/// <summary>Initializes the <see cref="EntityFactoryFactory"/> class.</summary>
		static EntityFactoryFactory()
		{
			Array entityTypeValues = Enum.GetValues(typeof(Vmgr.Data.Generic.EntityType));
			foreach(int entityTypeValue in entityTypeValues)
			{
				IEntity2 dummy = GeneralEntityFactory.Create((Vmgr.Data.Generic.EntityType)entityTypeValue);
				_factoryPerType.Add(dummy.GetType(), dummy.GetEntityFactory());
			}
		}

		/// <summary>Gets the factory of the entity with the .NET type specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory2 GetFactory(Type typeOfEntity)
		{
			IEntityFactory2 toReturn = null;
			_factoryPerType.TryGetValue(typeOfEntity, out toReturn);
			return toReturn;
		}

		/// <summary>Gets the factory of the entity with the Vmgr.Data.Generic.EntityType specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory2 GetFactory(Vmgr.Data.Generic.EntityType typeOfEntity)
		{
			return GetFactory(GeneralEntityFactory.Create(typeOfEntity).GetType());
		}
	}
		
	/// <summary>Element creator for creating project elements from somewhere else, like inside Linq providers.</summary>
	public class ElementCreator : ElementCreatorBase, IElementCreator2
	{
		/// <summary>Gets the factory of the Entity type with the Vmgr.Data.Generic.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory2 GetFactory(int entityTypeValue)
		{
			return (IEntityFactory2)this.GetFactoryImpl(entityTypeValue);
		}
		
		/// <summary>Gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory2 GetFactory(Type typeOfEntity)
		{
			return (IEntityFactory2)this.GetFactoryImpl(typeOfEntity);
		}

		/// <summary>Creates a new resultset fields object with the number of field slots reserved as specified</summary>
		/// <param name="numberOfFields">The number of fields.</param>
		/// <returns>ready to use resultsetfields object</returns>
		public IEntityFields2 CreateResultsetFields(int numberOfFields)
		{
			return new ResultsetFields(numberOfFields);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand)
		{
			return new DynamicRelation(leftOperand);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperand">The right operand.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, DerivedTableDefinition rightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, rightOperand, onClause);
		}
		
		/// <summary>Obtains the inheritance info provider instance from the singleton </summary>
		/// <returns>The singleton instance of the inheritance info provider</returns>
		public override IInheritanceInfoProvider ObtainInheritanceInfoProviderInstance()
		{
			return InheritanceInfoProviderSingleton.GetInstance();
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, string rightOperandEntityName, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, (Vmgr.Data.Generic.EntityType)Enum.Parse(typeof(Vmgr.Data.Generic.EntityType), rightOperandEntityName, false), aliasRightOperand, onClause);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperandEntityName">Name of the entity which is used as the left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasLeftOperand">The alias of the left operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(string leftOperandEntityName, JoinHint joinType, string rightOperandEntityName, string aliasLeftOperand, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation((Vmgr.Data.Generic.EntityType)Enum.Parse(typeof(Vmgr.Data.Generic.EntityType), leftOperandEntityName, false), joinType, (Vmgr.Data.Generic.EntityType)Enum.Parse(typeof(Vmgr.Data.Generic.EntityType), rightOperandEntityName, false), aliasLeftOperand, aliasRightOperand, onClause);
		}
		
		/// <summary>Implementation of the routine which gets the factory of the Entity type with the Vmgr.Data.Generic.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(int entityTypeValue)
		{
			return EntityFactoryFactory.GetFactory((Vmgr.Data.Generic.EntityType)entityTypeValue);
		}

		/// <summary>Implementation of the routine which gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(Type typeOfEntity)
		{
			return EntityFactoryFactory.GetFactory(typeOfEntity);
		}

	}
}
