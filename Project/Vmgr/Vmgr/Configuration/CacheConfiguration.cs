using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Configuration
{
    public class CacheConfiguration
    {
        #region Fields

        private static bool _useCache = false;

        #endregion

        #region Properties

        public static bool UseCache
        {
            get
            {
#if TEST
                _useCache = bool.Parse(Settings.GetSetting(Constants.VMGR_USE_CACHE_CONFIGURATION_TEST, true));
#else
                _useCache = bool.Parse(Settings.GetSetting(Constants.VMGR_USE_CACHE_CONFIGURATION, true));
#endif

                return _useCache;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
