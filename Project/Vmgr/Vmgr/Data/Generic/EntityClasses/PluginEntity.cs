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
using System.ComponentModel;
using System.Collections.Generic;
#if !CF
using System.Runtime.Serialization;
#endif
using System.Xml.Serialization;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr;
using Vmgr.Data.SqlServer;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Generic.EntityClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	/// <summary>Entity class which represents the entity 'Plugin'.<br/><br/>
	/// DataContract: true<br/></summary>
	[Serializable]
	public partial class PluginEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<ScheduleEntity> _schedules;
		private PackageEntity _package;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Package</summary>
			public static readonly string Package = "Package";
			/// <summary>Member name Schedules</summary>
			public static readonly string Schedules = "Schedules";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static PluginEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public PluginEntity():base("PluginEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public PluginEntity(IEntityFields2 fields):base("PluginEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this PluginEntity</param>
		public PluginEntity(IValidator validator):base("PluginEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="pluginId">PK value for Plugin which data should be fetched into this Plugin object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PluginEntity(System.Int32 pluginId):base("PluginEntity")
		{
			InitClassEmpty(null, null);
			this.PluginId = pluginId;
		}

		/// <summary> CTor</summary>
		/// <param name="pluginId">PK value for Plugin which data should be fetched into this Plugin object</param>
		/// <param name="validator">The custom validator object for this PluginEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PluginEntity(System.Int32 pluginId, IValidator validator):base("PluginEntity")
		{
			InitClassEmpty(validator, null);
			this.PluginId = pluginId;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected PluginEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_schedules = (EntityCollection<ScheduleEntity>)info.GetValue("_schedules", typeof(EntityCollection<ScheduleEntity>));
				_package = (PackageEntity)info.GetValue("_package", typeof(PackageEntity));
				if(_package!=null)
				{
					_package.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((PluginFieldIndex)fieldIndex)
			{
				case PluginFieldIndex.PackageId:
					DesetupSyncPackage(true, false);
					break;
				default:
					base.PerformDesyncSetupFKFieldChange(fieldIndex);
					break;
			}
		}

		/// <summary> Sets the related entity property to the entity specified. If the property is a collection, it will add the entity specified to that collection.</summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="entity">Entity to set as an related entity</param>
		/// <remarks>Used by prefetch path logic.</remarks>
		protected override void SetRelatedEntityProperty(string propertyName, IEntityCore entity)
		{
			switch(propertyName)
			{
				case "Package":
					this.Package = (PackageEntity)entity;
					break;
				case "Schedules":
					this.Schedules.Add((ScheduleEntity)entity);
					break;
				default:
					this.OnSetRelatedEntityProperty(propertyName, entity);
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		protected override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		internal static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "Package":
					toReturn.Add(Relations.PackageEntityUsingPackageId);
					break;
				case "Schedules":
					toReturn.Add(Relations.ScheduleEntityUsingPluginId);
					break;
				default:
					break;				
			}
			return toReturn;
		}
#if !CF
		/// <summary>Checks if the relation mapped by the property with the name specified is a one way / single sided relation. If the passed in name is null, it/ will return true if the entity has any single-sided relation</summary>
		/// <param name="propertyName">Name of the property which is mapped onto the relation to check, or null to check if the entity has any relation/ which is single sided</param>
		/// <returns>true if the relation is single sided / one way (so the opposite relation isn't present), false otherwise</returns>
		protected override bool CheckOneWayRelations(string propertyName)
		{
			int numberOfOneWayRelations = 0;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				default:
					return base.CheckOneWayRelations(propertyName);
			}
		}
#endif
		/// <summary> Sets the internal parameter related to the fieldname passed to the instance relatedEntity. </summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		protected override void SetRelatedEntity(IEntityCore relatedEntity, string fieldName)
		{
			switch(fieldName)
			{
				case "Package":
					SetupSyncPackage(relatedEntity);
					break;
				case "Schedules":
					this.Schedules.Add((ScheduleEntity)relatedEntity);
					break;
				default:
					break;
			}
		}

		/// <summary> Unsets the internal parameter related to the fieldname passed to the instance relatedEntity. Reverses the actions taken by SetRelatedEntity() </summary>
		/// <param name="relatedEntity">Instance to unset as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		/// <param name="signalRelatedEntityManyToOne">if set to true it will notify the manytoone side, if applicable.</param>
		protected override void UnsetRelatedEntity(IEntityCore relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "Package":
					DesetupSyncPackage(false, true);
					break;
				case "Schedules":
					this.PerformRelatedEntityRemoval(this.Schedules, relatedEntity, signalRelatedEntityManyToOne);
					break;
				default:
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			if(_package!=null)
			{
				toReturn.Add(_package);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Schedules);
			return toReturn;
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_schedules", ((_schedules!=null) && (_schedules.Count>0) && !this.MarkedForDeletion)?_schedules:null);
				info.AddValue("_package", (!this.MarkedForDeletion?_package:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new PluginRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Schedule' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoSchedules()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ScheduleFields.PluginId, null, ComparisonOperator.Equal, this.PluginId));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Package' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPackage()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PackageFields.PackageId, null, ComparisonOperator.Equal, this.PackageId));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(PluginEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._schedules);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._schedules = (EntityCollection<ScheduleEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._schedules != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ScheduleEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ScheduleEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Package", _package);
			toReturn.Add("Schedules", _schedules);
			return toReturn;
		}

		/// <summary> Initializes the class members</summary>
		private void InitClassMembers()
		{
			PerformDependencyInjection();
			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
			OnInitClassMembersComplete();
		}


		#region Custom Property Hashtable Setup
		/// <summary> Initializes the hashtables for the entity type and entity field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Dictionary<string, string>();
			_fieldsCustomProperties = new Dictionary<string, Dictionary<string, string>>();
			_customProperties.Add("DataContract", @"true");
			Dictionary<string, string> fieldHashtable;
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("CreateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("CreateUser", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("PackageId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("PluginId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Schedulable", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("UniqueId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("UpdateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("UpdateUser", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _package</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncPackage(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _package, new PropertyChangedEventHandler( OnPackagePropertyChanged ), "Package", Vmgr.Data.Generic.RelationClasses.StaticPluginRelations.PackageEntityUsingPackageIdStatic, true, signalRelatedEntity, "Plugins", resetFKFields, new int[] { (int)PluginFieldIndex.PackageId } );
			_package = null;
		}

		/// <summary> setups the sync logic for member _package</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncPackage(IEntityCore relatedEntity)
		{
			if(_package!=relatedEntity)
			{
				DesetupSyncPackage(true, true);
				_package = (PackageEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _package, new PropertyChangedEventHandler( OnPackagePropertyChanged ), "Package", Vmgr.Data.Generic.RelationClasses.StaticPluginRelations.PackageEntityUsingPackageIdStatic, true, new string[] { "PackageDeactivated", "PackageServerId", "PackageUniqueId" } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPackagePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				case "Deactivated":
					this.OnPropertyChanged("PackageDeactivated");
					break;
				case "ServerId":
					this.OnPropertyChanged("PackageServerId");
					break;
				case "UniqueId":
					this.OnPropertyChanged("PackageUniqueId");
					break;
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this PluginEntity</param>
		/// <param name="fields">Fields of this entity</param>
		private void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			this.Fields = fields ?? CreateFields();
			this.Validator = validator;
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END

			OnInitialized();

		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public  static PluginRelations Relations
		{
			get	{ return new PluginRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Schedule' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathSchedules
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ScheduleEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ScheduleEntityFactory))), (IEntityRelation)GetRelationsForField("Schedules")[0], (int)Vmgr.Data.Generic.EntityType.PluginEntity, (int)Vmgr.Data.Generic.EntityType.ScheduleEntity, 0, null, null, null, null, "Schedules", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Package' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPackage
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(PackageEntityFactory))),	(IEntityRelation)GetRelationsForField("Package")[0], (int)Vmgr.Data.Generic.EntityType.PluginEntity, (int)Vmgr.Data.Generic.EntityType.PackageEntity, 0, null, null, null, null, "Package", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}


		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return CustomProperties;}
		}

		/// <summary> The custom properties for the fields of this entity type. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary> The custom properties for the fields of the type of this entity instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
		{
			get { return FieldsCustomProperties;}
		}

		/// <summary> The CreateDate property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."CreateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreateDate
		{
			get { return (System.DateTime)GetValue((int)PluginFieldIndex.CreateDate, true); }
			set	{ SetValue((int)PluginFieldIndex.CreateDate, value); }
		}

		/// <summary> The CreateUser property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."CreateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CreateUser
		{
			get { return (System.String)GetValue((int)PluginFieldIndex.CreateUser, true); }
			set	{ SetValue((int)PluginFieldIndex.CreateUser, value); }
		}

		/// <summary> The Description property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)PluginFieldIndex.Description, true); }
			set	{ SetValue((int)PluginFieldIndex.Description, value); }
		}

		/// <summary> The Name property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)PluginFieldIndex.Name, true); }
			set	{ SetValue((int)PluginFieldIndex.Name, value); }
		}

		/// <summary> The PackageId property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."PackageId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PackageId
		{
			get { return (System.Int32)GetValue((int)PluginFieldIndex.PackageId, true); }
			set	{ SetValue((int)PluginFieldIndex.PackageId, value); }
		}

		/// <summary> The PluginId property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."PluginId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 PluginId
		{
			get { return (System.Int32)GetValue((int)PluginFieldIndex.PluginId, true); }
			set	{ SetValue((int)PluginFieldIndex.PluginId, value); }
		}

		/// <summary> The Schedulable property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."Schedulable"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Schedulable
		{
			get { return (System.Boolean)GetValue((int)PluginFieldIndex.Schedulable, true); }
			set	{ SetValue((int)PluginFieldIndex.Schedulable, value); }
		}

		/// <summary> The UniqueId property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."UniqueId"<br/>
		/// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Guid UniqueId
		{
			get { return (System.Guid)GetValue((int)PluginFieldIndex.UniqueId, true); }
			set	{ SetValue((int)PluginFieldIndex.UniqueId, value); }
		}

		/// <summary> The UpdateDate property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."UpdateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> UpdateDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)PluginFieldIndex.UpdateDate, false); }
			set	{ SetValue((int)PluginFieldIndex.UpdateDate, value); }
		}

		/// <summary> The UpdateUser property of the Entity Plugin<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Plugin"."UpdateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpdateUser
		{
			get { return (System.String)GetValue((int)PluginFieldIndex.UpdateUser, true); }
			set	{ SetValue((int)PluginFieldIndex.UpdateUser, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ScheduleEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ScheduleEntity))]
		public virtual EntityCollection<ScheduleEntity> Schedules
		{
			get { return GetOrCreateEntityCollection<ScheduleEntity, ScheduleEntityFactory>("Plugin", true, false, ref _schedules);	}
		}

		/// <summary> Gets / sets related entity of type 'PackageEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual PackageEntity Package
		{
			get	{ return _package; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncPackage(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Plugins", "Package", _package, true); 
				}
			}
		}
 
		/// <summary> Gets the value of the related field this.Package.Deactivated.<br/><br/>
		/// 
		/// DataMember: true<br/></summary>
		public virtual System.Boolean PackageDeactivated
		{
			get { return this.Package==null ? (System.Boolean)TypeDefaultValue.GetDefaultValue(typeof(System.Boolean)) : this.Package.Deactivated; }
		}
 
		/// <summary> Gets the value of the related field this.Package.ServerId.<br/><br/>
		/// </summary>
		public virtual System.Int32 PackageServerId
		{
			get { return this.Package==null ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : this.Package.ServerId; }
		}
 
		/// <summary> Gets the value of the related field this.Package.UniqueId.<br/><br/>
		/// 
		/// DataMember: true<br/></summary>
		public virtual System.Guid PackageUniqueId
		{
			get { return this.Package==null ? (System.Guid)TypeDefaultValue.GetDefaultValue(typeof(System.Guid)) : this.Package.UniqueId; }
		}
	
		/// <summary> Gets the type of the hierarchy this entity is in. </summary>
		protected override InheritanceHierarchyType LLBLGenProIsInHierarchyOfType
		{
			get { return InheritanceHierarchyType.None;}
		}
		
		/// <summary> Gets or sets a value indicating whether this entity is a subtype</summary>
		protected override bool LLBLGenProIsSubType
		{
			get { return false;}
		}
		
		/// <summary>Returns the Vmgr.Data.Generic.EntityType enum value for this entity.</summary>
		[Browsable(false), XmlIgnore]
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)Vmgr.Data.Generic.EntityType.PluginEntity; }
		}

		#endregion


		#region Custom Entity code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Included code

		#endregion
	}
}
