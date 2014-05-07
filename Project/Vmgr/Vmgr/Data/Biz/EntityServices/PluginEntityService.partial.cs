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
    internal partial class PluginService : BaseEntityBizService<PluginEntity>, IService<PluginEntity>
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
                     IQueryable<PluginEntity> e = base.linqMetaData.Plugin;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.PluginId == this.plugin.PluginId) ?? 
							new PluginEntity();
                    
					base.bizEntity = new GenericBizEntity<PluginEntity>(base._entity as PluginEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected PluginMetaData plugin
        {
            get
            {
                return base.MetaData as PluginMetaData;
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

        public PluginService(IAppService appService)
            : base(appService)
        {
        }

        public PluginService(PluginMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.plugin = metaData;
        }

        public PluginService(PluginEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The PluginEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<PluginEntity> entity);
        partial void OnPartialGetPlugins(ref IQueryable<PluginMetaData> q);
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

        public override PluginEntity ConvertTo(IMetaData metaData)
        {
            this.plugin = metaData as PluginMetaData;
			
            this.OnBeforeProcessing(this.plugin);
			
			PluginEntity e = this.Entity as PluginEntity;
			e.SetNewFieldValue("PluginId", this.plugin.PluginId); 
			e.Description = this.plugin.Description;
			e.Name = this.plugin.Name;
			e.PackageId = this.plugin.PackageId;
			e.Schedulable = this.plugin.Schedulable;
			e.UniqueId = this.plugin.UniqueId;
			e.UpdateDate = this.plugin.UpdateDate;
			
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
		
        public virtual PluginMetaData ConvertBack()
		{
            this.plugin = this.plugin ?? new PluginMetaData();			
			
			this.plugin.PluginId = (System.Int32)base.bizEntity.PrimaryKey;
			this.plugin.CreateDate = base.bizEntity.Entity.CreateDate;
			this.plugin.CreateUser = base.bizEntity.Entity.CreateUser;
			this.plugin.Description = base.bizEntity.Entity.Description;
			this.plugin.Name = base.bizEntity.Entity.Name;
			this.plugin.PackageId = base.bizEntity.Entity.PackageId;
			this.plugin.Schedulable = base.bizEntity.Entity.Schedulable;
			this.plugin.UniqueId = base.bizEntity.Entity.UniqueId;
			this.plugin.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.plugin.UpdateUser = base.bizEntity.Entity.UpdateUser;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.
			//this.plugin.PackageDeactivated = base.bizEntity.Entity.PackageDeactivated;
			//this.plugin.PackageServerId = base.bizEntity.Entity.PackageServerId;
			//this.plugin.PackageUniqueId = base.bizEntity.Entity.PackageUniqueId;

			return this.plugin;
		}
        
		public PluginMetaData GetPlugin(System.Int32 id)
        {
            return this.GetPlugins().FirstOrDefault(u => u.PluginId == id);
        }
		
		public virtual IQueryable<PluginMetaData> GetPlugins()
        {
            IQueryable<PluginMetaData> q = base.linqMetaData.Plugin
                .Select(c => new PluginMetaData
                {   
                    PluginId = c.PluginId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Description = c.Description,
					Name = c.Name,
					PackageId = c.PackageId,
					Schedulable = c.Schedulable,
					UniqueId = c.UniqueId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					//	Mapped fields on related field.
					PackageDeactivated = c.Package.Deactivated,
					PackageServerId = c.Package.ServerId,
					PackageUniqueId = c.Package.UniqueId,
                }
                )
                ;
				
            this.OnPartialGetPlugins(ref q);

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
            base.bizEntity.AddRules(PluginEntityRuleContainer.GetGeneratedRules(this.Entity as PluginEntity));

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

        private PluginService _pluginService;

        #endregion

        #region PROTECTED PROPERTIES

        internal PluginService pluginService
        {
            get
            {
                if (this._pluginService == null)
                {
                    this._pluginService = new PluginService(this);
                }
                return this._pluginService;
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

        public PluginMetaData GetPlugin(System.Int32 id)
        {
            return this.pluginService.GetPlugin(id);
        }

        public IQueryable<PluginMetaData> GetPlugins()
        {
            return this.pluginService.GetPlugins();
        }

        public bool Save(PluginMetaData pluginMetaData)
        {
            return this.pluginService.Save(pluginMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
