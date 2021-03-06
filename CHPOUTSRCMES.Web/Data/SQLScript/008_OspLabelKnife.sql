CREATE FUNCTION OspLabelKnife (@SPECIFICATION NVARCHAR(40))
RETURNS NVARCHAR(40) 
AS
BEGIN  
	DECLARE @SPEC NVARCHAR(40)
	SET @SPEC = @SPECIFICATION
	RETURN 
		CASE WHEN @SPEC LIKE '%[0-9]' THEN 
		'圓刀 ' + CAST(CAST(SUBSTRING(@SPEC,1,4) as decimal) AS NVARCHAR) + 'MM '
		+ '長刀 ' + CAST(CAST(SUBSTRING(@SPEC,5,4) as decimal) + 3 AS NVARCHAR) + 'MM'
		ELSE
		'圓刀 ' + CAST(FORMAT( ROUND( (cast(SUBSTRING(@SPEC,1,2) as decimal)  + cast(SUBSTRING(@SPEC,3,1) as decimal) /8) * 25.4, 0),'0') AS NVARCHAR) + 'MM '
		+'長刀 '+ CAST(FORMAT( ROUND( (cast(SUBSTRING(@SPEC,5,2) as decimal)  + cast(SUBSTRING(@SPEC,7,1) as decimal) /8) * 25.4, 0) + 3,'0') AS NVARCHAR)  + 'MM'
		END
END
