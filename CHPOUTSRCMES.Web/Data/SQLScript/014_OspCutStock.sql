CREATE FUNCTION OspCutStock (@OSP_HEADER_ID BIGINT)
RETURNS TABLE
AS
RETURN
(
	SELECT 
	H.BATCH_NO,
	H.CUTTING_DATE_TO,
	OPT.PAPER_TYPE,
	OPT.BASIC_WEIGHT,
	OPT.SPECIFICATION,
	OPT.SECONDARY_QUANTITY,
	DO.PACKING_TYPE,
	ROW_NUMBER() OVER(ORDER BY OPT.OSP_HEADER_ID ) AS SubId,
	(SELECT count(SECONDARY_QUANTITY) FROM OSP_PICKED_OUT_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID) as Count,
	(SELECT SUM(SECONDARY_QUANTITY) FROM OSP_PICKED_OUT_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID) as Summ,
	OPT.LOT_NUMBER,
	OPT.PRIMARY_QUANTITY
	FROM OSP_PICKED_OUT_T OPT
	JOIN OSP_HEADER_T H ON H.OSP_HEADER_ID = OPT.OSP_HEADER_ID
	JOIN OSP_DETAIL_OUT_T DO ON DO.OSP_HEADER_ID = H.OSP_HEADER_ID
	WHERE OPT.OSP_HEADER_ID = @OSP_HEADER_ID
);




