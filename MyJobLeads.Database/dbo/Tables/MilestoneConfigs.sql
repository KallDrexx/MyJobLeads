CREATE TABLE [dbo].[MilestoneConfigs] (
    [Id]                                                INT            IDENTITY (1, 1) NOT NULL,
    [Title]                                             NVARCHAR (MAX) NULL,
    [IsStartingMilestone]                               BIT            NOT NULL,
    [Instructions]                                      NVARCHAR (MAX) NULL,
    [CompletionDisplay]                                 NVARCHAR (MAX) NULL,
    [NextMilestoneId]                                   INT            NULL,
    [OrganizationId]                                    INT            NULL,
    [JobSearchMetrics_NumCompaniesCreated]              INT            NOT NULL,
    [JobSearchMetrics_NumContactsCreated]               INT            NOT NULL,
    [JobSearchMetrics_NumApplyTasksCreated]             INT            NOT NULL,
    [JobSearchMetrics_NumApplyTasksCompleted]           INT            NOT NULL,
    [JobSearchMetrics_NumPhoneInterviewTasksCreated]    INT            NOT NULL,
    [JobSearchMetrics_NumInPersonInterviewTasksCreated] INT            NOT NULL,
    CONSTRAINT [PK_MilestoneConfigs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MilestoneConfigs_MilestoneConfigs_NextMilestoneId] FOREIGN KEY ([NextMilestoneId]) REFERENCES [dbo].[MilestoneConfigs] ([Id]),
    CONSTRAINT [FK_MilestoneConfigs_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_NextMilestoneId]
    ON [dbo].[MilestoneConfigs]([NextMilestoneId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationId]
    ON [dbo].[MilestoneConfigs]([OrganizationId] ASC);

