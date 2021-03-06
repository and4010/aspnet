-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/22
-- Description:	SOA SUB_TRANSFER_ST 資料上傳
-- =============================================
CREATE PROCEDURE [dbo].[SP_P222_TrfMiscStUpload]
	@trfMiscHeaderId BIGINT,
	@processCode VARCHAR(20) OUTPUT,
	@serverCode VARCHAR(20) OUTPUT,
	@batchId VARCHAR(20) OUTPUT,
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
		, TRANSFER_MISCELLANEOUS_HEADER_ID BIGINT
		, TRANSACTION_DATE DATETIME
	)

	DECLARE @success INT = 0
	DECLARE @count INT = 0
	SELECT @message = 'TRANSFER_MISCELLANEOUS_HEADER_ID:' + CONVERT(varchar, @trfMiscHeaderId)

	SET @processCode = 'XXIFP222'
	SET @serverCode = 'FTY'
	SET @batchId = FORMAT(SYSDATETIME(), 'yyyyMMddHHmmssffffff')

	BEGIN TRY
		
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
			, ATTRIBUTE1
			, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE )
		OUTPUT inserted.PROCESS_CODE, inserted.SERVER_CODE, inserted.BATCH_ID, inserted.BATCH_LINE_ID, @trfMiscHeaderId, inserted.TRANSACTION_DATE
		INTO @table (PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, TRANSFER_MISCELLANEOUS_HEADER_ID, TRANSACTION_DATE)
		SELECT 
		@processCode, @serverCode, @batchId, ROW_NUMBER() OVER (ORDER BY (SELECT 1)), NULL
		, NULL, MIN(H.ORG_ID), MIN(O.ORG_NAME), MIN(H.ORGANIZATION_ID), MIN(H.ORGANIZATION_CODE)
		, NULL, MIN(H.SUBINVENTORY_CODE), MIN(H.LOCATOR_ID), MIN(H.LOCATOR_CODE), MIN(H.TRANSACTION_DATE)
		, MIN(H.TRANSACTION_TYPE_ID), MIN(H.TRANSACTION_TYPE_NAME), MIN(H.TRANSFER_ORG_ID), MIN(T.ORG_NAME), MIN(H.TRANSFER_ORGANIZATION_ID)
		, MIN(H.TRANSFER_ORGANIZATION_CODE), MIN(H.TRANSFER_SUBINVENTORY_CODE), MIN(H.TRANSFER_LOCATOR_ID), MIN(H.TRANSFER_LOCATOR_CODE), D.INVENTORY_ITEM_ID
		, MIN(D.ITEM_NUMBER), MIN(D.ITEM_DESCRIPTION)
		, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN '平版' THEN MIN(D.SECONDARY_UOM) ELSE MIN(D.PRIMARY_UOM) END
		, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN '平版' THEN SUM(D.TRANSFER_SECONDARY_QUANTITY) ELSE MIN(D.TRANSFER_PRIMARY_QUANTITY) END
		, MIN(D.PRIMARY_UOM)
		, MIN(D.TRANSFER_PRIMARY_QUANTITY)
		, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN '平版' THEN MIN(D.SECONDARY_UOM) ELSE NULL END
		, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN '平版' THEN SUM(D.TRANSFER_SECONDARY_QUANTITY) ELSE NULL END
		, dbo.ROLL_NUMBER(MIN(I.CATALOG_ELEM_VAL_070), MIN(D.LOT_NUMBER)) AS ROLL_NUMBER
		, dbo.ROLL_QUANTITY(MIN(I.CATALOG_ELEM_VAL_070), MIN(D.TRANSFER_PRIMARY_QUANTITY)) AS ROLL_NUMBER
		, dbo.GetAttributeByTransactionTypeId(MIN(H.TRANSACTION_TYPE_ID))
		, NULL, NULL, NULL, NULL
		, NULL, NULL, NULL, NULL, NULL
		, NULL, NULL, NULL, NULL, NULL
		, NULL, -1, MAX(D.CREATION_DATE), -1, ISNULL(MAX(D.LAST_UPDATE_DATE), MAX(D.CREATION_DATE)) 
		FROM [TRF_MISCELLANEOUS_HEADER_T] H
		JOIN [TRF_MISCELLANEOUS_HT] D ON D.TRANSFER_MISCELLANEOUS_HEADER_ID = H.TRANSFER_MISCELLANEOUS_HEADER_ID
		JOIN [ORG_UNIT_T] O ON O.ORG_ID = H.ORG_ID
		LEFT JOIN [ORG_UNIT_T] T ON T.ORG_ID = H.TRANSFER_ORG_ID
		JOIN [ITEMS_T] I ON I.INVENTORY_ITEM_ID = D.INVENTORY_ITEM_ID
 		WHERE H.TRANSFER_MISCELLANEOUS_HEADER_ID = @trfMiscHeaderId
		GROUP BY H.TRANSFER_MISCELLANEOUS_HEADER_ID, D.INVENTORY_ITEM_ID, D.LOT_NUMBER
		
		IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' TRANSFER_MISCELLANEOUS_HEADER_ID:' + CONVERT(varchar, @trfMiscHeaderId) + ' 上傳儲位異動失敗'
			RAISERROR(@message, 16, @success)
		END

		SET @success = @success + 1
		UPDATE dbo.XXIF_CHP_P222_SUB_TRANSFER_ST SET PRIMARY_QUANTITY = PQTY, SECONDARY_QUANTITY = SQTY, TRANSACTION_QUANTITY = TQTY
		FROM (
		SELECT
		S.PROCESS_CODE, S.SERVER_CODE, S.BATCH_ID, S.ATTRIBUTE1, S.INVENTORY_ITEM_ID
		, SUM(S.PRIMARY_QUANTITY) AS PQTY
		, SUM(S.SECONDARY_QUANTITY) AS SQTY
		, SUM(S.TRANSACTION_QUANTITY) AS TQTY
		FROM dbo.XXIF_CHP_P222_SUB_TRANSFER_ST S
		WHERE S.PROCESS_CODE = @processCode AND S.SERVER_CODE = @serverCode AND S.BATCH_ID = @batchId
		GROUP BY S.PROCESS_CODE, S.SERVER_CODE, S.BATCH_ID, S.ATTRIBUTE1, S.INVENTORY_ITEM_ID
		) A WHERE A.PROCESS_CODE = XXIF_CHP_P222_SUB_TRANSFER_ST.PROCESS_CODE 
			AND A.SERVER_CODE = XXIF_CHP_P222_SUB_TRANSFER_ST.SERVER_CODE
			AND A.BATCH_ID = XXIF_CHP_P222_SUB_TRANSFER_ST.BATCH_ID
			AND A.ATTRIBUTE1 = XXIF_CHP_P222_SUB_TRANSFER_ST.ATTRIBUTE1
			AND A.INVENTORY_ITEM_ID = XXIF_CHP_P222_SUB_TRANSFER_ST.INVENTORY_ITEM_ID
		IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' TRANSFER_MISCELLANEOUS_HEADER_ID:' + CONVERT(varchar, @trfMiscHeaderId) + ' 更新數量'
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

