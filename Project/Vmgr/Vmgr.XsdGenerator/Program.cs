using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Vmgr;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;

namespace Vmgr.XsdGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = null;

            Logger.Logs.Log("Test", LogType.Info);

            do
            {
                Console.WriteLine("Please provide a path to the XML file [Enter for default]:");

                string path = string.Empty;

                if (args.Count() == 0)
                    path = Console.ReadLine();
                else
                    path = args[0];

                try
                {
                    uri = new Uri(path);
                }
                catch
                {
                    uri = new Uri(@"http:\\");
                }
            }
            while (uri == null);


            if (uri.LocalPath == @"http:\\")
            {
                Package vmgrPluginPackage = null;

                vmgrPluginPackage = new Package
                {
                    Name = "Test Plugin",
                    Description = "Tests stuff.",
                    UniqueId = Guid.NewGuid(),
                    Assemblies = new List<AssemblyItem> { new AssemblyItem { Location = "Test.Plugin.Dll" } },
                }
                ;

                XmlSerializer serializer = new XmlSerializer(typeof(Package));

                // Create a new file stream to write the serialized object to a file
                string defaultFile = string.Format(@"D:\{0}.xml", DateTime.Now.ToFileTime());

                uri = new Uri(defaultFile);

                using (TextWriter textWriter = new StreamWriter(defaultFile))
                {
                    serializer.Serialize(textWriter, vmgrPluginPackage);
                }
            }

            Console.WriteLine("Using path '{0}'.", uri);

            XmlReader reader = XmlReader.Create(uri.LocalPath);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();

            schemaSet = schema.InferSchema(reader);

            string xsdPath = new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).CodeBase)
                , string.Format("{0}.xsd", DateTime.Now.ToFileTime()))).LocalPath;

            foreach (XmlSchema s in schemaSet.Schemas())
            {
                using (XmlTextWriter writer = new XmlTextWriter(xsdPath, Encoding.UTF8))
                {
                    s.Write(writer);
                }
            }
        }
    }
}
