﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
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

namespace Vmgr.Data.Biz
{
    public delegate void BeforeValidateHandler(IAppService appService);
    public delegate void AfterValidateHandler(IAppService appService);

    public abstract class BaseBiz
    {
        #region PRIVATE DATA MEMBERS

        private IValidator _validator;
        private LinqMetaData _linqMetaData;

        #endregion

        #region PROTECTED PROPERTIES

        protected LinqMetaData linqMetaData
        {
            get
            {
                if (this._linqMetaData == null)
                {
					//	Used to support RadGrid filtering
                    FunctionMappingStore store = new FunctionMappingStore();
                    store.Add(new FunctionMapping(typeof(String), "ToString", 0, "{0}"));
                    store.Add(new FunctionMapping(typeof(String), "ToUpper", 0, "UPPER({0})"));
                    
					this._linqMetaData = new LinqMetaData(DataAccessAdapterFactory.CreateStandardAdapter(), store);
                }
                return this._linqMetaData;
            }
        }

        protected IValidator Validator
        {
            get
            {
                if(_validator == null)
                {
                    this._validator = new Validator();
                }

                return this._validator;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public BeforeValidateHandler OnValidating { get; set; }

        /// <summary>
        /// name of the object.  Typically used in exceptions or when creating transactions
        /// </summary>
        public virtual string OBJECT_NAME
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual string TRAN_NAME
        {
            get
            {
                string str = base.GetType().Name.ToUpper() + "_TRAN_NAME";
                if (str.Length > 0x20)
                {
                    return str.Substring(0, 0x20);
                }
                return str;
            }
        }

        public static readonly string VALIDATION_SAVE_POINT = "VALIDATION_SAVE_POINT";

        #endregion

        #region CTOR

        public BaseBiz() :
            base()
        {
        }

        #endregion

        #region PUBLIC METHODS

        public bool IsValid
        {
            get { return this.Validator.IsValid; }
        }

        public bool Validate()
        {
            return this.Validate(null);
        }

        public bool Validate(IAppService appService)
        {
            if (this.OnValidating != null)
                this.OnValidating(appService);

            return this.Validator.Validate();
        }

        public void AddRule(IValidationRule validationRule)
        {
            this.Validator.ValidationRules.Add(validationRule);
        }

        public void AddRules(params IValidationRule[] validationRules)
        {
            foreach (IValidationRule validationRule in validationRules)
                this.Validator.ValidationRules.Add(validationRule);
        }

        public void AddRules(IList<IValidationRule> validationRules)
        {
            foreach (IValidationRule validationRule in validationRules)
                this.Validator.ValidationRules.Add(validationRule);
        }

        public IList<IValidationRule> GetBrokenRules()
        {
            return this.Validator.GetBrokenRules();
        }

        public void ClearRules()
        {
            this.Validator.ValidationRules.Clear();
        }

        public virtual void Invalidate()
        {
            this.Validator.Invalidate();

            this._validator = null;
            this._linqMetaData = null;
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region EVENTS

        #endregion
    }
} 