﻿/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
-- =============================================
-- Author:		Eric
-- Create date: 2020/08/19
-- Description:	加工產出檢貨Pick
-- =============================================
CREATE PROCEDURE [dbo].[SP_SaveOspDetailOut]
	-- Add the parameters for the stored procedure here
	@headerId BIGINT,
	@statusCode VARCHAR(10),
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128),
	@locatorId BIGINT,
	@locatorCode NVARCHAR(163)
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
	odo.INVENTORY_ITEM_ID,odo.INVENTORY_ITEM_NUMBER,ii.ITEM_DESC_TCH,ii.CATALOG_ELEM_VAL_070,pot.PAPER_TYPE,
	odo.BASIC_WEIGHT,'',pot.SECONDARY_QUANTITY,pot.SPECIFICATION,odo.PACKING_TYPE,
	oht.BATCH_NO,pot.LOT_NUMBER,pot.BARCODE,pot.PRIMARY_UOM, pot.PRIMARY_QUANTITY,
	pot.PRIMARY_QUANTITY,0,pot.SECONDARY_UOM, pot.SECONDARY_QUANTITY, pot.SECONDARY_QUANTITY,
	0,'','','',@statusCode,
	pot.CREATED_BY,pot.CREATION_DATE,pot.LAST_UPDATE_BY,pot.LAST_UPDATE_DATE
	FROM OSP_PICKED_OUT_T pot
	join OSP_DETAIL_OUT_T odo on odo.OSP_DETAIL_OUT_ID = pot.OSP_DETAIL_OUT_ID
	join OSP_HEADER_T oht on oht.OSP_HEADER_ID = pot.OSP_HEADER_ID
	join ITEMS_T ii on ii.INVENTORY_ITEM_ID = pot.INVENTORY_ITEM_ID
	where oht.OSP_HEADER_ID = @headerId

	IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
	BEGIN
		RAISERROR('!Stock新增錯誤!', 16, @step)
	END

	SET @step = @step + 1
	UPDATE OSP_PICKED_OUT_T set STOCK_ID = a.STOCK_ID
	FROM @table a
	where 
	OSP_PICKED_OUT_T.BARCODE = a.BARCODE 
	AND OSP_HEADER_ID = @headerId

	IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
	BEGIN
		RAISERROR('!PickOut，StockId更新錯誤!', 16, @step)
	END
	
END TRY
BEGIN CATCH
	SET @code = -1 * @step
	SET @message = CAST(@step AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
END CATCH
	
END