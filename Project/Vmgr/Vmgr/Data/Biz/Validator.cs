using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr;

namespace Vmgr.Data.Biz
{
    public class Validator : IValidator
    {
        #region PRIVATE PROPERTIES

        private bool _isValid = true;
        private IList<IValidationRule> _list;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public IList<IValidationRule> ValidationRules
        {
            get
            {
                if (this._list == null)
                {
                    this._list = new List<IValidationRule> { };
                }

                return this._list;
            }
        }

        public bool IsValid
        {
            get { return this._isValid; }
        }

        #endregion

        #region CTOR

        public Validator()
            : this(new List<IValidationRule>())
        {

        }

        public Validator(IList<IValidationRule> validationRules)
        {
            this._list = validationRules;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Validates all the rules in the list.
        /// </summary>
        /// <returns>Returns true if there are no broken rules, otherwise returns false if at least one rule is broken.</returns>
        public bool Validate()
        {
            bool broken = false;

            //  Loops through all rules within a priority, but do not move to next priority unless all pass.
            foreach (PriorityLevelType currentLevel in Enum.GetValues(typeof(PriorityLevelType)))
            {
                foreach (IValidationRule rule in this._list.Where(p => p.PriorityLevel == currentLevel))
                    if (rule.ValidateExpression() && !broken)
                        broken = true;

                if (broken)
                    break;
            }

            return this._isValid = !broken;
        }

        /// <summary>
        /// Gets a list of broken rules.
        /// </summary>
        /// <returns></returns>
        public IList<IValidationRule> GetBrokenRules()
        {
            return this.ValidationRules
                .Where(v => v.IsBroken)
                .ToList()
                ;
        }

        public void Invalidate()
        {
            this._list = null;
        }

        #endregion
    }
}