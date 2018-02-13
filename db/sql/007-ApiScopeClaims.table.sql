/****** Object:  Table [auth].[ApiScopeClaims]    Script Date: 5/3/2017 10:23:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'ApiScopeClaims')
CREATE TABLE [auth].[ApiScopeClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApiScopeId] [int] NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ApiScopeClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiScopeClaims_ApiScopeId]    Script Date: 1/24/2018 9:29:48 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiScopeClaims_ApiScopeId] ON [auth].[ApiScopeClaims]
(
	[ApiScopeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'ApiScopeClaims' and CONSTRAINT_Name = 'FK_ApiScopeClaims_ApiScopes_ApiScopeId' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[ApiScopeClaims]  WITH CHECK ADD  CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ApiScopeId] FOREIGN KEY([ApiScopeId])
REFERENCES [auth].[ApiScopes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[ApiScopeClaims] CHECK CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ApiScopeId]
GO


