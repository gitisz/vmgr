///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 4.1
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.SqlServer
{
	/// <summary>Singleton implementation of the PersistenceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the PersistenceInfoProviderBase class is threadsafe.</remarks>
	internal static class PersistenceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IPersistenceInfoProvider _providerInstance = new PersistenceInfoProviderCore();
		#endregion

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static PersistenceInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the PersistenceInfoProviderCore</summary>
		/// <returns>Instance of the PersistenceInfoProvider.</returns>
		public static IPersistenceInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the PersistenceInfoProvider. Used by singleton wrapper.</summary>
	internal class PersistenceInfoProviderCore : PersistenceInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="PersistenceInfoProviderCore"/> class.</summary>
		internal PersistenceInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores with the structure of hierarchical types.</summary>
		private void Init()
		{
			this.InitClass(15);
			InitFilterEntityMappings();
			InitJobEntityMappings();
			InitLogEntityMappings();
			InitMonitorEntityMappings();
			InitPackageEntityMappings();
			InitPluginEntityMappings();
			InitScheduleEntityMappings();
			InitSecurityMembershipEntityMappings();
			InitSecurityPermissionEntityMappings();
			InitSecurityRoleEntityMappings();
			InitSecurityRolePermissionEntityMappings();
			InitSecuritySiteMapEntityMappings();
			InitServerEntityMappings();
			InitSettingEntityMappings();
			InitTriggerEntityMappings();
		}

		/// <summary>Inits FilterEntity's mappings</summary>
		private void InitFilterEntityMappings()
		{
			this.AddElementMapping("FilterEntity", @"DOM_Config", @"vmg", "Filter", 11);
			this.AddElementFieldMapping("FilterEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("FilterEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("FilterEntity", "Deactivated", "Deactivated", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("FilterEntity", "Expression", "Expression", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("FilterEntity", "FilterId", "FilterId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("FilterEntity", "FilterType", "FilterType", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("FilterEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("FilterEntity", "ServerId", "ServerId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("FilterEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 8);
			this.AddElementFieldMapping("FilterEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("FilterEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
		}

		/// <summary>Inits JobEntity's mappings</summary>
		private void InitJobEntityMappings()
		{
			this.AddElementMapping("JobEntity", @"DOM_Config", @"vmg", "Job", 9);
			this.AddElementFieldMapping("JobEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("JobEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("JobEntity", "JobId", "JobId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("JobEntity", "JobKey", "JobKey", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("JobEntity", "JobKeyGroup", "JobKeyGroup", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("JobEntity", "JobStatusTypeId", "JobStatusTypeId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("JobEntity", "ScheduleId", "ScheduleId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("JobEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("JobEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits LogEntity's mappings</summary>
		private void InitLogEntityMappings()
		{
			this.AddElementMapping("LogEntity", @"DOM_Config", @"vmg", "Log", 12);
			this.AddElementFieldMapping("LogEntity", "ApplicationName", "ApplicationName", true, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 0);
			this.AddElementFieldMapping("LogEntity", "CorrelationId", "CorrelationId", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("LogEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 2);
			this.AddElementFieldMapping("LogEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("LogEntity", "Exception", "Exception", true, "VarChar", 2000, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("LogEntity", "Id", "Id", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("LogEntity", "Level", "Level", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("LogEntity", "Logger", "Logger", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("LogEntity", "Message", "Message", false, "VarChar", 4000, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("LogEntity", "Server", "Server", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("LogEntity", "Thread", "Thread", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("LogEntity", "ThreadId", "ThreadId", true, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 11);
		}

		/// <summary>Inits MonitorEntity's mappings</summary>
		private void InitMonitorEntityMappings()
		{
			this.AddElementMapping("MonitorEntity", @"DOM_Config", @"vmg", "Monitor", 7);
			this.AddElementFieldMapping("MonitorEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("MonitorEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("MonitorEntity", "MonitorId", "MonitorId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("MonitorEntity", "PackageId", "PackageId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("MonitorEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("MonitorEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("MonitorEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
		}

		/// <summary>Inits PackageEntity's mappings</summary>
		private void InitPackageEntityMappings()
		{
			this.AddElementMapping("PackageEntity", @"DOM_Config", @"vmg", "Package", 11);
			this.AddElementFieldMapping("PackageEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("PackageEntity", "CreateUser", "CreateUser", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("PackageEntity", "Deactivated", "Deactivated", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("PackageEntity", "Description", "Description", true, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("PackageEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("PackageEntity", "Package", "Package", true, "VarBinary", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 5);
			this.AddElementFieldMapping("PackageEntity", "PackageId", "PackageId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("PackageEntity", "ServerId", "ServerId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("PackageEntity", "UniqueId", "UniqueId", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 8);
			this.AddElementFieldMapping("PackageEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 9);
			this.AddElementFieldMapping("PackageEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 10);
		}

		/// <summary>Inits PluginEntity's mappings</summary>
		private void InitPluginEntityMappings()
		{
			this.AddElementMapping("PluginEntity", @"DOM_Config", @"vmg", "Plugin", 10);
			this.AddElementFieldMapping("PluginEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("PluginEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("PluginEntity", "Description", "Description", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("PluginEntity", "Name", "Name", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("PluginEntity", "PackageId", "PackageId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("PluginEntity", "PluginId", "PluginId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("PluginEntity", "Schedulable", "Schedulable", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("PluginEntity", "UniqueId", "UniqueId", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 7);
			this.AddElementFieldMapping("PluginEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 8);
			this.AddElementFieldMapping("PluginEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
		}

		/// <summary>Inits ScheduleEntity's mappings</summary>
		private void InitScheduleEntityMappings()
		{
			this.AddElementMapping("ScheduleEntity", @"DOM_Config", @"vmg", "Schedule", 16);
			this.AddElementFieldMapping("ScheduleEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("ScheduleEntity", "CreateUser", "CreateUser", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ScheduleEntity", "Deactivated", "Deactivated", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("ScheduleEntity", "Description", "Description", true, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ScheduleEntity", "End", "End", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("ScheduleEntity", "Exclusions", "Exclusions", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ScheduleEntity", "MisfireInstruction", "MisfireInstruction", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("ScheduleEntity", "Name", "Name", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ScheduleEntity", "PluginId", "PluginId", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ScheduleEntity", "RecurrenceRule", "RecurrenceRule", true, "NVarChar", 1024, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ScheduleEntity", "RecurrenceTypeId", "RecurrenceTypeId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("ScheduleEntity", "ScheduleId", "ScheduleId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("ScheduleEntity", "Start", "Start", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 12);
			this.AddElementFieldMapping("ScheduleEntity", "UniqueId", "UniqueId", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 13);
			this.AddElementFieldMapping("ScheduleEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 14);
			this.AddElementFieldMapping("ScheduleEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 15);
		}

		/// <summary>Inits SecurityMembershipEntity's mappings</summary>
		private void InitSecurityMembershipEntityMappings()
		{
			this.AddElementMapping("SecurityMembershipEntity", @"DOM_Config", @"vmg", "SecurityMembership", 9);
			this.AddElementFieldMapping("SecurityMembershipEntity", "Account", "Account", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 0);
			this.AddElementFieldMapping("SecurityMembershipEntity", "AccountType", "AccountType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("SecurityMembershipEntity", "Active", "Active", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("SecurityMembershipEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 3);
			this.AddElementFieldMapping("SecurityMembershipEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("SecurityMembershipEntity", "SecurityMembershipId", "SecurityMembershipId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("SecurityMembershipEntity", "SecurityRoleId", "SecurityRoleId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("SecurityMembershipEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("SecurityMembershipEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits SecurityPermissionEntity's mappings</summary>
		private void InitSecurityPermissionEntityMappings()
		{
			this.AddElementMapping("SecurityPermissionEntity", @"DOM_Config", @"vmg", "SecurityPermission", 3);
			this.AddElementFieldMapping("SecurityPermissionEntity", "Description", "Description", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 0);
			this.AddElementFieldMapping("SecurityPermissionEntity", "Name", "Name", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("SecurityPermissionEntity", "SecurityPermissionId", "SecurityPermissionId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
		}

		/// <summary>Inits SecurityRoleEntity's mappings</summary>
		private void InitSecurityRoleEntityMappings()
		{
			this.AddElementMapping("SecurityRoleEntity", @"DOM_Config", @"vmg", "SecurityRole", 8);
			this.AddElementFieldMapping("SecurityRoleEntity", "Active", "Active", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("SecurityRoleEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 1);
			this.AddElementFieldMapping("SecurityRoleEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SecurityRoleEntity", "Description", "Description", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("SecurityRoleEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("SecurityRoleEntity", "SecurityRoleId", "SecurityRoleId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("SecurityRoleEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("SecurityRoleEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits SecurityRolePermissionEntity's mappings</summary>
		private void InitSecurityRolePermissionEntityMappings()
		{
			this.AddElementMapping("SecurityRolePermissionEntity", @"DOM_Config", @"vmg", "SecurityRolePermission", 8);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "Active", "Active", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 1);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "SecurityPermissionId", "SecurityPermissionId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "SecurityRoleId", "SecurityRoleId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "SecurityRolePermissionId", "SecurityRolePermissionId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("SecurityRolePermissionEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits SecuritySiteMapEntity's mappings</summary>
		private void InitSecuritySiteMapEntityMappings()
		{
			this.AddElementMapping("SecuritySiteMapEntity", @"DOM_Config", @"vmg", "SecuritySiteMap", 8);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "Active", "Active", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 1);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "CreateUser", "CreateUser", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "SecurityPermissionId", "SecurityPermissionId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "SecuritySiteMapId", "SecuritySiteMapId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("SecuritySiteMapEntity", "Value", "Value", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits ServerEntity's mappings</summary>
		private void InitServerEntityMappings()
		{
			this.AddElementMapping("ServerEntity", @"DOM_Config", @"vmg", "Server", 14);
			this.AddElementFieldMapping("ServerEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("ServerEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ServerEntity", "Description", "Description", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ServerEntity", "Name", "Name", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ServerEntity", "RTFqdn", "RTFQDN", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ServerEntity", "RTPort", "RTPort", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ServerEntity", "RTProtocol", "RTProtocol", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ServerEntity", "ServerId", "ServerId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("ServerEntity", "UniqueId", "UniqueId", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 8);
			this.AddElementFieldMapping("ServerEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 9);
			this.AddElementFieldMapping("ServerEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ServerEntity", "WSFqdn", "WSFQDN", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("ServerEntity", "WSPort", "WSPort", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("ServerEntity", "WSProtocol", "WSProtocol", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 13);
		}

		/// <summary>Inits SettingEntity's mappings</summary>
		private void InitSettingEntityMappings()
		{
			this.AddElementMapping("SettingEntity", @"DOM_Config", @"vmg", "Setting", 8);
			this.AddElementFieldMapping("SettingEntity", "Cache", "Cache", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("SettingEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 1);
			this.AddElementFieldMapping("SettingEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SettingEntity", "Key", "Key", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("SettingEntity", "SettingId", "SettingId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("SettingEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
			this.AddElementFieldMapping("SettingEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("SettingEntity", "Value", "Value", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits TriggerEntity's mappings</summary>
		private void InitTriggerEntityMappings()
		{
			this.AddElementMapping("TriggerEntity", @"DOM_Config", @"vmg", "Trigger", 15);
			this.AddElementFieldMapping("TriggerEntity", "CreateDate", "CreateDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 0);
			this.AddElementFieldMapping("TriggerEntity", "CreateUser", "CreateUser", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("TriggerEntity", "Ended", "Ended", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 2);
			this.AddElementFieldMapping("TriggerEntity", "JobId", "JobId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("TriggerEntity", "Mayfire", "Mayfire", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("TriggerEntity", "Misfire", "Misfire", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("TriggerEntity", "Nextfire", "Nextfire", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("TriggerEntity", "Previousfire", "Previousfire", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("TriggerEntity", "Started", "Started", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 8);
			this.AddElementFieldMapping("TriggerEntity", "TriggerId", "TriggerId", false, "Int", 0, 10, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("TriggerEntity", "TriggerKey", "TriggerKey", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("TriggerEntity", "TriggerKeyGroup", "TriggerKeyGroup", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("TriggerEntity", "TriggerStatusTypeId", "TriggerStatusTypeId", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("TriggerEntity", "UpdateDate", "UpdateDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 13);
			this.AddElementFieldMapping("TriggerEntity", "UpdateUser", "UpdateUser", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
		}

	}
}
