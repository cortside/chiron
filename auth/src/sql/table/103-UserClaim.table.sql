/****** Object:  Table [auth].[UserClaim]    Script Date: 5/3/2017 10:34:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'UserClaim')
CREATE TABLE [auth].[UserClaim](
	[UserClaimId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] uniqueidentifier NOT NULL,
	ProviderName varchar(50) NULL,
	[Type] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_UserClaim] PRIMARY KEY CLUSTERED 
(
	[UserClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserClaim_UserId]    Script Date: 1/24/2018 9:29:48 AM ******/
CREATE NONCLUSTERED INDEX [IX_UserClaim_UserId] ON [auth].[UserClaim]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[FK_UserClaim_Users_UserId]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [auth].[UserClaim]  WITH CHECK ADD  CONSTRAINT [FK_UserClaim_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [auth].[User] ([UserId])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[UserClaim] CHECK CONSTRAINT [FK_UserClaim_Users_UserId]
GO


