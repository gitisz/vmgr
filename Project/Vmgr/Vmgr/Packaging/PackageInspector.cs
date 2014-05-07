using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Helpers;
using Vmgr.Plugins;

namespace Vmgr.Packaging
{
    public class PackageInspector : IPackageInspector
    {
        public IList<AssemblyMetaData> GetAssemblies()
        {
            IList<AssemblyMetaData> list = AppDomain.CurrentDomain.GetAssemblies().Select<Assembly, AssemblyMetaData>(delegate(Assembly a)
            {
                AssemblyMetaData data = new AssemblyMetaData
                {
                    Id = AppDomain.CurrentDomain.FriendlyName,
                    FullName = a.FullName
                };

                try
                {
                    data.Name = a.GetName().Name;
                    data.AssemblyVersion = a.GetName().Version.ToString(4);
                
                    object[] customAttributes = a.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    
                    object[] objArray2 = a.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    
                    if (customAttributes.Length > 0)
                    {
                        data.AssemblyFileVersion = (customAttributes[0] as AssemblyFileVersionAttribute).Version;
                    }
                    
                    if (objArray2.Length > 0)
                    {
                        data.CompanyName = (objArray2[0] as AssemblyCompanyAttribute).Company;
                    }
                }
                catch
                {
                }
                return data;

            })
            .ToList<AssemblyMetaData>();

            IList<AssemblyMetaData> assemblies = new List<AssemblyMetaData>();
            
            foreach (AssemblyMetaData data in list)
            {
                if (!(from l in assemblies select l.Name).Contains<string>(data.Name))
                {
                    assemblies.Add(data);
                }
            }
            
            return assemblies;
        }
    }
}
