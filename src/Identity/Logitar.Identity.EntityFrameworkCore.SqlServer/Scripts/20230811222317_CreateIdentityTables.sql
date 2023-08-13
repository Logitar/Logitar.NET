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

CREATE TABLE [Roles] (
    [RoleId] int NOT NULL IDENTITY,
    [TenantId] nvarchar(255) NULL,
    [UniqueName] nvarchar(255) NOT NULL,
    [UniqueNameNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [CustomAttributes] nvarchar(max) NULL,
    [AggregateId] nvarchar(255) NOT NULL,
    [CreatedBy] nvarchar(255) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [Version] bigint NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [TenantId] nvarchar(255) NULL,
    [UniqueName] nvarchar(255) NOT NULL,
    [UniqueNameNormalized] nvarchar(255) NOT NULL,
    [Password] nvarchar(255) NULL,
    [HasPassword] bit NOT NULL,
    [PasswordChangedBy] nvarchar(255) NULL,
    [PasswordChangedOn] datetime2 NULL,
    [DisabledBy] nvarchar(255) NULL,
    [DisabledOn] datetime2 NULL,
    [IsDisabled] bit NOT NULL,
    [AuthenticatedOn] datetime2 NULL,
    [AddressStreet] nvarchar(255) NULL,
    [AddressLocality] nvarchar(255) NULL,
    [AddressRegion] nvarchar(255) NULL,
    [AddressPostalCode] nvarchar(255) NULL,
    [AddressCountry] nvarchar(255) NULL,
    [AddressFormatted] nvarchar(1536) NULL,
    [AddressVerifiedBy] nvarchar(255) NULL,
    [AddressVerifiedOn] datetime2 NULL,
    [IsAddressVerified] bit NOT NULL,
    [EmailAddress] nvarchar(255) NULL,
    [EmailAddressNormalized] nvarchar(255) NULL,
    [EmailVerifiedBy] nvarchar(255) NULL,
    [EmailVerifiedOn] datetime2 NULL,
    [IsEmailVerified] bit NOT NULL,
    [PhoneCountryCode] nvarchar(16) NULL,
    [PhoneNumber] nvarchar(32) NULL,
    [PhoneExtension] nvarchar(16) NULL,
    [PhoneE164Formatted] nvarchar(64) NULL,
    [PhoneVerifiedBy] nvarchar(255) NULL,
    [PhoneVerifiedOn] datetime2 NULL,
    [IsPhoneVerified] bit NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [FirstName] nvarchar(255) NULL,
    [MiddleName] nvarchar(255) NULL,
    [LastName] nvarchar(255) NULL,
    [FullName] nvarchar(768) NULL,
    [Nickname] nvarchar(255) NULL,
    [Birthdate] datetime2 NULL,
    [Gender] nvarchar(255) NULL,
    [Locale] nvarchar(16) NULL,
    [TimeZone] nvarchar(255) NULL,
    [Picture] nvarchar(2048) NULL,
    [Profile] nvarchar(2048) NULL,
    [Website] nvarchar(2048) NULL,
    [CustomAttributes] nvarchar(max) NULL,
    [AggregateId] nvarchar(255) NOT NULL,
    [CreatedBy] nvarchar(255) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [Version] bigint NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Sessions] (
    [SessionId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Secret] nvarchar(255) NULL,
    [IsPersistent] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [SignedOutBy] nvarchar(255) NULL,
    [SignedOutOn] datetime2 NULL,
    [CustomAttributes] nvarchar(max) NULL,
    [AggregateId] nvarchar(255) NOT NULL,
    [CreatedBy] nvarchar(255) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NOT NULL,
    [UpdatedOn] datetime2 NOT NULL,
    [Version] bigint NOT NULL,
    CONSTRAINT [PK_Sessions] PRIMARY KEY ([SessionId]),
    CONSTRAINT [FK_Sessions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [UserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Roles_AggregateId] ON [Roles] ([AggregateId]);
GO

CREATE INDEX [IX_Roles_CreatedBy] ON [Roles] ([CreatedBy]);
GO

CREATE INDEX [IX_Roles_CreatedOn] ON [Roles] ([CreatedOn]);
GO

CREATE INDEX [IX_Roles_DisplayName] ON [Roles] ([DisplayName]);
GO

CREATE UNIQUE INDEX [IX_Roles_TenantId_UniqueNameNormalized] ON [Roles] ([TenantId], [UniqueNameNormalized]) WHERE [TenantId] IS NOT NULL;
GO

CREATE INDEX [IX_Roles_UniqueName] ON [Roles] ([UniqueName]);
GO

CREATE INDEX [IX_Roles_UpdatedBy] ON [Roles] ([UpdatedBy]);
GO

CREATE INDEX [IX_Roles_UpdatedOn] ON [Roles] ([UpdatedOn]);
GO

CREATE INDEX [IX_Roles_Version] ON [Roles] ([Version]);
GO

CREATE UNIQUE INDEX [IX_Sessions_AggregateId] ON [Sessions] ([AggregateId]);
GO

CREATE INDEX [IX_Sessions_CreatedBy] ON [Sessions] ([CreatedBy]);
GO

CREATE INDEX [IX_Sessions_CreatedOn] ON [Sessions] ([CreatedOn]);
GO

CREATE INDEX [IX_Sessions_IsActive] ON [Sessions] ([IsActive]);
GO

CREATE INDEX [IX_Sessions_IsPersistent] ON [Sessions] ([IsPersistent]);
GO

CREATE INDEX [IX_Sessions_SignedOutBy] ON [Sessions] ([SignedOutBy]);
GO

CREATE INDEX [IX_Sessions_SignedOutOn] ON [Sessions] ([SignedOutOn]);
GO

CREATE INDEX [IX_Sessions_UpdatedBy] ON [Sessions] ([UpdatedBy]);
GO

CREATE INDEX [IX_Sessions_UpdatedOn] ON [Sessions] ([UpdatedOn]);
GO

CREATE INDEX [IX_Sessions_UserId] ON [Sessions] ([UserId]);
GO

CREATE INDEX [IX_Sessions_Version] ON [Sessions] ([Version]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
GO

CREATE INDEX [IX_Users_AddressFormatted] ON [Users] ([AddressFormatted]);
GO

CREATE INDEX [IX_Users_AddressVerifiedBy] ON [Users] ([AddressVerifiedBy]);
GO

CREATE UNIQUE INDEX [IX_Users_AggregateId] ON [Users] ([AggregateId]);
GO

CREATE INDEX [IX_Users_AuthenticatedOn] ON [Users] ([AuthenticatedOn]);
GO

CREATE INDEX [IX_Users_Birthdate] ON [Users] ([Birthdate]);
GO

CREATE INDEX [IX_Users_CreatedBy] ON [Users] ([CreatedBy]);
GO

CREATE INDEX [IX_Users_CreatedOn] ON [Users] ([CreatedOn]);
GO

CREATE INDEX [IX_Users_DisabledBy] ON [Users] ([DisabledBy]);
GO

CREATE INDEX [IX_Users_DisabledOn] ON [Users] ([DisabledOn]);
GO

CREATE INDEX [IX_Users_EmailAddress] ON [Users] ([EmailAddress]);
GO

CREATE INDEX [IX_Users_EmailVerifiedBy] ON [Users] ([EmailVerifiedBy]);
GO

CREATE INDEX [IX_Users_FirstName] ON [Users] ([FirstName]);
GO

CREATE INDEX [IX_Users_FullName] ON [Users] ([FullName]);
GO

CREATE INDEX [IX_Users_Gender] ON [Users] ([Gender]);
GO

CREATE INDEX [IX_Users_HasPassword] ON [Users] ([HasPassword]);
GO

CREATE INDEX [IX_Users_IsConfirmed] ON [Users] ([IsConfirmed]);
GO

CREATE INDEX [IX_Users_IsDisabled] ON [Users] ([IsDisabled]);
GO

CREATE INDEX [IX_Users_LastName] ON [Users] ([LastName]);
GO

CREATE INDEX [IX_Users_Locale] ON [Users] ([Locale]);
GO

CREATE INDEX [IX_Users_MiddleName] ON [Users] ([MiddleName]);
GO

CREATE INDEX [IX_Users_Nickname] ON [Users] ([Nickname]);
GO

CREATE INDEX [IX_Users_PasswordChangedBy] ON [Users] ([PasswordChangedBy]);
GO

CREATE INDEX [IX_Users_PasswordChangedOn] ON [Users] ([PasswordChangedOn]);
GO

CREATE INDEX [IX_Users_PhoneE164Formatted] ON [Users] ([PhoneE164Formatted]);
GO

CREATE INDEX [IX_Users_PhoneVerifiedBy] ON [Users] ([PhoneVerifiedBy]);
GO

CREATE INDEX [IX_Users_TenantId_EmailAddressNormalized] ON [Users] ([TenantId], [EmailAddressNormalized]);
GO

CREATE UNIQUE INDEX [IX_Users_TenantId_UniqueNameNormalized] ON [Users] ([TenantId], [UniqueNameNormalized]) WHERE [TenantId] IS NOT NULL;
GO

CREATE INDEX [IX_Users_TimeZone] ON [Users] ([TimeZone]);
GO

CREATE INDEX [IX_Users_UniqueName] ON [Users] ([UniqueName]);
GO

CREATE INDEX [IX_Users_UpdatedBy] ON [Users] ([UpdatedBy]);
GO

CREATE INDEX [IX_Users_UpdatedOn] ON [Users] ([UpdatedOn]);
GO

CREATE INDEX [IX_Users_Version] ON [Users] ([Version]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230811222317_CreateIdentityTables', N'7.0.9');
GO

COMMIT;
GO
