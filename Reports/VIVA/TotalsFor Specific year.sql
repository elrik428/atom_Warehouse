---------  USED
select 
b.[Group],count(*) as TRXS,
sum(TAMOUNT) SUMAMNT
from dbo.TRANSLOG_TRANSACT_2017 a
join abc096.MIDs b on a.MID = b.MID
where 
(a.MID like '%0000838%' or a.MID = '000000160000001')  and a.MID <> '000000083890000' 
group by  b.[Group]



-----    eCommerce
---- 2016
 select  '2016 VIVA Stats',
 --Volume
sum(case when b.[Group]     in ('VIVA eCommerce') then +1  else 0 end ) as eCommerce_Volume,
 --Totals
sum(case when b.[Group]     in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as eCommerce_Totals
from dbo.TRANSLOG_TRANSACT_2016 a
join abc096.MIDs b on a.MID = b.MID
where --a.RESPKIND='OK' and 
(a.MID like '%0000838%' or a.MID = '000000160000001') and a.MID <> '000000083890000' 
--and month(DTSTAMP) ='1'
union all
---- 2017
 select  '2017 VIVA Stats',
  --Volume
sum(case when b.[Group]     in ('VIVA eCommerce') then +1  else 0 end ) as eCommerce_Volume,
 --Totals
sum(case when b.[Group]     in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as eCommerce_Totals
from dbo.TRANSLOG_TRANSACT_2017 a
join abc096.MIDs b on a.MID = b.MID
where --a.RESPKIND='OK' and 
(a.MID like '%0000838%' or a.MID = '000000160000001') and a.MID <> '000000083890000' 


----    POS
---- 2016
 select  '2016 VIVA Stats',
 --Volume
sum(case when b.[Group] not in ('VIVA eCommerce') then +1  else 0 end ) as POS_Volume,
 --Totals
sum(case when b.[Group] not in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as POS_Value
from dbo.TRANSLOG_TRANSACT_2016 a
join abc096.MIDs b on a.MID = b.MID
where --a.RESPKIND='OK' and 
(a.MID like '%0000838%' or a.MID = '000000160000001') and a.MID <> '000000083890000' 
--and month(DTSTAMP) ='1'
union all
---- 2017
 select  '2017 VIVA Stats',
 --Volume
sum(case when b.[Group] not in ('VIVA eCommerce') then +1  else 0 end ) as POS_Volume,
 --Totals
sum(case when b.[Group] not in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as POS_Value
from dbo.TRANSLOG_TRANSACT_2017 a
join abc096.MIDs b on a.MID = b.MID
where --a.RESPKIND='OK' and 
(a.MID like '%0000838%' or a.MID = '000000160000001') and a.MID <> '000000083890000' 




-------- NOT USED
---- 2016
 select  '2016 VIVA Stats',
 --Totals
sum(case when b.[Group] not in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as POS_Totals, 
sum(case when b.[Group]     in ('VIVA eCommerce') then +TAMOUNT else 0  end ) as eCommerce_Totals,
--Volume
sum(case when b.[Group] not in ('VIVA eCommerce') then +1  else 0 end ) as POS_Volume, 
sum(case when b.[Group]     in ('VIVA eCommerce') then +1  else 0 end ) as eCommerce_Volume
from dbo.TRANSLOG_TRANSACT_2016 a
join abc096.MIDs b on a.MID = b.MID
where --a.RESPKIND='OK' and 
(a.MID like '%0000838%' or a.MID = '000000160000001') and a.MID <> '000000083890000' 
--and month(DTSTAMP) ='1'



-- 2017
select  '2017 VIVA Stats',
sum(case when b.[Group] in 
('VIVA Gaming'
,'VIVA Hotels'
,'VIVA Non Profit'
,'VIVA Partners'
,'VIVA Retail'
,'VIVA Services '
,'VIVA Taxis'
,'VIVA Telecoms'
,'VIVA Tickets'
,'VIVA Travel'
,'VIVA Utility Bills') then +TAMOUNT
else 0 
end ) as POS_Totals, 
sum(case when b.[Group]  in 
('VIVA eCommerce') then +TAMOUNT 
else 0 
end ) as eCommerce_Totals,

sum(case when b.[Group] in 
('VIVA Gaming'
,'VIVA Hotels'
,'VIVA Non Profit'
,'VIVA Partners'
,'VIVA Retail'
,'VIVA Services '
,'VIVA Taxis'
,'VIVA Telecoms'
,'VIVA Tickets'
,'VIVA Travel'
,'VIVA Utility Bills') then +1 
else 0 
end ) as POS_Volume, 
sum(case when b.[Group]  in 
('VIVA eCommerce') then +1 
else 0
end ) as eCommerce_Volume
from dbo.TRANSLOG_TRANSACT_2017 a
join abc096.MIDs b on a.MID = b.MID
where a.RESPKIND = 'OK' and a.MID like '%0000838%' and a.MID <> '000000083890000'