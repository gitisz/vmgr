using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr;

namespace Vmgr.Data.Biz.Validation
{
    /// <summary>
    /// A simple validation rule to allow creating custom rules for tasks such as presenting messages to a user.
    /// </summary>
    public class SimpleValidationRule : IValidationRule
    {
        #region PRIVATE DATA MEMBERS


        #endregion

        #region PROTECTED PROPERTIES

        protected bool _isBroken = false;
        protected string _message = string.Empty;
        protected string _validationGroup = string.Empty;
        protected PriorityLevelType _priorityLevel = PriorityLevelType.Low;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// The message set for the give rule.
        /// </summary>
        public string Message
        {
            get { return this._message; }
        }

        /// <summary>
        /// The message set for the give rule.
        /// </summary>
        public string ValidationGroup
        {
            get { return this._validationGroup; }
        }

        /// <summary>
        /// A rule shall have knowledge of its state.  Returns true if the rule is broken, otherwise returns false.
        /// </summary>
        public bool IsBroken
        {
            get { return this._isBroken; }
            set { this._isBroken = value; }
        }

        /// <summary>
        /// Determines the precedence in which rules are to be evaluated in sets.  This member allows a set of rules to be validated
        /// prior to another set within the same Validator that have a lower prioroty (i.e. to preclude unecessary validation calls to the database).
        /// </summary>
        public PriorityLevelType PriorityLevel
        {
            get { return this._priorityLevel; }
            set { this._priorityLevel = value; }
        }

        #endregion

        #region CTOR

        public SimpleValidationRule()
            : this(string.Empty, false)
        {

        }

        public SimpleValidationRule(string errorMessage, bool isBroken)
        {
            this._message = errorMessage;
            this._isBroken = isBroken;
        }

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public bool ValidateExpression()
        {
            return this.ValidateExpression(null);
        }

        public bool ValidateExpression(IAppService appService)
        {
            return this.IsBroken;
        }

        public static SimpleValidationRule Create(string message)
        {
            return new SimpleValidationRule(message, true);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
