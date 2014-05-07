USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemovePackage]    Script Date: 05/02/2013 13:59:28 ******/
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

		PRINT 'DELETING [Package]'
		DELETE p
		FROM [vmg].[Package] p
		WHERE p.PackageId = @packageId
		;
		PRINT ''

END



GO


