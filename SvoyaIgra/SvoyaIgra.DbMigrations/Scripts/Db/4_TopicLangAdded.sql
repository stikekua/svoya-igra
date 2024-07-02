BEGIN TRANSACTION;
GO

ALTER TABLE [QBank].[Topic] ADD [Lang] nvarchar(2) NOT NULL DEFAULT N'ru';
GO

UPDATE [QBank].[Topic] SET [Lang] = N'ru'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [QBank].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240702191758_TopicLangAdded', N'6.0.6');
GO

COMMIT;
GO

