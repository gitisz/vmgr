using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using Vmgr.AD;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.SharePoint
{
    public class PermissionModule : IHttpModule
    {
        #region PRIVATE PROPERTIES

        public const string VMGR_MEMBERSHIP_LIST_KEY = "VMGR_MEMBERSHIP_LIST_KEY";
        public const string VMGR_ROLE_LIST_KEY = "VMGR_ROLE_LIST_KEY";
        public const string VMGR_ROLE_PERMISSION_LIST_KEY = "VMGR_ROLE_PERMISSION_LIST_KEY";
        public const string PERMISSION_VMGR_MAP_KEY = "PERMISSION_VMGR_MAP_KEY";

        #endregion

        #region PROTECTED PROPERTIES

        protected string siteId
        {
            get
            {
                try
                {
#if TEST
                    return "148104EF-9D9E-4CAC-A322-DAE73973F123";
#else
                    return SPContext.Current.Web.ID.ToString();
#endif
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failure to get web site ID.  Is the SharePoint context available?", ex, LogType.Error);

                    return "00000000-0000-0000-0000-000000000000";
                }
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private static string GetExecutingAssemblyName()
        {
            string executingAssemblyName = string.Empty;

            Assembly assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
                executingAssemblyName = assembly.FullName;
            else
            {
                assembly = Assembly.GetExecutingAssembly();

                if (assembly != null)
                    executingAssemblyName = assembly.FullName;
            }

            return executingAssemblyName;
        }

        public static string GetLocalIpAddress()
        {
            try
            {
                string strHostName = Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                return ipAddress.ToString();
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        private static IList<SecurityMembershipMetaData> GetSecurityMemberships()
        {
            IList<SecurityMembershipMetaData> SecurityMemberships = new List<SecurityMembershipMetaData> { };

            using (AppService app = new AppService(WindowsIdentity.GetCurrent(), DataAccessAdapterFactory.CreateNoLockAdapter()))
            {
                SecurityMemberships = app.GetSecurityMemberships()
                    .Where(s => s.Active == true)
                    .ToList()
                    ;
            }
            return SecurityMemberships;
        }

        private static IList<SecurityMembershipMetaData> GetSecurityMembershipsCache()
        {
            /*
             * SecurityMembership
             */

            Logger.Logs.Log(string.Format("Attempting to get the site membership cache with key: {0}, on server: {1}."
                , PermissionModule.VMGR_MEMBERSHIP_LIST_KEY
                , PermissionModule.GetLocalIpAddress()
                ), LogType.Info);

            IList<SecurityMembershipMetaData> securityMemberships =
                AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_MEMBERSHIP_LIST_KEY) as IList<SecurityMembershipMetaData>;

            if (securityMemberships == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Unable to get the site membership cache with key: {0}, on server: {1}.  A new cache will now be constructed."
                    , PermissionModule.VMGR_MEMBERSHIP_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif
                securityMemberships = PermissionModule.GetSecurityMemberships();

#if DEBUG
                Logger.Logs.Log(string.Format("The following site membership records will be added to the cache with key: {0}, on server: {1}.  Memberships: {2}."
                    , PermissionModule.VMGR_MEMBERSHIP_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    , string.Join(",", securityMemberships.Select(s =>
                    {
                        if (s.AccountType == 1)
                            return string.Format("[SecurityMembershipId: {0}, EID: {1}, SecurityRoleName: {2}]", s.SecurityMembershipId, JsonConvert.DeserializeObject<User>(s.Account).EID, s.SecurityRoleName);

                        return string.Format("[SecurityMembershipId: {0}, Group Name: {1}, SecurityRoleName: {2}]", s.SecurityMembershipId, JsonConvert.DeserializeObject<Group>(s.Account).Name, s.SecurityRoleName);
                    }
                    ).ToArray()
                    )
                    ), LogType.Info);

#endif
                AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_MEMBERSHIP_LIST_KEY, securityMemberships);
            }
            else
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Able to get the site membership cache with key: {0}, on server: {1}.  An existing cache will now be used."
                    , PermissionModule.VMGR_MEMBERSHIP_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif
            }

            return securityMemberships;
        }

        private static IList<SecurityRoleMetaData> GetSecurityRoles()
        {
            IList<SecurityRoleMetaData> securityRoles = new List<SecurityRoleMetaData> { };

            using (AppService app = new AppService(WindowsIdentity.GetCurrent(), DataAccessAdapterFactory.CreateNoLockAdapter()))
            {
                securityRoles = app.GetSecurityRoles()
                    .Where(s => s.Active == true)
                    .ToList()
                    ;
            }

            return securityRoles;
        }

        private static IList<SecurityRoleMetaData> GetSecurityRolesCache()
        {
            /*
             * SecurityRole
             */
#if DEBUG
            Logger.Logs.Log(string.Format("Attempting to get the site role cache with key: {0}, on server: {1}."
                , PermissionModule.VMGR_ROLE_LIST_KEY
                , PermissionModule.GetLocalIpAddress()
                ), LogType.Info);
#endif

            IList<SecurityRoleMetaData> securityRoles =
                AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_LIST_KEY) as IList<SecurityRoleMetaData>;

            if (securityRoles == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Unable to get the site role cache with key: {0}, on server: {1}.  A new cache will now be constructed."
                    , PermissionModule.VMGR_ROLE_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif

                securityRoles = PermissionModule.GetSecurityRoles();

#if DEBUG
                Logger.Logs.Log(string.Format("The following site role records will be added to the cache with key: {0}, on server: {1}.  Roles: {2}."
                    , PermissionModule.VMGR_ROLE_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    , string.Join(",", securityRoles.Select(s => string.Format("[SecurityRoleId: {0}, Name: {1}]", s.SecurityRoleId, s.Name)).ToArray())
                    ), LogType.Info);
#endif

                AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_ROLE_LIST_KEY, securityRoles);
            }
            else
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Able to get the site role cache with key: {0}, on server: {1}.  An existing cache will now be used."
                    , PermissionModule.VMGR_ROLE_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif
            }

            return securityRoles;
        }

        private static IList<SecurityRolePermissionMetaData> GetSecurityRolePermissions()
        {
            IList<SecurityRolePermissionMetaData> securityRolePermissions = new List<SecurityRolePermissionMetaData> { };

            using (AppService app = new AppService(WindowsIdentity.GetCurrent(), DataAccessAdapterFactory.CreateNoLockAdapter()))
            {
                securityRolePermissions = app.GetSecurityRolePermissions()
                    .Where(s => s.Active == true)
                    .ToList()
                    ;
            }

            return securityRolePermissions;
        }

        private static IList<SecurityRolePermissionMetaData> GetSecurityRolePermissionsCache()
        {
            /*
             * SecurityRolePermission
             */
#if DEBUG
            Logger.Logs.Log(string.Format("Attempting to get the site role permission type cache with key: {0}, on server: {1}."
                , PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY
                , PermissionModule.GetLocalIpAddress()
                ), LogType.Info);
#endif

            IList<SecurityRolePermissionMetaData> securityRolePermissions =
                AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY) as IList<SecurityRolePermissionMetaData>;

            if (securityRolePermissions == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Unable to get the site role permission type cache with key: {0}, on server: {1}.  A new cache will now be constructed."
                    , PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif

                securityRolePermissions = PermissionModule.GetSecurityRolePermissions();

#if DEBUG
                Logger.Logs.Log(string.Format("The following site role permission type records will be added to the cache key: {0}, on server: {1}.  Permission Types: {2}."
                    , PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    , string.Join(",", securityRolePermissions.Select(s => string.Format("[SecurityRolePermissionId: {0}, SecurityRoleName: {1}, PermissionName: {2}]", s.SecurityRolePermissionId, s.SecurityRoleName, s.SecurityPermissionName)).ToArray())
                    ), LogType.Info);
#endif

                AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY, securityRolePermissions);
            }
            else
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Able to get the site role permission type cache with key: {0}, on server: {1}.  An existing cache will now be used."
                    , PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif
            }

            return securityRolePermissions;
        }

        private static IList<SecuritySiteMapMetaData> GetsiteMaps()
        {
            IList<SecuritySiteMapMetaData> siteMaps = new List<SecuritySiteMapMetaData> { };

            using (AppService app = new AppService(WindowsIdentity.GetCurrent(), DataAccessAdapterFactory.CreateNoLockAdapter()))
            {
                siteMaps = app.GetSecuritySiteMaps()
                    .Where(s => s.Active == true)
                    .ToList()
                    ;
            }

            return siteMaps;
        }

        private static IList<SecuritySiteMapMetaData> GetSiteMapsCache()
        {
            /*
             * PermissionSiteMap
             */
#if DEBUG
            Logger.Logs.Log(string.Format("Attempting to get the permission type site map cache with key: {0}, on server: {1}."
                , PermissionModule.PERMISSION_VMGR_MAP_KEY
                , PermissionModule.GetLocalIpAddress()
                ), LogType.Info);
#endif

            IList<SecuritySiteMapMetaData> siteMaps =
                AppDomain.CurrentDomain.GetData(PermissionModule.PERMISSION_VMGR_MAP_KEY) as IList<SecuritySiteMapMetaData>;

            if (siteMaps == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Unable to get the permission type site map cache with key: {0}, on server: {1}.  A new cache will now be constructed."
                    , PermissionModule.PERMISSION_VMGR_MAP_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif

                siteMaps = PermissionModule.GetsiteMaps();

#if DEBUG
                Logger.Logs.Log(string.Format("The following site role permission type records will be added to the cache with key: {0}, on server: {1}.  Permission Types: {2}."
                    , PermissionModule.PERMISSION_VMGR_MAP_KEY
                    , PermissionModule.GetLocalIpAddress()
                    , string.Join(",", siteMaps.Select(s => string.Format("[PermissionSiteMapId: {0}, PermissionName: {1}, Value: {2}]", s.SecuritySiteMapId, s.PermissionName, s.Value)).ToArray())
                    ), LogType.Info);
#endif

                AppDomain.CurrentDomain.SetData(PermissionModule.PERMISSION_VMGR_MAP_KEY, siteMaps);
            }
            else
            {

            }
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Able to get the site role permission type cache with key: {0}, on server: {1}.  An existing cache will now be used."
                    , PermissionModule.PERMISSION_VMGR_MAP_KEY
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Warn);
#endif
            }

            return siteMaps;
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.PostAuthenticateRequest += new EventHandler(context_PostAuthenticateRequest);
        }

        public static void CreatePermissionCache()
        {
            if (CacheConfiguration.UseCache)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("The application is configured to use permission caching.  Server IP: {0}"
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Info);
#endif
            }
            else
            {
#if DEBUG
                Logger.Logs.Log(string.Format("The application is NOT configured to use permission caching.  Server IP: {0}"
                    , PermissionModule.GetLocalIpAddress()
                    ), LogType.Info);
#endif
            }

            try
            {
                if (CacheConfiguration.UseCache)
                    PermissionModule.GetSecurityMembershipsCache();
                else
                    PermissionModule.GetSecurityMemberships();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to get security memberships.", ex, LogType.Error);
            }

            try
            {
                if (CacheConfiguration.UseCache)
                    PermissionModule.GetSecurityRolesCache();
                else
                    PermissionModule.GetSecurityRoles();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to get security roles.", ex, LogType.Error);
            }

            try
            {
                if (CacheConfiguration.UseCache)
                    PermissionModule.GetSecurityRolePermissionsCache();
                else
                    PermissionModule.GetSecurityRolePermissions();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to get security permissions.", ex, LogType.Error);
            }

            try
            {
                if (CacheConfiguration.UseCache)
                    PermissionModule.GetSiteMapsCache();
                else
                    PermissionModule.GetsiteMaps();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to get security site maps.", ex, LogType.Error);
            }
        }

        public static void ClearPermissionCache()
        {
            IList<string> cacheServerUrls = new List<string> { };

            using (AppService app = new AppService(WindowsIdentity.GetCurrent(), DataAccessAdapterFactory.CreateNoLockAdapter()))
            {
                string result = app.GetSettings()
                    .Where(s => s.Key == "VMGR_CACHE_SERVER_URLS")
                    .Select(s => s.Value)
                    .FirstOrDefault()
                    ;

                if (!string.IsNullOrEmpty(result))
                {
                    cacheServerUrls.AddRange(result.Split(',').ToList());
                }
            }

            foreach (string url in cacheServerUrls)
            {
                try
                {
                    Impersonation.Impersonate(delegate
                    {
                        ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CertificatePolicy.ValidateRemoteCertificate);

#if DEBUG
                        Logger.Logs.Log(string.Format("Attempting to call web service: {0}."
                            , url
                            ), LogType.Info);
#endif

                        var client = new WebServiceClient<IPermissionsService>(url);
                        client.Channel.ClearPermissionCache();

#if DEBUG
                        Logger.Logs.Log(string.Format("Successfully able to call web service: {0}."
                            , url
                            ), LogType.Info);
#endif

                    }
                    )
                    ;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to call web service.", ex, LogType.Error);
                }
            }
        }

        public static bool GrantPermission(Permission permission)
        {
            IUser user = PermissionModule.GetUser(WindowsIdentity.GetCurrent());

            if (user == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Failed to grant permission using permission type: {0}. The current user was null.", permission), LogType.Warn);
#endif

                return false;
            }

            IList<IGroup> groups = PermissionModule.GetGroupsByMembership(user);

            return PermissionModule.GrantPermission(user, groups, permission);
        }

        public static bool GrantPermission(IUser user, Permission permission)
        {
            if (user == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Failed to grant permission using permission type: {0}. The current user was null.", permission), LogType.Warn);
#endif

                return false;
            }

            IList<IGroup> groups = PermissionModule.GetGroupsByMembership(user);

            return PermissionModule.GrantPermission(user, groups, permission);
        }

        public static bool GrantPermission(IUser user, IList<IGroup> groups, Permission permission)
        {
#if DEBUG
            Logger.Logs.Log(string.Format("Attempting to grant permission using permission type: {0}.", permission), LogType.Info);
#endif

            bool result = false;

            if (user == null)
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Failed to grant permission using permission type: {0}. The current user was null.", permission), LogType.Warn);
#endif

                return false;
            }

            try
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to create a permission cache."), LogType.Info);
#endif
                PermissionModule.CreatePermissionCache();

                if (SPContext.Current != null)
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            if (web.CurrentUser.IsSiteAdmin)
                            {
#if DEBUG
                                Logger.Logs.Log(string.Format("The current user: {0} is a site collection admin, permission is granted.", user.EID), LogType.Info);
#endif

                                return true;
                            }
                        }
                    }
                }

