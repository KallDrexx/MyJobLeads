CREATE TABLE [dbo].[TaskHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TaskDate] [datetime] NULL,
	[CompletionDate] [datetime] NULL,
	[Category] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TaskId] [int] NOT NULL,
	[ContactId] [int] NULL,
	[DateModified] [datetime] NOT NULL,
	[HistoryAction] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AuthoringUserId] [int] NOT NULL,
 CONSTRAINT [PK_TaskHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_TaskHistories_Contacts_ContactId] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([Id]),
 CONSTRAINT [FK_TaskHistories_Tasks_TaskId] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Tasks] ([Id])
ON DELETE CASCADE,
 CONSTRAINT [FK_TaskHistories_Users_AuthoringUserId] FOREIGN KEY([AuthoringUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
)


