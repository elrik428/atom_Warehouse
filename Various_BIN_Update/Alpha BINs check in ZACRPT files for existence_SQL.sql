SELECT  destport
  FROM [ZACRPT].[dbo].[INSTALLMENTBINS]
  group by destport


--Duplicate check
SELECT binlower
  FROM [ZACRPT].[dbo].[INSTALLMENTBINS]
  where destport = 'NET_CLBICALPHA'
  group by binlower
  having count(*) >1


    SELECT [Column 0], count(*)
    FROM [ZACRPT].[dbo].[Alph_bins_TempLN]
    group by [Column 0]
    having count(*) >1



-- Not exist in new list but exist in current  table INSTALLMENTBINS
SELECT [ID]
      ,[DESTPORT]
      ,[BINLOWER]
      ,[BINUPPER]
       [SYNC_ID]
  FROM [ZACRPT].[dbo].[INSTALLMENTBINS]
  where destport = 'NET_CLBICALPHA' and not   exists(
  SELECT [Column 0]
  FROM [ZACRPT].[dbo].[Alph_bins_TempLN]
  where [Column 0] = binlower )


-- BINs to be inserted to INSTALLMENTBINS
select q.binlow_er
from
(
  SELECT [Column 0] as binlow_er
  FROM [ZACRPT].[dbo].[Alph_bins_TempLN]
  where not  exists
  (
	select binlower
	from [ZACRPT].[dbo].[INSTALLMENTBINS]
	where binlower = [Column 0]
  and destport = 'NET_CLBICALPHA' )
  --order by binlow_er
  ) q
  where   exists (select binlower from [ZACRPT].[dbo].[INSTALLMENTBINS] where binlower = q.binlow_er)




NET_CLBICALPHA
NET_ALPMOR
NET_CLBICEBNK
NET_NTBN