#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to resurrect site memberships cache using key {0}.", PermissionModule.VMGR_MEMBERSHIP_LIST_KEY), LogType.Info);
#endif

                IList<SecurityMembershipMetaData> securityMemberships =
                    AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_MEMBERSHIP_LIST_KEY) as IList<SecurityMembershipMetaData>;

                if (securityMemberships == null)
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Failed to resurrect site membership cache using key {0}.", PermissionModule.VMGR_MEMBERSHIP_LIST_KEY), LogType.Warn);
#endif
                }
                else
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Successfully able to resurrect site membership cache using key {0}.  Active site memberships: {1}."
                        , PermissionModule.VMGR_MEMBERSHIP_LIST_KEY
                    , string.Join(",", securityMemberships.Select(s =>
                    {
                        if (s.AccountType == 1)
                            return string.Format("[SecurityMembershipId: {0}, EID: {1}, SecurityRoleName: {2}]", s.SecurityMembershipId, JsonConvert.DeserializeObject<User>(s.Account).EID, s.SecurityRoleName);

                        return string.Format("[SecurityMembershipId: {0}, Group Name: {1}, SecurityRoleName: {2}]", s.SecurityMembershipId, JsonConvert.DeserializeObject<Group>(s.Account).Name, s.SecurityRoleName);
                    }
                    ).ToArray()
                    )
                    ), LogType.Info);
