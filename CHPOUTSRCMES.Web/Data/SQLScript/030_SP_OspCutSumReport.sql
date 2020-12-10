/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
-- =============================================
-- Author:		Pon
-- Create date: 2020/12/09
-- Description:	裁切資料匯總報表
-- =============================================
CREATE PROCEDURE [dbo].[SP_OspCutSumReport]
	@planStartDateFrom VARCHAR(30),
	@planStartDateTo VARCHAR(30),
	@batchNo VARCHAR(32),
	@paperType VARCHAR(4),
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
h.PLAN_START_DATE AS PlanStartDate,
h.BATCH_NO AS BatchNo,
dbo.GetOspStatusNameByCode(h.STATUS) AS Status,
h.DUE_DATE AS DueDate,
h.CUTTING_DATE_FROM AS CuttingDateFrom,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.CUSTOMER_NAME 
ELSE
oht.CUSTOMER_NAME
END AS CustName,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.PAPER_TYPE 
ELSE
oht.PAPER_TYPE 
END AS PaperType,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.BASIC_WEIGHT 
ELSE
oht.BASIC_WEIGHT 
END AS BasicWeight,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.SPECIFICATION 
ELSE
oht.SPECIFICATION 
END AS Specification,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.GRAIN_DIRECTION 
ELSE
oht.GRAIN_DIRECTION 
END AS GrainDirection,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
(SELECT ISNULL(SECONDARY_QUANTITY, 0) FROM OSP_DETAIL_OUT_T WHERE OSP_HEADER_ID = h.OSP_HEADER_ID) 
ELSE
(SELECT SUM(ISNULL(SECONDARY_QUANTITY, 0)) FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID)
END AS DoReamQty,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
(SELECT ROUND(ISNULL(PRIMARY_QUANTITY, 0) / 1000, 3) FROM OSP_DETAIL_OUT_T WHERE OSP_HEADER_ID = h.OSP_HEADER_ID)
ELSE
(SELECT ROUND(SUM(ISNULL(PRIMARY_QUANTITY,0)) / 1000, 3)  FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID)
END AS DoTQty,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
0
ELSE
(SELECT ISNULL(COUNT(BARCODE),0) FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID)
END AS DoPalletQty,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.PACKING_TYPE 
ELSE 
oht.PACKING_TYPE 
END AS PackingType,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.OSP_REMARK
ELSE
oht.OSP_REMARK
END AS OspRemark,

CASE WHEN h.STATUS = ''0'' OR  h.STATUS = ''1'' OR  h.STATUS = ''2'' THEN 
ot.SUBINVENTORY
ELSE
oht.SUBINVENTORY
END AS SupplierName

FROM OSP_HEADER_T h
LEFT JOIN OSP_DETAIL_OUT_T ot on ot.OSP_HEADER_ID = h.OSP_HEADER_ID AND (h.STATUS = ''0'' or h.STATUS = ''1'' or h.STATUS = ''2'')
LEFT JOIN OSP_DETAIL_OUT_HT oht on oht.OSP_HEADER_ID = h.OSP_HEADER_ID AND (h.STATUS = ''3'' or h.STATUS = ''4'')
LEFT JOIN OSP_DETAIL_IN_T it on it.OSP_HEADER_ID = h.OSP_HEADER_ID AND (h.STATUS = ''0'' or h.STATUS = ''1'' or h.STATUS = ''2'')
LEFT JOIN OSP_DETAIL_IN_HT iht on iht.OSP_HEADER_ID = h.OSP_HEADER_ID AND (h.STATUS = ''3'' or h.STATUS = ''4'')
WHERE 1=1'

SET @success = @success + 1
IF (@user IS NOT NULL) AND (LEN(@user) > 0)
	set @cmd = @cmd + ' AND (((h.STATUS = ''0'' or h.STATUS = ''1'' or h.STATUS = ''2'') AND it.SUBINVENTORY in (SELECT SUBINVENTORY_CODE FROM USER_SUBINVENTORY_T WHERE UserId = ''' + @user + '''' + '))
OR ((h.STATUS = ''3'' or h.STATUS = ''4'') AND iht.SUBINVENTORY in (SELECT SUBINVENTORY_CODE FROM USER_SUBINVENTORY_T WHERE UserId = ''' + @user + '''' + ')))'

SET @success = @success + 1
IF (@dateFormStatus = 1) OR (@dateToStatus = 1)
	set @cmd = @cmd + ' AND (h.PLAN_START_DATE BETWEEN ''' + @planStartDateFrom + '''' + ' AND ''' + @planStartDateTo + '''' + ')'

SET @success = @success + 1
IF (@batchNo IS NOT NULL AND LEN(@batchNo) > 0)
	set @cmd = @cmd + ' AND h.BATCH_NO LIKE ''' + @batchNo + '%' + ''''

SET @success = @success + 1
IF (@paperType IS NOT NULL) AND (LEN(@paperType) > 0)
	set @cmd = @cmd + ' AND (((h.STATUS = ''0'' or h.STATUS = ''1'' or h.STATUS = ''2'') AND ot.PAPER_TYPE =''' + @paperType + '''' + ')
OR ((h.STATUS = ''3'' or h.STATUS = ''4'') AND oht.PAPER_TYPE =''' + @paperType + '''' + '))'

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

