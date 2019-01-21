

--Procedure for insert
declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

--SET @FromModel='Vx-675'
SET @FromModel='Vx-520'
--SET @FromModel='Vx-820'
SET @FromAppnm='EPOS0215P'


declare merch_cursor cursor for

select DISTINCT TERMID from vc30.relation
where

TERMID in

('73003849','73003852','73003853','73003850','73003851','73003779','73003778','73003775','73003777','73004689','73003776','73005861','73004367','73004368','73004370','73004369','73005862')

and FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin
--  delete from vc30.PARAMETER where PARTID = @tid

        insert into vc30.PARAMETER
        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
        (
        select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
        --VALUE,
        --'FALSE',
        --'NO_PP1000',
        --'TRUE',
		'15:20',
        --'3',
        --'1',
        --'*',
        PSIZE,FLAG
        from vc30.parameter where PARTID = 'TEPOS0215P' and PARNAMELOC = 'AUTO_HOSTSCH' and
		famnm = @FromModel
		        )

  fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
