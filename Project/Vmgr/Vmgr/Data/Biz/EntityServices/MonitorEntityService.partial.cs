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
    internal partial class MonitorService : BaseEntityBizService<MonitorEntity>, IService<MonitorEntity>
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
                     IQueryable<MonitorEntity> e = base.linqMetaData.Monitor;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.MonitorId == this.monitor.MonitorId) ?? 
							new MonitorEntity();
                    
					base.bizEntity = new GenericBizEntity<MonitorEntity>(base._entity as MonitorEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected MonitorMetaData monitor
        {
            get
            {
                return base.MetaData as MonitorMetaData;
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

        public MonitorService(IAppService appService)
            : base(appService)
        {
        }

        public MonitorService(MonitorMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.monitor = metaData;
        }

        public MonitorService(MonitorEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The MonitorEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<MonitorEntity> entity);
        partial void OnPartialGetMonitors(ref IQueryable<MonitorMetaData> q);
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

        public override MonitorEntity ConvertTo(IMetaData metaData)
        {
            this.monitor = metaData as MonitorMetaData;
			
            this.OnBeforeProcessing(this.monitor);
			
			MonitorEntity e = this.Entity as MonitorEntity;
			e.SetNewFieldValue("MonitorId", this.monitor.MonitorId); 
			e.PackageId = this.monitor.PackageId;
			e.UpdateDate = this.monitor.UpdateDate;
			e.Username = this.monitor.Username;
			
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
		
        public virtual MonitorMetaData ConvertBack()
		{
            this.monitor = this.monitor ?? new MonitorMetaData();			
			
			this.monitor.MonitorId = (System.Int32)base.bizEntity.PrimaryKey;
			this.monitor.CreateDate = base.bizEntity.Entity.CreateDate;
			this.monitor.CreateUser = base.bizEntity.Entity.CreateUser;
			this.monitor.PackageId = base.bizEntity.Entity.PackageId;
			this.monitor.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.monitor.UpdateUser = base.bizEntity.Entity.UpdateUser;
			this.monitor.Username = base.bizEntity.Entity.Username;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.monitor.PackageDeactivated = base.bizEntity.Entity.PackageDeactivated;
			//this.monitor.PackageName = base.bizEntity.Entity.PackageName;
			//this.monitor.PackageUniqueId = base.bizEntity.Entity.PackageUniqueId;

			return this.monitor;
		}
        
		public MonitorMetaData GetMonitor(System.Int32 id)
        {
            return this.GetMonitors().FirstOrDefault(u => u.MonitorId == id);
        }
		
		public virtual IQueryable<MonitorMetaData> GetMonitors()
        {
            IQueryable<MonitorMetaData> q = base.linqMetaData.Monitor
                .Select(c => new MonitorMetaData
                {   
                    MonitorId = c.MonitorId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					PackageId = c.PackageId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					Username = c.Username,
					//	Mapped fields on related field.
					PackageDeactivated = c.Package.Deactivated,
					PackageName = c.Package.Name,
					PackageUniqueId = c.Package.UniqueId,
                }
                )
                ;
				
            this.OnPartialGetMonitors(ref q);

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
            base.bizEntity.AddRules(MonitorEntityRuleContainer.GetGeneratedRules(this.Entity as MonitorEntity));

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

        private MonitorService _monitorService;

        #endregion

        #region PROTECTED PROPERTIES

        internal MonitorService monitorService
        {
            get
            {
                if (this._monitorService == null)
                {
                    this._monitorService = new MonitorService(this);
                }
                return this._monitorService;
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

        public MonitorMetaData GetMonitor(System.Int32 id)
        {
            return this.monitorService.GetMonitor(id);
        }

        public IQueryable<MonitorMetaData> GetMonitors()
        {
            return this.monitorService.GetMonitors();
        }

        public bool Save(MonitorMetaData monitorMetaData)
        {
            return this.monitorService.Save(monitorMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
