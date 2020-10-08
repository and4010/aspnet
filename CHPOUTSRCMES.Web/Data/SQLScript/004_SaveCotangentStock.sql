﻿/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
CREATE PROCEDURE [dbo].[SP_SaveCotangentStock]
	-- Add the parameters for the stored procedure here
	@headerId BIGINT,
	@statusCode VARCHAR(10),
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128),
	@locatorId BIGINT,
	@locatorCode NVARCHAR(163),
	@CATEGORY VARCHAR(30),
	@ACTION VARCHAR(50),
	@DOC VARCHAR(50),
	@CREATED_BY varchar(128)
AS
BEGIN

DECLARE @table TABLE (
	STOCK_ID BIGINT,
	BARCODE NVARCHAR(40)
)
BEGIN TRY
	DECLARE @step INT = 1

	UPDATE OSP_DETAIL_OUT_T SET LOCATOR_ID = nullif(@locatorId, 0), LOCATOR_CODE = @locatorCode
	WHERE OSP_HEADER_ID = @headerId

	SET @step = @step + 1

INSERT INTO [STOCK_T] 
([ORGANIZATION_ID],[ORGANIZATION_CODE],[SUBINVENTORY_CODE],[LOCATOR_ID],[LOCATOR_SEGMENTS],
[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION],[ITEM_CATEGORY] ,[PAPER_TYPE],
[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_WT],[SPECIFICATION],[PACKING_TYPE],
[OSP_BATCH_NO],[LOT_NUMBER],[BARCODE],[PRIMARY_UOM_CODE],[PRIMARY_TRANSACTION_QTY],
[PRIMARY_AVAILABLE_QTY],[PRIMARY_LOCKED_QTY],[SECONDARY_UOM_CODE],[SECONDARY_TRANSACTION_QTY],[SECONDARY_AVAILABLE_QTY],
[SECONDARY_LOCKED_QTY] ,[REASON_CODE] ,[REASON_DESC],[NOTE],[STATUS_CODE],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
OUTPUT inserted.STOCK_ID, inserted.BARCODE INTO @table (STOCK_ID, BARCODE)
SELECT oht.ORGANIZATION_ID,oht.ORGANIZATION_CODE,odo.SUBINVENTORY,odo.LOCATOR_ID,odo.LOCATOR_CODE,
odo.INVENTORY_ITEM_ID,odo.INVENTORY_ITEM_NUMBER,ii.ITEM_DESC_TCH,ii.CATALOG_ELEM_VAL_070,oc.PAPER_TYPE,
odo.BASIC_WEIGHT,'',oc.SECONDARY_QUANTITY,oc.SPECIFICATION,odo.PACKING_TYPE,
oht.BATCH_NO,oc.LOT_NUMBER,oc.BARCODE,oc.PRIMARY_UOM, oc.PRIMARY_QUANTITY,
oc.PRIMARY_QUANTITY,0,oc.SECONDARY_UOM, oc.SECONDARY_QUANTITY, oc.SECONDARY_QUANTITY,
0,'','','',@statusCode,
oc.CREATED_BY,oc.CREATION_DATE,oc.LAST_UPDATE_BY,oc.LAST_UPDATE_DATE
FROM OSP_COTANGENT_T oc
join OSP_DETAIL_OUT_T odo on odo.OSP_DETAIL_OUT_ID = oc.OSP_DETAIL_OUT_ID
join OSP_HEADER_T oht on oht.OSP_HEADER_ID = oc.OSP_HEADER_ID
join ITEMS_T ii on ii.INVENTORY_ITEM_ID = oc.INVENTORY_ITEM_ID
where oht.OSP_HEADER_ID = @headerId

	IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
	BEGIN
		RAISERROR('!Cotangent儲存stock新增錯誤!', 16, @step)
	END

	SET @step = @step + 1

	INSERT INTO [dbo].[STK_TXN_T]
        ([STOCK_ID],[ORGANIZATION_ID] ,[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
        ,[LOCATOR_ID],[DST_ORGANIZATION_ID]  ,[DST_ORGANIZATION_CODE],[DST_SUBINVENTORY_CODE] ,[DST_LOCATOR_ID]
        ,[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,[ITEM_CATEGORY] ,[LOT_NUMBER]
        ,[BARCODE],[PRY_UOM_CODE] ,[PRY_BEF_QTY],[PRY_AFT_QTY],[PRY_CHG_QTY]
        ,[SEC_UOM_CODE] ,[SEC_BEF_QTY],[SEC_CHG_QTY],[SEC_AFT_QTY],[CATEGORY]
        ,[DOC],[ACTION],[NOTE],[STATUS_CODE],[CREATED_BY]
        ,[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT T.[STOCK_ID],T.[ORGANIZATION_ID] ,T.[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
        ,T.[LOCATOR_ID],''  ,'','' ,''
        ,T.[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,T.[ITEM_CATEGORY] ,T.[LOT_NUMBER]
        ,T.[BARCODE],PRIMARY_UOM_CODE ,0,PRIMARY_TRANSACTION_QTY,PRIMARY_TRANSACTION_QTY
        ,SECONDARY_UOM_CODE ,0,SECONDARY_TRANSACTION_QTY,SECONDARY_TRANSACTION_QTY,@CATEGORY
        ,@DOC,@ACTION,T.[NOTE],[STATUS_CODE],@CREATED_BY
        ,GETDATE(),NULL,NULL
FROM STOCK_T T
JOIN @table a ON a.BARCODE = T.BARCODE
where T.STOCK_ID = a.STOCK_ID

IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
BEGIN
	RAISERROR('異動紀錄錯誤!!', 16, @step)
END

	SET @step = @step + 1
	UPDATE OSP_COTANGENT_T set STOCK_ID = a.STOCK_ID
	FROM @table a
	where 
	OSP_COTANGENT_T.BARCODE = a.BARCODE 
	AND OSP_HEADER_ID = @headerId

	IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
	BEGIN
		RAISERROR('!Cotangent，StockId更新錯誤!', 16, @step)
	END
	
END TRY
BEGIN CATCH
	SET @code = -1 * @step
	SET @message = CAST(@step AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
END CATCH
	
END