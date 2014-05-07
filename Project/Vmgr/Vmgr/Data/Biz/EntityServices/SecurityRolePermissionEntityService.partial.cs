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
    internal partial class SecurityRolePermissionService : BaseEntityBizService<SecurityRolePermissionEntity>, IService<SecurityRolePermissionEntity>
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
                     IQueryable<SecurityRolePermissionEntity> e = base.linqMetaData.SecurityRolePermission;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SecurityRolePermissionId == this.securityRolePermission.SecurityRolePermissionId) ?? 
							new SecurityRolePermissionEntity();
                    
					base.bizEntity = new GenericBizEntity<SecurityRolePermissionEntity>(base._entity as SecurityRolePermissionEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SecurityRolePermissionMetaData securityRolePermission
        {
            get
            {
                return base.MetaData as SecurityRolePermissionMetaData;
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

        public SecurityRolePermissionService(IAppService appService)
            : base(appService)
        {
        }

        public SecurityRolePermissionService(SecurityRolePermissionMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.securityRolePermission = metaData;
        }

        public SecurityRolePermissionService(SecurityRolePermissionEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SecurityRolePermissionEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SecurityRolePermissionEntity> entity);
        partial void OnPartialGetSecurityRolePermissions(ref IQueryable<SecurityRolePermissionMetaData> q);
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

        public override SecurityRolePermissionEntity ConvertTo(IMetaData metaData)
        {
            this.securityRolePermission = metaData as SecurityRolePermissionMetaData;
			
            this.OnBeforeProcessing(this.securityRolePermission);
			
			SecurityRolePermissionEntity e = this.Entity as SecurityRolePermissionEntity;
			e.SetNewFieldValue("SecurityRolePermissionId", this.securityRolePermission.SecurityRolePermissionId); 
			e.Active = this.securityRolePermission.Active;
			e.SecurityPermissionId = this.securityRolePermission.SecurityPermissionId;
			e.SecurityRoleId = this.securityRolePermission.SecurityRoleId;
			e.UpdateDate = this.securityRolePermission.UpdateDate;
			
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
		
        public virtual SecurityRolePermissionMetaData ConvertBack()
		{
            this.securityRolePermission = this.securityRolePermission ?? new SecurityRolePermissionMetaData();			
			
			this.securityRolePermission.SecurityRolePermissionId = (System.Int32)base.bizEntity.PrimaryKey;
			this.securityRolePermission.Active = base.bizEntity.Entity.Active;
			this.securityRolePermission.CreateDate = base.bizEntity.Entity.CreateDate;
			this.securityRolePermission.CreateUser = base.bizEntity.Entity.CreateUser;
			this.securityRolePermission.SecurityPermissionId = base.bizEntity.Entity.SecurityPermissionId;
			this.securityRolePermission.SecurityRoleId = base.bizEntity.Entity.SecurityRoleId;
			this.securityRolePermission.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.securityRolePermission.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.securityRolePermission.SecurityPermissionDescription = base.bizEntity.Entity.SecurityPermissionDescription;
			//this.securityRolePermission.SecurityPermissionName = base.bizEntity.Entity.SecurityPermissionName;
			//this.securityRolePermission.SecurityRoleName = base.bizEntity.Entity.SecurityRoleName;

			return this.securityRolePermission;
		}
        
		public SecurityRolePermissionMetaData GetSecurityRolePermission(System.Int32 id)
        {
            return this.GetSecurityRolePermissions().FirstOrDefault(u => u.SecurityRolePermissionId == id);
        }
		
		public virtual IQueryable<SecurityRolePermissionMetaData> GetSecurityRolePermissions()
        {
            IQueryable<SecurityRolePermissionMetaData> q = base.linqMetaData.SecurityRolePermission
                .Select(c => new SecurityRolePermissionMetaData
                {   
                    SecurityRolePermissionId = c.SecurityRolePermissionId,
					
					Active = c.Active,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					SecurityPermissionId = c.SecurityPermissionId,
					SecurityRoleId = c.SecurityRoleId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					SecurityPermissionDescription = c.SecurityPermission.Description,
					SecurityPermissionName = c.SecurityPermission.Name,
					SecurityRoleName = c.SecurityRole.Name,
                }
                )
                ;
				
            this.OnPartialGetSecurityRolePermissions(ref q);

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
            base.bizEntity.AddRules(SecurityRolePermissionEntityRuleContainer.GetGeneratedRules(this.Entity as SecurityRolePermissionEntity));

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

        private SecurityRolePermissionService _securityRolePermissionService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SecurityRolePermissionService securityRolePermissionService
        {
            get
            {
                if (this._securityRolePermissionService == null)
                {
                    this._securityRolePermissionService = new SecurityRolePermissionService(this);
                }
                return this._securityRolePermissionService;
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

        public SecurityRolePermissionMetaData GetSecurityRolePermission(System.Int32 id)
        {
            return this.securityRolePermissionService.GetSecurityRolePermission(id);
        }

        public IQueryable<SecurityRolePermissionMetaData> GetSecurityRolePermissions()
        {
            return this.securityRolePermissionService.GetSecurityRolePermissions();
        }

        public bool Save(SecurityRolePermissionMetaData securityRolePermissionMetaData)
        {
            return this.securityRolePermissionService.Save(securityRolePermissionMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
