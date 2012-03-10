CREATE TABLE [dbo].[FpJobApplyBasicStats](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PostedDate] [datetime] NOT NULL,
	[ApplyCount] [int] NOT NULL,
 CONSTRAINT [PK_FpJobApplyBasicStats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


