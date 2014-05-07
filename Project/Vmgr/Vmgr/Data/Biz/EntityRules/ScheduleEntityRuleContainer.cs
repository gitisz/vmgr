using System;
using System.Linq;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.Linq;
using Vmgr.Data.SqlServer;
/*
* THIS CODE WAS AUTO-GENERATED.  DO NOT MAKE MODIFICATIONS.
*/
using Vmgr.Data.Biz.Validation;

namespace Vmgr.Data.Biz.EntityRules
{
	public partial class ScheduleEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(ScheduleEntity scheduleEntity)
        {
			return ScheduleEntityRuleContainer.GetGeneratedRules(scheduleEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(ScheduleEntity scheduleEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
			list.Add(new NameUniqueIdRule(scheduleEntity, validationGroup));
				
			list.Add(new CreateUserMaxLengthRule(scheduleEntity, validationGroup));
			list.Add(new DescriptionMaxLengthRule(scheduleEntity, validationGroup));
			list.Add(new ExclusionsMaxLengthRule(scheduleEntity, validationGroup));
			list.Add(new NameMaxLengthRule(scheduleEntity, validationGroup));
			list.Add(new RecurrenceRuleMaxLengthRule(scheduleEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(scheduleEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< ScheduleEntity > scheduleEntityCollection)
        {
			return ScheduleEntityRuleContainer.GetGeneratedRules(scheduleEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< ScheduleEntity > scheduleEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(ScheduleEntity scheduleEntity in scheduleEntityCollection)
				list.AddRange(ScheduleEntityRuleContainer.GetGeneratedRules(scheduleEntity, validationGroup));
			
            return list;
        }

		public class NameUniqueIdRule : BaseValidationRule<ScheduleEntity>
        {
            public NameUniqueIdRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = EntityOverwriteSettings.GetEntityOverwriteSetting("ScheduleNameUniqueId") ??  "The Schedule is not unique. Duplicate NameUniqueId values are not allowed.";
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
                using (AppService app = new AppService(DataAccessAdapterFactory.CreateNoLockAdapter()))
                {
	                ScheduleEntity scheduleEntity = new ScheduleEntity();
					scheduleEntity.Name = this._value.Name;
					scheduleEntity.UniqueId = this._value.UniqueId;
					

					LinqMetaData linqMetaData = new LinqMetaData(app.Adapter);
						
					scheduleEntity = 
						linqMetaData.Schedule.Where(v => v.Name == this._value.Name).Where(v => v.UniqueId == this._value.UniqueId).FirstOrDefault() ?? new ScheduleEntity();
	                
					if(!scheduleEntity.IsNew)
					{
						if (scheduleEntity.ScheduleId != this._value.ScheduleId)
						{
							this._isBroken = true;
						}
						
					}
					
                    linqMetaData = null;
				}

                return this._isBroken;
            }
        }
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public CreateUserMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > ScheduleFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class DescriptionMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public DescriptionMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.Description.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > ScheduleFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class ExclusionsMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public ExclusionsMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.Exclusions.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.Exclusions.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Exclusions != null)
					if (this._value.Exclusions.Length > ScheduleFields.Exclusions.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class NameMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public NameMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.Name.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > ScheduleFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class RecurrenceRuleMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public RecurrenceRuleMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.RecurrenceRule.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.RecurrenceRule.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.RecurrenceRule != null)
					if (this._value.RecurrenceRule.Length > ScheduleFields.RecurrenceRule.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<ScheduleEntity>
        {
            public UpdateUserMaxLengthRule(ScheduleEntity scheduleEntity, string validationGroup)
            {
                this._message = string.Format(ScheduleFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", ScheduleFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = scheduleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > ScheduleFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
