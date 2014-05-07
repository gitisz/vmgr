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
    internal partial class PackageService : BaseEntityBizService<PackageEntity>, IService<PackageEntity>
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
                     IQueryable<PackageEntity> e = base.linqMetaData.Package;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.PackageId == this.package.PackageId) ?? 
							new PackageEntity();
                    
					base.bizEntity = new GenericBizEntity<PackageEntity>(base._entity as PackageEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected PackageMetaData package
        {
            get
            {
                return base.MetaData as PackageMetaData;
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

        public PackageService(IAppService appService)
            : base(appService)
        {
        }

        public PackageService(PackageMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.package = metaData;
        }

        public PackageService(PackageEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The PackageEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<PackageEntity> entity);
        partial void OnPartialGetPackages(ref IQueryable<PackageMetaData> q);
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

        public override PackageEntity ConvertTo(IMetaData metaData)
        {
            this.package = metaData as PackageMetaData;
			
            this.OnBeforeProcessing(this.package);
			
			PackageEntity e = this.Entity as PackageEntity;
			e.SetNewFieldValue("PackageId", this.package.PackageId); 
			e.Deactivated = this.package.Deactivated;
			e.Description = this.package.Description;
			e.Name = this.package.Name;
			e.Package = this.package.Package;
			e.ServerId = this.package.ServerId;
			e.UniqueId = this.package.UniqueId;
			e.UpdateDate = this.package.UpdateDate;
			
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
		
        public virtual PackageMetaData ConvertBack()
		{
            this.package = this.package ?? new PackageMetaData();			
			
			this.package.PackageId = (System.Int32)base.bizEntity.PrimaryKey;
			this.package.CreateDate = base.bizEntity.Entity.CreateDate;
			this.package.CreateUser = base.bizEntity.Entity.CreateUser;
			this.package.Deactivated = base.bizEntity.Entity.Deactivated;
			this.package.Description = base.bizEntity.Entity.Description;
			this.package.Name = base.bizEntity.Entity.Name;
			this.package.Package = base.bizEntity.Entity.Package;
			this.package.ServerId = base.bizEntity.Entity.ServerId;
			this.package.UniqueId = base.bizEntity.Entity.UniqueId;
			this.package.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.package.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.package.ServerUniqueId = base.bizEntity.Entity.ServerUniqueId;

			return this.package;
		}
        
		public PackageMetaData GetPackage(System.Int32 id)
        {
            return this.GetPackages().FirstOrDefault(u => u.PackageId == id);
        }
		
		public virtual IQueryable<PackageMetaData> GetPackages()
        {
            IQueryable<PackageMetaData> q = base.linqMetaData.Package
                .Select(c => new PackageMetaData
                {   
                    PackageId = c.PackageId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Deactivated = c.Deactivated,
					Description = c.Description,
					Name = c.Name,
					Package = c.Package,
					ServerId = c.ServerId,
					UniqueId = c.UniqueId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					ServerUniqueId = c.Server.UniqueId,
                }
                )
                ;
				
            this.OnPartialGetPackages(ref q);

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
            base.bizEntity.AddRules(PackageEntityRuleContainer.GetGeneratedRules(this.Entity as PackageEntity));

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

        private PackageService _packageService;

        #endregion

        #region PROTECTED PROPERTIES

        internal PackageService packageService
        {
            get
            {
                if (this._packageService == null)
                {
                    this._packageService = new PackageService(this);
                }
                return this._packageService;
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

        public PackageMetaData GetPackage(System.Int32 id)
        {
            return this.packageService.GetPackage(id);
        }

        public IQueryable<PackageMetaData> GetPackages()
        {
            return this.packageService.GetPackages();
        }

        public bool Save(PackageMetaData packageMetaData)
        {
            return this.packageService.Save(packageMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
