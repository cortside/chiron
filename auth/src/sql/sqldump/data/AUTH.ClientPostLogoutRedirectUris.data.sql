
SET IDENTITY_INSERT [AUTH].[ClientPostLogoutRedirectUris] ON 

INSERT [AUTH].[ClientPostLogoutRedirectUris] ([Id], [ClientId], [PostLogoutRedirectUri]) VALUES (1, 3, N'http://localhost:5002/signout-callback-oidc')
INSERT [AUTH].[ClientPostLogoutRedirectUris] ([Id], [ClientId], [PostLogoutRedirectUri]) VALUES (2, 4, N'http://localhost:5003/index.html')
SET IDENTITY_INSERT [AUTH].[ClientPostLogoutRedirectUris] OFF
