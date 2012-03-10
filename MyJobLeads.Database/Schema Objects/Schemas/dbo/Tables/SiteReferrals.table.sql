CREATE TABLE [dbo].[SiteReferrals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReferralCode] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Url] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IpAddress] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_SiteReferrals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


