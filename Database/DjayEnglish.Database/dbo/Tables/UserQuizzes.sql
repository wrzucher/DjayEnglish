CREATE TABLE [dbo].[UserQuizzes] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizId]        INT             NOT NULL,
    [UserId]        NVARCHAR(42)    NOT NULL,
    [Created]       DATETIMEOFFSET  NOT NULL,
    [Closed]        DATETIMEOFFSET  NULL,
    [State]         TINYINT         NOT NULL,
    CONSTRAINT [FK_UserQuizzesQuizId_ToQuizzes] FOREIGN KEY (QuizId) REFERENCES [dbo].Quizzes ([Id]),
    CONSTRAINT [FK_UserQuizzesUserId_ToUsers]   FOREIGN KEY (UserId) REFERENCES [dbo].Users ([Id])
);

