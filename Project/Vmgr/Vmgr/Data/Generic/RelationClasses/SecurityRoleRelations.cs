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
	/// <summary>Implements the relations factory for the entity: SecurityRole. </summary>
	public partial class SecurityRoleRelations
	{
		/// <summary>CTor</summary>
		public SecurityRoleRelations()
		{
		}

		/// <summary>Gets all relations of the SecurityRoleEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.SecurityMembershipEntityUsingSecurityRoleId);
			toReturn.Add(this.SecurityRolePermissionEntityUsingSecurityRoleId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between SecurityRoleEntity and SecurityMembershipEntity over the 1:n relation they have, using the relation between the fields:
		/// SecurityRole.SecurityRoleId - SecurityMembership.SecurityRoleId
		/// </summary>
		public virtual IEntityRelation SecurityMembershipEntityUsingSecurityRoleId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SecurityMemberships" , true);
				relation.AddEntityFieldPair(SecurityRoleFields.SecurityRoleId, SecurityMembershipFields.SecurityRoleId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRoleEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityMembershipEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between SecurityRoleEntity and SecurityRolePermissionEntity over the 1:n relation they have, using the relation between the fields:
		/// SecurityRole.SecurityRoleId - SecurityRolePermission.SecurityRoleId
		/// </summary>
		public virtual IEntityRelation SecurityRolePermissionEntityUsingSecurityRoleId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SecurityRolePermissions" , true);
				relation.AddEntityFieldPair(SecurityRoleFields.SecurityRoleId, SecurityRolePermissionFields.SecurityRoleId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRoleEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRolePermissionEntity", false);
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
	internal static class StaticSecurityRoleRelations
	{
		internal static readonly IEntityRelation SecurityMembershipEntityUsingSecurityRoleIdStatic = new SecurityRoleRelations().SecurityMembershipEntityUsingSecurityRoleId;
		internal static readonly IEntityRelation SecurityRolePermissionEntityUsingSecurityRoleIdStatic = new SecurityRoleRelations().SecurityRolePermissionEntityUsingSecurityRoleId;

		/// <summary>CTor</summary>
		static StaticSecurityRoleRelations()
		{
		}
	}
}
