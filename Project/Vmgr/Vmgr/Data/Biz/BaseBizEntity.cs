using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Biz
{
    public delegate bool ValidationCallback();

    /// <summary>
    /// Common abstract class for all Growing Technologies business classes that "represent" an LLBLGen pro Adapter DataEntity.
    /// </summary>
    public abstract class BaseBizEntity<T> : BaseBiz where T : EntityBase2, IEntity2
    {

        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        protected abstract string CREATE_USER_ID_COLUMN
        {
            get;
        }

        protected abstract string CREATE_DATE_COLUMN
        {
            get;
        }

        protected abstract string UPDATE_USER_ID_COLUMN
        {
            get;
        }

        protected abstract string UPDATE_DATE_COLUMN
        {
            get;
        }

        /// <summary>
        /// IAppService that the biz class can use for database work.
        /// </summary>
        protected abstract IAppService appService
        {
            get;
        }

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// The Entity that the base biz object uses
        /// </summary>
        public abstract T Entity
        {
            get;
        }

        /// <summary>
        /// Has the object been saved to the database
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                if (this.Entity == null)
                    return true;
                else
                    return Entity.IsNew;
            }
        }

        /// <summary>
        /// The create datetime field
        /// </summary>
        public virtual DateTime CreateDate
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (Entity != null)
                    for (int i = 0; i < Entity.Fields.Count; i++)
                        if (Entity.Fields[i].Name == CREATE_DATE_COLUMN)
                            result = (DateTime)Entity.Fields[i].CurrentValue;

                return result;
            }
        }

        /// <summary>
        /// The datetime the object was last updated
        /// </summary>
        public virtual DateTime? UpdateDate
        {
            get
            {
                DateTime? result = null;

                if (Entity != null)
                    for (int i = 0; i < Entity.Fields.Count; i++)
                        if (Entity.Fields[i].Name == UPDATE_DATE_COLUMN)
                            result = Entity.Fields[i].CurrentValue as DateTime?;

                return result;
            }
        }

        /// <summary>
        /// The user that intially created the object
        /// </summary>
        public virtual int CreateUserId
        {
            get
            {
                int result = 0;
                if (Entity != null)
                    for (int i = 0; i < Entity.Fields.Count; i++)
                        if (Entity.Fields[i].Name == CREATE_USER_ID_COLUMN)
                            result = (int)Entity.Fields[i].CurrentValue;

                return result;
            }
            set
            {
                if (Entity.IsNew == true) //only allow this on new objects
                {
                    if (Entity != null)
                        for (int i = 0; i < Entity.Fields.Count; i++)
                            if (Entity.Fields[i].Name == CREATE_USER_ID_COLUMN)
                                Entity.SetNewFieldValue(i, value);
                }
            }
        }

        /// <summary>
        /// The user that last updated the object
        /// </summary>
        public virtual int UpdateUserId
        {
            get
            {
                int result = 0;
                if (Entity != null)
                    for (int i = 0; i < Entity.Fields.Count; i++)
                        if (Entity.Fields[i].Name == UPDATE_USER_ID_COLUMN)
                            result = (int)Entity.Fields[i].CurrentValue;

                return result;
            }
            set
            {
                if (Entity.IsNew == false) //only allow this on existing objects
                {
                    if (Entity != null)
                        for (int i = 0; i < Entity.Fields.Count; i++)
                            if (Entity.Fields[i].Name == UPDATE_USER_ID_COLUMN)
                                Entity.SetNewFieldValue(i, value);
                }

            }
        }

        /// <summary>
        /// This will be overridden from the calling child class.  The child class's implimentation 
        /// will be called.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual object PrimaryKey
        {
            get
            {
                object result = 0;

                if (Entity != null)
                    if (Entity.PrimaryKeyFields.Count > 0)
                        result = ((IEntityField2)Entity.PrimaryKeyFields[0]).CurrentValue;

                return result;
            }
        }
        
        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="recurse">If true, save related objects.</param>
        /// <returns></returns>
        internal abstract bool Save(bool recurse);

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <returns></returns>
        internal abstract bool Delete();

        #endregion

        #region EVENTS

        #endregion
    }
} 