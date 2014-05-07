using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Administration;
using System.Security.Principal;

namespace Vmgr.SharePoint
{
    public class FeatureReceiver : SPFeatureReceiver
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void configureNavigation(SPWeb web)
        {
            SPNavigationNodeCollection quickLaunchNodesCollection = web.Navigation.QuickLaunch;
            IList<SPNavigationNode> nodes = quickLaunchNodesCollection.Cast<SPNavigationNode>()
                .Select(t => t)
                .ToList();

            foreach (SPNavigationNode node in nodes)
                quickLaunchNodesCollection.Delete(node);

            string linkDashboard = web.ServerRelativeUrl + "/Default.aspx";
            SPNavigationNode navigationDashboard = new SPNavigationNode("Dashboard", linkDashboard, true);
            navigationDashboard = quickLaunchNodesCollection.AddAsLast(navigationDashboard);
            navigationDashboard.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-dashboard-enabled-32.png");
            navigationDashboard.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-dashboard-disabled-32.png");
            navigationDashboard.Properties.Add("Permission", (int)Permission.PageDashboard);

            navigationDashboard.Update();

            string linkPackageManager = web.ServerRelativeUrl + "/Plugins/PackageManager.aspx";
            SPNavigationNode navigationPackageManager = new SPNavigationNode("Package Manager", linkPackageManager, true);
            navigationPackageManager = quickLaunchNodesCollection.AddAsLast(navigationPackageManager);
            navigationPackageManager.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-packagemgr-enabled-32.png");
            navigationPackageManager.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-packagemgr-disabled-32.png");
            navigationPackageManager.Properties.Add("Permission", (int)Permission.PagePackageManager);
            navigationPackageManager.Update();

            string linkScheduler = web.ServerRelativeUrl + "/Scheduler/Scheduler.aspx";
            SPNavigationNode navigationScheduler = new SPNavigationNode("Scheduler", linkScheduler, true);
            navigationScheduler = quickLaunchNodesCollection.AddAsLast(navigationScheduler);
            navigationScheduler.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-schedule-enabled-32.png");
            navigationScheduler.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-schedule-disabled-32.png");
            navigationScheduler.Properties.Add("Permission", (int)Permission.PageScheduler);
            navigationScheduler.Update();

            string linkJob = web.ServerRelativeUrl + "/Scheduler/Job.aspx";
            SPNavigationNode navigationJob = new SPNavigationNode("History", linkJob, true);
            navigationJob = quickLaunchNodesCollection.AddAsLast(navigationJob);
            navigationJob.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-history-enabled-32.png");
            navigationJob.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-history-disabled-32.png");
            navigationJob.Properties.Add("Permission", (int)Permission.PageJob);
            navigationJob.Update();

            string linkLogs = web.ServerRelativeUrl + "/Administration/Logs.aspx";
            SPNavigationNode navigationLogs = new SPNavigationNode("Logs", linkLogs, true);
            navigationLogs = quickLaunchNodesCollection.AddAsLast(navigationLogs);
            navigationLogs.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-logs-enabled-32.png");
            navigationLogs.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-logs-disabled-32.png");
            navigationLogs.Properties.Add("Permission", (int)Permission.PageLogs);
            navigationLogs.Update();

            string linkSecurity = web.ServerRelativeUrl + "/Administration/Security.aspx";
            SPNavigationNode navigationSecurity = new SPNavigationNode("Security", linkSecurity, true);
            navigationSecurity = quickLaunchNodesCollection.AddAsLast(navigationSecurity);
            navigationSecurity.Properties.Add("IMAGE_URL", "/_layouts/images/Vmgr/nav-security-enabled-32.png");
            navigationSecurity.Properties.Add("IMAGE_DISABLED_URL", "/_layouts/images/Vmgr/nav-security-disabled-32.png");
            navigationSecurity.Properties.Add("Permission", (int)Permission.PageSecurity);
            navigationSecurity.Update();

        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            this.configureNavigation(properties.Feature.Parent as SPWeb);

            SPWeb web = (SPWeb)properties.Feature.Parent;
            web.Files["default.aspx"].CopyTo("default_orig.aspx", true);
            web.Files["default.aspx"].DeleteAllPersonalizationsAllUsers();
            web.Files["default.aspx"].Delete();
            web.Files["Dashboard.aspx"].CopyTo("default.aspx", true);
            web.RootFolder.WelcomePage = "default.aspx";
            web.RootFolder.Update();

        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;

            SPFile defaultFile = web.Files.Cast<SPFile>()
                .Where(f => f.Name == "default_orig.aspx")
                .FirstOrDefault()
                ;

            if (defaultFile != null)
            {
                web.Files["default_orig.aspx"].CopyTo("default.aspx", true);
                web.RootFolder.WelcomePage = "default.aspx";
                web.RootFolder.Update();
            }
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        #endregion

        #region EVENTS

        #endregion
    }
}