#endif
                }

#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to resurrect site role cache using key {0}.", PermissionModule.VMGR_ROLE_LIST_KEY), LogType.Info);
#endif

                IList<SecurityRoleMetaData> securityRoles =
                    AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_LIST_KEY) as IList<SecurityRoleMetaData>;

                if (securityRoles == null)
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Failed to resurrect site role cache using key {0}.", PermissionModule.VMGR_ROLE_LIST_KEY), LogType.Warn);
#endif
                }
                else
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Successfully able to resurrect site role cache using key {0}.  Active site roles: {1}."
                        , PermissionModule.VMGR_ROLE_LIST_KEY
                        , string.Join(",", securityRoles.Select(s => s.Name).ToArray())
                        ), LogType.Info);
#endif
                }

#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to resurrect site permission type cache using key {0}.", PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY), LogType.Info);
#endif

                IList<SecurityRolePermissionMetaData> securityRolePermissions =
                    AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY) as IList<SecurityRolePermissionMetaData>;

                if (securityRolePermissions == null)
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Failed to resurrect site permission type cache using key {0}.", PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY), LogType.Warn);
#endif
                }
                else
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Successfully able to resurrect site permission type cache using key {0}.  Active site permission types: {1}."
                        , PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY
                        , string.Join(",", securityRolePermissions.Select(s => s.SecurityPermissionName).ToArray()))
                        , LogType.Info
                        );
