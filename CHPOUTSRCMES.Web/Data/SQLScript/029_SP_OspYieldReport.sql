/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
-- =============================================
-- Author:		Pon
-- Create date: 2020/12/08
-- Description:	工單得率報表
-- =============================================
CREATE PROCEDURE [dbo].[SP_OspYieldReport]
	@cuttingDateFrom VARCHAR(30),
	@cuttingDateTo VARCHAR(30),
	@batchNo VARCHAR(32),
	@machineNum VARCHAR(30),
	@itemNumber VARCHAR(40),
	@barcode VARCHAR(20),
	@subinventory VARCHAR(3),
	@dateFormStatus VARCHAR(1),
	@dateToStatus VARCHAR(1),
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @cmd nvarchar(Max)
	DECLARE @success INT = 0
BEGIN TRY
SET @success = @success + 1
set @cmd = 'SELECT 
iht.SUBINVENTORY AS Subinventory,
h.BATCH_NO AS BatchNo,
ISNULL(h.MACHINE_CODE,'''') AS MachineNum,
h.CUTTING_DATE_TO AS CuttingDateTo,
dbo.GetOspStatusNameByCode(h.STATUS) AS Status,

yht.DETAIL_IN_ITEM_NUMBER AS DiItemNumber,
ROUND(ISNULL(yht.DETAIL_IN_QUANTITY, 0), 2) AS DiPriQty,
FORMAT(ISNULL(yht.DETAIL_IN_SECONDARY_QUANTITY, 0),''0.##########'') AS DiSecQty,
yht.DETAIL_IN_PRIMARY_UOM AS DiPriUom,
yht.DETAIL_IN_SECONDARY_UOM AS DiSecUom,

yht.DETAIL_OUT_ITEM_NUMBER AS DoItemNumber,
ROUND(ISNULL(yht.DETAIL_OUT_PRIMARY_QUANTITY, 0), 2) AS DoPriQty,
FORMAT(ISNULL(yht.DETAIL_OUT_SECONDARY_QUANTITY, 0),''0.##########'') AS DoSecQty,
yht.DETAIL_OUT_PRIMARY_UOM AS DoPriUom,
yht.DETAIL_OUT_SECONDARY_UOM AS DoSecUom,

yht.COTANGENT_ITEM_NUMBER AS CotItemNumber,
ROUND(ISNULL(yht.COTANGENT_QUANTITY, 0), 2) AS CotPriQty,
FORMAT(ISNULL(yht.COTANGENT_SECONDARY_QUANTITY, 0),''0.##########'') AS CotSecQty,
yht.COTANGENT_PRIMARY_UOM AS CotPriUom,
yht.COTANGENT_SECONDARY_UOM AS CotSecUom,

yht.RATE AS Yield,
oht.CUSTOMER_NAME AS CustName,
oht.ORDER_NUMBER AS OrderNumber
FROM OSP_HEADER_T h
JOIN OSP_YIELD_VARIANCE_HT yht on h.OSP_HEADER_ID = yht.OSP_HEADER_ID
JOIN OSP_DETAIL_IN_HT iht on h.OSP_HEADER_ID = iht.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_HT oht on h.OSP_HEADER_ID = oht.OSP_HEADER_ID
JOIN USER_SUBINVENTORY_T us ON us.ORGANIZATION_ID = h.ORGANIZATION_ID AND us.SUBINVENTORY_CODE = iht.SUBINVENTORY AND us.SUBINVENTORY_CODE = oht.SUBINVENTORY
WHERE 1=1 AND (h.Status = ''3'' OR h.Status = ''4'') '

SET @success = @success + 1
IF (@user IS NOT NULL) AND (LEN(@user) > 0)
	set @cmd = @cmd + ' AND us.UserId =''' + @user + ''''

SET @success = @success + 1
IF (@dateFormStatus = 1) OR (@dateToStatus = 1)
	set @cmd = @cmd + ' AND ((h.CUTTING_DATE_FROM BETWEEN ''' + @cuttingDateFrom + '''' + ' AND ''' + @cuttingDateTo + '''' + ') OR (h.CUTTING_DATE_TO BETWEEN ''' + @cuttingDateFrom + '''' + ' AND ''' + @cuttingDateTo + '''' + '))'

SET @success = @success + 1
IF (@batchNo IS NOT NULL AND LEN(@batchNo) > 0)
	set @cmd = @cmd + ' AND h.BATCH_NO LIKE ''' + @batchNo + '%' + ''''

SET @success = @success + 1
IF (@machineNum <> '*')
	set @cmd = @cmd + ' AND h.MACHINE_CODE =''' + @machineNum + ''''

SET @success = @success + 1
IF (@itemNumber IS NOT NULL) AND (LEN(@itemNumber) > 0)
	set @cmd = @cmd + ' AND yht.DETAIL_IN_ITEM_NUMBER =''' + @itemNumber + ''''

SET @success = @success + 1
IF (@barcode IS NOT NULL) AND (LEN(@barcode) > 0)
	set @cmd = @cmd + ' AND ( oht.OSP_DETAIL_OUT_ID IN (SELECT OSP_DETAIL_OUT_ID FROM OSP_COTANGENT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID AND BARCODE =''' + @barcode + ''')' 
	+ 'OR oht.OSP_DETAIL_OUT_ID IN (SELECT OSP_DETAIL_OUT_ID FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID AND BARCODE =''' +  @barcode + ''')' 
	+ 'OR iht.OSP_DETAIL_IN_ID IN (SELECT OSP_DETAIL_IN_ID FROM OSP_PICKED_IN_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID AND BARCODE =''' +  @barcode + '''))'

SET @success = @success + 1
IF (@subinventory <> '*')
	set @cmd = @cmd + ' AND iht.SUBINVENTORY =''' + @subinventory + ''''

SET @code = 0
SET @message = ''

PRINT @cmd

EXECUTE (@cmd)

RETURN

	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
	END CATCH

END

