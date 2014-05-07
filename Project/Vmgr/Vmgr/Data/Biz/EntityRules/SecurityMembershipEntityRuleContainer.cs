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
	public partial class SecurityMembershipEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SecurityMembershipEntity securityMembershipEntity)
        {
			return SecurityMembershipEntityRuleContainer.GetGeneratedRules(securityMembershipEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SecurityMembershipEntity securityMembershipEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new AccountMaxLengthRule(securityMembershipEntity, validationGroup));
			list.Add(new CreateUserMaxLengthRule(securityMembershipEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(securityMembershipEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityMembershipEntity > securityMembershipEntityCollection)
        {
			return SecurityMembershipEntityRuleContainer.GetGeneratedRules(securityMembershipEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityMembershipEntity > securityMembershipEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SecurityMembershipEntity securityMembershipEntity in securityMembershipEntityCollection)
				list.AddRange(SecurityMembershipEntityRuleContainer.GetGeneratedRules(securityMembershipEntity, validationGroup));
			
            return list;
        }

		public class AccountMaxLengthRule : BaseValidationRule<SecurityMembershipEntity>
        {
            public AccountMaxLengthRule(SecurityMembershipEntity securityMembershipEntity, string validationGroup)
            {
                this._message = string.Format(SecurityMembershipFields.Account.Name.HumanizeString() + " must be less than {0} characters.", SecurityMembershipFields.Account.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityMembershipEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Account != null)
					if (this._value.Account.Length > SecurityMembershipFields.Account.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<SecurityMembershipEntity>
        {
            public CreateUserMaxLengthRule(SecurityMembershipEntity securityMembershipEntity, string validationGroup)
            {
                this._message = string.Format(SecurityMembershipFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityMembershipFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityMembershipEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > SecurityMembershipFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<SecurityMembershipEntity>
        {
            public UpdateUserMaxLengthRule(SecurityMembershipEntity securityMembershipEntity, string validationGroup)
            {
                this._message = string.Format(SecurityMembershipFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityMembershipFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityMembershipEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > SecurityMembershipFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
