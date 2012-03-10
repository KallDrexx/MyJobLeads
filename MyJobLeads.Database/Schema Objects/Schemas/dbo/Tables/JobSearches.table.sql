CREATE TABLE [dbo].[JobSearches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Metrics_NumCompaniesCreated] [int] NOT NULL,
	[Metrics_NumContactsCreated] [int] NOT NULL,
	[Metrics_NumApplyTasksCreated] [int] NOT NULL,
	[Metrics_NumApplyTasksCompleted] [int] NOT NULL,
	[Metrics_NumPhoneInterviewTasksCreated] [int] NOT NULL,
	[Metrics_NumInPersonInterviewTasksCreated] [int] NOT NULL,
	[HiddenCompanyStatuses] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MilestonesCompleted] [bit] NOT NULL,
	[UserId] [int] NULL,
	[CurrentMilestoneId] [int] NULL,
 CONSTRAINT [PK_JobSearches] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_JobSearches_MilestoneConfigs_CurrentMilestoneId] FOREIGN KEY([CurrentMilestoneId])
REFERENCES [dbo].[MilestoneConfigs] ([Id]),
 CONSTRAINT [FK_JobSearches_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
)


