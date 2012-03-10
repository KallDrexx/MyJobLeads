CREATE TABLE [dbo].[Positions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (MAX) NULL,
    [HasApplied] BIT            NOT NULL,
    [Notes]      NVARCHAR (MAX) NULL,
    [LinkedInId] NVARCHAR (MAX) NULL,
    [CompanyId]  INT            NULL,
    CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Positions_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CompanyId]
    ON [dbo].[Positions]([CompanyId] ASC);

