CREATE TABLE [dbo].[CompanyHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [Phone]           NVARCHAR (MAX) NULL,
    [City]            NVARCHAR (MAX) NULL,
    [State]           NVARCHAR (MAX) NULL,
    [Zip]             NVARCHAR (MAX) NULL,
    [MetroArea]       NVARCHAR (MAX) NULL,
    [Industry]        NVARCHAR (MAX) NULL,
    [LeadStatus]      NVARCHAR (MAX) NULL,
    [Website]         NVARCHAR (MAX) NULL,
    [JigsawId]        INT            NULL,
    [Notes]           NVARCHAR (MAX) NULL,
    [CompanyId]       INT            NOT NULL,
    [DateModified]    DATETIME       NOT NULL,
    [HistoryAction]   NVARCHAR (MAX) NULL,
    [AuthoringUserId] INT            NOT NULL,
    CONSTRAINT [PK_CompanyHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyHistories_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CompanyHistories_Users_AuthoringUserId] FOREIGN KEY ([AuthoringUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CompanyId]
    ON [dbo].[CompanyHistories]([CompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthoringUserId]
    ON [dbo].[CompanyHistories]([AuthoringUserId] ASC);

