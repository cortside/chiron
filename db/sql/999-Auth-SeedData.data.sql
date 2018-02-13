-- Sample seed data

SET IDENTITY_INSERT [AUTH].[Clients] ON 
INSERT [AUTH].[Clients] ([Id], [AbsoluteRefreshTokenLifetime], [AccessTokenLifetime], [AccessTokenType], [AllowAccessTokensViaBrowser], [AllowOfflineAccess], [AllowPlainTextPkce], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [AlwaysSendClientClaims], [AuthorizationCodeLifetime], [BackChannelLogoutSessionRequired], [BackChannelLogoutUri], [ClientClaimsPrefix], [ClientId], [ClientName], [ClientUri], [ConsentLifetime], [Description], [EnableLocalLogin], [Enabled], [FrontChannelLogoutSessionRequired], [FrontChannelLogoutUri], [IdentityTokenLifetime], [IncludeJwtId], [LogoUri], [PairWiseSubjectSalt], [ProtocolType], [RefreshTokenExpiration], [RefreshTokenUsage], [RequireClientSecret], [RequireConsent], [RequirePkce], [SlidingRefreshTokenLifetime], [UpdateAccessTokenClaimsOnRefresh], LogoutSessionRequired, PrefixClientClaims) 
	VALUES (1, 2592000, 3600, 0, 0, 0, 0, 1, 0, 0, 300, 1, NULL, N'client_', N'js', N'Javascript Client', NULL, NULL, NULL, 1, 1, 1, NULL, 300, 0, NULL, NULL, N'oidc', 1, 1, 1, 1, 0, 1296000, 0, 0, 0)
INSERT [AUTH].[Clients] ([Id], [AbsoluteRefreshTokenLifetime], [AccessTokenLifetime], [AccessTokenType], [AllowAccessTokensViaBrowser], [AllowOfflineAccess], [AllowPlainTextPkce], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [AlwaysSendClientClaims], [AuthorizationCodeLifetime], [BackChannelLogoutSessionRequired], [BackChannelLogoutUri], [ClientClaimsPrefix], [ClientId], [ClientName], [ClientUri], [ConsentLifetime], [Description], [EnableLocalLogin], [Enabled], [FrontChannelLogoutSessionRequired], [FrontChannelLogoutUri], [IdentityTokenLifetime], [IncludeJwtId], [LogoUri], [PairWiseSubjectSalt], [ProtocolType], [RefreshTokenExpiration], [RefreshTokenUsage], [RequireClientSecret], [RequireConsent], [RequirePkce], [SlidingRefreshTokenLifetime], [UpdateAccessTokenClaimsOnRefresh], LogoutSessionRequired, PrefixClientClaims) 
	VALUES (2, 2592000, 3600, 0, 0, 0, 0, 1, 0, 0, 300, 1, NULL, N'client_', N'customerui', N'Customer UI', NULL, NULL, NULL, 1, 1, 1, NULL, 300, 0, NULL, NULL, N'oidc', 1, 1, 1, 1, 0, 1296000, 0, 0, 0)
INSERT [AUTH].[Clients] ([Id], [AbsoluteRefreshTokenLifetime], [AccessTokenLifetime], [AccessTokenType], [AllowAccessTokensViaBrowser], [AllowOfflineAccess], [AllowPlainTextPkce], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [AlwaysSendClientClaims], [AuthorizationCodeLifetime], [BackChannelLogoutSessionRequired], [BackChannelLogoutUri], [ClientClaimsPrefix], [ClientId], [ClientName], [ClientUri], [ConsentLifetime], [Description], [EnableLocalLogin], [Enabled], [FrontChannelLogoutSessionRequired], [FrontChannelLogoutUri], [IdentityTokenLifetime], [IncludeJwtId], [LogoUri], [PairWiseSubjectSalt], [ProtocolType], [RefreshTokenExpiration], [RefreshTokenUsage], [RequireClientSecret], [RequireConsent], [RequirePkce], [SlidingRefreshTokenLifetime], [UpdateAccessTokenClaimsOnRefresh], LogoutSessionRequired, PrefixClientClaims) 
	VALUES (3, 2592000, 3600, 0, 0, 0, 0, 1, 0, 0, 300, 1, NULL, N'client_', N'hiringmanagerui', N'Hiring Manager UI', NULL, NULL, NULL, 1, 1, 1, NULL, 300, 0, NULL, NULL, N'oidc', 1, 1, 1, 1, 0, 1296000, 0, 0, 0)
