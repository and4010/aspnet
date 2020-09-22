-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/4
-- Description:	SOA CONTAINER_RV_ST 資料上傳
-- =============================================
CREATE PROCEDURE [dbo].[SP_P222_CtrStUpload]
	@ctrHeaderId BIGINT,
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN


	DECLARE @table TABLE (
		PROCESS_CODE VARCHAR(20)
		, SERVER_CODE VARCHAR(20)
		, BATCH_ID VARCHAR(20)
		, BATCH_LINE_ID BIGINT
		, CTR_HEADER_ID BIGINT
	)

	DECLARE @success INT = 0
	DECLARE @count INT = 0
	SELECT @message = 'CTR_HEADER_ID:' + CONVERT(varchar, @ctrHeaderId)

	DECLARE @txnDate DATETIME = GETDATE()
	DECLARE @processCode VARCHAR(20) = 'XXIFP218', @serverCode VARCHAR(20) = 'FTY'
	DECLARE @batchId VARCHAR(20) = FORMAT(@txnDate, 'yyyyMMddHHmmssffffff')

	BEGIN TRY
		
		--取出待處理項目
		SET @success = @success + 1
		INSERT INTO XXIF_CHP_P218_CONTAINER_RV_ST (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, HEADER_ID, ORG_ID, ORG_NAME, LINE_ID
			, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY, LOCATOR_ID, LOCATOR_CODE
			, DETAIL_ID, INVENTORY_ITEM_ID, SHIP_ITEM_NUMBER, TRANSACTION_DATE, TRANSACTION_QUANTITY
			, TRANSACTION_UOM, PRIMARY_QUANTITY, PRIMARY_UOM, SECONDARY_QUANTITY, SECONDARY_UOM
			, ROLL_NUMBER
			, ROLL_QUANTITY
			, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3
			, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8
			, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13
			, ATTRIBUTE14, ATTRIBUTE15, REQUEST_ID, CREATED_BY, CREATION_DATE
			, LAST_UPDATED_BY, LAST_UPDATE_DATE )

		OUTPUT inserted.PROCESS_CODE, inserted.SERVER_CODE, inserted.BATCH_ID, inserted.BATCH_LINE_ID, @ctrHeaderId INTO @table
		SELECT 
			@processCode, @serverCode, @batchId, ROW_NUMBER() OVER (ORDER BY (SELECT 1)), NULL
			, NULL, H.HEADER_ID, H.ORG_ID, H.ORG_NAME, H.LINE_ID
			, H.ORGANIZATION_ID, H.ORGANIZATION_CODE, H.SUBINVENTORY , D.LOCATOR_ID, D.LOCATOR_CODE
			, D.DETAIL_ID, D.INVENTORY_ITEM_ID, D.SHIP_ITEM_NUMBER, @txnDate, D.TRANSACTION_QUANTITY
			, D.TRANSACTION_UOM, D.PRIMARY_QUANTITY, D.PRIMARY_UOM, D.SECONDARY_QUANTITY, D.SECONDARY_UOM
			, CASE WHEN LEN(P.LOT_NUMBER) > 0 THEN P.LOT_NUMBER ELSE '' END AS ROLL_NUMBER
			, CASE WHEN LEN(P.THEORY_WEIGHT) > 0 THEN P.THEORY_WEIGHT ELSE '' END AS ROLL_QUANTITY
			, NULL, NULL, NULL
			, NULL, NULL, NULL, NULL, NULL
			, NULL, NULL, NULL, NULL, NULL
			, NULL, NULL, NULL, -1, P.CREATION_DATE
			, -1, ISNULL(P.LAST_UPDATE_DATE, P.CREATION_DATE)
		FROM CTR_HEADER_T H
		JOIN CTR_DETAIL_HT D ON D.CTR_HEADER_ID = H.CTR_HEADER_ID
		JOIN CTR_PICKED_HT P ON P.CTR_DETAIL_ID = D.CTR_DETAIL_ID
		JOIN CTR_ORG_T O ON O.PROCESS_CODE = D.PROCESS_CODE AND O.SERVER_CODE = D.SERVER_CODE AND O.BATCH_ID = D.BATCH_ID AND O.BATCH_LINE_ID = D.BATCH_LINE_ID
		WHERE H.CTR_HEADER_ID = @ctrHeaderId
		ORDER BY D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, D.BATCH_LINE_ID, P.CTR_PICKED_ID
		
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' CTR_HEADER_ID:' + CONVERT(varchar, @ctrHeaderId) + ' 上傳進櫃入庫失敗'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO XXIF_CHP_CONTROL_ST (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, ROW_NUM, PROCESS_DATE
			, REQUEST_ID, STATUS_CODE, ERROR_MSG, SOA_PULLING_FLAG, SOA_ERROR_MSG
			, SOA_PROCESS_CODE, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN
		)
		SELECT 
			C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID, COUNT(*) AS ROW_NUM, @txnDate
			, NULL, NULL, NULL, 'In-W', NULL
			, NULL, -1, MAX(C.CREATION_DATE), -1, MAX(C.LAST_UPDATE_DATE)
			, -1
		FROM XXIF_CHP_P218_CONTAINER_RV_ST C
		JOIN @table T ON T.PROCESS_CODE = C.PROCESS_CODE AND T.SERVER_CODE = C.SERVER_CODE AND T.BATCH_ID = C.BATCH_ID AND T.BATCH_LINE_ID = C.BATCH_LINE_ID
		GROUP BY C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID

		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' CTR_HEADER_ID:' + CONVERT(varchar, @ctrHeaderId) + ' 寫入 XXIF_CHP_CONTROL_ST 失敗'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO CTR_SOA_T (
			CTR_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, CREATED_BY, CREATION_DATE
		) 
		SELECT 
			CTR_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, @user, @txnDate
		FROM @table 
		GROUP BY CTR_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID

		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 CTR_SOA_T 錯誤'
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

