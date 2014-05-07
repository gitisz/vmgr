using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace Vmgr.AD
{
    /// <summary>
    /// A enumeration of the searchable IUser properties for strongly-typed searches
    /// </summary>
    public enum UserSearchableProperties
    {
        EID,
        FirstName,
        Initials,
        LastName,
        DisplayName,
        Title,
        Department,
        BusinessUnit,
        BusinessCategory,
        CodeOfConductGroup,
        OfficePhone,
        TieLine,
        MobilePhone,
        Pager,
        OfficeLocation,
        Floor,
        State,
        ActiveDirectoryPath,
        Email,
        ManagerEid
    }

    /// <summary>
    /// The set of AD users and the methods to query them
    /// </summary>
    public class UserSet
    {
        //A mapping of the property enumeration to the names of the properties in Active Directory
        private Dictionary<UserSearchableProperties, string> UserPropertyMapping = new Dictionary<UserSearchableProperties, string>()
        {
            { UserSearchableProperties.EID, "cn" },
            { UserSearchableProperties.FirstName, "givenname" },
            { UserSearchableProperties.Initials, "initials" },
            { UserSearchableProperties.LastName, "sn" },
            { UserSearchableProperties.DisplayName, "displayname" },
            { UserSearchableProperties.Title, "title" },
            { UserSearchableProperties.Department, "department" },
            { UserSearchableProperties.BusinessUnit, "company" },
            { UserSearchableProperties.BusinessCategory, "businesscategory" },
            { UserSearchableProperties.CodeOfConductGroup, "postalcode" },
            { UserSearchableProperties.OfficePhone, "telephonenumber" },
            { UserSearchableProperties.TieLine, "ipphone" },
            { UserSearchableProperties.MobilePhone, "mobile" },
            { UserSearchableProperties.Pager, "pager" },
            { UserSearchableProperties.OfficeLocation, "physicaldeliveryofficename" },
            { UserSearchableProperties.Floor, "postofficebox" },
            { UserSearchableProperties.State, "st" },
            { UserSearchableProperties.ActiveDirectoryPath, "adspath" },
            { UserSearchableProperties.Email, "mail" },
            { UserSearchableProperties.ManagerEid, "manager" }
        };

        /// <summary>
        /// The ActiveDirectoryContext to search against
        /// </summary>
        private ActiveDirectoryContext context;

        /// <summary>
        /// Creates a new UserSet
        /// </summary>
        /// <param name="context">The ActiveDirectoryContext that this UserSet will search on</param>
        public UserSet(ActiveDirectoryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Selects IUsers based on a property type and value
        /// </summary>
        /// <param name="property">The UserSearchableProperty to search on</param>
        /// <param name="value">The value of the property</param>
        /// <returns>A List of IUser objects containing the search results</returns>
        public List<IUser> SelectByProperty(UserSearchableProperties property, string value)
        {
            //For a few of the properties we need to alter the value to accommodate how we store data in our AD
            if (property == UserSearchableProperties.CodeOfConductGroup)
            {
                value = string.Format("CoC: {0}", value);
            }
            else if (property == UserSearchableProperties.TieLine)
            {
                value = string.Format("TIELINE {0}", value);
            }
            else if (property == UserSearchableProperties.Floor)
            {
                value = string.Format("FLOOR: {0}", value);
            }
            else if (property == UserSearchableProperties.ManagerEid)
            {
                value = string.Format("CN={0},OU=Users,OU=Common,{1}", value, ActiveDirectoryHelper.CleanPath(context.DirectoryEntry.Path));
            }

            return this.SelectByProperty(UserPropertyMapping[property], value);
        }

        /// <summary>
        /// Selects IUsers based on membership in a group
        /// </summary>
        /// <param name="groupName">The name of the group</param>
        /// <returns>The list of IUsers in the group, or null of the group does not exist</returns>
        public List<IUser> SelectByGroup(string groupName)
        {
            IGroup group = context.Groups.SelectByName(groupName).FirstOrDefault();

            if (group != null)
                return this.SelectByProperty("memberOf", ActiveDirectoryHelper.CleanPath(group.ActiveDirectoryPath));
            else
                return null;
        }

        /// <summary>
        /// Selects IUsers based on a property name and value
        /// </summary>
        /// <param name="propertyName">The name of the Active Directory property to search on</param>
        /// <param name="value">The value of the property</param>
        /// <returns>A List of IUser objects containing the search results</returns>
        private List<IUser> SelectByProperty(string propertyName, string value)
        {
            List<IUser> users = new List<IUser>();

            //Create a DirectorySearcher
            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the filter
                searcher.Filter = string.Format("(&(objectCategory=user)({0}={1}))", propertyName, value);
                searcher.PageSize = 1000;
                searcher.ReferralChasing = ReferralChasingOption.All;

                //Loop through the results and create User objects
                foreach (SearchResult result in searcher.FindAll())
                {
                    User user = new User
                    {
                        EID =                   result.Properties[UserPropertyMapping[UserSearchableProperties.EID]].Count > 0                  ? result.Properties[UserPropertyMapping[UserSearchableProperties.EID]][0].ToString() : "",
                        FirstName =             result.Properties[UserPropertyMapping[UserSearchableProperties.FirstName]].Count > 0            ? result.Properties[UserPropertyMapping[UserSearchableProperties.FirstName]][0].ToString() : "",
                        Initials =              result.Properties[UserPropertyMapping[UserSearchableProperties.Initials]].Count > 0             ? result.Properties[UserPropertyMapping[UserSearchableProperties.Initials]][0].ToString() : "",
                        LastName =              result.Properties[UserPropertyMapping[UserSearchableProperties.LastName]].Count > 0             ? result.Properties[UserPropertyMapping[UserSearchableProperties.LastName]][0].ToString() : "",
                        DisplayName =           result.Properties[UserPropertyMapping[UserSearchableProperties.DisplayName]].Count > 0          ? result.Properties[UserPropertyMapping[UserSearchableProperties.DisplayName]][0].ToString() : "",
                        Title =                 result.Properties[UserPropertyMapping[UserSearchableProperties.Title]].Count > 0                ? result.Properties[UserPropertyMapping[UserSearchableProperties.Title]][0].ToString() : "",
                        Department =            result.Properties[UserPropertyMapping[UserSearchableProperties.Department]].Count > 0           ? result.Properties[UserPropertyMapping[UserSearchableProperties.Department]][0].ToString() : "",
                        BusinessUnit =          result.Properties[UserPropertyMapping[UserSearchableProperties.BusinessUnit]].Count > 0         ? result.Properties[UserPropertyMapping[UserSearchableProperties.BusinessUnit]][0].ToString() : "",
                        BusinessCategory =      result.Properties[UserPropertyMapping[UserSearchableProperties.BusinessCategory]].Count > 0     ? result.Properties[UserPropertyMapping[UserSearchableProperties.BusinessCategory]][0].ToString() : "",
                        CodeOfConductGroup =    result.Properties[UserPropertyMapping[UserSearchableProperties.CodeOfConductGroup]].Count > 0   ? result.Properties[UserPropertyMapping[UserSearchableProperties.CodeOfConductGroup]][0].ToString().Replace("CoC: ", "") : "",
                        OfficePhone =           result.Properties[UserPropertyMapping[UserSearchableProperties.OfficePhone]].Count > 0          ? result.Properties[UserPropertyMapping[UserSearchableProperties.OfficePhone]][0].ToString() : "",
                        TieLine =               result.Properties[UserPropertyMapping[UserSearchableProperties.TieLine]].Count > 0              ? result.Properties[UserPropertyMapping[UserSearchableProperties.TieLine]][0].ToString().Replace("TIELINE ", "") : "",
                        MobilePhone =           result.Properties[UserPropertyMapping[UserSearchableProperties.MobilePhone]].Count > 0          ? result.Properties[UserPropertyMapping[UserSearchableProperties.MobilePhone]][0].ToString() : "",
                        Pager =                 result.Properties[UserPropertyMapping[UserSearchableProperties.Pager]].Count > 0                ? result.Properties[UserPropertyMapping[UserSearchableProperties.Pager]][0].ToString() : "",
                        OfficeLocation =        result.Properties[UserPropertyMapping[UserSearchableProperties.OfficeLocation]].Count > 0       ? result.Properties[UserPropertyMapping[UserSearchableProperties.OfficeLocation]][0].ToString() : "",
                        Floor =                 result.Properties[UserPropertyMapping[UserSearchableProperties.Floor]].Count > 0                ? result.Properties[UserPropertyMapping[UserSearchableProperties.Floor]][0].ToString().Replace("FLOOR: ", "") : "",
                        State =                 result.Properties[UserPropertyMapping[UserSearchableProperties.State]].Count > 0                ? result.Properties[UserPropertyMapping[UserSearchableProperties.State]][0].ToString() : "",
                        ActiveDirectoryPath =   result.Properties[UserPropertyMapping[UserSearchableProperties.ActiveDirectoryPath]].Count > 0  ? result.Properties[UserPropertyMapping[UserSearchableProperties.ActiveDirectoryPath]][0].ToString() : "",
                        Email =                 result.Properties[UserPropertyMapping[UserSearchableProperties.Email]].Count > 0                ? result.Properties[UserPropertyMapping[UserSearchableProperties.Email]][0].ToString() : "",
                        ManagerEid =            result.Properties[UserPropertyMapping[UserSearchableProperties.ManagerEid]].Count > 0           ? ActiveDirectoryHelper.ParseCn(result.Properties[UserPropertyMapping[UserSearchableProperties.ManagerEid]][0].ToString()) : "",
                        DirectoryPath = this.context.DirectoryEntry.Path
                    };

                    //Split CoC if needed
                    if (user.CodeOfConductGroup.IndexOf(",") != -1)
                    {
                        string[] cocParts = user.CodeOfConductGroup.Split(',');

                        user.CodeOfConductGroup = cocParts[0].Trim();
                        user.CodeOfConductGroupSecondary = cocParts[1].Trim();

                        if (cocParts.Length == 3)
                            user.CodeOfConductGroupTertiary = cocParts[2].Trim();
                    }
                    else
                        user.CodeOfConductGroupSecondary = "";

                    users.Add(user);

                }
            }

            //Sort and return the User list
            users.Sort();
            return users;
        }

        /// <summary>
        /// Checks to see if an EID is in a group
        /// </summary>
        /// <param name="eid">The EID to check</param>
        /// <param name="groupName">The name of the group to check</param>
        /// <returns>True if a user with the given EID is in the group</returns>
        public bool IsEidInGroup(string eid, string groupName)
        {
            IGroup group = context.Groups.SelectByName(groupName).FirstOrDefault();

            if (group == null)
                return false;

            bool found = false;
            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the filter
                searcher.Filter = string.Format("(&(&(objectCategory=user)(cn={0}))(memberOf={1}))", eid, ActiveDirectoryHelper.CleanPath(group.ActiveDirectoryPath));
                searcher.ReferralChasing = ReferralChasingOption.All;

                //Get the result and set the return boolean
                SearchResult result = searcher.FindOne();
                found = result != null;
            }

            return found;
        }

        /// <summary>
        /// Checks to see if an EID is in a group
        /// </summary>
        /// <param name="eid">The EID to check</param>
        /// <param name="group">The IGroup to check</param>
        /// <returns>True if a user with the given EID is in the group</returns>
        public bool IsEidInGroup(string eid, IGroup group)
        {
            bool found = false;
            using (DirectorySearcher searcher = new DirectorySearcher(context.DirectoryEntry))
            {
                //Setup the filter
                searcher.Filter = string.Format("(&(&(objectCategory=user)(cn={0}))(memberOf={1}))", eid, ActiveDirectoryHelper.CleanPath(group.ActiveDirectoryPath));

                //Get the result and set the return boolean
                SearchResult result = searcher.FindOne();
                found = result != null;
            }

            return found;
        }

        /// <summary>
        /// Checks to see if an IUser is in a group
        /// </summary>
        /// <param name="user">The IUser to check</param>
        /// <param name="groupName">The name of the group to check</param>
        /// <returns>>True if the IUser is in the group</returns>
        public bool IsUserInGroup(IUser user, string groupName)
        {
            return this.IsEidInGroup(user.EID, groupName);
        }

        /// <summary>
        /// Checks to see if an IUser is in an IGroup
        /// </summary>
        /// <param name="user">The IUser to check</param>
        /// <param name="groupName">The IGroup to check</param>
        /// <returns>>True if the IUser is in the IGroup</returns>
        public bool IsUserInGroup(IUser user, IGroup group)
        {
            return this.IsEidInGroup(user.EID, group);
        }
    }
}
