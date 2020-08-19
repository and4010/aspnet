-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/14
-- Description:	紙別機台資料同步(MACHINE_PAPER_TYPE_T 及 MACHINE_PAPER_TYPE_TMP_T)
-- =============================================
CREATE PROCEDURE [dbo].[SP_MachinePaperTypeSync]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	
	DECLARE @deletelist TABLE (
		MACHINE_CODE NVARCHAR(30),
		ORGANIZATION_ID BIGINT,
		ORGANIZATION_CODE NVARCHAR(3),
		MACHINE_MEANING NVARCHAR(80),
		DESCRIPTION NVARCHAR(240),
		PAPER_TYPE NVARCHAR(4),
		MACHINE_NUM NVARCHAR(10),
		SUPPLIER_NUM NVARCHAR(4),
		SUPPLIER_NAME NVARCHAR(240),
		CREATED_BY BIGINT,
		CREATION_DATE DATETIME,
		LAST_UPDATE_BY BIGINT,
		LAST_UPDATE_DATE DATETIME
	);
	
	DECLARE @mergelist TABLE (
		MACHINE_CODE NVARCHAR(30),
		ORGANIZATION_ID BIGINT,
		ORGANIZATION_CODE NVARCHAR(3),
		MACHINE_MEANING NVARCHAR(80),
		DESCRIPTION NVARCHAR(240),
		PAPER_TYPE NVARCHAR(4),
		MACHINE_NUM NVARCHAR(10),
		SUPPLIER_NUM NVARCHAR(4),
		SUPPLIER_NAME NVARCHAR(240),
		CREATED_BY BIGINT,
		CREATION_DATE DATETIME,
		LAST_UPDATE_BY BIGINT,
		LAST_UPDATE_DATE DATETIME,
		CONTROL_FLAG CHAR(1)
	);

	DECLARE @tableVar TABLE (
		MergeAction VARCHAR(20), 
		InsertedID  NVARCHAR(30), 
		DeletedID  NVARCHAR(30)
	);
	DECLARE @success INT = 0, @code INT = 0, @message NVARCHAR(500) = '';
	DECLARE @deleteCount INT = 0, @updateCount INT = 0, @insertCount INT = 0;
	
	
	BEGIN TRY
		SET @success = @success + 1
		--收集需要刪除的項目
		INSERT INTO @deletelist (
			MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
			PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
			CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
		) 
		SELECT 
			MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
			PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
			CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
		FROM (
			SELECT 
				MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
			FROM MACHINE_PAPER_TYPE_T O
			WHERE CONTROL_FLAG <> 'D'
			EXCEPT
			SELECT 
				MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
			FROM MACHINE_PAPER_TYPE_TMP_T T
		) A

		--如有須刪除的 更新CONTROL_FLAG = D
		IF EXISTS (SELECT TOP 1 * FROM @deletelist)
		BEGIN
			--更新CONTROL_FLAG = D
			UPDATE MACHINE_PAPER_TYPE_T SET CONTROL_FLAG = 'D' WHERE 
				MACHINE_CODE IN (SELECT MACHINE_CODE FROM @deletelist)
			
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
				MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, CONTROL_FLAG) 
		SELECT	MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, CONTROL_FLAG 
		FROM (
			SELECT MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM MACHINE_PAPER_TYPE_TMP_T T
			EXCEPT
			SELECT MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
				PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
				CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, CONTROL_FLAG 
			FROM MACHINE_PAPER_TYPE_T O
		) A

		IF EXISTS (SELECT TOP 1 * FROM @mergelist)
		BEGIN
			MERGE INTO MACHINE_PAPER_TYPE_T WITH (HOLDLOCK) AS A 
				USING @mergelist AS B
				ON A.MACHINE_CODE = B.MACHINE_CODE
				--更新CONTROL_FLAG = '' 及其它欄位值
				WHEN MATCHED THEN 
					UPDATE SET 
						A.CONTROL_FLAG = '', A.MACHINE_CODE = B.MACHINE_CODE, A.ORGANIZATION_ID = B.ORGANIZATION_ID, A.ORGANIZATION_CODE = B.ORGANIZATION_CODE,
						A.MACHINE_MEANING = B.MACHINE_MEANING, A.DESCRIPTION = B.DESCRIPTION, A.PAPER_TYPE = B.PAPER_TYPE, A.MACHINE_NUM = B.MACHINE_NUM, 
						A.SUPPLIER_NUM = B.SUPPLIER_NUM, A.SUPPLIER_NAME = B.SUPPLIER_NAME, A.CREATED_BY = B.CREATED_BY, A.CREATION_DATE = B.CREATION_DATE, 
						A.LAST_UPDATE_BY = B.LAST_UPDATE_BY, A.LAST_UPDATE_DATE = B.LAST_UPDATE_DATE
				--新增資料
				WHEN NOT MATCHED THEN 
					INSERT (
						MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
						PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
						CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, CONTROL_FLAG 
					) VALUES (
						MACHINE_CODE, ORGANIZATION_ID, ORGANIZATION_CODE, MACHINE_MEANING, DESCRIPTION,
						PAPER_TYPE, MACHINE_NUM, SUPPLIER_NUM, SUPPLIER_NAME, CREATED_BY,
						CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE, ''
					)
				OUTPUT $action, inserted.MACHINE_CODE 'inserted', deleted.MACHINE_CODE 'deleted' INTO @tableVar;

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
