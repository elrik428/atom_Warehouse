select a.tid,a.mask,a.mid,v.Merchant,
case 
when b.DMID is null then 'N/A'
else b.DMID end as 'Eurobank MID',
a.AMOUNT,a.DTSTAMP,a.AUTHCODE as 'Approval Code'
from abc096.IMP_TRANSACT_D a
join 
(select  tid,mid, UPLOADHOSTNAME,DMID from abc096.MERCHANTS
where UPLOADHOSTNAME = 'NET_EBNK'
group by tid,mid, UPLOADHOSTNAME,DMID) b on a.tid = b.TID --and a.MID = b.MID
join abc096.MIDs v on a.MID = v.MID
where 
substring(mask,1,6) in ('406301','413492','418895','418895','455039','479273','479275','479276','494092','455039','455039','510155','539731','545865','552202','552202','533598','601976','601976','601976','601976''601976','601976','404934','402957','439506')
and RESPKIND = 'OK' 
AND DTSTAMP > '2019-10-25 13:47:00.000' AND DTSTAMP < '2019-10-29 12:00:00.000' 
and INST <> 0
-- and b.DMID is not null 
and a.TID in (select [Column 0] from dbo.ebnk_tids)
ORDER BY  a.DTSTAMP




--- BONUS MIDs
select a.mask,
a.mid,
case 
when b.DMID is null then 'N/A'
else b.DMID end as 'DMID',
a.AMOUNT,a.DTSTAMP,a.AUTHCODE  from abc096.IMP_TRANSACT_D a
join 
(select  tid,mid, UPLOADHOSTNAME,DMID from abc096.MERCHANTS
where UPLOADHOSTNAME = 'NET_EBNK'
group by tid,mid, UPLOADHOSTNAME,DMID) b on a.tid = b.TID and a.MID = b.MID
where substring(mask,1,6) in ('406301','413492','418895','418895','455039','479273','479275','479276','494092','455039','455039','510155','539731','545865','552202','552202','533598','601976','601976','601976','601976''601976','601976','404934','402957','439506')
and RESPKIND = 'OK' AND 
DTSTAMP > '2019-10-25 13:47:00.000' and 
b.DMID is not null and 
INST <> 0 and 
a.MID in 
(select a.MID from abc096.MIDs a
where    exists (select '0'+substring(b.MID,2,20) from abc096.MIDs  b
where substring(b.MID,1,1) <> '0' and substring(b.MID,1,1) = 'B' and a.MID ='0'+substring(b.MID,2,20) and b.mid <> '000000078000000' ) 
and a.mid <> '000000078000000')
ORDER BY DTSTAMP