INSERT [AUTH].[Clients] ([Id], [AbsoluteRefreshTokenLifetime], [AccessTokenLifetime], [AccessTokenType], [AllowAccessTokensViaBrowser], [AllowOfflineAccess], [AllowPlainTextPkce], [AllowRememberConsent], [AlwaysIncludeUserClaimsInIdToken], [AlwaysSendClientClaims], [AuthorizationCodeLifetime], [BackChannelLogoutSessionRequired], [BackChannelLogoutUri], [ClientClaimsPrefix], [ClientId], [ClientName], [ClientUri], [ConsentLifetime], [Description], [EnableLocalLogin], [Enabled], [FrontChannelLogoutSessionRequired], [FrontChannelLogoutUri], [IdentityTokenLifetime], [IncludeJwtId], [LogoUri], [PairWiseSubjectSalt], [ProtocolType], [RefreshTokenExpiration], [RefreshTokenUsage], [RequireClientSecret], [RequireConsent], [RequirePkce], [SlidingRefreshTokenLifetime], [UpdateAccessTokenClaimsOnRefresh], LogoutSessionRequired, PrefixClientClaims) 
	VALUES (5, 2592000, 3600, 0, 0, 0, 0, 1, 0, 0, 300, 1, NULL, N'client_', N'client', NULL, NULL, NULL, NULL, 1, 1, 1, NULL, 300, 0, NULL, NULL, N'oidc', 1, 1, 1, 1, 0, 1296000, 0, 0, 0)

-- too lazy to figure out which position that is....
update auth.Clients set AllowAccessTokensViaBrowser=1 where Id<5
update auth.Clients set RequireConsent=0 where Id<5 
SET IDENTITY_INSERT [AUTH].[Clients] OFF

