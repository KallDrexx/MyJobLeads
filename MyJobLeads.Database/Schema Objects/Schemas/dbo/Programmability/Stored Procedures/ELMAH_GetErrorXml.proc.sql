CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
	@Application [nvarchar](60),
	@ErrorId [uniqueidentifier]
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application


