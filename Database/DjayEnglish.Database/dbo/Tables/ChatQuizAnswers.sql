CREATE TABLE [dbo].[ChatQuizAnswers] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ChatQuizId]    INT             NOT NULL,
    [AnswerId]      INT             NULL,
    CONSTRAINT [FK_ChatQuizAnswersAnswerId_ToQuizAnswerOptions]
        FOREIGN KEY (AnswerId) REFERENCES [dbo].QuizAnswerOptions ([Id]),
);
