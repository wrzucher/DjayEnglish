CREATE TABLE [dbo].[TranslationUnitAntonyms] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitId]        INT NOT NULL,
    [AntonymTranslationUnitId] INT NOT NULL,
    [IsActive]                 BIT NOT NULL,
    CONSTRAINT [FK_TranslationUnitAntonymsTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([TranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id]),
    CONSTRAINT [FK_TranslationUnitAntonymsAntonymTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([AntonymTranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id])
);

