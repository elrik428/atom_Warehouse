select month(dtstamp), count(*) from dbo.TRANSLOG_TRANSACT_2018
where mid in ('000000120002500','000000120002510','000000120002540','000000120002550','000000120002580')
and (month(dtstamp) >= '3' and month(dtstamp) <='8') and DESTCOMID<>'NET_NTBNLTY'
group by month(dtstamp)
order by month(dtstamp)
