CREATE TABLE [dbo].[MilestoneConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsStartingMilestone] [bit] NOT NULL,
	[Instructions] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CompletionDisplay] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[NextMilestoneId] [int] NULL,
	[OrganizationId] [int] NULL,
	[JobSearchMetrics_NumCompaniesCreated] [int] NOT NULL,
	[JobSearchMetrics_NumContactsCreated] [int] NOT NULL,
	[JobSearchMetrics_NumApplyTasksCreated] [int] NOT NULL,
	[JobSearchMetrics_NumApplyTasksCompleted] [int] NOT NULL,
	[JobSearchMetrics_NumPhoneInterviewTasksCreated] [int] NOT NULL,
	[JobSearchMetrics_NumInPersonInterviewTasksCreated] [int] NOT NULL,
 CONSTRAINT [PK_MilestoneConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_MilestoneConfigs_MilestoneConfigs_NextMilestoneId] FOREIGN KEY([NextMilestoneId])
REFERENCES [dbo].[MilestoneConfigs] ([Id]),
 CONSTRAINT [FK_MilestoneConfigs_Organizations_OrganizationId] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([Id])
)


