USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[GetTriggersByPlugin]    Script Date: 07/10/2013 13:14:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[vmg].[GetTriggersByPlugin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [vmg].[GetTriggersByPlugin]
GO

USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[GetTriggersByPlugin]    Script Date: 07/10/2013 13:14:13 ******/
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


