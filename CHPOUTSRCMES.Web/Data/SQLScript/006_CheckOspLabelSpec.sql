Create FUNCTION CheckOspLabelSpec (@SPECIFICATION varchar(40))
RETURNS varchar(40) 
AS
BEGIN  
 DECLARE @Result varchar(40)
	  IF (DATALENGTH(@SPECIFICATION) = 7)
    BEGIN 
        select @Result = '0'+ @SPECIFICATION
    END
    ELSE 
    BEGIN 
		select @Result = @SPECIFICATION
    END
    RETURN @result;
END

