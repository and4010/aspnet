CREATE FUNCTION OspOutSourcCut (@OSP_HEADER_ID BIGINT)
RETURNS TABLE
AS
RETURN
(
SELECT
H.BATCH_NO AS BatchNo,
H.DUE_DATE AS DueDate,
MPT.SUPPLIER_NAME,
DO.CUSTOMER_NAME AS CustomerName,
DO.ORDER_NUMBER AS OrderNumber,
DO.BASIC_WEIGHT AS BasicWeight,
DO.SPECIFICATION AS Specification,
DO.GRAIN_DIRECTION AS GrainDirection,
DO.REAM_WT AS REAM_WT,
DO.ORDER_WEIGHT AS OrderWeight,
DO.PACKING_TYPE AS PackingType,
DO.PAPER_TYPE AS PaperType,
DO.PRIMARY_QUANTITY,
DI.PAPER_TYPE AS DIPaperType,
CASE WHEN SUBSTRING(DI.SPECIFICATION,4,1) LIKE '%[A-Z]' THEN SUBSTRING(DI.SPECIFICATION,1,3) + 'MM' ELSE DI.SPECIFICATION+'MM' END AS DISPEC,
IT.ITEM_DESC_TCH,
H.NOTE AS Note
FROM [OSP_HEADER_T] H
JOIN OSP_DETAIL_OUT_T DO ON DO.OSP_HEADER_ID = H.OSP_HEADER_ID
JOIN ITEMS_T IT ON IT.INVENTORY_ITEM_ID = DO.INVENTORY_ITEM_ID
JOIN OSP_DETAIL_IN_T DI ON DI.OSP_HEADER_ID = DO.OSP_HEADER_ID
JOIN MACHINE_PAPER_TYPE_T MPT ON MPT.ORGANIZATION_ID = H.ORGANIZATION_ID AND MPT.MACHINE_NUM = H.MACHINE_CODE AND MPT.PAPER_TYPE = DI.PAPER_TYPE
WHERE H.OSP_HEADER_ID = @OSP_HEADER_ID
);
GO