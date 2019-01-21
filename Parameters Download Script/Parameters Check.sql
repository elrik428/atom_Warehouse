
1.
select q.date, sum(q.terminals) from
(
select  left(value,8)as date ,right(value,6) as time, value, count (*)as terminals  from vc30.parameter where
parnameloc = 'PARAMS_DNLD'
and substring(appnm,1,4) in ('PIRA', 'EPOS') and
parnameloc = 'PARAMS_DNLD' and
substring(partid,1,1) not  in ('T','A','9','C','B','6','5','3','8')
--and substring((right(value,6)),1,1) <> '?' a
--and left(value,8) = '01.12.17'
group by left(value,8), right(value,5),value
--having count(*) <> 150
--order by left(value,8),right(value,5)
)q
group by q.date, q.terminals
order by q.date


2.
select value, count(*) from vc30.parameter where
parnameloc = 'AUTOINCR'
--and (appnm like '%PIR%' or appnm like '%EPOS%')
and partid in (select termid from vc30.relation where clusterid like 'EPOS_%' or clusterid like '%Piraeus%')
--substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8','1','2')
group by value


3.i.
  select partid, parnameloc, value 
  --count(*)
  from vc30.parameter where
  partid = '01312900'
  order by parnameloc
  parnameloc = 'AUTOINCR'

ii.
  update vc30.parameter
  set value = '05'
  where partid = '01312900'
  and
  parnameloc = 'AUTOINCR'
