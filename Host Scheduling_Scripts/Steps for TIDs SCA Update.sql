-- 1.
-- Script for TID choice per Application and type
select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
substring(appnm,1,4) = ('PIRA')
and substring(appnm,9,1) = ('P')
AND acccnt = -1
and appnm  in ('PIRA0201P')
and substring(TERMID,1,5) <> 'TPIRA'
and famnm = 'Vx-675WiFi'
--and famnm = 'Vx-675'
--and famnm = 'Vx-520'
order by famnm,appnm

-- 2.
-- Save tids from above query to csv file and import to temp table in DB

-- 3. 
-- Check total of TIDs of temp table

Select count(*) from vc30.temp_tids 

--4. 
-- Check that all TIDs have the same libraries and have all libraries

select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
(
 '01026885','01440112'
)
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm

-- Most common case is that CTLS library might be missing. 
-- In that case it shoukd be used the scripts that insert the CTLS library too

-- 4.
-- Basic SQL script for Appl Update
 
declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

SET @FromModel='Vx-520'
SET @FromAppnm='PIRA0204P'
--set @TID = '520GCTLSRES'


declare merch_cursor cursor for
--select a.temptid  from vc30.temp_tids a
select [Column 0]  from vc30.[TIDs - Copy_LN]
--where FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin

--- Apply script for multiple insert + deletes + updates
-- START
	select * 
	from vc30.parameter 
	where PARTID = @tid
-- END			
	
fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor





-- Xtra queries
-- Create temp table for import TIDs
create table vc30.temp_tids
(temptid varchar(15))
