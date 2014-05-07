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
	public partial class LogEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(LogEntity logEntity)
        {
			return LogEntityRuleContainer.GetGeneratedRules(logEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(LogEntity logEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
				
			list.Add(new ApplicationNameMaxLengthRule(logEntity, validationGroup));
			list.Add(new CorrelationIdMaxLengthRule(logEntity, validationGroup));
			list.Add(new CreateUserMaxLengthRule(logEntity, validationGroup));
			list.Add(new ExceptionMaxLengthRule(logEntity, validationGroup));
			list.Add(new LevelMaxLengthRule(logEntity, validationGroup));
			list.Add(new LoggerMaxLengthRule(logEntity, validationGroup));
			list.Add(new MessageMaxLengthRule(logEntity, validationGroup));
			list.Add(new ServerMaxLengthRule(logEntity, validationGroup));
			list.Add(new ThreadMaxLengthRule(logEntity, validationGroup));
			list.Add(new ThreadIdMaxLengthRule(logEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< LogEntity > logEntityCollection)
        {
			return LogEntityRuleContainer.GetGeneratedRules(logEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< LogEntity > logEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(LogEntity logEntity in logEntityCollection)
				list.AddRange(LogEntityRuleContainer.GetGeneratedRules(logEntity, validationGroup));
			
            return list;
        }

		public class ApplicationNameMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public ApplicationNameMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.ApplicationName.Name.HumanizeString() + " must be less than {0} characters.", LogFields.ApplicationName.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.ApplicationName != null)
					if (this._value.ApplicationName.Length > LogFields.ApplicationName.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class CorrelationIdMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public CorrelationIdMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.CorrelationId.Name.HumanizeString() + " must be less than {0} characters.", LogFields.CorrelationId.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CorrelationId != null)
					if (this._value.CorrelationId.Length > LogFields.CorrelationId.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public CreateUserMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", LogFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > LogFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ExceptionMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public ExceptionMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Exception.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Exception.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Exception != null)
					if (this._value.Exception.Length > LogFields.Exception.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		public class LevelMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public LevelMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Level.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Level.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Level != null)
					if (this._value.Level.Length > LogFields.Level.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class LoggerMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public LoggerMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Logger.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Logger.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Logger != null)
					if (this._value.Logger.Length > LogFields.Logger.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class MessageMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public MessageMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Message.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Message.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Message != null)
					if (this._value.Message.Length > LogFields.Message.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ServerMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public ServerMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Server.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Server.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Server != null)
					if (this._value.Server.Length > LogFields.Server.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ThreadMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public ThreadMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.Thread.Name.HumanizeString() + " must be less than {0} characters.", LogFields.Thread.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Thread != null)
					if (this._value.Thread.Length > LogFields.Thread.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class ThreadIdMaxLengthRule : BaseValidationRule<LogEntity>
        {
            public ThreadIdMaxLengthRule(LogEntity logEntity, string validationGroup)
            {
                this._message = string.Format(LogFields.ThreadId.Name.HumanizeString() + " must be less than {0} characters.", LogFields.ThreadId.MaxLength);
                this._validationGroup = validationGroup;
				this._value = logEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.ThreadId != null)
					if (this._value.ThreadId.Length > LogFields.ThreadId.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
