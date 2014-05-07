using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Vmgr.Biz.EntityServices.ComposecurityServices;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.EntityServices;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Helpers;

namespace Vmgr.Biz.EntityServices.ComposecurityServices
{
    internal class CompositeSecurityRolePermissionService : BaseEntityService, IService
    {
        #region PRIVATE PROPERTIES

        private IList<IService> _services = null;
        private IMetaData _metaData = null;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public IMetaData MetaData
        {
            get { return this._metaData; }
        }

        public IList<IService> Services
        {
            get
            {
                if (this._services == null)
                {
                    this._services = new List<IService> {  };
                }

                return this._services;
            }
        }

        public override IList<IValidationRule> BrokenRules
        {
            get
            {
                return this.Services.Where(s => s != null)
                    .SelectMany(s => s.BrokenRules)
                    .ToList();
            }
        }

        #endregion

        #region CTOR

        public CompositeSecurityRolePermissionService(IAppService appService)
            : base(appService)
        {
        }

        #endregion

        #region PRIVATE METHODS

        private void ConvertTo(ISecurityRolePermission securityRolePermissioType)
        {
            IList<SecurityRolePermissionMetaData> securityRolePermissions = (this.appService as AppService).GetSecurityRolePermissions()
                .Where(s => s.SecurityRoleId == securityRolePermissioType.SecurityRole.SecurityRoleId)
                .ToList();

            foreach (SecurityRolePermissionMetaData securityRolePermission in securityRolePermissions)
                securityRolePermission.Active = false;

            foreach (int permissionId in securityRolePermissioType.SelectedPermissions)
            {
                SecurityRolePermissionMetaData securityRolePermission = securityRolePermissions
                    .Where(s => s.SecurityPermissionId == permissionId)
                    .FirstOrDefault() ?? new SecurityRolePermissionMetaData { };

                securityRolePermission.Active = true;
                securityRolePermission.SecurityRoleId = securityRolePermissioType.SecurityRole.SecurityRoleId;
                securityRolePermission.SecurityPermissionId = permissionId;

                if (!securityRolePermissions.Contains(securityRolePermission))
                    securityRolePermissions.Add(securityRolePermission);
            }

            foreach (SecurityRolePermissionMetaData securityRolePermission in securityRolePermissions)
            {
                SecurityRolePermissionService securityRolePermissionService =
                    new SecurityRolePermissionService(securityRolePermission, base.appService);
                securityRolePermissionService.BeforeProcessing += new EventHandler<BeforeProcessingEventArgs>(securityRolePermissionService_BeforeProcessing);
                securityRolePermissionService.Validating += new EventHandler<ValidatingEventArgs<SecurityRolePermissionEntity>>(securityRolePermissionService_Validating);
                securityRolePermissionService.BeforeSave += new EventHandler<BeforeSaveEventArgs<SecurityRolePermissionEntity>>(securityRolePermissionService_BeforeSave);
                securityRolePermissionService.AfterSave += new EventHandler<AfterSaveEventArgs<SecurityRolePermissionEntity>>(securityRolePermissionService_AfterSave);
                securityRolePermissionService.ProcessingComplete += new EventHandler<ProcessingCompleteEventArgs<SecurityRolePermissionEntity>>(securityRolePermissionService_ProcessingComplete);

                this.Services.Add(securityRolePermissionService);
            }
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public bool Save(IMetaData metaData)
        {
            this._metaData = metaData;

            if (!(metaData is ISecurityRolePermission))
                throw new InvalidCastException("MetaData is not of type ISecurityRolePermission.");

            return this.Save(metaData as ISecurityRolePermission);
        }

        public bool Save(ISecurityRolePermission source)
        {
            this._metaData = source;

            if (!(source is ISecurityRolePermission))
                throw new ArgumentException("Argument must implement ISecurityRolePermission.");

            appService.CreateTransaction(true);

            this.ConvertTo(source);

            bool result = true;

            try
            {
                bool commit = true;

                foreach (IService s in this.Services)
                {
                    result = s.Save(s.MetaData);

                    if (!result)
                    {
                        foreach (IValidationRule rule in s.BrokenRules)
                            this.BrokenRules.Add(rule);

                        //  Don't continue processing if Source fails.
                        if (s.MetaData == this.MetaData)
                        {
                            commit = false;
                            break;
                        }
                    }

                    EventHandlerHelper.RemoveAllEventHandlers(s);
                }

                if (appService.Adapter.IsTransactionInProgress)
                {
                    if (commit)
                        appService.Adapter.Commit();
                    else
                        appService.Adapter.Rollback();

                }
            }
            catch (Exception ex)
            {
                result = false;

                Logger.Logs.Log(ex.ToString(), LogType.Error);

                appService.Adapter.Rollback();
            }
            finally
            {
                appService.Adapter.CloseConnection();
            }

            return result;
        }

        public bool Delete(IMetaData metaData)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region EVENTS

        void securityRolePermissionService_BeforeProcessing(object sender, BeforeProcessingEventArgs e)
        {
            Logger.Logs.Log("CompositeSecurityRolePermissionService.BeforeProcessing event called.", LogType.Info);
        }

        void securityRolePermissionService_Validating(object sender, ValidatingEventArgs<SecurityRolePermissionEntity> e)
        {
            Logger.Logs.Log("CompositeSecurityRolePermissionService.Validating event called.", LogType.Info);
        }

        void securityRolePermissionService_BeforeSave(object sender, BeforeSaveEventArgs<SecurityRolePermissionEntity> e)
        {
            Logger.Logs.Log("CompositeSecurityRolePermissionService.BeforeSave event called.", LogType.Info);
        }

        void securityRolePermissionService_AfterSave(object sender, AfterSaveEventArgs<SecurityRolePermissionEntity> e)
        {
            Logger.Logs.Log("CompositeSecurityRolePermissionService.AfterSave event called.", LogType.Info);
        }

        void securityRolePermissionService_ProcessingComplete(object sender, ProcessingCompleteEventArgs<SecurityRolePermissionEntity> e)
        {
            Logger.Logs.Log("CompositeSecurityRolePermissionService.ProcessingComplete event called.", LogType.Info);
        }

        #endregion

    }
}

namespace Vmgr.Data.Biz
{
    public partial class AppService
    {
        #region PRIVATE PROPERTIES

        private CompositeSecurityRolePermissionService _compositeSecuritySecurityRolePermissionService;

        #endregion

        #region PROTECTED PROPERTIES

        internal CompositeSecurityRolePermissionService compositeSecuritySecurityRolePermissionService
        {
            get
            {
                if (this._compositeSecuritySecurityRolePermissionService == null)
                {
                    this._compositeSecuritySecurityRolePermissionService = new CompositeSecurityRolePermissionService(this);
                }
                return this._compositeSecuritySecurityRolePermissionService;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public bool Save(ISecurityRolePermission securityRolePermission)
        {
            return this.compositeSecuritySecurityRolePermissionService.Save(securityRolePermission);
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
