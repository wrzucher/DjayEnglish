CREATE TABLE [dbo].[WordSynonyms] (
    [Id]            INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordId]        INT NOT NULL,
    [SynonymWordId] INT NOT NULL,
    [IsActive]      BIT NOT NULL,
    CONSTRAINT [FK_WordSynonymsWordId_ToWords] FOREIGN KEY ([WordId]) REFERENCES [dbo].[Words] ([Id]),
    CONSTRAINT [FK_WordSynonymsSynonymWordId_ToWords] FOREIGN KEY ([SynonymWordId]) REFERENCES [dbo].[Words] ([Id])
);

