CREATE TABLE [dbo].[JobSearchHistories] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (MAX) NULL,
    [Description]           NVARCHAR (MAX) NULL,
    [HiddenCompanyStatuses] NVARCHAR (MAX) NULL,
    [JobSearchId]           INT            NOT NULL,
    [DateModified]          DATETIME       NOT NULL,
    [HistoryAction]         NVARCHAR (MAX) NULL,
    [AuthoringUserId]       INT            NOT NULL,
    CONSTRAINT [PK_JobSearchHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobSearchHistories_JobSearches_JobSearchId] FOREIGN KEY ([JobSearchId]) REFERENCES [dbo].[JobSearches] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_JobSearchHistories_Users_AuthoringUserId] FOREIGN KEY ([AuthoringUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_JobSearchId]
    ON [dbo].[JobSearchHistories]([JobSearchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthoringUserId]
    ON [dbo].[JobSearchHistories]([AuthoringUserId] ASC);

