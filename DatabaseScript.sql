USE [master];
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SurveyAppDb')
BEGIN
    CREATE DATABASE [SurveyAppDb];
END;
GO

USE [SurveyAppDb];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [Surveys] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedById] nvarchar(450) NOT NULL,
        [ClosedAt] datetime2 NULL,
        CONSTRAINT [PK_Surveys] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Surveys_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [Questions] (
        [Id] int NOT NULL IDENTITY,
        [Text] nvarchar(max) NOT NULL,
        [QuestionType] int NOT NULL,
        [Order] int NOT NULL,
        [SurveyId] int NOT NULL,
        CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Questions_Surveys_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [Responses] (
        [Id] int NOT NULL IDENTITY,
        [SubmittedAt] datetime2 NOT NULL,
        [SurveyId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Responses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Responses_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Responses_Surveys_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [Options] (
        [Id] int NOT NULL IDENTITY,
        [Text] nvarchar(max) NOT NULL,
        [QuestionId] int NOT NULL,
        [IsCorrect] bit NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Options] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Options_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE TABLE [Answers] (
        [Id] int NOT NULL IDENTITY,
        [TextAnswer] nvarchar(max) NULL,
        [ResponseId] int NOT NULL,
        [QuestionId] int NOT NULL,
        [SelectedOptionId] int NULL,
        CONSTRAINT [PK_Answers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Answers_Options_SelectedOptionId] FOREIGN KEY ([SelectedOptionId]) REFERENCES [Options] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Answers_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Answers_Responses_ResponseId] FOREIGN KEY ([ResponseId]) REFERENCES [Responses] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Answers_QuestionId] ON [Answers] ([QuestionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Answers_ResponseId] ON [Answers] ([ResponseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Answers_SelectedOptionId] ON [Answers] ([SelectedOptionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Options_QuestionId] ON [Options] ([QuestionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Questions_SurveyId] ON [Questions] ([SurveyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Responses_SurveyId] ON [Responses] ([SurveyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Responses_UserId] ON [Responses] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Surveys_CreatedById] ON [Surveys] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306073446_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260306073446_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO



--  DATA ----
--

USE [SurveyAppDb];
GO

SET IDENTITY_INSERT [Surveys] ON;
SET IDENTITY_INSERT [Questions] ON;
SET IDENTITY_INSERT [Options] ON;
SET IDENTITY_INSERT [Responses] ON;
SET IDENTITY_INSERT [Answers] ON;
GO

-- AspNetRoles
IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [Id]='5b251d84-4c73-4a4f-bc21-b0ac90ce36b7')
  INSERT INTO [AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp]) VALUES('5b251d84-4c73-4a4f-bc21-b0ac90ce36b7','Admin','ADMIN',NULL);
IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [Id]='69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb')
  INSERT INTO [AspNetRoles]([Id],[Name],[NormalizedName],[ConcurrencyStamp]) VALUES('69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb','User','USER',NULL);
GO

-- AspNetUsers
IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [Id]='084ee4de-ffcf-465d-af9f-8f0a18b564c3')
  INSERT INTO [AspNetUsers]([Id],[FullName],[CreatedAt],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
  VALUES('084ee4de-ffcf-465d-af9f-8f0a18b564c3','anhnt','2026-03-08 16:28:34.2479639','sicano20@gmail.com','SICANO20@GMAIL.COM','sicano20@gmail.com','SICANO20@GMAIL.COM',1,'AQAAAAIAAYagAAAAEGp0i1hvgjGUb+Pwh85xtclZGr250cC9t27VSd/982XczL1RYnAvroLtxQQWhYu0XQ==','IWFJKND3TCNTPSZP4ZJLLNPM6KU5SUV2','a1df0d00-2b01-4973-a5eb-b41ba0019704',NULL,0,0,1,0);
IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [Id]='19f5de7b-069a-498c-aaf0-ea40f2b4a13c')
  INSERT INTO [AspNetUsers]([Id],[FullName],[CreatedAt],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
  VALUES('19f5de7b-069a-498c-aaf0-ea40f2b4a13c','aaa','2026-03-06 07:40:06.0141474','anhnt@gmail.com','ANHNT@GMAIL.COM','anhnt@gmail.com','ANHNT@GMAIL.COM',1,'AQAAAAIAAYagAAAAEMZ9y/xY7EYWPN21Y4SpKLAaqTNH6x+UaTJnQ/u8XwG7frmzPQpGSwVeMiDXqJu4pw==','RML6ZSKETIIXCYPBHEIRQC276HLJT3UB','ae4351cc-257a-4341-9d41-6394332fe79e',NULL,0,0,1,0);
IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [Id]='2f95f4be-1d1b-495b-b184-3d92acee1b9e')
  INSERT INTO [AspNetUsers]([Id],[FullName],[CreatedAt],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
  VALUES('2f95f4be-1d1b-495b-b184-3d92acee1b9e','Administrator','2026-03-06 07:36:41.7220556','admin@survey.com','ADMIN@SURVEY.COM','admin@survey.com','ADMIN@SURVEY.COM',1,'AQAAAAIAAYagAAAAEO76HrYDV7q3sPSYLrNR3LnSghT8UI69IKhAH2E8ISn1am8D0ezpii+/E6JGuqVVAQ==','WSPSP5MJGBSFIKRLNT5DJVU3RSM7FWQT','5642695a-fd37-4673-8971-67b4a6c153df',NULL,0,0,1,0);
IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [Id]='6277e92b-63a1-4203-8824-ade6cf65115e')
  INSERT INTO [AspNetUsers]([Id],[FullName],[CreatedAt],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
  VALUES('6277e92b-63a1-4203-8824-ade6cf65115e','anhnt','2026-03-17 03:57:20.4634028','tkltc001@gmail.com','TKLTC001@GMAIL.COM','tkltc001@gmail.com','TKLTC001@GMAIL.COM',1,'AQAAAAIAAYagAAAAEL14rqssTHq/VNLOZ1D1h4fazjoTKHFX7SyjExlwxnKafckV46v8/ZsqRlYuMmbKRQ==','ER3XIDHHR3Y6437KNTPAHCIF2ZQPRUBQ','25ae481f-9904-4025-99f0-155f29f7ebc2',NULL,0,0,1,0);
IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [Id]='f40f6f26-4830-4309-bdff-4962a3da3101')
  INSERT INTO [AspNetUsers]([Id],[FullName],[CreatedAt],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
  VALUES('f40f6f26-4830-4309-bdff-4962a3da3101','trump','2026-03-18 03:57:59.7458253','nguyentuananh12e.lqd@gmail.com','NGUYENTUANANH12E.LQD@GMAIL.COM','nguyentuananh12e.lqd@gmail.com','NGUYENTUANANH12E.LQD@GMAIL.COM',1,'AQAAAAIAAYagAAAAEIhq+j+EtcFj9paChVBbsT4Wej95siGRqlDx0dc8dSP8uuJA+nMam6pQd5FRY3Vk9A==','6HDBWXU3PTHKUM3KXYKSVXDU7JJNRLCX','a3de39f6-628d-4346-913b-1b15d89b02e8',NULL,0,0,1,0);
GO

-- AspNetUserRoles
IF NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId]='2f95f4be-1d1b-495b-b184-3d92acee1b9e' AND [RoleId]='5b251d84-4c73-4a4f-bc21-b0ac90ce36b7')
  INSERT INTO [AspNetUserRoles]([UserId],[RoleId]) VALUES('2f95f4be-1d1b-495b-b184-3d92acee1b9e','5b251d84-4c73-4a4f-bc21-b0ac90ce36b7');
IF NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId]='084ee4de-ffcf-465d-af9f-8f0a18b564c3' AND [RoleId]='69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb')
  INSERT INTO [AspNetUserRoles]([UserId],[RoleId]) VALUES('084ee4de-ffcf-465d-af9f-8f0a18b564c3','69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb');
