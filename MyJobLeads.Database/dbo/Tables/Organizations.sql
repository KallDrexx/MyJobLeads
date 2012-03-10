CREATE TABLE [dbo].[Organizations] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [Name]                    NVARCHAR (MAX)   NULL,
    [RegistrationToken]       UNIQUEIDENTIFIER NOT NULL,
    [IsEmailDomainRestricted] BIT              NOT NULL,
    [FillPerfectPilotKey]     NVARCHAR (MAX)   NULL,
    [FpPilotLicenseCount]     INT              NOT NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

