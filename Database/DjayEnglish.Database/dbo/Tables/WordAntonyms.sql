CREATE TABLE [dbo].[WordAntonyms] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitId]        INT NOT NULL,
    [AntonymTranslationUnitId] INT NOT NULL,
    [IsActive]                 BIT NOT NULL,
    CONSTRAINT [FK_WordAntonymsTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([TranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id]),
    CONSTRAINT [FK_WordAntonymsAntonymTranslationUnitId_ToTranslationUnits]
        FOREIGN KEY ([AntonymTranslationUnitId])
        REFERENCES [dbo].[TranslationUnits] ([Id])
);

