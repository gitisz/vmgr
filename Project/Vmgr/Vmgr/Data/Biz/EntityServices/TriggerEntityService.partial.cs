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
    internal partial class TriggerService : BaseEntityBizService<TriggerEntity>, IService<TriggerEntity>
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
                     IQueryable<TriggerEntity> e = base.linqMetaData.Trigger;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.TriggerId == this.trigger.TriggerId) ?? 
							new TriggerEntity();
                    
					base.bizEntity = new GenericBizEntity<TriggerEntity>(base._entity as TriggerEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected TriggerMetaData trigger
        {
            get
            {
                return base.MetaData as TriggerMetaData;
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

        public TriggerService(IAppService appService)
            : base(appService)
        {
        }

        public TriggerService(TriggerMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.trigger = metaData;
        }

        public TriggerService(TriggerEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The TriggerEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<TriggerEntity> entity);
        partial void OnPartialGetTriggers(ref IQueryable<TriggerMetaData> q);
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

        public override TriggerEntity ConvertTo(IMetaData metaData)
        {
            this.trigger = metaData as TriggerMetaData;
			
            this.OnBeforeProcessing(this.trigger);
			
			TriggerEntity e = this.Entity as TriggerEntity;
			e.SetNewFieldValue("TriggerId", this.trigger.TriggerId); 
			e.Ended = this.trigger.Ended;
			e.JobId = this.trigger.JobId;
			e.Mayfire = this.trigger.Mayfire;
			e.Misfire = this.trigger.Misfire;
			e.Nextfire = this.trigger.Nextfire;
			e.Previousfire = this.trigger.Previousfire;
			e.Started = this.trigger.Started;
			e.TriggerKey = this.trigger.TriggerKey;
			e.TriggerKeyGroup = this.trigger.TriggerKeyGroup;
			e.TriggerStatusTypeId = this.trigger.TriggerStatusTypeId;
			e.UpdateDate = this.trigger.UpdateDate;
			
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
		
        public virtual TriggerMetaData ConvertBack()
		{
            this.trigger = this.trigger ?? new TriggerMetaData();			
			
			this.trigger.TriggerId = (System.Int32)base.bizEntity.PrimaryKey;
			this.trigger.CreateDate = base.bizEntity.Entity.CreateDate;
			this.trigger.CreateUser = base.bizEntity.Entity.CreateUser;
			this.trigger.Ended = base.bizEntity.Entity.Ended;
			this.trigger.JobId = base.bizEntity.Entity.JobId;
			this.trigger.Mayfire = base.bizEntity.Entity.Mayfire;
			this.trigger.Misfire = base.bizEntity.Entity.Misfire;
			this.trigger.Nextfire = base.bizEntity.Entity.Nextfire;
			this.trigger.Previousfire = base.bizEntity.Entity.Previousfire;
			this.trigger.Started = base.bizEntity.Entity.Started;
			this.trigger.TriggerKey = base.bizEntity.Entity.TriggerKey;
			this.trigger.TriggerKeyGroup = base.bizEntity.Entity.TriggerKeyGroup;
			this.trigger.TriggerStatusTypeId = base.bizEntity.Entity.TriggerStatusTypeId;
			this.trigger.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.trigger.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.trigger;
		}
        
		public TriggerMetaData GetTrigger(System.Int32 id)
        {
            return this.GetTriggers().FirstOrDefault(u => u.TriggerId == id);
        }
		
		public virtual IQueryable<TriggerMetaData> GetTriggers()
        {
            IQueryable<TriggerMetaData> q = base.linqMetaData.Trigger
                .Select(c => new TriggerMetaData
                {   
                    TriggerId = c.TriggerId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Ended = c.Ended,
					JobId = c.JobId,
					Mayfire = c.Mayfire,
					Misfire = c.Misfire,
					Nextfire = c.Nextfire,
					Previousfire = c.Previousfire,
					Started = c.Started,
					TriggerKey = c.TriggerKey,
					TriggerKeyGroup = c.TriggerKeyGroup,
					TriggerStatusTypeId = c.TriggerStatusTypeId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetTriggers(ref q);

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
            base.bizEntity.AddRules(TriggerEntityRuleContainer.GetGeneratedRules(this.Entity as TriggerEntity));

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

        private TriggerService _triggerService;

        #endregion

        #region PROTECTED PROPERTIES

        internal TriggerService triggerService
        {
            get
            {
                if (this._triggerService == null)
                {
                    this._triggerService = new TriggerService(this);
                }
                return this._triggerService;
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

        public TriggerMetaData GetTrigger(System.Int32 id)
        {
            return this.triggerService.GetTrigger(id);
        }

        public IQueryable<TriggerMetaData> GetTriggers()
        {
            return this.triggerService.GetTriggers();
        }

        public bool Save(TriggerMetaData triggerMetaData)
        {
            return this.triggerService.Save(triggerMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
