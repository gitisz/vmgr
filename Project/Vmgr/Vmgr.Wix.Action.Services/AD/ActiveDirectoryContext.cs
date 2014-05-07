using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// A connection context to an Active Directory instance
    /// </summary>
    public class ActiveDirectoryContext : IDisposable
    {
        /// <summary>
        /// The set of users in Active Directory.  Use this set to select Users.
        /// </summary>
        public UserSet Users;

        /// <summary>
        /// The set of groups in Active Directory.  Use this set to select Groups.
        /// </summary>
        public GroupSet Groups;

        /// <summary>
        /// The directory entry to search against
        /// </summary>
        private DirectoryEntry directoryEntry;

        /// <summary>
        /// Gets the active directory entry in the context
        /// </summary>
        public DirectoryEntry DirectoryEntry
        {
            get
            {
                return this.directoryEntry;
            }
        }

        /// <summary>
        /// Creates a new ActiveDirectoryContext using the default Dominion production LDAP path of LDAP://DC=mbu,DC=ad,DC=dominionnet,DC=com
        /// </summary>
        public ActiveDirectoryContext()
        {
#if DEBUG
            directoryEntry = new DirectoryEntry("LDAP://DC=iszland,DC=com");
#else
            directoryEntry = new DirectoryEntry("LDAP://DC=mbu,DC=ad,DC=dominionnet,DC=com");
#endif
            Users = new UserSet(this);
            Groups = new GroupSet(this);
        }

        /// <summary>
        /// Creates a new ActiveDirectoryContext using the specified LDAP path
        /// </summary>
        /// <param name="path">The LDAP path to search on</param>
        public ActiveDirectoryContext(string path)
        {
            directoryEntry = new DirectoryEntry(path);

            Users = new UserSet(this);
            Groups = new GroupSet(this);
        }

        /// <summary>
        /// Releases all object resources
        /// </summary>
        public void Dispose()
        {
            directoryEntry.Dispose();
        }
    }
}
