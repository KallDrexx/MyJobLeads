CREATE TABLE [dbo].[JobSearches] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [Name]                                     NVARCHAR (MAX) NULL,
    [Description]                              NVARCHAR (MAX) NULL,
    [Metrics_NumCompaniesCreated]              INT            NOT NULL,
    [Metrics_NumContactsCreated]               INT            NOT NULL,
    [Metrics_NumApplyTasksCreated]             INT            NOT NULL,
    [Metrics_NumApplyTasksCompleted]           INT            NOT NULL,
    [Metrics_NumPhoneInterviewTasksCreated]    INT            NOT NULL,
    [Metrics_NumInPersonInterviewTasksCreated] INT            NOT NULL,
    [HiddenCompanyStatuses]                    NVARCHAR (MAX) NULL,
    [MilestonesCompleted]                      BIT            NOT NULL,
    [UserId]                                   INT            NULL,
    [CurrentMilestoneId]                       INT            NULL,
    CONSTRAINT [PK_JobSearches] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobSearches_MilestoneConfigs_CurrentMilestoneId] FOREIGN KEY ([CurrentMilestoneId]) REFERENCES [dbo].[MilestoneConfigs] ([Id]),
    CONSTRAINT [FK_JobSearches_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[JobSearches]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CurrentMilestoneId]
    ON [dbo].[JobSearches]([CurrentMilestoneId] ASC);

