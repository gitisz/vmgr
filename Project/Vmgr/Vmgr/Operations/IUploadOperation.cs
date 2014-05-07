using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Vmgr.Operations
{
    [ServiceContract(Namespace = "http://Vmgr.Operations")]
    public interface IUploadOperation
    {
        [OperationContract]
        void Upload(byte[] bytes, string group, bool overwrite);
    }
}
