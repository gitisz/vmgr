USE [DOM_Config]
GO
/****** Object:  Table [vmg].[SecurityRole]    Script Date: 01/06/2014 13:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityRole](
	[SecurityRoleId] [int] IDENTITY(10000,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityRole] PRIMARY KEY CLUSTERED 
(
	[SecurityRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_SecurityRole] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [vmg].[SecurityRole] ON
INSERT [vmg].[SecurityRole] ([SecurityRoleId], [Name], [Description], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10000, N'Administrators', N'May perform most actions in the V-Mananger site.', 1, CAST(0x0000A29200000000 AS DateTime), N'SYSTEM', NULL, NULL)
INSERT [vmg].[SecurityRole] ([SecurityRoleId], [Name], [Description], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10001, N'Schedulers', N'Able to create and modify schedules.', 1, CAST(0x0000A29600E7FEE5 AS DateTime), N'ISZLAND\SPADMIN', NULL, NULL)
INSERT [vmg].[SecurityRole] ([SecurityRoleId], [Name], [Description], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10002, N'Viewers', N'Able to view packages, schedules, and jobs, but not modify them.', 1, CAST(0x0000A29600E89620 AS DateTime), N'ISZLAND\SPADMIN', NULL, NULL)
SET IDENTITY_INSERT [vmg].[SecurityRole] OFF
/****** Object:  Table [vmg].[SecurityPermission]    Script Date: 01/06/2014 13:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityPermission](
	[SecurityPermissionId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[SecurityPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Permission] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [vmg].[SecurityPermission] ([SecurityPermissionId], [Name], [Description]) VALUES (1, N'PageDashboard', N'Access to Dashboard.aspx')
/****** Object:  Table [vmg].[SecuritySiteMap]    Script Date: 01/06/2014 13:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecuritySiteMap](
	[SecuritySiteMapId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityPermissionId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](255) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](255) NULL,
 CONSTRAINT [PK_SecuritySiteMap] PRIMARY KEY CLUSTERED 
(
	[SecuritySiteMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [vmg].[SecurityRolePermission]    Script Date: 01/06/2014 13:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityRolePermission](
	[SecurityRolePermissionId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityRoleId] [int] NOT NULL,
	[SecurityPermissionId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityRolePermission] PRIMARY KEY CLUSTERED 
(
	[SecurityRolePermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_SecurityRolePermission] UNIQUE NONCLUSTERED 
(
	[SecurityRoleId] ASC,
	[SecurityPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [vmg].[SecurityRolePermission] ON
INSERT [vmg].[SecurityRolePermission] ([SecurityRolePermissionId], [SecurityRoleId], [SecurityPermissionId], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10000, 10000, 1, 1, CAST(0x0000A29600FC68AB AS DateTime), N'SYSTEM', CAST(0x0000A296010BDACE AS DateTime), N'ISZLAND\SPADMIN')
SET IDENTITY_INSERT [vmg].[SecurityRolePermission] OFF
/****** Object:  Table [vmg].[SecurityMembership]    Script Date: 01/06/2014 13:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [vmg].[SecurityMembership](
	[SecurityMembershipId] [int] IDENTITY(10000,1) NOT NULL,
	[SecurityRoleId] [int] NOT NULL,
	[Account] [nvarchar](max) NOT NULL,
	[AccountType] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_SecurityMembership] PRIMARY KEY CLUSTERED 
(
	[SecurityMembershipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [vmg].[SecurityMembership] ON
INSERT [vmg].[SecurityMembership] ([SecurityMembershipId], [SecurityRoleId], [Account], [AccountType], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10000, 10001, N'{"EID":"MOSS0001","FirstName":"MOSS0001-FIRST","Initials":"","LastName":"MOSS0001-LAST","DisplayName":"MOSS0001 (ISZLAND 1)","Title":"","Department":"","BusinessUnit":"","BusinessCategory":"","CodeOfConductGroup":"","CodeOfConductGroupSecondary":"","CodeOfConductGroupTertiary":null,"OfficePhone":"","TieLine":"","MobilePhone":"","Pager":"","OfficeLocation":"","Floor":"","State":"","ActiveDirectoryPath":"LDAP://CN=MOSS0001,CN=Users,DC=ISZLAND,DC=COM","Email":"moss0001@iszland.com","DirectoryPath":"LDAP://DC=iszland,DC=com","ManagerEid":"MOSS0000"}', 1, 1, CAST(0x0000A29200FD7A7E AS DateTime), N'ISZLAND\spadmin', CAST(0x0000A29600ED0246 AS DateTime), N'ISZLAND\SPADMIN')
INSERT [vmg].[SecurityMembership] ([SecurityMembershipId], [SecurityRoleId], [Account], [AccountType], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10005, 10000, N'{"Name":"IT_000","ActiveDirectoryPath":"LDAP://CN=IT_000,CN=Users,DC=ISZLAND,DC=COM","DirectoryPath":"LDAP://DC=iszland,DC=com"}', 2, 1, CAST(0x0000A29300C12575 AS DateTime), N'ISZLAND\spadmin', CAST(0x0000A29600B428F0 AS DateTime), N'ISZLAND\SPADMIN')
INSERT [vmg].[SecurityMembership] ([SecurityMembershipId], [SecurityRoleId], [Account], [AccountType], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10007, 10000, N'{"EID":"MOSS0002","FirstName":"MOSS0002-FIRST","Initials":"","LastName":"MOSS0002-LAST","DisplayName":"MOSS0002 (ISZLAND 1)","Title":"","Department":"","BusinessUnit":"","BusinessCategory":"","CodeOfConductGroup":"","CodeOfConductGroupSecondary":"","CodeOfConductGroupTertiary":null,"OfficePhone":"","TieLine":"","MobilePhone":"","Pager":"","OfficeLocation":"","Floor":"","State":"","ActiveDirectoryPath":"LDAP://CN=MOSS0002,CN=Users,DC=ISZLAND,DC=COM","Email":"moss0002@iszland.com","DirectoryPath":"LDAP://DC=iszland,DC=com","ManagerEid":"MOSS0000"}', 1, 1, CAST(0x0000A29600BABA87 AS DateTime), N'ISZLAND\SPADMIN', NULL, NULL)
INSERT [vmg].[SecurityMembership] ([SecurityMembershipId], [SecurityRoleId], [Account], [AccountType], [Active], [CreateDate], [CreateUser], [UpdateDate], [UpdateUser]) VALUES (10008, 10000, N'{"Name":"IT_001","ActiveDirectoryPath":"LDAP://CN=IT_001,CN=Users,DC=ISZLAND,DC=COM","DirectoryPath":"LDAP://DC=iszland,DC=com"}', 2, 1, CAST(0x0000A29600BC7AC4 AS DateTime), N'ISZLAND\SPADMIN', NULL, NULL)
SET IDENTITY_INSERT [vmg].[SecurityMembership] OFF
/****** Object:  Default [DF_SecurityMembership_CreateDate]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityMembership] ADD  CONSTRAINT [DF_SecurityMembership_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecurityRole_CreateDate]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityRole] ADD  CONSTRAINT [DF_SecurityRole_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecurityRolePermission_CreateDate]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission] ADD  CONSTRAINT [DF_SecurityRolePermission_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_SecuritySiteMap_CreateDate]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecuritySiteMap] ADD  CONSTRAINT [DF_SecuritySiteMap_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  ForeignKey [FK_SecurityMembership_SecurityRole]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityMembership]  WITH CHECK ADD  CONSTRAINT [FK_SecurityMembership_SecurityRole] FOREIGN KEY([SecurityRoleId])
REFERENCES [vmg].[SecurityRole] ([SecurityRoleId])
GO
ALTER TABLE [vmg].[SecurityMembership] CHECK CONSTRAINT [FK_SecurityMembership_SecurityRole]
GO
/****** Object:  ForeignKey [FK_SecurityRolePermission_SecurityPermission]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRolePermission_SecurityPermission] FOREIGN KEY([SecurityPermissionId])
REFERENCES [vmg].[SecurityPermission] ([SecurityPermissionId])
GO
ALTER TABLE [vmg].[SecurityRolePermission] CHECK CONSTRAINT [FK_SecurityRolePermission_SecurityPermission]
GO
/****** Object:  ForeignKey [FK_SecurityRolePermission_SecurityRole]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecurityRolePermission]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRolePermission_SecurityRole] FOREIGN KEY([SecurityRoleId])
REFERENCES [vmg].[SecurityRole] ([SecurityRoleId])
GO
ALTER TABLE [vmg].[SecurityRolePermission] CHECK CONSTRAINT [FK_SecurityRolePermission_SecurityRole]
GO
/****** Object:  ForeignKey [FK_SecuritySiteMap_SecurityPermission]    Script Date: 01/06/2014 13:13:42 ******/
ALTER TABLE [vmg].[SecuritySiteMap]  WITH CHECK ADD  CONSTRAINT [FK_SecuritySiteMap_SecurityPermission] FOREIGN KEY([SecurityPermissionId])
REFERENCES [vmg].[SecurityPermission] ([SecurityPermissionId])
GO
ALTER TABLE [vmg].[SecuritySiteMap] CHECK CONSTRAINT [FK_SecuritySiteMap_SecurityPermission]
GO
