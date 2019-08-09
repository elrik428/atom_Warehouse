-- !!!!!!!!
-- For the production terminals that merchants have sent us to update so to check the new application, YOU RUN THE SCRIPTS AS THEY ARE IGNORE BELOW INSTRUCTIONS
-- Just replace (@tid) with TIDs and find corresponding script for the version that TIDs are right now
-- e.g for TID 73000822(NOTOS) as it is in version 0217 you 'll choose 'SCA EPOS_0214, 15, 17, 18 to 0219_Template.sql' as the corresponding script to run


-- 1. Script for TID choice per Application and type 
-- A. EPOS terminals
select distinct CLUSTERID,termid, appnm   from vc30.relation where
CLUSTERID = 'EPOS_ALOUETTE'
AND substring(appnm,1,4) = ('EPOS')and
substring(appnm,9,1) = ('P') and
acccnt = -1
--and appnm  in ('EPOS01C6P')
--and appnm   in ('EPOS0201P')X
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
order by appnm

-- B. PBG terminals
-- IN order to choose PBG terminals for update you should get weekly file from \\10.7.17.11\PDS_TMS_Reports\Archive and pick file 8-4-19-VERICENTRE.xlsx
-- From there you can filter by type of Connections, e.g ETH, WIFI, GPRS etc and you can create the file so to upload it to DB as mentioned in step 2.

-- 2.
-- Save tids from above query to csv file and import to temp table in DB
-- Keep in mind to remove any template terminals along with any test terminals

-- 3. 
-- Check total of TIDs of temp table

Select count(*) from vc30.temp_tids 

--4. 
-- Check that all TIDs have the same libraries and have all libraries
-- Below script the sme for EPOS + PIR

select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
(
 '01026885','01440112'
)
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm

-- There might be a case that CTLS library will be missing for PIR TIDs. 
-- In that case, the scripts that must be used are the ones that insert the CTLS library too
 -- e.g  SCA_0206-07-08 to 09 + CTLS library.sql

-- 5.
-- In order to run scripts for update you can also check  'RTR_EPOS_0201 to 0219.sql'  as basic template or  'Template for Basic SQL script.sql'

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


--- COPY SCRIPT FROM FILE HERE for multiple insert + deletes + updates
-- START

	-- select * 
	-- from vc30.parameter 
	-- where PARTID = @tid

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
