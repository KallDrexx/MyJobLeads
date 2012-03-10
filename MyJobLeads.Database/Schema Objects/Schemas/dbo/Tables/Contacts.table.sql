CREATE TABLE [dbo].[Contacts](
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
	[CompanyId] [int] NULL,
	[Contact_Id] [int] NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_Contacts_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id]),
 CONSTRAINT [FK_Contacts_Contacts_Contact_Id] FOREIGN KEY([Contact_Id])
REFERENCES [dbo].[Contacts] ([Id])
)


