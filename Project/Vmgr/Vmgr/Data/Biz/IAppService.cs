using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Security.Principal;

namespace Vmgr.Data.Biz
{
    public interface IAppService : IDisposable
    {
        IDataAccessAdapter Adapter { get; }

        void CreateTransaction(bool keepConnectionOption);

        void SaveTransaction(string transactionName);

        string LastSavePoint { get; }

        IIdentity User { get; }

    }
}
 