CREATE TABLE [dbo].[QuizeAnswerOptions] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizeId]       INT             NOT NULL,
    [Text]          NVARCHAR (150)  NOT NULL,
    [IsRightAnswer] BIT             NOT NULL,
    CONSTRAINT [FK_QuizeAnswerOptionsQuizeId_ToQuizes] FOREIGN KEY (QuizeId) REFERENCES [dbo].Quizzes ([Id])
);

