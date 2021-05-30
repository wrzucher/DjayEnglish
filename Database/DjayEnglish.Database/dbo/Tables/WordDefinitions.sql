CREATE TABLE [dbo].[WordDefinitions] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitId]     INT            NOT NULL,
    [Definition]            NVARCHAR (700) NOT NULL,
    [IsActive]              BIT            NOT NULL DEFAULT(0),
    [SourceId]              INT            NOT NULL DEFAULT(0),
    [SourceName]            NVARCHAR (50)  NOT NULL,
    CONSTRAINT [FK_WordDefinitionsTranslationUnitId_ToTranslationUnits]
    FOREIGN KEY ([TranslationUnitId])
    REFERENCES [dbo].[TranslationUnits] ([Id])
);

