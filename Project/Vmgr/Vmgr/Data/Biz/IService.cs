using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Biz
{
    interface IService<T> : IService where T : IEntity2
    {
        T ConvertTo(IMetaData metaData);
    }

    interface IService
    {
        IList<IValidationRule> BrokenRules { get; }

        IList<IService> Services { get; }

        IMetaData MetaData { get; }

        bool Save(IMetaData metaData);

        bool Delete(IMetaData metaData);

        void Invalidate();
    }
} 