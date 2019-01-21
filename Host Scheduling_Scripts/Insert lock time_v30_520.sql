declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

SET @FromModel='Vx-520'
SET @FromAppnm='PIRA0203P'
--set @TID = '520GCTLSRES'


declare merch_cursor cursor for
select DISTINCT TERMID from vc30.relation
join vc30.PARAMETER on termid = partid
where
CLUSTERID =  'PIRAEUS' and vc30.relation.APPNM = 'PIRA0203P' and TERMID <> 'TPIRA0203P' and PARNAMELOC = 'MCC' and
--[value] = 'RESTAURANT'
[value] <> 'RESTAURANT'

and vc30.relation.FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin

--        insert into vc30.PARAMETER
--        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
--        (
			select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
			'00:01',
			 --'06:00',
			PSIZE,FLAG
			from vc30.parameter where PARTID = 'TPIRA0203P' and PARNAMELOC = 'LOCK_TRX_TIME' and
			famnm = @FromModel
--		      )

fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
