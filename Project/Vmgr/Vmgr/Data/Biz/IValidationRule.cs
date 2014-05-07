using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr;

namespace Vmgr.Data.Biz
{
    /// <summary>
    /// BaseRule.  An interface that accommodated passing a typed value with which to evaluate.
    /// </summary>
    public interface IValidationRule<T>
    {
        T Value
        {
            get;
        }
    }

    public interface IValidationRule : IValidationMessage
    {
        /// <summary>
        /// Evaluates the express to determine if the rule is broken.
        /// </summary>
        /// <returns>Returns true if the rule is broken, otherwise returns false.</returns>
        bool ValidateExpression();

    }
} 