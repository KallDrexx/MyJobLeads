CREATE TABLE [dbo].[TaskHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [TaskDate]        DATETIME       NULL,
    [CompletionDate]  DATETIME       NULL,
    [Category]        NVARCHAR (MAX) NULL,
    [Notes]           NVARCHAR (MAX) NULL,
    [TaskId]          INT            NOT NULL,
    [ContactId]       INT            NULL,
    [DateModified]    DATETIME       NOT NULL,
    [HistoryAction]   NVARCHAR (MAX) NULL,
    [AuthoringUserId] INT            NOT NULL,
    CONSTRAINT [PK_TaskHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TaskHistories_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]),
    CONSTRAINT [FK_TaskHistories_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TaskHistories_Users_AuthoringUserId] FOREIGN KEY ([AuthoringUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_TaskId]
    ON [dbo].[TaskHistories]([TaskId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[TaskHistories]([ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthoringUserId]
    ON [dbo].[TaskHistories]([AuthoringUserId] ASC);

