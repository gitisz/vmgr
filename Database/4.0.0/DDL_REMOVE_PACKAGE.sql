USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemovePackage]    Script Date: 07/03/2013 11:03:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[vmg].[RemovePackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [vmg].[RemovePackage]
GO

USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemovePackage]    Script Date: 07/03/2013 11:03:04 ******/
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


