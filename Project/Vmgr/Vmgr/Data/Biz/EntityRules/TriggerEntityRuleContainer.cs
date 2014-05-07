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
	public partial class TriggerEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(TriggerEntity triggerEntity)
        {
			return TriggerEntityRuleContainer.GetGeneratedRules(triggerEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(TriggerEntity triggerEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(triggerEntity, validationGroup));
			list.Add(new TriggerKeyMaxLengthRule(triggerEntity, validationGroup));
			list.Add(new TriggerKeyGroupMaxLengthRule(triggerEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(triggerEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< TriggerEntity > triggerEntityCollection)
        {
			return TriggerEntityRuleContainer.GetGeneratedRules(triggerEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< TriggerEntity > triggerEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(TriggerEntity triggerEntity in triggerEntityCollection)
				list.AddRange(TriggerEntityRuleContainer.GetGeneratedRules(triggerEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<TriggerEntity>
        {
            public CreateUserMaxLengthRule(TriggerEntity triggerEntity, string validationGroup)
            {
                this._message = string.Format(TriggerFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", TriggerFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = triggerEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > TriggerFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		
		
		
		
		
		public class TriggerKeyMaxLengthRule : BaseValidationRule<TriggerEntity>
        {
            public TriggerKeyMaxLengthRule(TriggerEntity triggerEntity, string validationGroup)
            {
                this._message = string.Format(TriggerFields.TriggerKey.Name.HumanizeString() + " must be less than {0} characters.", TriggerFields.TriggerKey.MaxLength);
                this._validationGroup = validationGroup;
				this._value = triggerEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.TriggerKey != null)
					if (this._value.TriggerKey.Length > TriggerFields.TriggerKey.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class TriggerKeyGroupMaxLengthRule : BaseValidationRule<TriggerEntity>
        {
            public TriggerKeyGroupMaxLengthRule(TriggerEntity triggerEntity, string validationGroup)
            {
                this._message = string.Format(TriggerFields.TriggerKeyGroup.Name.HumanizeString() + " must be less than {0} characters.", TriggerFields.TriggerKeyGroup.MaxLength);
                this._validationGroup = validationGroup;
				this._value = triggerEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.TriggerKeyGroup != null)
					if (this._value.TriggerKeyGroup.Length > TriggerFields.TriggerKeyGroup.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<TriggerEntity>
        {
            public UpdateUserMaxLengthRule(TriggerEntity triggerEntity, string validationGroup)
            {
                this._message = string.Format(TriggerFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", TriggerFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = triggerEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > TriggerFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
