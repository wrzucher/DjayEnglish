CREATE TABLE [dbo].[QuizeExamples] (
    [Id]            INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizeId]       INT NOT NULL,
    [WordUsageId]   INT NOT NULL,
    CONSTRAINT [FK_QuizeExamplesWordUsageId_ToWordUsages] FOREIGN KEY (WordUsageId) REFERENCES [dbo].WordUsages ([Id]),
    CONSTRAINT [FK_QuizeExamplesQuizeId_ToQuizes] FOREIGN KEY (QuizeId) REFERENCES [dbo].Quizzes ([Id])
);

