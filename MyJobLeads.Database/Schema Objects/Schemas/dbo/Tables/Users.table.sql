CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Password] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FullName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsOrganizationAdmin] [bit] NOT NULL,
	[IsSiteAdmin] [bit] NOT NULL,
	[OrganizationId] [int] NULL,
	[LastVisitedJobSearchId] [int] NULL,
	[LinkedInOAuthDataId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Users_JobSearches_LastVisitedJobSearchId] FOREIGN KEY([LastVisitedJobSearchId])
REFERENCES [dbo].[JobSearches] ([Id]),
 CONSTRAINT [FK_Users_Organizations_OrganizationId] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([Id])
)


