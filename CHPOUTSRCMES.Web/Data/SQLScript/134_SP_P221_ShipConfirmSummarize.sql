-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/1
-- Description:	SOA OSP_BATCH_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P221_ShipConfirmSummarize]
	@tripId BIGINT,
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
		INSERT INTO XXIF_CHP_CONTROL_ST (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, ROW_NUM, PROCESS_DATE
			, REQUEST_ID, STATUS_CODE, ERROR_MSG, SOA_PULLING_FLAG, SOA_ERROR_MSG
			, SOA_PROCESS_CODE, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN
		)
		SELECT 
			A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID, COUNT(*) AS ROW_NUM, MAX(A.TRANSACTION_DATE)
			, NULL, NULL, NULL, 'In-W', NULL
			, NULL, -1, MAX(A.CREATION_DATE), -1, MAX(A.LAST_UPDATE_DATE)
			, -1
		FROM XXIF_CHP_P221_SHIP_CONFIRM_ST A
		WHERE A.PROCESS_CODE = @processCode AND A.SERVER_CODE = @serverCode AND A.BATCH_ID = @batchId
		GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode 
				+ ' BATCH_ID:' + @batchId + ' 寫入 XXIF_CHP_CONTROL_ST 失敗'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO DLV_SOA_T (
			TRIP_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, CREATED_BY, CREATION_DATE
		) 
		SELECT 
			TRIP_ID, @processCode AS PROCESS_CODE, @serverCode AS SERVER_CODE, @batchId AS BATCH_ID, @user, GETDATE()
		FROM DLV_HEADER_T H
		WHERE TRIP_ID = @tripId 
		GROUP BY TRIP_ID

		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 DLV_SOA_T 錯誤'
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

