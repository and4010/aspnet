CREATE FUNCTION CheckOspLabelSize (@SPECIFICATION NVARCHAR(40))
RETURNS NVARCHAR(40) 
AS
BEGIN  
DECLARE @SPEC NVARCHAR(40)
SET @SPEC = @SPECIFICATION
    RETURN
CASE WHEN @SPEC LIKE '%[0-9]' THEN 
SUBSTRING(@SPEC,1,4) +'MM'+'*'+ SUBSTRING(@SPEC,5,8)+'MM'
ELSE
CAST(SUBSTRING(@SPEC,1,2) + CAST(SUBSTRING(@SPEC,3,1) AS FLOAT)/8 AS NVARCHAR)
+'*'+ CAST(SUBSTRING(@SPEC,5,2) + CAST(SUBSTRING(@SPEC,7,1) AS FLOAT)/8 AS NVARCHAR)
END
END
GO
