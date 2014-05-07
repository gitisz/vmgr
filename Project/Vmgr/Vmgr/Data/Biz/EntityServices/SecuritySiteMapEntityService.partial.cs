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
    internal partial class SecuritySiteMapService : BaseEntityBizService<SecuritySiteMapEntity>, IService<SecuritySiteMapEntity>
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
                     IQueryable<SecuritySiteMapEntity> e = base.linqMetaData.SecuritySiteMap;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SecuritySiteMapId == this.securitySiteMap.SecuritySiteMapId) ?? 
							new SecuritySiteMapEntity();
                    
					base.bizEntity = new GenericBizEntity<SecuritySiteMapEntity>(base._entity as SecuritySiteMapEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SecuritySiteMapMetaData securitySiteMap
        {
            get
            {
                return base.MetaData as SecuritySiteMapMetaData;
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

        public SecuritySiteMapService(IAppService appService)
            : base(appService)
        {
        }

        public SecuritySiteMapService(SecuritySiteMapMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.securitySiteMap = metaData;
        }

        public SecuritySiteMapService(SecuritySiteMapEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SecuritySiteMapEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SecuritySiteMapEntity> entity);
        partial void OnPartialGetSecuritySiteMaps(ref IQueryable<SecuritySiteMapMetaData> q);
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

        public override SecuritySiteMapEntity ConvertTo(IMetaData metaData)
        {
            this.securitySiteMap = metaData as SecuritySiteMapMetaData;
			
            this.OnBeforeProcessing(this.securitySiteMap);
			
			SecuritySiteMapEntity e = this.Entity as SecuritySiteMapEntity;
			e.SetNewFieldValue("SecuritySiteMapId", this.securitySiteMap.SecuritySiteMapId); 
			e.Active = this.securitySiteMap.Active;
			e.SecurityPermissionId = this.securitySiteMap.SecurityPermissionId;
			e.UpdateDate = this.securitySiteMap.UpdateDate;
			e.Value = this.securitySiteMap.Value;
			
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
		
        public virtual SecuritySiteMapMetaData ConvertBack()
		{
            this.securitySiteMap = this.securitySiteMap ?? new SecuritySiteMapMetaData();			
			
			this.securitySiteMap.SecuritySiteMapId = (System.Int32)base.bizEntity.PrimaryKey;
			this.securitySiteMap.Active = base.bizEntity.Entity.Active;
			this.securitySiteMap.CreateDate = base.bizEntity.Entity.CreateDate;
			this.securitySiteMap.CreateUser = base.bizEntity.Entity.CreateUser;
			this.securitySiteMap.SecurityPermissionId = base.bizEntity.Entity.SecurityPermissionId;
			this.securitySiteMap.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.securitySiteMap.UpdateUser = base.bizEntity.Entity.UpdateUser;
			this.securitySiteMap.Value = base.bizEntity.Entity.Value;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.securitySiteMap.PermissionName = base.bizEntity.Entity.PermissionName;

			return this.securitySiteMap;
		}
        
		public SecuritySiteMapMetaData GetSecuritySiteMap(System.Int32 id)
        {
            return this.GetSecuritySiteMaps().FirstOrDefault(u => u.SecuritySiteMapId == id);
        }
		
		public virtual IQueryable<SecuritySiteMapMetaData> GetSecuritySiteMaps()
        {
            IQueryable<SecuritySiteMapMetaData> q = base.linqMetaData.SecuritySiteMap
                .Select(c => new SecuritySiteMapMetaData
                {   
                    SecuritySiteMapId = c.SecuritySiteMapId,
					
					Active = c.Active,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					SecurityPermissionId = c.SecurityPermissionId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					Value = c.Value,
					//	Mapped fields on related field.
					PermissionName = c.SecurityPermission.Name,
                }
                )
                ;
				
            this.OnPartialGetSecuritySiteMaps(ref q);

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
            base.bizEntity.AddRules(SecuritySiteMapEntityRuleContainer.GetGeneratedRules(this.Entity as SecuritySiteMapEntity));

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

        private SecuritySiteMapService _securitySiteMapService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SecuritySiteMapService securitySiteMapService
        {
            get
            {
                if (this._securitySiteMapService == null)
                {
                    this._securitySiteMapService = new SecuritySiteMapService(this);
                }
                return this._securitySiteMapService;
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

        public SecuritySiteMapMetaData GetSecuritySiteMap(System.Int32 id)
        {
            return this.securitySiteMapService.GetSecuritySiteMap(id);
        }

        public IQueryable<SecuritySiteMapMetaData> GetSecuritySiteMaps()
        {
            return this.securitySiteMapService.GetSecuritySiteMaps();
        }

        public bool Save(SecuritySiteMapMetaData securitySiteMapMetaData)
        {
            return this.securitySiteMapService.Save(securitySiteMapMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
