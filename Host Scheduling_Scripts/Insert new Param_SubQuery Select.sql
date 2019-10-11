
--Procedure for insert
declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

SET @FromModel='Vx-675'
SET @FromAppnm='EPOS0219P'


declare merch_cursor cursor for

select DISTINCT TERMID from vc30.relation
where

TERMID in

(
select distinct termid from vc30.relation where
CLUSTERID = 'EPOS_IKEA'
AND substring(appnm,1,4) = ('EPOS')and
substring(appnm,9,1) = ('P') and
acccnt = -1 and TERMID not in ('73000744','73000731','73000736')
)

and FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin

        insert into vc30.PARAMETER
        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
        (
        select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
        '*',
        PSIZE,FLAG
        from vc30.parameter where PARTID = 'TEPOS0219P' and PARNAMELOC = 'PBG_MID' and
		famnm = @FromModel
		        )

  fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
