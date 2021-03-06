declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

SET @FromModel='Vx-520'
SET @FromAppnm='PIRA0204P'
--set @TID = '520GCTLSRES'


declare merch_cursor cursor for
select DISTINCT TERMID from vc30.relation
where

CLUSTERID =  'PIRAEUS' and APPNM = 'PIRA0204P' and TERMID <> 'TPIRA0204P'

--('520GCAR','520GCTLSCAR','520GCTLSHOTEL','520GCTLSTRAVEL','520GHOTEL','520GINST','520GRES','520GSUPER','520GCTLSSALES','520GCTLSSUPER','520GCTLSRES','520GCTLSINST','520GSALES','520GSERV','520GTRAVEL','COMBOCTLSCAR','COMBOCTLSINST','COMBOCTLSRES','COMBOCTLSSALES','COMBOINST','COMBOSALES','COMBOSUPER','COMBOCAR','COMBOCASH','COMBOCTLSHOTEL','COMBOCTLSSUPER','COMBOCTLSTRAVEL','COMBOHOTEL','COMBORES','COMBOSERV','COMBOTRAVEL','675CTLSCAR-CO','675CTLSHOT','675CTLSCAR','675CAR','675CTLSHOT-CO','675CTLSINS','675CTLSINS-CO','675CTLSRES','675CTLSRES-CO','675HOTEL','675INST','675SALES','675TRAVEL','675CTLSSAL','675CTLSSAL-CO','675CTLSSUP','675CTLSSUP-CO','675CTLSTRA','675CTLSTRA-CO','675INSTANT','675RES','675SUPER','675TESTCTLSP','675WIFICAR','675WIFIHOT','675WIFIINS','675WIFIRES','675WIFISAL','675WITEST','675INSTANTWF','675WIFISUP','675WIFITRA')

and FAMNM = @FromModel

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
			(select
			case
			when PARNAMELOC = 'MCC' and [value] = 'RESTAURANT' then '06:00'
			else '00:01'
			end as new_Value
			from vc30.PARAMETER where PARTID = @tid and PARNAMELOC = 'MCC'),
			--VALUE,
			--'FALSE',
			--'NO_PP1000',
			--'TRUE',
			--'0',
			--'3',
			--'1	',
			--'*',
			PSIZE,FLAG
			from vc30.parameter where PARTID = 'TPIRA0204P' and PARNAMELOC = 'LOCK_TRX_TIME' and
			famnm = @FromModel
		--        )

fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
