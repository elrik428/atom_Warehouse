select  tid, 'ATTIKA',
--uploadhostid, ALLOWED_ACTIONS, 
case 
when substring(ALLOWED_ACTIONS,1,1) = '1' then 'ALLOWED'
when substring(ALLOWED_ACTIONS,1,1) = '0' then 'NOT ALLOWED'
end as 'Status'
--count(*)
--ALLOWED_ACTIONS 
from dbo.merchants
where uploadhostid = '7' and substring(tid,1,4) = '7300'
group by tid, uploadhostid, ALLOWED_ACTIONS
--having count(*) > 1