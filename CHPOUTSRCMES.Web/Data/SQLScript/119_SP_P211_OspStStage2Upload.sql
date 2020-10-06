-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/16
-- Description:	SOA XXIF_CHP_P211_IN_MMT_PROD_ST 資料上傳
-- 未解決.. RXD  捲筒理論重
-- =============================================
CREATE PROCEDURE [dbo].[SP_P211_OspStStage2Upload]
	@ospHeaderId BIGINT,
	@processCode VARCHAR(20) OUTPUT,
	@serverCode VARCHAR(20) OUTPUT,
	@batchId VARCHAR(20) OUTPUT,
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN

	DECLARE @soa TABLE (
		PROCESS_CODE VARCHAR(20)
		, SERVER_CODE VARCHAR(20)
		, BATCH_ID VARCHAR(20)
		, OSP_SOA_S2_ID BIGINT
	)

	DECLARE @orgTable TABLE (
		[PROCESS_CODE] [nvarchar](20) NOT NULL,
		[SERVER_CODE] [nvarchar](20) NOT NULL,
		[BATCH_ID] [nvarchar](20) NOT NULL,
		[BATCH_LINE_ID] [bigint] NOT NULL,
		[STATUS_CODE] [nvarchar](1) NULL,
		[ERROR_MSG] [nvarchar](2000) NULL,
		[ORGCODE] [nvarchar](3) NOT NULL,
		[RXID] [bigint] NOT NULL,
		[PREVIOUS_RXID] [bigint] NULL,
		[BATCH_NO] [nvarchar](32) NOT NULL,
		[MACHINE_NO] [nvarchar](5) NULL,
		[ITEM_NO] [nvarchar](40) NOT NULL,
		[SUBINVENTORY_CODE] [nvarchar](10) NOT NULL,
		[LOCATOR] [nvarchar](50) NULL,
		[TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
		[TRANSACTION_UOM] [nvarchar](3) NULL,
		[SECONDARY_TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
		[SECONDARY_UOM_CODE] [nvarchar](3) NULL,
		[TRANSACTION_DATE] [datetime] NOT NULL,
		[LOT_NUMBER] [nvarchar](80) NULL,
		[LOT_ATTRIBUTE1] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE2] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE3] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE4] [float] NULL,
		[LOT_ATTRIBUTE5] [float] NULL,
		[LOT_ATTRIBUTE6] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE7] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE8] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE9] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE10] [nvarchar](20) NULL,
		[ORDER_NUMBER] [bigint] NULL,
		[LINE_NUMBER] [nvarchar](20) NULL,
		[STATUS] [nvarchar](1) NOT NULL,
		[RESERVATION_ID] [bigint] NULL,
		[TRANSACTION_ID] [bigint] NULL,
		[ATTRIBUTE1] [nvarchar](150) NULL,
		[ATTRIBUTE2] [nvarchar](150) NULL,
		[ATTRIBUTE3] [nvarchar](150) NULL,
		[ATTRIBUTE4] [nvarchar](150) NULL,
		[ATTRIBUTE5] [nvarchar](150) NULL,
		[ATTRIBUTE6] [nvarchar](150) NULL,
		[ATTRIBUTE7] [nvarchar](150) NULL,
		[ATTRIBUTE8] [nvarchar](150) NULL,
		[ATTRIBUTE9] [nvarchar](150) NULL,
		[ATTRIBUTE10] [nvarchar](150) NULL,
		[ATTRIBUTE11] [nvarchar](150) NULL,
		[ATTRIBUTE12] [nvarchar](150) NULL,
		[ATTRIBUTE13] [nvarchar](150) NULL,
		[ATTRIBUTE14] [nvarchar](150) NULL,
		[ATTRIBUTE15] [nvarchar](150) NULL,
		[REQUEST_ID] [bigint] NULL,
		[CREATED_BY] [bigint] NULL,
		[CREATION_DATE] [datetime] NULL,
		[LAST_UPDATED_BY] [bigint] NULL,
		[LAST_UPDATE_DATE] [datetime] NULL,
		[LAST_UPDATE_LOGIN] [bigint] NULL
	)

	DECLARE @dstTable TABLE (
		[PROCESS_CODE] [nvarchar](20) NOT NULL,
		[SERVER_CODE] [nvarchar](20) NOT NULL,
		[BATCH_ID] [nvarchar](20) NOT NULL,
		[BATCH_LINE_ID] [bigint] NOT NULL,
		[STATUS_CODE] [nvarchar](1) NULL,
		[ERROR_MSG] [nvarchar](2000) NULL,
		[ORGCODE] [nvarchar](3) NOT NULL,
		[RXID] [bigint] NOT NULL,
		[PREVIOUS_RXID] [bigint] NULL,
		[BATCH_NO] [nvarchar](32) NOT NULL,
		[MACHINE_NO] [nvarchar](5) NULL,
		[ITEM_NO] [nvarchar](40) NOT NULL,
		[SUBINVENTORY_CODE] [nvarchar](10) NOT NULL,
		[LOCATOR] [nvarchar](50) NULL,
		[TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
		[TRANSACTION_UOM] [nvarchar](3) NULL,
		[SECONDARY_TRANSACTION_QUANTITY] [decimal](30, 10) NULL,
		[SECONDARY_UOM_CODE] [nvarchar](3) NULL,
		[TRANSACTION_DATE] [datetime] NOT NULL,
		[LOT_NUMBER] [nvarchar](80) NULL,
		[LOT_ATTRIBUTE1] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE2] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE3] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE4] [float] NULL,
		[LOT_ATTRIBUTE5] [float] NULL,
		[LOT_ATTRIBUTE6] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE7] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE8] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE9] [nvarchar](20) NULL,
		[LOT_ATTRIBUTE10] [nvarchar](20) NULL,
		[ORDER_NUMBER] [bigint] NULL,
		[LINE_NUMBER] [nvarchar](20) NULL,
		[STATUS] [nvarchar](1) NOT NULL,
		[RESERVATION_ID] [bigint] NULL,
		[TRANSACTION_ID] [bigint] NULL,
		[ATTRIBUTE1] [nvarchar](150) NULL,
		[ATTRIBUTE2] [nvarchar](150) NULL,
		[ATTRIBUTE3] [nvarchar](150) NULL,
		[ATTRIBUTE4] [nvarchar](150) NULL,
		[ATTRIBUTE5] [nvarchar](150) NULL,
		[ATTRIBUTE6] [nvarchar](150) NULL,
		[ATTRIBUTE7] [nvarchar](150) NULL,
		[ATTRIBUTE8] [nvarchar](150) NULL,
		[ATTRIBUTE9] [nvarchar](150) NULL,
		[ATTRIBUTE10] [nvarchar](150) NULL,
		[ATTRIBUTE11] [nvarchar](150) NULL,
		[ATTRIBUTE12] [nvarchar](150) NULL,
		[ATTRIBUTE13] [nvarchar](150) NULL,
		[ATTRIBUTE14] [nvarchar](150) NULL,
		[ATTRIBUTE15] [nvarchar](150) NULL,
		[REQUEST_ID] [bigint] NULL,
		[CREATED_BY] [bigint] NULL,
		[CREATION_DATE] [datetime] NULL,
		[LAST_UPDATED_BY] [bigint] NULL,
		[LAST_UPDATE_DATE] [datetime] NULL,
		[LAST_UPDATE_LOGIN] [bigint] NULL
	)

	DECLARE @success INT = 0
	DECLARE @count INT = 0
	SELECT @message = 'OSP_HEADER_ID:' + CONVERT(varchar, @ospHeaderId)

	DECLARE @txnDate DATETIME = GETDATE()
	SET @processCode = 'XXIFP211'
	SET @serverCode = 'FTY'
	SET @batchId = FORMAT(@txnDate, 'yyyyMMddHHmmssffffff')

	
	BEGIN TRY
		
		SET @success = @success + 1
		INSERT INTO @orgTable (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, ORGCODE, RXID, PREVIOUS_RXID, BATCH_NO
			, MACHINE_NO, ITEM_NO, SUBINVENTORY_CODE, LOCATOR, TRANSACTION_QUANTITY
			, TRANSACTION_UOM, SECONDARY_TRANSACTION_QUANTITY, SECONDARY_UOM_CODE, TRANSACTION_DATE, LOT_NUMBER
			, LOT_ATTRIBUTE1, LOT_ATTRIBUTE2, LOT_ATTRIBUTE3, LOT_ATTRIBUTE4, LOT_ATTRIBUTE5
			, LOT_ATTRIBUTE6, LOT_ATTRIBUTE7, LOT_ATTRIBUTE8, LOT_ATTRIBUTE9, LOT_ATTRIBUTE10
			, ORDER_NUMBER, LINE_NUMBER, [STATUS], RESERVATION_ID, TRANSACTION_ID
			, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN )
		SELECT 
			ST.PROCESS_CODE, ST.SERVER_CODE, ST.BATCH_ID, ST.BATCH_LINE_ID, NULL
			, NULL, ST.ORGCODE, 0, ST.PREVIOUS_RXID, ST.BATCH_NO
			, ST.MACHINE_NO, ST.ITEM_NO, ST.SUBINVENTORY_CODE, ST.LOCATOR, 0
			, ST.TRANSACTION_UOM, 0, ST.SECONDARY_UOM_CODE, ST.TRANSACTION_DATE, ST.LOT_NUMBER
			, ST.LOT_ATTRIBUTE1, ST.LOT_ATTRIBUTE2, ST.LOT_ATTRIBUTE3, ST.LOT_ATTRIBUTE4, ST.LOT_ATTRIBUTE5
			, ST.LOT_ATTRIBUTE6, ST.LOT_ATTRIBUTE7, ST.LOT_ATTRIBUTE8, ST.LOT_ATTRIBUTE9, ST.LOT_ATTRIBUTE10
			, ST.ORDER_NUMBER, ST.LINE_NUMBER, 'M', ST.RESERVATION_ID, ST.TRANSACTION_ID
			, ST.ATTRIBUTE1, ST.ATTRIBUTE2, ST.ATTRIBUTE3, ST.ATTRIBUTE4, ST.ATTRIBUTE5
			, ST.ATTRIBUTE6, ST.ATTRIBUTE7, ST.ATTRIBUTE8, ST.ATTRIBUTE9, ST.ATTRIBUTE10
			, ST.ATTRIBUTE11, ST.ATTRIBUTE12, ST.ATTRIBUTE13, ST.ATTRIBUTE14, ST.ATTRIBUTE15
			, ST.REQUEST_ID, ST.CREATED_BY, ST.CREATION_DATE, ST.LAST_UPDATED_BY, ST.LAST_UPDATE_DATE
			, ST.LAST_UPDATE_LOGIN
		 FROM OSP_SOA_S2_T S1 
		 JOIN OSP_HEADER_MOD_T M ON M.ORG_OSP_HEADER_ID = S1.OSP_HEADER_ID
		JOIN OSP_SOA_DTL_S2_T SD1 ON SD1.OSP_HEADER_ID = S1.OSP_HEADER_ID
		JOIN XXIF_CHP_P211_IN_MMT_PROD_ST ST ON ST.PROCESS_CODE = S1.PROCESS_CODE AND ST.SERVER_CODE = S1.SERVER_CODE AND ST.BATCH_ID = S1.BATCH_ID AND ST.BATCH_LINE_ID = SD1.BATCH_LINE_ID
		WHERE M.OSP_HEADER_ID = @ospHeaderId
		ORDER BY ST.PROCESS_CODE, ST.SERVER_CODE, ST.BATCH_ID, ST.BATCH_LINE_ID

		--取出待處理項目
		SET @success = @success + 1
		
		--INSERT INTO dbo.XXIF_CHP_P211_IN_MMT_PROD_ST ( 
		--	  PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
		--	, ERROR_MSG, ORGCODE, RXID, PREVIOUS_RXID, BATCH_NO
		--	, MACHINE_NO, ITEM_NO, SUBINVENTORY_CODE, LOCATOR, TRANSACTION_QUANTITY
		--	, TRANSACTION_UOM, SECONDARY_TRANSACTION_QUANTITY, SECONDARY_UOM_CODE, TRANSACTION_DATE, LOT_NUMBER
		--	, LOT_ATTRIBUTE1, LOT_ATTRIBUTE2, LOT_ATTRIBUTE3, LOT_ATTRIBUTE4, LOT_ATTRIBUTE5
		--	, LOT_ATTRIBUTE6, LOT_ATTRIBUTE7, LOT_ATTRIBUTE8, LOT_ATTRIBUTE9, LOT_ATTRIBUTE10
		--	, ORDER_NUMBER, LINE_NUMBER, STATUS, RESERVATION_ID, TRANSACTION_ID
		--	, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
		--	, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
		--	, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
		--	, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
		--	, LAST_UPDATE_LOGIN
		--)
		INSERT INTO @dstTable (
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, ORGCODE, RXID, PREVIOUS_RXID, BATCH_NO
			, MACHINE_NO, ITEM_NO, SUBINVENTORY_CODE, LOCATOR, TRANSACTION_QUANTITY
			, TRANSACTION_UOM, SECONDARY_TRANSACTION_QUANTITY, SECONDARY_UOM_CODE, TRANSACTION_DATE, LOT_NUMBER
			, LOT_ATTRIBUTE1, LOT_ATTRIBUTE2, LOT_ATTRIBUTE3, LOT_ATTRIBUTE4, LOT_ATTRIBUTE5
			, LOT_ATTRIBUTE6, LOT_ATTRIBUTE7, LOT_ATTRIBUTE8, LOT_ATTRIBUTE9, LOT_ATTRIBUTE10
			, ORDER_NUMBER, LINE_NUMBER, [STATUS], RESERVATION_ID, TRANSACTION_ID
			, ATTRIBUTE1
			, ATTRIBUTE2
			, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN )
		SELECT
			@processCode, @serverCode, @batchId, ROW_NUMBER() OVER (ORDER BY (SELECT 1)), NULL
			, NULL, MIN(H.ORGANIZATION_CODE), 0, NULL, MIN(BATCH_NO)
			, '', MIN(D.INVENTORY_ITEM_NUMBER), MIN(D.SUBINVENTORY), MIN(D.LOCATOR_CODE), SUM(P.PRIMARY_QUANTITY)
			, MIN(P.PRIMARY_UOM), SUM(P.SECONDARY_QUANTITY), MIN(P.SECONDARY_UOM), @txnDate, NULL
			, NULL, NULL, NULL, NULL, NULL
			, NULL, NULL, NULL, NULL, NULL
			, MIN(D.ORDER_NUMBER), MIN(D.ORDER_LINE_NUMBER), 'C', NULL, NULL
			, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN  '捲筒' THEN P.LOT_NUMBER ELSE NULL END
			, CASE MIN(I.CATALOG_ELEM_VAL_070) WHEN  '捲筒' THEN MIN(P.LOT_QUANTITY) ELSE NULL END
			, NULL, NULL, NULL
			, NULL, NULL, NULL, NULL, NULL
			, NULL, NULL, NULL, NULL, NULL
			, NULL, -1, MIN(P.CREATION_DATE), -1, ISNULL(MIN(P.LAST_UPDATE_DATE), MIN(P.CREATION_DATE) )
			, -1
		FROM OSP_HEADER_T H 
		JOIN OSP_DETAIL_OUT_HT D ON D.OSP_HEADER_ID = H.OSP_HEADER_ID
		JOIN OSP_PICKED_OUT_HT P ON P.OSP_DETAIL_OUT_ID = D.OSP_DETAIL_OUT_ID
		JOIN ITEMS_T I ON I.INVENTORY_ITEM_ID = P.INVENTORY_ITEM_ID
		WHERE H.OSP_HEADER_ID = @ospHeaderId
		GROUP BY P.LOT_NUMBER, D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, D.BATCH_LINE_ID
		ORDER BY D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, D.BATCH_LINE_ID
		
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' OSP_HEADER_ID:' + CONVERT(varchar, @ospHeaderId) + ' 上傳生產批物料耗用失敗'
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
			, @user, @txnDate, NULL, NULL
		FROM @dstTable 
		GROUP BY PROCESS_CODE, SERVER_CODE, BATCH_ID

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 OSP_SOA_T 錯誤'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO dbo.OSP_SOA_DTL_S2_T
           (OSP_HEADER_ID, OSP_SOA_S2_ID, BATCH_LINE_ID)
		SELECT
			@ospHeaderId, S.OSP_SOA_S2_ID, D.BATCH_LINE_ID
		FROM @soa S
		JOIN @dstTable D ON D.PROCESS_CODE = S.PROCESS_CODE AND D.SERVER_CODE = S.SERVER_CODE AND D.BATCH_ID = S.BATCH_ID

		UPDATE D SET RXID = SD1.OSP_SOA_S2_ID
		--SELECT SD1.OspSoaDtlS1Id, D.PROCESS_CODE, D.SERVER_CODE, D.BATCH_ID, D.BATCH_LINE_ID
		FROM @dstTable D
		JOIN @soa S ON S.PROCESS_CODE = D.PROCESS_CODE AND S.SERVER_CODE = D.SERVER_CODE AND S.BATCH_ID = D.BATCH_ID
		JOIN OSP_SOA_DTL_S2_T SD1 ON SD1.BATCH_LINE_ID = D.BATCH_LINE_ID AND SD1.OSP_SOA_S2_ID = S.OSP_SOA_S2_ID

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = @message + ' 更新 OSP_SOA_DTL_T 錯誤'
			RAISERROR(@message, 16, @success)
		END

		INSERT INTO dbo.XXIF_CHP_P211_IN_MMT_PROD_ST ( 
			  PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, ORGCODE, RXID, PREVIOUS_RXID, BATCH_NO
			, MACHINE_NO, ITEM_NO, SUBINVENTORY_CODE, LOCATOR, TRANSACTION_QUANTITY
			, TRANSACTION_UOM, SECONDARY_TRANSACTION_QUANTITY, SECONDARY_UOM_CODE, TRANSACTION_DATE, LOT_NUMBER
			, LOT_ATTRIBUTE1, LOT_ATTRIBUTE2, LOT_ATTRIBUTE3, LOT_ATTRIBUTE4, LOT_ATTRIBUTE5
			, LOT_ATTRIBUTE6, LOT_ATTRIBUTE7, LOT_ATTRIBUTE8, LOT_ATTRIBUTE9, LOT_ATTRIBUTE10
			, ORDER_NUMBER, LINE_NUMBER, STATUS, RESERVATION_ID, TRANSACTION_ID
			, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN
		) 
		SELECT 
			PROCESS_CODE, SERVER_CODE, BATCH_ID, BATCH_LINE_ID, STATUS_CODE
			, ERROR_MSG, ORGCODE, RXID, PREVIOUS_RXID, BATCH_NO
			, MACHINE_NO, ITEM_NO, SUBINVENTORY_CODE, LOCATOR, TRANSACTION_QUANTITY
			, TRANSACTION_UOM, SECONDARY_TRANSACTION_QUANTITY, SECONDARY_UOM_CODE, TRANSACTION_DATE, LOT_NUMBER
			, LOT_ATTRIBUTE1, LOT_ATTRIBUTE2, LOT_ATTRIBUTE3, LOT_ATTRIBUTE4, LOT_ATTRIBUTE5
			, LOT_ATTRIBUTE6, LOT_ATTRIBUTE7, LOT_ATTRIBUTE8, LOT_ATTRIBUTE9, LOT_ATTRIBUTE10
			, ORDER_NUMBER, LINE_NUMBER, STATUS, RESERVATION_ID, TRANSACTION_ID
			, ATTRIBUTE1, ATTRIBUTE2, ATTRIBUTE3, ATTRIBUTE4, ATTRIBUTE5
			, ATTRIBUTE6, ATTRIBUTE7, ATTRIBUTE8, ATTRIBUTE9, ATTRIBUTE10
			, ATTRIBUTE11, ATTRIBUTE12, ATTRIBUTE13, ATTRIBUTE14, ATTRIBUTE15
			, REQUEST_ID, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE
			, LAST_UPDATE_LOGIN
		FROM @dstTable
		IF (@@ROWCOUNT <= 0 AND @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' OSP_HEADER_ID:' + CONVERT(varchar, @ospHeaderId) + ' 上傳生產批完工入庫且保留失敗'
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
		FROM XXIF_CHP_P211_IN_MMT_PROD_ST C
		JOIN @dstTable T ON T.PROCESS_CODE = C.PROCESS_CODE AND T.SERVER_CODE = C.SERVER_CODE AND T.BATCH_ID = C.BATCH_ID AND T.BATCH_LINE_ID = C.BATCH_LINE_ID
		GROUP BY C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID

		IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:' 
			+ @batchId + ' OSP_HEADER_ID:' + CONVERT(varchar, @ospHeaderId) + ' 寫入 XXIF_CHP_CONTROL_ST 失敗'
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

