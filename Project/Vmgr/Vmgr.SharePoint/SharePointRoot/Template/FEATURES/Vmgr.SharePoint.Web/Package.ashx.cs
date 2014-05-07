using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vmgr.Data.Biz;
using System.Security.Principal;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.SharePoint
{
    /// <summary>
    /// Summary description for Package
    /// </summary>
    public class Package : IHttpHandler
    {

        public Guid PackageUniqueId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["UniqueId"] != null)
                    return new Guid(HttpContext.Current.Request.QueryString["UniqueId"]);

                return Guid.Empty;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            using (AppService app = new AppService())
            {
                PackageMetaData package = app.GetPackages()
                    .Where(p => p.UniqueId == this.PackageUniqueId)
                    .FirstOrDefault();

                if (package == null)
                    return;

                context.Response.ContentType = "Application/vnd.vmgx-download";
                context.Response.AppendHeader("Content-Disposition"
                    , "attachment; filename=" + package.Name + ".vmgx");
                context.Response.BinaryWrite(package.Package);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}