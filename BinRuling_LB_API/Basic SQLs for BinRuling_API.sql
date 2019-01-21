--  1. Bin ruling
with merchtid(#mid,#tid,#name) as
 (select mid,TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b where mid = '000000120001942' and a.uploadhostid = '1' and a.uploadhostid = b.uploadhostid),

merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as
(select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from dbo.MERCHBINS_TEST where TID = '1111    ' and DESTPORT ='NET_ABC')

--insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER ,0,1, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
   select #tid, #DESTPORT, #BINLOWER, #BINUPPER,0,1, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX from  merchtid,merchInfo
ORDER BY #TID




---2. GMAXINST
with  fndMaxPBnk (tid_, maxinst,desport) as (select tid,max(instmax) max_inst,
case destport
when 'NET_ABC' THEN '1'
when 'NET_NTBN' THEN '6'
when 'NET_CLBICALPHA' THEN '202'
when 'NET_CLBICEBNK' THEN '205'
when 'NET_ALPMOR' THEN '201'
end des_port
from merchbins where tid = '78000000'
group by tid,destport)
--order by tid)
select a.gmaxinst, a.*
from MERCHANTS a
join fndMaxPBnk b on a.tid = b.tid_ and a.uploadhostid = b.desport
--update MERCHANTS
--set gmaxinst = maxinst
--from fndMaxPBnk
--where tid_ = tid and uploadhostid = desport



-- 3. Load balances

------ 1.
drop TABLE [zacrpt_test].[dbo].[tmp_LB]
CREATE TABLE [zacrpt_test].[dbo].[tmp_LB] (
 ID int IDENTITY(1,1) PRIMARY KEY, TID nvarchar(16))

------ 2.
declare @NumberRecords int, @RowCount int
declare @TID varchar(16)
declare @BALGROUP varchar(16)

Insert into [zacrpt_test].[dbo].[tmp_LB](tid)
Select distinct('M'+TID) FROM dbo.MERCHANTS_test WHERE mid = '000000120005108'

set @NumberRecords = (select count(*) from [zacrpt_test].[dbo].[tmp_LB])
set @RowCount = 1

While @RowCount <= @NumberRecords
Begin


select distinct @tid=tid
from [zacrpt_test].[dbo].[tmp_LB]
where id = @RowCount
set @BALGROUP = @tid

      INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_CLBICALPHA', 'C', '01')
      INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_CLBICEBNK', 'C', '00')
      INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_NTBN', 'C', '1000000')
      INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
              VALUES (@BALGROUP, 'NET_ABC', 'C', '0')

set @rowcount = @rowcount + 1
end


---------------

delete  from [zacrpt_test].[dbo].[tmp_LB]
select * from [zacrpt_test].[dbo].[tmp_LB] order by tid asc


select * from [zacrpt_test].[dbo].[LOADBALANCE_test]
where balancegroup like '%M7300627%'


/*
delete from [zacrpt_test].[dbo].[LOADBALANCE_test]
where balancegroup like '%M73006278%'
*/
