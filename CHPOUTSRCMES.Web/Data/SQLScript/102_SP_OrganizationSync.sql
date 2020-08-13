-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/12
-- Description:	組織資料同步(ORGANIZATION_T 及 ORGANIZATION_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_OrganizationSync]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	
	DECLARE @deletelist TABLE (
		ORGANIZATION_ID BIGINT,
		ORGANIZATION_CODE VARCHAR(3),
		ORGANIZATION_NAME VARCHAR(240),
		ORG_ID BIGINT
	);
	
	DECLARE @mergelist TABLE (
		ORGANIZATION_ID BIGINT,
		ORGANIZATION_CODE VARCHAR(3),
		ORGANIZATION_NAME VARCHAR(240),
		ORG_ID BIGINT,
		CONTROL_FLAG CHAR(1)
	);

	DECLARE @tableVar TABLE (
		MergeAction VARCHAR(20), 
		InsertedID INT, 
		DeletedID INT)

	DECLARE @success INT = 0, @code INT = 0, @message NVARCHAR(500) = '';
	DECLARE @deleteCount INT = 0, @updateCount INT = 0, @insertCount INT = 0;
	
	
	BEGIN TRY
		SET @success = @success + 1
		--收集需要刪除的項目
		INSERT INTO @deletelist (ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID) 
		SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID FROM (
			SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID FROM ORGANIZATION_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID FROM ORGANIZATION_TMP_T T
			) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE ORGANIZATION_T SET CONTROL_FLAG = 'D' WHERE 
				ORGANIZATION_ID IN (SELECT ORGANIZATION_ID FROM @deletelist)
			
			SELECT @deleteCount = @@ROWCOUNT

			--失敗，引發EXCEPTION 記錄問題
			IF(@deleteCount <= 0 OR @@ERROR <> 0)
			BEGIN
				SELECT @message = '刪除，更新CONTROL_FLAG失敗 ERROR:' + CAST(@@ERROR AS VARCHAR(100))
				RAISERROR(@message, 16, @success)
			END
		END


		SET @success = @success + 1
		--收集需要更新或新增的項目
		INSERT INTO @mergelist (ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, CONTROL_FLAG) 
		SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, CONTROL_FLAG FROM (
			SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, CONTROL_FLAG FROM ORGANIZATION_TMP_T T
			EXCEPT
			SELECT ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, CONTROL_FLAG FROM ORGANIZATION_T O
			) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO ORGANIZATION_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON A.ORGANIZATION_ID = B.ORGANIZATION_ID
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET A.CONTROL_FLAG = '', A.ORGANIZATION_CODE = B.ORGANIZATION_CODE, A.ORGANIZATION_NAME = B.ORGANIZATION_NAME, A.ORG_ID = B.ORG_ID
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, CONTROL_FLAG) 
						VALUES (ORGANIZATION_ID, ORGANIZATION_CODE, ORGANIZATION_NAME, ORG_ID, '')
				OUTPUT $action, inserted.ORGANIZATION_ID 'inserted', deleted.ORGANIZATION_ID 'deleted' INTO @tableVar;

			SELECT @insertCount = COUNT(*) FROM @tableVar WHERE MergeAction ='INSERT'
			SELECT @updateCount = COUNT(*) FROM @tableVar WHERE MergeAction ='UPDATE'

			--失敗，引發EXCEPTION 記錄問題
			IF ((@insertCount + @updateCount) <= 0 OR @@ERROR <> 0)
			BEGIN
				SELECT @message = '更新，更新或新增資料失敗 ERROR:' + CAST(@@ERROR AS VARCHAR(100)) + ' COUNT:' + CAST(@@ROWCOUNT AS VARCHAR(100))
				RAISERROR(@message, 16, @success)
			END 

			
		END

		SET @code = 0
		SET @message = ''
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
	END CATCH

	SELECT @code 'CODE', @message 'MESSAGE', @insertCount 'INSERT_COUNT', @updateCount 'UPDATE_COUNT', @deleteCount 'DELETE_COUNT'

END
