CREATE TABLE [dbo].[ChatQuizzes] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizId]       INT             NOT NULL,
    [ChatId]        BIGINT          NOT NULL,
    [Created]       DATETIMEOFFSET  NOT NULL,
    [Closed]        DATETIMEOFFSET  NULL,
    [State]    TINYINT         NOT NULL,
    CONSTRAINT [FK_ChatQuizzesQuizId_ToQuizzes] FOREIGN KEY (QuizId) REFERENCES [dbo].Quizzes ([Id]),
    CONSTRAINT [FK_ChatQuizzesChatId_ToChats]   FOREIGN KEY (ChatId)  REFERENCES [dbo].Chats ([Id])
);

