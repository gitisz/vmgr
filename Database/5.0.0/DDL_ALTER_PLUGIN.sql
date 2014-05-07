USE [DOM_Config]
BEGIN TRANSACTION
GO
ALTER TABLE vmg.Plugin
	DROP COLUMN Deactivated
GO
COMMIT


USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemovePlugin]    Script Date: 04/22/2014 16:44:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[vmg].[RemovePlugin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [vmg].[RemovePlugin]
GO

USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemovePlugin]    Script Date: 04/22/2014 16:44:00 ******/
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


