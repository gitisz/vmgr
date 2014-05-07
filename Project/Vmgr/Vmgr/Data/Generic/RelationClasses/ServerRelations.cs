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
	/// <summary>Implements the relations factory for the entity: Server. </summary>
	public partial class ServerRelations
	{
		/// <summary>CTor</summary>
		public ServerRelations()
		{
		}

		/// <summary>Gets all relations of the ServerEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterEntityUsingServerId);
			toReturn.Add(this.PackageEntityUsingServerId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ServerEntity and FilterEntity over the 1:n relation they have, using the relation between the fields:
		/// Server.ServerId - Filter.ServerId
		/// </summary>
		public virtual IEntityRelation FilterEntityUsingServerId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Filters" , true);
				relation.AddEntityFieldPair(ServerFields.ServerId, FilterFields.ServerId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ServerEntity and PackageEntity over the 1:n relation they have, using the relation between the fields:
		/// Server.ServerId - Package.ServerId
		/// </summary>
		public virtual IEntityRelation PackageEntityUsingServerId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(ServerFields.ServerId, PackageFields.ServerId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageEntity", false);
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
	internal static class StaticServerRelations
	{
		internal static readonly IEntityRelation FilterEntityUsingServerIdStatic = new ServerRelations().FilterEntityUsingServerId;
		internal static readonly IEntityRelation PackageEntityUsingServerIdStatic = new ServerRelations().PackageEntityUsingServerId;

		/// <summary>CTor</summary>
		static StaticServerRelations()
		{
		}
	}
}
