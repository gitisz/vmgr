using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Vmgr
{
    public class ResourceHelper
    {
        public static string UnpackEmbeddedResourceToString(string resourceName)
        {
            Assembly executingAssembly = Assembly.GetCallingAssembly();

            Stream resourceStream = executingAssembly
              .GetManifestResourceStream(resourceName);

            using (StreamReader reader = new StreamReader(resourceStream, Encoding.ASCII))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream UnpackEmbeddedResourceToStream(string resourceName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            Stream resourceStream = executingAssembly
              .GetManifestResourceStream(resourceName);

            return resourceStream;
        }
    }
}
