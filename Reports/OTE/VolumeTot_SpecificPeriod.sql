select mid,(CASE substring(mid,11,2)
WHEN '00' THEN 'OTE OWN'
WHEN '10' THEN 'COSMOTE OWN'
WHEN '11' THEN 'COSMOTE FRANCHISEE'
WHEN '20' THEN 'GERMANOS OWN'
WHEN '21' THEN 'GERMANOS FRANCHISEE'
end) as 'Description',
 sum(CASE substring(mid,11,2)
WHEN '00' THEN +TrxNo
WHEN '10' THEN +TrxNo 
WHEN '11' THEN +TrxNo 
WHEN '20' THEN +TrxNo 
WHEN '21' THEN +TrxNo 
else 0
end) as 'Totals'
from [abc096].[TrxMonth]
where substring(mid,1,8)= '00000017' and mid like '%PPC'
group by mid
order by [Description]