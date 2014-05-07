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
    internal partial class ScheduleService : BaseEntityBizService<ScheduleEntity>, IService<ScheduleEntity>
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
                     IQueryable<ScheduleEntity> e = base.linqMetaData.Schedule;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.ScheduleId == this.schedule.ScheduleId) ?? 
							new ScheduleEntity();
                    
					base.bizEntity = new GenericBizEntity<ScheduleEntity>(base._entity as ScheduleEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected ScheduleMetaData schedule
        {
            get
            {
                return base.MetaData as ScheduleMetaData;
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

        public ScheduleService(IAppService appService)
            : base(appService)
        {
        }

        public ScheduleService(ScheduleMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.schedule = metaData;
        }

        public ScheduleService(ScheduleEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The ScheduleEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<ScheduleEntity> entity);
        partial void OnPartialGetSchedules(ref IQueryable<ScheduleMetaData> q);
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

        public override ScheduleEntity ConvertTo(IMetaData metaData)
        {
            this.schedule = metaData as ScheduleMetaData;
			
            this.OnBeforeProcessing(this.schedule);
			
			ScheduleEntity e = this.Entity as ScheduleEntity;
			e.SetNewFieldValue("ScheduleId", this.schedule.ScheduleId); 
			e.Deactivated = this.schedule.Deactivated;
			e.Description = this.schedule.Description;
			e.End = this.schedule.End;
			e.Exclusions = this.schedule.Exclusions;
			e.MisfireInstruction = this.schedule.MisfireInstruction;
			e.Name = this.schedule.Name;
			e.PluginId = this.schedule.PluginId;
			e.RecurrenceRule = this.schedule.RecurrenceRule;
			e.RecurrenceTypeId = this.schedule.RecurrenceTypeId;
			e.Start = this.schedule.Start;
			e.UniqueId = this.schedule.UniqueId;
			e.UpdateDate = this.schedule.UpdateDate;
			
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
		
        public virtual ScheduleMetaData ConvertBack()
		{
            this.schedule = this.schedule ?? new ScheduleMetaData();			
			
			this.schedule.ScheduleId = (System.Int32)base.bizEntity.PrimaryKey;
			this.schedule.CreateDate = base.bizEntity.Entity.CreateDate;
			this.schedule.CreateUser = base.bizEntity.Entity.CreateUser;
			this.schedule.Deactivated = base.bizEntity.Entity.Deactivated;
			this.schedule.Description = base.bizEntity.Entity.Description;
			this.schedule.End = base.bizEntity.Entity.End;
			this.schedule.Exclusions = base.bizEntity.Entity.Exclusions;
			this.schedule.MisfireInstruction = base.bizEntity.Entity.MisfireInstruction;
			this.schedule.Name = base.bizEntity.Entity.Name;
			this.schedule.PluginId = base.bizEntity.Entity.PluginId;
			this.schedule.RecurrenceRule = base.bizEntity.Entity.RecurrenceRule;
			this.schedule.RecurrenceTypeId = base.bizEntity.Entity.RecurrenceTypeId;
			this.schedule.Start = base.bizEntity.Entity.Start;
			this.schedule.UniqueId = base.bizEntity.Entity.UniqueId;
			this.schedule.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.schedule.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.schedule.PluginUniqueId = base.bizEntity.Entity.PluginUniqueId;

			return this.schedule;
		}
        
		public ScheduleMetaData GetSchedule(System.Int32 id)
        {
            return this.GetSchedules().FirstOrDefault(u => u.ScheduleId == id);
        }
		
		public virtual IQueryable<ScheduleMetaData> GetSchedules()
        {
            IQueryable<ScheduleMetaData> q = base.linqMetaData.Schedule
                .Select(c => new ScheduleMetaData
                {   
                    ScheduleId = c.ScheduleId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Deactivated = c.Deactivated,
					Description = c.Description,
					End = c.End,
					Exclusions = c.Exclusions,
					MisfireInstruction = c.MisfireInstruction,
					Name = c.Name,
					PluginId = c.PluginId,
					RecurrenceRule = c.RecurrenceRule,
					RecurrenceTypeId = c.RecurrenceTypeId,
					Start = c.Start,
					UniqueId = c.UniqueId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					PluginUniqueId = c.Plugin.UniqueId,
                }
                )
                ;
				
            this.OnPartialGetSchedules(ref q);

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
            base.bizEntity.AddRules(ScheduleEntityRuleContainer.GetGeneratedRules(this.Entity as ScheduleEntity));

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

        private ScheduleService _scheduleService;

        #endregion

        #region PROTECTED PROPERTIES

        internal ScheduleService scheduleService
        {
            get
            {
                if (this._scheduleService == null)
                {
                    this._scheduleService = new ScheduleService(this);
                }
                return this._scheduleService;
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

        public ScheduleMetaData GetSchedule(System.Int32 id)
        {
            return this.scheduleService.GetSchedule(id);
        }

        public IQueryable<ScheduleMetaData> GetSchedules()
        {
            return this.scheduleService.GetSchedules();
        }

        public bool Save(ScheduleMetaData scheduleMetaData)
        {
            return this.scheduleService.Save(scheduleMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