SET IDENTITY_INSERT [AUTH].[IdentityResources] ON
INSERT [AUTH].[IdentityResources] ([Id], [Description], [DisplayName], [Emphasize], [Enabled], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (1, NULL, N'Your user identifier', 0, 1, N'openid', 1, 1)
INSERT [AUTH].[IdentityResources] ([Id], [Description], [DisplayName], [Emphasize], [Enabled], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (2, N'Your user profile information (first name, last name, etc.)', N'User profile', 1, 1, N'profile', 0, 1)
INSERT [AUTH].[IdentityResources] ([Id], [Description], [DisplayName], [Emphasize], [Enabled], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (3, N'Roles assigned to the user', N'Roles', 0, 1, N'role', 0, 1)
SET IDENTITY_INSERT [AUTH].[IdentityResources] OFF

SET IDENTITY_INSERT [AUTH].[IdentityClaims] ON 
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (1, 1, N'sub')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (2, 2, N'name')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (3, 2, N'family_name')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (4, 2, N'given_name')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (5, 2, N'middle_name')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (6, 2, N'nickname')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (7, 2, N'preferred_username')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (8, 2, N'profile')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (9, 2, N'picture')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (10, 2, N'website')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (11, 2, N'gender')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (12, 2, N'birthdate')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (13, 2, N'zoneinfo')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (14, 2, N'locale')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (15, 2, N'updated_at')
INSERT [AUTH].[IdentityClaims] ([Id], [IdentityResourceId], [Type]) VALUES (16, 3, N'role')
SET IDENTITY_INSERT [AUTH].[IdentityClaims] OFF

SET IDENTITY_INSERT [AUTH].[ApiResources] ON 
INSERT [AUTH].[ApiResources] ([Id], [Description], [DisplayName], [Enabled], [Name]) VALUES (1, NULL, N'Admin API', 1, N'adminapi')
SET IDENTITY_INSERT [AUTH].[ApiResources] OFF

SET IDENTITY_INSERT [AUTH].[ApiScopes] ON 
INSERT [AUTH].[ApiScopes] ([Id], [ApiResourceId], [Description], [DisplayName], [Emphasize], [Name], [Required], [ShowInDiscoveryDocument]) VALUES (1, 1, N'Admin API Desc', N'Admin API', 0, N'adminapi', 0, 1)
SET IDENTITY_INSERT [AUTH].[ApiScopes] OFF

SET IDENTITY_INSERT [AUTH].[ClientCorsOrigins] ON 
INSERT [AUTH].[ClientCorsOrigins] ([Id], [ClientId], [Origin]) VALUES (1, 5, N'http://localhost:5003')
INSERT [AUTH].[ClientCorsOrigins] ([Id], [ClientId], [Origin]) VALUES (2, 1, N'http://chiron.docker.local:8000')
SET IDENTITY_INSERT [AUTH].[ClientCorsOrigins] OFF

SET IDENTITY_INSERT [AUTH].[ClientGrantTypes] ON 
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (1, 1, N'implicit')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (3, 2, N'implicit')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (4, 3, N'implicit')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (5, 5, N'client_credentials')
SET IDENTITY_INSERT [AUTH].[ClientGrantTypes] OFF

SET IDENTITY_INSERT [AUTH].[ClientPostLogoutRedirectUris] ON 
INSERT [AUTH].[ClientPostLogoutRedirectUris] ([Id], [ClientId], [PostLogoutRedirectUri]) VALUES (1, 1, N'http://chiron.docker.local:8000/adminui')
--INSERT [AUTH].[ClientPostLogoutRedirectUris] ([Id], [ClientId], [PostLogoutRedirectUri]) VALUES (1, 3, N'http://localhost:5002/signout-callback-oidc')
--INSERT [AUTH].[ClientPostLogoutRedirectUris] ([Id], [ClientId], [PostLogoutRedirectUri]) VALUES (2, 4, N'http://localhost:5003/index.html')
SET IDENTITY_INSERT [AUTH].[ClientPostLogoutRedirectUris] OFF

SET IDENTITY_INSERT [AUTH].[ClientRedirectUris] ON 
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (1, 1, N'http://chiron.docker.local:8000/adminui/login')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (2, 1, N'http://prod-chiron.docker.local:8000/adminui/login')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (3, 2, N'http://chiron.docker.local:8000/customer/login')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (4, 2, N'http://prod-chiron.docker.local:8000/customer/login')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (5, 3, N'http://chiron.docker.local:8000/clerk/login')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (6, 3, N'http://prod-chiron.docker.local:8000/clerk/login')
SET IDENTITY_INSERT [AUTH].[ClientRedirectUris] OFF

SET IDENTITY_INSERT [AUTH].[ClientScopes] ON 
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (1, 1, N'openid')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (2, 1, N'profile')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (3, 1, N'adminapi')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (4, 2, N'openid')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (5, 2, N'profile')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (6, 3, N'openid')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (7, 3, N'profile')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (10, 1, N'role')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (11, 2, N'role')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (12, 3, N'role')
INSERT [AUTH].[ClientScopes] ([Id], [ClientId], [Scope]) VALUES (13, 5, N'adminapi')
SET IDENTITY_INSERT [AUTH].[ClientScopes] OFF

SET IDENTITY_INSERT [AUTH].[ClientSecrets] ON 
-- 'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=' == 'secret'
INSERT [AUTH].[ClientSecrets] ([Id], [ClientId], [Description], [Expiration], [Type], [Value]) VALUES (1, 5, NULL, NULL, N'SharedSecret', N'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=')
SET IDENTITY_INSERT [AUTH].[ClientSecrets] OFF

SET IDENTITY_INSERT [AUTH].[User] ON 
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (1, N'Active', N'system', N'7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==', N'', 0, NULL, NULL, CAST(N'2017-05-16T16:59:29.270' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-04T16:43:06.480' AS DateTime), CAST(N'2017-05-16T16:59:29.270' AS DateTime), 1, CAST(N'2017-05-16T16:59:29.270' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (2, N'Active', N'test', N'7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==', N'', 0, NULL, NULL, CAST(N'2017-05-16T16:59:29.270' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-04T16:43:06.480' AS DateTime), CAST(N'2017-05-16T16:59:29.270' AS DateTime), 1, CAST(N'2017-05-16T16:59:29.270' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (3, N'Active', N'admin', N'7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==', N'', 0, NULL, NULL, CAST(N'2017-08-04T22:08:41.080' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-04T22:08:39.957' AS DateTime), CAST(N'2017-08-04T22:08:41.080' AS DateTime), 1, CAST(N'2017-08-04T22:08:41.080' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (4, N'Active', N'clerk', N'7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==', N'', 0, NULL, NULL, CAST(N'2017-08-05T00:05:52.410' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-05T00:05:52.063' AS DateTime), CAST(N'2017-08-05T00:05:52.410' AS DateTime), 1, CAST(N'2017-08-05T00:05:52.410' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (10, N'Active', N'daffy@duck.org', N'7iaw3Ur350mqGo7jwQrpkj9hiYB3Lkc/iBml1JQODbJ6wYX4oOHV+E+IvIh/1nsUNzLDBMxfqa2Ob1f1ACio/w==', N'', 0, NULL, NULL, CAST(N'2017-08-05T00:06:56.893' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-05T00:06:56.737' AS DateTime), CAST(N'2017-08-05T00:06:56.893' AS DateTime), 1, CAST(N'2017-08-05T00:06:56.893' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (17, N'Active', N'customer@email.com', N'becpobs9iEXGPurJPxTrBuFzH247WEuxDqBnoN65cWfudYdXSCM80dAQXcCLvx08Hr/bSB2EliWCVVnLZQ8IEQ==', N'6f2f8d29-f478-4cef-9a9b-02a72f8569e8', 0, NULL, NULL, CAST(N'2017-08-28T19:09:03.947' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-28T18:56:39.443' AS DateTime), CAST(N'2017-08-28T19:09:03.947' AS DateTime), 1, CAST(N'2017-08-28T19:09:03.947' AS DateTime), 1)
INSERT [AUTH].[User] ([UserId], [UserStatus], [Username], [Password], [Salt], [LoginCount], [LastLogin], [LastLoginIPAddress], [EffectiveDate], [ExpirationDate], [SecurityToken], [TokenExpiration], [TermsOfUseAcceptanceDate], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (18, N'Active', N'clerk@email.com', N'OS3HlSFDHeq5PEryzVCA7e4Qxc/QHDDsmwPLwfzG1WAmVxG3as9vgcyphFR6Ly9agtj64zcNipzYmP0EWkNAgA==', N'7a32492f-2f66-4f2d-a5a3-b750547df37f', 0, NULL, NULL, CAST(N'2017-08-28T19:09:05.950' AS DateTime), NULL, NULL, NULL, CAST(N'2017-08-28T18:57:19.560' AS DateTime), CAST(N'2017-08-28T19:09:05.950' AS DateTime), 1, CAST(N'2017-08-28T19:09:05.950' AS DateTime), 1)
SET IDENTITY_INSERT [AUTH].[User] OFF

SET IDENTITY_INSERT [AUTH].[Role] ON 
INSERT [AUTH].[Role] ([RoleId], [Name], [Description], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (1, N'customer', N'customer', CAST(N'2017-08-17T18:21:12.353' AS DateTime), 1, CAST(N'2017-08-17T18:21:12.353' AS DateTime), 1)
INSERT [AUTH].[Role] ([RoleId], [Name], [Description], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (2, N'clerk', N'clerk', CAST(N'2017-08-17T18:21:12.383' AS DateTime), 1, CAST(N'2017-08-17T18:21:12.383' AS DateTime), 1)
INSERT [AUTH].[Role] ([RoleId], [Name], [Description], [CreateDate], [CreateUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (3, N'admin', N'admin', CAST(N'2017-08-17T18:21:12.353' AS DateTime), 1, CAST(N'2017-08-17T18:21:12.353' AS DateTime), 1)
SET IDENTITY_INSERT [AUTH].[Role] OFF

INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (1, 9)
INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (1, 17)
INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (2, 10)
INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (2, 18)

INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (1, 2)
INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (3, 3)
INSERT [AUTH].[UserRole] ([RoleId], [UserId]) VALUES (2, 4)

update auth.IdentityResources set Emphasize=1, Required=1 where id=3
update auth.clients set AlwaysIncludeUserClaimsInIdToken=1, AlwaysSendClientClaims=1 where id=1
insert into auth.ApiClaims values(1, 'role')
insert into auth.ClientClaims values(1, 'role', 'admin')


insert into auth.ClientCorsOrigins values(2, 'http://localhost:5002')
insert into auth.ClientCorsOrigins values(2, 'http://chiron.docker.local:8000')
