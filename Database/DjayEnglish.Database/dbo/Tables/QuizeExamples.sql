CREATE TABLE [dbo].[QuizeExamples] (
    [Id]            INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [QuizeId]       INT NOT NULL,
    [WordExampleId] INT NOT NULL,
    CONSTRAINT [FK_QuizeExamplesWordExampleId_ToWordExamples] FOREIGN KEY (WordExampleId) REFERENCES [dbo].WordExamples ([Id]),
    CONSTRAINT [FK_QuizeExamplesQuizeId_ToQuizes] FOREIGN KEY (QuizeId) REFERENCES [dbo].Quizzes ([Id])
);

