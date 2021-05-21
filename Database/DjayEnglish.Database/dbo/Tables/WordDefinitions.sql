CREATE TABLE [dbo].[WordDefinitions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordId]     INT            NOT NULL,
    [Definition] NVARCHAR (700) NOT NULL,
    [IsActive]   BIT            NOT NULL DEFAULT(0),
    [SourceId]   INT            NOT NULL DEFAULT(0),
    [SourceName] NVARCHAR (50)  NOT NULL DEFAULT('UNKNOWN 1')
    CONSTRAINT [FK_WordDefinitionsWordId_ToWords] FOREIGN KEY ([WordId]) REFERENCES [dbo].[Words] ([Id])
);

