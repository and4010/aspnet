-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/1
-- Description:	SOA OSP_BATCH_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P211_InMmtProdStSummarize]
	@ospHeaderId BIGINT,
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

	DECLARE @soa TABLE (
		PROCESS_CODE VARCHAR(20)
		, SERVER_CODE VARCHAR(20)
		, BATCH_ID VARCHAR(20)
		, OSP_SOA_S2_ID BIGINT
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
		FROM XXIF_CHP_P211_IN_MMT_PROD_ST A
		WHERE A.PROCESS_CODE = @processCode AND A.SERVER_CODE = @serverCode AND A.BATCH_ID = @batchId
		GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode 
				+ ' BATCH_ID:' + @batchId + ' 寫入 XXIF_CHP_CONTROL_ST 失敗'
			RAISERROR(@message, 16, @success)
		END

		SET @success = @success + 1
		INSERT INTO OSP_SOA_S2_T (
			OSP_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, STATUS_CODE
			, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
		) 
		OUTPUT inserted.PROCESS_CODE, inserted.SERVER_CODE, inserted.BATCH_ID, inserted.OSP_SOA_S2_ID INTO @soa (PROCESS_CODE, SERVER_CODE, BATCH_ID, OSP_SOA_S2_ID)
		SELECT 
			@ospHeaderId, PROCESS_CODE, SERVER_CODE, BATCH_ID, NULL
			, @user, MAX(TRANSACTION_DATE), NULL, NULL
		FROM XXIF_CHP_P211_IN_MMT_PROD_ST 
		WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
		GROUP BY PROCESS_CODE, SERVER_CODE, BATCH_ID

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 OSP_SOA_S2_T 錯誤'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO dbo.OSP_SOA_DTL_S2_T
           (OSP_HEADER_ID, OSP_SOA_S2_ID, INVENTORY_ITEM_ID)
		SELECT
			@ospHeaderId, MIN(S.OSP_SOA_S2_ID), I.INVENTORY_ITEM_ID
		FROM @soa S
		JOIN XXIF_CHP_P211_IN_MMT_PROD_ST D ON D.PROCESS_CODE = S.PROCESS_CODE AND D.SERVER_CODE = S.SERVER_CODE AND D.BATCH_ID = S.BATCH_ID
		JOIN ITEMS_T I ON I.ITEM_NUMBER = D.ITEM_NO
		GROUP BY D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, I.INVENTORY_ITEM_ID
		
		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 OSP_SOA_DTL_S2_T 錯誤'
			RAISERROR(@message, 16, @success)
		END

		UPDATE D SET RXID = SD1.OSP_SOA_DTL_S2_ID
		--SELECT SD1.OspSoaDtlS1Id, D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, D.BATCH_LINE_ID
		FROM XXIF_CHP_P211_IN_MMT_PROD_ST D
		JOIN ITEMS_T I ON I.ITEM_NUMBER = D.ITEM_NO
		JOIN @soa S ON S.PROCESS_CODE = D.PROCESS_CODE AND S.SERVER_CODE = D.SERVER_CODE AND S.BATCH_ID = D.BATCH_ID
		JOIN OSP_SOA_DTL_S2_T SD1 ON SD1.INVENTORY_ITEM_ID = I.INVENTORY_ITEM_ID AND SD1.OSP_SOA_S2_ID = S.OSP_SOA_S2_ID
		
		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 OSP_SOA_DTL_S2_T 錯誤'
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
