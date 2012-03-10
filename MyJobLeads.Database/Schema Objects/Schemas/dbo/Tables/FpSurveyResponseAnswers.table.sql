CREATE TABLE [dbo].[FpSurveyResponseAnswers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Order] [smallint] NOT NULL,
	[Question] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Answer] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SurveyId] [int] NOT NULL,
 CONSTRAINT [PK_FpSurveyResponseAnswers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_FpSurveyResponseAnswers_FpSurveyResponses_SurveyId] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[FpSurveyResponses] ([Id])
ON DELETE CASCADE
)


