CREATE TABLE [dbo].[PositionHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HasApplied] [bit] NOT NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LinkedInId] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PositionId] [int] NULL,
	[DateModified] [datetime] NOT NULL,
	[HistoryAction] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AuthoringUserId] [int] NOT NULL,
 CONSTRAINT [PK_PositionHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_PositionHistories_Positions_PositionId] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Positions] ([Id]),
 CONSTRAINT [FK_PositionHistories_Users_AuthoringUserId] FOREIGN KEY([AuthoringUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
)


