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
	public partial class MonitorEntityRuleContainer
	{
		public static IList<IValidationRule> GetGeneratedRules(MonitorEntity monitorEntity)
        {
			return MonitorEntityRuleContainer.GetGeneratedRules(monitorEntity, string.Empty);
        }
        public static IList<IValidationRule> GetGeneratedRules(MonitorEntity monitorEntity, string validationGroup)
        {
            List<IValidationRule> list = new List<IValidationRule>();
			list.Add(new PackageIdUsernameRule(monitorEntity, validationGroup));
				
			list.Add(new CreateUserMaxLengthRule(monitorEntity, validationGroup));
			list.Add(new UpdateUserMaxLengthRule(monitorEntity, validationGroup));
			list.Add(new UsernameMaxLengthRule(monitorEntity, validationGroup));
			
            return list;
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< MonitorEntity > monitorEntityCollection)
        {
			return MonitorEntityRuleContainer.GetGeneratedRules(monitorEntityCollection, string.Empty);
        }
		
		public static IList<IValidationRule> GetGeneratedRules(IEnumerable< MonitorEntity > monitorEntityCollection, string validationGroup)
        {
			List<IValidationRule> list = new List<IValidationRule>();
			foreach(MonitorEntity monitorEntity in monitorEntityCollection)
				list.AddRange(MonitorEntityRuleContainer.GetGeneratedRules(monitorEntity, validationGroup));
			
            return list;
        }

		public class PackageIdUsernameRule : BaseValidationRule<MonitorEntity>
        {
            public PackageIdUsernameRule(MonitorEntity monitorEntity, string validationGroup)
            {
                this._message = EntityOverwriteSettings.GetEntityOverwriteSetting("MonitorPackageIdUsername") ??  "The Monitor is not unique. Duplicate PackageIdUsername values are not allowed.";
                this._validationGroup = validationGroup;
				this._value = monitorEntity;
            }

            public override bool ValidateExpression()
            {
                using (AppService app = new AppService(DataAccessAdapterFactory.CreateNoLockAdapter()))
                {
	                MonitorEntity monitorEntity = new MonitorEntity();
					monitorEntity.PackageId = this._value.PackageId;
					monitorEntity.Username = this._value.Username;
					

					LinqMetaData linqMetaData = new LinqMetaData(app.Adapter);
						
					monitorEntity = 
						linqMetaData.Monitor.Where(v => v.PackageId == this._value.PackageId).Where(v => v.Username == this._value.Username).FirstOrDefault() ?? new MonitorEntity();
	                
					if(!monitorEntity.IsNew)
					{
						if (monitorEntity.MonitorId != this._value.MonitorId)
						{
							this._isBroken = true;
						}
						
					}
					
                    linqMetaData = null;
				}

                return this._isBroken;
            }
        }
		
		
		public class CreateUserMaxLengthRule : BaseValidationRule<MonitorEntity>
        {
            public CreateUserMaxLengthRule(MonitorEntity monitorEntity, string validationGroup)
            {
                this._message = string.Format(MonitorFields.CreateUser.Name.HumanizeString() + " must be less than {0} characters.", MonitorFields.CreateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = monitorEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.CreateUser != null)
					if (this._value.CreateUser.Length > MonitorFields.CreateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
		
		public class UpdateUserMaxLengthRule : BaseValidationRule<MonitorEntity>
        {
            public UpdateUserMaxLengthRule(MonitorEntity monitorEntity, string validationGroup)
            {
                this._message = string.Format(MonitorFields.UpdateUser.Name.HumanizeString() + " must be less than {0} characters.", MonitorFields.UpdateUser.MaxLength);
                this._validationGroup = validationGroup;
				this._value = monitorEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.UpdateUser != null)
					if (this._value.UpdateUser.Length > MonitorFields.UpdateUser.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		public class UsernameMaxLengthRule : BaseValidationRule<MonitorEntity>
        {
            public UsernameMaxLengthRule(MonitorEntity monitorEntity, string validationGroup)
            {
                this._message = string.Format(MonitorFields.Username.Name.HumanizeString() + " must be less than {0} characters.", MonitorFields.Username.MaxLength);
                this._validationGroup = validationGroup;
				this._value = monitorEntity;
            }

            public override bool ValidateExpression()
            {
				if (this._value.Username != null)
					if (this._value.Username.Length > MonitorFields.Username.MaxLength)
						this._isBroken = true;

                return this._isBroken;
            }
        }
		
		
		
    }
}
