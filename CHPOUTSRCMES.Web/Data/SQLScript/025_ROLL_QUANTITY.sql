CREATE FUNCTION ROLL_QUANTITY (@itemCategory NVARCHAR(10), @primaryQuantity decimal(30, 10))
RETURNS NVARCHAR(150) 
AS
BEGIN  
	RETURN  CASE @itemCategory WHEN '平版' THEN NULL ELSE CAST(ABS(@primaryQuantity) AS nvarchar(150)) END
END
GO
