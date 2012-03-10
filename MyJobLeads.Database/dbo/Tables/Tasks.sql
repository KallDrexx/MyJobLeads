CREATE TABLE [dbo].[Tasks] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [TaskDate]       DATETIME       NULL,
    [CompletionDate] DATETIME       NULL,
    [Category]       NVARCHAR (MAX) NULL,
    [Notes]          NVARCHAR (MAX) NULL,
    [CompanyId]      INT            NULL,
    [ContactId]      INT            NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tasks_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_Tasks_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CompanyId]
    ON [dbo].[Tasks]([CompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ContactId]
    ON [dbo].[Tasks]([ContactId] ASC);

