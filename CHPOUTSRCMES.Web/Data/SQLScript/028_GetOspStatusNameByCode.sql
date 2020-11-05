USE [CHPOUTSRCMES]
GO
/****** Object:  UserDefinedFunction [dbo].[GetProcessNameByCode]    Script Date: 2020/11/5 上午 11:45:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetOspStatusNameByCode] (@statusCode NVARCHAR(20))
RETURNS NVARCHAR(20)
AS
BEGIN
RETURN 
CASE @statusCode
WHEN '0' THEN '待排單'  
WHEN '1' THEN '已排單'  
WHEN '2' THEN '待核准'  
WHEN '3' THEN '已完工'  
WHEN '4' THEN '已關帳'  
WHEN '5' THEN '已修改'  
WHEN '6' THEN '已取消'  
ELSE '其它' END

END;
