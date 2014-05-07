USE [Dom_Config]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[vmg].[FK_Monitor_Package]') AND parent_object_id = OBJECT_ID(N'[vmg].[Monitor]'))
ALTER TABLE [vmg].[Monitor] DROP CONSTRAINT [FK_Monitor_Package]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Monitor_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [vmg].[Monitor] DROP CONSTRAINT [DF_Monitor_CreateDate]
END

GO

USE [Dom_Config]
GO

/****** Object:  Table [vmg].[Monitor]    Script Date: 07/16/2013 12:51:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[vmg].[Monitor]') AND type in (N'U'))
DROP TABLE [vmg].[Monitor]
GO

USE [Dom_Config]
GO

/****** Object:  Table [vmg].[Monitor]    Script Date: 07/16/2013 12:51:24 ******/
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

ALTER TABLE [vmg].[Monitor]  WITH CHECK ADD  CONSTRAINT [FK_Monitor_Package] FOREIGN KEY([PackageId])
REFERENCES [vmg].[Package] ([PackageId])
GO

ALTER TABLE [vmg].[Monitor] CHECK CONSTRAINT [FK_Monitor_Package]
GO

ALTER TABLE [vmg].[Monitor] ADD  CONSTRAINT [DF_Monitor_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO


