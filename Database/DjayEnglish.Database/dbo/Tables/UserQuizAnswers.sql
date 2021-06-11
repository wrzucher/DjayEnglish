CREATE TABLE [dbo].[UserQuizAnswers] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [UserQuizeId]   INT             NOT NULL,
    [AnswerId]      INT             NULL,
    CONSTRAINT [FK_UserQuizAnswersAnswerId_ToQuizAnswerOptions]
        FOREIGN KEY (AnswerId) REFERENCES [dbo].QuizAnswerOptions ([Id]),
    CONSTRAINT [FK_UserQuizAnswersUserId_ToUserQuizzes]
        FOREIGN KEY (UserQuizeId) REFERENCES [dbo].UserQuizzes ([Id]),
);
