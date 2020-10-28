-- =============================================
-- Author:		Polo Lin
-- Create date: 2020/09/16
-- Description:	SOA XXIF_CHP_P210_IN_MMT_INGR_ST 資料上傳
-- 未解決.. RXD  捲筒理論重
-- =============================================
CREATE PROCEDURE [dbo].[SP_ClearSoaStage]
	@processCode VARCHAR(20),
	@serverCode VARCHAR(20),
	@batchId VARCHAR(20),
	@code INT OUTPUT,
	@message VARCHAR(500) OUTPUT,
	@user VARCHAR(128)
AS
BEGIN
	DECLARE @success INT = 0
	SET @message = ''

	
	BEGIN TRY
		
		IF(@processCode = 'XXIFP218')
		BEGIN
			SET @success = 1
			--清除進櫃SOA上傳註記
			DELETE FROM CTR_SOA_T 
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + ' 清除進櫃上傳註記失敗'
				RAISERROR(@message, 16, @success)
			END

		END
		ELSE IF(@processCode = 'XXIFP221')
		BEGIN
			SET @success = 2
			--清除出貨SOA上傳註記
			DELETE FROM DLV_SOA_T 
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + ' 清除出貨SOA上傳註記失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		ELSE IF(@processCode = 'XXIFP210')
		BEGIN
			SET @success = 3
			--清除加工物料耗用SOA上傳註記
			DELETE D FROM OSP_SOA_S1_T H
			JOIN OSP_SOA_DTL_S1_T D ON D.OSP_SOA_S1_ID = H.OSP_SOA_S1_ID
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			DELETE H FROM OSP_SOA_S1_T H
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + ' 清除出貨SOA上傳註記失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		ELSE IF(@processCode = 'XXIFP211')
		BEGIN
			SET @success = 3
			--清除加工完工入庫且保留SOA上傳註記

			DELETE D FROM OSP_SOA_S2_T H
			JOIN OSP_SOA_DTL_S2_T D ON D.OSP_SOA_S2_ID = H.OSP_SOA_S2_ID
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			DELETE H FROM OSP_SOA_S2_T H
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + ' 清除加工完工入庫且保留SOA上傳註記失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		ELSE IF(@processCode = 'XXIFP213')
		BEGIN
			--清除完工狀態變更SOA上傳註記
			DELETE D FROM OSP_SOA_S3_T H
			JOIN OSP_SOA_DTL_S3_T D ON D.OSP_SOA_S3_ID = H.OSP_SOA_S3_ID
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId

			DELETE H FROM OSP_SOA_S3_T H
			WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
			
			IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
			BEGIN
				SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + ' 清除完工狀態變更SOA上傳註記失敗'
				RAISERROR(@message, 16, @success)
			END
		END
		ELSE IF(@processCode = 'XXIFP222')
		BEGIN
			DECLARE @transactionTypeId BIGINT = 0
			SELECT TOP 1 @transactionTypeId= S.TRANSACTION_TYPE_ID FROM XXIF_CHP_CONTROL_ST C
			JOIN XXIF_CHP_P222_SUB_TRANSFER_ST S ON S.PROCESS_CODE = C.PROCESS_CODE AND S.SERVER_CODE = C.SERVER_CODE AND S.BATCH_ID = C.BATCH_ID

			IF (@transactionTypeId = 355 OR @transactionTypeId = 356)
			BEGIN
				--盤點SOA上傳註記
				DELETE FROM TRF_INVENTORY_SOA_T
				WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
				IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
				BEGIN
					SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + 'TRANSACTION_TYPE_ID:' + CONVERT(varchar, @transactionTypeId) + ' 清除盤點SOA上傳註記失敗'
					RAISERROR(@message, 16, @success)
				END
			END

			IF (@transactionTypeId = 440 OR @transactionTypeId = 441)
			BEGIN
				--雜收發SOA上傳註記
				DELETE FROM TRF_MISCELLANEOUS_SOA_T
				WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
				IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
				BEGIN
					SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + 'TRANSACTION_TYPE_ID:' + CONVERT(varchar, @transactionTypeId) + ' 清除雜收發SOA上傳註記失敗'
					RAISERROR(@message, 16, @success)
				END
			END

			IF (@transactionTypeId = 370)
			BEGIN
				--存貨報廢SOA上傳註記
				DELETE FROM TRF_OBSOLETE_SOA_T
				WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
				IF (@@ROWCOUNT <= 0 OR @@ERROR <> 0)
				BEGIN
					SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + 'TRANSACTION_TYPE_ID:' + CONVERT(varchar, @transactionTypeId) + ' 清除存貨報廢SOA上傳註記失敗'
					RAISERROR(@message, 16, @success)
				END
			END

			IF (@transactionTypeId = 21 OR @transactionTypeId = 375)
			BEGIN
				--庫存移轉SOA上傳註記
				DELETE FROM TRF_SOA_T
				WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
				IF (@@ERROR <> 0)
				BEGIN
					SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + 'TRANSACTION_TYPE_ID:' + CONVERT(varchar, @transactionTypeId) + ' 清除庫存移轉SOA上傳註記失敗'
					RAISERROR(@message, 16, @success)
				END

				--貨故
				DELETE FROM TRF_REASON_SOA_T
				WHERE PROCESS_CODE = @processCode AND SERVER_CODE = @serverCode AND BATCH_ID = @batchId
				IF (@@ERROR <> 0)
				BEGIN
					SET @message = 'PROCESS_CODE:' + @processCode + ' SERVER_CODE:' + @serverCode + ' BATCH_ID:'  + @batchId + 'TRANSACTION_TYPE_ID:' + CONVERT(varchar, @transactionTypeId) + ' 清除貨故SOA上傳註記失敗'
					RAISERROR(@message, 16, @success)
				END
			END
		END

		SET @code = 0
		SET @message = ''
		
	END TRY
	BEGIN CATCH
		SET @code = -1 * @success
		SET @message = CAST(@success AS VARCHAR(2)) + ':' + ERROR_MESSAGE()
	END CATCH

END

