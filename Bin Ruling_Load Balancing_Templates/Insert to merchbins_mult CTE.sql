with merchtid(@tid, @name) as (
                    select  TID, uploadhostname from MERCHANTS a, uploadhosts b
                    where mid = ('000000120002800')
                    and a.uploadhostid = '205'
                    and a.uploadhostid = b.uploadhostid
                  ),
merchInfo(@DESTPORT, @BINLOWER, @BINUPPER, @GRACEMIN, @GRACEMAX, @ALLOWED, @AMOUNTMIN, @AMOUNTMAX ) as(
                    select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX
                    from MERCHBINS
                    where TID = '1111    ' and DESTPORT = 'NET_CLBICEBNK'
                  )

/*insert into zacreporting.dbo.MERCHBINS
(TID, DESTPORT, BINLOWER, BINUPPER ,0,1, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
(select @tid, @name, @DESTPORT, @BINLOWER, @BINUPPER, @GRACEMIN, @GRACEMAX, @ALLOWED, @AMOUNTMIN, @AMOUNTMAX
from  merchtid,merchInfo )*/


--TID, DESTPORT, BINLOWER, BINUPPER ,0,1, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
select @tid, @name, @DESTPORT, @BINLOWER, @BINUPPER,0,1, @GRACEMIN, @GRACEMAX, @ALLOWED, @AMOUNTMIN, @AMOUNTMAX
from  merchtid,merchInfo


-- Working
with merchtid(#mid,#tid,#name) as (
                    select mid,TID, uploadhostname from MERCHANTS a, uploadhosts b
                    where mid = ('000000001100010') and tid = 71002860
                    --and a.uploadhostid = '1'
                    and a.uploadhostid = b.uploadhostid
                  ),
merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as(
                    select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX
                    from MERCHBINS
                    where TID = '1111    ' and DESTPORT = 'NET_ABC'
                  )

/*insert into zacreporting.dbo.MERCHBINS
(TID, DESTPORT, BINLOWER, BINUPPER ,0,1, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
(select #tid, #name, #DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX
from  merchtid,merchInfo )*/


--TID, DESTPORT, BINLOWER, BINUPPER ,0,1, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
select #mid,#tid, #name, #DESTPORT, #BINLOWER, #BINUPPER,0,1, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX
from  merchtid,merchInfo
