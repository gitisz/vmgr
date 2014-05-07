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
	/// <summary>Entity class which represents the entity 'SecurityRolePermission'.<br/><br/></summary>
	[Serializable]
	public partial class SecurityRolePermissionEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private SecurityPermissionEntity _securityPermission;
		private SecurityRoleEntity _securityRole;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name SecurityPermission</summary>
			public static readonly string SecurityPermission = "SecurityPermission";
			/// <summary>Member name SecurityRole</summary>
			public static readonly string SecurityRole = "SecurityRole";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static SecurityRolePermissionEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public SecurityRolePermissionEntity():base("SecurityRolePermissionEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public SecurityRolePermissionEntity(IEntityFields2 fields):base("SecurityRolePermissionEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this SecurityRolePermissionEntity</param>
		public SecurityRolePermissionEntity(IValidator validator):base("SecurityRolePermissionEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="securityRolePermissionId">PK value for SecurityRolePermission which data should be fetched into this SecurityRolePermission object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public SecurityRolePermissionEntity(System.Int32 securityRolePermissionId):base("SecurityRolePermissionEntity")
		{
			InitClassEmpty(null, null);
			this.SecurityRolePermissionId = securityRolePermissionId;
		}

		/// <summary> CTor</summary>
		/// <param name="securityRolePermissionId">PK value for SecurityRolePermission which data should be fetched into this SecurityRolePermission object</param>
		/// <param name="validator">The custom validator object for this SecurityRolePermissionEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public SecurityRolePermissionEntity(System.Int32 securityRolePermissionId, IValidator validator):base("SecurityRolePermissionEntity")
		{
			InitClassEmpty(validator, null);
			this.SecurityRolePermissionId = securityRolePermissionId;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected SecurityRolePermissionEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_securityPermission = (SecurityPermissionEntity)info.GetValue("_securityPermission", typeof(SecurityPermissionEntity));
				if(_securityPermission!=null)
				{
					_securityPermission.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_securityRole = (SecurityRoleEntity)info.GetValue("_securityRole", typeof(SecurityRoleEntity));
				if(_securityRole!=null)
				{
					_securityRole.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((SecurityRolePermissionFieldIndex)fieldIndex)
			{
				case SecurityRolePermissionFieldIndex.SecurityPermissionId:
					DesetupSyncSecurityPermission(true, false);
					break;
				case SecurityRolePermissionFieldIndex.SecurityRoleId:
					DesetupSyncSecurityRole(true, false);
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
				case "SecurityPermission":
					this.SecurityPermission = (SecurityPermissionEntity)entity;
					break;
				case "SecurityRole":
					this.SecurityRole = (SecurityRoleEntity)entity;
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
				case "SecurityPermission":
					toReturn.Add(Relations.SecurityPermissionEntityUsingSecurityPermissionId);
					break;
				case "SecurityRole":
					toReturn.Add(Relations.SecurityRoleEntityUsingSecurityRoleId);
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
				case "SecurityPermission":
					SetupSyncSecurityPermission(relatedEntity);
					break;
				case "SecurityRole":
					SetupSyncSecurityRole(relatedEntity);
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
				case "SecurityPermission":
					DesetupSyncSecurityPermission(false, true);
					break;
				case "SecurityRole":
					DesetupSyncSecurityRole(false, true);
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
			if(_securityPermission!=null)
			{
				toReturn.Add(_securityPermission);
			}
			if(_securityRole!=null)
			{
				toReturn.Add(_securityRole);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
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
				info.AddValue("_securityPermission", (!this.MarkedForDeletion?_securityPermission:null));
				info.AddValue("_securityRole", (!this.MarkedForDeletion?_securityRole:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary> Method which will construct a filter (predicate expression) for the unique constraint defined on the fields:
		/// SecurityPermissionId , SecurityRoleId .</summary>
		/// <returns>true if succeeded and the contents is read, false otherwise</returns>
		public IPredicateExpression ConstructFilterForUCSecurityPermissionIdSecurityRoleId()
		{
			IPredicateExpression filter = new PredicateExpression();
			filter.Add(Vmgr.Data.Generic.HelperClasses.SecurityRolePermissionFields.SecurityPermissionId == this.Fields.GetCurrentValue((int)SecurityRolePermissionFieldIndex.SecurityPermissionId));
			filter.Add(Vmgr.Data.Generic.HelperClasses.SecurityRolePermissionFields.SecurityRoleId == this.Fields.GetCurrentValue((int)SecurityRolePermissionFieldIndex.SecurityRoleId));
 			return filter;
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new SecurityRolePermissionRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'SecurityPermission' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoSecurityPermission()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(SecurityPermissionFields.SecurityPermissionId, null, ComparisonOperator.Equal, this.SecurityPermissionId));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'SecurityRole' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoSecurityRole()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(SecurityRoleFields.SecurityRoleId, null, ComparisonOperator.Equal, this.SecurityRoleId));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(SecurityRolePermissionEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("SecurityPermission", _securityPermission);
			toReturn.Add("SecurityRole", _securityRole);
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
			Dictionary<string, string> fieldHashtable;
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Active", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreateUser", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SecurityPermissionId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SecurityRoleId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SecurityRolePermissionId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UpdateDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UpdateUser", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _securityPermission</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncSecurityPermission(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _securityPermission, new PropertyChangedEventHandler( OnSecurityPermissionPropertyChanged ), "SecurityPermission", Vmgr.Data.Generic.RelationClasses.StaticSecurityRolePermissionRelations.SecurityPermissionEntityUsingSecurityPermissionIdStatic, true, signalRelatedEntity, "SecurityRolePermissions", resetFKFields, new int[] { (int)SecurityRolePermissionFieldIndex.SecurityPermissionId } );
			_securityPermission = null;
		}

		/// <summary> setups the sync logic for member _securityPermission</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncSecurityPermission(IEntityCore relatedEntity)
		{
			if(_securityPermission!=relatedEntity)
			{
				DesetupSyncSecurityPermission(true, true);
				_securityPermission = (SecurityPermissionEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _securityPermission, new PropertyChangedEventHandler( OnSecurityPermissionPropertyChanged ), "SecurityPermission", Vmgr.Data.Generic.RelationClasses.StaticSecurityRolePermissionRelations.SecurityPermissionEntityUsingSecurityPermissionIdStatic, true, new string[] { "SecurityPermissionDescription", "SecurityPermissionName" } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSecurityPermissionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				case "Description":
					this.OnPropertyChanged("SecurityPermissionDescription");
					break;
				case "Name":
					this.OnPropertyChanged("SecurityPermissionName");
					break;
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _securityRole</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncSecurityRole(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _securityRole, new PropertyChangedEventHandler( OnSecurityRolePropertyChanged ), "SecurityRole", Vmgr.Data.Generic.RelationClasses.StaticSecurityRolePermissionRelations.SecurityRoleEntityUsingSecurityRoleIdStatic, true, signalRelatedEntity, "SecurityRolePermissions", resetFKFields, new int[] { (int)SecurityRolePermissionFieldIndex.SecurityRoleId } );
			_securityRole = null;
		}

		/// <summary> setups the sync logic for member _securityRole</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncSecurityRole(IEntityCore relatedEntity)
		{
			if(_securityRole!=relatedEntity)
			{
				DesetupSyncSecurityRole(true, true);
				_securityRole = (SecurityRoleEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _securityRole, new PropertyChangedEventHandler( OnSecurityRolePropertyChanged ), "SecurityRole", Vmgr.Data.Generic.RelationClasses.StaticSecurityRolePermissionRelations.SecurityRoleEntityUsingSecurityRoleIdStatic, true, new string[] { "SecurityRoleName" } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSecurityRolePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				case "Name":
					this.OnPropertyChanged("SecurityRoleName");
					break;
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this SecurityRolePermissionEntity</param>
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
		public  static SecurityRolePermissionRelations Relations
		{
			get	{ return new SecurityRolePermissionRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'SecurityPermission' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathSecurityPermission
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(SecurityPermissionEntityFactory))),	(IEntityRelation)GetRelationsForField("SecurityPermission")[0], (int)Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity, (int)Vmgr.Data.Generic.EntityType.SecurityPermissionEntity, 0, null, null, null, null, "SecurityPermission", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'SecurityRole' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathSecurityRole
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(SecurityRoleEntityFactory))),	(IEntityRelation)GetRelationsForField("SecurityRole")[0], (int)Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity, (int)Vmgr.Data.Generic.EntityType.SecurityRoleEntity, 0, null, null, null, null, "SecurityRole", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The Active property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."Active"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Active
		{
			get { return (System.Boolean)GetValue((int)SecurityRolePermissionFieldIndex.Active, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.Active, value); }
		}

		/// <summary> The CreateDate property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."CreateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreateDate
		{
			get { return (System.DateTime)GetValue((int)SecurityRolePermissionFieldIndex.CreateDate, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.CreateDate, value); }
		}

		/// <summary> The CreateUser property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."CreateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CreateUser
		{
			get { return (System.String)GetValue((int)SecurityRolePermissionFieldIndex.CreateUser, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.CreateUser, value); }
		}

		/// <summary> The SecurityPermissionId property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."SecurityPermissionId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SecurityPermissionId
		{
			get { return (System.Int32)GetValue((int)SecurityRolePermissionFieldIndex.SecurityPermissionId, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.SecurityPermissionId, value); }
		}

		/// <summary> The SecurityRoleId property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."SecurityRoleId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SecurityRoleId
		{
			get { return (System.Int32)GetValue((int)SecurityRolePermissionFieldIndex.SecurityRoleId, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.SecurityRoleId, value); }
		}

		/// <summary> The SecurityRolePermissionId property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."SecurityRolePermissionId"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int32 SecurityRolePermissionId
		{
			get { return (System.Int32)GetValue((int)SecurityRolePermissionFieldIndex.SecurityRolePermissionId, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.SecurityRolePermissionId, value); }
		}

		/// <summary> The UpdateDate property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."UpdateDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> UpdateDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)SecurityRolePermissionFieldIndex.UpdateDate, false); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.UpdateDate, value); }
		}

		/// <summary> The UpdateUser property of the Entity SecurityRolePermission<br/><br/></summary>
		/// <remarks>Mapped on  table field: "SecurityRolePermission"."UpdateUser"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpdateUser
		{
			get { return (System.String)GetValue((int)SecurityRolePermissionFieldIndex.UpdateUser, true); }
			set	{ SetValue((int)SecurityRolePermissionFieldIndex.UpdateUser, value); }
		}

		/// <summary> Gets / sets related entity of type 'SecurityPermissionEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual SecurityPermissionEntity SecurityPermission
		{
			get	{ return _securityPermission; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncSecurityPermission(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "SecurityRolePermissions", "SecurityPermission", _securityPermission, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'SecurityRoleEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(false)]
		public virtual SecurityRoleEntity SecurityRole
		{
			get	{ return _securityRole; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncSecurityRole(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "SecurityRolePermissions", "SecurityRole", _securityRole, true); 
				}
			}
		}
 
		/// <summary> Gets the value of the related field this.SecurityPermission.Description.<br/><br/>
		/// </summary>
		public virtual System.String SecurityPermissionDescription
		{
			get { return this.SecurityPermission==null ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : this.SecurityPermission.Description; }
		}
 
		/// <summary> Gets the value of the related field this.SecurityPermission.Name.<br/><br/>
		/// </summary>
		public virtual System.String SecurityPermissionName
		{
			get { return this.SecurityPermission==null ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : this.SecurityPermission.Name; }
		}
 
		/// <summary> Gets the value of the related field this.SecurityRole.Name.<br/><br/>
		/// </summary>
		public virtual System.String SecurityRoleName
		{
			get { return this.SecurityRole==null ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : this.SecurityRole.Name; }
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
			get { return (int)Vmgr.Data.Generic.EntityType.SecurityRolePermissionEntity; }
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
