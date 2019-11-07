
----  *      Variable  declaration
-- *  Installment tmp fields
declare @instnbr as numeric(3,0)
declare @instnbr_tmp as numeric(3,0)

-- 
declare @date_tmp as datetime
declare @date_tmpRVRT as datetime
declare @date_Original as datetime
declare @date_Toins as varchar(50)

-- Tmp
declare @date_chk_1 as datetime

-- *   Final date for each Bank
declare @date_NBG as varchar(50)
declare @date_PIR as varchar(50)
declare @date_EBNK as varchar(50)
declare @date_ALPHA as varchar(50)

-- *  Tmp date in final format YYYYMMDD
declare @date_chk as varchar(50)
declare @date_chk#2 as varchar(50)


-- Counter of days for PIR date
declare @ebnkcnt as int


-----  *    SCRIPTs

-- Get installments number & initial trx date
select @instnbr = inst, @date_tmp = DTSTAMP from  abc096.IMP_TRANSACT_D_tempLN

set @date_Toins = convert(nvarchar(MAX), @date_tmp, 112)
set @date_chk = DATENAME(DW,cast(year(@date_tmp) as varchar)+'/' + cast(month(DATEADD(month,1,@date_tmp)) as varchar) + '/' + cast(day(@date_tmp) as varchar))
set @date_tmpRVRT = @date_tmp
set @date_Original = @date_tmp


-- ALPHA date script
set @date_tmp = DATEADD(month,1,@date_tmp)
set @date_tmp = DATEADD(DAY,1,@date_tmp)
set @date_ALPHA=  convert(nvarchar(MAX), @date_tmp, 112)


set @date_tmp = @date_tmpRVRT
-- NBG date script
set @date_tmp =  DATEADD(month, DATEDIFF(month, 0, @date_tmp), 0)
set @date_tmp =  DATEADD(month,1,@date_tmp)
set @date_tmp =  DATEADD(day,4,@date_tmp)
set @date_NBG =  convert(nvarchar(MAX), @date_tmp, 112)


set @date_tmp = @date_tmpRVRT
-- EBNK date script
set @ebnkcnt = 0
set @date_tmp =  DATEADD(month, DATEDIFF(month, 0, @date_tmp), 0)
set @date_tmp =  DATEADD(MONTH, 1, @date_tmp)
set @date_tmp =  DATEADD(DAY, @ebnkcnt, @date_tmp)
set @date_chk =  DATENAME(DW,cast(year(@date_tmp) as varchar)+'/' + cast(month(@date_tmp) as varchar) + '/' + cast(day(@date_tmp) as varchar))

WHILE @date_chk in ('Sunday','Saturday')
BEGIN
set @ebnkcnt =  1
set @date_tmp =  DATEADD(DAY, @ebnkcnt, @date_tmp)
set @date_chk = DATENAME(DW,cast(year(@date_tmp) as varchar)+'/' + cast(month(@date_tmp) as varchar) + '/' + cast(day(@date_tmp) as varchar)) 
END
set @date_EBNK =  convert(nvarchar(MAX), @date_tmp, 112)



-- Print variables
--print '@inst =  ' +  cast(@instnbr as varchar (20))
print '@date_Original :'  + cast(@date_Original as varchar(20))
print '@date_tmpRVRT  : ' + cast(@date_tmp as varchar (20))
--print '@date_chk  : '  +  @date_chk
print '@date_ALPHA    : ' + @date_ALPHA
print '@date_NBG      : ' + @date_NBG
print '@date_EBNK     : ' + @date_EBNK




set @instnbr_tmp = 1

while @instnbr_tmp <= @instnbr
begin

INSERT INTO [dbo].[TRANSACT_INST]
           ([DTSTAMP_TRX]
           ,[MARKET_NBR]
           ,[POS_ID]
           ,[MATURE_DATE]
           ,[PACKET_NBR]
           ,[MASK]
           ,[AMOUNT]
           ,[INST]
		   ,[INST_NBR]
           ,[INST_VALUE]
           ,[BANK_CODE]
           ,[CARD_TYPE]
           ,[UNQTRX_ID]
           ,[TRXTYPE_ID])
select DTSTAMP,
'0000xxxx',
'E0003000001',
'20190101',
'00000001',
'123456xxxxxx7890',
AMOUNT,
@instnbr, 
@instnbr_tmp,
amount/@instnbr,
'001',
'VISA',
'0001',
ORIGINATOR
from abc096.IMP_TRANSACT_D_tempLN

-- New installment
set @instnbr_tmp = @instnbr_tmp + 1
print '@instnbr_tmp = ' + cast(@instnbr_tmp as varchar(20))

-- Find new maturity date for new installment
select @date_tmp = MATURE_DATE from dbo.TRANSACT_INST

end

GO