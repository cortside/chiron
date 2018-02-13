
SET IDENTITY_INSERT [AUTH].[ClientGrantTypes] ON 

INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (1, 1, N'client_credentials')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (2, 4, N'implicit')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (3, 2, N'password')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (4, 3, N'hybrid')
INSERT [AUTH].[ClientGrantTypes] ([Id], [ClientId], [GrantType]) VALUES (5, 3, N'client_credentials')
SET IDENTITY_INSERT [AUTH].[ClientGrantTypes] OFF
