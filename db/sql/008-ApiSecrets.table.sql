/****** Object:  Table [auth].[ApiSecrets]    Script Date: 5/3/2017 10:24:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'ApiSecrets')
CREATE TABLE [auth].[ApiSecrets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Expiration] [datetime2](7) NULL,
	[Type] [nvarchar](250) NULL,
	[Value] [nvarchar](2000) NULL,
 CONSTRAINT [PK_ApiSecrets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiSecrets_ApiResourceId]    Script Date: 1/24/2018 9:29:48 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiSecrets_ApiResourceId] ON [auth].[ApiSecrets]
(
	[ApiResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[FK_ApiSecrets_ApiResources_ApiResourceId]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [auth].[ApiSecrets]  WITH CHECK ADD  CONSTRAINT [FK_ApiSecrets_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [auth].[ApiResources] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[ApiSecrets] CHECK CONSTRAINT [FK_ApiSecrets_ApiResources_ApiResourceId]
GO


