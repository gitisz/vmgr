using System.Linq;
using System.Collections.Generic;


#if NET_40

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Net.NetworkInformation;
using Owin;
using Microsoft.Owin.Cors;
using Vmgr.Monitoring;

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

    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
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