IF NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId]='19f5de7b-069a-498c-aaf0-ea40f2b4a13c' AND [RoleId]='69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb')
  INSERT INTO [AspNetUserRoles]([UserId],[RoleId]) VALUES('19f5de7b-069a-498c-aaf0-ea40f2b4a13c','69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb');
IF NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId]='6277e92b-63a1-4203-8824-ade6cf65115e' AND [RoleId]='69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb')
  INSERT INTO [AspNetUserRoles]([UserId],[RoleId]) VALUES('6277e92b-63a1-4203-8824-ade6cf65115e','69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb');
IF NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId]='f40f6f26-4830-4309-bdff-4962a3da3101' AND [RoleId]='69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb')
  INSERT INTO [AspNetUserRoles]([UserId],[RoleId]) VALUES('f40f6f26-4830-4309-bdff-4962a3da3101','69bdfaa5-6ea9-4b9d-8065-f5ab82a019fb');
GO

-- Surveys
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=1)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(1,'test','discription test','2026-03-06 07:40:25.1236027',1,'19f5de7b-069a-498c-aaf0-ea40f2b4a13c',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=2)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(2,'test','description test','2026-03-17 03:56:07.6309468',1,'2f95f4be-1d1b-495b-b184-3d92acee1b9e',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=3)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(3,'test2','test@#','2026-03-17 03:57:41.1251989',1,'6277e92b-63a1-4203-8824-ade6cf65115e',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=4)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(4,'test3',NULL,'2026-03-17 03:58:36.7027779',1,'6277e92b-63a1-4203-8824-ade6cf65115e',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=5)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(5,'bạn nghĩ sao về việc AI đang dần thay thế junior',NULL,'2026-03-17 04:03:13.1333431',1,'084ee4de-ffcf-465d-af9f-8f0a18b564c3',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=6)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(6,'test','test','2026-03-18 04:03:48.7099674',1,'2f95f4be-1d1b-495b-b184-3d92acee1b9e',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=7)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(7,'test delete','đây là description','2026-03-19 03:42:10.3506492',0,'6277e92b-63a1-4203-8824-ade6cf65115e',NULL);
IF NOT EXISTS (SELECT 1 FROM [Surveys] WHERE [Id]=8)
  INSERT INTO [Surveys]([Id],[Title],[Description],[CreatedAt],[IsActive],[CreatedById],[ClosedAt]) VALUES(8,'Test Survey DB','Testing DB write','2026-03-21 03:49:23.4878833',1,'2f95f4be-1d1b-495b-b184-3d92acee1b9e',NULL);
