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
	/// <summary>Implements the relations factory for the entity: Job. </summary>
	public partial class JobRelations
	{
		/// <summary>CTor</summary>
		public JobRelations()
		{
		}

		/// <summary>Gets all relations of the JobEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.TriggerEntityUsingJobId);
			toReturn.Add(this.ScheduleEntityUsingScheduleId);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between JobEntity and TriggerEntity over the 1:n relation they have, using the relation between the fields:
		/// Job.JobId - Trigger.JobId
		/// </summary>
		public virtual IEntityRelation TriggerEntityUsingJobId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Triggers" , true);
				relation.AddEntityFieldPair(JobFields.JobId, TriggerFields.JobId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("JobEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TriggerEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between JobEntity and ScheduleEntity over the m:1 relation they have, using the relation between the fields:
		/// Job.ScheduleId - Schedule.ScheduleId
		/// </summary>
		public virtual IEntityRelation ScheduleEntityUsingScheduleId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Schedule", false);
				relation.AddEntityFieldPair(ScheduleFields.ScheduleId, JobFields.ScheduleId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScheduleEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("JobEntity", true);
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
	internal static class StaticJobRelations
	{
		internal static readonly IEntityRelation TriggerEntityUsingJobIdStatic = new JobRelations().TriggerEntityUsingJobId;
		internal static readonly IEntityRelation ScheduleEntityUsingScheduleIdStatic = new JobRelations().ScheduleEntityUsingScheduleId;

		/// <summary>CTor</summary>
		static StaticJobRelations()
		{
		}
	}
}
