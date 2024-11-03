USE [P10_MediLabo_Patient-back]
GO
SET IDENTITY_INSERT [dbo].[Addresses] ON 
GO
INSERT [dbo].[Addresses] ([Id], [AddressContent]) VALUES (1, N'1 Brookside St')
GO
INSERT [dbo].[Addresses] ([Id], [AddressContent]) VALUES (2, N'2 High St')
GO
INSERT [dbo].[Addresses] ([Id], [AddressContent]) VALUES (3, N'3 Club Road')
GO
INSERT [dbo].[Addresses] ([Id], [AddressContent]) VALUES (4, N'4 Valley Dr')
GO
SET IDENTITY_INSERT [dbo].[Addresses] OFF
GO
SET IDENTITY_INSERT [dbo].[Patients] ON 
GO
INSERT [dbo].[Patients] ([Id], [Firstname], [Lastname], [BirthDate], [Gender], [AddressId], [PhoneNumber]) VALUES (1, N'Test', N'TestNone', CAST(N'1966-12-31T00:00:00.0000000' AS DateTime2), N'F', 1, N'100-222-3333')
GO
INSERT [dbo].[Patients] ([Id], [Firstname], [Lastname], [BirthDate], [Gender], [AddressId], [PhoneNumber]) VALUES (2, N'Test', N'TestBorderline', CAST(N'1945-06-24T00:00:00.0000000' AS DateTime2), N'M', 2, N'200-333-4444')
GO
INSERT [dbo].[Patients] ([Id], [Firstname], [Lastname], [BirthDate], [Gender], [AddressId], [PhoneNumber]) VALUES (3, N'Test', N'TestInDanger', CAST(N'2004-06-18T00:00:00.0000000' AS DateTime2), N'M', 3, N'300-444-5555')
GO
INSERT [dbo].[Patients] ([Id], [Firstname], [Lastname], [BirthDate], [Gender], [AddressId], [PhoneNumber]) VALUES (4, N'Test', N'TestEarlyOnset', CAST(N'2002-06-28T00:00:00.0000000' AS DateTime2), N'F', 4, N'400-555-6666')
GO
SET IDENTITY_INSERT [dbo].[Patients] OFF
GO
