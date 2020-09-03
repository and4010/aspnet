-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/17
-- Description:	組織料號資料同步(ORG_ITEMS_T 及 ORG_ITEMS_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_OrgItemSync]

AS
BEGIN
	
	DECLARE @deletelist TABLE (
		INVENTORY_ITEM_ID BIGINT,
		ORGANIZATION_ID BIGINT
	);
	
	DECLARE @mergelist TABLE (
		INVENTORY_ITEM_ID BIGINT,
		ORGANIZATION_ID BIGINT
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
			INVENTORY_ITEM_ID, ORGANIZATION_ID
		) 
		SELECT 
			INVENTORY_ITEM_ID, ORGANIZATION_ID
		FROM (
			SELECT 
				INVENTORY_ITEM_ID, ORGANIZATION_ID
			FROM ORG_ITEMS_T O
			EXCEPT
			SELECT
				INVENTORY_ITEM_ID, ORGANIZATION_ID
			FROM ORG_ITEMS_TMP_T T
		) A

		--如有須刪除的
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN

			DELETE o FROM ORG_ITEMS_T o 
			JOIN @deletelist d ON d.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID AND d.ORGANIZATION_ID = o.ORGANIZATION_ID
			
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
		INSERT INTO @mergelist (INVENTORY_ITEM_ID, ORGANIZATION_ID)
		SELECT
			INVENTORY_ITEM_ID, ORGANIZATION_ID
		FROM (
			SELECT 
				INVENTORY_ITEM_ID, ORGANIZATION_ID
			FROM ORG_ITEMS_TMP_T T
			EXCEPT
			SELECT 
				INVENTORY_ITEM_ID, ORGANIZATION_ID
			FROM ORG_ITEMS_T O
		) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO ORG_ITEMS_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON (A.INVENTORY_ITEM_ID = B.INVENTORY_ITEM_ID AND A.ORGANIZATION_ID = B.ORGANIZATION_ID)
				WHEN NOT MATCHED THEN 
					INSERT (
						INVENTORY_ITEM_ID, ORGANIZATION_ID
					) VALUES (
						INVENTORY_ITEM_ID, ORGANIZATION_ID )
				OUTPUT $action, inserted.INVENTORY_ITEM_ID 'inserted', deleted.INVENTORY_ITEM_ID 'deleted' INTO @tableVar;

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

