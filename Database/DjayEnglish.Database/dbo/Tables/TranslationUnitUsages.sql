CREATE TABLE [dbo].[TranslationUnitUsages] (
    [Id]                          INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitDefinitionId] INT NOT NULL,
    [Example]                     NVARCHAR(700) NOT NULL,
    [IsActive]                    BIT NOT NULL,
    CONSTRAINT [FK_TranslationUnitUsagesTranslationUnitDefinitionId_ToTranslationUnitDefinitions]
        FOREIGN KEY (TranslationUnitDefinitionId)
        REFERENCES [dbo].TranslationUnitDefinitions ([Id])
);

