CREATE TABLE [dbo].[OAuthDatas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Secret] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TokenTypeValue] [int] NOT NULL,
	[TokenProviderValue] [int] NOT NULL,
	[LinkedInUser_Id] [int] NULL,
 CONSTRAINT [PK_OAuthDatas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_OAuthDatas_Users_LinkedInUser_Id] FOREIGN KEY([LinkedInUser_Id])
REFERENCES [dbo].[Users] ([Id])
)


