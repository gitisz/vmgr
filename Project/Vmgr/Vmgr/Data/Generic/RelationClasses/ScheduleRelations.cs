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
	/// <summary>Implements the relations factory for the entity: Schedule. </summary>
	public partial class ScheduleRelations
	{
		/// <summary>CTor</summary>
		public ScheduleRelations()
		{
		}

		/// <summary>Gets all relations of the ScheduleEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.JobEntityUsingScheduleId);
			toReturn.Add(this.PluginEntityUsingPluginId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ScheduleEntity and JobEntity over the 1:n relation they have, using the relation between the fields:
		/// Schedule.ScheduleId - Job.ScheduleId
		/// </summary>
		public virtual IEntityRelation JobEntityUsingScheduleId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Jobs" , true);
				relation.AddEntityFieldPair(ScheduleFields.ScheduleId, JobFields.ScheduleId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScheduleEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("JobEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between ScheduleEntity and PluginEntity over the m:1 relation they have, using the relation between the fields:
		/// Schedule.PluginId - Plugin.PluginId
		/// </summary>
		public virtual IEntityRelation PluginEntityUsingPluginId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Plugin", false);
				relation.AddEntityFieldPair(PluginFields.PluginId, ScheduleFields.PluginId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PluginEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScheduleEntity", true);
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
	internal static class StaticScheduleRelations
	{
		internal static readonly IEntityRelation JobEntityUsingScheduleIdStatic = new ScheduleRelations().JobEntityUsingScheduleId;
		internal static readonly IEntityRelation PluginEntityUsingPluginIdStatic = new ScheduleRelations().PluginEntityUsingPluginId;

		/// <summary>CTor</summary>
		static StaticScheduleRelations()
		{
		}
	}
}
