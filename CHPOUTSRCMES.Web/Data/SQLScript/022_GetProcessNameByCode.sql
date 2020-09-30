CREATE FUNCTION GetProcessNameByCode (@processCode NVARCHAR(20))
RETURNS NVARCHAR(20)
AS
BEGIN
RETURN 
CASE @processCode
WHEN 'XXIFP217' THEN '進櫃入庫-資料轉入'  
WHEN 'XXIFP218' THEN '進櫃入庫-資料回傳'  
WHEN 'XXIFP220' THEN '出貨-資料轉入'  
WHEN 'XXIFP221' THEN '出貨-資料回傳'  
WHEN 'XXIFP219' THEN '加工-資料轉入'  
WHEN 'XXIFP210' THEN '加工-生產批物料耗用'  
WHEN 'XXIFP211' THEN '加工-生產批完工入庫且保留'  
WHEN 'XXIFP213' THEN '加工-生產批完工狀態變更'  
WHEN 'XXIFP222' THEN '庫存異動'
ELSE '其它' END

END;