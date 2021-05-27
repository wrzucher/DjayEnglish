CREATE TABLE [dbo].[Quizzes] (
    [Id]                INT                IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordDefinitionId]  INT                NOT NULL,
    [Question]          NVARCHAR (250)     NOT NULL,
    [IsActive]          BIT                NOT NULL,
    [Created]           DATETIMEOFFSET (7) NOT NULL,
    [Closed]            DATETIMEOFFSET (7) NULL,
    [FileName]          NVARCHAR (50)      NULL,
    CONSTRAINT [FK_QuizzesWordDefinitionId_ToWordDefinitions] FOREIGN KEY (WordDefinitionId) REFERENCES [dbo].WordDefinitions ([Id])
);
