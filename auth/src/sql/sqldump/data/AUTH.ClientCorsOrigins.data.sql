
SET IDENTITY_INSERT [AUTH].[ClientCorsOrigins] ON 

INSERT [AUTH].[ClientCorsOrigins] ([Id], [ClientId], [Origin]) VALUES (1, 4, N'http://localhost:5003')
SET IDENTITY_INSERT [AUTH].[ClientCorsOrigins] OFF
