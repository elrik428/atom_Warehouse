select MONTH(datum) from dbo.RVUW
group by MONTH(datum)

select MONTH(datum) from dbo.RegencyW
where datum > '2019-02-01 00:00:00'
group by MONTH(datum)

select * from dbo.RVUW
order by datum

select * from
(
select datum, vispan ,merchant_amount, merchant_currency  from dbo.RVUW
where datum > '2019-06-01 00:00:00'
group by datum, vispan ,merchant_amount, merchant_currency
having count(*) > 1
) as q
join dbo.RVUW a on a.datum = q.datum and a.vispan = q.vispan
order by a.datum, a.vispan ,a.merchant_amount, a.merchant_currency


select * from
(
select datum, vispan ,merchant_amount, merchant_currency  from dbo.RegencyW
where datum > '2019-02-01 00:00:00'
group by datum, vispan ,merchant_amount, merchant_currency
having count(*) > 1
) as q
join dbo.RVUW a on a.datum = q.datum and a.vispan = q.vispan
order by a.datum, a.vispan ,a.merchant_amount, a.merchant_currency


select min(datum),max(datum) from dbo.RegencyW
where datum > '2019-02-01 00:00:00'
group by MONTH(datum)



