CREATE TABLE [dbo].[FillPerfectContactResponses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[School] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Program] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ReceivedDate] [datetime] NOT NULL,
	[RepliedDate] [datetime] NULL,
 CONSTRAINT [PK_FillPerfectContactResponses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


