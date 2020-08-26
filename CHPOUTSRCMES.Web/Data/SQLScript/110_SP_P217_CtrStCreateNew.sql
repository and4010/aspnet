-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/08/21
-- Description:	SOA CONTAINER_ST 資料接收
-- =============================================
CREATE PROCEDURE [dbo].[SP_P217_CtrStCreateNew]
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

	DECLARE @detailTable TABLE (
		PROCESS_CODE NVARCHAR(20),
		SERVER_CODE NVARCHAR(20),
		BATCH_ID NVARCHAR(20),
		BATCH_LINE_ID BIGINT,
		STATUS_CODE NVARCHAR(1),
		ERROR_MSG NVARCHAR(2000)
	)

	DECLARE @table TABLE (
		CTR_ORG_ID BIGINT
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

		 --取出進櫃明細，檢查資料是否正確?
		SET @success = @success + 1
		INSERT INTO @detailTable (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID,
			STATUS_CODE, ERROR_MSG)
		SELECT 
			c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID, 
			CASE WHEN c.ATTRIBUTE1 = 'N' 
				AND o.CTR_ORG_ID IS NULL 
				AND (r.ORGANIZATION_ID IS NOT NULL AND r.CONTROL_FLAG <> 'D')
				AND (s.SUBINVENTORY_CODE IS NOT NULL AND s.CONTROL_FLAG <> 'D')
				AND (t.INVENTORY_ITEM_ID IS NOT NULL AND t.CONTROL_FLAG <> 'D')
				AND ((s.OSP_FLAG <> 'Y') OR (s.OSP_FLAG = 'Y' AND l.LOCATOR_ID IS NOT NULL AND l.CONTROL_FLAG <> 'D')) 
				THEN 'S' 
				ELSE 'E' END AS STATUS_CODE,
			CASE WHEN o.CTR_ORG_ID IS NOT NULL THEN N'櫃號資料已存在!! ' ELSE '' END +
			CASE WHEN r.ORGANIZATION_ID IS NULL THEN N'組織不存在!! ' WHEN r.CONTROL_FLAG = 'D' THEN N'組織已刪除' ELSE '' END +
			CASE WHEN s.SUBINVENTORY_CODE IS NULL THEN N'倉庫不存在!! ' WHEN s.CONTROL_FLAG = 'D' THEN N'倉庫已刪除' ELSE '' END +
			CASE WHEN l.LOCATOR_ID IS NULL AND s.OSP_FLAG = 'Y' THEN N'儲位不存在!!' WHEN l.CONTROL_FLAG = 'D' THEN N'儲位已刪除' ELSE '' END + 
			CASE WHEN t.INVENTORY_ITEM_ID IS NULL THEN N'料號資料不存在!! ' WHEN t.CONTROL_FLAG = 'D' THEN N'料號已刪除' ELSE '' END
			AS ERROR_MSG
		  FROM [XXIF_CHP_CONTROL_ST] ctl
		  JOIN @controlStage cs ON cs.PROCESS_CODE = ctl.PROCESS_CODE AND cs.SERVER_CODE = ctl.SERVER_CODE AND cs.BATCH_ID = ctl.BATCH_ID
		  JOIN [XXIF_CHP_P217_CONTAINER_ST] c ON c.PROCESS_CODE = ctl.PROCESS_CODE AND c.SERVER_CODE = ctl.SERVER_CODE AND c.BATCH_ID = ctl.BATCH_ID
		  LEFT JOIN CTR_ORG_T o ON c.HEADER_ID = o.HEADER_ID AND c.LINE_ID = o.LINE_ID AND c.DETAIL_ID = o.DETAIL_ID 
		  LEFT JOIN ITEMS_T t ON t.INVENTORY_ITEM_ID = c.INVENTORY_ITEM_ID 
		  LEFT JOIN ORGANIZATION_T r ON r.ORGANIZATION_ID = c.ORGANIZATION_ID
		  LEFT JOIN SUBINVENTORY_T s ON s.SUBINVENTORY_CODE = c.SUBINVENTORY 
		  LEFT JOIN LOCATOR_T l ON l.LOCATOR_ID = c.LOCATOR_ID AND l.SUBINVENTORY_CODE = c.SUBINVENTORY 
		  WHERE ctl.PROCESS_CODE = @processCode 
		  AND ctl.SERVER_CODE = @serverCode
		  AND ctl.BATCH_ID = @batchId
		  AND c.ATTRIBUTE1 = 'N' 

		IF NOT EXISTS(SELECT TOP 1 * FROM @detailTable)
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
				, ( SELECT  ERROR_MSG + '' FROM @detailTable  d 
					WHERE d.PROCESS_CODE = A.PROCESS_CODE AND d.SERVER_CODE = A.SERVER_CODE AND d.BATCH_ID = A.BATCH_ID 
					ORDER BY BATCH_LINE_ID FOR XML PATH('')) AS ERROR_MSG
			FROM @controlStage A
			JOIN @detailTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID
			WHERE B.STATUS_CODE = 'E'
			GROUP BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID
			) D ON C.PROCESS_CODE = D.PROCESS_CODE AND C.SERVER_CODE = D.SERVER_CODE AND C.BATCH_ID = D.BATCH_ID

		SET @success = 10
		--資料正常 寫入進櫃主檔、檔頭及明細
		IF NOT EXISTS (SELECT TOP 1 * FROM @controlStage WHERE STATUS_CODE = 'E')
		BEGIN -- 檢查正常，寫入主檔
			
			--主檔
			INSERT INTO CTR_ORG_T (
				PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, HEADER_ID
				, ORG_ID, ORG_NAME, BL_NO, LINE_ID, CONTAINER_NO
				, MV_CONTAINER_DATE, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY, LOCATOR_ID
				, LOCATOR_CODE, DETAIL_ID, INVENTORY_ITEM_ID, SHIP_ITEM_NUMBER, PAPER_TYPE
				, BASIC_WEIGHT, REAM_WEIGHT, ROLL_REAM_QTY, ROLL_REAM_WT, TTL_ROLL_REAM
				, SPECIFICATION, PACKING_TYPE, SHIP_MT_QTY, TRANSACTION_QUANTITY, TRANSACTION_UOM
				, PRIMARY_QUANTITY, PRIMARY_UOM, SECONDARY_QUANTITY, SECONDARY_UOM, ATTRIBUTE1
				, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6
				, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11
				, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15, REQUEST_ID
				, CREATED_BY, CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_DATE
			)
			OUTPUT inserted.CTR_ORG_ID INTO @table (CTR_ORG_ID)
			SELECT
				c.PROCESS_CODE, c.SERVER_CODE, c.BATCH_ID, c.BATCH_LINE_ID, c.HEADER_ID
				,c.ORG_ID, c.ORG_NAME, c.BL_NO, c.LINE_ID, c.CONTAINER_NO
				,c.MV_CONTAINER_DATE, c.ORGANIZATION_ID, c.ORGANIZATION_CODE, c.SUBINVENTORY, c.LOCATOR_ID
				,c.LOCATOR_CODE, c.DETAIL_ID, c.INVENTORY_ITEM_ID, c.SHIP_ITEM_NUMBER, c.PAPER_TYPE
				,c.BASIC_WEIGHT, c.REAM_WEIGHT, c.ROLL_REAM_QTY, c.ROLL_REAM_WT, c.TTL_ROLL_REAM
				,c.SPECIFICATION, c.PACKING_TYPE, c.SHIP_MT_QTY, c.TRANSACTION_QUANTITY, c.TRANSACTION_UOM
				,c.PRIMARY_QUANTITY, c.PRIMARY_UOM, c.SECONDARY_QUANTITY,  c.SECONDARY_UOM, c.ATTRIBUTE1
				,c.ATTRIBUTE2, c.ATTRIBUTE3,  c.ATTRIBUTE4,  c.ATTRIBUTE5,  c.ATTRIBUTE6
				,c.ATTRIBUTE7, c.ATTRIBUTE8, c.ATTRIBUTE9, c.ATTRIBUTE10, c.ATTRIBUTE11
				,c.ATTRIBUTE12, c.ATTRIBUTE13, c.ATTRIBUTE14, c.ATTRIBUTE15, c.REQUEST_ID
				,c.CREATED_BY, c.CREATION_DATE, c.LAST_UPDATED_BY, c.LAST_UPDATE_DATE
			FROM [XXIF_CHP_P217_CONTAINER_ST] c
			JOIN @detailTable d ON d.PROCESS_CODE = c.PROCESS_CODE AND d.SERVER_CODE = c.SERVER_CODE AND d.BATCH_ID = c.BATCH_ID AND d.BATCH_LINE_ID = c.BATCH_LINE_ID
			JOIN @controlStage s ON s.PROCESS_CODE = d.PROCESS_CODE AND s.SERVER_CODE = d.SERVER_CODE AND s.BATCH_ID = d.BATCH_ID
			WHERE s.STATUS_CODE = 'S'

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入進櫃主檔失敗'
				RAISERROR(@message, 16, @success)
			END

			--檔頭
			SET @success = @success + 1
			INSERT INTO CTR_HEADER_T (
				HEADER_ID, ORG_ID, ORG_NAME, BL_NO, LINE_ID, CONTAINER_NO
				,MV_CONTAINER_DATE, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY, [STATUS]
				,CREATED_BY, CREATED_USER_NAME, CREATION_DATE, LAST_UPDATE_USER_NAME, LAST_UPDATE_BY
				,LAST_UPDATE_DATE
			)
			SELECT 
				HEADER_ID, ORG_ID, MIN(ORG_NAME) AS ORG_NAME, MIN(BL_NO) AS BL_NO, LINE_ID, CONTAINER_NO 
				, MV_CONTAINER_DATE, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY, 1 
				,'SYS' AS CREATED_BY, 'SYS' CREATE_USER_NAME, MIN(CREATION_DATE) AS CREATION_DATE
				, NULL AS LAST_UPDATE_USER_NAME, NULL AS LAST_UPDATED_BY 
				, NULL AS LAST_UPDATE_DATE
			FROM CTR_ORG_T o 
			WHERE o.CTR_ORG_ID IN (SELECT CTR_ORG_ID FROM @table)
			GROUP BY HEADER_ID, ORG_ID, LINE_ID, CONTAINER_NO, MV_CONTAINER_DATE, ORGANIZATION_ID, ORGANIZATION_CODE, SUBINVENTORY

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入進櫃檔頭失敗'
				RAISERROR(@message, 16, @success)
			END

			--明細
			SET @success = @success + 1
			INSERT INTO CTR_DETAIL_T (
				CTR_HEADER_ID, PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID
				,HEADER_ID ,LINE_ID, DETAIL_ID, LOCATOR_ID, LOCATOR_CODE
				,INVENTORY_ITEM_ID, SHIP_ITEM_NUMBER, PAPER_TYPE, BASIC_WEIGHT, REAM_WEIGHT
				,ROLL_REAM_QTY, ROLL_REAM_WT, TTL_ROLL_REAM, SPECIFICATION, PACKING_TYPE
				,SHIP_MT_QTY, TRANSACTION_QUANTITY, TRANSACTION_UOM, PRIMARY_QUANTITY, PRIMARY_UOM
				,SECONDARY_QUANTITY, SECONDARY_UOM, ITEM_CATEGORY, ATTRIBUTE1, ATTRIBUTE2
				,ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7
				,ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12
				,ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15, CREATED_BY, CREATED_USER_NAME
				,CREATION_DATE, LAST_UPDATE_BY, LAST_UPDATE_USER_NAME, LAST_UPDATE_DATE
			)
			SELECT
				h.CTR_HEADER_ID, o.PROCESS_CODE, o.SERVER_CODE, o.BATCH_ID, o.BATCH_LINE_ID
				,o.HEADER_ID, o.LINE_ID, DETAIL_ID, LOCATOR_ID, LOCATOR_CODE 
				,o.INVENTORY_ITEM_ID, SHIP_ITEM_NUMBER, PAPER_TYPE, BASIC_WEIGHT, REAM_WEIGHT
				,ROLL_REAM_QTY, ROLL_REAM_WT, TTL_ROLL_REAM, SPECIFICATION, PACKING_TYPE
				,SHIP_MT_QTY, TRANSACTION_QUANTITY, TRANSACTION_UOM, PRIMARY_QUANTITY, PRIMARY_UOM
				,SECONDARY_QUANTITY, SECONDARY_UOM, ISNULL(i.CATALOG_ELEM_VAL_070, '') AS ITEM_CATEGORY, ATTRIBUTE1, ATTRIBUTE2
				,ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5, ATTRIBUTE6, ATTRIBUTE7
				,ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10, ATTRIBUTE11, ATTRIBUTE12
				,ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15, 'SYS' AS CREATED_BY, 'SYS' CREATE_USER_NAME
				,h.CREATION_DATE, NULL AS LAST_UPDATED_BY , NULL AS LAST_UPDATE_USER_NAME, NULL AS LAST_UPDATE_DATE
			FROM CTR_ORG_T o 
			JOIN CTR_HEADER_T h ON h.CONTAINER_NO = o.CONTAINER_NO
			LEFT JOIN ITEMS_T i ON i.INVENTORY_ITEM_ID = o.INVENTORY_ITEM_ID
			WHERE o.CTR_ORG_ID IN (SELECT CTR_ORG_ID FROM @table)

			IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' + @batchId + ' 寫入進櫃明細失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		
		--處理完成回寫 XXIF_CHP_P217_CONTAINER_ST 
		SET @success = @success + 1
		UPDATE A SET STATUS_CODE = B.STATUS_CODE, ERROR_MSG = B.ERROR_MSG
		FROM XXIF_CHP_P217_CONTAINER_ST A 
		JOIN @detailTable B ON B.PROCESS_CODE = A.PROCESS_CODE AND B.SERVER_CODE = A.SERVER_CODE AND B.BATCH_ID = A.BATCH_ID AND B.BATCH_LINE_ID = A.BATCH_LINE_ID

		IF(@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 XXIF_CHP_P217_CONTAINER_ST錯誤'
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

