using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// Contains classes for parsing various paths from Active Directory
    /// </summary>
    public class ActiveDirectoryHelper
    {
#if DEBUG
        public const string DOMAIN = "ISZLAND\\";
#else
        public const string DOMAIN = "MBULOGIN\\";
#endif
        /// <summary>
        /// Removes the LDAP:// from an Active Directory path
        /// </summary>
        /// <param name="path">The path to clean</param>
        /// <returns>The clean path</returns>
        public static string CleanPath(string path)
        {
            return path.Replace("LDAP://", "");
        }

        /// <summary>
        /// Parses the CN= value out of an entry
        /// </summary>
        /// <param name="entry">The entry to search in</param>
        /// <returns>Returns the CN value of the entry, or blank if no CN values exists</returns>
        public static string ParseCn(string entry)
        {
            Match match = Regex.Match(entry, "CN=(\\w+),");

            if (match.Groups.Count == 2)
                return match.Groups[1].Value;
            else
                return "";
        }

        /// <summary>
        /// Removes domain from eid
        /// </summary>
        /// <param name="path">The path to clean</param>
        /// <returns>The clean path</returns>
        public static string ResolveEidForADQuery(string eid)
        {
            if (eid.IndexOf(ActiveDirectoryHelper.DOMAIN, StringComparison.InvariantCultureIgnoreCase) > -1)
                return eid.ToUpper().Replace(ActiveDirectoryHelper.DOMAIN, "");
            else
                return eid;
        }

        /// <summary>
        /// Returns display name for user, if user is not found then it returns their EID
        /// </summary>
        /// <param name="path">The path to clean</param>
        /// <returns>The clean path</returns>
        public static string GetDisplayName(string eid)
        {
            eid = ResolveEidForADQuery(eid);

            ActiveDirectoryContext adc = new ActiveDirectoryContext();

            IUser user = adc.Users
                    .SelectByProperty(UserSearchableProperties.EID, eid)
                    .FirstOrDefault();
            
            return  user != null ? user.DisplayName : eid;
        }

        /// <summary>
        /// Returns display name for user, if user is not found then it returns their EID
        /// </summary>
        /// <param name="path">The path to clean</param>
        /// <returns>The clean path</returns>
        public static string GetEmailAddress(string eid)
        {
            eid = ResolveEidForADQuery(eid);

            ActiveDirectoryContext adc = new ActiveDirectoryContext();

            IUser user = adc.Users
                    .SelectByProperty(UserSearchableProperties.EID, eid)
                    .FirstOrDefault();

            return user != null ? user.Email : eid + "@dom.com";
        }
    }
}
