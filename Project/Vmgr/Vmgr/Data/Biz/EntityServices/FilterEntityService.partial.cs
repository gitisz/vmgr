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
    internal partial class FilterService : BaseEntityBizService<FilterEntity>, IService<FilterEntity>
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
                     IQueryable<FilterEntity> e = base.linqMetaData.Filter;

                    //  Partial method with which to hook IQueryable expressions from custom code.  Great for adding prefetch paths!
                    this.OnPartialSelecting(ref e);

                    base.OnSelecting(ref e);
					
                    base._entity = e
						.FirstOrDefault(u => u.FilterId == this.filter.FilterId) ?? 
							new FilterEntity();
                    
					base.bizEntity = new GenericBizEntity<FilterEntity>(base._entity as FilterEntity, base.appService);
                }

                return base._entity;
            }
        }

        protected FilterMetaData filter
        {
            get
            {
                return base.MetaData as FilterMetaData;
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

        public FilterService(IAppService appService)
            : base(appService)
        {
        }

        public FilterService(FilterMetaData metaData, IAppService appService)
            : base(appService)
        {
            this.filter = metaData;
        }

        public FilterService(FilterEntity entity, IAppService appService)
            : base(appService)
        {
            if (entity == null)
                throw new NullReferenceException("The FilterEntity argument must not be undefined.");

            this._entity = entity;
        }

        #endregion

        #region PRIVATE METHODS

        partial void OnPartialSelecting(ref IQueryable<FilterEntity> entity);
        partial void OnPartialGetFilters(ref IQueryable<FilterMetaData> q);
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

        public override FilterEntity ConvertTo(IMetaData metaData)
        {
            this.filter = metaData as FilterMetaData;
			
            this.OnBeforeProcessing(this.filter);
			
			FilterEntity e = this.Entity as FilterEntity;
			e.SetNewFieldValue("FilterId", this.filter.FilterId); 
			e.Deactivated = this.filter.Deactivated;
			e.Expression = this.filter.Expression;
			e.FilterType = this.filter.FilterType;
			e.Name = this.filter.Name;
			e.ServerId = this.filter.ServerId;
			e.UpdateDate = this.filter.UpdateDate;
			e.Username = this.filter.Username;
			
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
		
        public virtual FilterMetaData ConvertBack()
		{
            this.filter = this.filter ?? new FilterMetaData();			
			
			this.filter.FilterId = (System.Int32)base.bizEntity.PrimaryKey;
			this.filter.CreateDate = base.bizEntity.Entity.CreateDate;
			this.filter.CreateUser = base.bizEntity.Entity.CreateUser;
			this.filter.Deactivated = base.bizEntity.Entity.Deactivated;
			this.filter.Expression = base.bizEntity.Entity.Expression;
			this.filter.FilterType = base.bizEntity.Entity.FilterType;
			this.filter.Name = base.bizEntity.Entity.Name;
			this.filter.ServerId = base.bizEntity.Entity.ServerId;
			this.filter.UpdateDate = base.bizEntity.Entity.UpdateDate;
			this.filter.UpdateUser = base.bizEntity.Entity.UpdateUser;
			this.filter.Username = base.bizEntity.Entity.Username;
			

			//	TODO: Figure out why this doesn't work... Mapped fields on related field.

			return this.filter;
		}
        
		public FilterMetaData GetFilter(System.Int32 id)
        {
            return this.GetFilters().FirstOrDefault(u => u.FilterId == id);
        }
		
		public virtual IQueryable<FilterMetaData> GetFilters()
        {
            IQueryable<FilterMetaData> q = base.linqMetaData.Filter
                .Select(c => new FilterMetaData
                {   
                    FilterId = c.FilterId,
					
					CreateDate = c.CreateDate,
					CreateUser = c.CreateUser,
					Deactivated = c.Deactivated,
					Expression = c.Expression,
					FilterType = c.FilterType,
					Name = c.Name,
					ServerId = c.ServerId,
					UpdateDate = c.UpdateDate,
					UpdateUser = c.UpdateUser,
					Username = c.Username,
					//	Mapped fields on related field.
                }
                )
                ;
				
            this.OnPartialGetFilters(ref q);

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
            base.bizEntity.AddRules(FilterEntityRuleContainer.GetGeneratedRules(this.Entity as FilterEntity));

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

        private FilterService _filterService;

        #endregion

        #region PROTECTED PROPERTIES

        internal FilterService filterService
        {
            get
            {
                if (this._filterService == null)
                {
                    this._filterService = new FilterService(this);
                }
                return this._filterService;
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

        public FilterMetaData GetFilter(System.Int32 id)
        {
            return this.filterService.GetFilter(id);
        }

        public IQueryable<FilterMetaData> GetFilters()
        {
            return this.filterService.GetFilters();
        }

        public bool Save(FilterMetaData filterMetaData)
        {
            return this.filterService.Save(filterMetaData);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
