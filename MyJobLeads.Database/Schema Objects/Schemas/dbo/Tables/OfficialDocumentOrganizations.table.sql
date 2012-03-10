CREATE TABLE [dbo].[OfficialDocumentOrganizations](
	[OfficialDocument_Id] [int] NOT NULL,
	[Organization_Id] [int] NOT NULL,
 CONSTRAINT [PK_OfficialDocumentOrganizations] PRIMARY KEY CLUSTERED 
(
	[OfficialDocument_Id] ASC,
	[Organization_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_OfficialDocumentOrganizations_OfficialDocuments_OfficialDocument_Id] FOREIGN KEY([OfficialDocument_Id])
REFERENCES [dbo].[OfficialDocuments] ([Id])
ON DELETE CASCADE,
 CONSTRAINT [FK_OfficialDocumentOrganizations_Organizations_Organization_Id] FOREIGN KEY([Organization_Id])
REFERENCES [dbo].[Organizations] ([Id])
ON DELETE CASCADE
)


