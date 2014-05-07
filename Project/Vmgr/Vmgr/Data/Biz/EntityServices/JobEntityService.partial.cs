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
    internal partial class JobService : BaseEntityBizService<JobEntity>, IService<JobEntity>
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
                     IQueryable<JobEntity> e = base.linqMetaData.Job;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.JobId == this.job.JobId) ?? 
							new JobEntity();
                    
					base.bizEntity = new GenericBizEntity<JobEntity>(base._entity as JobEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected JobMetaData job
        {
            get
            {
                return base.MetaData as JobMetaData;
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

        public JobService(IAppService appService)
            : base(appService)
        {
        }

        public JobService(JobMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.job = metaData;
        }

        public JobService(JobEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The JobEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<JobEntity> entity);
        partial void OnPartialGetJobs(ref IQueryable<JobMetaData> q);
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

        public override JobEntity ConvertTo(IMetaData metaData)
        {
            this.job = metaData as JobMetaData;
			
            this.OnBeforeProcessing(this.job);
			
			JobEntity e = this.Entity as JobEntity;
			e.SetNewFieldValue("JobId", this.job.JobId); 
			e.JobKey = this.job.JobKey;
			e.JobKeyGroup = this.job.JobKeyGroup;
			e.JobStatusTypeId = this.job.JobStatusTypeId;
			e.ScheduleId = this.job.ScheduleId;
			e.UpdateDate = this.job.UpdateDate;
			
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
		
        public virtual JobMetaData ConvertBack()
		{
            this.job = this.job ?? new JobMetaData();			
			
			this.job.JobId = (System.Int32)base.bizEntity.PrimaryKey;
			this.job.CreateDate = base.bizEntity.Entity.CreateDate;
			this.job.CreateUser = base.bizEntity.Entity.CreateUser;
			this.job.JobKey = base.bizEntity.Entity.JobKey;
			this.job.JobKeyGroup = base.bizEntity.Entity.JobKeyGroup;
			this.job.JobStatusTypeId = base.bizEntity.Entity.JobStatusTypeId;
			this.job.ScheduleId = base.bizEntity.Entity.ScheduleId;
			this.job.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.job.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.job.ScheduleName = base.bizEntity.Entity.ScheduleName;
			//this.job.ScheduleUniqueId = base.bizEntity.Entity.ScheduleUniqueId;

			return this.job;
		}
        
		public JobMetaData GetJob(System.Int32 id)
        {
            return this.GetJobs().FirstOrDefault(u => u.JobId == id);
        }
		
		public virtual IQueryable<JobMetaData> GetJobs()
        {
            IQueryable<JobMetaData> q = base.linqMetaData.Job
                .Select(c => new JobMetaData
                {   
                    JobId = c.JobId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					JobKey = c.JobKey,
					JobKeyGroup = c.JobKeyGroup,
					JobStatusTypeId = c.JobStatusTypeId,
					ScheduleId = c.ScheduleId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					ScheduleName = c.Schedule.Name,
					ScheduleUniqueId = c.Schedule.UniqueId,
                }
                )
                ;
				
            this.OnPartialGetJobs(ref q);

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
            base.bizEntity.AddRules(JobEntityRuleContainer.GetGeneratedRules(this.Entity as JobEntity));

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

        private JobService _jobService;

        #endregion

        #region PROTECTED PROPERTIES

        internal JobService jobService
        {
            get
            {
                if (this._jobService == null)
                {
                    this._jobService = new JobService(this);
                }
                return this._jobService;
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

        public JobMetaData GetJob(System.Int32 id)
        {
            return this.jobService.GetJob(id);
        }

        public IQueryable<JobMetaData> GetJobs()
        {
            return this.jobService.GetJobs();
        }

        public bool Save(JobMetaData jobMetaData)
        {
            return this.jobService.Save(jobMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
