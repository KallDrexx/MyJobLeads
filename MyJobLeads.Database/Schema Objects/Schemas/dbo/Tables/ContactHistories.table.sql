CREATE TABLE [dbo].[ContactHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Title] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DirectPhone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MobilePhone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Extension] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Assistant] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ReferredBy] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[JigsawId] [int] NULL,
	[HasJigsawAccess] [bit] NOT NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactId] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[HistoryAction] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AuthoringUserId] [int] NOT NULL,
 CONSTRAINT [PK_ContactHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_ContactHistories_Contacts_ContactId] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([Id])
ON DELETE CASCADE,
 CONSTRAINT [FK_ContactHistories_Users_AuthoringUserId] FOREIGN KEY([AuthoringUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
)


