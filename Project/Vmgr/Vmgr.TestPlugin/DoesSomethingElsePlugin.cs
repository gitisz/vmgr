using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Vmgr.Data.Biz.Logging;
using Vmgr.Plugins;

namespace Vmgr.TestPlugin
{
    [ServiceContract(Namespace = "http://Vmgr.TestPlugin.DoesSomethingElsePlugin")]
    public interface IDoesSomethingElsePlugin
    {
        [OperationContract]
        string GetMessage();
    }

    [Serializable]
    public class DoesSomethingElsePlugin : BasePlugin, IDoesSomethingElsePlugin
    {
        public override string Name
        {
            get { return "A Plugin That Does Something Else"; }
        }

        public override string Description
        {
            get { return "Does something else."; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("B5485BEC-607F-4E7C-8B8F-62159722BAB3"); }
        }

        public override bool Schedulable
        {
            get { return false; }
        }

        public static ServiceHost doesSomethingElsePluginServiceHost { get; set; }

        public string GetMessage()
        {
            return "Successfully called plugin: A Plugin That Does Something Else";
        }

        public override void Loaded()
        {
            try
            {
                ServiceMetadataBehavior doesSomethingElsePluginServiceBehavior = new ServiceMetadataBehavior();
                doesSomethingElsePluginServiceBehavior.HttpGetEnabled = true;
                doesSomethingElsePluginServiceBehavior.HttpGetUrl = new Uri(string.Format("{0}://localhost:{1}/Vmgr.TestPlugin.DoesSomethingElsePlugin/GetMessage"
                    , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSProtocol)
                    , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSPort)
                    )
                    );

                BasicHttpBinding doesSomethingElsePluginBinding = new BasicHttpBinding();
                doesSomethingElsePluginBinding.MaxReceivedMessageSize = 4194304;
                doesSomethingElsePluginBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                doesSomethingElsePluginServiceHost = new ServiceHost(typeof(DoesSomethingElsePlugin));
                doesSomethingElsePluginServiceHost.Description.Behaviors.Add(doesSomethingElsePluginServiceBehavior);
                doesSomethingElsePluginServiceHost.AddServiceEndpoint(typeof(IDoesSomethingElsePlugin)
                    , doesSomethingElsePluginBinding
                    , new Uri(string.Format("{0}://localhost:{1}/Vmgr.TestPlugin.DoesSomethingElsePlugin/GetMessage"
                        , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSProtocol)
                        , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSPort)
                        )
                        )
                        );
                doesSomethingElsePluginServiceHost.Open();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(string.Format("Failed to initialize service hosts.  Exception: {0}", ex), LogType.Error);
            }
        }

        public override void UnLoaded()
        {
            if (doesSomethingElsePluginServiceHost != null)
                doesSomethingElsePluginServiceHost.Close();
        }
    }
}
