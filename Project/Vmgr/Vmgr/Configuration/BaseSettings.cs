using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Vmgr.Configuration
{
    public static class BaseSettings
    {
        public static string GetValue(string sectionName, string key)
        {
            string str = "";

            object section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
                throw new ArgumentNullException("section", string.Format("Argument must not be undefined, please check configuration file for missing key/value pair; section={0}", section));

            NameValueCollection values = (NameValueCollection)section;

            str = values[key];

            if (str == null)
                throw new ArgumentNullException("value", string.Format("Argument must not be undefined, please check configuration file for missing key/value pair; key={0}", key));

            return str;
        }

        public static NameValueCollection GetValues(string sectionName)
        {
            object section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
                throw new ArgumentNullException("section", string.Format("Argument must not be undefined, please check configuration file for missing key/value pair; section={0}", section));

            return (NameValueCollection)section;
        }
    }
}
