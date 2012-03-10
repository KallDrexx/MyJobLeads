CREATE TABLE [dbo].[CompanyHistories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Phone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[City] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[State] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Zip] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MetroArea] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Industry] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LeadStatus] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Website] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[JigsawId] [int] NULL,
	[Notes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CompanyId] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[HistoryAction] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AuthoringUserId] [int] NOT NULL,
 CONSTRAINT [PK_CompanyHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_CompanyHistories_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE CASCADE,
 CONSTRAINT [FK_CompanyHistories_Users_AuthoringUserId] FOREIGN KEY([AuthoringUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
)


