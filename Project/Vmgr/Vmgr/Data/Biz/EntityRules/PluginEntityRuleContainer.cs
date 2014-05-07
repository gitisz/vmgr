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
	public partial class PluginEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(PluginEntity pluginEntity)
        {
			return PluginEntityRuleContainer.GetGeneratedRules(pluginEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(PluginEntity pluginEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(pluginEntity, validationGroup));
			list.Add(new DescriptionMaxLengthRule(pluginEntity, validationGroup));
			list.Add(new NameMaxLengthRule(pluginEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(pluginEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< PluginEntity > pluginEntityCollection)
        {
			return PluginEntityRuleContainer.GetGeneratedRules(pluginEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< PluginEntity > pluginEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(PluginEntity pluginEntity in pluginEntityCollection)
				list.AddRange(PluginEntityRuleContainer.GetGeneratedRules(pluginEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<PluginEntity>
        {
            public CreateUserMaxLengthRule(PluginEntity pluginEntity, string validationGroup)
            {
                this._message = string.Format(PluginFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", PluginFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = pluginEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > PluginFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class DescriptionMaxLengthRule : BaseValidationRule<PluginEntity>
        {
            public DescriptionMaxLengthRule(PluginEntity pluginEntity, string validationGroup)
            {
                this._message = string.Format(PluginFields.Description.Name.HumanizeString() + " must be less than {0} characters.", PluginFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = pluginEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > PluginFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<PluginEntity>
        {
            public NameMaxLengthRule(PluginEntity pluginEntity, string validationGroup)
            {
                this._message = string.Format(PluginFields.Name.Name.HumanizeString() + " must be less than {0} characters.", PluginFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = pluginEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > PluginFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<PluginEntity>
        {
            public UpdateUserMaxLengthRule(PluginEntity pluginEntity, string validationGroup)
            {
                this._message = string.Format(PluginFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", PluginFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = pluginEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > PluginFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
