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
    internal partial class LogService : BaseEntityBizService<LogEntity>, IService<LogEntity>
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
                     IQueryable<LogEntity> e = base.linqMetaData.Log;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.Id == this.log.Id) ?? 
							new LogEntity();
                    
					base.bizEntity = new GenericBizEntity<LogEntity>(base._entity as LogEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected LogMetaData log
        {
            get
            {
                return base.MetaData as LogMetaData;
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

        public LogService(IAppService appService)
            : base(appService)
        {
        }

        public LogService(LogMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.log = metaData;
        }

        public LogService(LogEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The LogEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<LogEntity> entity);
        partial void OnPartialGetLogs(ref IQueryable<LogMetaData> q);
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

        public override LogEntity ConvertTo(IMetaData metaData)
        {
            this.log = metaData as LogMetaData;
			
            this.OnBeforeProcessing(this.log);
			
			LogEntity e = this.Entity as LogEntity;
			e.SetNewFieldValue("Id", this.log.Id); 
			e.ApplicationName = this.log.ApplicationName;
			e.CorrelationId = this.log.CorrelationId;
			e.Exception = this.log.Exception;
			e.Level = this.log.Level;
			e.Logger = this.log.Logger;
			e.Message = this.log.Message;
			e.Server = this.log.Server;
			e.Thread = this.log.Thread;
			e.ThreadId = this.log.ThreadId;
			
			if (this.Entity.IsNew)
            {
                e.CreateUser = base.appService.User.Name;
            }
            else
            {
            }
			
			return e;
        }
		
        public virtual LogMetaData ConvertBack()
		{
            this.log = this.log ?? new LogMetaData();			
			
			this.log.Id = (System.Int32)base.bizEntity.PrimaryKey;
			this.log.ApplicationName = base.bizEntity.Entity.ApplicationName;
			this.log.CorrelationId = base.bizEntity.Entity.CorrelationId;
			this.log.CreateDate = base.bizEntity.Entity.CreateDate;
			this.log.CreateUser = base.bizEntity.Entity.CreateUser;
			this.log.Exception = base.bizEntity.Entity.Exception;
			this.log.Level = base.bizEntity.Entity.Level;
			this.log.Logger = base.bizEntity.Entity.Logger;
			this.log.Message = base.bizEntity.Entity.Message;
			this.log.Server = base.bizEntity.Entity.Server;
			this.log.Thread = base.bizEntity.Entity.Thread;
			this.log.ThreadId = base.bizEntity.Entity.ThreadId;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.log;
		}
        
		public LogMetaData GetLog(System.Int32 id)
        {
            return this.GetLogs().FirstOrDefault(u => u.Id == id);
        }
		
		public virtual IQueryable<LogMetaData> GetLogs()
        {
            IQueryable<LogMetaData> q = base.linqMetaData.Log
                .Select(c => new LogMetaData
                {   
                    Id = c.Id,
					
					ApplicationName = c.ApplicationName,
					CorrelationId = c.CorrelationId,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Exception = c.Exception,
					Level = c.Level,
					Logger = c.Logger,
					Message = c.Message,
					Server = c.Server,
					Thread = c.Thread,
					ThreadId = c.ThreadId,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetLogs(ref q);

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
            base.bizEntity.AddRules(LogEntityRuleContainer.GetGeneratedRules(this.Entity as LogEntity));

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

        private LogService _logService;

        #endregion

        #region PROTECTED PROPERTIES

        internal LogService logService
        {
            get
            {
                if (this._logService == null)
                {
                    this._logService = new LogService(this);
                }
                return this._logService;
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

        public LogMetaData GetLog(System.Int32 id)
        {
            return this.logService.GetLog(id);
        }

        public IQueryable<LogMetaData> GetLogs()
        {
            return this.logService.GetLogs();
        }

        public bool Save(LogMetaData logMetaData)
        {
            return this.logService.Save(logMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
