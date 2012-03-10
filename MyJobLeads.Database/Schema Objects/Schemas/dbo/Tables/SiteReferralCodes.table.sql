CREATE TABLE [dbo].[SiteReferralCodes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GivenTo] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SiteReferralCodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


