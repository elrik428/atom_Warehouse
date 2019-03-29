select  q.sub_str ,count(*)
from
(
SELECT tid, substring([MERCHTITLE],1,3) sub_str
FROM [ZACRPT].[dbo].[MERCHANTS]
where substring([MERCHTITLE],1,3) in ('FLY','CLX','OPA','TOR','FDH','APS','VIV')
--and tid =  '50000003'
group by tid,substring([MERCHTITLE],1,3)
--having count(*) >1
) q
group by q.sub_str
order by q.sub_str
