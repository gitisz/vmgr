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
	/// <summary>Implements the relations factory for the entity: SecuritySiteMap. </summary>
	public partial class SecuritySiteMapRelations
	{
		/// <summary>CTor</summary>
		public SecuritySiteMapRelations()
		{
		}

		/// <summary>Gets all relations of the SecuritySiteMapEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.SecurityPermissionEntityUsingSecurityPermissionId);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between SecuritySiteMapEntity and SecurityPermissionEntity over the m:1 relation they have, using the relation between the fields:
		/// SecuritySiteMap.SecurityPermissionId - SecurityPermission.SecurityPermissionId
		/// </summary>
		public virtual IEntityRelation SecurityPermissionEntityUsingSecurityPermissionId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "SecurityPermission", false);
				relation.AddEntityFieldPair(SecurityPermissionFields.SecurityPermissionId, SecuritySiteMapFields.SecurityPermissionId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecurityPermissionEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SecuritySiteMapEntity", true);
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
	internal static class StaticSecuritySiteMapRelations
	{
		internal static readonly IEntityRelation SecurityPermissionEntityUsingSecurityPermissionIdStatic = new SecuritySiteMapRelations().SecurityPermissionEntityUsingSecurityPermissionId;

		/// <summary>CTor</summary>
		static StaticSecuritySiteMapRelations()
		{
		}
	}
}
