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
	public partial class PackageEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(PackageEntity packageEntity)
        {
			return PackageEntityRuleContainer.GetGeneratedRules(packageEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(PackageEntity packageEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(packageEntity, validationGroup));
			list.Add(new DescriptionMaxLengthRule(packageEntity, validationGroup));
			list.Add(new NameMaxLengthRule(packageEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(packageEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< PackageEntity > packageEntityCollection)
        {
			return PackageEntityRuleContainer.GetGeneratedRules(packageEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< PackageEntity > packageEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(PackageEntity packageEntity in packageEntityCollection)
				list.AddRange(PackageEntityRuleContainer.GetGeneratedRules(packageEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<PackageEntity>
        {
            public CreateUserMaxLengthRule(PackageEntity packageEntity, string validationGroup)
            {
                this._message = string.Format(PackageFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", PackageFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = packageEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > PackageFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class DescriptionMaxLengthRule : BaseValidationRule<PackageEntity>
        {
            public DescriptionMaxLengthRule(PackageEntity packageEntity, string validationGroup)
            {
                this._message = string.Format(PackageFields.Description.Name.HumanizeString() + " must be less than {0} characters.", PackageFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = packageEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > PackageFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<PackageEntity>
        {
            public NameMaxLengthRule(PackageEntity packageEntity, string validationGroup)
            {
                this._message = string.Format(PackageFields.Name.Name.HumanizeString() + " must be less than {0} characters.", PackageFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = packageEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > PackageFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<PackageEntity>
        {
            public UpdateUserMaxLengthRule(PackageEntity packageEntity, string validationGroup)
            {
                this._message = string.Format(PackageFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", PackageFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = packageEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > PackageFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
