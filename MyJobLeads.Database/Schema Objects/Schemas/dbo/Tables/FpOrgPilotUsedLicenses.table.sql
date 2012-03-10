CREATE TABLE [dbo].[FpOrgPilotUsedLicenses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FullName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LicenseGrantDate] [datetime] NOT NULL,
	[Organization_Id] [int] NULL,
 CONSTRAINT [PK_FpOrgPilotUsedLicenses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [FK_FpOrgPilotUsedLicenses_Organizations_Organization_Id] FOREIGN KEY([Organization_Id])
REFERENCES [dbo].[Organizations] ([Id])
)


