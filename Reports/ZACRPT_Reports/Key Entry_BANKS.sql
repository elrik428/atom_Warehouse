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


-- Update 

update dbo.merchants
set ALLOWED_ACTIONS = '0' + substring(ALLOWED_ACTIONS,2,50)
where uploadhostid = '205' 
and TID in
('73008109','73008437','73008442','73008443','73008444','73008445','73008446','73008447','73008488','73008489','73008490','73008491','73009887','73009888','73009889','73009890','73009891','73009892','73009893','73008110','73008113','73008114','73008485','73008486','73008487','73008492','73008493','73008494','73008127','73008139','73008131','73008132','73008155','73008156','73008157','73008452','73008453','73008454','73008455','73008456','73008472','73008473')
