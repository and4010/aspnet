-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/14
-- Description:	庫存交易類別 資料同步(TRANSACTION_TYPE_T 及 TRANSACTION_TYPE_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_TransactionTypeSync]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	
	DECLARE @deletelist TABLE (
		TRANSACTION_TYPE_ID BIGINT ,
		TRANSACTION_TYPE_NAME NVARCHAR(80) ,
		DESCRIPTION NVARCHAR(2400) ,
		TRANSACTION_ACTION_ID BIGINT ,
		TRANSACTION_ACTION_NAME NVARCHAR(80) ,
		TRANSACTION_SOURCE_TYPE_ID BIGINT ,
		TRANSACTION_SOURCE_TYPE_NAME NVARCHAR(30) ,
		CREATED_BY BIGINT ,
		CREATION_DATE DATETIME ,
		LAST_UPDATE_BY BIGINT ,
		LAST_UPDATE_DATE DATETIME
	);
	
	DECLARE @mergelist TABLE (
		TRANSACTION_TYPE_ID BIGINT ,
		TRANSACTION_TYPE_NAME NVARCHAR(80) ,
		DESCRIPTION NVARCHAR(2400) ,
		TRANSACTION_ACTION_ID BIGINT ,
		TRANSACTION_ACTION_NAME NVARCHAR(80) ,
		TRANSACTION_SOURCE_TYPE_ID BIGINT ,
		TRANSACTION_SOURCE_TYPE_NAME NVARCHAR(30) ,
		CREATED_BY BIGINT ,
		CREATION_DATE DATETIME ,
		LAST_UPDATE_BY BIGINT ,
		LAST_UPDATE_DATE DATETIME ,
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
		INSERT INTO @deletelist (
			TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
			TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
			LAST_UPDATE_DATE
		) 
		SELECT 
			TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
			TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
			LAST_UPDATE_DATE
		FROM (
			SELECT 
				TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE
			FROM TRANSACTION_TYPE_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT 
				TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE
			FROM TRANSACTION_TYPE_TMP_T T
		) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE TRANSACTION_TYPE_T SET CONTROL_FLAG = 'D' WHERE 
				TRANSACTION_TYPE_ID IN (SELECT TRANSACTION_TYPE_ID FROM @deletelist)
			
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
		INSERT INTO @mergelist (
				TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG) 
		SELECT TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
		FROM (
			SELECT TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM TRANSACTION_TYPE_TMP_T T
			EXCEPT
			SELECT TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
				TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM TRANSACTION_TYPE_T O
		) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO TRANSACTION_TYPE_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON A.TRANSACTION_TYPE_ID = B.TRANSACTION_TYPE_ID
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET 
						A.CONTROL_FLAG = '', A.TRANSACTION_TYPE_ID = B.TRANSACTION_TYPE_ID, A.TRANSACTION_TYPE_NAME = B.TRANSACTION_TYPE_NAME, A.DESCRIPTION = B.DESCRIPTION,
						A.TRANSACTION_ACTION_ID = B.TRANSACTION_ACTION_ID, A.TRANSACTION_ACTION_NAME = B.TRANSACTION_ACTION_NAME, A.TRANSACTION_SOURCE_TYPE_ID = B.TRANSACTION_SOURCE_TYPE_ID, 
						A.TRANSACTION_SOURCE_TYPE_NAME = B.TRANSACTION_SOURCE_TYPE_NAME, A.CREATED_BY = B.CREATED_BY, A.CREATION_DATE = B.CREATION_DATE, A.LAST_UPDATE_BY = B.LAST_UPDATE_BY, 
						A.LAST_UPDATE_DATE = B.LAST_UPDATE_DATE
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (
						TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
						TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
						LAST_UPDATE_DATE, CONTROL_FLAG 
					) VALUES (
						TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, DESCRIPTION, TRANSACTION_ACTION_ID, TRANSACTION_ACTION_NAME, 
						TRANSACTION_SOURCE_TYPE_ID, TRANSACTION_SOURCE_TYPE_NAME, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
						LAST_UPDATE_DATE, ''
					)
				OUTPUT $action, inserted.TRANSACTION_TYPE_ID 'inserted', deleted.TRANSACTION_TYPE_ID 'deleted' INTO @tableVar;

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

