using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Vmgr.Data.Biz.Logging;
using Vmgr.Plugins;

namespace Vmgr.TestPlugin
{
    [ServiceContract(Namespace = "http://Vmgr.TestPlugin.DoesSomethingEvenBetterPlugin")]
    public interface IDoesSomethingEvenBetterPlugin
    {
        [OperationContract]
        string GetMessage();
    }

    [Serializable]
    public class DoesSomethingEvenBetterPlugin : BasePlugin, IDoesSomethingEvenBetterPlugin
    {
        public override string Name
        {
            get { return "A Plugin That Does Something Even Better"; }
        }

        public override string Description
        {
            get { return "Does something even better."; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("3076DA8C-EE69-4E87-8639-CE0A4F2C5239"); }
        }

        public override bool Schedulable
        {
            get { return false; }
        }

        public static ServiceHost doesSomethingEvenBetterPluginServiceHost { get; set; }

        public string GetMessage()
        {
            return "Successfully called plugin: A Plugin That Does Something Even Better";
        }

        public override void Loaded()
        {
            try
            {
                ServiceMetadataBehavior doesSomethingEvenBetterPluginServiceBehavior = new ServiceMetadataBehavior();
                doesSomethingEvenBetterPluginServiceBehavior.HttpGetEnabled = true;
                doesSomethingEvenBetterPluginServiceBehavior.HttpGetUrl = new Uri(string.Format("{0}://localhost:{1}/Vmgr.TestPlugin.DoesSomethingEvenBetterPlugin/GetMessage"
                    , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSProtocol)
                    , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSPort)
                    )
                    );

                BasicHttpBinding doesSomethingEvenBetterPluginBinding = new BasicHttpBinding();
                doesSomethingEvenBetterPluginBinding.MaxReceivedMessageSize = 4194304;
                doesSomethingEvenBetterPluginBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                doesSomethingEvenBetterPluginServiceHost = new ServiceHost(typeof(DoesSomethingEvenBetterPlugin));
                doesSomethingEvenBetterPluginServiceHost.Description.Behaviors.Add(doesSomethingEvenBetterPluginServiceBehavior);
                doesSomethingEvenBetterPluginServiceHost.AddServiceEndpoint(typeof(IDoesSomethingEvenBetterPlugin)
                    , doesSomethingEvenBetterPluginBinding
                    , new Uri(string.Format("{0}://localhost:{1}/Vmgr.TestPlugin.DoesSomethingEvenBetterPlugin/GetMessage"
                        , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSProtocol)
                        , Vmgr.Configuration.Settings.GetSetting(Configuration.Settings.Setting.WSPort)
                        )
                        )
                        );
                doesSomethingEvenBetterPluginServiceHost.Open();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(string.Format("Failed to initialize service hosts.  Exception: {0}", ex), LogType.Error);
            }
        }

        public override void UnLoaded()
        {
            if (doesSomethingEvenBetterPluginServiceHost != null)
                doesSomethingEvenBetterPluginServiceHost.Close();
        }
    }
}
