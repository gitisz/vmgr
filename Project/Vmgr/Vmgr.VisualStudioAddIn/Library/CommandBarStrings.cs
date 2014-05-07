﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Vmgr.VisualStudioAddin.Library
{
    public class CommandBarStrings
    {
        public static string GetName(string name, _DTE application)
        {
            string result = name;

            try
            {
                string resourceName;

                ResourceManager resourceManager = new ResourceManager("Vmgr.VisualStudioAddin.CommandBar", Assembly.GetExecutingAssembly());
                CultureInfo cultureInfo = new CultureInfo(application.LocaleID);

                if (cultureInfo.TwoLetterISOLanguageName == "zh")
                {
                    System.Globalization.CultureInfo parentCultureInfo = cultureInfo.Parent;
                    resourceName = String.Concat(parentCultureInfo.Name, name);
                }
                else
                {
                    resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, name);
                }

                result = resourceManager.GetString(resourceName);
            }
            catch
            {
                //We tried to find a localized version of the word Tools, but one was not found.
                //  Default to the en-US word, which may work for the current culture.
                result = name;
            }

            return result;
        }
    }
}
