-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/14
-- Description:	餘切規格資料同步(RELATED_T 及 RELATED_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_RelatedSync]

AS
BEGIN
	
	DECLARE @deletelist TABLE (
		INVENTORY_ITEM_ID BIGINT,
		ITEM_NUMBER NVARCHAR(40),
		ITEM_DESCRIPTION NVARCHAR(240),
		RELATED_ITEM_ID BIGINT,
		RELATED_ITEM_NUMBER NVARCHAR(40),
		RELATED_ITEM_DESCRIPTION NVARCHAR(240),
		CREATED_BY BIGINT,
		CREATION_DATE DATETIME,
		LAST_UPDATE_BY BIGINT,
		LAST_UPDATE_DATE DATETIME
	);
	
	DECLARE @mergelist TABLE (
		INVENTORY_ITEM_ID BIGINT,
		ITEM_NUMBER NVARCHAR(40),
		ITEM_DESCRIPTION NVARCHAR(240),
		RELATED_ITEM_ID BIGINT,
		RELATED_ITEM_NUMBER NVARCHAR(40),
		RELATED_ITEM_DESCRIPTION NVARCHAR(240),
		CREATED_BY BIGINT,
		CREATION_DATE DATETIME,
		LAST_UPDATE_BY BIGINT,
		LAST_UPDATE_DATE DATETIME,
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
			INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
			RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
			LAST_UPDATE_DATE) 
		SELECT 
			INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
			RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
			LAST_UPDATE_DATE 
		FROM (
			SELECT 
				INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE 
			FROM RELATED_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT
				INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE  
			FROM RELATED_TMP_T T
		) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE RELATED_T SET CONTROL_FLAG = 'D' 
			FROM (
				SELECT  * FROM @deletelist
			) A 
			WHERE RELATED_T.INVENTORY_ITEM_ID = A.INVENTORY_ITEM_ID AND RELATED_T.RELATED_ITEM_ID = A.RELATED_ITEM_ID 
			

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
		INSERT INTO @mergelist (INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG)
		SELECT
				INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
		FROM (
			SELECT 
				INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM RELATED_TMP_T T
			EXCEPT
			SELECT 
				INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
				RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
				LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM RELATED_T O
		) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO RELATED_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON (A.INVENTORY_ITEM_ID = B.INVENTORY_ITEM_ID AND A.RELATED_ITEM_ID = B.RELATED_ITEM_ID)
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET 
						A.CONTROL_FLAG = '', A.INVENTORY_ITEM_ID = B.INVENTORY_ITEM_ID, A.ITEM_NUMBER = B.ITEM_NUMBER, A.ITEM_DESCRIPTION = B.ITEM_DESCRIPTION, 
						A.RELATED_ITEM_ID = B.RELATED_ITEM_ID, A.RELATED_ITEM_NUMBER = B.RELATED_ITEM_NUMBER, A.CREATED_BY = B.CREATED_BY, 
						A.CREATION_DATE = B.CREATION_DATE, A.LAST_UPDATE_BY = B.LAST_UPDATE_BY, A.LAST_UPDATE_DATE = B.LAST_UPDATE_DATE
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (
						INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
						RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
						LAST_UPDATE_DATE, CONTROL_FLAG 
					) VALUES (
						INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, RELATED_ITEM_ID, 
						RELATED_ITEM_NUMBER, RELATED_ITEM_DESCRIPTION, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, 
						LAST_UPDATE_DATE, '')
				OUTPUT $action, inserted.RELATED_ID 'inserted', deleted.RELATED_ID 'deleted' INTO @tableVar;

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

