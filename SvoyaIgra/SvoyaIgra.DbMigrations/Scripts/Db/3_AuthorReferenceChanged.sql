BEGIN TRANSACTION;
GO

ALTER TABLE [QBank].[Topic] DROP CONSTRAINT [FK_Topic_Author_AuthorId];
GO

DROP INDEX [IX_Topic_AuthorId] ON [QBank].[Topic];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[QBank].[Topic]') AND [c].[name] = N'AuthorId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [QBank].[Topic] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [QBank].[Topic] DROP COLUMN [AuthorId];
GO

ALTER TABLE [QBank].[Question] ADD [AuthorId] int NOT NULL DEFAULT 0;
GO

UPDATE [QBank].[Question] SET [AuthorId] = 1
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [QBank].[Question] SET [AuthorId] = 1
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [QBank].[Question] SET [Answer] = N'Answer3!', [AuthorId] = 1
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [QBank].[Question] SET [AuthorId] = 1
WHERE [Id] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [QBank].[Question] SET [AuthorId] = 1
WHERE [Id] = 5;
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_Question_AuthorId] ON [QBank].[Question] ([AuthorId]);
GO

ALTER TABLE [QBank].[Question] ADD CONSTRAINT [FK_Question_Author_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [QBank].[Author] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [QBank].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220623160833_AuthorReferenceChanged', N'6.0.6');
GO

COMMIT;
GO

