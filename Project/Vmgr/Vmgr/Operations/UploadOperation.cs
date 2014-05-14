using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Messaging;
using Vmgr.Packaging;

namespace Vmgr.Operations
{
    public class UploadOperation : IUploadOperation
    {
        #region PRIVATE PROPERTIES

        private ServerMetaData _server = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected ServerMetaData server
        {
            get
            {
                if (this._server == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._server = app.GetServers()
                            .Where(s => s.Name == System.Environment.MachineName)
                            .FirstOrDefault()
                            ;
                    }
                }

                return this._server;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        protected IHubContext context = GlobalHost.ConnectionManager.GetHubContext<VmgrHub>();

        private void OnUploadProgress(UploadProgress uploadProgress)
        {
            try
            {
                VmgrClient.Instance.Proxy
                    .Invoke("UploadProgress", uploadProgress)
                    .Wait()
                    ;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnUploadProgress failed.", ex, LogType.Error);
            }
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Upload(byte[] bytes, string group, bool overwrite)
        {
            /*
             * Validate package.
             */
            this.OnUploadProgress(
                new UploadProgress
                {
                    IsFaulted = false,
                    Group = group,
                    Message = "File received.  Validating package.",
                    PrimaryTotal = 5,
                    PrimaryValue = 1,
                }
                )
                ;

            System.Threading.Thread.Sleep(1000);

            XDocument document = null;

            try
            {
                document = PackageManager.GetPackageManifest(bytes);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to retrieve package or validate package manifest.  Review the log for additional information.", ex, LogType.Error);

                this.OnUploadProgress(
                    new UploadProgress
                    {
                        IsFaulted = true,
                        Group = group,
                        Message = "Failed to retrieve package or validate package manifest.  Review the log for additional information.",
                        PrimaryTotal = 5,
                        PrimaryValue = 5,
                    }
                    )
                    ;
            }


            /*
             * Save package.
             */
            System.Threading.Thread.Sleep(1000);

            PackageMetaData package = null;

            if (document != null)
            {
                this.OnUploadProgress(
                    new UploadProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = "Package validated.  Saving package to the database.",
                        PrimaryTotal = 5,
                        PrimaryValue = 2,
                    }
                    )
                    ;

                try
                {
                    using (AppService app = new AppService())
                    {
                        package = new PackageMetaData
                        {
                            Deactivated = false,
                            Package = bytes,
                            Name = document.Elements("package").Select(s => s.Attribute("name").Value).First(),
                            Description = document.Elements("package").Select(s => s.Attribute("description").Value).First(),
                            UniqueId = new Guid(document.Elements("package").Select(s => s.Attribute("uniqueId").Value).First()),
                            ServerId = this.server.ServerId,
                        }
                        ;

                        PackageMetaData p = app.GetPackages()
                            .Where(x => x.UniqueId == package.UniqueId)
                            .FirstOrDefault();

                        if (p != null)
                        {
                            package.PackageId = p.PackageId;
                            package.Deactivated = p.Deactivated;

                            if (overwrite)
                            {
                                if (!app.Save(package))
                                    throw Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);
                            }
                            else
                            {
                                string message = string.Format("The uploaded package <strong>{0} - &#123;{1}&#125;</strong> already exists.  <br />Please select the Overwrite checkbox if you with to replace it."
                                    , p.Name
                                    , p.UniqueId
                                    );

                                this.OnUploadProgress(
                                    new UploadProgress
                                    {
                                        IsFaulted = true,
                                        Group = group,
                                        Message = message,
                                        PrimaryTotal = 5,
                                        PrimaryValue = 5,
                                    }
                                    )
                                    ;

                                package = null;
                            }
                        }
                        else
                        {
                            if (!app.Save(package))
                                throw Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                        }
                    }
                }
                catch (Exception ex)
                {
                    package = null;

                    Logger.Logs.Log("Failed to save package package to the database.  Review the log for additional information.", ex, LogType.Error);

                    this.OnUploadProgress(
                        new UploadProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to save package to the database.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                        }
                        )
                        ;
                }
            }


            /*
             * Load the package.
             */
            System.Threading.Thread.Sleep(1000);

            bool loaded = false;

            if (package != null)
            {
                this.OnUploadProgress(
                    new UploadProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' saved.  Loading the package into service.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 3,
                    }
                    )
                    ;

                try
                {
                    IPackageOperation packageOperation = new PackageOperation { };
                    packageOperation.LoadPackage(package);

                    loaded = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to load the package into service.  Review the log for additional information.", ex, LogType.Error);

                    this.OnUploadProgress(
                        new UploadProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to load the package into service.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                        }
                        )
                        ;
                }
            }

            /*
             * Schedule jobs.
             */
            System.Threading.Thread.Sleep(1000);

            bool scheduled = false;

            if (loaded)
            {
                this.OnUploadProgress(
                    new UploadProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' loaded.  Detecting plugins and initializing schedules.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 4,
                    }
                    )
                    ;

                try
                {
                    IScheduleOperation scheduleOperation = new ScheduleOperation { };
                    scheduleOperation.SchedulePackage(package);

                    scheduled = true;
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to detect plugins or initialize schedules.  Review the log for additional information.", ex, LogType.Error);

                    this.OnUploadProgress(
                        new UploadProgress
                        {
                            IsFaulted = true,
                            Group = group,
                            Message = "Failed to detect plugins or initialize schedules.  Review the log for additional information.",
                            PrimaryTotal = 5,
                            PrimaryValue = 5,
                        }
                        )
                        ;
                }
            }

            /*
             * Complete.
             */

            System.Threading.Thread.Sleep(1000);

            if (scheduled)
            {
                this.OnUploadProgress(
                    new UploadProgress
                    {
                        IsFaulted = false,
                        Group = group,
                        Message = string.Format("Package '{0}' successfully uploaded.", package.Name),
                        PrimaryTotal = 5,
                        PrimaryValue = 5,
                    }
                    )
                    ;
            }
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
