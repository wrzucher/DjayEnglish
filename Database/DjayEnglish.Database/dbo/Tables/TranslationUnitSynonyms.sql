CREATE TABLE [dbo].[TranslationUnitSynonyms] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitId]        INT NOT NULL,
    [SynonymTranslationUnitId] INT NOT NULL,
    [IsActive]                 BIT NOT NULL,
    CONSTRAINT [FK_TranslationUnitSynonymsTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([TranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id]),
    CONSTRAINT [FK_TranslationUnitSynonymsSynonymTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([SynonymTranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id])
);

