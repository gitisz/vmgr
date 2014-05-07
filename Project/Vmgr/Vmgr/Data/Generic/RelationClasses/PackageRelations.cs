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
	/// <summary>Implements the relations factory for the entity: Package. </summary>
	public partial class PackageRelations
	{
		/// <summary>CTor</summary>
		public PackageRelations()
		{
		}

		/// <summary>Gets all relations of the PackageEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.MonitorEntityUsingPackageId);
			toReturn.Add(this.PluginEntityUsingPackageId);
			toReturn.Add(this.ServerEntityUsingServerId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between PackageEntity and MonitorEntity over the 1:n relation they have, using the relation between the fields:
		/// Package.PackageId - Monitor.PackageId
		/// </summary>
		public virtual IEntityRelation MonitorEntityUsingPackageId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Monitors" , true);
				relation.AddEntityFieldPair(PackageFields.PackageId, MonitorFields.PackageId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MonitorEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between PackageEntity and PluginEntity over the 1:n relation they have, using the relation between the fields:
		/// Package.PackageId - Plugin.PackageId
		/// </summary>
		public virtual IEntityRelation PluginEntityUsingPackageId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Plugins" , true);
				relation.AddEntityFieldPair(PackageFields.PackageId, PluginFields.PackageId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PluginEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between PackageEntity and ServerEntity over the m:1 relation they have, using the relation between the fields:
		/// Package.ServerId - Server.ServerId
		/// </summary>
		public virtual IEntityRelation ServerEntityUsingServerId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Server", false);
				relation.AddEntityFieldPair(ServerFields.ServerId, PackageFields.ServerId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageEntity", true);
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
	internal static class StaticPackageRelations
	{
		internal static readonly IEntityRelation MonitorEntityUsingPackageIdStatic = new PackageRelations().MonitorEntityUsingPackageId;
		internal static readonly IEntityRelation PluginEntityUsingPackageIdStatic = new PackageRelations().PluginEntityUsingPackageId;
		internal static readonly IEntityRelation ServerEntityUsingServerIdStatic = new PackageRelations().ServerEntityUsingServerId;

		/// <summary>CTor</summary>
		static StaticPackageRelations()
		{
		}
	}
}
