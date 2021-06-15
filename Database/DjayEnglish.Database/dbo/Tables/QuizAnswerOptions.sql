CREATE TABLE [dbo].[QuizAnswerOptions] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizId]        INT             NOT NULL,
    [Text]          NVARCHAR (200)  NOT NULL,
    [IsRightAnswer] BIT             NOT NULL,
    CONSTRAINT [FK_QuizAnswerOptionsQuizId_ToQuizes] FOREIGN KEY (QuizId) REFERENCES [dbo].Quizzes ([Id])
);

