CREATE TABLE [dbo].[OfficialDocumentOrganizations] (
    [OfficialDocument_Id] INT NOT NULL,
    [Organization_Id]     INT NOT NULL,
    CONSTRAINT [PK_OfficialDocumentOrganizations] PRIMARY KEY CLUSTERED ([OfficialDocument_Id] ASC, [Organization_Id] ASC),
    CONSTRAINT [FK_OfficialDocumentOrganizations_OfficialDocuments_OfficialDocument_Id] FOREIGN KEY ([OfficialDocument_Id]) REFERENCES [dbo].[OfficialDocuments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OfficialDocumentOrganizations_Organizations_Organization_Id] FOREIGN KEY ([Organization_Id]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OfficialDocument_Id]
    ON [dbo].[OfficialDocumentOrganizations]([OfficialDocument_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Organization_Id]
    ON [dbo].[OfficialDocumentOrganizations]([Organization_Id] ASC);

