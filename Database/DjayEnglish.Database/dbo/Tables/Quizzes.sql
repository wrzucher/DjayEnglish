CREATE TABLE [dbo].[Quizzes] (
    [Id]                           INT                IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [TranslationUnitDefinitionId]  INT                NOT NULL,
    [Question]                     NVARCHAR (250)     NOT NULL,
    [IsActive]                     BIT                NOT NULL,
    [Created]                      DATETIMEOFFSET (7) NOT NULL,
    [Closed]                       DATETIMEOFFSET (7) NULL,
    [QuestionShowType]             TINYINT            NOT NULL DEFAULT(0),
    [ExampleShowType]              TINYINT            NOT NULL DEFAULT(0),
    [AnswerShowType]               TINYINT            NOT NULL DEFAULT(0),
    [HasAudioFiles]                BIT                NOT NULL DEFAULT(0),
    CONSTRAINT [FK_QuizzesTranslationUnitDefinitionId_ToTranslationUnitDefinitions]
        FOREIGN KEY (TranslationUnitDefinitionId)
        REFERENCES [dbo].TranslationUnitDefinitions ([Id])
);
