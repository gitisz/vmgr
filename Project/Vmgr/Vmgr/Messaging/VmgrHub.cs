using System.Linq;
using System.Collections.Generic;


#if NET_40

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Net.NetworkInformation;
using Owin;
using Microsoft.Owin.Cors;
using Vmgr.Monitoring;
using System;
using Microsoft.AspNet.SignalR.Client;
using Vmgr.Packaging;

namespace Vmgr.Messaging
{
    [HubName("VmgrHub")]
    public class VmgrHub : Hub
    {
        public static string GetLocalhostFqdn(string hostName)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            if (string.IsNullOrEmpty(hostName))
                return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
            else
                return string.Format("{0}.{1}", hostName, ipProperties.DomainName);
        }

        public void GlobalMessage(string param)
        {
            Clients.All.addGlobalMessage(param);
        }

        public void Monitor(string key, IList<Monitor> data)
        {
            Clients.All.addMonitor(key, data);
        }

        public void Schedule(string key, Schedule data)
        {
            Clients.All.addSchedule(key, data);
        }

        public void JobHistory(string key, JobHistory data)
        {
            Clients.All.addJobHistory(key, data);
        }

        public void AddToGroup(string group)
        {
            base.Groups.Add(Context.ConnectionId, group);
        }

        public void UploadProgress(UploadProgress progress)
        {
            if (string.IsNullOrEmpty(progress.Group))
                Clients.All.uploadProgress(progress);
            else
                Clients.Group(progress.Group).uploadProgress(progress);
        }

        public void MoveProgress(MoveProgress progress)
        {
            if (string.IsNullOrEmpty(progress.Group))
                Clients.All.moveProgress(progress);
            else
                Clients.Group(progress.Group).moveProgress(progress);
        }
    }

    public class VmgrClient
    {
        private readonly static Lazy<VmgrClient> _instance =
            new Lazy<VmgrClient>(() => new VmgrClient());

        public static VmgrClient Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private HubConnection Connection
        {
            get;
            set;
        }

        public IHubProxy Proxy
        {
            get;
            set;
        }

        private VmgrClient()
        {
            Connection = new HubConnection(string.Format("{0}://{1}:{2}/"
                , PackageManager.Manage.Server.RTProtocol
                , PackageManager.Manage.Server.RTFqdn
                , PackageManager.Manage.Server.RTPort
                )
                )
                ;

            Proxy = Connection.CreateHubProxy("VmgrHub");

            lock (Connection)
            {
                if (Connection.State == ConnectionState.Disconnected)
                {
                    Connection.Start().Wait();
                }
            }
        }
    }

    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("", map =>
            {
                HubConfiguration config = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true,
                    EnableJSONP = true,
                }
                ;
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(config);
            });

        }
    }

}

#endif
