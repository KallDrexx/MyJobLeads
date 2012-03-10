CREATE TABLE [dbo].[FpOrgPilotUsedLicenses] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Email]            NVARCHAR (MAX) NULL,
    [FullName]         NVARCHAR (MAX) NULL,
    [LicenseGrantDate] DATETIME       NOT NULL,
    [Organization_Id]  INT            NULL,
    CONSTRAINT [PK_FpOrgPilotUsedLicenses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FpOrgPilotUsedLicenses_Organizations_Organization_Id] FOREIGN KEY ([Organization_Id]) REFERENCES [dbo].[Organizations] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Organization_Id]
    ON [dbo].[FpOrgPilotUsedLicenses]([Organization_Id] ASC);

