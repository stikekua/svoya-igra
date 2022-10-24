IF OBJECT_ID(N'[Game].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'Game') IS NULL EXEC(N'CREATE SCHEMA [Game];');
    CREATE TABLE [Game].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'Game') IS NULL EXEC(N'CREATE SCHEMA [Game];');
GO

CREATE TABLE [Game].[GameSessions] (
    [Id] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ParametersConfig] nvarchar(max) NULL,
    [TopicsConfig] nvarchar(max) NULL,
    CONSTRAINT [PK_GameSessions] PRIMARY KEY ([Id])
);
GO

INSERT INTO [Game].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221024205616_CreateTable', N'6.0.6');
GO

COMMIT;
GO

