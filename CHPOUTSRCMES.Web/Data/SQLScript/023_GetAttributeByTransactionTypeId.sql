CREATE FUNCTION GetAttributeByTransactionTypeId (@transactionTypeId BIGINT)
RETURNS NVARCHAR(240)
AS
BEGIN
RETURN 
CASE @transactionTypeId
WHEN 355 THEN 'O'  --CHP-16庫存調整(出)
WHEN 356 THEN 'I'  --CHP-16庫存調整(入)
WHEN 370 THEN 'O'  --CHP-26存貨報廢(出)
WHEN 440 THEN 'O'  --CHP-37出貨數尾差調整(出)
WHEN 441 THEN 'I'  --CHP-37出貨數尾差調整(入)
ELSE '' END

END;