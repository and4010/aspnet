-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/13
-- Description:	作業單元資料同步(ORG_UNIT_T 及 ORG_UNIT_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_OrgUnitSync]

AS
BEGIN
	
	DECLARE @deletelist TABLE (
		[ORG_ID] BIGINT,
		[ORG_NAME] NVARCHAR(240),
		[OSP_FLAG] NVARCHAR(10)
	);
	
	DECLARE @mergelist TABLE (
		[ORG_ID] BIGINT,
		[ORG_NAME] NVARCHAR(240),
		[OSP_FLAG] NVARCHAR(10),
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
		INSERT INTO @deletelist (ORG_ID, ORG_NAME, OSP_FLAG) 
		SELECT ORG_ID, ORG_NAME FROM (
			SELECT ORG_ID, ORG_NAME FROM ORG_UNIT_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT ORG_ID, ORG_NAME FROM ORG_UNIT_TMP_T T
			) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE ORG_UNIT_T SET CONTROL_FLAG = 'D' 
			FROM (
				SELECT  * FROM @deletelist
			) A 
			WHERE ORG_UNIT_T.ORG_ID = A.ORG_ID 
			

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
		INSERT INTO @mergelist (ORG_ID, ORG_NAME, CONTROL_FLAG) 
		SELECT ORG_ID, ORG_NAME, CONTROL_FLAG FROM (
			SELECT ORG_ID, ORG_NAME, CONTROL_FLAG FROM ORG_UNIT_TMP_T T
			EXCEPT
			SELECT ORG_ID, ORG_NAME, CONTROL_FLAG FROM ORG_UNIT_T O
			) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO ORG_UNIT_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON (A.ORG_ID = B.ORG_ID)
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET A.CONTROL_FLAG = '', A.ORG_ID = B.ORG_ID, A.ORG_NAME = B.ORG_NAME
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (ORG_ID, ORG_NAME, CONTROL_FLAG) 
						VALUES (ORG_ID, ORG_NAME, '')
				OUTPUT $action, inserted.ORG_ID 'inserted', deleted.ORG_ID 'deleted' INTO @tableVar;

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

