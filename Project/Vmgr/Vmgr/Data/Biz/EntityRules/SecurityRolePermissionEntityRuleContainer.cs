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
	public partial class SecurityRolePermissionEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SecurityRolePermissionEntity securityRolePermissionEntity)
        {
			return SecurityRolePermissionEntityRuleContainer.GetGeneratedRules(securityRolePermissionEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SecurityRolePermissionEntity securityRolePermissionEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
			list.Add(new SecurityPermissionIdSecurityRoleIdRule(securityRolePermissionEntity, validationGroup));
				
			list.Add(new CreateUserMaxLengthRule(securityRolePermissionEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(securityRolePermissionEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityRolePermissionEntity > securityRolePermissionEntityCollection)
        {
			return SecurityRolePermissionEntityRuleContainer.GetGeneratedRules(securityRolePermissionEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityRolePermissionEntity > securityRolePermissionEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SecurityRolePermissionEntity securityRolePermissionEntity in securityRolePermissionEntityCollection)
				list.AddRange(SecurityRolePermissionEntityRuleContainer.GetGeneratedRules(securityRolePermissionEntity, validationGroup));
			
            return list;
        }

		public class SecurityPermissionIdSecurityRoleIdRule : BaseValidationRule<SecurityRolePermissionEntity>
        {
            public SecurityPermissionIdSecurityRoleIdRule(SecurityRolePermissionEntity securityRolePermissionEntity, string validationGroup)
            {
                this._message = EntityOverwriteSettings.GetEntityOverwriteSetting("SecurityRolePermissionSecurityPermissionIdSecurityRoleId") ??  "The SecurityRolePermission is not unique. Duplicate SecurityPermissionIdSecurityRoleId values are not allowed.";
                this._validationGroup = validationGroup;
				this._value = securityRolePermissionEntity;
            }

            public override bool ValidateExpression()
            {
                using (AppService app = new AppService(DataAccessAdapterFactory.CreateNoLockAdapter()))
                {
	                SecurityRolePermissionEntity securityRolePermissionEntity = new SecurityRolePermissionEntity();
					securityRolePermissionEntity.SecurityPermissionId = this._value.SecurityPermissionId;
					securityRolePermissionEntity.SecurityRoleId = this._value.SecurityRoleId;
					

					LinqMetaData linqMetaData = new LinqMetaData(app.Adapter);
						
					securityRolePermissionEntity = 
						linqMetaData.SecurityRolePermission.Where(v => v.SecurityPermissionId == this._value.SecurityPermissionId).Where(v => v.SecurityRoleId == this._value.SecurityRoleId).FirstOrDefault() ?? new SecurityRolePermissionEntity();
	                
					if(!securityRolePermissionEntity.IsNew)
					{
						if (securityRolePermissionEntity.SecurityRolePermissionId != this._value.SecurityRolePermissionId)
						{
							this._isBroken = true;
						}
						
					}
					
                    linqMetaData = null;
				}

                return this._isBroken;
            }
        }
		
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<SecurityRolePermissionEntity>
        {
            public CreateUserMaxLengthRule(SecurityRolePermissionEntity securityRolePermissionEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRolePermissionFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityRolePermissionFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRolePermissionEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > SecurityRolePermissionFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<SecurityRolePermissionEntity>
        {
            public UpdateUserMaxLengthRule(SecurityRolePermissionEntity securityRolePermissionEntity, string validationGroup)
            {
                this._message = string.Format(SecurityRolePermissionFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", SecurityRolePermissionFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityRolePermissionEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > SecurityRolePermissionFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