#endif
                }

#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to collect my-memberships for user: {0}.", user.EID), LogType.Info);
#endif

                IList<SecurityMembershipMetaData> myMemberships = securityMemberships
                    .Where(s =>
                    {
                        bool r = false;

                        IList<string> gps = groups
                            .Select(g => (ActiveDirectoryHelper.DOMAIN + g.Name).ToUpper())
                            .ToList()
                            ;

                        string eid = string.Empty;
                        string name = string.Empty;

                        if (s.AccountType == 1)
                        {
                            IUser u = JsonConvert.DeserializeObject<User>(s.Account);

                            eid = u.EID;
                        }


                        if (s.AccountType == 2)
                        {
                            IGroup g = JsonConvert.DeserializeObject<Group>(s.Account);

                            name = g.Name;
                        }

                        r = (eid.Equals(WindowsIdentity.GetCurrent().Name.RemoveDomain(), StringComparison.InvariantCultureIgnoreCase) && s.AccountType == 1)
                            || ((gps.Contains(name.RemoveDomain().ToUpper()) && s.AccountType == 2));

                        return r;
                    }
                    )
                    .ToList();

                if (myMemberships.Count == 0)
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Failed to grant permission using permission type: {0}. The current user does not have any my-memberships."
                        , permission
                        ), LogType.Warn);
