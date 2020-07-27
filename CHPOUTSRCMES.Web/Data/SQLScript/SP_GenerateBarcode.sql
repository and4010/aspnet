-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_GenerateBarcodes
	-- Add the parameters for the stored procedure here
	@organizationId BIGINT,
	@subinventory VARCHAR(3),
	@prefix VARCHAR(10),
	@requestQty INT,
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @barcodes TABLE ( 
		BARCODE VARCHAR(20)
	)
	DECLARE @miscPrefix NVARCHAR(10) = '', @serialSize INT = 4, @maxSerial BIGINT = 9999, @startSerial INT = 1, @endSerial INT = @requestQty
	DECLARE @bcdDate VARCHAR(8) =  CONVERT(VARCHAR, GETDATE(), 12)
	DECLARE @step INT = 1
	
	BEGIN TRY

		IF NOT EXISTS (SELECT TOP 1 * FROM dbo.BCD_MISC_T WHERE ORGANIZATION_ID = @organizationId AND SUBINVENTORY_CODE = @subinventory)
		BEGIN
			RAISERROR('產生條碼失敗，倉庫並未設置前置碼!!', 16, @step)
		END

		SET @step = @step + 1
		--取得條碼設定 前置碼及最大流水號
		SELECT TOP 1 @miscPrefix = PREFIX_CODE, @maxSerial = POWER(10, SERIAL_SIZE) -1, @serialSize = SERIAL_SIZE
		FROM dbo.BCD_MISC_T WHERE ORGANIZATION_ID = @organizationId AND SUBINVENTORY_CODE = @subinventory

		--以傳入前置碼為主
		IF(@prefix IS NULL OR LEN(@prefix) = 0)
		BEGIN
			SET @prefix = @miscPrefix
		END

		SET @step = @step + 1
		--取出目前流水號
		SELECT TOP 1 @startSerial = SERIAL_NUMBER + 1, @endSerial = SERIAL_NUMBER + @requestQty FROM dbo.BCD_SERIAL_T 
			WHERE BCD_DATE = @bcdDate 
			AND ORGANIZATION_ID = @organizationId 
			AND SUBINVENTORY_CODE = @subinventory 
			AND PREFIX_CODE = @prefix 

		IF(@endSerial > @maxSerial)
		BEGIN
			RAISERROR('產生條碼失敗，流水號已超出限制長度!!', 16, @step)
		END

		print '@endSerial:' + STR(@endSerial)
		print '@startSerial:' + STR(@startSerial )
		print '@maxSerial:' + STR(@maxSerial)
		
		SET @step = @step + 1
		--寫入條碼流水號
		MERGE INTO dbo.BCD_SERIAL_T WITH (HOLDLOCK) AS A 
			USING (VALUES(@bcdDate, @organizationId, @subinventory, @prefix)) AS B (BCD_DATE, ORGANIZATION_ID, SUBINVENTORY_CODE, PREFIX_CODE) 
			ON A.BCD_DATE = B.BCD_DATE AND A.ORGANIZATION_ID = B.ORGANIZATION_ID AND A.SUBINVENTORY_CODE = B.SUBINVENTORY_CODE AND A.PREFIX_CODE = B.PREFIX_CODE 
			WHEN MATCHED THEN UPDATE SET SERIAL_NUMBER = @endSerial, CREATED_BY = @user, CREATION_DATE = GETDATE()
			WHEN NOT MATCHED THEN INSERT (BCD_DATE, ORGANIZATION_ID, SUBINVENTORY_CODE, PREFIX_CODE, SERIAL_NUMBER, CREATED_BY, CREATION_DATE) 
				VALUES (@bcdDate, @organizationId, @subinventory, @prefix, @endSerial, @user, GETDATE()); 
		IF (@@ROWCOUNT < 0 OR @@ERROR <> 0)
		BEGIN
			RAISERROR('產生條碼失敗，寫入流水號失敗!!', 16, @step)
		END

		SET @step = @step + 1
		--產生條碼清單
		DECLARE @currSerial INT = @startSerial
		WHILE @currSerial <= @endSerial
		BEGIN
			DECLARE @barcode varchar(20) = @prefix + @bcdDate +  + Replace(Str(@currSerial, @serialSize), ' ' , '0')
			
			INSERT INTO @barcodes (BARCODE) VALUES (@barcode)
			SET @currSerial = @currSerial + 1;

			INSERT INTO dbo.BCD_UNIQUE_T (BARCODE, CREATED_BY, CREATION_DATE) VALUES (@barcode, @user, GETDATE())
			IF (@@ROWCOUNT < 0 OR @@ERROR <> 0)
			BEGIN
				RAISERROR('產生條碼失敗，條碼重複!!', 16, @step)
			END
		END

		SET @code = 0
		SET @message = ''
		
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @step
		SET @message = CAST(@step AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
	END CATCH
	
	SELECT BARCODE FROM @barcodes
END

