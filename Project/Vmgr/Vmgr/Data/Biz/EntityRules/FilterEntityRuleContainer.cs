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
	public partial class FilterEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(FilterEntity filterEntity)
        {
			return FilterEntityRuleContainer.GetGeneratedRules(filterEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(FilterEntity filterEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(filterEntity, validationGroup));
			list.Add(new ExpressionMaxLengthRule(filterEntity, validationGroup));
			list.Add(new FilterTypeMaxLengthRule(filterEntity, validationGroup));
			list.Add(new NameMaxLengthRule(filterEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(filterEntity, validationGroup));
			list.Add(new UsernameMaxLengthRule(filterEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< FilterEntity > filterEntityCollection)
        {
			return FilterEntityRuleContainer.GetGeneratedRules(filterEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< FilterEntity > filterEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(FilterEntity filterEntity in filterEntityCollection)
				list.AddRange(FilterEntityRuleContainer.GetGeneratedRules(filterEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public CreateUserMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > FilterFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class ExpressionMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public ExpressionMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.Expression.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.Expression.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Expression != null)
					if (this._value.Expression.Length > FilterFields.Expression.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class FilterTypeMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public FilterTypeMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.FilterType.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.FilterType.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.FilterType != null)
					if (this._value.FilterType.Length > FilterFields.FilterType.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public NameMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.Name.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > FilterFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public UpdateUserMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > FilterFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class UsernameMaxLengthRule : BaseValidationRule<FilterEntity>
        {
            public UsernameMaxLengthRule(FilterEntity filterEntity, string validationGroup)
            {
                this._message = string.Format(FilterFields.Username.Name.HumanizeString() + " must be less than {0} characters.", FilterFields.Username.MaxLength);
                this._validationGroup = validationGroup;
				this._value = filterEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Username != null)
					if (this._value.Username.Length > FilterFields.Username.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
