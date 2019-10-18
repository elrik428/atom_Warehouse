select month(dtstamp),
(case month(dtstamp)
when '01' then 'January'
when '02' then 'February'
when '03' then 'March'
when '04' then 'April'
when '05' then 'May'
when '06' then 'June'
when '07' then 'July'
when '08' then 'August'
when '09' then 'September'
when '10' then 'October'
when '11' then 'November'
when '12' then 'December'
ELSE 'UNKNOWN'
END) as Month,
 count(*) from dbo.TRANSLOG_TRANSACT_2018
where mid in ('000000120002500','000000120002510','000000120002540','000000120002550','000000120002580')
and (month(dtstamp) >= '3' and month(dtstamp) <='8') and DESTCOMID<>'NET_NTBNLTY'
group by month(dtstamp)
order by month(dtstamp)
