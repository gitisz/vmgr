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
	/// <summary>Implements the relations factory for the entity: SecurityRolePermission. </summary>
	public partial class SecurityRolePermissionRelations
	{
		/// <summary>CTor</summary>
		public SecurityRolePermissionRelations()
		{
		}

		/// <summary>Gets all relations of the SecurityRolePermissionEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.SecurityPermissionEntityUsingSecurityPermissionId);
			toReturn.Add(this.SecurityRoleEntityUsingSecurityRoleId);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between SecurityRolePermissionEntity and SecurityPermissionEntity over the m:1 relation they have, using the relation between the fields:
		/// SecurityRolePermission.SecurityPermissionId - SecurityPermission.SecurityPermissionId
		/// </summary>
		public virtual IEntityRelation SecurityPermissionEntityUsingSecurityPermissionId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "SecurityPermission", false);
				relation.AddEntityFieldPair(SecurityPermissionFields.SecurityPermissionId, SecurityRolePermissionFields.SecurityPermissionId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityPermissionEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRolePermissionEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between SecurityRolePermissionEntity and SecurityRoleEntity over the m:1 relation they have, using the relation between the fields:
		/// SecurityRolePermission.SecurityRoleId - SecurityRole.SecurityRoleId
		/// </summary>
		public virtual IEntityRelation SecurityRoleEntityUsingSecurityRoleId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "SecurityRole", false);
				relation.AddEntityFieldPair(SecurityRoleFields.SecurityRoleId, SecurityRolePermissionFields.SecurityRoleId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRoleEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityRolePermissionEntity", true);
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
	internal static class StaticSecurityRolePermissionRelations
	{
		internal static readonly IEntityRelation SecurityPermissionEntityUsingSecurityPermissionIdStatic = new SecurityRolePermissionRelations().SecurityPermissionEntityUsingSecurityPermissionId;
		internal static readonly IEntityRelation SecurityRoleEntityUsingSecurityRoleIdStatic = new SecurityRolePermissionRelations().SecurityRoleEntityUsingSecurityRoleId;

		/// <summary>CTor</summary>
		static StaticSecurityRolePermissionRelations()
		{
		}
	}
}
