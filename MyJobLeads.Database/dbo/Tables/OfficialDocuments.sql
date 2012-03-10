CREATE TABLE [dbo].[OfficialDocuments] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [MeantForMembers] BIT            NOT NULL,
    [DownloadUrl]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OfficialDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

