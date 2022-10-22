BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[QBank].[Author]'))
    SET IDENTITY_INSERT [QBank].[Author] ON;
INSERT INTO [QBank].[Author] ([Id], [Name])
VALUES (1, N'Test');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[QBank].[Author]'))
    SET IDENTITY_INSERT [QBank].[Author] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'Difficulty', N'Name') AND [object_id] = OBJECT_ID(N'[QBank].[Topic]'))
    SET IDENTITY_INSERT [QBank].[Topic] ON;
INSERT INTO [QBank].[Topic] ([Id], [AuthorId], [Difficulty], [Name])
VALUES (1, 1, 1, N'Tema1');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'Difficulty', N'Name') AND [object_id] = OBJECT_ID(N'[QBank].[Topic]'))
    SET IDENTITY_INSERT [QBank].[Topic] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Answer', N'Difficulty', N'MultimediaId', N'Text', N'TopicId', N'Type') AND [object_id] = OBJECT_ID(N'[QBank].[Question]'))
    SET IDENTITY_INSERT [QBank].[Question] ON;
INSERT INTO [QBank].[Question] ([Id], [Answer], [Difficulty], [MultimediaId], [Text], [TopicId], [Type])
VALUES (1, N'Answer1!', 1, N'00000000-0000-0000-0000-000000000000', N'Question1?', 1, 1),
(2, N'Answer2!', 2, N'00000000-0000-0000-0000-000000000000', N'Question2?', 1, 1),
(3, N'Answer3!', 3, N'00000000-0000-0000-0000-000000000000', N'Question3?', 1, 1),
(4, N'Answer4!', 4, N'00000000-0000-0000-0000-000000000000', N'Question4?', 1, 1),
(5, N'Answer5!', 5, N'00000000-0000-0000-0000-000000000000', N'Question5?', 1, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Answer', N'Difficulty', N'MultimediaId', N'Text', N'TopicId', N'Type') AND [object_id] = OBJECT_ID(N'[QBank].[Question]'))
    SET IDENTITY_INSERT [QBank].[Question] OFF;
GO

INSERT INTO [QBank].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220623131046_AddInitialData', N'6.0.6');
GO

COMMIT;
GO

