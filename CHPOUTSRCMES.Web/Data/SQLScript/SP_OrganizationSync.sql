-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/12
-- Description:	組織資料同步(ORGANIZATION_T 及 ORGANIZATION_SHADOWED_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_OrganizationSync]
	-- Add the parameters for the stored procedure here
	
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN
	
	DECLARE @orglist table (
		ORGANIZATION_ID BIGINT,
		ORGANIZATION_CODE VARCHAR(3),
		ORGANIZATION_NAME VARCHAR(240)
	);
	
	DECLARE @step int = 0;

	
	
	BEGIN TRY
	
	
	INSERT INTO ORGANIZATION_T (ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME)
		SELECT * FROM ORGANIZATION_T 
		WHERE CONTROL_FLAG <> 'D'
		EXCEPT
		SELECT * FROM ORGANIZATION_TMP_T


		SET @code = 0
		SET @message = ''
		
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @step
		SET @message = CAST(@step AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
	END CATCH
END

