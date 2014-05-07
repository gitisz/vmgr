USE [Dom_Config]

ALTER TABLE vmg.[Server] ADD
	WSProtocol nvarchar(10) NULL,
	WSPort int NULL
GO

UPDATE s
SET s.[WSProtocol] = 'http'
FROM vmg.[Server] s

GO


UPDATE s
SET s.[WSPort] = s.[Port]
FROM vmg.[Server] s

GO

ALTER TABLE vmg.[Server]
	DROP COLUMN Port

GO

ALTER TABLE vmg.[Server] ALTER COLUMN
	WSProtocol nvarchar(10) NOT NULL
GO


ALTER TABLE vmg.[Server] ALTER COLUMN
	WSPort int NOT NULL
GO


ALTER TABLE vmg.[Server] ADD
	RTProtocol nvarchar(10) NULL,
	RTPort int NULL
GO

UPDATE s
SET s.[RTProtocol] = 'https'
FROM vmg.[Server] s

GO

UPDATE s
SET s.[RTPort] = 8080
FROM vmg.[Server] s

GO

ALTER TABLE vmg.[Server] ALTER COLUMN
	RTProtocol nvarchar(10) NOT NULL
GO


ALTER TABLE vmg.[Server] ALTER COLUMN
	RTPort int NOT NULL
GO

