---- No CTLS
select  'CTLS:NO /  MSD:NO',  '01/2017 - 12/2017',a.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount
from
	(select b.mid, b.Merchant,c.AMOUNT, substring(c.mask,1,2) twoplace, substring(c.mask,1,3) threeplace,substring(c.mask,1,4) fourplace,substring(c.mask,1,6) sixplace
	 from abc096.IMP_TRANSACT_D_2017 c
	 join abc096.mids b on c.MID = b.MID
     where c.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')) a
where
(  a.sixplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.fourplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.threeplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and a.twoplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
)
group by a.MID,a.Merchant

---- No CTLS /  MSD
select  'CTLS:NO /  MSD:YES','01/2017 - 12/2017',a.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from
	(select b.mid, b.Merchant,c.MASK,c.AMOUNT, substring(c.mask,1,2) twoplace, substring(c.mask,1,3) threeplace,substring(c.mask,1,4) fourplace,substring(c.mask,1,6) sixplace
	 from abc096.IMP_TRANSACT_D_2017 c
	 join abc096.mids b on c.MID = b.MID
     where c.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')) a
where
(  a.sixplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.fourplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.threeplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and a.twoplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
)
and substring(a.MASK,1,2) between '50' and '59'
--and MONTH(a.DTSTAMP) in('4')
group by a.MID,a.Merchant


 ---- CTLS
select  'CTLS:YES /  MSD:NO','01/2017 - 12/2017',a.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from
(select b.mid, b.Merchant,c.MASK,c.POSDATA,c.AMOUNT, substring(c.mask,1,2) twoplace, substring(c.mask,1,3) threeplace,substring(c.mask,1,4) fourplace,substring(c.mask,1,6) sixplace
	 from abc096.IMP_TRANSACT_D_2017 c
	 join abc096.mids b on c.MID = b.MID
     where c.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')) a
where
(  a.sixplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.fourplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.threeplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and a.twoplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
)
and  substring(POSDATA,1,2) in ('07','91')
group by a.MID,a.Merchant


---- CTLS /  MSD
select  'CTLS:YES /  MSD:YES','01/2017 - 12/2017',a.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount from
(select b.mid, b.Merchant,c.MASK,c.POSDATA,c.AMOUNT, substring(c.mask,1,2) twoplace, substring(c.mask,1,3) threeplace,substring(c.mask,1,4) fourplace,substring(c.mask,1,6) sixplace
	 from abc096.IMP_TRANSACT_D_2017 c
	 join abc096.mids b on c.MID = b.MID
     where c.MID in ('000000001100001','000000001100010','000000001100017','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','B00000001100001','B00000001100010','B00000001100011','000000001100018','000000001100019')) a
where
(  a.sixplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.fourplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and  a.threeplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
and a.twoplace not in (select binlower from [dbo].[MERCHBINS_lntemp] )
)
and substring(a.MASK,1,2) between '50' and '59'
--and MONTH(a.DTSTAMP) in('4')
and  substring(POSDATA,1,2) in ('07','91')
group by a.MID,a.Merchant
