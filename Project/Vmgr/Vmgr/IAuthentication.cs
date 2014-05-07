using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Vmgr
{
    public interface IAuthentication
    {
        string Domain { get; }
        string Username { get; }
        SecureString Password { get; }
        bool CacheUserCredentials { get; }
        bool ? ShowDialog();
    }
}
