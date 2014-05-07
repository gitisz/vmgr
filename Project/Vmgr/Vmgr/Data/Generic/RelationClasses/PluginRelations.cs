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
	/// <summary>Implements the relations factory for the entity: Plugin. </summary>
	public partial class PluginRelations
	{
		/// <summary>CTor</summary>
		public PluginRelations()
		{
		}

		/// <summary>Gets all relations of the PluginEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ScheduleEntityUsingPluginId);
			toReturn.Add(this.PackageEntityUsingPackageId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between PluginEntity and ScheduleEntity over the 1:n relation they have, using the relation between the fields:
		/// Plugin.PluginId - Schedule.PluginId
		/// </summary>
		public virtual IEntityRelation ScheduleEntityUsingPluginId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Schedules" , true);
				relation.AddEntityFieldPair(PluginFields.PluginId, ScheduleFields.PluginId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PluginEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScheduleEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between PluginEntity and PackageEntity over the m:1 relation they have, using the relation between the fields:
		/// Plugin.PackageId - Package.PackageId
		/// </summary>
		public virtual IEntityRelation PackageEntityUsingPackageId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Package", false);
				relation.AddEntityFieldPair(PackageFields.PackageId, PluginFields.PackageId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PluginEntity", true);
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
	internal static class StaticPluginRelations
	{
		internal static readonly IEntityRelation ScheduleEntityUsingPluginIdStatic = new PluginRelations().ScheduleEntityUsingPluginId;
		internal static readonly IEntityRelation PackageEntityUsingPackageIdStatic = new PluginRelations().PackageEntityUsingPackageId;

		/// <summary>CTor</summary>
		static StaticPluginRelations()
		{
		}
	}
}
