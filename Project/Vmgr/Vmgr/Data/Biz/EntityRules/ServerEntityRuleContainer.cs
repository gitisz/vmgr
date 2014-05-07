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
	public partial class ServerEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(ServerEntity serverEntity)
        {
			return ServerEntityRuleContainer.GetGeneratedRules(serverEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(ServerEntity serverEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new CreateUserMaxLengthRule(serverEntity, validationGroup));
			list.Add(new DescriptionMaxLengthRule(serverEntity, validationGroup));
			list.Add(new NameMaxLengthRule(serverEntity, validationGroup));
			list.Add(new RTFqdnMaxLengthRule(serverEntity, validationGroup));
			list.Add(new RTProtocolMaxLengthRule(serverEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(serverEntity, validationGroup));
			list.Add(new WSFqdnMaxLengthRule(serverEntity, validationGroup));
			list.Add(new WSProtocolMaxLengthRule(serverEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< ServerEntity > serverEntityCollection)
        {
			return ServerEntityRuleContainer.GetGeneratedRules(serverEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< ServerEntity > serverEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(ServerEntity serverEntity in serverEntityCollection)
				list.AddRange(ServerEntityRuleContainer.GetGeneratedRules(serverEntity, validationGroup));
			
            return list;
        }

		
		public class CreateUserMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public CreateUserMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > ServerFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class DescriptionMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public DescriptionMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.Description.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.Description.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Description != null)
					if (this._value.Description.Length > ServerFields.Description.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class NameMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public NameMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.Name.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.Name.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Name != null)
					if (this._value.Name.Length > ServerFields.Name.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class RTFqdnMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public RTFqdnMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.RTFqdn.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.RTFqdn.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.RTFqdn != null)
					if (this._value.RTFqdn.Length > ServerFields.RTFqdn.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class RTProtocolMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public RTProtocolMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.RTProtocol.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.RTProtocol.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.RTProtocol != null)
					if (this._value.RTProtocol.Length > ServerFields.RTProtocol.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public UpdateUserMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > ServerFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class WSFqdnMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public WSFqdnMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.WSFqdn.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.WSFqdn.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.WSFqdn != null)
					if (this._value.WSFqdn.Length > ServerFields.WSFqdn.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class WSProtocolMaxLengthRule : BaseValidationRule<ServerEntity>
        {
            public WSProtocolMaxLengthRule(ServerEntity serverEntity, string validationGroup)
            {
                this._message = string.Format(ServerFields.WSProtocol.Name.HumanizeString() + " must be less than {0} characters.", ServerFields.WSProtocol.MaxLength);
                this._validationGroup = validationGroup;
				this._value = serverEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.WSProtocol != null)
					if (this._value.WSProtocol.Length > ServerFields.WSProtocol.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
