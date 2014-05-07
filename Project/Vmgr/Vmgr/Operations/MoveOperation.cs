using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Messaging;
using Vmgr.Packaging;

namespace Vmgr.Operations
{
    public class MoveOperation : IMoveOperation
    {
        #region PRIVATE PROPERTIES

        private ServerMetaData _server = null;
        private HubConnection _hubConnection = null;
        private IHubProxy _proxy = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected HubConnection hubConnection
        {
            get
            {
                if (this._hubConnection == null)
                {
                    _hubConnection = new HubConnection(string.Format("{0}://{1}:{2}/"
                        , PackageManager.Manage.Server.RTProtocol
                        , PackageManager.Manage.Server.RTFqdn
                        , PackageManager.Manage.Server.RTPort
                        )
                        )
                        ;

                    IHubProxy proxy = _hubConnection.CreateHubProxy("VmgrHub");
                }

                return this._hubConnection;
            }
        }

        protected IHubProxy proxy
        {
            get
            {
                if (this._proxy == null)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                if (hubConnection.State == ConnectionState.Disconnected)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                return this._proxy;
            }
        }
        
        protected ServerMetaData server
        {
            get
            {
                if (this._server == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._server = app.GetServers()
                            .Where(s => s.Name == System.Environment.MachineName)
                            .FirstOrDefault()
                            ;
                    }
                }

                return this._server;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        protected IHubContext context = GlobalHost.ConnectionManager.GetHubContext<VmgrHub>();

        private void OnMoveProgress(MoveProgress moveProgress)
        {
            try
            {
                proxy.Invoke("MoveProgress", moveProgress)
                    .Wait()
                    ;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnMoveProgress failed.", ex, LogType.Error);
            }
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Move(IPackage package, Guid destination, string group)
        {
            /*
             * Unload package.
             */
            ServerMetaData destinationServer = null;

            this.OnMoveProgress(
                new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = "Package received.  Unloading package from source server.",
                        PrimaryTotal = 5,
                        PrimaryValue = 1,
                        ServerUniqueId = this.server.UniqueId,
                    }
                )
                ;

            System.Threading.Thread.Sleep(1000);

            bool unloaded = false;

            try
            {
                PackageManager.Manage.UnloadPackage(package);

                unloaded = true;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to unload package from the source server.  Review the log for additional information.", ex, LogType.Error);

                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = true,
                        Group = group,
                        Message = "Failed to unload package from the source server.  Review the log for additional information.",
                        PrimaryTotal = 5,
                        PrimaryValue = 5,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;
            }


            /*
             * Save package.
             */
            System.Threading.Thread.Sleep(1000);

            bool updated = false;

            if (unloaded)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = "Package unloaded.  Updating package to new destination.",
                        PrimaryTotal = 5,
                        PrimaryValue = 2,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;

                try
                {
                    using (AppService app = new AppService())
                    {
                        destinationServer = app.GetServers()
                            .Where(s => s.UniqueId == destination)
                            .First()
                            ;

                        PackageMetaData p = app.GetPackages()
                            .Where(x => x.UniqueId == package.UniqueId)
                            .First()
                            ;

                        p.ServerId = destinationServer.ServerId;

                        if (!app.Save(p))
                            throw Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                        updated = true;

                        package = p;
                    }
                }
                catch (Exception ex)
                {
                    package = null;

                    Logger.Logs.Log("Failed to update package to new destination .  Review the log for additional information.", ex, LogType.Error);

                    this.OnMoveProgress(
                        new MoveProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to save package to the database.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                            ServerUniqueId = destinationServer.UniqueId,
                        }
                        )
                        ;
                }
            }


            /*
             * Load the package.
             */
            System.Threading.Thread.Sleep(1000);

            bool loaded = false;

