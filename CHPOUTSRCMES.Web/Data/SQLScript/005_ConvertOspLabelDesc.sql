CREATE FUNCTION ConvertOspLabelDesc (@packingType NVARCHAR(30))
RETURNS NVARCHAR(30) 
AS
BEGIN
    RETURN
	CASE @packingType WHEN '無令打件' THEN '大標籤'
	WHEN '令包' THEN '小標籤'
	ELSE '小標籤' END 
END;