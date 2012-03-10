CREATE TABLE [dbo].[OAuthDatas] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Token]              NVARCHAR (MAX) NULL,
    [Secret]             NVARCHAR (MAX) NULL,
    [TokenTypeValue]     INT            NOT NULL,
    [TokenProviderValue] INT            NOT NULL,
    [LinkedInUser_Id]    INT            NULL,
    CONSTRAINT [PK_OAuthDatas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OAuthDatas_Users_LinkedInUser_Id] FOREIGN KEY ([LinkedInUser_Id]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_LinkedInUser_Id]
    ON [dbo].[OAuthDatas]([LinkedInUser_Id] ASC);

