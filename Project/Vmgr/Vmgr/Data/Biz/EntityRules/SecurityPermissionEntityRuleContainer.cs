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
	public partial class SecurityPermissionEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(SecurityPermissionEntity securityPermissionEntity)
        {
			return SecurityPermissionEntityRuleContainer.GetGeneratedRules(securityPermissionEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(SecurityPermissionEntity securityPermissionEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
			list.Add(new NameRule(securityPermissionEntity, validationGroup));
				
			list.Add(new DescriptionMaxLengthRule(securityPermissionEntity, validationGroup));
			list.Add(new NameMaxLengthRule(securityPermissionEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityPermissionEntity > securityPermissionEntityCollection)
        {
			return SecurityPermissionEntityRuleContainer.GetGeneratedRules(securityPermissionEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< SecurityPermissionEntity > securityPermissionEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(SecurityPermissionEntity securityPermissionEntity in securityPermissionEntityCollection)
				list.AddRange(SecurityPermissionEntityRuleContainer.GetGeneratedRules(securityPermissionEntity, validationGroup));
			
            return list;
        }

		public class NameRule : BaseValidationRule<SecurityPermissionEntity>
        {
            public NameRule(SecurityPermissionEntity securityPermissionEntity, string validationGroup)
            {
                this._message = EntityOverwriteSettings.GetEntityOverwriteSetting("SecurityPermissionName") ??  "The SecurityPermission is not unique. Duplicate Name values are not allowed.";
                this._validationGroup = validationGroup;
				this._value = securityPermissionEntity;
            }

            public override bool ValidateExpression()
            {
                using (AppService app = new AppService(DataAccessAdapterFactory.CreateNoLockAdapter()))
                {
	                SecurityPermissionEntity securityPermissionEntity = new SecurityPermissionEntity();
					securityPermissionEntity.Name = this._value.Name;
					

					LinqMetaData linqMetaData = new LinqMetaData(app.Adapter);
						
					securityPermissionEntity = 
						linqMetaData.SecurityPermission.Where(v => v.Name == this._value.Name).FirstOrDefault() ?? new SecurityPermissionEntity();
	                
					if(!securityPermissionEntity.IsNew)
					{
						if (securityPermissionEntity.SecurityPermissionId != this._value.SecurityPermissionId)
						{
							this._isBroken = true;
						}
						
					}
					
                    linqMetaData = null;
				}

                return this._isBroken;
            }
        }
		
		public class DescriptionMaxLengthRule : BaseValidationRule<SecurityPermissionEntity>
        {
            public DescriptionMaxLengthRule(SecurityPermissionEntity securityPermissionEntity, string validationGroup)
            {
                this._message = string.Format(SecurityPermissionFields.Description.Name.HumanizeString() + " must be less than {0} characters.", SecurityPermissionFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityPermissionEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > SecurityPermissionFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<SecurityPermissionEntity>
        {
            public NameMaxLengthRule(SecurityPermissionEntity securityPermissionEntity, string validationGroup)
            {
                this._message = string.Format(SecurityPermissionFields.Name.Name.HumanizeString() + " must be less than {0} characters.", SecurityPermissionFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = securityPermissionEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > SecurityPermissionFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
    }
}
