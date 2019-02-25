-- A
--Backup table
drop table dbo.INSTALLMENTBINS_BUP
select * into dbo.INSTALLMENTBINS_BUP from dbo.INSTALLMENTBINS


-- B
-- Insert into temp file that is created by scratch every time a request arrives
-- [dbo].[Bins_TempLN]

-- C
-- Check TIDs
-- Duplicates in Temp file
SELECT [Column 0], count(*)
FROM [ZACRPT].[dbo].[Bins_TempLN]
group by [Column 0]
having count(*) >1

-- To be inserted
SELECT [Column 0] as binlow_er
FROM [ZACRPT].[dbo].[Bins_TempLN]
where not  exists
( select binlower
  from [ZACRPT].[dbo].[INSTALLMENTBINS]
  where binlower = [Column 0]
  and destport = 'NET_ABC' )

-- D
-- Delete
--i. Very small number of BINs
delete from [dbo].[INSTALLMENTBINS]
where destport in('NET_CLBICALPHA','NET_ALPMOR') and binlower in ('379528',
'467108',
'483687',
'541871')

--ii. List of BINs
delete from [ZACRPT].[dbo].[INSTALLMENTBINS]
where binlower in(
  SELECT [BINLOWER]
  FROM [ZACRPT].[dbo].[INSTALLMENTBINS]
  where destport = 'NET_NTBN' and not   exists(
  SELECT [Column 0]
  FROM [ZACRPT].[dbo].[Bins_TempLN]
  where [Column 0] = binlower )) and destport = 'NET_NTBN'


--E
-- Insert
declare @binlow_er varchar(22)
declare @destpo_rt varchar(20)

SET @destpo_rt= 'NET_NTBN'

declare Bins_cursor cursor for
  SELECT [Column 0] as binlow_er
  FROM [ZACRPT].[dbo].[Bins_TempLN]
  where not  exists
  (
	select binlower
	from [ZACRPT].[dbo].[INSTALLMENTBINS]
	where binlower = [Column 0]
  and destport = @destpo_rt )


open Bins_cursor
if @@ERROR > 0
  return

fetch next from Bins_cursor
into @binlow_er

while @@FETCH_STATUS = 0
begin

       INSERT INTO [dbo].[INSTALLMENTBINS]
           ([DESTPORT]
           ,[BINLOWER]
           ,[BINUPPER]
           ,[SYNC_ID])
        VALUES
		(@destpo_rt, @binlow_er,@binlow_er,null)

fetch next from Bins_cursor
  into @binlow_er
end

CLOSE Bins_cursor
deallocate Bins_cursor
