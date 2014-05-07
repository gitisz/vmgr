using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// The interface for an Active Directory group object
    /// </summary>
    public interface IGroup : ISerializable
    {
        /// <summary>
        /// The name of the IGroup
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The Active Directory path of this IGroup object
        /// </summary>
        string ActiveDirectoryPath { get; set; }

        /// <summary>
        /// The directory path that was searched to find this IGroup object
        /// </summary>
        string DirectoryPath { get; set; }

        /// <summary>
        /// Gets a list of IUser objects for this IGroup's membership
        /// </summary>
        List<IUser> Users { get; }
    }
}
