CREATE TABLE [dbo].[FpSurveyResponses] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [FpUserId] NVARCHAR (MAX) NULL,
    [SurveyId] NVARCHAR (MAX) NULL,
    [Date]     DATETIME       NOT NULL,
    CONSTRAINT [PK_FpSurveyResponses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

