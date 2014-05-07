using mscoree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;
using Vmgr.Plugins;

namespace Vmgr.Operations
{
    [Serializable]
    public class PackageOperation : IPackageOperation
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

        public PackageOperation()
        {
            if (this.server == null)
                throw new ApplicationException("Failed to define server.");
        }

        #endregion

        #region PRIVATE METHODS

        private void EnsurePlugins(IPackage package)
        {
            IStatusOperation operation = new StatusOperation();

            IList<IPlugin> loadedPlugins = operation.GetPluginsByDomain(package.UniqueId.ToString());

            using (AppService app = new AppService())
            {
                var _package = app.GetPackages()
                    .Where(p => p.UniqueId == package.UniqueId)
                    .FirstOrDefault()
                    ;

                if (_package == null)
                {
                    _package = new PackageMetaData
                    {
                        Deactivated = false,
                        Description = package.Description,
                        Name = package.Name,
                        Package = package.Package,
                        ServerId = this.server.ServerId,
                        UniqueId = package.UniqueId,
                    };

                    if (!app.Save(_package))
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                }

                IList<PluginMetaData> currentPlugins = app.GetPlugins()
                    .Where(p => p.PackageUniqueId == package.UniqueId)
                    .ToList()
                    ;

                IList<PluginMetaData> removingPlugins = currentPlugins
                    .Where(c => currentPlugins
                    .Select(p => p.UniqueId)
                    .Except(loadedPlugins.Select(p => p.UniqueId), new LambdaComparer<Guid>((a, b) => a == b))
                    .Contains(c.UniqueId)
                    )
                    .ToList()
                    ;

                IList<PluginMetaData> addingPlugins = loadedPlugins
                    .Where(l => loadedPlugins
                        .Select(p => p.UniqueId)
                        .Except(currentPlugins.Select(p => p.UniqueId), new LambdaComparer<Guid>((a, b) => a == b))
                        .Contains(l.UniqueId)
                        )
                    .Select(p => new PluginMetaData
                    {
                        Name = p.Name,
                        Description = p.Description,
                        PackageId = _package.PackageId,
                        Schedulable = p.Schedulable,
                        UniqueId = p.UniqueId,
                    }
                    )
                    .ToList();

                IList<PluginMetaData> updatingPlugins = currentPlugins
                    .Except(removingPlugins, new LambdaComparer<PluginMetaData>((a, b) => a.UniqueId == b.UniqueId))
                    .Except(addingPlugins, new LambdaComparer<PluginMetaData>((a, b) => a.UniqueId == b.UniqueId))
                    .Select(p =>
                    {
                        IPlugin loaded = loadedPlugins
                            .Where(l => l.UniqueId == p.UniqueId)
                            .FirstOrDefault()
                            ;
                        p.Name = loaded.Name;
                        p.Description = loaded.Description;
                        p.Schedulable = loaded.Schedulable;

                        return p;
                    }
                    )
                    .ToList()
                    ;

                foreach (PluginMetaData plugin in removingPlugins)
                {
                    Logger.Logs.Log(string.Format("Removing obsolete plugin ID: {0}, Name: {1}, UniqueID: {2}"
                        , plugin.PluginId
                        , plugin.Name
                        , plugin.UniqueId
                        ), LogType.Warn);

                    app.RemovePlugin(plugin.PluginId);
                }

                foreach (PluginMetaData plugin in addingPlugins)
                {
                    if (!app.Save(plugin))
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);
                }

                foreach (PluginMetaData plugin in updatingPlugins)
                {
                    if (!app.Save(plugin))
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);
                }
            }
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Load()
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IList<PackageMetaData> packages = app.GetPackages()
                        .Where(p => p.ServerId == this.server.ServerId)
                        .Where(p => !p.Deactivated)
                        .Where(p => !PackageManager.Manage.AppDomains.Select(a => new Guid(a.Key)).Contains(p.UniqueId))
                        .ToList();

                    foreach (IPackage package in packages)
                    {
                        try
                        {
                            PackageManager.Manage.LoadPackage(package);
                        }
                        catch (Exception ex)
                        {
                            throw Logger.Logs.Log(string.Format("Failed to load package: {0}.", package.Name), ex, LogType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  Do not rethrow, because we want the service to continue to start.
                Logger.Logs.Log("Exception occurred in PackageOperation.Load.", ex, LogType.Error);
            }
        }

        public void LoadPackage(IPackage package)
        {
            try
            {
                PackageManager.Manage.UnloadPackage(package);

                PackageManager.Manage.LoadPackage(package);

                this.EnsurePlugins(package);
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log(string.Format("Failed to load package: {0}.", package.Name), ex, LogType.Error);
            }
        }

        public void Unload()
        {
            try
            {
                using (AppService app = new AppService())
                {
                    IList<PackageMetaData> packages = app.GetPackages()
                           .Where(p => p.ServerId == this.server.ServerId)
                           .Where(p => PackageManager.Manage.AppDomains.Select(a => new Guid(a.Key)).Contains(p.UniqueId))
                           .ToList();

                    foreach (IPackage package in packages)
                    {
                        try
                        {
                            PackageManager.Manage.UnloadPackage(package);
                        }
                        catch (Exception ex)
                        {
                            throw Logger.Logs.Log(string.Format("Failed to unload package: {0}.", package.Name), ex, LogType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Exception occurred in PackageOperation.UnloadPackage.", ex, LogType.Error);
            }
        }

        public void UnloadPackage(IPackage package)
        {
            try
            {
                PackageManager.Manage.UnloadPackage(package);
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log(string.Format("Failed to unload package: {0}.", package.Name), ex, LogType.Error);
            }
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
