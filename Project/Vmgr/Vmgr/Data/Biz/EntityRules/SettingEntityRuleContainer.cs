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
	public partial class SettingEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SettingEntity settingEntity)
        {
			return SettingEntityRuleContainer.GetGeneratedRules(settingEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SettingEntity settingEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(settingEntity, validationGroup));
			list.Add(new KeyMaxLengthRule(settingEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(settingEntity, validationGroup));
			list.Add(new ValueMaxLengthRule(settingEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SettingEntity > settingEntityCollection)
        {
			return SettingEntityRuleContainer.GetGeneratedRules(settingEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SettingEntity > settingEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SettingEntity settingEntity in settingEntityCollection)
				list.AddRange(SettingEntityRuleContainer.GetGeneratedRules(settingEntity, validationGroup));
			
            return list;
        }

		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<SettingEntity>
        {
            public CreateUserMaxLengthRule(SettingEntity settingEntity, string validationGroup)
            {
                this._message = string.Format(SettingFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", SettingFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = settingEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > SettingFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class KeyMaxLengthRule : BaseValidationRule<SettingEntity>
        {
            public KeyMaxLengthRule(SettingEntity settingEntity, string validationGroup)
            {
                this._message = string.Format(SettingFields.Key.Name.HumanizeString() + " must be less than {0} characters.", SettingFields.Key.MaxLength);
                this._validationGroup = validationGroup;
				this._value = settingEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Key != null)
					if (this._value.Key.Length > SettingFields.Key.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<SettingEntity>
        {
            public UpdateUserMaxLengthRule(SettingEntity settingEntity, string validationGroup)
            {
                this._message = string.Format(SettingFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", SettingFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = settingEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > SettingFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ValueMaxLengthRule : BaseValidationRule<SettingEntity>
        {
            public ValueMaxLengthRule(SettingEntity settingEntity, string validationGroup)
            {
                this._message = string.Format(SettingFields.Value.Name.HumanizeString() + " must be less than {0} characters.", SettingFields.Value.MaxLength);
                this._validationGroup = validationGroup;
				this._value = settingEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Value != null)
					if (this._value.Value.Length > SettingFields.Value.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
