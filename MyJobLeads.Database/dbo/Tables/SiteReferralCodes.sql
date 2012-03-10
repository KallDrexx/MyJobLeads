CREATE TABLE [dbo].[SiteReferralCodes] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Code]    NVARCHAR (MAX) NULL,
    [GivenTo] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SiteReferralCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

