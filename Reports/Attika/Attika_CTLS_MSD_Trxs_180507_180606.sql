---- No CTLS
select  'All Trxs',  '07/05/2018 - 06/06/2018',b.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from abc096.IMP_TRANSACT_D_tempLN a
join abc096.mids b on a.MID = b.MID
where a.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')
 --and MONTH(a.DTSTAMP) in('4')
group by a.MID,b.Merchant

---- No CTLS /  MSD
select  'Mastercard ONLY','07/05/2018 - 06/06/2018',b.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from abc096.IMP_TRANSACT_D_tempLN a
join abc096.mids b on a.MID = b.MID
where a.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')
and substring(a.MASK,1,2) between '50' and '59'
--and MONTH(a.DTSTAMP) in('4')
group by a.MID,b.Merchant

 ---- CTLS
select  'Contacteless Trxs','07/05/2018 - 06/06/2018',b.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from abc096.IMP_TRANSACT_D_tempLN a
join abc096.mids b on a.MID = b.MID
where a.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')
--and MONTH(a.DTSTAMP) in('4')
and  substring(POSDATA,1,2) in ('07','91')
group by a.MID,b.Merchant

---- CTLS /  MSD
select  'Mastercard Contacteless Trxs ONLY','07/05/2018 - 06/06/2018',b.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from abc096.IMP_TRANSACT_D_tempLN a
join abc096.mids b on a.MID = b.MID
where a.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')
and substring(a.MASK,1,2) between '50' and '59'
--and MONTH(a.DTSTAMP) in('4')
and  substring(POSDATA,1,2) in ('07','91')
group by a.MID,b.Merchant
