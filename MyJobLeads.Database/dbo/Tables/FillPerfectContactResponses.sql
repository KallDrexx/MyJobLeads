CREATE TABLE [dbo].[FillPerfectContactResponses] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [Email]        NVARCHAR (MAX) NULL,
    [School]       NVARCHAR (MAX) NULL,
    [Program]      NVARCHAR (MAX) NULL,
    [ReceivedDate] DATETIME       NOT NULL,
    [RepliedDate]  DATETIME       NULL,
    CONSTRAINT [PK_FillPerfectContactResponses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

