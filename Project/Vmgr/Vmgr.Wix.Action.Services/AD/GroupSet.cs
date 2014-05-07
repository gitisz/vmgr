using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// A enumeration of the IGroup properties for strongly-typed mappings
    /// </summary>
    public enum GroupProperties
    {
        Name,
        ActiveDirectoryPath
    }

    /// <summary>
    /// The set of AD groups and the methods to query them
    /// </summary>
    public class GroupSet
    {
        //A mapping of the property enumeration to the names of the properties in Active Directory
        private Dictionary<GroupProperties, string> GroupPropertyMapping = new Dictionary<GroupProperties, string>()
        {
            { GroupProperties.Name, "cn" },
            { GroupProperties.ActiveDirectoryPath, "adspath" }
        };

        /// <summary>
        /// The ActiveDirectoryContext to search against
        /// </summary>
        private ActiveDirectoryContext context;

        /// <summary>
        /// Creates a new GroupSet
        /// </summary>
        /// <param name="context">The ActiveDirectoryContext that this GroupSet will search on</param>
        public GroupSet(ActiveDirectoryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Selects a group from a name
        /// </summary>
        /// <param name="name">The name of the group to select</param>
        /// <returns>The IGroup object containing the given name</returns>
        public List<IGroup> SelectByName(string name)
        {
            List<IGroup> groups = new List<IGroup>();

            //Create a DirectorySearcher
            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the fitler and query the result
                searcher.Filter = string.Format("(&(objectClass=Group)(cn={0}))", name);
                searcher.PageSize = 1000;
                searcher.ReferralChasing = ReferralChasingOption.All;

                //Loop through the results and add a group for each one
                foreach (SearchResult result in searcher.FindAll())
                {
                    //Create the group object
                    groups.Add(new Group()
                    {
                        Name = result.Properties[GroupPropertyMapping[GroupProperties.Name]].Count > 0 ? result.Properties[GroupPropertyMapping[GroupProperties.Name]][0].ToString() : "",
                        ActiveDirectoryPath = result.Properties[GroupPropertyMapping[GroupProperties.ActiveDirectoryPath]].Count > 0 ? result.Properties[GroupPropertyMapping[GroupProperties.ActiveDirectoryPath]][0].ToString() : "",
                        DirectoryPath = this.context.DirectoryEntry.Path
                    });
                }
            }

            //Return the group
            return groups;
        }

        /// <summary>
        /// Selects a list of IGroups that the given IUser is a memebr of
        /// </summary>
        /// <param name="user">The user to check the membership of</param>
        /// <returns>A list of IGroup objects representing the groups that the given IUser is a member of</returns>
        public List<IGroup> SelectByMembership(IUser user)
        {
            List<IGroup> groups = new List<IGroup>();

            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the filter
                searcher.Filter = string.Format("(&(objectCategory=group)(member={0}))", ActiveDirectoryHelper.CleanPath(user.ActiveDirectoryPath));
                searcher.PageSize = 1000;
                searcher.ReferralChasing = ReferralChasingOption.All;

                //Loop through the results and create group objects
                foreach (SearchResult result in searcher.FindAll())
                {
                    Group group = new Group()
                    {
                        Name =                  result.Properties[GroupPropertyMapping[GroupProperties.Name]].Count > 0                   ? result.Properties[GroupPropertyMapping[GroupProperties.Name]][0].ToString() : "",
                        ActiveDirectoryPath =   result.Properties[GroupPropertyMapping[GroupProperties.ActiveDirectoryPath]].Count > 0    ? result.Properties[GroupPropertyMapping[GroupProperties.ActiveDirectoryPath]][0].ToString() : "",
                        DirectoryPath = this.context.DirectoryEntry.Path
                    };

                    groups.Add(group);
                }
            }

            return groups;
        }

        /// <summary>
        /// Selects a list of IGroups that the IUser with the given EID is a memebr of
        /// </summary>
        /// <param name="eid">The EID of the user to check the membership of</param>
        /// <returns>A list of IGroup objects representing the groups that the IUser with the given EID is a member of</returns>
        public List<IGroup> SelectByMembership(string eid)
        {
            IUser user = context.Users.SelectByProperty(UserSearchableProperties.EID, eid).FirstOrDefault();

            if (user == null)
                return null;

            return this.SelectByMembership(user);
        }

        /// <summary>
        /// Selects a lists of group names by EID
        /// </summary>
        /// <param name="eid">The EID of the user to check the membership of</param>
        /// <returns>A list of names of the groups that the IUser with the given EID is a member of</returns>
        public List<string> SelectGroupNamesByMembership(string eid)
        {
            List<string> groupNames = new List<string>();

            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the filter
                searcher.Filter = string.Format("(&(objectCategory=group)(member=CN={0},OU=Users,OU=Common,{1}))", eid, ActiveDirectoryHelper.CleanPath(context.DirectoryEntry.Path));
                searcher.PropertiesToLoad.Add(GroupPropertyMapping[GroupProperties.Name]);
                searcher.PageSize = 1000;
                searcher.ReferralChasing = ReferralChasingOption.All;

                //Loop through the results and store the group names
                foreach (SearchResult result in searcher.FindAll())
                {
                    groupNames.Add(result.Properties[GroupPropertyMapping[GroupProperties.Name]][0].ToString());
                }
            }

            return groupNames;
        }
    }
}
