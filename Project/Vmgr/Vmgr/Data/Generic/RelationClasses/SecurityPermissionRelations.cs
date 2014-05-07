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
using System.Collections;
using System.Collections.Generic;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Generic.RelationClasses
{
	/// <summary>Implements the relations factory for the entity: SecurityPermission. </summary>
	public partial class SecurityPermissionRelations
	{
		/// <summary>CTor</summary>
		public SecurityPermissionRelations()
		{
		}

		/// <summary>Gets all relations of the SecurityPermissionEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.SecurityRolePermissionEntityUsingSecurityPermissionId);
			toReturn.Add(this.SecuritySiteMapEntityUsingSecurityPermissionId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between SecurityPermissionEntity and SecurityRolePermissionEntity over the 1:n relation they have, using the relation between the fields:
		/// SecurityPermission.SecurityPermissionId - SecurityRolePermission.SecurityPermissionId
		/// </summary>
		public virtual IEntityRelation SecurityRolePermissionEntityUsingSecurityPermissionId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SecurityRolePermissions" , true);
				relation.AddEntityFieldPair(SecurityPermissionFields.SecurityPermissionId, SecurityRolePermissionFields.SecurityPermissionId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityPermissionEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRolePermissionEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between SecurityPermissionEntity and SecuritySiteMapEntity over the 1:n relation they have, using the relation between the fields:
		/// SecurityPermission.SecurityPermissionId - SecuritySiteMap.SecurityPermissionId
		/// </summary>
		public virtual IEntityRelation SecuritySiteMapEntityUsingSecurityPermissionId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SecuritySiteMaps" , true);
				relation.AddEntityFieldPair(SecurityPermissionFields.SecurityPermissionId, SecuritySiteMapFields.SecurityPermissionId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityPermissionEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecuritySiteMapEntity", false);
				return relation;
			}
		}


		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSubTypeRelation(string subTypeEntityName) { return null; }
		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSuperTypeRelation() { return null;}
		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticSecurityPermissionRelations
	{
		internal static readonly IEntityRelation SecurityRolePermissionEntityUsingSecurityPermissionIdStatic = new SecurityPermissionRelations().SecurityRolePermissionEntityUsingSecurityPermissionId;
		internal static readonly IEntityRelation SecuritySiteMapEntityUsingSecurityPermissionIdStatic = new SecurityPermissionRelations().SecuritySiteMapEntityUsingSecurityPermissionId;

		/// <summary>CTor</summary>
		static StaticSecurityPermissionRelations()
		{
		}
	}
}