#endif

                    return false;
                }
                else
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Successfully able to collect my-memberships to the following roles: {0}."
                        , string.Join(",", myMemberships.Select(s => s.SecurityRoleName).ToArray())
                        ), LogType.Info);
#endif
                }

                IList<SecurityRoleMetaData> mySecurityRoles = securityRoles
                    .Where(s => myMemberships.Select(m => m.SecurityRoleId).Contains(s.SecurityRoleId))
                    .ToList()
                    ;

                IList<SecurityRolePermissionMetaData> mySecurityRolePermissions = securityRolePermissions
                    .Where(r => mySecurityRoles.Select(sr => sr.SecurityRoleId).Contains(r.SecurityRoleId))
                    .ToList()
                    ;

                result = mySecurityRolePermissions
                    .Where(r => r.SecurityPermissionId == (int)permission)
                    .Count() > 0;

#if DEBUG
                if (result)
                    Logger.Logs.Log(string.Format("Successfully able to grant permission using permission type: {0}.", permission), LogType.Info);
                else
                    Logger.Logs.Log(string.Format("Failed to grant permission on site: {0}, using permission type: {1}. The current user does not have a membership to a role containing the permission type being evaluated."
                        , permission)
                        , LogType.Warn
                        );
#endif
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to grant permission.", ex, LogType.Error);

                result = false;
            }

            return result;
        }

        public static IUser GetUser(IIdentity identity)
        {
            ActiveDirectoryContext context = new ActiveDirectoryContext();

            IUser user = null;

            string accountName = identity.Name;

            try
            {
                if (accountName.Contains(ActiveDirectoryHelper.DOMAIN, StringComparison.InvariantCultureIgnoreCase))
                    accountName = accountName.ToUpper().Replace(ActiveDirectoryHelper.DOMAIN, "");

#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to find active directory user: {0}.", accountName), LogType.Info);
#endif

                Impersonation.Impersonate(delegate
                {
                    user = context.Users
                        .SelectByProperty(UserSearchableProperties.EID, accountName)
                        .FirstOrDefault();
                }
                 );

#if DEBUG
                if (user != null)
                    Logger.Logs.Log(string.Format("Successfully found active directory user: {0}.", accountName), LogType.Info);
#endif
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to call active directory.", ex, LogType.Error);
            }

            return user;
        }

        public static IList<IGroup> GetGroupsByMembership(IUser user)
        {
            ActiveDirectoryContext adc = new ActiveDirectoryContext();

            IList<IGroup> groups = null;

            try
            {
#if DEBUG
                Logger.Logs.Log(string.Format("Attempting to find active directory groups by user: {0}.", user.EID)
                    , LogType.Info
                    );
#endif

                Impersonation.Impersonate(delegate
                {
                    groups = adc.Groups
                        .SelectByMembership(user)
                        .ToList();
                }
                );

#if DEBUG
                if (groups != null)
                    Logger.Logs.Log(string.Format("Successfully found active directory groups by user: {0}.  The following group memberships were found: {1}."
                        , user.EID
                        , string.Join(",", groups.Select(s => s.Name).ToArray()))
                        , LogType.Info
                        );
#endif
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to call active directory.", ex, LogType.Error);
            }

            return groups;
        }

        #endregion

        #region EVENTS

        void context_BeginRequest(object sender, EventArgs e)
        {
#if DEBUG
            SD.Tools.OrmProfiler.Interceptor.InterceptorCore.Initialize("Vmgr");
#endif
        }

        private void context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {

                string path = HttpContext.Current.Request.Url.AbsolutePath;

                if (path.Contains("/_layouts") || path.StartsWith("/favicon.ico"))
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("The path being evaluated is not relevant to V-Manager security. Path: {0}"
                        , path
                        )
                        , LogType.Info
                        );
