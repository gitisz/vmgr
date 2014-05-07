USE [DOM_Config]
GO
/****** Object:  Table [vmg].[Filter]    Script Date: 01/06/2014 14:54:06 ******/
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
/****** Object:  Default [DF_Filter_CreateDate]    Script Date: 01/06/2014 14:54:06 ******/
ALTER TABLE [vmg].[Filter] ADD  CONSTRAINT [DF_Filter_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  ForeignKey [FK_Filter_Server]    Script Date: 01/06/2014 14:54:06 ******/
ALTER TABLE [vmg].[Filter]  WITH CHECK ADD  CONSTRAINT [FK_Filter_Server] FOREIGN KEY([ServerId])
REFERENCES [vmg].[Server] ([ServerId])
GO
ALTER TABLE [vmg].[Filter] CHECK CONSTRAINT [FK_Filter_Server]
GO
