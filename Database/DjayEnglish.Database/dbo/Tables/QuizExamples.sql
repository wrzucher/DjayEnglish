CREATE TABLE [dbo].[QuizExamples] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizId]                   INT NOT NULL,
    [TranslationUnitUsageId]   INT NULL,
    [Text]                     NVARCHAR(700) NOT NULL,
    CONSTRAINT [FK_QuizExamplesTranslationUnitUsageId_ToTranslationUnitUsages]
        FOREIGN KEY (TranslationUnitUsageId)
        REFERENCES [dbo].TranslationUnitUsages ([Id]),
    CONSTRAINT [FK_QuizExamplesQuizId_ToQuizes]
        FOREIGN KEY (QuizId)
        REFERENCES [dbo].Quizzes ([Id])
);

