CREATE TABLE [dbo].[WordUsages] (
    [Id]               INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [WordDefinitionId] INT NOT NULL,
    [Example]          NVARCHAR(700) NOT NULL,
    [IsActive]         BIT NOT NULL,
    CONSTRAINT [FK_WordUsagesWordDefinitionId_ToWordDefinitions] FOREIGN KEY (WordDefinitionId) REFERENCES [dbo].WordDefinitions ([Id])
);

