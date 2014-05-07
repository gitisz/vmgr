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
	public partial class JobEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(JobEntity jobEntity)
        {
			return JobEntityRuleContainer.GetGeneratedRules(jobEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(JobEntity jobEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(jobEntity, validationGroup));
			list.Add(new JobKeyMaxLengthRule(jobEntity, validationGroup));
			list.Add(new JobKeyGroupMaxLengthRule(jobEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(jobEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< JobEntity > jobEntityCollection)
        {
			return JobEntityRuleContainer.GetGeneratedRules(jobEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< JobEntity > jobEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(JobEntity jobEntity in jobEntityCollection)
				list.AddRange(JobEntityRuleContainer.GetGeneratedRules(jobEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<JobEntity>
        {
            public CreateUserMaxLengthRule(JobEntity jobEntity, string validationGroup)
            {
                this._message = string.Format(JobFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", JobFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = jobEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > JobFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class JobKeyMaxLengthRule : BaseValidationRule<JobEntity>
        {
            public JobKeyMaxLengthRule(JobEntity jobEntity, string validationGroup)
            {
                this._message = string.Format(JobFields.JobKey.Name.HumanizeString() + " must be less than {0} characters.", JobFields.JobKey.MaxLength);
                this._validationGroup = validationGroup;
				this._value = jobEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.JobKey != null)
					if (this._value.JobKey.Length > JobFields.JobKey.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class JobKeyGroupMaxLengthRule : BaseValidationRule<JobEntity>
        {
            public JobKeyGroupMaxLengthRule(JobEntity jobEntity, string validationGroup)
            {
                this._message = string.Format(JobFields.JobKeyGroup.Name.HumanizeString() + " must be less than {0} characters.", JobFields.JobKeyGroup.MaxLength);
                this._validationGroup = validationGroup;
				this._value = jobEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.JobKeyGroup != null)
					if (this._value.JobKeyGroup.Length > JobFields.JobKeyGroup.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<JobEntity>
        {
            public UpdateUserMaxLengthRule(JobEntity jobEntity, string validationGroup)
            {
                this._message = string.Format(JobFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", JobFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = jobEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > JobFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
