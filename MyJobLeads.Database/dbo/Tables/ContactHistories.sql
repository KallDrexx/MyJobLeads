CREATE TABLE [dbo].[ContactHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [Title]           NVARCHAR (MAX) NULL,
    [DirectPhone]     NVARCHAR (MAX) NULL,
    [MobilePhone]     NVARCHAR (MAX) NULL,
    [Extension]       NVARCHAR (MAX) NULL,
    [Email]           NVARCHAR (MAX) NULL,
    [Assistant]       NVARCHAR (MAX) NULL,
    [ReferredBy]      NVARCHAR (MAX) NULL,
    [JigsawId]        INT            NULL,
    [HasJigsawAccess] BIT            NOT NULL,
    [Notes]           NVARCHAR (MAX) NULL,
    [ContactId]       INT            NOT NULL,
    [DateModified]    DATETIME       NOT NULL,
    [HistoryAction]   NVARCHAR (MAX) NULL,
    [AuthoringUserId] INT            NOT NULL,
    CONSTRAINT [PK_ContactHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContactHistories_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ContactHistories_Users_AuthoringUserId] FOREIGN KEY ([AuthoringUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[ContactHistories]([ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthoringUserId]
    ON [dbo].[ContactHistories]([AuthoringUserId] ASC);

