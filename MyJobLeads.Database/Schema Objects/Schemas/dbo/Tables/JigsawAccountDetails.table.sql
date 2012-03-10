CREATE TABLE [dbo].[JigsawAccountDetails](
	[Id] [int] NOT NULL,
	[Username] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EncryptedPassword] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_JigsawAccountDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_JigsawAccountDetails_Users_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Users] ([Id])
)


