CREATE TABLE [dbo].[FpJobApplyBasicStats] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (MAX) NULL,
    [PostedDate] DATETIME       NOT NULL,
    [ApplyCount] INT            NOT NULL,
    CONSTRAINT [PK_FpJobApplyBasicStats] PRIMARY KEY CLUSTERED ([Id] ASC)
);

