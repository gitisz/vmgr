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
    internal partial class SecurityMembershipService : BaseEntityBizService<SecurityMembershipEntity>, IService<SecurityMembershipEntity>
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
                     IQueryable<SecurityMembershipEntity> e = base.linqMetaData.SecurityMembership;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.SecurityMembershipId == this.securityMembership.SecurityMembershipId) ?? 
							new SecurityMembershipEntity();
                    
					base.bizEntity = new GenericBizEntity<SecurityMembershipEntity>(base._entity as SecurityMembershipEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected SecurityMembershipMetaData securityMembership
        {
            get
            {
                return base.MetaData as SecurityMembershipMetaData;
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

        public SecurityMembershipService(IAppService appService)
            : base(appService)
        {
        }

        public SecurityMembershipService(SecurityMembershipMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.securityMembership = metaData;
        }

        public SecurityMembershipService(SecurityMembershipEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The SecurityMembershipEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<SecurityMembershipEntity> entity);
        partial void OnPartialGetSecurityMemberships(ref IQueryable<SecurityMembershipMetaData> q);
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

        public override SecurityMembershipEntity ConvertTo(IMetaData metaData)
        {
            this.securityMembership = metaData as SecurityMembershipMetaData;
			
            this.OnBeforeProcessing(this.securityMembership);
			
			SecurityMembershipEntity e = this.Entity as SecurityMembershipEntity;
			e.SetNewFieldValue("SecurityMembershipId", this.securityMembership.SecurityMembershipId); 
			e.Account = this.securityMembership.Account;
			e.AccountType = this.securityMembership.AccountType;
			e.Active = this.securityMembership.Active;
			e.SecurityRoleId = this.securityMembership.SecurityRoleId;
			e.UpdateDate = this.securityMembership.UpdateDate;
			
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
		
        public virtual SecurityMembershipMetaData ConvertBack()
		{
            this.securityMembership = this.securityMembership ?? new SecurityMembershipMetaData();			
			
			this.securityMembership.SecurityMembershipId = (System.Int32)base.bizEntity.PrimaryKey;
			this.securityMembership.Account = base.bizEntity.Entity.Account;
			this.securityMembership.AccountType = base.bizEntity.Entity.AccountType;
			this.securityMembership.Active = base.bizEntity.Entity.Active;
			this.securityMembership.CreateDate = base.bizEntity.Entity.CreateDate;
			this.securityMembership.CreateUser = base.bizEntity.Entity.CreateUser;
			this.securityMembership.SecurityRoleId = base.bizEntity.Entity.SecurityRoleId;
			this.securityMembership.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.securityMembership.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.securityMembership.SecurityRoleName = base.bizEntity.Entity.SecurityRoleName;

			return this.securityMembership;
		}
        
		public SecurityMembershipMetaData GetSecurityMembership(System.Int32 id)
        {
            return this.GetSecurityMemberships().FirstOrDefault(u => u.SecurityMembershipId == id);
        }
		
		public virtual IQueryable<SecurityMembershipMetaData> GetSecurityMemberships()
        {
            IQueryable<SecurityMembershipMetaData> q = base.linqMetaData.SecurityMembership
                .Select(c => new SecurityMembershipMetaData
                {   
                    SecurityMembershipId = c.SecurityMembershipId,
					
					Account = c.Account,
					AccountType = c.AccountType,
					Active = c.Active,
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					SecurityRoleId = c.SecurityRoleId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					SecurityRoleName = c.SecurityRole.Name,
                }
                )
                ;
				
            this.OnPartialGetSecurityMemberships(ref q);

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
            base.bizEntity.AddRules(SecurityMembershipEntityRuleContainer.GetGeneratedRules(this.Entity as SecurityMembershipEntity));

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

        private SecurityMembershipService _securityMembershipService;

        #endregion

        #region PROTECTED PROPERTIES

        internal SecurityMembershipService securityMembershipService
        {
            get
            {
                if (this._securityMembershipService == null)
                {
                    this._securityMembershipService = new SecurityMembershipService(this);
                }
                return this._securityMembershipService;
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

        public SecurityMembershipMetaData GetSecurityMembership(System.Int32 id)
        {
            return this.securityMembershipService.GetSecurityMembership(id);
        }

        public IQueryable<SecurityMembershipMetaData> GetSecurityMemberships()
        {
            return this.securityMembershipService.GetSecurityMemberships();
        }

        public bool Save(SecurityMembershipMetaData securityMembershipMetaData)
        {
            return this.securityMembershipService.Save(securityMembershipMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
