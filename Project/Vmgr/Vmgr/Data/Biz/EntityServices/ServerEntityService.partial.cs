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
    internal partial class ServerService : BaseEntityBizService<ServerEntity>, IService<ServerEntity>
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
                     IQueryable<ServerEntity> e = base.linqMetaData.Server;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.ServerId == this.server.ServerId) ?? 
							new ServerEntity();
                    
					base.bizEntity = new GenericBizEntity<ServerEntity>(base._entity as ServerEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected ServerMetaData server
        {
            get
            {
                return base.MetaData as ServerMetaData;
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

        public ServerService(IAppService appService)
            : base(appService)
        {
        }

        public ServerService(ServerMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.server = metaData;
        }

        public ServerService(ServerEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The ServerEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<ServerEntity> entity);
        partial void OnPartialGetServers(ref IQueryable<ServerMetaData> q);
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

        public override ServerEntity ConvertTo(IMetaData metaData)
        {
            this.server = metaData as ServerMetaData;
			
            this.OnBeforeProcessing(this.server);
			
			ServerEntity e = this.Entity as ServerEntity;
			e.SetNewFieldValue("ServerId", this.server.ServerId); 
			e.Description = this.server.Description;
			e.Name = this.server.Name;
			e.RTFqdn = this.server.RTFqdn;
			e.RTPort = this.server.RTPort;
			e.RTProtocol = this.server.RTProtocol;
			e.UniqueId = this.server.UniqueId;
			e.UpdateDate = this.server.UpdateDate;
			e.WSFqdn = this.server.WSFqdn;
			e.WSPort = this.server.WSPort;
			e.WSProtocol = this.server.WSProtocol;
			
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
		
        public virtual ServerMetaData ConvertBack()
		{
            this.server = this.server ?? new ServerMetaData();			
			
			this.server.ServerId = (System.Int32)base.bizEntity.PrimaryKey;
			this.server.CreateDate = base.bizEntity.Entity.CreateDate;
			this.server.CreateUser = base.bizEntity.Entity.CreateUser;
			this.server.Description = base.bizEntity.Entity.Description;
			this.server.Name = base.bizEntity.Entity.Name;
			this.server.RTFqdn = base.bizEntity.Entity.RTFqdn;
			this.server.RTPort = base.bizEntity.Entity.RTPort;
			this.server.RTProtocol = base.bizEntity.Entity.RTProtocol;
			this.server.UniqueId = base.bizEntity.Entity.UniqueId;
			this.server.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.server.UpdateUser = base.bizEntity.Entity.UpdateUser;
			this.server.WSFqdn = base.bizEntity.Entity.WSFqdn;
			this.server.WSPort = base.bizEntity.Entity.WSPort;
			this.server.WSProtocol = base.bizEntity.Entity.WSProtocol;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.server;
		}
        
		public ServerMetaData GetServer(System.Int32 id)
        {
            return this.GetServers().FirstOrDefault(u => u.ServerId == id);
        }
		
		public virtual IQueryable<ServerMetaData> GetServers()
        {
            IQueryable<ServerMetaData> q = base.linqMetaData.Server
                .Select(c => new ServerMetaData
                {   
                    ServerId = c.ServerId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Description = c.Description,
					Name = c.Name,
					RTFqdn = c.RTFqdn,
					RTPort = c.RTPort,
					RTProtocol = c.RTProtocol,
					UniqueId = c.UniqueId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					WSFqdn = c.WSFqdn,
					WSPort = c.WSPort,
					WSProtocol = c.WSProtocol,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetServers(ref q);

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
            base.bizEntity.AddRules(ServerEntityRuleContainer.GetGeneratedRules(this.Entity as ServerEntity));

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

        private ServerService _serverService;

        #endregion

        #region PROTECTED PROPERTIES

        internal ServerService serverService
        {
            get
            {
                if (this._serverService == null)
                {
                    this._serverService = new ServerService(this);
                }
                return this._serverService;
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

        public ServerMetaData GetServer(System.Int32 id)
        {
            return this.serverService.GetServer(id);
        }

        public IQueryable<ServerMetaData> GetServers()
        {
            return this.serverService.GetServers();
        }

        public bool Save(ServerMetaData serverMetaData)
        {
            return this.serverService.Save(serverMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
