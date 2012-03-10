CREATE TABLE [dbo].[Companies] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Phone]       NVARCHAR (MAX) NULL,
    [City]        NVARCHAR (MAX) NULL,
    [State]       NVARCHAR (MAX) NULL,
    [Zip]         NVARCHAR (MAX) NULL,
    [MetroArea]   NVARCHAR (MAX) NULL,
    [Industry]    NVARCHAR (MAX) NULL,
    [LeadStatus]  NVARCHAR (MAX) NULL,
    [Website]     NVARCHAR (MAX) NULL,
    [JigsawId]    INT            NULL,
    [Notes]       NVARCHAR (MAX) NULL,
    [JobSearchID] INT            NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Companies_JobSearches_JobSearchID] FOREIGN KEY ([JobSearchID]) REFERENCES [dbo].[JobSearches] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_JobSearchID]
    ON [dbo].[Companies]([JobSearchID] ASC);

