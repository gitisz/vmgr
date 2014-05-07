using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.Validation;

namespace Vmgr.Data.Biz
{
    public delegate void BeforeSaveHandler(IAppService appService);
    public delegate void BeforeDeleteHandler(IAppService appService);
    public delegate void AfterSaveHandler(IAppService appService);
    public delegate void AfterDeleteHandler(IAppService appService);

    /// <summary>
    /// A generic business class used for LLBLGen Adapter Entities.  It is designed to be used with Validation object to validate business rules and save data to the database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericBizEntity<T> : BaseBizEntity<T> where T : EntityBase2, IEntity2, new()
    {

        #region PRIVATE PROPERTIES

        private T _genericEntity = new T();
        private IAppService _service;

        #endregion

        #region PROTECTED PROPERTIES

        protected override string CREATE_DATE_COLUMN
        {
            get { return "CreateDate"; }
        }

        protected override string CREATE_USER_ID_COLUMN
        {
            get { return "CreateUser"; }
        }

        protected override string UPDATE_USER_ID_COLUMN
        {
            get { return "UpdateUser"; }
        }

        protected override string UPDATE_DATE_COLUMN
        {
            get { return "UpdateDate"; }
        }

        /// <summary>
        /// IAppService for the class.
        /// </summary>
        protected override IAppService appService
        {
            get
            {
                return this._service;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public BeforeSaveHandler OnBeforeSave { get; set; }
        public BeforeDeleteHandler OnBeforeDelete { get; set; }
        public AfterSaveHandler OnAfterSave { get; set; }
        public AfterDeleteHandler OnAfterDelete { get; set; }

        /// <summary>
        /// The Entity this object has
        /// </summary>
        public override T Entity
        {
            get { return this._genericEntity; }
        }

        public override string OBJECT_NAME
        {
            get { return this._genericEntity.LLBLGenProEntityName; }
        }

        public override string TRAN_NAME
        {
            get
            {
                string name =
                    this._genericEntity.LLBLGenProEntityName.ToUpper() + "_TRAN_NAME";

                if (name.Length > 32)
                    return name.Substring(0, 32);
                else
                    return name;
            }
        }

        #endregion

        #region CTOR

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="IAppService service"></param>
        public GenericBizEntity(IAppService service)
            : this(null, service)
        {
        }

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="genericEntity"></param>
        /// <param name="adapter"></param>
        public GenericBizEntity(T genericEntity, IAppService service)
        {
            this._genericEntity = genericEntity;
            this._service = service;
        }

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Saves the business object using the adapter specified.
        /// </summary>
        /// <param name="recurse"></param>
        /// <returns></returns>
        internal override bool Save(bool recurse)
        {
            bool result = false;

            try
            {
                if (Entity != null)
                {
                    //	Set the UpdateDate here.
                    if (Entity.IsNew == false)
                    {
                        for (int i = 0; i < Entity.Fields.Count; i++)
                            if (Entity.Fields[i].Name == UPDATE_DATE_COLUMN)
                            {
                                Entity.SetNewFieldValue(i, DateTime.Now);
                                break;
                            }
                    }

                    result = Validate(appService);

                    if (result)
                    {
                        if (this.OnBeforeSave != null)
                            this.OnBeforeSave(appService);

                        result = appService.Adapter.SaveEntity(Entity, true, recurse);
                    }
                }
            }
            catch (System.Exception ex)
            {
                string errorMessage = string.Format("Failed to save {0}.", this.GetType().FullName);

                this.AddRule(SimpleValidationRule.Create(errorMessage));

                result = false;

                //  Objects that set a SavePoint should be responsible for rolling them back.
                if (string.IsNullOrEmpty(appService.LastSavePoint))
                    appService.Adapter.Rollback();

                Logger.Logs.Log(errorMessage, ex, LogType.Error);
            }
            finally
            {
                try
                {
                    if (this.OnAfterSave != null)
                        this.OnAfterSave(appService);
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("An error occurred during OnAfterSave.", ex, LogType.Error);
                }

                this.OnBeforeSave = null;
                this.OnAfterSave = null;
            }

            return result;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal override bool Delete()
        {
            bool result = false;

            try
            {
                if (Entity != null)
                {
                    if (this.OnBeforeDelete != null)
                        this.OnBeforeDelete(appService);

                    result = appService.Adapter.DeleteEntity(Entity);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Failed to delete {0}.", this.GetType().FullName);

                this.AddRule(SimpleValidationRule.Create(errorMessage));

                result = false;

                //  Objects that set a SavePoint should be responsible for rolling them back.
                if (string.IsNullOrEmpty(appService.LastSavePoint))
                    appService.Adapter.Rollback();

                Logger.Logs.Log(errorMessage, ex, LogType.Error);
            }
            finally
            {
                try
                {
                    if (this.OnAfterDelete != null)
                        this.OnAfterDelete(appService);
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("An error occurred during OnAfterDelete.", ex, LogType.Error);
                }

                this.OnBeforeDelete = null;
                this.OnAfterDelete = null;
            }

            return result;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            this.OnAfterDelete = null;
            this.OnAfterSave = null;
            this.OnBeforeDelete = null;
            this.OnBeforeSave = null;
            this.OnValidating = null;

            _genericEntity = null;
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
