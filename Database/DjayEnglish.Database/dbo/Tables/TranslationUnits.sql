CREATE TABLE [dbo].[TranslationUnits] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [Spelling]     NVARCHAR (40) NOT NULL,
    [PartOfSpeech] TINYINT       NOT NULL,
    [Language]     TINYINT       NOT NULL,
    [IsActive]     BIT           NOT NULL
);

