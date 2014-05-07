using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Biz
{
    public interface IValidator
    {
        IList<IValidationRule> ValidationRules
        {
            get;
        }

        bool IsValid
        {
            get;
        }

        /// <summary>
        /// Invokes the validation rule associated with the IValidator.
        /// </summary>
        /// <returns>True if the validation is valid.</returns>
        bool Validate();

        /// <summary>
        /// Gets a list of broken rules.
        /// </summary>
        /// <returns></returns>
        IList<IValidationRule> GetBrokenRules();

        /// <summary>
        /// Invalidates the Validator.
        /// </summary>
        void Invalidate();
    }
} 