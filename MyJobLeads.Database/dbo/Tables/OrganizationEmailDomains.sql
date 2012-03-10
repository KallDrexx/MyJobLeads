CREATE TABLE [dbo].[OrganizationEmailDomains] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Domain]         NVARCHAR (MAX) NULL,
    [IsActive]       BIT            NOT NULL,
    [OrganizationId] INT            NULL,
    CONSTRAINT [PK_OrganizationEmailDomains] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrganizationEmailDomains_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[OrganizationEmailDomains]([OrganizationId] ASC);

