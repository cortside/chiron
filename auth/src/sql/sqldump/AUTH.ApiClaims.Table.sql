USE [chiron]
GO
/****** Object:  Table [AUTH].[ApiClaims]    Script Date: 1/24/2018 9:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AUTH].[ApiClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ApiClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [AUTH].[ApiClaims]  WITH CHECK ADD  CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [AUTH].[ApiResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AUTH].[ApiClaims] CHECK CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId]
GO
