CREATE TABLE [dbo].[QuizExamples] (
    [Id]            INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizId]        INT NOT NULL,
    [WordUsageId]   INT NOT NULL,
    CONSTRAINT [FK_QuizExamplesWordUsageId_ToWordUsages] FOREIGN KEY (WordUsageId) REFERENCES [dbo].WordUsages ([Id]),
    CONSTRAINT [FK_QuizExamplesQuizId_ToQuizes]          FOREIGN KEY (QuizId)      REFERENCES [dbo].Quizzes ([Id])
);

