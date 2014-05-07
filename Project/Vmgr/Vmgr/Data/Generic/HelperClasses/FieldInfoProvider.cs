///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 4.1
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Generic.HelperClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>Singleton implementation of the FieldInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the FieldInfoProviderBase class is threadsafe.</remarks>
	internal static class FieldInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IFieldInfoProvider _providerInstance = new FieldInfoProviderCore();
		#endregion

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static FieldInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the FieldInfoProviderCore</summary>
		/// <returns>Instance of the FieldInfoProvider.</returns>
		public static IFieldInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the FieldInfoProvider. Used by singleton wrapper.</summary>
	internal class FieldInfoProviderCore : FieldInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="FieldInfoProviderCore"/> class.</summary>
		internal FieldInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores.</summary>
		private void Init()
		{
			this.InitClass( (15 + 0));
			InitFilterEntityInfos();
			InitJobEntityInfos();
			InitLogEntityInfos();
			InitMonitorEntityInfos();
			InitPackageEntityInfos();
			InitPluginEntityInfos();
			InitScheduleEntityInfos();
			InitSecurityMembershipEntityInfos();
			InitSecurityPermissionEntityInfos();
			InitSecurityRoleEntityInfos();
			InitSecurityRolePermissionEntityInfos();
			InitSecuritySiteMapEntityInfos();
			InitServerEntityInfos();
			InitSettingEntityInfos();
			InitTriggerEntityInfos();

			this.ConstructElementFieldStructures(InheritanceInfoProviderSingleton.GetInstance());
		}

		/// <summary>Inits FilterEntity's FieldInfo objects</summary>
		private void InitFilterEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterFieldIndex), "FilterEntity");
			this.AddElementFieldInfo("FilterEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)FilterFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Deactivated", typeof(System.Boolean), false, false, false, false,  (int)FilterFieldIndex.Deactivated, 0, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Expression", typeof(System.String), false, false, false, true,  (int)FilterFieldIndex.Expression, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "FilterId", typeof(System.Int32), true, false, false, false,  (int)FilterFieldIndex.FilterId, 0, 0, 10);
			this.AddElementFieldInfo("FilterEntity", "FilterType", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.FilterType, 50, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Name", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "ServerId", typeof(System.Int32), false, true, false, false,  (int)FilterFieldIndex.ServerId, 0, 0, 10);
			this.AddElementFieldInfo("FilterEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)FilterFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)FilterFieldIndex.UpdateUser, 50, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Username", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.Username, 50, 0, 0);
		}
		/// <summary>Inits JobEntity's FieldInfo objects</summary>
		private void InitJobEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(JobFieldIndex), "JobEntity");
			this.AddElementFieldInfo("JobEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)JobFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("JobEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)JobFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("JobEntity", "JobId", typeof(System.Int32), true, false, true, false,  (int)JobFieldIndex.JobId, 0, 0, 10);
			this.AddElementFieldInfo("JobEntity", "JobKey", typeof(System.String), false, false, false, false,  (int)JobFieldIndex.JobKey, 255, 0, 0);
			this.AddElementFieldInfo("JobEntity", "JobKeyGroup", typeof(System.String), false, false, false, false,  (int)JobFieldIndex.JobKeyGroup, 255, 0, 0);
			this.AddElementFieldInfo("JobEntity", "JobStatusTypeId", typeof(System.Int32), false, false, false, false,  (int)JobFieldIndex.JobStatusTypeId, 0, 0, 10);
			this.AddElementFieldInfo("JobEntity", "ScheduleId", typeof(System.Int32), false, true, false, false,  (int)JobFieldIndex.ScheduleId, 0, 0, 10);
			this.AddElementFieldInfo("JobEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)JobFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("JobEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)JobFieldIndex.UpdateUser, 50, 0, 0);
		}
		/// <summary>Inits LogEntity's FieldInfo objects</summary>
		private void InitLogEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(LogFieldIndex), "LogEntity");
			this.AddElementFieldInfo("LogEntity", "ApplicationName", typeof(System.String), false, false, false, true,  (int)LogFieldIndex.ApplicationName, 255, 0, 0);
			this.AddElementFieldInfo("LogEntity", "CorrelationId", typeof(System.String), false, false, false, true,  (int)LogFieldIndex.CorrelationId, 100, 0, 0);
			this.AddElementFieldInfo("LogEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)LogFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("LogEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)LogFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Exception", typeof(System.String), false, false, false, true,  (int)LogFieldIndex.Exception, 2000, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Id", typeof(System.Int32), true, false, true, false,  (int)LogFieldIndex.Id, 0, 0, 10);
			this.AddElementFieldInfo("LogEntity", "Level", typeof(System.String), false, false, false, false,  (int)LogFieldIndex.Level, 50, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Logger", typeof(System.String), false, false, false, false,  (int)LogFieldIndex.Logger, 255, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Message", typeof(System.String), false, false, false, false,  (int)LogFieldIndex.Message, 4000, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Server", typeof(System.String), false, false, false, true,  (int)LogFieldIndex.Server, 50, 0, 0);
			this.AddElementFieldInfo("LogEntity", "Thread", typeof(System.String), false, false, false, false,  (int)LogFieldIndex.Thread, 255, 0, 0);
			this.AddElementFieldInfo("LogEntity", "ThreadId", typeof(System.String), false, false, false, true,  (int)LogFieldIndex.ThreadId, 255, 0, 0);
		}
		/// <summary>Inits MonitorEntity's FieldInfo objects</summary>
		private void InitMonitorEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MonitorFieldIndex), "MonitorEntity");
			this.AddElementFieldInfo("MonitorEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)MonitorFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("MonitorEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)MonitorFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("MonitorEntity", "MonitorId", typeof(System.Int32), true, false, true, false,  (int)MonitorFieldIndex.MonitorId, 0, 0, 10);
			this.AddElementFieldInfo("MonitorEntity", "PackageId", typeof(System.Int32), false, true, false, false,  (int)MonitorFieldIndex.PackageId, 0, 0, 10);
			this.AddElementFieldInfo("MonitorEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)MonitorFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("MonitorEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)MonitorFieldIndex.UpdateUser, 50, 0, 0);
			this.AddElementFieldInfo("MonitorEntity", "Username", typeof(System.String), false, false, false, false,  (int)MonitorFieldIndex.Username, 50, 0, 0);
		}
		/// <summary>Inits PackageEntity's FieldInfo objects</summary>
		private void InitPackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PackageFieldIndex), "PackageEntity");
			this.AddElementFieldInfo("PackageEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)PackageFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)PackageFieldIndex.CreateUser, 255, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "Deactivated", typeof(System.Boolean), false, false, false, false,  (int)PackageFieldIndex.Deactivated, 0, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "Description", typeof(System.String), false, false, false, true,  (int)PackageFieldIndex.Description, 1073741823, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "Name", typeof(System.String), false, false, false, false,  (int)PackageFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "Package", typeof(System.Byte[]), false, false, false, true,  (int)PackageFieldIndex.Package, 2147483647, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "PackageId", typeof(System.Int32), true, false, true, false,  (int)PackageFieldIndex.PackageId, 0, 0, 10);
			this.AddElementFieldInfo("PackageEntity", "ServerId", typeof(System.Int32), false, true, false, false,  (int)PackageFieldIndex.ServerId, 0, 0, 10);
			this.AddElementFieldInfo("PackageEntity", "UniqueId", typeof(System.Guid), false, false, false, false,  (int)PackageFieldIndex.UniqueId, 0, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)PackageFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("PackageEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)PackageFieldIndex.UpdateUser, 255, 0, 0);
		}
		/// <summary>Inits PluginEntity's FieldInfo objects</summary>
		private void InitPluginEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PluginFieldIndex), "PluginEntity");
			this.AddElementFieldInfo("PluginEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)PluginFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)PluginFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "Description", typeof(System.String), false, false, false, true,  (int)PluginFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "Name", typeof(System.String), false, false, false, false,  (int)PluginFieldIndex.Name, 255, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "PackageId", typeof(System.Int32), false, true, false, false,  (int)PluginFieldIndex.PackageId, 0, 0, 10);
			this.AddElementFieldInfo("PluginEntity", "PluginId", typeof(System.Int32), true, false, true, false,  (int)PluginFieldIndex.PluginId, 0, 0, 10);
			this.AddElementFieldInfo("PluginEntity", "Schedulable", typeof(System.Boolean), false, false, false, false,  (int)PluginFieldIndex.Schedulable, 0, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "UniqueId", typeof(System.Guid), false, false, false, false,  (int)PluginFieldIndex.UniqueId, 0, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)PluginFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("PluginEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)PluginFieldIndex.UpdateUser, 50, 0, 0);
		}
		/// <summary>Inits ScheduleEntity's FieldInfo objects</summary>
		private void InitScheduleEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ScheduleFieldIndex), "ScheduleEntity");
			this.AddElementFieldInfo("ScheduleEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)ScheduleFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)ScheduleFieldIndex.CreateUser, 255, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "Deactivated", typeof(System.Boolean), false, false, false, false,  (int)ScheduleFieldIndex.Deactivated, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "Description", typeof(System.String), false, false, false, true,  (int)ScheduleFieldIndex.Description, 1073741823, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "End", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ScheduleFieldIndex.End, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "Exclusions", typeof(System.String), false, false, false, true,  (int)ScheduleFieldIndex.Exclusions, 2147483647, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "MisfireInstruction", typeof(System.Int32), false, false, false, false,  (int)ScheduleFieldIndex.MisfireInstruction, 0, 0, 10);
			this.AddElementFieldInfo("ScheduleEntity", "Name", typeof(System.String), false, false, false, false,  (int)ScheduleFieldIndex.Name, 255, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "PluginId", typeof(Nullable<System.Int32>), false, true, false, true,  (int)ScheduleFieldIndex.PluginId, 0, 0, 10);
			this.AddElementFieldInfo("ScheduleEntity", "RecurrenceRule", typeof(System.String), false, false, false, true,  (int)ScheduleFieldIndex.RecurrenceRule, 1024, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "RecurrenceTypeId", typeof(System.Int32), false, false, false, false,  (int)ScheduleFieldIndex.RecurrenceTypeId, 0, 0, 10);
			this.AddElementFieldInfo("ScheduleEntity", "ScheduleId", typeof(System.Int32), true, false, true, false,  (int)ScheduleFieldIndex.ScheduleId, 0, 0, 10);
			this.AddElementFieldInfo("ScheduleEntity", "Start", typeof(System.DateTime), false, false, false, false,  (int)ScheduleFieldIndex.Start, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "UniqueId", typeof(System.Guid), false, false, false, false,  (int)ScheduleFieldIndex.UniqueId, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ScheduleFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("ScheduleEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)ScheduleFieldIndex.UpdateUser, 255, 0, 0);
		}
		/// <summary>Inits SecurityMembershipEntity's FieldInfo objects</summary>
		private void InitSecurityMembershipEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SecurityMembershipFieldIndex), "SecurityMembershipEntity");
			this.AddElementFieldInfo("SecurityMembershipEntity", "Account", typeof(System.String), false, false, false, false,  (int)SecurityMembershipFieldIndex.Account, 2147483647, 0, 0);
			this.AddElementFieldInfo("SecurityMembershipEntity", "AccountType", typeof(System.Int32), false, false, false, false,  (int)SecurityMembershipFieldIndex.AccountType, 0, 0, 10);
			this.AddElementFieldInfo("SecurityMembershipEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)SecurityMembershipFieldIndex.Active, 0, 0, 0);
			this.AddElementFieldInfo("SecurityMembershipEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)SecurityMembershipFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityMembershipEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)SecurityMembershipFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("SecurityMembershipEntity", "SecurityMembershipId", typeof(System.Int32), true, false, true, false,  (int)SecurityMembershipFieldIndex.SecurityMembershipId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityMembershipEntity", "SecurityRoleId", typeof(System.Int32), false, true, false, false,  (int)SecurityMembershipFieldIndex.SecurityRoleId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityMembershipEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SecurityMembershipFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityMembershipEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)SecurityMembershipFieldIndex.UpdateUser, 50, 0, 0);
		}
		/// <summary>Inits SecurityPermissionEntity's FieldInfo objects</summary>
		private void InitSecurityPermissionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SecurityPermissionFieldIndex), "SecurityPermissionEntity");
			this.AddElementFieldInfo("SecurityPermissionEntity", "Description", typeof(System.String), false, false, false, true,  (int)SecurityPermissionFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("SecurityPermissionEntity", "Name", typeof(System.String), false, false, false, false,  (int)SecurityPermissionFieldIndex.Name, 255, 0, 0);
			this.AddElementFieldInfo("SecurityPermissionEntity", "SecurityPermissionId", typeof(System.Int32), true, false, false, false,  (int)SecurityPermissionFieldIndex.SecurityPermissionId, 0, 0, 10);
		}
		/// <summary>Inits SecurityRoleEntity's FieldInfo objects</summary>
		private void InitSecurityRoleEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SecurityRoleFieldIndex), "SecurityRoleEntity");
			this.AddElementFieldInfo("SecurityRoleEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)SecurityRoleFieldIndex.Active, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)SecurityRoleFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)SecurityRoleFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "Description", typeof(System.String), false, false, false, true,  (int)SecurityRoleFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "Name", typeof(System.String), false, false, false, false,  (int)SecurityRoleFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "SecurityRoleId", typeof(System.Int32), true, false, true, false,  (int)SecurityRoleFieldIndex.SecurityRoleId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityRoleEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SecurityRoleFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRoleEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)SecurityRoleFieldIndex.UpdateUser, 50, 0, 0);
		}
		/// <summary>Inits SecurityRolePermissionEntity's FieldInfo objects</summary>
		private void InitSecurityRolePermissionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SecurityRolePermissionFieldIndex), "SecurityRolePermissionEntity");
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)SecurityRolePermissionFieldIndex.Active, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)SecurityRolePermissionFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)SecurityRolePermissionFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "SecurityPermissionId", typeof(System.Int32), false, true, false, false,  (int)SecurityRolePermissionFieldIndex.SecurityPermissionId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "SecurityRoleId", typeof(System.Int32), false, true, false, false,  (int)SecurityRolePermissionFieldIndex.SecurityRoleId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "SecurityRolePermissionId", typeof(System.Int32), true, false, true, false,  (int)SecurityRolePermissionFieldIndex.SecurityRolePermissionId, 0, 0, 10);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SecurityRolePermissionFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecurityRolePermissionEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)SecurityRolePermissionFieldIndex.UpdateUser, 50, 0, 0);
		}
		/// <summary>Inits SecuritySiteMapEntity's FieldInfo objects</summary>
		private void InitSecuritySiteMapEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SecuritySiteMapFieldIndex), "SecuritySiteMapEntity");
			this.AddElementFieldInfo("SecuritySiteMapEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)SecuritySiteMapFieldIndex.Active, 0, 0, 0);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)SecuritySiteMapFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)SecuritySiteMapFieldIndex.CreateUser, 255, 0, 0);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "SecurityPermissionId", typeof(System.Int32), false, true, false, false,  (int)SecuritySiteMapFieldIndex.SecurityPermissionId, 0, 0, 10);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "SecuritySiteMapId", typeof(System.Int32), true, false, true, false,  (int)SecuritySiteMapFieldIndex.SecuritySiteMapId, 0, 0, 10);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SecuritySiteMapFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)SecuritySiteMapFieldIndex.UpdateUser, 255, 0, 0);
			this.AddElementFieldInfo("SecuritySiteMapEntity", "Value", typeof(System.String), false, false, false, false,  (int)SecuritySiteMapFieldIndex.Value, 2147483647, 0, 0);
		}
		/// <summary>Inits ServerEntity's FieldInfo objects</summary>
		private void InitServerEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ServerFieldIndex), "ServerEntity");
			this.AddElementFieldInfo("ServerEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)ServerFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "Description", typeof(System.String), false, false, false, true,  (int)ServerFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "Name", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.Name, 255, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "RTFqdn", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.RTFqdn, 50, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "RTPort", typeof(System.Int32), false, false, false, false,  (int)ServerFieldIndex.RTPort, 0, 0, 10);
			this.AddElementFieldInfo("ServerEntity", "RTProtocol", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.RTProtocol, 10, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "ServerId", typeof(System.Int32), true, false, true, false,  (int)ServerFieldIndex.ServerId, 0, 0, 10);
			this.AddElementFieldInfo("ServerEntity", "UniqueId", typeof(System.Guid), false, false, false, false,  (int)ServerFieldIndex.UniqueId, 0, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServerFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)ServerFieldIndex.UpdateUser, 50, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "WSFqdn", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.WSFqdn, 50, 0, 0);
			this.AddElementFieldInfo("ServerEntity", "WSPort", typeof(System.Int32), false, false, false, false,  (int)ServerFieldIndex.WSPort, 0, 0, 10);
			this.AddElementFieldInfo("ServerEntity", "WSProtocol", typeof(System.String), false, false, false, false,  (int)ServerFieldIndex.WSProtocol, 10, 0, 0);
		}
		/// <summary>Inits SettingEntity's FieldInfo objects</summary>
		private void InitSettingEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SettingFieldIndex), "SettingEntity");
			this.AddElementFieldInfo("SettingEntity", "Cache", typeof(System.Boolean), false, false, false, false,  (int)SettingFieldIndex.Cache, 0, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)SettingFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)SettingFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "Key", typeof(System.String), false, false, false, false,  (int)SettingFieldIndex.Key, 100, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "SettingId", typeof(System.Int32), true, false, true, false,  (int)SettingFieldIndex.SettingId, 0, 0, 10);
			this.AddElementFieldInfo("SettingEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SettingFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)SettingFieldIndex.UpdateUser, 50, 0, 0);
			this.AddElementFieldInfo("SettingEntity", "Value", typeof(System.String), false, false, false, true,  (int)SettingFieldIndex.Value, 2147483647, 0, 0);
		}
		/// <summary>Inits TriggerEntity's FieldInfo objects</summary>
		private void InitTriggerEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TriggerFieldIndex), "TriggerEntity");
			this.AddElementFieldInfo("TriggerEntity", "CreateDate", typeof(System.DateTime), false, false, false, false,  (int)TriggerFieldIndex.CreateDate, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "CreateUser", typeof(System.String), false, false, false, false,  (int)TriggerFieldIndex.CreateUser, 50, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "Ended", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)TriggerFieldIndex.Ended, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "JobId", typeof(System.Int32), false, true, false, false,  (int)TriggerFieldIndex.JobId, 0, 0, 10);
			this.AddElementFieldInfo("TriggerEntity", "Mayfire", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)TriggerFieldIndex.Mayfire, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "Misfire", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)TriggerFieldIndex.Misfire, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "Nextfire", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)TriggerFieldIndex.Nextfire, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "Previousfire", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)TriggerFieldIndex.Previousfire, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "Started", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)TriggerFieldIndex.Started, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "TriggerId", typeof(System.Int32), true, false, true, false,  (int)TriggerFieldIndex.TriggerId, 0, 0, 10);
			this.AddElementFieldInfo("TriggerEntity", "TriggerKey", typeof(System.String), false, false, false, false,  (int)TriggerFieldIndex.TriggerKey, 255, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "TriggerKeyGroup", typeof(System.String), false, false, false, false,  (int)TriggerFieldIndex.TriggerKeyGroup, 255, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "TriggerStatusTypeId", typeof(System.Int32), false, false, false, false,  (int)TriggerFieldIndex.TriggerStatusTypeId, 0, 0, 10);
			this.AddElementFieldInfo("TriggerEntity", "UpdateDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)TriggerFieldIndex.UpdateDate, 0, 0, 0);
			this.AddElementFieldInfo("TriggerEntity", "UpdateUser", typeof(System.String), false, false, false, true,  (int)TriggerFieldIndex.UpdateUser, 50, 0, 0);
		}
		
	}
}




