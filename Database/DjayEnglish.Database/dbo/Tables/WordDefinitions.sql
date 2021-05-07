CREATE TABLE [dbo].[WordDefinitions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordId]     INT            NOT NULL,
    [Definition] NVARCHAR (150) NOT NULL,
    CONSTRAINT [FK_WordDefinitionsWordId_ToWords] FOREIGN KEY ([WordId]) REFERENCES [dbo].[Words] ([Id])
);

