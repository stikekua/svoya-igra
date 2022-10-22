IF OBJECT_ID(N'[QBank].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'QBank') IS NULL EXEC(N'CREATE SCHEMA [QBank];');
    CREATE TABLE [QBank].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'QBank') IS NULL EXEC(N'CREATE SCHEMA [QBank];');
GO

CREATE TABLE [QBank].[Author] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Author] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [QBank].[Topic] (
    [Id] int NOT NULL IDENTITY,
    [AuthorId] int NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Difficulty] int NOT NULL,
    CONSTRAINT [PK_Topic] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Topic_Author_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [QBank].[Author] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [QBank].[Question] (
    [Id] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Difficulty] int NOT NULL,
    [TopicId] int NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [MultimediaId] nchar(36) NOT NULL,
    [Answer] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Question_Topic_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [QBank].[Topic] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Question_TopicId] ON [QBank].[Question] ([TopicId]);
GO

CREATE INDEX [IX_Topic_AuthorId] ON [QBank].[Topic] ([AuthorId]);
GO

INSERT INTO [QBank].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220623131002_CreateTables', N'6.0.6');
GO

COMMIT;
GO

