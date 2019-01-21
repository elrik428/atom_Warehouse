select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency,
                                                count(*) as ALL_TRN,
                                                sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,
                                                sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,
                                                sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,
  (replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted,
(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted
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
