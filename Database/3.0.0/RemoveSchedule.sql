USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[RemoveSchedule]    Script Date: 05/02/2013 14:00:22 ******/
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


