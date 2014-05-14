USE [DOM_Config]
GO

/****** Object:  Table [vmg].[Monitor]    Script Date: 05/14/2014 08:30:38 ******/
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


