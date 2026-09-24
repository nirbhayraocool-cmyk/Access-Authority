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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
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
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260901064317_first', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ContactViewModels] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [ServiceInterest] nvarchar(max) NULL,
    [ProjectDetails] nvarchar(max) NOT NULL,
    [PrivacyAccepted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactViewModels] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260903083052_AddContactViewModel', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CareerViewModels] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [EmailAddress] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [LinkedInProfile] nvarchar(max) NOT NULL,
    [ResumeFileName] nvarchar(max) NOT NULL,
    [ResumeFilePath] nvarchar(max) NOT NULL,
    [AppliedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_CareerViewModels] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260904122205_AddCareerViewModelnew', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CareerViewApplys] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [EmailAddress] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [LinkedInProfile] nvarchar(200) NULL,
    [ResumePath] nvarchar(500) NOT NULL,
    [AppliedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_CareerViewApplys] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260905115929_AddCareerViewApply', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CareerPositions] (
    [Id] int NOT NULL IDENTITY,
    [JobTitle] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Location] nvarchar(200) NULL,
    [Type] nvarchar(50) NULL,
    [Status] nvarchar(50) NOT NULL,
    [PostedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_CareerPositions] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260907072757_AddCareerPodition', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CareerViewModels] ADD [CareerPositionId] int NULL;
GO

CREATE INDEX [IX_CareerViewModels_CareerPositionId] ON [CareerViewModels] ([CareerPositionId]);
GO

ALTER TABLE [CareerViewModels] ADD CONSTRAINT [FK_CareerViewModels_CareerPositions_CareerPositionId] FOREIGN KEY ([CareerPositionId]) REFERENCES [CareerPositions] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260907102101_AddCareerPositionToApplications', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CreateBlogs] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [FeaturedImage] nvarchar(500) NULL,
    [Tags] nvarchar(500) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_CreateBlogs] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260908070344_AddCreateBlog', N'8.0.24');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [ShortDescription] nvarchar(500) NOT NULL,
    [HeroImage] nvarchar(max) NOT NULL,
    [Slug] nvarchar(200) NOT NULL,
    [Client] nvarchar(200) NOT NULL,
    [Industry] nvarchar(100) NOT NULL,
    [Duration] nvarchar(100) NOT NULL,
    [Team] nvarchar(100) NOT NULL,
    [Overview] nvarchar(max) NOT NULL,
    [Challenge] nvarchar(max) NOT NULL,
    [Solution] nvarchar(max) NOT NULL,
    [Result] nvarchar(max) NOT NULL,
    [Metric1Value] nvarchar(100) NOT NULL,
    [Metric1Label] nvarchar(150) NOT NULL,
    [Metric1Icon] nvarchar(100) NOT NULL,
    [Metric2Value] nvarchar(100) NOT NULL,
    [Metric2Label] nvarchar(150) NOT NULL,
    [Metric2Icon] nvarchar(100) NOT NULL,
    [Technologies] nvarchar(max) NOT NULL,
    [Features] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260911101344_AddProject', N'8.0.24');
GO

COMMIT;
GO

