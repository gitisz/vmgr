using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;
using Vmgr.Plugins;
using Vmgr.Scheduling;

namespace Vmgr.Operations
{
    public class ScheduleOperation : IScheduleOperation
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        public ScheduleOperation()
        {
        }

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Schedule()
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IList<ISchedule> schedules = app.GetSchedules()
                        .Where(s => !s.Deactivated)
                        .Select(s => s as ISchedule)
                        .ToList()
                        ;

                    foreach (ISchedule schedule in schedules)
                    {
                        try
                        {
                            /*
                             * Call to web service to start the plugin's schedule.  A web service is required 
                             * to reach the appdomain where the plugin is running.
                             */

                            PluginMetaData plugin = app.GetPlugins()
                                .Where(p => p.UniqueId == schedule.PluginUniqueId)
                                .Where(p => !p.PackageDeactivated)
                                .FirstOrDefault()
                                ;

                            if (plugin != null)
                            {
                                BasicHttpBinding binding = new BasicHttpBinding();

                                if (Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                                }

                                ChannelFactory<IScheduler> httpFactory = new ChannelFactory<IScheduler>(binding
                                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Scheduling/{3}/SchedulerManager"
                                    , PackageManager.Manage.Server.WSProtocol
                                    , PackageManager.Manage.Server.WSFqdn
                                    , PackageManager.Manage.Server.WSPort
                                    , plugin.PackageUniqueId)
                                    )
                                    )
                                    ;

                                IScheduler schedulerProxy = httpFactory.CreateChannel();
                                schedulerProxy.Schedule(schedule);
                            }
                            else
                            {
                                Logger.Logs.Log(string.Format("The plugin belongs to a package that has been deactivated, and therefore schedule '{0}' cannot be scheduled.", schedule.Name), LogType.Warn);
                            }
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to schedule package on destination server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log(string.Format("Failed to start schedule: {0}.", schedule.Name), ex, LogType.Error);
                        }
                    }

                    schedules.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to start one ore more schedules.", ex, LogType.Error);
            }
        }

        public void SchedulePackage(IPackage package)
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IPackage pkg = app.GetPackages()
                        .Where(p => p.UniqueId == package.UniqueId)
                        .Where(p => !p.Deactivated)
                        .FirstOrDefault()
                        ;

                    if (pkg == null)
                        throw new ApplicationException("The package provided was not found or it has been deactivated, therefore it cannot be scheduled.");

                    IList<IPlugin> plugins = app.GetPlugins()
                        .Where(p => p.PackageUniqueId == package.UniqueId)
                        .Where(p => p.Schedulable)
                        .Select(p => p as IPlugin)
                        .ToList()
                        ;

                    IList<ISchedule> schedules = app.GetSchedules()
                        .Where(s => !s.Deactivated)
                        .Where(s => plugins.Select(p => p.UniqueId).Contains(s.PluginUniqueId))
                        .Select(s => s as ISchedule)
                        .ToList()
                        ;

                    foreach (ISchedule schedule in schedules)
                    {
                        try
                        {
                            BasicHttpBinding binding = new BasicHttpBinding();

                            if (Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase))
                            {
                                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            }

                            ChannelFactory<IScheduler> httpFactory = new ChannelFactory<IScheduler>(binding
                                , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Scheduling/{3}/SchedulerManager"
                                , PackageManager.Manage.Server.WSProtocol
                                , PackageManager.Manage.Server.WSFqdn
                                , PackageManager.Manage.Server.WSPort
                                , package.UniqueId)
                                )
                                )
                                ;

                            IScheduler schedulerProxy = httpFactory.CreateChannel();
                            schedulerProxy.Schedule(schedule);
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to schedule package on destination server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log(string.Format("Failed to start schedule: {0}.", schedule.Name), ex, LogType.Error);
                        }
                    }

                    plugins.Clear();
                    schedules.Clear();
                    pkg = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to start one ore more schedules.", ex, LogType.Error);
            }
        }

        public void Unschedule()
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IList<ISchedule> schedules = app.GetSchedules()
                        .Where(s => !s.Deactivated)
                        .Select(s => s as ISchedule)
                        .ToList()
                        ;

                    foreach (ISchedule schedule in schedules)
                    {
                        try
                        {
                            /*
                             * Call to web service to stop the plugin's schedule.  A web service is required 
                             * to reach the appdomain where the plugin is running.
                             */

                            PluginMetaData plugin = app.GetPlugins()
                                .Where(p => p.UniqueId == schedule.PluginUniqueId)
                                .FirstOrDefault()
                                ;

                            if (plugin != null)
                            {
                                BasicHttpBinding binding = new BasicHttpBinding();

                                if (Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                                }

                                ChannelFactory<IScheduler> httpFactory = new ChannelFactory<IScheduler>(binding
                                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Scheduling/{3}/SchedulerManager"
                                    , PackageManager.Manage.Server.WSProtocol
                                    , PackageManager.Manage.Server.WSFqdn
                                    , PackageManager.Manage.Server.WSPort
                                    , plugin.PackageUniqueId)
                                    )
                                    )
                                    ;

                                IScheduler schedulerProxy = httpFactory.CreateChannel();
                                schedulerProxy.UnSchedule(schedule);
                            }
                            else
                            {
                                Logger.Logs.Log(string.Format("The plugin for this schedule was not found, and therefore '{0}' cannot be unscheduled.", schedule.Name), LogType.Warn);
                            }
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to schedule package on destination server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log(string.Format("Failed to create schedule: {0}.", schedule.Name), ex, LogType.Error);
                        }
                    }

                    schedules.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to stop one ore more schedules.", ex, LogType.Error);
            }
        }

        public void UnschedulePackage(IPackage package)
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IPackage pkg = app.GetPackages()
                        .Where(p => p.UniqueId == package.UniqueId)
                        .Where(p => !p.Deactivated)
                        .FirstOrDefault()
                        ;

                    if (pkg == null)
                        throw new ApplicationException("The package provided was not found or it has been deactivated, therefore it cannot be unscheduled.");

                    IList<IPlugin> plugins = app.GetPlugins()
                        .Where(p => p.PackageUniqueId == package.UniqueId)
                        .Where(p => p.Schedulable)
                        .Select(p => p as IPlugin)
                        .ToList()
                        ;

                    IList<ISchedule> schedules = app.GetSchedules()
                        .Where(s => plugins.Select(p => p.UniqueId).Contains(s.PluginUniqueId))
                        .Select(s => s as ISchedule)
                        .ToList()
                        ;

                    foreach (ISchedule schedule in schedules)
                    {
                        try
                        {
                            BasicHttpBinding binding = new BasicHttpBinding();

                            if (Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase))
                            {
                                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            }

                            ChannelFactory<IScheduler> httpFactory = new ChannelFactory<IScheduler>(binding
                                , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Scheduling/{3}/SchedulerManager"
                                    , PackageManager.Manage.Server.WSProtocol
                                    , PackageManager.Manage.Server.WSFqdn
                                    , PackageManager.Manage.Server.WSPort
                                    , package.UniqueId)
                                    )
                                    )
                                    ;

                            IScheduler schedulerProxy = httpFactory.CreateChannel();
                            schedulerProxy.UnSchedule(schedule);
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to unschedule package on destination server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log(string.Format("Failed to stop schedule: {0}.", schedule.Name), ex, LogType.Error);
                        }
                    }

                    plugins.Clear();
                    schedules.Clear();
                    pkg = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to stop one ore more schedules.", ex, LogType.Error);
            }
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
