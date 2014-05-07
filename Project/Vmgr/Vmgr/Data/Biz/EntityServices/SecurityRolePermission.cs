using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Data.Biz.EntityServices
{
    public class SecurityRolePermission : ISecurityRolePermission 
    {
        public SecurityRoleMetaData SecurityRole { get; set; }
        public IList<int> SelectedPermissions { get; set; }
    }

    public interface ISecurityRolePermission : IMetaData
    {
        SecurityRoleMetaData SecurityRole { get; set; }
        IList<int> SelectedPermissions { get; set; }
    }
}
