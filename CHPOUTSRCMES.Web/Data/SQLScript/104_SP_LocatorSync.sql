-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/13
-- Description:	儲位資料同步(LOCATOR_T 及 LOCATOR_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_LocatorSync]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	
	DECLARE @deletelist TABLE (
		LOCATOR_ID BIGINT,
		ORGANIZATION_ID BIGINT,
		SUBINVENTORY_CODE NVARCHAR(20),
		LOCATOR_SEGMENTS NVARCHAR(163),
		LOCATOR_DESC NVARCHAR(50),
		SEGMENT1 NVARCHAR(40),
		SEGMENT2 NVARCHAR(40),
		SEGMENT3 NVARCHAR(40),
		SEGMENT4 NVARCHAR(40),
		LOCATOR_STATUS BIGINT,
		LOCATOR_STATUS_CODE NVARCHAR(50),
		LOCATOR_PICKING_ORDER BIGINT,
		LOCATOR_DISABLE_DATE DATETIME
	);
	
	DECLARE @mergelist TABLE (
		LOCATOR_ID BIGINT,
		ORGANIZATION_ID BIGINT,
		SUBINVENTORY_CODE NVARCHAR(20),
		LOCATOR_SEGMENTS NVARCHAR(163),
		LOCATOR_DESC NVARCHAR(50),
		SEGMENT1 NVARCHAR(40),
		SEGMENT2 NVARCHAR(40),
		SEGMENT3 NVARCHAR(40),
		SEGMENT4 NVARCHAR(40),
		LOCATOR_STATUS BIGINT,
		LOCATOR_STATUS_CODE NVARCHAR(50),
		LOCATOR_PICKING_ORDER BIGINT,
		LOCATOR_DISABLE_DATE DATETIME,
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
			LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
			SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
			LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE
		) 
		SELECT 
			LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
			SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
			LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE 
		FROM (
			SELECT 
				LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE
			FROM LOCATOR_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT 
				LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE
			FROM LOCATOR_TMP_T T
		) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE LOCATOR_T SET CONTROL_FLAG = 'D' WHERE 
				LOCATOR_ID IN (SELECT LOCATOR_ID FROM @deletelist)
			
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
				LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, CONTROL_FLAG) 
		SELECT LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, CONTROL_FLAG 
		FROM (
			SELECT LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, CONTROL_FLAG 
			FROM LOCATOR_TMP_T T
			EXCEPT
			SELECT LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
				SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
				LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, CONTROL_FLAG 
			FROM LOCATOR_T O
		) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO LOCATOR_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON A.LOCATOR_ID = B.LOCATOR_ID
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET 
						A.CONTROL_FLAG = '', A.ORGANIZATION_ID = B.ORGANIZATION_ID, A.SUBINVENTORY_CODE = B.SUBINVENTORY_CODE, A.LOCATOR_SEGMENTS = B.LOCATOR_SEGMENTS,
						A.LOCATOR_DESC = B.LOCATOR_DESC, A.SEGMENT1 = B.SEGMENT1, A.SEGMENT2 = B.SEGMENT2, A.SEGMENT3 = B.SEGMENT3, A.SEGMENT4 = B.SEGMENT4,
						A.LOCATOR_STATUS = B.LOCATOR_STATUS, A.LOCATOR_STATUS_CODE = B.LOCATOR_STATUS_CODE, A.LOCATOR_PICKING_ORDER = B.LOCATOR_PICKING_ORDER, A.LOCATOR_DISABLE_DATE = B.LOCATOR_DISABLE_DATE
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (
						LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
						SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
						LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, CONTROL_FLAG 
					) VALUES (
						LOCATOR_ID, ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_SEGMENTS, LOCATOR_DESC, 
						SEGMENT1, SEGMENT2, SEGMENT3, SEGMENT4, LOCATOR_STATUS, 
						LOCATOR_STATUS_CODE, LOCATOR_PICKING_ORDER, LOCATOR_DISABLE_DATE, ''
					)
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

