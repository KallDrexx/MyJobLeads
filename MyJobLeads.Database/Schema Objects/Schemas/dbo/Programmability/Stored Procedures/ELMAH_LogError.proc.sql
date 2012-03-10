CREATE PROCEDURE [dbo].[ELMAH_LogError]
	@ErrorId [uniqueidentifier],
	@Application [nvarchar](60),
	@Host [nvarchar](30),
	@Type [nvarchar](100),
	@Source [nvarchar](60),
	@Message [nvarchar](500),
	@User [nvarchar](50),
	@AllXml [nvarchar](max),
	@StatusCode [int],
	@TimeUtc [datetime]
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )


