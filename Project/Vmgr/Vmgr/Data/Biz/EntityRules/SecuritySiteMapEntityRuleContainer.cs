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
	public partial class SecuritySiteMapEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SecuritySiteMapEntity securitySiteMapEntity)
        {
			return SecuritySiteMapEntityRuleContainer.GetGeneratedRules(securitySiteMapEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SecuritySiteMapEntity securitySiteMapEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(securitySiteMapEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(securitySiteMapEntity, validationGroup));
			list.Add(new ValueMaxLengthRule(securitySiteMapEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecuritySiteMapEntity > securitySiteMapEntityCollection)
        {
			return SecuritySiteMapEntityRuleContainer.GetGeneratedRules(securitySiteMapEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecuritySiteMapEntity > securitySiteMapEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SecuritySiteMapEntity securitySiteMapEntity in securitySiteMapEntityCollection)
				list.AddRange(SecuritySiteMapEntityRuleContainer.GetGeneratedRules(securitySiteMapEntity, validationGroup));
			
            return list;
        }

		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<SecuritySiteMapEntity>
        {
            public CreateUserMaxLengthRule(SecuritySiteMapEntity securitySiteMapEntity, string validationGroup)
            {
                this._message = string.Format(SecuritySiteMapFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", SecuritySiteMapFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securitySiteMapEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > SecuritySiteMapFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<SecuritySiteMapEntity>
        {
            public UpdateUserMaxLengthRule(SecuritySiteMapEntity securitySiteMapEntity, string validationGroup)
            {
                this._message = string.Format(SecuritySiteMapFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", SecuritySiteMapFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securitySiteMapEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > SecuritySiteMapFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ValueMaxLengthRule : BaseValidationRule<SecuritySiteMapEntity>
        {
            public ValueMaxLengthRule(SecuritySiteMapEntity securitySiteMapEntity, string validationGroup)
            {
                this._message = string.Format(SecuritySiteMapFields.Value.Name.HumanizeString() + " must be less than {0} characters.", SecuritySiteMapFields.Value.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securitySiteMapEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Value != null)
					if (this._value.Value.Length > SecuritySiteMapFields.Value.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
