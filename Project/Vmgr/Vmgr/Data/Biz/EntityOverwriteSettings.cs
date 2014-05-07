using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.Linq;
using Vmgr.Data.SqlServer;
/*
* THIS CODE WAS AUTO-GENERATED.  DO NOT MAKE MODIFICATIONS.
*/

namespace Vmgr.Data.Biz
{
	public class EntityOverwriteSettings
	{

        private const string EntityOverwriteSectionKey = "entityOverwriteSettings";

        /// <summary>
        /// Key of EntityOverwrite message.
        /// </summary>
        /// <remarks>
        /// The value of the Message key is equivalent to the string name of the specified EntityOverwrite enum.
        /// </remarks>
        public enum EntityOverwriteSetting
        {
            WorkPackageWorkPackageNumberClientId,
        }

        /// <summary>
        /// Get's a seting from the Message section of the web.config file.
        /// </summary>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static string GetEntityOverwriteSetting(EntityOverwriteSetting entityOverwriteSetting)
        {
            string key = string.Empty;

            key = Enum.GetName(typeof(EntityOverwriteSetting), entityOverwriteSetting);

            string text = string.Empty; //BaseSettings.GetValue(EntityOverwriteSettings.EntityOverwriteSectionKey, key);

            return text;
        }

        /// <summary>
        /// Get's a seting from the Message section if it exists of the web.config file, or null.
        /// </summary>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static string GetEntityOverwriteSetting(string entityOverwriteSetting)
        {
            if (Enum.GetNames(typeof(EntityOverwriteSetting)).Contains(entityOverwriteSetting))
                return string.Empty; //BaseSettings.GetValue(EntityOverwriteSettings.EntityOverwriteSectionKey, entityOverwriteSetting);
            else
                return null;
        }

        /// <summary>
        /// Determines if an entity overwrite setting exists in the Enumeration
        /// </summary>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static bool IsEntityOverwriteSetting(string entityOverwriteSetting)
        {
            return Enum.GetNames(typeof(EntityOverwriteSetting)).Contains(entityOverwriteSetting);
        }
    }
}