            if (updated)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' updated.  Loading the package on destination server.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 3,
                        ServerUniqueId = destinationServer.UniqueId,
                    }
                    )
                    ;

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if (destinationServer.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }

                    ChannelFactory<IPackageOperation> httpFactory = new ChannelFactory<IPackageOperation>(binding
                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/PackageOperation"
                            , destinationServer.WSProtocol
                            , destinationServer.WSFqdn
                            , destinationServer.WSPort
                        )
                        )
                        )
                        ;

                    IPackageOperation packageOperationProxy = httpFactory.CreateChannel();
                    packageOperationProxy.LoadPackage(package);

                    loaded = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to load the package on destination server.  Review the log for additional information.", ex, LogType.Error);

                    this.OnMoveProgress(
                        new MoveProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to load the package on destination server.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                            ServerUniqueId = destinationServer.UniqueId,
                        }
                        )
                        ;
                }
            }

            /*
             * Schedule jobs.
             */
            System.Threading.Thread.Sleep(1000);

            bool scheduled = false;

            if (loaded)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' loaded.  Detecting plugins and initializing schedules.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 4,
                        ServerUniqueId = destinationServer.UniqueId,
                    }
                    )
                    ;

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if (destinationServer.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }

                    ChannelFactory<IScheduleOperation> httpFactory = new ChannelFactory<IScheduleOperation>(binding
                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/ScheduleOperation"
                            , destinationServer.WSProtocol
                            , destinationServer.WSFqdn
                            , destinationServer.WSPort
                        )
                        )
                        )
                        ;

                    IScheduleOperation scheduleOperationProxy = httpFactory.CreateChannel();
                    scheduleOperationProxy.SchedulePackage(package);

                    scheduled = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to detect plugins or initialize schedules.  Review the log for additional information.", ex, LogType.Error);

                    this.OnMoveProgress(
                        new MoveProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to detect plugins or initialize schedules.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                            ServerUniqueId = destinationServer.UniqueId,
                        }
                        )
                        ;
                }
            }

            /*
             * Complete.
             */

            System.Threading.Thread.Sleep(1000);

            if (scheduled)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' successfully uploaded.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 5,
                        ServerUniqueId = destinationServer.UniqueId,
                    }
                    )
                    ;
            }
        }

        public void Load(IPackage package, string group)
        {
            this.OnMoveProgress(
                new MoveProgress
                {
                    IsFaulted = false,
                    Group = group,
                    Message = "Package received.  Saving package to destination server.",
                    PrimaryTotal = 4,
                    PrimaryValue = 1,
                    ServerUniqueId = this.server.UniqueId,
                }
                )
                ;

            /*
             * Save package.
             */
            System.Threading.Thread.Sleep(1000);

            bool updated = false;

            try
            {
                using (AppService app = new AppService())
                {
                    PackageMetaData p = app.GetPackages()
                        .Where(x => x.UniqueId == package.UniqueId)
                        .First()
                        ;

                    p.ServerId = this.server.ServerId;

                    if (!app.Save(p))
                        throw Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                    updated = true;

                    package = p;
                }
            }
            catch (Exception ex)
            {
                package = null;

                Logger.Logs.Log("Failed to update package to new destination.  Review the log for additional information.", ex, LogType.Error);

                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = true,
                        Group = group,
                        Message = "Failed to save package to the database.  Review the log for additional information.",
                        PrimaryTotal = 4,
                        PrimaryValue = 4,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;
            }


            /*
             * Load the package.
             */
            System.Threading.Thread.Sleep(1000);

            bool loaded = false;

            if (updated)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' updated.  Loading the package on destination server.", package.Name),
                        PrimaryTotal = 4,
                        PrimaryValue = 2,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if (this.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }

                    ChannelFactory<IPackageOperation> httpFactory = new ChannelFactory<IPackageOperation>(binding
                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/PackageOperation"
                            , this.server.WSProtocol
                            , this.server.WSFqdn
                            , this.server.WSPort
                        )
                        )
                        )
                        ;

                    IPackageOperation packageOperationProxy = httpFactory.CreateChannel();
                    packageOperationProxy.LoadPackage(package);

                    loaded = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to load the package on destination server.  Review the log for additional information.", ex, LogType.Error);

                    this.OnMoveProgress(
                        new MoveProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to load the package on destination server.  Review the log for additional information.",
                            PrimaryTotal = 4,
                            PrimaryValue = 4,
                            ServerUniqueId = this.server.UniqueId,
                        }
                        )
                        ;

                }
            }

            /*
             * Schedule jobs.
             */
            System.Threading.Thread.Sleep(1000);

            bool scheduled = false;

            if (loaded)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' loaded.  Detecting plugins and initializing schedules.", package.Name),
                        PrimaryTotal = 4,
                        PrimaryValue = 3,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if (this.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }

                    ChannelFactory<IScheduleOperation> httpFactory = new ChannelFactory<IScheduleOperation>(binding
                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/ScheduleOperation"
                            , this.server.WSProtocol
                            , this.server.WSFqdn
                            , this.server.WSPort
                        )
                        )
                        )
                        ;

                    IScheduleOperation scheduleOperationProxy = httpFactory.CreateChannel();
                    scheduleOperationProxy.SchedulePackage(package);

                    scheduled = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to detect plugins or initialize schedules.  Review the log for additional information.", ex, LogType.Error);

                    this.OnMoveProgress(
                        new MoveProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to detect plugins or initialize schedules.  Review the log for additional information.",
                            PrimaryTotal = 4,
                            PrimaryValue = 4,
                            ServerUniqueId = this.server.UniqueId,
                        }
                        )
                        ;
                }
            }

            /*
             * Complete.
             */

            System.Threading.Thread.Sleep(1000);

            if (scheduled)
            {
                this.OnMoveProgress(
                    new MoveProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' successfully uploaded.", package.Name),
                        PrimaryTotal = 4,
                        PrimaryValue = 4,
                        ServerUniqueId = this.server.UniqueId,
                    }
                    )
                    ;
            }
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
