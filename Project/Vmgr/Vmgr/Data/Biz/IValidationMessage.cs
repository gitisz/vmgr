using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Data.Biz
{
    public interface IValidationMessage
    {
        string Message
        {
            get;
        }

        string ValidationGroup
        {
            get;
        }

        /// <summary>
        /// A rule shall have knowledge of its state.  Returns true if the rule is broken, otherwise returns false.
        /// </summary>
        bool IsBroken
        {
            get;
        }

        /// <summary>
        /// Determines the precedence in which rules are to be evaluated in sets.  This member allows a set of rules to be validated
        /// prior to another set within the same Validator that have a lower prioroty (i.e. to preclude unecessary validation calls to the database).
        /// </summary>
        PriorityLevelType PriorityLevel
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Determines the precedence in which rules are to be evaluated in sets.
    /// </summary>
    public enum PriorityLevelType : int
    {
        /// <summary>
        /// Rules that shall be evaluated first.
        /// </summary>
        High = 0,

        /// <summary>
        /// Rules that shall be evaluated next.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// Rules that shal be evaluated last.
        /// </summary>
        Low = 3,
    }
}
