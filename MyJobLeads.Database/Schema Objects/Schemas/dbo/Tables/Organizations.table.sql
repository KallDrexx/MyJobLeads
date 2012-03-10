CREATE TABLE [dbo].[Organizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RegistrationToken] [uniqueidentifier] NOT NULL,
	[IsEmailDomainRestricted] [bit] NOT NULL,
	[FillPerfectPilotKey] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FpPilotLicenseCount] [int] NOT NULL,
 CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)


