using System;
using System.Linq;
using System.Security.Principal;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Configuration
{
    public static class Settings
    {
        public static string GetSetting(Setting setting)
        {
            string key = string.Empty;
            key = Enum.GetName(typeof(Setting), setting);
            return BaseSettings.GetValue("appSettings", key);
        }

        public static string GetSetting(Setting setting, bool db)
        {
            return Settings.GetSetting(Enum.GetName(typeof(Setting), setting), db);
        }

        /// <summary>
        /// Retrieves a value from the appSettings configuration section of the application's configuration file or from the V-Manager database.
        /// </summary>
        /// <param name="setting">An string key to retrieve.</param>
        /// <param name="db">If true, retrieve the value from the database, otherwise use the application's configuration file.</param>
        /// <returns></returns>
        public static string GetSetting(string key, bool db)
        {
            if (!db)
            {
                return BaseSettings.GetValue("appSettings", key);
            }
            else
            {
                if (AppDomain.CurrentDomain.GetData(key) == null)
                {
                    using (AppService app = new AppService())
                    {
                        SettingMetaData s = app.GetSettings()
                            .Where(a => a.Key == key)
                            .FirstOrDefault()
                            ;

                        if (s == null)
                            throw new ArgumentNullException("value", string.Format("Argument must not be undefined, please check database for missing key/value pair; key={0}", key));

                        if (s.Cache)
                            AppDomain.CurrentDomain.SetData(key, s.Value);
                        else
                            return s.Value;
                    }
                }

                return AppDomain.CurrentDomain.GetData(key) as string;
            }
        }

        public enum Setting
        {
            VmgrConnectionString,
            DefaultPackageRelationship,
            VmgrLogger,
            PackageMaxReceivedMessageSize,
            ServerNameOverride,
            CreateSchedules,
            RTProtocol,
            RTPort,
            WSProtocol,
            WSPort,
        }
    }
}
