-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/1
-- Description:	SOA OSP_BATCH_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P219_OspStSummarize]
	@processCode NVARCHAR(20),
	@serverCode NVARCHAR(20),
	@batchId NVARCHAR(20),
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN

	DECLARE @controlStage TABLE (
		PROCESS_CODE NVARCHAR(20),
		SERVER_CODE NVARCHAR(20),
		BATCH_ID NVARCHAR(20),
		STATUS_CODE NVARCHAR(1),
		ERROR_MSG NVARCHAR(2000),
		SOA_PULLING_FLAG NVARCHAR(30),
		SOA_PROCESS_CODE NVARCHAR(30)
	)

	DECLARE @success INT = 0
	
	SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId

	BEGIN TRY
		
		--取出出貨明細，檢查資料是否正確?
		SET @success = @success + 1

		IF EXISTS ( 
			SELECT TOP 1 * FROM XXIF_CHP_P219_OSP_BATCH_ST B 
				WHERE B.STATUS_CODE = 'E' AND B.PROCESS_CODE = @processCode 
				AND B.SERVER_CODE = @serverCode AND B.BATCH_ID = @batchId)
		BEGIN
			UPDATE C SET 
				STATUS_CODE = 'E'
				, ERROR_MSG = '資料輸入錯誤，詳見明細'
				, SOA_PULLING_FLAG = 'OutAck-W'
			FROM XXIF_CHP_CONTROL_ST C
			JOIN 
			(
			SELECT 
				A.PROCESS_CODE
				, A.SERVER_CODE
				, A.BATCH_ID
				, 'E' AS STATUS_CODE
			FROM XXIF_CHP_CONTROL_ST A
			JOIN XXIF_CHP_P219_OSP_BATCH_ST B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
			WHERE B.STATUS_CODE = 'E' AND B.PROCESS_CODE = @processCode AND B.SERVER_CODE = @serverCode AND B.BATCH_ID = @batchId
			GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
			) D ON C.PROCESS_CODE = D.PROCESS_CODE AND C.SERVER_CODE = D.SERVER_CODE AND C.BATCH_ID = D.BATCH_ID
		END
		ELSE
		BEGIN
			UPDATE A SET 
				STATUS_CODE = 'S'
				, ERROR_MSG = NULL
				, SOA_PULLING_FLAG = 'OutAck-W'
			FROM XXIF_CHP_CONTROL_ST A
			JOIN XXIF_CHP_P219_OSP_BATCH_ST B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
			WHERE B.PROCESS_CODE = @processCode AND B.SERVER_CODE = @serverCode AND B.BATCH_ID = @batchId
		END

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 錯誤!!'
			RAISERROR(@message, 16, @success)
		END

		SET @code = 0
		SET @message = ''
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()

	END CATCH

END

