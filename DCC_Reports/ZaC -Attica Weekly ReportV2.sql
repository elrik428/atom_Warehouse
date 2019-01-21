-- CREATE TABLE [dbo].[ATTIKAW](
--      [datum] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [tid] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [vispan] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [cardtyp] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [STAN] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [merchant_amount] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [merchant_currency] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [dcc_currency] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [dcc_amount] [nvarchar](100) COLLATE Greek_CI_AS NULL ,
--      [DCCCHOSEN_DCCELIGIBLE] [nvarchar](100) COLLATE Greek_CI_AS NULL 
-- ) ON [PRIMARY]
-- 
-- go


--YTD Report
select left(datum,10),
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
--sum(cast(merchant_amount as dec(15,2))) as ALL_AMNT3,
--sum(merchant_amount/100) as ALL_AMNT4,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted
from attikaw
where (
(TID in ('GR73000005','GR73000038') 
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
-- new pad to be inserted
)
-- new pad to be inserted
group by left(datum,10)
order by left(datum,10)


--Per TID-Day-Currency
select right(tid,8),left(datum,10),dcc_currency,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
--sum(cast(merchant_amount as dec(15,2))) as ALL_AMNT3,
--sum(merchant_amount/100) as ALL_AMNT4,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted
from attikaw
where 
--===========================================================================================================================================================================
datum > '2018-02-01 00:00:00' and
--===========================================================================================================================================================================
(
(TID in ('GR73000005','GR73000038') 
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
-- new pad to be inserted
)
group by  right(tid,8),left(datum,10),dcc_currency
order by  right(tid,8),left(datum,10),dcc_currency
;


--Details
select left(datum,10) as Transaction_Date, Datum As Transaction_TimeStamp, right(TID,8) as TID_, vispan as PAN,
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) as Original_Amount,
(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,
(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_AMOUNT,
(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN,
left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN
 from attikaw
 where 
 --===========================================================================================================================================================================
datum > '2018-02-01 00:00:00' and
--===========================================================================================================================================================================
(
(TID in ('GR73000005','GR73000038') 
and datum>='2017-10-13 00:00:01') or
(TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
-- new pad to be inserted
)
--and datepart(year, datum) >2015 and datepart(week, datum) >= (select max(datepart(week, datum))-5 from attikaw where datepart(year, datum) >2015)
 order by left(datum,10),Datum,TID
;


--Per AREA-Day
select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,10) as Date,
count(*) as ALL_TRN,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
--sum(cast(merchant_amount as dec(15,2))) as ALL_AMNT3,
--sum(merchant_amount/100) as ALL_AMNT4,
sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then
cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted
from attikaw a, abc096.merchants b
where
--===========================================================================================================================================================================
datum > '2018-02-01 00:00:00' and
--===========================================================================================================================================================================
right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and
(
(a.TID in ('GR73000005','GR73000038') 
and datum>='2017-10-13 00:00:01') or
(a.TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
and datum>='2017-11-20 00:00:01')
-- new pad to be inserted
)
group by  merchaddress,left(datum,10)
order by  merchaddress,left(datum,10)
;

-- --------------------------------------
-- ------------------------------------
-- --Per AREA-Day ED.Foteinos
-- select right(a.TID,8), merchaddress, 
-- --count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, 
-- left(datum,10) as Date,
-- count(*) as ALL_TRN,
-- sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
-- sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
-- --sum(cast(merchant_amount as dec(15,2))) as ALL_AMNT3,
-- --sum(merchant_amount/100) as ALL_AMNT4,
-- sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted
-- from attikaw a, abc096.merchants b
-- where
-- --===========================================================================================================================================================================
-- datum > '2018-01-01 00:00:00' and
-- --===========================================================================================================================================================================
-- right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and
-- (
-- (a.TID in ('GR73000005','GR73000038') 
-- and datum>='2017-10-13 00:00:01') or
-- (a.TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
-- and datum>='2017-11-20 00:00:01')
-- -- new pad to be inserted
-- )
-- group by  right(a.TID,8),merchaddress, left(datum,10)
-- order by  right(a.TID,8),merchaddress, left(datum,10)
-- ;
-- 
-- --------------------------------------
-- ------------------------------------
-- --Per AREA-Day ED.Foteinos
-- select right(a.TID,8), merchaddress, 
-- --count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, 
-- --left(datum,10) as Date,
-- count(*) as ALL_TRN,
-- sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
-- sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,
-- --sum(cast(merchant_amount as dec(15,2))) as ALL_AMNT3,
-- --sum(merchant_amount/100) as ALL_AMNT4,
-- sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted,
-- sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then
-- cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Not_Accepted
-- from attikaw a, abc096.merchants b
-- where
-- --===========================================================================================================================================================================
-- datum > '2017-10-01 00:00:00' and
-- --===========================================================================================================================================================================
-- right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and
-- (
-- (a.TID in ('GR73000005','GR73000038') 
-- and datum>='2017-10-13 00:00:01') or
-- (a.TID in ('GR73000022','GR73000093','GR73000097','GR73000171','GR73000217','GR73002538','GR73007520','GR73007521')
-- and datum>='2017-11-20 00:00:01')
-- -- new pad to be inserted
-- )
-- group by  right(a.TID,8),merchaddress
-- order by  right(a.TID,8),merchaddress
-- ;
-- 
-- 

