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
	public partial class SecurityRoleEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SecurityRoleEntity securityRoleEntity)
        {
			return SecurityRoleEntityRuleContainer.GetGeneratedRules(securityRoleEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SecurityRoleEntity securityRoleEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
			list.Add(new NameRule(securityRoleEntity, validationGroup));
				
			list.Add(new CreateUserMaxLengthRule(securityRoleEntity, validationGroup));
			list.Add(new DescriptionMaxLengthRule(securityRoleEntity, validationGroup));
			list.Add(new NameMaxLengthRule(securityRoleEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(securityRoleEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityRoleEntity > securityRoleEntityCollection)
        {
			return SecurityRoleEntityRuleContainer.GetGeneratedRules(securityRoleEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityRoleEntity > securityRoleEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SecurityRoleEntity securityRoleEntity in securityRoleEntityCollection)
				list.AddRange(SecurityRoleEntityRuleContainer.GetGeneratedRules(securityRoleEntity, validationGroup));
			
            return list;
        }

		public class NameRule : BaseValidationRule<SecurityRoleEntity>
        {
            public NameRule(SecurityRoleEntity securityRoleEntity, string validationGroup)
            {
                this._message = EntityOverwriteSettings.GetEntityOverwriteSetting("SecurityRoleName") ??  "The SecurityRole is not unique. Duplicate Name values are not allowed.";
                this._validationGroup = validationGroup;
				this._value = securityRoleEntity;
            }

            public override bool ValidateExpression()
            {
                using (AppService app = new AppService(DataAccessAdapterFactory.CreateNoLockAdapter()))
                {
	                SecurityRoleEntity securityRoleEntity = new SecurityRoleEntity();
					securityRoleEntity.Name = this._value.Name;
					

					LinqMetaData linqMetaData = new LinqMetaData(app.Adapter);
						
					securityRoleEntity = 
						linqMetaData.SecurityRole.Where(v => v.Name == this._value.Name).FirstOrDefault() ?? new SecurityRoleEntity();
	                
					if(!securityRoleEntity.IsNew)
					{
						if (securityRoleEntity.SecurityRoleId != this._value.SecurityRoleId)
						{
							this._isBroken = true;
						}
						
					}
					
                    linqMetaData = null;
				}

                return this._isBroken;
            }
        }
		
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<SecurityRoleEntity>
        {
            public CreateUserMaxLengthRule(SecurityRoleEntity securityRoleEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRoleFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityRoleFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRoleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > SecurityRoleFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class DescriptionMaxLengthRule : BaseValidationRule<SecurityRoleEntity>
        {
            public DescriptionMaxLengthRule(SecurityRoleEntity securityRoleEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRoleFields.Description.Name.HumanizeString() + " must be less than {0} characters.", SecurityRoleFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRoleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > SecurityRoleFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<SecurityRoleEntity>
        {
            public NameMaxLengthRule(SecurityRoleEntity securityRoleEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRoleFields.Name.Name.HumanizeString() + " must be less than {0} characters.", SecurityRoleFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRoleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > SecurityRoleFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<SecurityRoleEntity>
        {
            public UpdateUserMaxLengthRule(SecurityRoleEntity securityRoleEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRoleFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityRoleFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRoleEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > SecurityRoleFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
