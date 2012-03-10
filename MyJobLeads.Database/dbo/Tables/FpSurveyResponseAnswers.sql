CREATE TABLE [dbo].[FpSurveyResponseAnswers] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Order]    SMALLINT       NOT NULL,
    [Question] NVARCHAR (MAX) NULL,
    [Answer]   NVARCHAR (MAX) NULL,
    [SurveyId] INT            NOT NULL,
    CONSTRAINT [PK_FpSurveyResponseAnswers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FpSurveyResponseAnswers_FpSurveyResponses_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[FpSurveyResponses] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyId]
    ON [dbo].[FpSurveyResponseAnswers]([SurveyId] ASC);

