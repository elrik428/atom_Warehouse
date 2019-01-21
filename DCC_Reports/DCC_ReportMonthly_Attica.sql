
-- Attica
-- 1.YTD
-- 1a.
select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2),
replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,
replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,
replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,
replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted
from attikaw
where
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 )
 group by left(datum,10)
 order by left(datum,10)

 -- 1b.
 select 'Totals',
replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,
replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,
replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,
replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted
from attikaw
where
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 )

 -- 1c.
 select  ' ',' ' ,
convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' ,
convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%',
' ',' ',
convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%',
convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' '
from
(
	select
	count(*) as All_trxs,
	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,
	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,
	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,
	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt
	from attikaw
	where (
	(TID in ('GR73000005','GR73000038')
	and datum>='2017-10-13 00:00:01') or
	(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
	and datum>='2017-11-20 00:00:01')
	)
)q

-- 2. PerTID
select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted
from attikaw
where
datum > '2018-" + file_Arr[2] + "-01 00:00:00' and
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 )
group by  right(tid,8),left(datum,10),dcc_currency
order by  right(tid,8),left(datum,10),dcc_currency

-- 3. Details
select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted
from attikaw
where
datum > '2018-" + file_Arr[2] + "-01 00:00:00' and
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 )
group by  right(tid,8),left(datum,10),dcc_currency
order by  right(tid,8),left(datum,10),dcc_currency


-- 4. PerArea
select substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) as Transaction_Date,
substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) +' ' + substring(RIGHT(datum,8),1,5) As Transaction_TimeStamp,
right(TID,8) as TID_, vispan as PAN,
(replace((cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2))),'.',',')) as  Original_Amount,
(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,
(replace((case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(dcc_amount as dec(15,0))/100) as dec(15,2)) else 0 end),'.',',')) as DCC_AMOUNT,
(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN,
left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN
 from attikaw
 where
datum > '2018-" + file_Arr[2] + "-01 00:00:00' and
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 )
 order by left(datum,10),Datum,TID
