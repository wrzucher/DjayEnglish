CREATE TABLE [dbo].[Words] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [Word]         NVARCHAR (40) NOT NULL,
    [PartOfSpeech] TINYINT       NOT NULL,
    [Language]     TINYINT       NOT NULL,
    [IsActive]     BIT           NOT NULL
);

