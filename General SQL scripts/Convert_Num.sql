select 'Totals',
count(*)   as ALL_TRN,
replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,
replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,
replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,
replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted,

count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,


sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
								 sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted ,
(convert(varchar,(convert(money,(sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as Eligible,
(convert(varchar,(convert(money,(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as DCC_Accepted,
(convert(varchar,(convert(money,(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as DCC_Not_Accepted ,



replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') ,
--as Eligible,

convert(varchar,(convert(money,(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)),1)),1) , --as DCC_Accepted,
convert(varchar,(convert(dec(15,0),(convert(money,(convert(decimal(15,0),(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)))),1)))),1)     as DCC_Not_Accepted,
								 sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
(convert(varchar,(convert(money,(sum(cast((merchant_amount/100) as dec(15,2)))),1)),1) + '$') as ALL_AMNT,

								 sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
(convert(varchar,(convert(money,(sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as Eligible,
								 sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
(convert(varchar,(convert(money,(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as DCC_Accepted,
								 sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted  ,
(convert(varchar,(convert(money,(sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end)),1)),1) + '$') as DCC_Not_Accepted

from attikaw
where
(
(TID in ('GR73000005','GR73000038')
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
 ) 
