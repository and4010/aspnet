-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/15
-- Description:	產生不重複的條碼清單
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenerateDocNum]
	-- Add the parameters for the stored procedure here
	@subinventory VARCHAR(3),
	@dstSubinventory VARCHAR(3),
	@createDate DATETIME,
	@digit INT,
	@docNo VARCHAR(50) OUTPUT,
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @maxValue INT =  POWER(10, @digit) -1
	DECLARE @docPrefix VARCHAR(30) =  '' 
	DECLARE @sequence INT = 0
	SET @docPrefix = '(' + @subinventory + '-' + @dstSubinventory + ')' + CONVERT(VARCHAR, @createDate, 112)

	DECLARE @step INT = 1
	
	BEGIN TRY

		--取得目前系統單號最大值
		SELECT @sequence = MAX(ISNULL(DOC_SEQ,0)) FROM DOC_UNIQUE_T WHERE DOC_PREFIX = @docPrefix GROUP BY DOC_PREFIX 
		IF (@maxValue <= @sequence)
		BEGIN
			RAISERROR('產生單號失敗，流水號已超出限制長度!!', 16, @step)
		END

		SET @step = @step + 1
		--寫入單號記錄表
		SET @sequence = @sequence + 1
		SET @docNo = @docPrefix + '-' + CAST(@sequence AS VARCHAR) 
		INSERT INTO DOC_UNIQUE_T (DOC_NO, DOC_PREFIX, DOC_SEQ, CREATED_BY, CREATION_DATE)
		VALUES (@docNo, @docPrefix, @sequence, @user, GETDATE())

		IF(@@ROWCOUNT <= 0 OR @@ERROR <> 0)
		BEGIN
			RAISERROR('產生單號失敗，寫入單號記錄表失敗!!', 16, @step)
		END

		

		SET @code = 0
		SET @message = ''
		
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @step
		SET @message = CAST(@step AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
		SET @docNo = ''
	END CATCH

END

