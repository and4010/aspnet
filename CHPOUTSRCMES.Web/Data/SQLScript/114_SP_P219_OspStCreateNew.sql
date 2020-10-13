-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/1
-- Description:	SOA OSP_BATCH_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P219_OspStCreateNew]

	@processCode NVARCHAR(20),
	@serverCode NVARCHAR(20),
	@batchId NVARCHAR(20),
	@peBatchId BIGINT,
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
		PE_BATCH_ID BIGINT,
		OSP_REMARK NVARCHAR(240),
		STATUS_CODE NVARCHAR(1),
		ERROR_MSG NVARCHAR(2000)
	)

	DECLARE @table TABLE (
		OSP_ORG_ID BIGINT
	)

	DECLARE @headerTable TABLE (
		OSP_HEADER_ID BIGINT,
		SRC_BATCH_NO NVARCHAR(32)
	)

	DECLARE @detailInTable TABLE (
		OSP_HEADER_ID BIGINT,
		SUBINVENTORY NVARCHAR(3)
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
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, PE_BATCH_ID, OSP_REMARK, 
			STATUS_CODE, ERROR_MSG)
		SELECT 
			c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID, c.PE_BATCH_ID, ISNULL(c.OSP_REMARK, ''),
			CASE WHEN o.OSP_ORG_ID IS NULL --未轉檔
				AND (r.ORGANIZATION_ID IS NOT NULL AND r.CONTROL_FLAG <> 'D') --組織存在
				AND ( (c.LINE_TYPE = 'I' AND s.SUBINVENTORY_CODE IS NOT NULL AND s.CONTROL_FLAG <> 'D') OR (c.LINE_TYPE = 'P') ) --組成成份必須有倉別
				AND (t.INVENTORY_ITEM_ID IS NOT NULL AND t.CONTROL_FLAG <> 'D') --商品存在
				AND ((p.PE_BATCH_ID IS NULL AND c.BATCH_STATUS = 2) OR (p.PE_BATCH_ID IS NOT NULL AND c.BATCH_STATUS <> 2)) --未有工單號
				THEN 'S' 
				ELSE 'E' END AS STATUS_CODE,
			CASE WHEN c.BATCH_STATUS = 2 AND  p.PE_BATCH_ID IS NOT NULL THEN N'加工單資料已存在，無法新增!! ' ELSE '' END +
			CASE WHEN c.BATCH_STATUS <> 2 AND p.PE_BATCH_ID IS NULL THEN N'加工單資料不存在，無法異動或取消!! ' ELSE '' END +
			CASE WHEN r.ORGANIZATION_ID IS NULL THEN N'組織不存在!! ' WHEN r.CONTROL_FLAG = 'D' THEN N'組織已刪除' ELSE '' END +
			CASE WHEN c.LINE_TYPE = 'P' THEN '' WHEN c.LINE_TYPE = 'I' AND s.SUBINVENTORY_CODE IS NULL THEN N'倉庫不存在!! ' WHEN s.CONTROL_FLAG = 'D' THEN N'倉庫已刪除' ELSE '' END +
			CASE WHEN t.INVENTORY_ITEM_ID IS NULL THEN N'料號資料不存在!! ' WHEN t.CONTROL_FLAG = 'D' THEN N'料號已刪除' ELSE '' END
			AS ERROR_MSG
		  FROM [XXIF_CHP_CONTROL_ST] ctl
		  JOIN @controlStage cs ON cs.PROCESS_CODE = ctl.PROCESS_CODE AND cs.SERVER_CODE = ctl.SERVER_CODE AND cs.BATCH_ID = ctl.BATCH_ID
		  JOIN XXIF_CHP_P219_OSP_BATCH_ST c ON c.PROCESS_CODE = ctl.PROCESS_CODE AND c.SERVER_CODE = ctl.SERVER_CODE AND c.BATCH_ID = ctl.BATCH_ID
		  LEFT JOIN OSP_ORG_T o ON o.PROCESS_CODE = c.PROCESS_CODE AND o.SERVER_CODE = c.SERVER_CODE AND o.BATCH_ID = c.BATCH_ID AND o.BATCH_LINE_ID = c.BATCH_LINE_ID
		  LEFT JOIN ITEMS_T t ON t.INVENTORY_ITEM_ID = c.INVENTORY_ITEM_ID 
		  LEFT JOIN ORGANIZATION_T r ON r.ORGANIZATION_ID = c.ORGANIZATION_ID
		  LEFT JOIN SUBINVENTORY_T s ON s.SUBINVENTORY_CODE = c.SUBINVENTORY
		  LEFT JOIN LOCATOR_T l ON l.LOCATOR_ID = c.LOCATOR_ID AND l.SUBINVENTORY_CODE = c.SUBINVENTORY 
		  LEFT JOIN OSP_HEADER_T p ON p.PE_BATCH_ID = c.PE_BATCH_ID
		  WHERE ctl.PROCESS_CODE = @processCode 
		  AND ctl.SERVER_CODE = @serverCode
		  AND ctl.BATCH_ID = @batchId 
		  AND c.PE_BATCH_ID = @peBatchId

		IF NOT EXISTS(SELECT TOP 1 * FROM @orgTable)
		BEGIN
			SET @message = @message + ' 無資料'
			RAISERROR(@message, 16, @success)
		END

		--統計錯誤訊息
		SET @success = @success + 1
		UPDATE C SET STATUS_CODE = D.STATUS_CODE, ERROR_MSG = CASE WHEN RTRIM(D.ERROR_MSG) = '' THEN NULL ELSE SUBSTRING(RTRIM(D.ERROR_MSG),0, 500) END
			FROM @controlStage C
			JOIN 
			(
			SELECT 
				A.PROCESS_CODE
				, A.SERVER_CODE
				, A.BATCH_ID
				, 'E' AS STATUS_CODE
				, ( SELECT  ERROR_MSG + '' FROM @orgTable  d 
					WHERE d.PROCESS_CODE = A.PROCESS_CODE AND d.SERVER_CODE = A.SERVER_CODE AND d.BATCH_ID = A.BATCH_ID
					GROUP BY d.PROCESS_CODE, d.SERVER_CODE, d.BATCH_ID, d.ERROR_MSG 
					FOR XML PATH('')) AS ERROR_MSG
			FROM @controlStage A
			JOIN @orgTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
			WHERE B.STATUS_CODE = 'E'
			GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
			) D ON C.PROCESS_CODE = D.PROCESS_CODE AND C.SERVER_CODE = D.SERVER_CODE AND C.BATCH_ID = D.BATCH_ID

		SET @success = 10
		--資料正常 寫入進櫃主檔、檔頭及明細
		IF NOT EXISTS (SELECT TOP 1 * FROM @controlStage WHERE STATUS_CODE = 'E')
		BEGIN -- 檢查正常，寫入主檔
			
			--主檔
			INSERT INTO OSP_ORG_T (
				PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				, PE_BATCH_ID, BATCH_NO, BATCH_TYPE, BATCH_STATUS, BATCH_STATUS_DESC 
				, ORG_ID, ORG_NAME, ORGANIZATION_ID, ORGANIZATION_CODE, DUE_DATE
				, PLAN_START_DATE, PLAN_CMPLT_DATE, PE_CREATED_BY, PE_CREATION_DATE, PE_LAST_UPDATE_BY
				, PE_LAST_UPDATE_DATE, LINE_TYPE, LINE_NO, INVENTORY_ITEM_ID, INVENTORY_ITEM_NUMBER
				, BASIC_WEIGHT, SPECIFICATION, GRAIN_DIRECTION, ORDER_WEIGHT, REAM_WT
				, PACKING_TYPE, PLAN_QTY, WIP_PLAN_QTY, DTL_UOM, PAPER_TYPE
				, ORDER_HEADER_ID, ORDER_NUMBER, ORDER_LINE_ID, ORDER_LINE_NUMBER, CUSTOMER_ID
				, CUSTOMER_NUMBER, CUSTOMER_NAME, PR_NUMBER, PR_LINE_NUMBER, REQUISITION_LINE_ID
				, PO_NUMBER, PO_LINE_NUMBER, PO_LINE_ID, PO_UNIT_PRICE, PO_REVISION_NUM
				, PO_STATUS, PO_VENDOR_NUM, OSP_REMARK, SUBINVENTORY, LOCATOR_ID
				, LOCATOR_CODE, RESERVATION_UOM_CODE, RESERVATION_QUANTITY, LINE_CREATED_BY
				, LINE_CREATION_DATE, LINE_LAST_UPDATE_BY, LINE_LAST_UPDATE_DATE, TRANSACTION_QUANTITY, TRANSACTION_UOM
				, PRIMARY_QUANTITY, PRIMARY_UOM, SECONDARY_QUANTITY, SECONDARY_UOM, ATTRIBUTE1
				, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6
				, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11
				, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15, CREATED_BY
				, CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			OUTPUT inserted.OSP_ORG_ID INTO @table (OSP_ORG_ID)
			SELECT
				c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID
				, c.PE_BATCH_ID, c.BATCH_NO, c.BATCH_TYPE, c.BATCH_STATUS, c.BATCH_STATUS_DESC 
				, c.ORG_ID, c.ORG_NAME, c.ORGANIZATION_ID, c.ORGANIZATION_CODE, c.DUE_DATE
				, c.PLAN_START_DATE, c.PLAN_CMPLT_DATE, c.PE_CREATED_BY, c.PE_CREATION_DATE, c.PE_LAST_UPDATED_BY
				, c.PE_LAST_UPDATE_DATE, c.LINE_TYPE, c.LINE_NO, c.INVENTORY_ITEM_ID, c.INVENTORY_ITEM_NUMBER
				, i.CATALOG_ELEM_VAL_040, i.CATALOG_ELEM_VAL_050, i.CATALOG_ELEM_VAL_100, i.CATALOG_ELEM_VAL_060, c.REAM_WT
				, i.CATALOG_ELEM_VAL_110, c.PLAN_QTY, c.WIP_PLAN_QTY, c.DTL_UOM, i.CATALOG_ELEM_VAL_020
				, c.HEADER_ID, c.ORDER_NUMBER, c.LINE_ID, c.LINE_NUMBER, c.CUSTOMER_ID
				, c.CUSTOMER_NUMBER, ISNULL(c.CUSTOMER_NAME, '中華紙漿'), PR_NUMBER, c.PR_LINE_NUMBER, c.REQUISITION_LINE_ID
				, c.PO_NUMBER, c.PO_LINE_NUMBER, c.PO_LINE_ID, c.PO_UNIT_PRICE, c.PO_REVISION_NUM
				, c.PO_STATUS, c.PO_VENDOR_NUM, ISNULL(c.OSP_REMARK, ''), c.SUBINVENTORY, c.LOCATOR_ID
				, c.LOCATOR_CODE, c.RESERVATION_UOM_CODE, c.RESERVATION_QUANTITY, c.LINE_CREATED_BY, c.LINE_CREATION_DATE
				, c.LINE_LAST_UPDATED_BY, c.LINE_LAST_UPDATE_DATE, c.TRANSACTION_QUANTITY, c.TRANSACTION_UOM, c.PRIMARY_QUANTITY
				, c.PRIMARY_UOM, c.SECONDARY_QUANTITY, c.SECONDARY_UOM, c.ATTRIBUTE1, c.ATTRIBUTE2
				, c.ATTRIBUTE3, c.ATTRIBUTE4, c.ATTRIBUTE5, c.ATTRIBUTE6, c.ATTRIBUTE7
				, c.ATTRIBUTE8, c.ATTRIBUTE9, c.ATTRIBUTE10, c.ATTRIBUTE11, c.ATTRIBUTE12
				, c.ATTRIBUTE13, c.ATTRIBUTE14, c.ATTRIBUTE15, c.CREATED_BY, c.CREATION_DATE
				, c.LAST_UPDATED_BY, c.LAST_UPDATE_DATE
			FROM XXIF_CHP_P219_OSP_BATCH_ST c
			JOIN @orgTable d ON d.PROCESS_CODE = c.PROCESS_CODE AND d.SERVER_CODE = c.SERVER_CODE AND d.BATCH_ID = c.BATCH_ID AND d.BATCH_LINE_ID = c.BATCH_LINE_ID
			JOIN @controlStage s ON s.PROCESS_CODE = d.PROCESS_CODE AND s.SERVER_CODE = d.SERVER_CODE AND s.BATCH_ID = d.BATCH_ID
			JOIN ITEMS_T i ON i.INVENTORY_ITEM_ID = c.INVENTORY_ITEM_ID
			WHERE s.STATUS_CODE = 'S'

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨主檔失敗'
				RAISERROR(@message, 16, @success)
			END

			--檔頭
			SET @success = @success + 1
			INSERT INTO OSP_HEADER_T (
				PE_BATCH_ID, BATCH_NO, BATCH_TYPE, BATCH_STATUS, BATCH_STATUS_DESC
				, ORG_ID, ORG_NAME, ORGANIZATION_ID, ORGANIZATION_CODE, DUE_DATE
				, PLAN_START_DATE, PLAN_CMPLT_DATE, PE_CREATED_BY, PE_CREATION_DATE, PE_LAST_UPDATE_BY
				, PE_LAST_UPDATE_DATE, STATUS, CUTTING_DATE_FROM, CUTTING_DATE_TO, MACHINE_CODE
				, SRC_OSP_HEADER_ID, SRC_BATCH_NO
				, NOTE, MODIFICATIONS, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY
				, LAST_UPDATE_DATE
			)
			OUTPUT inserted.OSP_HEADER_ID, inserted.SRC_BATCH_NO INTO @headerTable (OSP_HEADER_ID, SRC_BATCH_NO)
			SELECT 
				o.PE_BATCH_ID, o.BATCH_NO
				, CASE WHEN MIN(o.BATCH_TYPE) <> 'BTH' THEN MIN(o.BATCH_TYPE) WHEN MIN(o.BATCH_TYPE) = 'BTH' AND SUBSTRING(MIN(o.BATCH_NO), 1, 3) = 'REP' THEN 'REP' ELSE 'OSP' END AS BATCH_TYPE
				, MIN(o.BATCH_STATUS) AS BATCH_STATUS, MIN(o.BATCH_STATUS_DESC) AS BATCH_STATUS_DESC
				, MIN(o.ORG_ID) AS ORG_ID, MIN(o.ORG_NAME) AS ORG_NAME, MIN(o.ORGANIZATION_ID) AS ORGANIZATION_ID, MIN(o.ORGANIZATION_CODE) AS ORGANIZATION_CODE, MIN(o.DUE_DATE) AS DUE_DATE
				, MIN(o.PLAN_START_DATE) AS PLAN_START_DATE, MIN(o.PLAN_CMPLT_DATE) AS PLAN_CMPLT_DATE, MIN(o.PE_CREATED_BY) AS PE_CREATED_BY, MIN(o.PE_CREATION_DATE) AS PE_CREATION_DATE, MIN(PE_LAST_UPDATE_BY) AS PE_LAST_UPDATE_BY
				, MIN(o.PE_LAST_UPDATE_DATE) AS PE_LAST_UPDATE_DATE, '0' AS [STATUS], NULL AS CUTTING_DATE_FROM, NULL AS CUTTING_DATE_TO, MIN(m.MACHINE_NUM) AS MACHINE_CODE
				, NULL AS SRC_OSP_HEADER_ID, MIN(o.ATTRIBUTE6) AS SRC_BATCH_NO
				, '' AS NOTE, 0 AS MODIFICATIONS, 'SYS' AS CREATED_BY, MIN(o.CREATION_DATE) AS CREATION_DATE, NULL AS LAST_UPDATED_BY
				, NULL AS LAST_UPDATE_DATE
			FROM OSP_ORG_T o 
			LEFT JOIN ITEMS_T t ON t.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			LEFT JOIN ORG_ITEMS_T g ON g.INVENTORY_ITEM_ID = t.INVENTORY_ITEM_ID AND g.ORGANIZATION_ID = o.ORGANIZATION_ID
			LEFT JOIN MACHINE_PAPER_TYPE_T m ON m.PAPER_TYPE = t.CATALOG_ELEM_VAL_020 AND m.ORGANIZATION_ID = g.ORGANIZATION_ID
			WHERE o.OSP_ORG_ID IN (SELECT OSP_ORG_ID FROM @table)
			GROUP BY PROCESS_CODE, SERVER_CODE, BATCH_ID, PE_BATCH_ID, BATCH_NO

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨檔頭失敗'
				RAISERROR(@message, 16, @success)
			END
			
			UPDATE OSP_HEADER_T SET SRC_OSP_HEADER_ID = (SELECT TOP 1 OSP_HEADER_ID FROM OSP_HEADER_T AS SRC_OSP_HEADER_ID WHERE BATCH_NO = h.SRC_BATCH_NO)
			FROM OSP_HEADER_T h 
			JOIN @headerTable t ON t.OSP_HEADER_ID = h.OSP_HEADER_ID
			WHERE h.SRC_BATCH_NO IS NOT NULL
			IF (@@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入代紙工單檔頭失敗'
				RAISERROR(@message, 16, @success)
			END

			--組成料號明細
			SET @success = @success + 1
			INSERT INTO OSP_DETAIL_IN_T (
				OSP_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				, LINE_TYPE, LINE_NO, INVENTORY_ITEM_ID, INVENTORY_ITEM_NUMBER, BASIC_WEIGHT
				, SPECIFICATION, GRAIN_DIRECTION, ORDER_WEIGHT, REAM_WT, PAPER_TYPE
				, PACKING_TYPE, PLAN_QTY, WIP_PLAN_QTY, DTL_UOM, ORDER_HEADER_ID
				, ORDER_NUMBER, ORDER_LINE_ID, ORDER_LINE_NUMBER, CUSTOMER_ID, CUSTOMER_NUMBER
				, CUSTOMER_NAME, PR_NUMBER, PR_LINE_NUMBER, REQUISITION_LINE_ID, PO_NUMBER
				, PO_LINE_NUMBER, PO_LINE_ID, PO_UNIT_PRICE, PO_REVISION_NUM, PO_STATUS
				, PO_VENDOR_NUM, OSP_REMARK, SUBINVENTORY, LOCATOR_ID, LOCATOR_CODE
				, RESERVATION_UOM_CODE, RESERVATION_QUANTITY, LINE_CREATED_BY, LINE_CREATION_DATE, LINE_LAST_UPDATE_BY
				, LINE_LAST_UPDATE_DATE, TRANSACTION_QUANTITY, TRANSACTION_UOM, PRIMARY_QUANTITY, PRIMARY_UOM
				, SECONDARY_QUANTITY, SECONDARY_UOM, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3
				, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8
				, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13
				, ATTRIBUTE14, ATTRIBUTE15, REQUEST_ID, CREATED_BY, CREATION_DATE
				, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			OUTPUT inserted.OSP_HEADER_ID, inserted.SUBINVENTORY INTO @detailInTable (OSP_HEADER_ID, SUBINVENTORY)
			SELECT
				h.OSP_HEADER_ID, o.PROCESS_CODE, o.SERVER_CODE, o.BATCH_ID, o.BATCH_LINE_ID
				, o.LINE_TYPE, o.LINE_NO, o.INVENTORY_ITEM_ID, o.INVENTORY_ITEM_NUMBER, o.BASIC_WEIGHT
				, o.SPECIFICATION, o.GRAIN_DIRECTION, o.ORDER_WEIGHT, o.REAM_WT, o.PAPER_TYPE
				, o.PACKING_TYPE, o.PLAN_QTY, o.WIP_PLAN_QTY, o.DTL_UOM, o.ORDER_HEADER_ID
				, o.ORDER_NUMBER, o.ORDER_LINE_ID, o.ORDER_LINE_NUMBER, o.CUSTOMER_ID, o.CUSTOMER_NUMBER
				, o.CUSTOMER_NAME, o.PR_NUMBER, o.PR_LINE_NUMBER, o.REQUISITION_LINE_ID, o.PO_NUMBER
				, o.PO_LINE_NUMBER, o.PO_LINE_ID, o.PO_UNIT_PRICE, o.PO_REVISION_NUM, o.PO_STATUS
				, o.PO_VENDOR_NUM, o.OSP_REMARK, o.SUBINVENTORY, o.LOCATOR_ID, o.LOCATOR_CODE
				, RESERVATION_UOM_CODE, RESERVATION_QUANTITY, LINE_CREATED_BY, LINE_CREATION_DATE, LINE_LAST_UPDATE_BY
				, o.LINE_LAST_UPDATE_DATE, o.TRANSACTION_QUANTITY, o.TRANSACTION_UOM, o.PRIMARY_QUANTITY, o.PRIMARY_UOM
				, o.SECONDARY_QUANTITY, o.SECONDARY_UOM, o.ATTRIBUTE1, o.ATTRIBUTE2, o.ATTRIBUTE3
				, o.ATTRIBUTE4, o.ATTRIBUTE5, o.ATTRIBUTE6, o.ATTRIBUTE7, o.ATTRIBUTE8
				, o.ATTRIBUTE9, o.ATTRIBUTE10, o.ATTRIBUTE11, o.ATTRIBUTE12, o.ATTRIBUTE13
				, o.ATTRIBUTE14, o.ATTRIBUTE15, o.REQUEST_ID, o.CREATED_BY, o.CREATION_DATE
				, o.LAST_UPDATE_BY, o.LAST_UPDATE_DATE
			FROM OSP_ORG_T o 
			JOIN OSP_HEADER_T h ON h.PE_BATCH_ID = o.PE_BATCH_ID
			LEFT JOIN ITEMS_T i ON i.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			WHERE o.OSP_ORG_ID IN (SELECT OSP_ORG_ID FROM @table) AND o.LINE_TYPE = 'I'

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨明細失敗'
				RAISERROR(@message, 16, @success)
			END

			--產品料號明細
			SET @success = @success + 1
			INSERT INTO OSP_DETAIL_OUT_T (
				OSP_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				, LINE_TYPE, LINE_NO, INVENTORY_ITEM_ID, INVENTORY_ITEM_NUMBER, BASIC_WEIGHT
				, SPECIFICATION, GRAIN_DIRECTION, ORDER_WEIGHT, REAM_WT, PAPER_TYPE
				, PACKING_TYPE, PLAN_QTY, WIP_PLAN_QTY, DTL_UOM, ORDER_HEADER_ID
				, ORDER_NUMBER, ORDER_LINE_ID, ORDER_LINE_NUMBER, CUSTOMER_ID, CUSTOMER_NUMBER
				, CUSTOMER_NAME, PR_NUMBER, PR_LINE_NUMBER, REQUISITION_LINE_ID, PO_NUMBER
				, PO_LINE_NUMBER, PO_LINE_ID, PO_UNIT_PRICE, PO_REVISION_NUM, PO_STATUS
				, PO_VENDOR_NUM, OSP_REMARK, SUBINVENTORY, LOCATOR_ID, LOCATOR_CODE
				, RESERVATION_UOM_CODE, RESERVATION_QUANTITY, LINE_CREATED_BY, LINE_CREATION_DATE, LINE_LAST_UPDATE_BY
				, LINE_LAST_UPDATE_DATE, TRANSACTION_QUANTITY, TRANSACTION_UOM, PRIMARY_QUANTITY, PRIMARY_UOM
				, SECONDARY_QUANTITY, SECONDARY_UOM, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3
				, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8
				, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13
				, ATTRIBUTE14, ATTRIBUTE15, REQUEST_ID, CREATED_BY, CREATION_DATE
				, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			SELECT
				h.OSP_HEADER_ID, o.PROCESS_CODE, o.SERVER_CODE, o.BATCH_ID, o.BATCH_LINE_ID
				, o.LINE_TYPE, o.LINE_NO, o.INVENTORY_ITEM_ID, o.INVENTORY_ITEM_NUMBER, o.BASIC_WEIGHT
				, o.SPECIFICATION, o.GRAIN_DIRECTION, o.ORDER_WEIGHT, o.REAM_WT, o.PAPER_TYPE
				, o.PACKING_TYPE, o.PLAN_QTY, o.WIP_PLAN_QTY, o.DTL_UOM, o.ORDER_HEADER_ID
				, o.ORDER_NUMBER, o.ORDER_LINE_ID, o.ORDER_LINE_NUMBER, o.CUSTOMER_ID, o.CUSTOMER_NUMBER
				, o.CUSTOMER_NAME, o.PR_NUMBER, o.PR_LINE_NUMBER, o.REQUISITION_LINE_ID, o.PO_NUMBER
				, o.PO_LINE_NUMBER, o.PO_LINE_ID, o.PO_UNIT_PRICE, o.PO_REVISION_NUM, o.PO_STATUS
				, o.PO_VENDOR_NUM, o.OSP_REMARK, d.SUBINVENTORY, o.LOCATOR_ID, o.LOCATOR_CODE
				, RESERVATION_UOM_CODE, RESERVATION_QUANTITY, LINE_CREATED_BY, LINE_CREATION_DATE, LINE_LAST_UPDATE_BY
				, o.LINE_LAST_UPDATE_DATE, o.TRANSACTION_QUANTITY, o.TRANSACTION_UOM, o.PRIMARY_QUANTITY, o.PRIMARY_UOM
				, o.SECONDARY_QUANTITY, o.SECONDARY_UOM, o.ATTRIBUTE1, o.ATTRIBUTE2, o.ATTRIBUTE3
				, o.ATTRIBUTE4, o.ATTRIBUTE5, o.ATTRIBUTE6, o.ATTRIBUTE7, o.ATTRIBUTE8
				, o.ATTRIBUTE9, o.ATTRIBUTE10, o.ATTRIBUTE11, o.ATTRIBUTE12, o.ATTRIBUTE13
				, o.ATTRIBUTE14, o.ATTRIBUTE15, o.REQUEST_ID, o.CREATED_BY, o.CREATION_DATE
				, o.LAST_UPDATE_BY, o.LAST_UPDATE_DATE
			FROM OSP_ORG_T o 
			JOIN OSP_HEADER_T h ON h.PE_BATCH_ID = o.PE_BATCH_ID
			LEFT JOIN @detailInTable d ON d.OSP_HEADER_ID = h.OSP_HEADER_ID
			LEFT JOIN ITEMS_T i ON i.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			WHERE o.OSP_ORG_ID IN (SELECT OSP_ORG_ID FROM @table) AND o.LINE_TYPE = 'P'

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入出貨明細失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		
		--處理完成回寫 XXIF_CHP_P219_OSP_BATCH_ST 
		SET @success = @success + 1
		UPDATE A SET STATUS_CODE = B.STATUS_CODE, ERROR_MSG = B.ERROR_MSG
		FROM XXIF_CHP_P219_OSP_BATCH_ST A 
		JOIN @orgTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID AND B.BATCH_LINE_ID = A.BATCH_LINE_ID

		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 XXIF_CHP_P219_OSP_BATCH_ST 錯誤'
			RAISERROR(@message, 16, @success)
		END
		
		SET @code = 0
		SET @message = ''
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()

		UPDATE XXIF_CHP_P219_OSP_BATCH_ST SET STATUS_CODE = 'E', ERROR_MSG = @message
			WHERE PROCESS_CODE =@processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId AND PE_BATCH_ID = @peBatchId

	END CATCH

END

