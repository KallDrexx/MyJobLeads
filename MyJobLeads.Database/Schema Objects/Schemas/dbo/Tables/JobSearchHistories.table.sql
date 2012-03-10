CREATE TABLE [dbo].[JobSearchHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HiddenCompanyStatuses] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[JobSearchId] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[HistoryAction] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AuthoringUserId] [int] NOT NULL,
 CONSTRAINT [PK_JobSearchHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_JobSearchHistories_JobSearches_JobSearchId] FOREIGN KEY([JobSearchId])
REFERENCES [dbo].[JobSearches] ([Id])
ON DELETE CASCADE,
 CONSTRAINT [FK_JobSearchHistories_Users_AuthoringUserId] FOREIGN KEY([AuthoringUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
)


