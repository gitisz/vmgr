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
    internal partial class SecurityRoleService : BaseEntityBizService<SecurityRoleEntity>, IService<SecurityRoleEntity>
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
                     IQueryable<SecurityRoleEntity> e = base.linqMetaData.SecurityRole;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SecurityRoleId == this.securityRole.SecurityRoleId) ?? 
							new SecurityRoleEntity();
                    
					base.bizEntity = new GenericBizEntity<SecurityRoleEntity>(base._entity as SecurityRoleEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SecurityRoleMetaData securityRole
        {
            get
            {
                return base.MetaData as SecurityRoleMetaData;
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

        public SecurityRoleService(IAppService appService)
            : base(appService)
        {
        }

        public SecurityRoleService(SecurityRoleMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.securityRole = metaData;
        }

        public SecurityRoleService(SecurityRoleEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SecurityRoleEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SecurityRoleEntity> entity);
        partial void OnPartialGetSecurityRoles(ref IQueryable<SecurityRoleMetaData> q);
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

        public override SecurityRoleEntity ConvertTo(IMetaData metaData)
        {
            this.securityRole = metaData as SecurityRoleMetaData;
			
            this.OnBeforeProcessing(this.securityRole);
			
			SecurityRoleEntity e = this.Entity as SecurityRoleEntity;
			e.SetNewFieldValue("SecurityRoleId", this.securityRole.SecurityRoleId); 
			e.Active = this.securityRole.Active;
			e.Description = this.securityRole.Description;
			e.Name = this.securityRole.Name;
			e.UpdateDate = this.securityRole.UpdateDate;
			
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
		
        public virtual SecurityRoleMetaData ConvertBack()
		{
            this.securityRole = this.securityRole ?? new SecurityRoleMetaData();			
			
			this.securityRole.SecurityRoleId = (System.Int32)base.bizEntity.PrimaryKey;
			this.securityRole.Active = base.bizEntity.Entity.Active;
			this.securityRole.CreateDate = base.bizEntity.Entity.CreateDate;
			this.securityRole.CreateUser = base.bizEntity.Entity.CreateUser;
			this.securityRole.Description = base.bizEntity.Entity.Description;
			this.securityRole.Name = base.bizEntity.Entity.Name;
			this.securityRole.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.securityRole.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.securityRole;
		}
        
		public SecurityRoleMetaData GetSecurityRole(System.Int32 id)
        {
            return this.GetSecurityRoles().FirstOrDefault(u => u.SecurityRoleId == id);
        }
		
		public virtual IQueryable<SecurityRoleMetaData> GetSecurityRoles()
        {
            IQueryable<SecurityRoleMetaData> q = base.linqMetaData.SecurityRole
                .Select(c => new SecurityRoleMetaData
                {   
                    SecurityRoleId = c.SecurityRoleId,
					
					Active = c.Active,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Description = c.Description,
					Name = c.Name,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetSecurityRoles(ref q);

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
            base.bizEntity.AddRules(SecurityRoleEntityRuleContainer.GetGeneratedRules(this.Entity as SecurityRoleEntity));

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

        private SecurityRoleService _securityRoleService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SecurityRoleService securityRoleService
        {
            get
            {
                if (this._securityRoleService == null)
                {
                    this._securityRoleService = new SecurityRoleService(this);
                }
                return this._securityRoleService;
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

        public SecurityRoleMetaData GetSecurityRole(System.Int32 id)
        {
            return this.securityRoleService.GetSecurityRole(id);
        }

        public IQueryable<SecurityRoleMetaData> GetSecurityRoles()
        {
            return this.securityRoleService.GetSecurityRoles();
        }

        public bool Save(SecurityRoleMetaData securityRoleMetaData)
        {
            return this.securityRoleService.Save(securityRoleMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
