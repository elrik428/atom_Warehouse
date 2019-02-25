declare @Commision varchar(15)
declare @Brand varchar(20)
declare @BankDescr varchar(15)
declare @tid varchar(15)
declare @mid varchar(20)
declare  @DMID         varchar(10)
declare	@DTID 				varchar(8)
declare	@MERCHTITLE   varchar(25)
declare	@MERCHADDRESS varchar(50)
declare	@STORE_CODE   varchar(20)
declare @upload varchar(10)


SET @BankDescr= 'EUROBANK'
SET @Brand = 'OTHER'
SET @Commision = 0
set @upload = 'NET_EBNK'


declare merch_cursor cursor for

select TID, MID,DMID ,DTID,MERCHTITLE,MERCHADDRESS,STORE_CODE from abc096.MERCHANTS
where mid like '%120004000%' and UPLOADHOSTNAME =  @upload
--('NET_NTBN','NET_ALPHA','NET_ABC','NET_EBNK')
order by TID,UPLOADHOSTNAME

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid, @mid ,@DMID,@DTID,@MERCHTITLE,@MERCHADDRESS,@STORE_CODE
while @@FETCH_STATUS = 0
begin


INSERT INTO [abc096].[MERCHANTS_COMM]
           ([SMID]
           ,[STID]
           ,[MERCHTITLE]
           ,[MERCHADDRESS]
           ,[STORE_CODE]
           ,[BANK]
           ,[DMID]
           ,[DTID]
           ,[BRAND]
           ,[COMMISION])

values	(@mid
		   ,@tid
           ,@MERCHTITLE
           ,@MERCHADDRESS
           ,@STORE_CODE
           ,@BankDescr
           ,@DMID
           ,@DTID
           ,@Brand
           ,@Commision)



fetch next from merch_cursor
  into @tid, @mid ,@DMID,@DTID,@MERCHTITLE,@MERCHADDRESS,@STORE_CODE
end

CLOSE merch_cursor
deallocate merch_cursor