GO

-- Questions
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=1)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(1,'test câu hỏi',0,0,1);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=2)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(2,'giá trị thặng dư được tạo ra như nào',1,0,4);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=3)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(3,'người mắc hội chứng down là do thiếu nhiễm sắc thể số bao nhiêu',0,1,4);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=4)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(4,'giá trị thặng dư được tạo ra như nào',0,0,3);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=5)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(5,'giá trị thặng dư được tạo ra như nào',0,0,6);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=6)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(6,'test text',1,0,7);
IF NOT EXISTS (SELECT 1 FROM [Questions] WHERE [Id]=7)
  INSERT INTO [Questions]([Id],[Text],[QuestionType],[Order],[SurveyId]) VALUES(7,'BBan thich mau gi?',0,0,8);
GO

-- Options
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=1)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(1,'test true',1,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=2)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(2,'test failse',1,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=3)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(3,'21',3,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=4)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(4,'22',3,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=5)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(5,'27',3,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=6)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(6,'giá trị thặng dư Ngay trong quá trình sản xuất Giá trị thặng dư được tạo ra ngay trong quá trình sản xuất',4,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=7)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(7,'không biết',4,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=8)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(8,'giá trị thặng dư Ngay trong quá trình sản xuất Giá trị thặng dư được tạo ra ngay trong quá trình sản xuất',5,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=9)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(9,'không biết',5,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=10)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(10,'Do',7,0);
IF NOT EXISTS (SELECT 1 FROM [Options] WHERE [Id]=11)
  INSERT INTO [Options]([Id],[Text],[QuestionId],[IsCorrect]) VALUES(11,'Xanh',7,0);
GO

-- Responses
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=1)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(1,'2026-03-06 07:45:50.2065938',1,'2f95f4be-1d1b-495b-b184-3d92acee1b9e');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=2)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(2,'2026-03-08 16:29:12.9173625',1,'084ee4de-ffcf-465d-af9f-8f0a18b564c3');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=3)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(3,'2026-03-17 04:00:23.7439193',4,'6277e92b-63a1-4203-8824-ade6cf65115e');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=4)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(4,'2026-03-17 04:01:46.8541072',4,'084ee4de-ffcf-465d-af9f-8f0a18b564c3');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=5)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(5,'2026-03-18 03:58:16.8107096',3,'f40f6f26-4830-4309-bdff-4962a3da3101');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=6)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(6,'2026-03-18 03:58:31.9923168',3,'084ee4de-ffcf-465d-af9f-8f0a18b564c3');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=7)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(7,'2026-03-18 04:07:11.8022256',5,'6277e92b-63a1-4203-8824-ade6cf65115e');
IF NOT EXISTS (SELECT 1 FROM [Responses] WHERE [Id]=8)
  INSERT INTO [Responses]([Id],[SubmittedAt],[SurveyId],[UserId]) VALUES(8,'2026-03-18 04:07:45.7472022',1,'6277e92b-63a1-4203-8824-ade6cf65115e');
GO

-- Answers
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=1)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(1,NULL,1,1,1);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=2)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(2,NULL,2,1,1);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=3)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(3,NULL,3,2,NULL);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=4)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(4,NULL,3,3,4);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=5)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(5,NULL,4,2,NULL);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=6)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(6,NULL,4,3,4);
IF NOT EXISTS (SELECT 1 FROM [Answers] WHERE [Id]=7)
  INSERT INTO [Answers]([Id],[TextAnswer],[ResponseId],[QuestionId],[SelectedOptionId]) VALUES(7,NULL,8,1,1);
GO

SET IDENTITY_INSERT [Surveys] OFF;
SET IDENTITY_INSERT [Questions] OFF;
SET IDENTITY_INSERT [Options] OFF;
SET IDENTITY_INSERT [Responses] OFF;
SET IDENTITY_INSERT [Answers] OFF;
GO
