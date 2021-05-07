CREATE TABLE [dbo].[WordAntonyms] (
    [Id]            INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordId]        INT NOT NULL,
    [AntonymWordId] INT NOT NULL,
    [IsActive]      BIT NOT NULL,
    CONSTRAINT [FK_WordAntonymsWordId_ToWords] FOREIGN KEY ([WordId]) REFERENCES [dbo].[Words] ([Id])
);

