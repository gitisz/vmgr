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
	/// <summary>Entity class which represents the entity 'Package'.<br/><br/>
	/// DataContract: true<br/></summary>
	[Serializable]
	public partial class PackageEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<MonitorEntity> _monitors;
		private EntityCollection<PluginEntity> _plugins;
		private ServerEntity _server;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Server</summary>
			public static readonly string Server = "Server";
			/// <summary>Member name Monitors</summary>
			public static readonly string Monitors = "Monitors";
			/// <summary>Member name Plugins</summary>
			public static readonly string Plugins = "Plugins";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static PackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public PackageEntity():base("PackageEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public PackageEntity(IEntityFields2 fields):base("PackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this PackageEntity</param>
		public PackageEntity(IValidator validator):base("PackageEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="packageId">PK value for Package which data should be fetched into this Package object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PackageEntity(System.Int32 packageId):base("PackageEntity")
		{
			InitClassEmpty(null, null);
			this.PackageId = packageId;
		}

		/// <summary> CTor</summary>
		/// <param name="packageId">PK value for Package which data should be fetched into this Package object</param>
		/// <param name="validator">The custom validator object for this PackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PackageEntity(System.Int32 packageId, IValidator validator):base("PackageEntity")
		{
			InitClassEmpty(validator, null);
			this.PackageId = packageId;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected PackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_monitors = (EntityCollection<MonitorEntity>)info.GetValue("_monitors", typeof(EntityCollection<MonitorEntity>));
				_plugins = (EntityCollection<PluginEntity>)info.GetValue("_plugins", typeof(EntityCollection<PluginEntity>));
				_server = (ServerEntity)info.GetValue("_server", typeof(ServerEntity));
				if(_server!=null)
				{
					_server.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((PackageFieldIndex)fieldIndex)
			{
				case PackageFieldIndex.ServerId:
					DesetupSyncServer(true, false);
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
				case "Server":
					this.Server = (ServerEntity)entity;
					break;
				case "Monitors":
					this.Monitors.Add((MonitorEntity)entity);
					break;
				case "Plugins":
					this.Plugins.Add((PluginEntity)entity);
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
				case "Server":
					toReturn.Add(Relations.ServerEntityUsingServerId);
					break;
				case "Monitors":
					toReturn.Add(Relations.MonitorEntityUsingPackageId);
					break;
				case "Plugins":
					toReturn.Add(Relations.PluginEntityUsingPackageId);
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
				case "Server":
					SetupSyncServer(relatedEntity);
					break;
				case "Monitors":
					this.Monitors.Add((MonitorEntity)relatedEntity);
					break;
				case "Plugins":
					this.Plugins.Add((PluginEntity)relatedEntity);
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
				case "Server":
					DesetupSyncServer(false, true);
					break;
				case "Monitors":
					this.PerformRelatedEntityRemoval(this.Monitors, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "Plugins":
					this.PerformRelatedEntityRemoval(this.Plugins, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_server!=null)
			{
				toReturn.Add(_server);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Monitors);
			toReturn.Add(this.Plugins);
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
				info.AddValue("_monitors", ((_monitors!=null) && (_monitors.Count>0) && !this.MarkedForDeletion)?_monitors:null);
				info.AddValue("_plugins", ((_plugins!=null) && (_plugins.Count>0) && !this.MarkedForDeletion)?_plugins:null);
				info.AddValue("_server", (!this.MarkedForDeletion?_server:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new PackageRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Monitor' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoMonitors()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(MonitorFields.PackageId, null, ComparisonOperator.Equal, this.PackageId));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Plugin' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPlugins()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PluginFields.PackageId, null, ComparisonOperator.Equal, this.PackageId));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Server' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoServer()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ServerFields.ServerId, null, ComparisonOperator.Equal, this.ServerId));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(PackageEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._monitors);
			collectionsQueue.Enqueue(this._plugins);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._monitors = (EntityCollection<MonitorEntity>) collectionsQueue.Dequeue();
			this._plugins = (EntityCollection<PluginEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._monitors != null);
			toReturn |=(this._plugins != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<MonitorEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MonitorEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<PluginEntity>(EntityFactoryCache2.GetEntityFactory(typeof(PluginEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Server", _server);
			toReturn.Add("Monitors", _monitors);
			toReturn.Add("Plugins", _plugins);
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
			_fieldsCustomProperties.Add("Deactivated", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("Package", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			fieldHashtable.Add("DataMember", @"true");
			_fieldsCustomProperties.Add("PackageId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ServerId", fieldHashtable);
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

		/// <summary> Removes the sync logic for member _server</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncServer(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _server, new PropertyChangedEventHandler( OnServerPropertyChanged ), "Server", Vmgr.Data.Generic.RelationClasses.StaticPackageRelations.ServerEntityUsingServerIdStatic, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)PackageFieldIndex.ServerId } );
			_server = null;
		}

		/// <summary> setups the sync logic for member _server</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncServer(IEntityCore relatedEntity)
		{
			if(_server!=relatedEntity)
			{
				DesetupSyncServer(true, true);
				_server = (ServerEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _server, new PropertyChangedEventHandler( OnServerPropertyChanged ), "Server", Vmgr.Data.Generic.RelationClasses.StaticPackageRelations.ServerEntityUsingServerIdStatic, true, new string[] { "ServerUniqueId" } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnServerPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				case "UniqueId":
					this.OnPropertyChanged("ServerUniqueId");
					break;
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this PackageEntity</param>
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
		public  static PackageRelations Relations
		{
			get	{ return new PackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Monitor' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathMonitors
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<MonitorEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MonitorEntityFactory))), (IEntityRelation)GetRelationsForField("Monitors")[0], (int)Vmgr.Data.Generic.EntityType.PackageEntity, (int)Vmgr.Data.Generic.EntityType.MonitorEntity, 0, null, null, null, null, "Monitors", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Plugin' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPlugins
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<PluginEntity>(EntityFactoryCache2.GetEntityFactory(typeof(PluginEntityFactory))), (IEntityRelation)GetRelationsForField("Plugins")[0], (int)Vmgr.Data.Generic.EntityType.PackageEntity, (int)Vmgr.Data.Generic.EntityType.PluginEntity, 0, null, null, null, null, "Plugins", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Server' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathServer
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ServerEntityFactory))),	(IEntityRelation)GetRelationsForField("Server")[0], (int)Vmgr.Data.Generic.EntityType.PackageEntity, (int)Vmgr.Data.Generic.EntityType.ServerEntity, 0, null, null, null, null, "Server", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The CreateDate property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."CreateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreateDate
		{
			get { return (System.DateTime)GetValue((int)PackageFieldIndex.CreateDate, true); }
			set	{ SetValue((int)PackageFieldIndex.CreateDate, value); }
		}

		/// <summary> The CreateUser property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."CreateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CreateUser
		{
			get { return (System.String)GetValue((int)PackageFieldIndex.CreateUser, true); }
			set	{ SetValue((int)PackageFieldIndex.CreateUser, value); }
		}

		/// <summary> The Deactivated property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."Deactivated"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Deactivated
		{
			get { return (System.Boolean)GetValue((int)PackageFieldIndex.Deactivated, true); }
			set	{ SetValue((int)PackageFieldIndex.Deactivated, value); }
		}

		/// <summary> The Description property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)PackageFieldIndex.Description, true); }
			set	{ SetValue((int)PackageFieldIndex.Description, value); }
		}

		/// <summary> The Name property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)PackageFieldIndex.Name, true); }
			set	{ SetValue((int)PackageFieldIndex.Name, value); }
		}

		/// <summary> The Package property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."Package"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.Byte[] Package
		{
			get { return (System.Byte[])GetValue((int)PackageFieldIndex.Package, true); }
			set	{ SetValue((int)PackageFieldIndex.Package, value); }
		}

		/// <summary> The PackageId property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."PackageId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 PackageId
		{
			get { return (System.Int32)GetValue((int)PackageFieldIndex.PackageId, true); }
			set	{ SetValue((int)PackageFieldIndex.PackageId, value); }
		}

		/// <summary> The ServerId property of the Entity Package<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Package"."ServerId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ServerId
		{
			get { return (System.Int32)GetValue((int)PackageFieldIndex.ServerId, true); }
			set	{ SetValue((int)PackageFieldIndex.ServerId, value); }
		}

		/// <summary> The UniqueId property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."UniqueId"<br/>
		/// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Guid UniqueId
		{
			get { return (System.Guid)GetValue((int)PackageFieldIndex.UniqueId, true); }
			set	{ SetValue((int)PackageFieldIndex.UniqueId, value); }
		}

		/// <summary> The UpdateDate property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."UpdateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> UpdateDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)PackageFieldIndex.UpdateDate, false); }
			set	{ SetValue((int)PackageFieldIndex.UpdateDate, value); }
		}

		/// <summary> The UpdateUser property of the Entity Package<br/><br/>
		/// DataMember: true<br/></summary>
		/// <remarks>Mapped on  table field: "Package"."UpdateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpdateUser
		{
			get { return (System.String)GetValue((int)PackageFieldIndex.UpdateUser, true); }
			set	{ SetValue((int)PackageFieldIndex.UpdateUser, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'MonitorEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(MonitorEntity))]
		public virtual EntityCollection<MonitorEntity> Monitors
		{
			get { return GetOrCreateEntityCollection<MonitorEntity, MonitorEntityFactory>("Package", true, false, ref _monitors);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'PluginEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(PluginEntity))]
		public virtual EntityCollection<PluginEntity> Plugins
		{
			get { return GetOrCreateEntityCollection<PluginEntity, PluginEntityFactory>("Package", true, false, ref _plugins);	}
		}

		/// <summary> Gets / sets related entity of type 'ServerEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual ServerEntity Server
		{
			get	{ return _server; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncServer(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Packages", "Server", _server, true); 
				}
			}
		}
 
		/// <summary> Gets the value of the related field this.Server.UniqueId.<br/><br/>
		/// </summary>
		public virtual System.Guid ServerUniqueId
		{
			get { return this.Server==null ? (System.Guid)TypeDefaultValue.GetDefaultValue(typeof(System.Guid)) : this.Server.UniqueId; }
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
			get { return (int)Vmgr.Data.Generic.EntityType.PackageEntity; }
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
