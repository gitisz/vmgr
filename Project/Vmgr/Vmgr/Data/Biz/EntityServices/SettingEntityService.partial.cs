using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
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
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Biz.EntityServices;
using Vmgr.Data.Biz.EntityRules;

namespace Vmgr.Data.Biz.EntityServices
{
    internal partial class SettingService : BaseEntityBizService<SettingEntity>, IService<SettingEntity>
    {
        #region PRIVATE PROPERTIES
		
        private IList<IService> _services = null;

		#endregion

        #region PROTECTED PROPERTIES

        internal override IEntity2 Entity
        {
            get
            {
                if (base._entity == null)
                {
                     IQueryable<SettingEntity> e = base.linqMetaData.Setting;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SettingId == this.setting.SettingId) ?? 
							new SettingEntity();
                    
					base.bizEntity = new GenericBizEntity<SettingEntity>(base._entity as SettingEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SettingMetaData setting
        {
            get
            {
                return base.MetaData as SettingMetaData;
            }
            private set
            {
                base.MetaData = value;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public override IList<IService> Services
        {
            get
            {
                if (this._services == null)
                {
                    this._services = new List<IService> {  };
                }

                return this._services;
            }
        }

        #endregion

        #region CTOR

        public SettingService(IAppService appService)
            : base(appService)
        {
        }

        public SettingService(SettingMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.setting = metaData;
        }

        public SettingService(SettingEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SettingEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SettingEntity> entity);
        partial void OnPartialGetSettings(ref IQueryable<SettingMetaData> q);
        partial void OnPartialValidating(IAppService appService);
        partial void OnPartialBeforeSave(IAppService appService);
        partial void OnPartialAfterSave(IAppService appService);
        partial void OnPartialBeforeDelete(IAppService appService);
        partial void OnPartialAfterDelete(IAppService appService);
        partial void OnPartialProcessingComplete(IAppService appService);

		#endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public override bool Save(IMetaData metaData)
        {
            this.ConvertTo(metaData);

			base.bizEntity.OnValidating = this.OnValidating;
			base.bizEntity.OnBeforeSave = this.OnBeforeSave;
            base.bizEntity.OnAfterSave = this.OnAfterSave;

			//	Validation will automatically occur
            bool result = base.bizEntity.Save(true);

			if(result)
			{
				this.ConvertBack();
			}

            this.OnProcessingComplete(base.appService);

			return result;
        }

        public override bool Delete(IMetaData metaData)
        {
            this.ConvertTo(metaData);
			
            base.bizEntity.OnValidating = this.OnValidating;
            base.bizEntity.OnBeforeDelete = this.OnBeforeDelete;
            base.bizEntity.OnAfterDelete = this.OnAfterDelete;

            bool result =  base.bizEntity.Delete();

            this.OnProcessingComplete(base.appService);

			return result;
        }

        public override SettingEntity ConvertTo(IMetaData metaData)
        {
            this.setting = metaData as SettingMetaData;
			
            this.OnBeforeProcessing(this.setting);
			
			SettingEntity e = this.Entity as SettingEntity;
			e.SetNewFieldValue("SettingId", this.setting.SettingId); 
			e.Cache = this.setting.Cache;
			e.Key = this.setting.Key;
			e.UpdateDate = this.setting.UpdateDate;
			e.Value = this.setting.Value;
			
			if (this.Entity.IsNew)
            {
                e.CreateUser = base.appService.User.Name;
            }
            else
            {
                e.UpdateUser = base.appService.User.Name;
            }
			
			return e;
        }
		
        public virtual SettingMetaData ConvertBack()
		{
            this.setting = this.setting ?? new SettingMetaData();			
			
			this.setting.SettingId = (System.Int32)base.bizEntity.PrimaryKey;
			this.setting.Cache = base.bizEntity.Entity.Cache;
			this.setting.CreateDate = base.bizEntity.Entity.CreateDate;
			this.setting.CreateUser = base.bizEntity.Entity.CreateUser;
			this.setting.Key = base.bizEntity.Entity.Key;
			this.setting.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.setting.UpdateUser = base.bizEntity.Entity.UpdateUser;
			this.setting.Value = base.bizEntity.Entity.Value;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.setting;
		}
        
		public SettingMetaData GetSetting(System.Int32 id)
        {
            return this.GetSettings().FirstOrDefault(u => u.SettingId == id);
        }
		
		public virtual IQueryable<SettingMetaData> GetSettings()
        {
            IQueryable<SettingMetaData> q = base.linqMetaData.Setting
                .Select(c => new SettingMetaData
                {   
                    SettingId = c.SettingId,
					
					Cache = c.Cache,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Key = c.Key,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					Value = c.Value,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetSettings(ref q);

            return q;
        }
		
        
		
        public override void Invalidate()
        {
            base.Invalidate();

            this._services = null;
        }
				
        #endregion

        #region EVENTS

        protected override void OnBeforeProcessing(IMetaData metaData)
        {
			base.OnBeforeProcessing(metaData);
        }

        protected override void OnValidating(IAppService appService)
        {
            base.bizEntity.AddRules(SettingEntityRuleContainer.GetGeneratedRules(this.Entity as SettingEntity));

            this.OnPartialValidating(appService);

	    	base.OnValidating(appService);
        }

        protected override void OnBeforeSave(IAppService appService)
        {
            this.OnPartialBeforeSave(appService);

            base.OnBeforeSave(appService);
        }

        protected override void OnAfterSave(IAppService appService)
        {
            this.OnPartialAfterSave(appService);

            base.OnAfterSave(appService);
        }

        protected override void OnBeforeDelete(IAppService appService)
        {
            this.OnPartialBeforeDelete(appService);

            base.OnBeforeDelete(appService);
        }

        protected override void OnAfterDelete(IAppService appService)
        {
            this.OnPartialAfterDelete(appService);

            base.OnAfterDelete(appService);
        }

        protected override void OnProcessingComplete(IAppService appService)
        {
            this.OnPartialProcessingComplete(appService);

            base.OnProcessingComplete(appService);
        }

        #endregion

    }
}

namespace Vmgr.Data.Biz
{
    public partial class AppService
    {
        #region PRIVATE PROPERTIES

        private SettingService _settingService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SettingService settingService
        {
            get
            {
                if (this._settingService == null)
                {
                    this._settingService = new SettingService(this);
                }
                return this._settingService;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public SettingMetaData GetSetting(System.Int32 id)
        {
            return this.settingService.GetSetting(id);
        }

        public IQueryable<SettingMetaData> GetSettings()
        {
            return this.settingService.GetSettings();
        }

        public bool Save(SettingMetaData settingMetaData)
        {
            return this.settingService.Save(settingMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
