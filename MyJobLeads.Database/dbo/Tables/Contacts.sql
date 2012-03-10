CREATE TABLE [dbo].[Contacts] (
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
    [CompanyId]       INT            NULL,
    [Contact_Id]      INT            NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contacts_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_Contacts_Contacts_Contact_Id] FOREIGN KEY ([Contact_Id]) REFERENCES [dbo].[Contacts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CompanyId]
    ON [dbo].[Contacts]([CompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_Id]
    ON [dbo].[Contacts]([Contact_Id] ASC);

