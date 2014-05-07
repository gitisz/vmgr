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
	/// <summary>Entity class which represents the entity 'Schedule'.<br/><br/>
	/// DataContract: true<br/></summary>
	[Serializable]
	public partial class ScheduleEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<JobEntity> _jobs;
		private PluginEntity _plugin;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Plugin</summary>
			public static readonly string Plugin = "Plugin";
			/// <summary>Member name Jobs</summary>
			public static readonly string Jobs = "Jobs";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ScheduleEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ScheduleEntity():base("ScheduleEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ScheduleEntity(IEntityFields2 fields):base("ScheduleEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ScheduleEntity</param>
		public ScheduleEntity(IValidator validator):base("ScheduleEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="scheduleId">PK value for Schedule which data should be fetched into this Schedule object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScheduleEntity(System.Int32 scheduleId):base("ScheduleEntity")
		{
			InitClassEmpty(null, null);
			this.ScheduleId = scheduleId;
		}

		/// <summary> CTor</summary>
		/// <param name="scheduleId">PK value for Schedule which data should be fetched into this Schedule object</param>
		/// <param name="validator">The custom validator object for this ScheduleEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScheduleEntity(System.Int32 scheduleId, IValidator validator):base("ScheduleEntity")
		{
			InitClassEmpty(validator, null);
			this.ScheduleId = scheduleId;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ScheduleEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_jobs = (EntityCollection<JobEntity>)info.GetValue("_jobs", typeof(EntityCollection<JobEntity>));
				_plugin = (PluginEntity)info.GetValue("_plugin", typeof(PluginEntity));
				if(_plugin!=null)
				{
					_plugin.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((ScheduleFieldIndex)fieldIndex)
			{
				case ScheduleFieldIndex.PluginId:
					DesetupSyncPlugin(true, false);
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
				case "Plugin":
					this.Plugin = (PluginEntity)entity;
					break;
				case "Jobs":
					this.Jobs.Add((JobEntity)entity);
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
				case "Plugin":
					toReturn.Add(Relations.PluginEntityUsingPluginId);
					break;
				case "Jobs":
					toReturn.Add(Relations.JobEntityUsingScheduleId);
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
				case "Plugin":
					SetupSyncPlugin(relatedEntity);
					break;
				case "Jobs":
					this.Jobs.Add((JobEntity)relatedEntity);
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
				case "Plugin":
					DesetupSyncPlugin(false, true);
					break;
				case "Jobs":
					this.PerformRelatedEntityRemoval(this.Jobs, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_plugin!=null)
			{
				toReturn.Add(_plugin);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Jobs);
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
				info.AddValue("_jobs", ((_jobs!=null) && (_jobs.Count>0) && !this.MarkedForDeletion)?_jobs:null);
				info.AddValue("_plugin", (!this.MarkedForDeletion?_plugin:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary> Method which will construct a filter (predicate expression) for the unique constraint defined on the fields:
		/// Name , UniqueId .</summary>
		/// <returns>true if succeeded and the contents is read, false otherwise</returns>
		public IPredicateExpression ConstructFilterForUCNameUniqueId()
		{
			IPredicateExpression filter = new PredicateExpression();
			filter.Add(Vmgr.Data.Generic.HelperClasses.ScheduleFields.Name == this.Fields.GetCurrentValue((int)ScheduleFieldIndex.Name));
			filter.Add(Vmgr.Data.Generic.HelperClasses.ScheduleFields.UniqueId == this.Fields.GetCurrentValue((int)ScheduleFieldIndex.UniqueId));
 			return filter;
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ScheduleRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Job' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoJobs()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(JobFields.ScheduleId, null, ComparisonOperator.Equal, this.ScheduleId));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Plugin' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPlugin()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PluginFields.PluginId, null, ComparisonOperator.Equal, this.PluginId));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ScheduleEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._jobs);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._jobs = (EntityCollection<JobEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._jobs != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<JobEntity>(EntityFactoryCache2.GetEntityFactory(typeof(JobEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Plugin", _plugin);
			toReturn.Add("Jobs", _jobs);
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
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("CreateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("CreateUser", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("Deactivated", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("End", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Exclusions", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("MisfireInstruction", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("PluginId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("RecurrenceRule", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("RecurrenceTypeId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("ScheduleId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("Start", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("UniqueId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("UpdateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"True");
			_fieldsCustomProperties.Add("UpdateUser", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _plugin</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncPlugin(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _plugin, new PropertyChangedEventHandler( OnPluginPropertyChanged ), "Plugin", Vmgr.Data.Generic.RelationClasses.StaticScheduleRelations.PluginEntityUsingPluginIdStatic, true, signalRelatedEntity, "Schedules", resetFKFields, new int[] { (int)ScheduleFieldIndex.PluginId } );
			_plugin = null;
		}

		/// <summary> setups the sync logic for member _plugin</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncPlugin(IEntityCore relatedEntity)
		{
			if(_plugin!=relatedEntity)
			{
				DesetupSyncPlugin(true, true);
				_plugin = (PluginEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _plugin, new PropertyChangedEventHandler( OnPluginPropertyChanged ), "Plugin", Vmgr.Data.Generic.RelationClasses.StaticScheduleRelations.PluginEntityUsingPluginIdStatic, true, new string[] { "PluginUniqueId" } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPluginPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				case "UniqueId":
					this.OnPropertyChanged("PluginUniqueId");
					break;
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ScheduleEntity</param>
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
		public  static ScheduleRelations Relations
		{
			get	{ return new ScheduleRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Job' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathJobs
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<JobEntity>(EntityFactoryCache2.GetEntityFactory(typeof(JobEntityFactory))), (IEntityRelation)GetRelationsForField("Jobs")[0], (int)Vmgr.Data.Generic.EntityType.ScheduleEntity, (int)Vmgr.Data.Generic.EntityType.JobEntity, 0, null, null, null, null, "Jobs", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Plugin' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPlugin
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(PluginEntityFactory))),	(IEntityRelation)GetRelationsForField("Plugin")[0], (int)Vmgr.Data.Generic.EntityType.ScheduleEntity, (int)Vmgr.Data.Generic.EntityType.PluginEntity, 0, null, null, null, null, "Plugin", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The CreateDate property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."CreateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreateDate
		{
			get { return (System.DateTime)GetValue((int)ScheduleFieldIndex.CreateDate, true); }
			set	{ SetValue((int)ScheduleFieldIndex.CreateDate, value); }
		}

		/// <summary> The CreateUser property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."CreateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CreateUser
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.CreateUser, true); }
			set	{ SetValue((int)ScheduleFieldIndex.CreateUser, value); }
		}

		/// <summary> The Deactivated property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."Deactivated"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Deactivated
		{
			get { return (System.Boolean)GetValue((int)ScheduleFieldIndex.Deactivated, true); }
			set	{ SetValue((int)ScheduleFieldIndex.Deactivated, value); }
		}

		/// <summary> The Description property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.Description, true); }
			set	{ SetValue((int)ScheduleFieldIndex.Description, value); }
		}

		/// <summary> The End property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."End"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> End
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ScheduleFieldIndex.End, false); }
			set	{ SetValue((int)ScheduleFieldIndex.End, value); }
		}

		/// <summary> The Exclusions property of the Entity Schedule<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."Exclusions"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Exclusions
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.Exclusions, true); }
			set	{ SetValue((int)ScheduleFieldIndex.Exclusions, value); }
		}

		/// <summary> The MisfireInstruction property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."MisfireInstruction"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 MisfireInstruction
		{
			get { return (System.Int32)GetValue((int)ScheduleFieldIndex.MisfireInstruction, true); }
			set	{ SetValue((int)ScheduleFieldIndex.MisfireInstruction, value); }
		}

		/// <summary> The Name property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.Name, true); }
			set	{ SetValue((int)ScheduleFieldIndex.Name, value); }
		}

		/// <summary> The PluginId property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."PluginId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> PluginId
		{
			get { return (Nullable<System.Int32>)GetValue((int)ScheduleFieldIndex.PluginId, false); }
			set	{ SetValue((int)ScheduleFieldIndex.PluginId, value); }
		}

		/// <summary> The RecurrenceRule property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."RecurrenceRule"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String RecurrenceRule
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.RecurrenceRule, true); }
			set	{ SetValue((int)ScheduleFieldIndex.RecurrenceRule, value); }
		}

		/// <summary> The RecurrenceTypeId property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."RecurrenceTypeId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RecurrenceTypeId
		{
			get { return (System.Int32)GetValue((int)ScheduleFieldIndex.RecurrenceTypeId, true); }
			set	{ SetValue((int)ScheduleFieldIndex.RecurrenceTypeId, value); }
		}

		/// <summary> The ScheduleId property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."ScheduleId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 ScheduleId
		{
			get { return (System.Int32)GetValue((int)ScheduleFieldIndex.ScheduleId, true); }
			set	{ SetValue((int)ScheduleFieldIndex.ScheduleId, value); }
		}

		/// <summary> The Start property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."Start"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime Start
		{
			get { return (System.DateTime)GetValue((int)ScheduleFieldIndex.Start, true); }
			set	{ SetValue((int)ScheduleFieldIndex.Start, value); }
		}

		/// <summary> The UniqueId property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."UniqueId"<br/>
		/// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Guid UniqueId
		{
			get { return (System.Guid)GetValue((int)ScheduleFieldIndex.UniqueId, true); }
			set	{ SetValue((int)ScheduleFieldIndex.UniqueId, value); }
		}

		/// <summary> The UpdateDate property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."UpdateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> UpdateDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ScheduleFieldIndex.UpdateDate, false); }
			set	{ SetValue((int)ScheduleFieldIndex.UpdateDate, value); }
		}

		/// <summary> The UpdateUser property of the Entity Schedule<br/><br/>
		/// DataMember: True<br/></summary>
		/// <remarks>Mapped on  table field: "Schedule"."UpdateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpdateUser
		{
			get { return (System.String)GetValue((int)ScheduleFieldIndex.UpdateUser, true); }
			set	{ SetValue((int)ScheduleFieldIndex.UpdateUser, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'JobEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(JobEntity))]
		public virtual EntityCollection<JobEntity> Jobs
		{
			get { return GetOrCreateEntityCollection<JobEntity, JobEntityFactory>("Schedule", true, false, ref _jobs);	}
		}

		/// <summary> Gets / sets related entity of type 'PluginEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual PluginEntity Plugin
		{
			get	{ return _plugin; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncPlugin(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Schedules", "Plugin", _plugin, true); 
				}
			}
		}
 
		/// <summary> Gets the value of the related field this.Plugin.UniqueId.<br/><br/>
		/// 
		/// DataMember: true<br/></summary>
		public virtual System.Guid PluginUniqueId
		{
			get { return this.Plugin==null ? (System.Guid)TypeDefaultValue.GetDefaultValue(typeof(System.Guid)) : this.Plugin.UniqueId; }
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
			get { return (int)Vmgr.Data.Generic.EntityType.ScheduleEntity; }
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
