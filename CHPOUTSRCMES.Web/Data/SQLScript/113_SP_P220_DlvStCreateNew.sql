-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/31
-- Description:	SOA DELIVERY_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P220_DlvStCreateNew]
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

	DECLARE @orgTable TABLE (
		PROCESS_CODE NVARCHAR(20),
		SERVER_CODE NVARCHAR(20),
		BATCH_ID NVARCHAR(20),
		BATCH_LINE_ID BIGINT,
		TRIP_ID BIGINT,
		DELIVERY_ID BIGINT,
		NOTE NVARCHAR(240),
		STATUS_CODE NVARCHAR(1),
		ERROR_MSG NVARCHAR(2000),
		DELIVERY_STATUS_CODE NVARCHAR(10)
	)

	DECLARE @table TABLE (
		DLV_ORG_ID BIGINT
	)

	DECLARE @success INT = 0
	
	SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId

	BEGIN TRY
		
		--取出待處理項目
		SET @success = @success + 1
		INSERT INTO @controlStage (PROCESS_CODE, SERVER_CODE, BATCH_ID, STATUS_CODE, ERROR_MSG, SOA_PULLING_FLAG, SOA_PROCESS_CODE)
			SELECT PROCESS_CODE, SERVER_CODE, BATCH_ID, 'S' AS STATUS_CODE, ERROR_MSG, SOA_PULLING_FLAG, SOA_PROCESS_CODE FROM XXIF_CHP_CONTROL_ST 
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
		
		IF NOT EXISTS(SELECT TOP 1 * FROM @controlStage)
		BEGIN
			SET @message = @message + ' 無資料'
			RAISERROR(@message, 16, @success)
		END

		 --取出出貨明細，檢查資料是否正確?
		SET @success = @success + 1
		INSERT INTO @orgTable (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, TRIP_ID, DELIVERY_ID, NOTE, 
			STATUS_CODE, ERROR_MSG, DELIVERY_STATUS_CODE)
		SELECT 
			c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID, c.TRIP_ID, c.DELIVERY_ID, c.REMARK
			, 'S' AS STATUS_CDOE,
			CASE WHEN o.DLV_ORG_ID IS NOT NULL THEN N'出貨單資料已存在!! ' ELSE '' END +
			CASE WHEN DATEADD(day, 1, CAST( FLOOR(CAST(c.TRIP_ACTUAL_SHIP_DATE AS FLOAT)) AS DATETIME )) <= GETDATE() THEN '系統日期已超過 [組車日]!!' ELSE '' END +
			CASE WHEN r.ORGANIZATION_ID IS NULL THEN N'組織不存在!! ' WHEN r.CONTROL_FLAG = 'D' THEN N'組織已刪除!!' ELSE '' END +
			CASE WHEN s.SUBINVENTORY_CODE IS NULL THEN N'倉庫不存在!! ' WHEN s.CONTROL_FLAG = 'D' THEN N'倉庫已刪除!!' ELSE '' END +
			CASE WHEN l.LOCATOR_ID IS NULL AND s.OSP_FLAG = 'Y' THEN N'儲位不存在!!' WHEN l.CONTROL_FLAG = 'D' THEN N'儲位已刪除!!' ELSE '' END + 
			CASE WHEN t.INVENTORY_ITEM_ID IS NULL THEN N'料號資料不存在!! ' WHEN t.CONTROL_FLAG = 'D' THEN N'料號已刪除!!' ELSE '' END + 
			CASE WHEN c.BATCH_NO IS NOT NULL AND p.BATCH_NO IS NULL THEN N'代紙工單資料不存在!! ' ELSE '' END + 
			CASE WHEN c.INVENTORY_ITEM_NUMBER IS NOT NULL AND m.ITEM_NUMBER IS NULL THEN N'代紙料號不存在!! ' ELSE '' END
			--CASE WHEN h.TRIP_ID IS NOT NULL AND h.DELIVERY_STATUS_CODE = 'DH5' THEN N'此航程號已出貨!! ' ELSE '' END
			AS ERROR_MSG,
			'DH1' AS DELIVERY_STATUS_CODE
		  FROM [XXIF_CHP_CONTROL_ST] ctl
		  JOIN @controlStage cs ON cs.PROCESS_CODE = ctl.PROCESS_CODE AND cs.SERVER_CODE = ctl.SERVER_CODE AND cs.BATCH_ID = ctl.BATCH_ID
		  JOIN [XXIF_CHP_P220_DELIVERY_ST] c ON c.PROCESS_CODE = ctl.PROCESS_CODE AND c.SERVER_CODE = ctl.SERVER_CODE AND c.BATCH_ID = ctl.BATCH_ID
		  LEFT JOIN DLV_ORG_T o ON o.PROCESS_CODE = c.PROCESS_CODE AND o.SERVER_CODE = c.SERVER_CODE AND o.BATCH_ID = c.BATCH_ID AND o.BATCH_LINE_ID = c.BATCH_LINE_ID
		  LEFT JOIN ITEMS_T t ON t.INVENTORY_ITEM_ID = c.INVENTORY_ITEM_ID 
		  LEFT JOIN ORGANIZATION_T r ON r.ORGANIZATION_ID = c.ORGANIZATION_ID
		  LEFT JOIN SUBINVENTORY_T s ON s.SUBINVENTORY_CODE = c.SUBINVENTORY_CODE
		  LEFT JOIN LOCATOR_T l ON l.LOCATOR_ID = c.LOCATOR_ID AND l.SUBINVENTORY_CODE = c.SUBINVENTORY_CODE 
		  LEFT JOIN OSP_HEADER_T p ON p.BATCH_NO = c.BATCH_NO
		  LEFT JOIN ITEMS_T m ON m.ITEM_NUMBER = c.INVENTORY_ITEM_NUMBER
		  WHERE ctl.PROCESS_CODE = @processCode 
		  AND ctl.SERVER_CODE = @serverCode
		  AND ctl.BATCH_ID = @batchId

		IF NOT EXISTS(SELECT TOP 1 * FROM @orgTable)
		BEGIN
			SET @message = @message + ' 無資料'
			RAISERROR(@message, 16, @success)
		END

		--篩選已出貨，更新至狀態值 避免覆蓋
		UPDATE O SET DELIVERY_STATUS_CODE = 'DH5'
		FROM @orgTable O
		JOIN DLV_HEADER_T H ON H.TRIP_ID = O.TRIP_ID AND H.DELIVERY_STATUS_CODE = 'DH5'

		--更新狀態碼
		UPDATE O SET STATUS_CODE = CASE WHEN LEN(O.ERROR_MSG) = '' THEN 'S' ELSE 'E' END
		FROM @orgTable O

		--統計錯誤訊息
		SET @success = @success + 1
		UPDATE C SET STATUS_CODE = CASE WHEN RTRIM(D.ERROR_MSG) = '' THEN 'S' ELSE 'E' END, ERROR_MSG = CASE WHEN RTRIM(D.ERROR_MSG) = '' THEN NULL ELSE SUBSTRING(RTRIM(D.ERROR_MSG),0, 500) END
			FROM @controlStage C
			JOIN 
			(
			SELECT 
				A.PROCESS_CODE
				, A.SERVER_CODE
				, A.BATCH_ID
				, ( SELECT  ERROR_MSG + '' FROM @orgTable  d 
					WHERE d.PROCESS_CODE = A.PROCESS_CODE AND d.SERVER_CODE = A.SERVER_CODE AND d.BATCH_ID = A.BATCH_ID
					GROUP BY d.PROCESS_CODE, d.SERVER_CODE, d.BATCH_ID, d.ERROR_MSG 
					FOR XML PATH('')) AS ERROR_MSG
			FROM @controlStage A
			JOIN @orgTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
			GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
			) D ON C.PROCESS_CODE = D.PROCESS_CODE AND C.SERVER_CODE = D.SERVER_CODE AND C.BATCH_ID = D.BATCH_ID
		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 取消航程號錯誤'
			RAISERROR(@message, 16, @success)
		END


		SET @success = 10
		--資料正常 寫入進櫃主檔、檔頭及明細
		IF NOT EXISTS (SELECT TOP 1 * FROM @orgTable WHERE STATUS_CODE = 'E')
		BEGIN 
			
			--清除出貨資料，排除已貨的航程號
			DECLARE @tripId BIGINT = 0
			DECLARE csr CURSOR FOR SELECT DISTINCT TRIP_ID FROM @orgTable WHERE DELIVERY_STATUS_CODE <> 'DH5'
			OPEN csr
			FETCH NEXT FROM csr INTO @tripId  --(3.抓取Cursor中之記錄資料)
			WHILE @@FETCH_STATUS = 0   /*0: Fetch成功 -1: Fetch失敗 -2: 要Fetch之記錄已不存在(一般發生 在KEYSET之CURSOR) */ 
			BEGIN
				EXEC dbo.SP_DlvTripDelete @tripId, @code output, @message output, @user
				IF(@code != 0)
				BEGIN
					RAISERROR(@message, 16, @success)
				END
				FETCH NEXT FROM csr INTO @tripId
			END
			CLOSE csr
			DEALLOCATE csr

			--主檔 ，排除已貨的航程號
			INSERT INTO DLV_ORG_T (
				PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				, ORG_ID, ORG_NAME, ORGANIZATION_ID, ORGANIZATION_CODE, TRIP_CAR
				, TRIP_ID, TRIP_NAME, TRIP_ACTUAL_SHIP_DATE, DELIVERY_ID, DELIVERY_NAME
				, CUSTOMER_ID, CUSTOMER_NUMBER, CUSTOMER_NAME, CUSTOMER_LOCATION_CODE, SHIP_CUSTOMER_ID
				, SHIP_CUSTOMER_NUMBER, SHIP_CUSTOMER_NAME, SHIP_LOCATION_CODE, FREIGHT_TERMS_NAME, ORDER_HEADER_ID
				, Order_Number, ORDER_LINE_ID, ORDER_SHIP_NUMBER, DELIVERY_DETAIL_ID, PACKING_TYPE
				, INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, PAPER_TYPE, BASIC_WEIGHT
				, SPECIFICATION, GRAIN_DIRECTION, SUBINVENTORY_CODE, LOCATOR_ID, LOCATOR_CODE
				, SRC_REQUESTED_QUANTITY, SRC_REQUESTED_QUANTITY_UOM, REQUESTED_QUANTITY, REQUESTED_QUANTITY_UOM, REQUESTED_QUANTITY2
				, REQUESTED_QUANTITY_UOM2, BATCH_NO, INVENTORY_ITEM_NUMBER
				, REMARK, REQUEST_ID, ATTRIBUTE1, ATTRIBUTE2
				, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7
				, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12
				, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15, CREATED_BY, CREATION_DATE
				, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			OUTPUT inserted.DLV_ORG_ID INTO @table (DLV_ORG_ID)
			SELECT
				c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID
				, c.ORG_ID, c.ORG_NAME, c.ORGANIZATION_ID, c.ORGANIZATION_CODE, c.TRIP_CAR
				, c.TRIP_ID, c.TRIP_NAME, c.TRIP_ACTUAL_SHIP_DATE, c.DELIVERY_ID, c.DELIVERY_NAME
				, c.CUSTOMER_ID, c.CUSTOMER_NUMBER, c.CUSTOMER_NAME, c.CUSTOMER_LOCATION_CODE, c.SHIP_CUSTOMER_ID
				, c.SHIP_CUSTOMER_NUMBER, c.SHIP_CUSTOMER_NAME, c.SHIP_LOCATION_CODE, c.FREIGHT_TERMS_NAME, c.ORDER_HEADER_ID
				, c.Order_Number, c.ORDER_LINE_ID, c.ORDER_SHIP_NUMBER, c.DELIVERY_DETAIL_ID, c.PACKING_TYPE
				, c.INVENTORY_ITEM_ID, c.ITEM_NUMBER, c.ITEM_DESCRIPTION, c.PAPER_TYPE, c.BASIC_WEIGHT
				, c.SPECIFICATION, c.GRAIN_DIRECTION, c.SUBINVENTORY_CODE, c.LOCATOR_ID, c.LOCATOR_CODE
				, c.SRC_REQUESTED_QUANTITY, c.SRC_REQUESTED_QUANTITY_UOM, c.REQUESTED_QUANTITY, c.REQUESTED_QUANTITY_UOM, c.REQUESTED_QUANTITY2
				, c.REQUESTED_QUANTITY_UOM2, c.BATCH_NO, c.INVENTORY_ITEM_NUMBER
				, c.REMARK, c.REQUEST_ID, c.ATTRIBUTE1, c.ATTRIBUTE2
				, c.ATTRIBUTE3, c.ATTRIBUTE4, c.ATTRIBUTE5, c.ATTRIBUTE6, c.ATTRIBUTE7
				, c.ATTRIBUTE8, c.ATTRIBUTE9, c.ATTRIBUTE10, c.ATTRIBUTE11, c.ATTRIBUTE12
				, c.ATTRIBUTE13, c.ATTRIBUTE14, c.ATTRIBUTE15, c.CREATED_BY, c.CREATION_DATE
				, c.LAST_UPDATED_BY, c.LAST_UPDATE_DATE

			FROM XXIF_CHP_P220_DELIVERY_ST c
			JOIN @orgTable d ON d.PROCESS_CODE = c.PROCESS_CODE AND d.SERVER_CODE = c.SERVER_CODE AND d.BATCH_ID = c.BATCH_ID AND d.BATCH_LINE_ID = c.BATCH_LINE_ID
			JOIN @controlStage s ON s.PROCESS_CODE = d.PROCESS_CODE AND s.SERVER_CODE = d.SERVER_CODE AND s.BATCH_ID = d.BATCH_ID
			WHERE s.STATUS_CODE = 'S' AND d.DELIVERY_STATUS_CODE <> 'DH5' AND d.STATUS_CODE = 'S'

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨主檔失敗'
				RAISERROR(@message, 16, @success)
			END

			--檔頭
			SET @success = @success + 1
			INSERT INTO DLV_HEADER_T (
				ORG_ID, ORG_NAME, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY_CODE
				, TRIP_CAR, TRIP_ID, TRIP_NAME, TRIP_ACTUAL_SHIP_DATE, DELIVERY_ID
				, DELIVERY_NAME, ITEM_CATEGORY, CUSTOMER_ID, CUSTOMER_NUMBER, CUSTOMER_NAME
				, CUSTOMER_LOCATION_CODE, SHIP_CUSTOMER_ID, SHIP_CUSTOMER_NUMBER, SHIP_CUSTOMER_NAME, SHIP_LOCATION_CODE
				, FREIGHT_TERMS_NAME, DELIVERY_STATUS_CODE, DELIVERY_STATUS_NAME, TRANSACTION_BY, TRANSACTION_USER_NAME
				, TRANSACTION_DATE, AUTHORIZE_BY, AUTHORIZE_USER_NAME, AUTHORIZE_DATE, NOTE
				, CREATED_BY, CREATED_USER_NAME, CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_USER_NAME
				, LAST_UPDATE_DATE
			)
			SELECT 
				o.ORG_ID, MIN(o.ORG_NAME) AS ORG_NAME, o.ORGANIZATION_ID, MIN(o.ORGANIZATION_CODE) AS ORGANIZATION_CODE, o.SUBINVENTORY_CODE 
				, MIN(o.TRIP_CAR) AS TRIP_CAR, o.TRIP_ID, MIN(TRIP_NAME) AS TRIP_NAME, MIN(TRIP_ACTUAL_SHIP_DATE) AS TRIP_ACTUAL_SHIP_DATE, DELIVERY_ID
				, MIN(DELIVERY_NAME) AS DELIVERY_NAME, MIN(t.CATALOG_ELEM_VAL_070) AS ITEM_CATEGORY, MIN(o.CUSTOMER_ID) AS CUSTOMER_ID, MIN(o.CUSTOMER_NUMBER) AS CUSTOMER_NUMBER, MIN(o.CUSTOMER_NAME) AS CUSTOMER_NAME
				, MIN(o.CUSTOMER_LOCATION_CODE) AS CUSTOMER_LOCATION_CODE, MIN(o.SHIP_CUSTOMER_ID) AS SHIP_CUSTOMER_ID, MIN(o.SHIP_CUSTOMER_NUMBER) AS SHIP_CUSTOMER_NUMBER, MIN(SHIP_CUSTOMER_NAME) AS SHIP_CUSTOMER_NAME, MIN(o.SHIP_LOCATION_CODE) AS SHIP_LOCATION_CODE
				, MIN(FREIGHT_TERMS_NAME) AS FREIGHT_TERMS_NAME, 'DH1' AS DELIVERY_STATUS_CODE, '未印' AS DELIVERY_STATUS_NAME, NULL AS TRANSACTION_BY, NULL AS TRANSACTION_USER_NAME
				, NULL AS TRANSACTION_DATE, NULL AS AUTHORIZE_BY, NULL AS AUTHORIZE_USER_NAME, NULL AS AUTHORIZE_DATE
				, ( SELECT  NOTE + '' FROM @orgTable  d 
					WHERE d.PROCESS_CODE = o.PROCESS_CODE AND d.SERVER_CODE = o.SERVER_CODE AND d.BATCH_ID = o.BATCH_ID AND d.TRIP_ID = o.TRIP_ID AND d.DELIVERY_ID = o.DELIVERY_ID
					GROUP BY d.PROCESS_CODE, d.SERVER_CODE, d.BATCH_ID, d.TRIP_ID, d.DELIVERY_ID, d.NOTE 
					FOR XML PATH('')) AS NOTE
				, @user AS CREATED_BY, @user CREATE_USER_NAME, MIN(o.CREATION_DATE) AS CREATION_DATE, NULL AS LAST_UPDATED_BY, NULL AS LAST_UPDATE_USER_NAME
				, NULL AS LAST_UPDATE_DATE
			FROM DLV_ORG_T o 
			JOIN ITEMS_T t ON t.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			WHERE o.DLV_ORG_ID IN (SELECT DLV_ORG_ID FROM @table)
			GROUP BY PROCESS_CODE, SERVER_CODE, BATCH_ID, TRIP_ID, DELIVERY_ID, ORG_ID, ORGANIZATION_ID, SUBINVENTORY_CODE

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨檔頭失敗'
				RAISERROR(@message, 16, @success)
			END

			--明細
			SET @success = @success + 1
			INSERT INTO DLV_DETAIL_T (
				DLV_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				, Order_Number, ORDER_LINE_ID, ORDER_SHIP_NUMBER, DELIVERY_DETAIL_ID
				, PACKING_TYPE, INVENTORY_ITEM_ID, ITEM_NUMBER, ITEM_DESCRIPTION, REAM_WEIGHT
				, ITEM_CATEGORY, PAPER_TYPE, BASIC_WEIGHT, SPECIFICATION, GRAIN_DIRECTION
				, LOCATOR_ID, LOCATOR_CODE, REQUESTED_TRANSACTION_QUANTITY, REQUESTED_TRANSACTION_UOM, REQUESTED_PRIMARY_QUANTITY
				, REQUESTED_PRIMARY_UOM, REQUESTED_SECONDARY_QUANTITY, REQUESTED_SECONDARY_UOM, OSP_BATCH_ID, OSP_BATCH_NO
				, OSP_BATCH_TYPE, TMP_ITEM_ID, TMP_ITEM_NUMBER, TMP_ITEM_DESCRIPTION, CREATED_BY
				, CREATED_USER_NAME, CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_USER_NAME, LAST_UPDATE_DATE
			)
			SELECT
				h.DLV_HEADER_ID, o.PROCESS_CODE, o.SERVER_CODE, o.BATCH_ID, o.BATCH_LINE_ID
				, o.Order_Number, o.ORDER_LINE_ID, o.ORDER_SHIP_NUMBER, o.DELIVERY_DETAIL_ID
				, o.PACKING_TYPE, o.INVENTORY_ITEM_ID, o.ITEM_NUMBER, o.ITEM_DESCRIPTION, i.CATALOG_ELEM_VAL_060
				, i.CATALOG_ELEM_VAL_070, o.PAPER_TYPE, o.BASIC_WEIGHT, o.SPECIFICATION, o.GRAIN_DIRECTION
				, o.LOCATOR_ID, o.LOCATOR_CODE, o.SRC_REQUESTED_QUANTITY, o.SRC_REQUESTED_QUANTITY_UOM, o.REQUESTED_QUANTITY
				, o.REQUESTED_QUANTITY_UOM, o.REQUESTED_QUANTITY2, o.REQUESTED_QUANTITY_UOM2, p.PE_BATCH_ID, p.BATCH_NO
				, p.BATCH_TYPE, t.INVENTORY_ITEM_ID, t.ITEM_NUMBER, t.ITEM_DESC_TCH, @user AS CREATED_BY 
				, @user CREATE_USER_NAME, h.CREATION_DATE, NULL AS LAST_UPDATED_BY , NULL AS LAST_UPDATE_USER_NAME, NULL AS LAST_UPDATE_DATE
			FROM DLV_ORG_T o 
			JOIN DLV_HEADER_T h ON h.DELIVERY_ID = o.DELIVERY_ID
			LEFT JOIN OSP_HEADER_T p ON p.BATCH_NO = o.BATCH_NO
			LEFT JOIN ITEMS_T i ON i.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			LEFT JOIN ITEMS_T t ON t.ITEM_NUMBER = o.INVENTORY_ITEM_NUMBER
			WHERE o.DLV_ORG_ID IN (SELECT DLV_ORG_ID FROM @table)

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨明細失敗'
				RAISERROR(@message, 16, @success)
			END
		END

		--處理完成回寫 XXIF_CHP_P217_CONTAINER_ST 
		SET @success = @success + 1
		UPDATE A SET STATUS_CODE = B.STATUS_CODE, ERROR_MSG = B.ERROR_MSG
		FROM XXIF_CHP_P220_DELIVERY_ST A 
		JOIN @orgTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID AND B.BATCH_LINE_ID = A.BATCH_LINE_ID

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 XXIF_CHP_P220_DELIVERY_ST錯誤'
			RAISERROR(@message, 16, @success)
		END

		--處理完成寫 XXIF_CHP_CONTROL_ST
		SET @success = @success + 1
		UPDATE A SET STATUS_CODE = B.STATUS_CODE, ERROR_MSG = B.ERROR_MSG, SOA_PULLING_FLAG = 'OutAck-W'
			FROM XXIF_CHP_CONTROL_ST A
			JOIN @controlStage B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
		
		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 XXIF_CHP_CONTROL_ST錯誤'
			RAISERROR(@message, 16, @success)
		END
		
		SET @code = 0
		SET @message = ''
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()

		UPDATE XXIF_CHP_CONTROL_ST SET STATUS_CODE = 'E', ERROR_MSG = @message, SOA_PULLING_FLAG = 'OutAck-W'
		WHERE PROCESS_CODE =@processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
	END CATCH

END

