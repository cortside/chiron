
SET IDENTITY_INSERT [AUTH].[ClientRedirectUris] ON 

INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (1, 3, N'http://localhost:5002/signin-oidc')
INSERT [AUTH].[ClientRedirectUris] ([Id], [ClientId], [RedirectUri]) VALUES (2, 4, N'http://localhost:5003/callback.html')
SET IDENTITY_INSERT [AUTH].[ClientRedirectUris] OFF
