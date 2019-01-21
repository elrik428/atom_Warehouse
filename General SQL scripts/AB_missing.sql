select * from abc096.IMP_TRANSACT_D_monthly
where
MID in ('000000150000001', '000000150000011')
and DTSTAMP_INSERT is null
and RESPKIND = 'OK'
--and REVERSED not in('F', 'A')
AND LEFT(PROCCODE,2) = '20'
ORDER BY DTSTAMP
m

select dtstamp,bdtstamp,*
from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]
where left(bdtstamp,11)<>left(dtstamp,11)
 and MSGID = '0220' and  MID in ('000000150000001', '000000150000011')
order by left(dtstamp,11)


select *
from  [abc096].[IMP_TRANSACT_D]


-- Void of refund
select * from abc096.IMP_TRANSACT_D_2018
where
month(DTSTAMP) = '5' and
MID in ('000000150000001', '000000150000011')
and PROCCODE = '220000'

-- Εκτος πακετου
select * from abc096.IMP_TRANSACT_D_2018
where
MID in ('000000150000001', '000000150000011')
--and DTSTAMP_INSERT is null
and month(dtstamp) = '5'
and RESPKIND = 'OK'
and btbl = ''
and REVERSED not in('F', 'A')
and PROCCODE  = '000000'
and MSGID in('0200','0220')
   and PROCCODE  in ('020000','200000')
--AND LEFT(PROCCODE,2) = '20'
ORDER BY DTSTAMP

--  1.

select * from abc096.IMP_TRANSACT_D_monthly
where
MID in ('000000150000001', '000000150000011')
and DTSTAMP_INSERT is null
and RESPKIND = 'OK'
and REVERSED not in('F', 'A')
AND LEFT(PROCCODE,2) = '22'
ORDER BY DTSTAMP

-- 2.
select * from abc096.IMP_TRANSACT_D_monthly
where
MID in ('000000150000001', '000000150000011')
and DTSTAMP_INSERT is null
and RESPKIND = 'OK'
--and REVERSED not in('F', 'A')
AND LEFT(PROCCODE,2) = '20'
ORDER BY DTSTAMP


update zacreporting.[abc096].[imp_TRansact_D]
set productid = (
select b.id from zacreporting.[abc096].[imp_TRansact_D] c
inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
group by b.id,b.bin)
