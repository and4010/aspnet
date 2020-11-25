-- =============================================
-- Author:		Pon
-- Create date: 2020/11/05
-- Description: 加工得率報表明細
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE SP_OspYieldReport
	@cuttingDateFrom VARCHAR(30),
	@cuttingDateTo VARCHAR(30),
	@batchNo VARCHAR(32),
	@machineNum VARCHAR(30),
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
set @cmd = 'SELECT h.BATCH_NO AS BatchNo,
dbo.GetOspStatusNameByCode(h.STATUS) AS Status,
iht.INVENTORY_ITEM_NUMBER AS DiItemNumber,
FORMAT(ROUND(ISNULL(yht.DETAIL_IN_QUANTITY, 0) / 1000, 5),''0.##########'') AS DiQty,
oht.INVENTORY_ITEM_NUMBER AS DoItemNumber,
FORMAT((SELECT ISNULL(SUM(SECONDARY_QUANTITY), 0) FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID),''0.##########'') AS DoReQty,
FORMAT(ROUND(ISNULL(yht.DETAIL_OUT_QUANTITY, 0) /1000, 5),''0.##########'') AS DoQty,
FORMAT(ROUND(yht.RATE, 2),''0.##########'') AS Yield
FROM OSP_HEADER_T h
JOIN OSP_YIELD_VARIANCE_HT yht on h.OSP_HEADER_ID = yht.OSP_HEADER_ID
JOIN OSP_DETAIL_IN_HT iht on h.OSP_HEADER_ID = iht.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_HT oht on h.OSP_HEADER_ID = oht.OSP_HEADER_ID
JOIN USER_SUBINVENTORY_T us ON us.ORGANIZATION_ID = h.ORGANIZATION_ID AND us.SUBINVENTORY_CODE = iht.SUBINVENTORY AND us.SUBINVENTORY_CODE = oht.SUBINVENTORY
WHERE 1=1'

SET @success = @success + 1
IF (@user IS NOT NULL) AND (LEN(@user) > 0)
	set @cmd = @cmd + 'AND us.UserId =''' + @user + ''''

SET @success = @success + 1
IF (@dateFormStatus = 1) OR (@dateToStatus = 1)
	set @cmd = @cmd + 'AND ((h.CUTTING_DATE_FROM BETWEEN ''' + @cuttingDateFrom + '''' + ' AND ''' + @cuttingDateTo + '''' + ') OR (h.CUTTING_DATE_TO BETWEEN ''' + @cuttingDateFrom + '''' + ' AND ''' + @cuttingDateTo + '''' + '))'

SET @success = @success + 1
IF (@batchNo IS NOT NULL AND LEN(@batchNo) > 0)
	set @cmd = @cmd + 'AND h.BATCH_NO LIKE ''' + @batchNo + '%' + ''''

SET @success = @success + 1
IF (@machineNum <> '*')
	set @cmd = @cmd + 'AND h.MACHINE_CODE =''' + @machineNum + ''''

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

