CREATE TABLE [dbo].[UnitTestEntities] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Text] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UnitTestEntities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

