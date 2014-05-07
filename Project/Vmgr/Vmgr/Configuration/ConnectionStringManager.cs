using System;

namespace Vmgr.Configuration
{
    public class ConnectionStringManager
    {
        #region Fields

        private static string _connectionString = String.Empty;

        #endregion

        #region Properties

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
#if TEST
                    _connectionString = "";
#else
                    _connectionString = Settings.GetSetting(Settings.Setting.VmgrConnectionString);
#endif
                }

                return _connectionString;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
