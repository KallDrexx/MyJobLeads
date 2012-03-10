CREATE TABLE [dbo].[Users] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [Email]                  NVARCHAR (MAX) NULL,
    [Password]               NVARCHAR (MAX) NULL,
    [FullName]               NVARCHAR (MAX) NULL,
    [IsOrganizationAdmin]    BIT            NOT NULL,
    [IsSiteAdmin]            BIT            NOT NULL,
    [OrganizationId]         INT            NULL,
    [LastVisitedJobSearchId] INT            NULL,
    [LinkedInOAuthDataId]    INT            NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_JobSearches_LastVisitedJobSearchId] FOREIGN KEY ([LastVisitedJobSearchId]) REFERENCES [dbo].[JobSearches] ([Id]),
    CONSTRAINT [FK_Users_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[Users]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LastVisitedJobSearchId]
    ON [dbo].[Users]([LastVisitedJobSearchId] ASC);

