SELECT  [ZacReporting].[abc096].[IMP_TRANSACT_D_2017].TID,Shop,Mask,Amount,DESTCOMID,Proccode,procbatch,eposbatch,dtstamp,orgsystan,borgsystan,authcode,dmid,dtid
FROM (abc096.TIDs LEFT JOIN [ZacReporting].[abc096].[IMP_TRANSACT_D_2017] ON abc096.TIDs.TID=[ZacReporting].[abc096].[IMP_TRANSACT_D_2017].TID) LEFT JOIN abc096.MIDs
ON [ZacReporting].[abc096].[IMP_TRANSACT_D_2017].MID=abc096.MIDs.MID
WHERE abc096.MIDs.mid='000000120002100' And [ZacReporting].[abc096].[IMP_TRANSACT_D_2017].mid='000000120002100' and respkind='OK' and msgid in ('0200','0220') and amount<>0 and dtstamp<'2017-05-01 00:00:00.000'
and shop in ('Leroy Merlin ÁÅÑÏÄÑÏÌÉÏ ÁÈÇÍÁ','Leroy Merlin ÁÃ.É.ÑÅÍÔÇÓ')
order by shop,[ZacReporting].[abc096].[IMP_TRANSACT_D_2017].tid,dtstamp,destcomid,eposbatch,procbatch
