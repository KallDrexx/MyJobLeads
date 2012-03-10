CREATE TABLE [dbo].[PositionHistories] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (MAX) NULL,
    [HasApplied]      BIT            NOT NULL,
    [Notes]           NVARCHAR (MAX) NULL,
    [LinkedInId]      NVARCHAR (MAX) NULL,
    [PositionId]      INT            NULL,
    [DateModified]    DATETIME       NOT NULL,
    [HistoryAction]   NVARCHAR (MAX) NULL,
    [AuthoringUserId] INT            NOT NULL,
    CONSTRAINT [PK_PositionHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PositionHistories_Positions_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Positions] ([Id]),
    CONSTRAINT [FK_PositionHistories_Users_AuthoringUserId] FOREIGN KEY ([AuthoringUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PositionId]
    ON [dbo].[PositionHistories]([PositionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AuthoringUserId]
    ON [dbo].[PositionHistories]([AuthoringUserId] ASC);

