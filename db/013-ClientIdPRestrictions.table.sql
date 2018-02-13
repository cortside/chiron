/****** Object:  Table [auth].[ClientIdPRestrictions]    Script Date: 5/3/2017 10:35:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'ClientIdPRestrictions')
CREATE TABLE [auth].[ClientIdPRestrictions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Provider] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ClientIdPRestrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[FK_ClientIdPRestrictions_Clients_ClientId]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [auth].[ClientIdPRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [auth].[Clients] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[ClientIdPRestrictions] CHECK CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId]
GO