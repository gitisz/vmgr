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
	/// <summary>Implements the relations factory for the entity: Trigger. </summary>
	public partial class TriggerRelations
	{
		/// <summary>CTor</summary>
		public TriggerRelations()
		{
		}

		/// <summary>Gets all relations of the TriggerEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.JobEntityUsingJobId);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between TriggerEntity and JobEntity over the m:1 relation they have, using the relation between the fields:
		/// Trigger.JobId - Job.JobId
		/// </summary>
		public virtual IEntityRelation JobEntityUsingJobId
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Job", false);
				relation.AddEntityFieldPair(JobFields.JobId, TriggerFields.JobId);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("JobEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TriggerEntity", true);
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
	internal static class StaticTriggerRelations
	{
		internal static readonly IEntityRelation JobEntityUsingJobIdStatic = new TriggerRelations().JobEntityUsingJobId;

		/// <summary>CTor</summary>
		static StaticTriggerRelations()
		{
		}
	}
}
