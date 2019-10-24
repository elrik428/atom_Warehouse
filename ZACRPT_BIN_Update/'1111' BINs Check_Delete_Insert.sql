-- A.
--Backup table
drop table [dbo].[MERCHBINS_BACKUP]
select * into dbo.[MERCHBINS_BACKUP] from dbo.merchbins

-- B.
-- Insert into temp file that is created by scratch every time a request arrives
-- [dbo].[Bins_TempLN]

-- C.
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
  from [ZACRPT].[dbo].[MERCHBINS]
  where binlower = [Column 0]
  and destport = 'NET_ABC' and tid = '1111    ' )

-- D.
-- Delete
--i. Very small number of BINs
delete from [dbo].[MERCHBINS]
where destport in('NET_BICALPHA','NET_ALPMOR') and binlower in ('379528',
'467108',
'483687',
'541871')

--ii. List of BINs
delete from [ZACRPT].[dbo].[MERCHBINS]
where binlower in(
  SELECT [BINLOWER]
  FROM [ZACRPT].[dbo].[MERCHBINS]
  where destport = 'NET_NTBN' and tid = '1111    ' and not exists(
  SELECT [Column 0]
  FROM [ZACRPT].[dbo].[Bins_TempLN]
  where [Column 0] = binlower )) and destport = 'NET_NTBN'


--E.
-- Insert

declare @tid varchar(16)

declare merch_cursor cursor for
select DISTINCT TID from MERCHBINS
where (binlower = '479273' and binupper = '479273')
and tid <> '1111    '  and tid not in ('11000344','11000345','11000346','11000347','11000348','11000349','11000350','11000351','11000352','11000353','11000360','11000361','11000363','11000364','11000365','11000366','11000367','11000368','11000369','11000370','11000371','11000437','73008523','73008524')

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin

  insert into MERCHBINS
  (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
  (
  select @TID, DESTPORT, '45503901', '45503901', INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from MERCHBINS
  where TID = @TID and (binlower = '479273' and binupper = '479273')
  )

  fetch next from merch_cursor
  into @tid

end

CLOSE merch_cursor
deallocate merch_cursor




-- F. Duplicate check in 1111
select binlower from dbo.merchbins
where tid = '1111    ' 
group by binlower
having count(*) >1