#endif

                }
                else
                {
                    if (SPContext.GetContext(HttpContext.Current) != null)
                    {
                        bool featureActivated = false;

                        if (SPContext.Current.Site.ID != Guid.Empty)
                        {
                            SPSecurity.RunWithElevatedPrivileges(() =>
                            {
                                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                {
                                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                                    {
                                        if (web.Features.Cast<SPFeature>()
                                            .Where(t => t.DefinitionId.Equals(new Guid("2FD80FBB-412C-4ECB-8545-FAD981E6EA73")))
                                            .FirstOrDefault() != null)
                                        {
                                            featureActivated = true;
                                        }
                                    }
                                }
                            }
                            )
                            ;
                        }

                        if (featureActivated)
                        {
#if DEBUG
                            Impersonation.Impersonate(delegate
                            {
                                Logger.Logs.Log(string.Format("Started permission evaluation at: {0}."
                                    , DateTime.Now)
                                    , LogType.Info
                                    );
                            }
                            )
                            ;
#endif

                            string webName = string.Empty;

                            SPSecurity.RunWithElevatedPrivileges(() =>
                            {
                                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                {
                                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                                    {
                                        webName = web.Name;
                                    }
                                }
                            }
                            );

#if DEBUG
                            Impersonation.Impersonate(delegate
                            {
                                Logger.Logs.Log(string.Format("Using web {0}."
                                    , webName
                                    )
                                    , LogType.Info
                                    );
                            }
                            )
                            ;
#endif
                            if (!SPContext.Current.Web.UserIsSiteAdmin)
                            {
                                bool permitted = PermissionModule.ValidateSiteMap(path
                                    , string.IsNullOrEmpty(webName) ? "" : webName + "/"
                                    )
                                    ;

                                if (path.Contains("Vmgr/PermissionsService.asmx", StringComparison.InvariantCultureIgnoreCase))
                                    permitted = true;

                                if (path.Contains("Vmgr/PollingService.asmx", StringComparison.InvariantCultureIgnoreCase))
                                    permitted = true;

                                if (path.EndsWith("AccessDenied.aspx", StringComparison.InvariantCultureIgnoreCase))
                                    permitted = true;

#if DEBUG
                                Logger.Logs.Log(string.Format("Completed permission evaluation for site {0} at: {1}. Permitted: {2}"
                                    , this.siteId
                                    , DateTime.Now
                                    , permitted)
                                    , LogType.Info
                                    );
#endif

                                if (!permitted)
                                {
                                    HttpContext.Current.Response.Redirect(path + "/AccessDenied.aspx");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failure in Vmgr.SharePoint.PermissionModule PostAuthenticateRequest.", ex, LogType.Error);
            }
        }

        public static bool ValidateSiteMap(string currentUrl, string vpath)
        {

#if DEBUG
            Logger.Logs.Log(string.Format("Validating site map current URL {0} and virtual path: {1}."
                , currentUrl
                , vpath
                )
                , LogType.Info
                );
#endif

            PermissionModule.CreatePermissionCache();

            IList<SecuritySiteMapMetaData> siteMaps =
                AppDomain.CurrentDomain.GetData(PERMISSION_VMGR_MAP_KEY) as List<SecuritySiteMapMetaData>;

            if (siteMaps == null)
                return false;

            bool permitted = true;

            foreach (SecuritySiteMapMetaData permissionSiteMap in siteMaps)
            {
                string compareUrl = string.Format(permissionSiteMap.Value, vpath);

                if (compareUrl.Equals(currentUrl, StringComparison.InvariantCultureIgnoreCase))
                {
#if DEBUG
                    Logger.Logs.Log(string.Format("Attempting to grant permission to access url: {0} with permission type {1}."
                        , currentUrl
                        , (Permission)permissionSiteMap.SecurityPermissionId)
                        , LogType.Info
                        );
#endif

                    permitted = PermissionModule.GrantPermission((Permission)permissionSiteMap.SecurityPermissionId);

#if DEBUG
                    if (permitted)
                    {
                        Logger.Logs.Log(string.Format("Successfully able to grant permission to access url: {0} with permission type {1}."
                            , currentUrl
                            , (Permission)permissionSiteMap.SecurityPermissionId)
                            , LogType.Info
                            );
                    }
                    else
                    {
                        Logger.Logs.Log(string.Format("Failed to grant permission to access url: {0} with permission type {1}."
                            , currentUrl
                            , (Permission)permissionSiteMap.SecurityPermissionId)
                            , LogType.Warn
                            );
                    }
#endif

                    break;
                }
            }

            return permitted;
        }

        #endregion
    }
}