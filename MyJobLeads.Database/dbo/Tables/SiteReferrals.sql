CREATE TABLE [dbo].[SiteReferrals] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ReferralCode] NVARCHAR (MAX) NULL,
    [Url]          NVARCHAR (MAX) NULL,
    [IpAddress]    NVARCHAR (MAX) NULL,
    [Date]         DATETIME       NOT NULL,
    CONSTRAINT [PK_SiteReferrals] PRIMARY KEY CLUSTERED ([Id] ASC)
);

