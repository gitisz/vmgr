using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vmgr.Helpers
{
    public class ResourceHelper
    {
        public static string UnpackEmbeddedResourceToString(string resourceName)
        {
            string result = string.Empty;

            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a stream from an embedded resource.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns>Returs a stream.  It is up to the subscriber to dispose of the stream, if this method is used.</returns>
        public static Stream UnpackEmbeddedResourceToStream(string resourceName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            Stream resourceStream = executingAssembly
              .GetManifestResourceStream(resourceName);

            return resourceStream;
        }
    }
}
