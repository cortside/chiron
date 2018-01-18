/****** Object:  Table [auth].[Clients]    Script Date: 5/3/2017 10:36:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'Clients')
CREATE TABLE [auth].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AbsoluteRefreshTokenLifetime] [int] NOT NULL,
	[AccessTokenLifetime] [int] NOT NULL,
	[AccessTokenType] [int] NOT NULL,
	[AllowAccessTokensViaBrowser] [bit] NOT NULL,
	[AllowOfflineAccess] [bit] NOT NULL,
	[AllowPlainTextPkce] [bit] NOT NULL,
	[AllowRememberConsent] [bit] NOT NULL,
	[AlwaysIncludeUserClaimsInIdToken] [bit] NOT NULL,
	[AlwaysSendClientClaims] [bit] NOT NULL,
	[AuthorizationCodeLifetime] [int] NOT NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[ClientName] [nvarchar](200) NULL,
	[ClientUri] [nvarchar](2000) NULL,
	[EnableLocalLogin] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[IdentityTokenLifetime] [int] NOT NULL,
	[IncludeJwtId] [bit] NOT NULL,
	[LogoUri] [nvarchar](max) NULL,
	[LogoutSessionRequired] [bit] NOT NULL,
	[LogoutUri] [nvarchar](max) NULL,
	[PrefixClientClaims] [bit] NOT NULL,
	[ProtocolType] [nvarchar](200) NOT NULL,
	[RefreshTokenExpiration] [int] NOT NULL,
	[RefreshTokenUsage] [int] NOT NULL,
	[RequireClientSecret] [bit] NOT NULL,
	[RequireConsent] [bit] NOT NULL,
	[RequirePkce] [bit] NOT NULL,
	[SlidingRefreshTokenLifetime] [int] NOT NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


