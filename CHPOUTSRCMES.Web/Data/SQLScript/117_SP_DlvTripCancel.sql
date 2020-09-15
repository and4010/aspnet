-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/11
-- Description:	取消航程號
-- =============================================
CREATE PROCEDURE [dbo].[SP_DlvTripDelete]
	@tripId BIGINT,
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN

	DECLARE @cancel TABLE (
		DLV_HEADER_ID BIGINT
	)

	DECLARE @success INT = 0
	DECLARE @now DATETIME = GETDATE();
	BEGIN TRY
		
		SET @success = @success + 1
		INSERT INTO @cancel (DLV_HEADER_ID) 
		SELECT DLV_HEADER_ID FROM DLV_HEADER_T
		WHERE TRIP_ID = @tripId AND DELIVERY_STATUS_CODE != 'DH5'

		IF EXISTS(SELECT TOP 1 * FROM @cancel) 
		BEGIN

			SET @success = @success + 1
			INSERT INTO dbo.STK_TXN_T (
				STOCK_ID, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY_CODE, LOCATOR_ID
				, DST_ORGANIZATION_ID, DST_ORGANIZATION_CODE, DST_SUBINVENTORY_CODE, DST_LOCATOR_ID, INVENTORY_ITEM_ID
				, ITEM_NUMBER, ITEM_DESCRIPTION, ITEM_CATEGORY, LOT_NUMBER, BARCODE
				, PRY_UOM_CODE, PRY_BEF_QTY, PRY_AFT_QTY, PRY_CHG_QTY, SEC_UOM_CODE
				, SEC_BEF_QTY, SEC_CHG_QTY, SEC_AFT_QTY, CATEGORY, DOC
				, ACTION, NOTE, STATUS_CODE, CREATED_BY, CREATION_DATE
				, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			SELECT P.STOCK_ID, H.ORGANIZATION_ID, H.ORGANIZATION_CODE, H.SUBINVENTORY_CODE, P.LOCATOR_ID
				, NULL, NULL, NULL, NULL, P.INVENTORY_ITEM_ID
				, P.ITEM_NUMBER, I.ITEM_DESC_TCH, I.CATALOG_ELEM_VAL_070, P.LOT_NUMBER, P.BARCODE
				, P.PRIMARY_UOM, S.PRIMARY_AVAILABLE_QTY, S.PRIMARY_AVAILABLE_QTY + P.PRIMARY_QUANTITY, P.PRIMARY_QUANTITY, P.SECONDARY_UOM
				, S.SECONDARY_AVAILABLE_QTY, P.SECONDARY_QUANTITY, S.SECONDARY_AVAILABLE_QTY + P.SECONDARY_QUANTITY, '出貨', H.TRIP_NAME
				, '刪除', '', S.STATUS_CODE, @user, @now,
				NULL, NULL
			FROM DLV_PICKED_T P
			JOIN DLV_HEADER_T H ON H.DLV_HEADER_ID = P.DLV_HEADER_ID
			JOIN ITEMS_T I ON I.INVENTORY_ITEM_ID = P.INVENTORY_ITEM_ID
			JOIN STOCK_T S ON S.STOCK_ID = P.STOCK_ID
			WHERE H.DLV_HEADER_ID IN (SELECT DLV_HEADER_ID FROM @cancel)

			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-新增庫存交易記錄失敗'
				RAISERROR(@message, 16, @success)
			END

			SET @success = @success + 1
			UPDATE S SET 
				PRIMARY_AVAILABLE_QTY = S.PRIMARY_AVAILABLE_QTY + P.PRIMARY_QUANTITY
				, PRIMARY_LOCKED_QTY = S.PRIMARY_LOCKED_QTY - P.PRIMARY_QUANTITY
				, SECONDARY_AVAILABLE_QTY = S.SECONDARY_AVAILABLE_QTY + P.SECONDARY_QUANTITY 
				, SECONDARY_LOCKED_QTY = S.SECONDARY_LOCKED_QTY - P.SECONDARY_QUANTITY 
				, STATUS_CODE = CASE WHEN (S.PRIMARY_AVAILABLE_QTY + P.PRIMARY_QUANTITY) > 0 THEN 'S0' ELSE S.STATUS_CODE END
				, LAST_UPDATE_BY = @user
				, LAST_UPDATE_DATE = @now
			FROM DLV_PICKED_T P
			JOIN STOCK_T S ON S.STOCK_ID = P.STOCK_ID
			WHERE DLV_HEADER_ID IN (SELECT DLV_HEADER_ID FROM @cancel)
	
			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-更新庫存失敗'
				RAISERROR(@message, 16, @success)
			END

			SET @success = @success + 1
			DELETE P FROM DLV_PICKED_HT P 
			JOIN DLV_HEADER_T H ON H.DLV_HEADER_ID = P.DLV_HEADER_ID
			JOIN @cancel C ON C.DLV_HEADER_ID = H.DLV_HEADER_ID

			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-刪除揀貨歷史失敗'
				RAISERROR(@message, 16, @success)
			END

			SET @success = @success + 1
			DELETE P FROM DLV_PICKED_T P 
			JOIN DLV_HEADER_T H ON H.DLV_HEADER_ID = P.DLV_HEADER_ID
			JOIN @cancel C ON C.DLV_HEADER_ID = H.DLV_HEADER_ID

			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-刪除揀貨失敗'
				RAISERROR(@message, 16, @success)
			END

			SET @success = @success + 1
			DELETE D FROM DLV_DETAIL_HT D 
			JOIN DLV_HEADER_T H ON H.DLV_HEADER_ID = D.DLV_HEADER_ID
			JOIN @cancel C ON C.DLV_HEADER_ID = H.DLV_HEADER_ID

			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-更新出貨歷史明細失敗'
				RAISERROR(@message, 16, @success)
			END

			DELETE D FROM DLV_DETAIL_T D 
			JOIN DLV_HEADER_T H ON H.DLV_HEADER_ID = D.DLV_HEADER_ID
			JOIN @cancel C ON C.DLV_HEADER_ID = H.DLV_HEADER_ID

			IF(@@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-更新出貨明細失敗'
				RAISERROR(@message, 16, @success)
			END

			DELETE H FROM DLV_HEADER_T H 
			JOIN @cancel C ON C.DLV_HEADER_ID = H.DLV_HEADER_ID

			IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = @message + ' 取消航程號-更新單號狀態失敗'
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

END
