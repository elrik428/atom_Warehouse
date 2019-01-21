select tid, min(a.dtstamp), max(a.dtstamp), count(*) from (select tid, DTSTAMP from abc096.IMP_TRANSACT_D_2018 a union select tid, DTSTAMP from abc096.IMP_TRANSACT_D_2018_B) a
where  exists(select TID from abc096.temp_tid b where a.tid = b.TID )
group by TID
