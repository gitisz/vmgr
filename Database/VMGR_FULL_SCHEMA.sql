USE [DOM_Config]
GO
/****** Object:  Schema [vmg]    Script Date: 05/14/2014 12:38:43 ******/
CREATE SCHEMA [vmg] AUTHORIZATION [dbo]
GO
/****** Object:  Table [vmg].[Log]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [vmg].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[ThreadId] [varchar](255) NULL,
	[ApplicationName] [varchar](255) NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Server] [nvarchar](50) NULL,
	[CorrelationId] [nvarchar](100) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [vmg].[GetLogs]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[GetLogs]
(
	  @pPageNumber INT 
	, @pPageSize INT
	, @pSortBy VARCHAR(50) = NULL
	, @pSortMode VARCHAR(10) = NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @vMainResultset NVARCHAR(MAX) = '', 
		@vDerivedTbl NVARCHAR(MAX) = '', 
		@RowNumber INT = @pPageNumber * @pPageSize + 1

	SET @pSortBy = COALESCE(@pSortBy, 'CreateDate')
	SET @pSortMode = COALESCE(@pSortMode, 'DESC')

	SET @vDerivedTbl = 'SELECT 
							  l.Id
							, l.Thread
							, l.ThreadId
							, l.ApplicationName
							, l.Level
							, l.Logger
							, l.Message
							, l.Exception
							, l.CreateDate
							, l.CreateUser
							, ROW_NUMBER() OVER(ORDER BY ' + @pSortBy + ' ' + @pSortMode + ') AS RowNumber
							FROM vmg.Log l WITH(NOLOCK)
							'
	
	SET @vMainResultset = 'SELECT  
							  Id
							, Thread
							, ThreadId
							, ApplicationName
							, Level
							, Logger
							, Message
							, Exception
							, CreateDate
							, CreateUser
							FROM (' + @vDerivedTbl
	SET @vMainResultset += ') As tbl WHERE RowNumber BETWEEN ' + CONVERT(VARCHAR(9), @RowNumber) + ' AND ' + CONVERT(VARCHAR(9), @pPageSize) + ' * ' + CONVERT(VARCHAR(9), @pPageNumber + 1)
	
	--PRINT @vMainResultset
    EXEC(@vMainResultset)

	
	--Return Total Number of Rows count For Paging
	SET @vDerivedTbl = 'SELECT COUNT(*) AS TotalRows FROM (' + @vDerivedTbl + ') As tbl'
	
	--PRINT @vMainResultset
    EXEC(@vDerivedTbl)
    
	RETURN   
    SELECT  1 AS Id
			, ' ' AS Thread
			, ' ' AS ThreadId
			, ' ' AS ApplicationName
			, ' ' AS Level
			, ' ' AS Logger
			, ' ' AS Message
			, ' ' AS Exception
			, ' ' AS Exception
			, CAST(NULL AS DateTime) AS CreateDate
			, ' ' AS CreateUser
END
GO
/****** Object:  Table [vmg].[SecurityRole]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityRole](
	[SecurityRoleId] [int] IDENTITY(10000,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityRole] PRIMARY KEY CLUSTERED 
(
	[SecurityRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_SecurityRole] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[SecurityPermission]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityPermission](
	[SecurityPermissionId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[SecurityPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Permission] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[Setting]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Setting](
	[SettingId] [int] IDENTITY(10000,1) NOT NULL,
	[Key] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Cache] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[Server]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Server](
	[ServerId] [int] IDENTITY(10000,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[WSProtocol] [nvarchar](10) NOT NULL,
	[WSPort] [int] NOT NULL,
	[RTProtocol] [nvarchar](10) NOT NULL,
	[RTPort] [int] NOT NULL,
	[WSFQDN] [nvarchar](50) NOT NULL,
	[RTFQDN] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Server] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[SecuritySiteMap]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecuritySiteMap](
	[SecuritySiteMapId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityPermissionId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](255) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](255) NULL,
 CONSTRAINT [PK_SecuritySiteMap] PRIMARY KEY CLUSTERED 
(
	[SecuritySiteMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[SecurityRolePermission]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityRolePermission](
	[SecurityRolePermissionId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityRoleId] [int] NOT NULL,
	[SecurityPermissionId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityRolePermission] PRIMARY KEY CLUSTERED 
(
	[SecurityRolePermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_SecurityRolePermission] UNIQUE NONCLUSTERED 
(
	[SecurityRoleId] ASC,
	[SecurityPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[SecurityMembership]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityMembership](
	[SecurityMembershipId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityRoleId] [int] NOT NULL,
	[Account] [nvarchar](max) NOT NULL,
	[AccountType] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityMembership] PRIMARY KEY CLUSTERED 
(
	[SecurityMembershipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[Filter]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Filter](
	[FilterId] [int] IDENTITY(10000,1) NOT NULL,
	[ServerId] [int] NOT NULL,
	[FilterType] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Expression] [nvarchar](max) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Deactivated] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED 
(
	[FilterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [vmg].[DeleteLogs]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[DeleteLogs]
(
	  @pDays INT 
)
AS
BEGIN

	DELETE l
	FROM vmg.[Log] l
	WHERE l.CreateDate < DATEADD(DAY, -(@pDays), GETDATE()) 

END
GO
/****** Object:  Table [vmg].[Package]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [vmg].[Package](
	[PackageId] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [ntext] NULL,
	[Package] [varbinary](max) NULL,
	[Deactivated] [bit] NOT NULL,
	[ServerId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](255) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](255) NULL,
 CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [vmg].[Monitor]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Monitor](
	[MonitorId] [int] IDENTITY(10000,1) NOT NULL,
	[PackageId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Monitor] PRIMARY KEY CLUSTERED 
(
	[MonitorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Monitor] UNIQUE NONCLUSTERED 
(
	[PackageId] ASC,
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [vmg].[RemoveFilter]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[RemoveFilter]
(
	  @FilterId INT 
)
AS
BEGIN


		PRINT 'DELETING [Filter]'
		DELETE p
		FROM [vmg].[Filter] p
		WHERE p.FilterId = @FilterId
		;
		PRINT ''

END
GO
/****** Object:  Table [vmg].[Plugin]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Plugin](
	[PluginId] [int] IDENTITY(10000,1) NOT NULL,
	[PackageId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Schedulable] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Plugin] PRIMARY KEY CLUSTERED 
(
	[PluginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [vmg].[RemoveMonitor]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[RemoveMonitor]
(
	  @MonitorId INT 
)
AS
BEGIN


		PRINT 'DELETING [Monitor]'
		DELETE p
		FROM [vmg].[Monitor] p
		WHERE p.MonitorId = @MonitorId
		;
		PRINT ''

END
GO
/****** Object:  Table [vmg].[Schedule]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Schedule](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [ntext] NULL,
	[PluginId] [int] NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NULL,
	[RecurrenceTypeId] [int] NOT NULL,
	[RecurrenceRule] [nvarchar](1024) NULL,
	[MisfireInstruction] [int] NOT NULL,
	[Deactivated] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](255) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](255) NULL,
	[Exclusions] [nvarchar](max) NULL,
 CONSTRAINT [PK_schedule] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_schedule] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [vmg].[Job]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Job](
	[JobId] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[JobKey] [nvarchar](255) NOT NULL,
	[JobKeyGroup] [nvarchar](255) NOT NULL,
	[JobStatusTypeId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[Trigger]    Script Date: 05/14/2014 12:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[Trigger](
	[TriggerId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[TriggerKey] [nvarchar](255) NOT NULL,
	[TriggerKeyGroup] [nvarchar](255) NOT NULL,
	[Started] [datetime] NULL,
	[Ended] [datetime] NULL,
	[Previousfire] [datetime] NULL,
	[Nextfire] [datetime] NULL,
	[Mayfire] [bit] NULL,
	[Misfire] [bit] NULL,
	[TriggerStatusTypeId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_JobTrigger] PRIMARY KEY CLUSTERED 
(
	[TriggerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [vmg].[RemoveSchedule]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[RemoveSchedule]
(
	  @scheduleId INT 
)
AS
BEGIN

		PRINT 'DELETING [Trigger]'
		DELETE t
		FROM [vmg].[Trigger] t
		INNER JOIN [vmg].[Job] j ON t.[JobId] = j.[JobId]
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		WHERE s.ScheduleId = @scheduleId
		;
		PRINT ''

		PRINT 'DELETING [Job]'
		DELETE j
		FROM [vmg].[Job] j
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		WHERE s.ScheduleId = @scheduleId
		;
		PRINT ''

		PRINT 'DELETING [Schedule]'
		DELETE s
		FROM [vmg].[Schedule] s
		WHERE s.ScheduleId = @scheduleId
		;
		PRINT ''

END
GO
/****** Object:  StoredProcedure [vmg].[RemovePlugin]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[RemovePlugin]
(
	  @pluginId INT 
)
AS
BEGIN

		PRINT 'DELETING [Trigger]'
		DELETE t
		FROM [vmg].[Trigger] t
		INNER JOIN [vmg].[Job] j ON t.[JobId] = j.[JobId]
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PluginId = @pluginId
		;
		PRINT ''

		PRINT 'DELETING [Job]'
		DELETE j
		FROM [vmg].[Job] j
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PluginId = @pluginId
		;
		PRINT ''

		PRINT 'DELETING [Schedule]'
		DELETE s
		FROM [vmg].[Schedule] s
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PluginId = @pluginId
		;
		PRINT ''

		PRINT 'DELETING [Plugin]'
		DELETE p
		FROM [vmg].[Plugin] p
		WHERE p.PluginId = @pluginId
		;
		PRINT ''


END
GO
/****** Object:  StoredProcedure [vmg].[RemovePackage]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [vmg].[RemovePackage]
(
	  @packageId INT 
)
AS
BEGIN

		PRINT 'DELETING [Trigger]'
		DELETE t
		FROM [vmg].[Trigger] t
		INNER JOIN [vmg].[Job] j ON t.[JobId] = j.[JobId]
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PackageId = @packageId
		;
		PRINT ''

		PRINT 'DELETING [Job]'
		DELETE j
		FROM [vmg].[Job] j
		INNER JOIN [vmg].[Schedule] s ON j.[ScheduleId] = s.[ScheduleId]
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PackageId = @packageId
		;
		PRINT ''

		PRINT 'DELETING [Schedule]'
		DELETE s
		FROM [vmg].[Schedule] s
		INNER JOIN [vmg].[Plugin] p ON s.[PluginId] = p.[PluginId]
		WHERE p.PackageId = @packageId
		;
		PRINT ''

		PRINT 'DELETING [Plugin]'
		DELETE p
		FROM [vmg].[Plugin] p
		WHERE p.PackageId = @packageId
		;
		PRINT ''

		PRINT 'DELETING [Monitor]'
		DELETE p
		FROM [vmg].[Monitor] p
		WHERE p.PackageId = @packageId
		;
		PRINT ''

		PRINT 'DELETING [Package]'
		DELETE p
		FROM [vmg].[Package] p
		WHERE p.PackageId = @packageId
		;
		PRINT ''

END
GO
/****** Object:  StoredProcedure [vmg].[GetTriggersByPlugin]    Script Date: 05/14/2014 12:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		TSW
-- Create date: 
-- Description:	Get a list of triggers within X days.
-- =============================================
CREATE PROCEDURE [vmg].[GetTriggersByPlugin]
(
	  @uniqueId UNIQUEIDENTIFIER
	, @days INT = null
)
AS
BEGIN

	SELECT *
	FROM vmg.[Trigger] t
	INNER JOIN vmg.[Job] j on t.JobId = j.JobId
	INNER JOIN vmg.[Schedule] s on j.ScheduleId = s.ScheduleId
	INNER JOIN vmg.[Plugin] p on s.PluginId = p.PluginId
	WHERE 1=1
	AND p.UniqueId = @uniqueId
	AND (@days IS NULL OR DATEDIFF(DAY, t.CreateDate, GETDATE()) < @days) 
	ORDER BY t.CreateDate DESC

END
GO
/****** Object:  Default [DF_Filter_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Filter] ADD  CONSTRAINT [DF_Filter_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Job_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Job] ADD  CONSTRAINT [DF_Job_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Log_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Log] ADD  CONSTRAINT [DF_Log_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Monitor_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Monitor] ADD  CONSTRAINT [DF_Monitor_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_PackageCreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Package] ADD  CONSTRAINT [DF_PackageCreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Plugin_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Plugin] ADD  CONSTRAINT [DF_Plugin_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_schedule_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Schedule] ADD  CONSTRAINT [DF_schedule_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecurityMembership_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityMembership] ADD  CONSTRAINT [DF_SecurityMembership_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecurityRole_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityRole] ADD  CONSTRAINT [DF_SecurityRole_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecurityRolePermission_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission] ADD  CONSTRAINT [DF_SecurityRolePermission_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecuritySiteMap_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecuritySiteMap] ADD  CONSTRAINT [DF_SecuritySiteMap_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Server_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Server] ADD  CONSTRAINT [DF_Server_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Trigger_CreateDate]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Trigger] ADD  CONSTRAINT [DF_Trigger_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  ForeignKey [FK_Filter_Server]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Filter]  WITH CHECK ADD  CONSTRAINT [FK_Filter_Server] FOREIGN KEY([ServerId])
REFERENCES [vmg].[Server] ([ServerId])
GO
ALTER TABLE [vmg].[Filter] CHECK CONSTRAINT [FK_Filter_Server]
GO
/****** Object:  ForeignKey [FK_Job_Schedule]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Schedule] FOREIGN KEY([ScheduleId])
REFERENCES [vmg].[Schedule] ([ScheduleId])
GO
ALTER TABLE [vmg].[Job] CHECK CONSTRAINT [FK_Job_Schedule]
GO
/****** Object:  ForeignKey [FK_Monitor_Package]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Package] FOREIGN KEY([PackageId])
REFERENCES [vmg].[Package] ([PackageId])
GO
ALTER TABLE [vmg].[Monitor] CHECK CONSTRAINT [FK_Monitor_Package]
GO
/****** Object:  ForeignKey [FK_Package_Server]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Package]  WITH CHECK ADD  CONSTRAINT [FK_Package_Server] FOREIGN KEY([ServerId])
REFERENCES [vmg].[Server] ([ServerId])
GO
ALTER TABLE [vmg].[Package] CHECK CONSTRAINT [FK_Package_Server]
GO
/****** Object:  ForeignKey [FK_Plugin_Package]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Plugin]  WITH CHECK ADD  CONSTRAINT [FK_Plugin_Package] FOREIGN KEY([PackageId])
REFERENCES [vmg].[Package] ([PackageId])
GO
ALTER TABLE [vmg].[Plugin] CHECK CONSTRAINT [FK_Plugin_Package]
GO
/****** Object:  ForeignKey [FK_Schedule_Plugin]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_Plugin] FOREIGN KEY([PluginId])
REFERENCES [vmg].[Plugin] ([PluginId])
GO
ALTER TABLE [vmg].[Schedule] CHECK CONSTRAINT [FK_Schedule_Plugin]
GO
/****** Object:  ForeignKey [FK_SecurityMembership_SecurityRole]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityMembership]  WITH CHECK ADD  CONSTRAINT [FK_SecurityMembership_SecurityRole] FOREIGN KEY([SecurityRoleId])
REFERENCES [vmg].[SecurityRole] ([SecurityRoleId])
GO
ALTER TABLE [vmg].[SecurityMembership] CHECK CONSTRAINT [FK_SecurityMembership_SecurityRole]
GO
/****** Object:  ForeignKey [FK_SecurityRolePermission_SecurityPermission]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRolePermission_SecurityPermission] FOREIGN KEY([SecurityPermissionId])
REFERENCES [vmg].[SecurityPermission] ([SecurityPermissionId])
GO
ALTER TABLE [vmg].[SecurityRolePermission] CHECK CONSTRAINT [FK_SecurityRolePermission_SecurityPermission]
GO
/****** Object:  ForeignKey [FK_SecurityRolePermission_SecurityRole]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRolePermission_SecurityRole] FOREIGN KEY([SecurityRoleId])
REFERENCES [vmg].[SecurityRole] ([SecurityRoleId])
GO
ALTER TABLE [vmg].[SecurityRolePermission] CHECK CONSTRAINT [FK_SecurityRolePermission_SecurityRole]
GO
/****** Object:  ForeignKey [FK_SecuritySiteMap_SecurityPermission]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[SecuritySiteMap]  WITH CHECK ADD  CONSTRAINT [FK_SecuritySiteMap_SecurityPermission] FOREIGN KEY([SecurityPermissionId])
REFERENCES [vmg].[SecurityPermission] ([SecurityPermissionId])
GO
ALTER TABLE [vmg].[SecuritySiteMap] CHECK CONSTRAINT [FK_SecuritySiteMap_SecurityPermission]
GO
/****** Object:  ForeignKey [FK_Trigger_Job]    Script Date: 05/14/2014 12:38:42 ******/
ALTER TABLE [vmg].[Trigger]  WITH CHECK ADD  CONSTRAINT [FK_Trigger_Job] FOREIGN KEY([JobId])
REFERENCES [vmg].[Job] ([JobId])
GO
ALTER TABLE [vmg].[Trigger] CHECK CONSTRAINT [FK_Trigger_Job]
GO
