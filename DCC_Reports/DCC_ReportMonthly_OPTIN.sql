
-- 1. Regency
select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'
from regencyw a, abc096.merchants b
where datum > '2018-01-01 00:00:00' and
right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and
(
(a.TID in ('GR73007243','GR73007244','GR73007246')
and datum>='2018-01-01 00:00:01')
)
group by  merchaddress,left(datum,7)
order by  merchaddress,left(datum,7)
;

-- 2. Attica
select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'
from attikaw a, abc096.merchants b
where datum > '2017-10-01 00:00:00' and
right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and
(
(a.TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(a.TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
)
group by  merchaddress,left(datum,7)
order by  merchaddress,left(datum,7)
;

-- 3. Duty Free
select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'
from dutyfreew a, abc096.merchants b
where datum > '2017-01-01 00:00:00' and
right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC'
group by  merchaddress,left(datum,7)
order by  merchaddress,left(datum,7)
;
