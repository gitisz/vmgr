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
    internal partial class SecurityPermissionService : BaseEntityBizService<SecurityPermissionEntity>, IService<SecurityPermissionEntity>
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
                     IQueryable<SecurityPermissionEntity> e = base.linqMetaData.SecurityPermission;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SecurityPermissionId == this.securityPermission.SecurityPermissionId) ?? 
							new SecurityPermissionEntity();
                    
					base.bizEntity = new GenericBizEntity<SecurityPermissionEntity>(base._entity as SecurityPermissionEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SecurityPermissionMetaData securityPermission
        {
            get
            {
                return base.MetaData as SecurityPermissionMetaData;
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

        public SecurityPermissionService(IAppService appService)
            : base(appService)
        {
        }

        public SecurityPermissionService(SecurityPermissionMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.securityPermission = metaData;
        }

        public SecurityPermissionService(SecurityPermissionEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SecurityPermissionEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SecurityPermissionEntity> entity);
        partial void OnPartialGetSecurityPermissions(ref IQueryable<SecurityPermissionMetaData> q);
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

        public override SecurityPermissionEntity ConvertTo(IMetaData metaData)
        {
            this.securityPermission = metaData as SecurityPermissionMetaData;
			
            this.OnBeforeProcessing(this.securityPermission);
			
			SecurityPermissionEntity e = this.Entity as SecurityPermissionEntity;
			e.SetNewFieldValue("SecurityPermissionId", this.securityPermission.SecurityPermissionId); 
			e.Description = this.securityPermission.Description;
			e.Name = this.securityPermission.Name;
			
			if (this.Entity.IsNew)
            {
            }
            else
            {
            }
			
			return e;
        }
		
        public virtual SecurityPermissionMetaData ConvertBack()
		{
            this.securityPermission = this.securityPermission ?? new SecurityPermissionMetaData();			
			
			this.securityPermission.SecurityPermissionId = (System.Int32)base.bizEntity.PrimaryKey;
			this.securityPermission.Description = base.bizEntity.Entity.Description;
			this.securityPermission.Name = base.bizEntity.Entity.Name;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.securityPermission;
		}
        
		public SecurityPermissionMetaData GetSecurityPermission(System.Int32 id)
        {
            return this.GetSecurityPermissions().FirstOrDefault(u => u.SecurityPermissionId == id);
        }
		
		public virtual IQueryable<SecurityPermissionMetaData> GetSecurityPermissions()
        {
            IQueryable<SecurityPermissionMetaData> q = base.linqMetaData.SecurityPermission
                .Select(c => new SecurityPermissionMetaData
                {   
                    SecurityPermissionId = c.SecurityPermissionId,
					
					Description = c.Description,
					Name = c.Name,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetSecurityPermissions(ref q);

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
            base.bizEntity.AddRules(SecurityPermissionEntityRuleContainer.GetGeneratedRules(this.Entity as SecurityPermissionEntity));

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

        private SecurityPermissionService _securityPermissionService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SecurityPermissionService securityPermissionService
        {
            get
            {
                if (this._securityPermissionService == null)
                {
                    this._securityPermissionService = new SecurityPermissionService(this);
                }
                return this._securityPermissionService;
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

        public SecurityPermissionMetaData GetSecurityPermission(System.Int32 id)
        {
            return this.securityPermissionService.GetSecurityPermission(id);
        }

        public IQueryable<SecurityPermissionMetaData> GetSecurityPermissions()
        {
            return this.securityPermissionService.GetSecurityPermissions();
        }

        public bool Save(SecurityPermissionMetaData securityPermissionMetaData)
        {
            return this.securityPermissionService.Save(securityPermissionMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
