/****** Object:  Table [auth].[ApiClaims]    Script Date: 5/3/2017 10:23:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'ApiClaims')
CREATE TABLE [auth].[ApiClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ApiClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'ApiClaims' and CONSTRAINT_Name = 'FK_ApiClaims_ApiResources_ApiResourceId' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[ApiClaims]  WITH CHECK ADD CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [auth].[ApiResources] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[ApiClaims] CHECK CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId]
GO


