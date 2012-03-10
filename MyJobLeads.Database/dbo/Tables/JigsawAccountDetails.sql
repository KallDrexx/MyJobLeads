CREATE TABLE [dbo].[JigsawAccountDetails] (
    [Id]                INT            NOT NULL,
    [Username]          NVARCHAR (MAX) NULL,
    [EncryptedPassword] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_JigsawAccountDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JigsawAccountDetails_Users_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[JigsawAccountDetails]([Id] ASC);

