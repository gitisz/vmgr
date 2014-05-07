USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[DeleteLogs]    Script Date: 04/19/2014 08:08:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[vmg].[DeleteLogs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [vmg].[DeleteLogs]
GO

USE [DOM_Config]
GO

/****** Object:  StoredProcedure [vmg].[DeleteLogs]    Script Date: 04/19/2014 08:08:18 ******/
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

WITH T
    AS (
    SELECT TOP 5000 *
    FROM vmg.[Log] l
	WHERE l.CreateDate < DATEADD(DAY, -(@pDays), GETDATE()) 
	ORDER BY l.CreateDate ASC
	)
DELETE FROM T

END


GO


