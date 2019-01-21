SELECT  left(dtstamp,3),Shop,destcomid,left(proccode,2),count(left(proccode,2)),sum(tamount)
FROM (abc096.TIDs LEFT JOIN [ZacReporting].dbo.[TRANSLOG_TRANSACT_2017] ON abc096.TIDs.TID=[ZacReporting].dbo.[TRANSLOG_TRANSACT_2017].TID) LEFT JOIN abc096.MIDs
ON [ZacReporting].dbo.[TRANSLOG_TRANSACT_2017].MID=abc096.MIDs.MID
WHERE abc096.MIDs.mid='000000120002100' And [ZacReporting].dbo.[TRANSLOG_TRANSACT_2017].mid='000000120002100' and respkind='OK' and tamount<>0
GROUP BY left(dtstamp,3),Shop,destcomid,left(proccode,2)
