-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/22
-- Description:	SOA SUB_TRANSFER_ST 資料上傳
-- =============================================
CREATE PROCEDURE [dbo].[SP_P222_TrfStUpload]
	@trfHeaderId BIGINT,
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
		, TRANSFER_HEADER_ID BIGINT
		, TRANSFER_TYPE NVARCHAR(10)
		, TRANSACTION_DATE DATETIME
	)

	DECLARE @success INT = 0
	DECLARE @count INT = 0
	SELECT @message = 'TRANSFER_HEADER_ID:' + CONVERT(varchar, @trfHeaderId)
	DECLARE @trfType NVARCHAR(10) = ''
	DECLARE @txnDate DATETIME = GETDATE()
	DECLARE @processCode VARCHAR(20) = 'XXIFP222', @serverCode VARCHAR(20) = 'FTY'
	DECLARE @batchId VARCHAR(20) = FORMAT(@txnDate, 'yyyyMMddHHmmssffffff')

	BEGIN TRY

		SELECT TOP 1 @trfType = TRANSFER_TYPE FROM TRF_HEADER_T WHERE TRANSFER_HEADER_ID = @trfHeaderId
		
		IF (@trfType IS NULL)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' TRANSFER_HEADER_ID:' + CONVERT(varchar, @trfHeaderId) + ' 找不到庫存移轉資料'
			RAISERROR(@message, 16, @success)
		END

		--取出待處理項目
		SET @success = @success + 1
		INSERT INTO dbo.XXIF_CHP_P222_SUB_TRANSFER_ST (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, ORG_ID, ORG_NAME, ORGANIZATION_ID, ORGANIZATION_CODE
			, SHIPMENT_NUMBER, SUBINVENTORY_CODE, LOCATOR_ID, LOCATOR_CODE, TRANSACTION_DATE
			, TRANSACTION_TYPE_ID, TRANSACTION_TYPE_NAME, TRANSFER_ORG_ID, TRANSFER_ORG_NAME, TRANSFER_ORGANIZATION_ID
			, TRANSFER_ORGANIZATION_CODE, TRANSFER_SUBINVENTORY_CODE, TRANSFER_LOCATOR_ID, TRANSFER_LOCATOR_CODE, INVENTORY_ITEM_ID
			, ITEM_NUMBER, ITEM_DESCRIPTION, TRANSACTION_UOM, TRANSACTION_QUANTITY, PRIMARY_UOM
			, PRIMARY_QUANTITY, SECONDARY_UOM_CODE, SECONDARY_QUANTITY, ROLL_NUMBER, ROLL_QUANTITY
			, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE )
		OUTPUT inserted.PROCESS_CODE, inserted.SERVER_CODE, inserted.BATCH_ID, inserted.BATCH_LINE_ID, @trfHeaderId, @trfType, inserted.TRANSACTION_DATE
		INTO @table (PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, TRANSFER_HEADER_ID, TRANSFER_TYPE, TRANSACTION_DATE)
		SELECT 
		@processCode, @serverCode, @batchId, ROW_NUMBER() OVER (ORDER BY (SELECT 1)), NULL
		, NULL, MIN(H.ORG_ID), MIN(O.ORG_NAME), MIN(H.ORGANIZATION_ID), MIN(H.ORGANIZATION_CODE)
		, MIN(H.SHIPMENT_NUMBER), MIN(H.SUBINVENTORY_CODE), MIN(H.LOCATOR_ID), MIN(H.LOCATOR_CODE), MIN(H.TRANSACTION_DATE)
		, MIN(H.TRANSACTION_TYPE_ID), MIN(H.TRANSACTION_TYPE_NAME), MIN(H.TRANSFER_ORG_ID), MIN(T.ORG_NAME), MIN(H.TRANSFER_ORGANIZATION_ID)
		, MIN(H.TRANSFER_ORGANIZATION_CODE), MIN(H.TRANSFER_SUBINVENTORY_CODE), MIN(H.TRANSFER_LOCATOR_ID), MIN(H.TRANSFER_LOCATOR_CODE), D.INVENTORY_ITEM_ID
		, MIN(D.ITEM_NUMBER), MIN(D.ITEM_DESCRIPTION), CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN MIN(P.SECONDARY_UOM) ELSE MIN(P.PRIMARY_UOM) END, CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN SUM(P.SECONDARY_QUANTITY) ELSE SUM(P.PRIMARY_QUANTITY) END, MIN(P.PRIMARY_UOM)
		, SUM(P.PRIMARY_QUANTITY), CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN MIN(P.SECONDARY_UOM) ELSE NULL END
		, CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN SUM(P.SECONDARY_QUANTITY) ELSE NULL END
		, CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN NULL ELSE MIN(P.LOT_NUMBER) END
		, CASE MIN(D.ITEM_CATEGORY) WHEN '平版' THEN NULL ELSE MIN(P.LOT_QUANTITY) END
		, MIN(H.TRANSFER_TYPE), NULL, NULL, NULL, NULL
		, NULL, NULL, NULL, NULL, NULL
		, NULL, NULL, NULL, NULL, NULL
		, NULL, -1, MAX(P.CREATION_DATE), -1, ISNULL(MAX(P.LAST_UPDATE_DATE), MAX(P.CREATION_DATE)) 
		FROM [TRF_HEADER_T] H
		JOIN [TRF_DETAIL_HT] D ON D.TRANSFER_HEADER_ID = H.TRANSFER_HEADER_ID
		JOIN [TRF_OUTBOUND_PICKED_HT] P ON P.TRANSFER_DETAIL_ID = D.TRANSFER_DETAIL_ID
		JOIN [ORG_UNIT_T] O ON O.ORG_ID = H.ORG_ID
		JOIN [ORG_UNIT_T] T ON T.ORG_ID = H.TRANSFER_ORG_ID
		WHERE H.TRANSFER_HEADER_ID = @trfHeaderId AND H.TRANSFER_TYPE = @trfType
		GROUP BY H.TRANSFER_HEADER_ID, D.INVENTORY_ITEM_ID, P.LOT_NUMBER
		
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' TRANSFER_HEADER_ID:' + CONVERT(varchar, @trfHeaderId) + 'TRANSFER_TYPE:' + @trfType + ' 上傳進櫃入庫失敗'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO XXIF_CHP_CONTROL_ST (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, ROW_NUM, PROCESS_DATE
			, REQUEST_ID, STATUS_CODE, ERROR_MSG, SOA_PULLING_FLAG, SOA_ERROR_MSG
			, SOA_PROCESS_CODE, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN
		)
		SELECT 
			C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID, COUNT(*) AS ROW_NUM, MIN(T.TRANSACTION_DATE)
			, NULL, NULL, NULL, 'In-W', NULL
			, NULL, -1, MAX(C.CREATION_DATE), -1, MAX(C.LAST_UPDATE_DATE)
			, -1
		FROM XXIF_CHP_P222_SUB_TRANSFER_ST C
		JOIN @table T ON T.PROCESS_CODE = C.PROCESS_CODE AND T.SERVER_CODE = C.SERVER_CODE AND T.BATCH_ID = C.BATCH_ID AND T.BATCH_LINE_ID = C.BATCH_LINE_ID
		GROUP BY C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID

		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' TRANSFER_HEADER_ID:' + CONVERT(varchar, @trfHeaderId) + 'TRANSFER_TYPE:' + @trfType + ' 寫入 XXIF_CHP_CONTROL_ST 失敗'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO TRF_SOA_T (
			TRANSFER_HEADER_ID, TRANSFER_TYPE, PROCESS_CODE, SERVER_CODE, BATCH_ID, STATUS_CODE, CREATED_BY, CREATION_DATE
		) 
		SELECT 
			TRANSFER_HEADER_ID, TRANSFER_TYPE, PROCESS_CODE, SERVER_CODE, BATCH_ID, NULL, @user, @txnDate
		FROM @table 
		GROUP BY TRANSFER_HEADER_ID, TRANSFER_TYPE, PROCESS_CODE, SERVER_CODE, BATCH_ID

		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 TRF_SOA_T 錯誤'
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

