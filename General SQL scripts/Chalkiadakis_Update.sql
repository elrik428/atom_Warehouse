update dbo.Chalkiadakis_dailyValues
set
       NetValue_VISA=b.net_Value
      ,ComValue_VISA=b.commis_
      ,TotValue_VISA=b.Summary_Amount
      ,CountTrxs_VISA=b.Totals
      
from
dbo.Chalkiadakis_dailyValues q
inner join
(SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
and a.PROCESSED = 'ETHNIKI' and a.Brand = 'VISA'
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
--order by PROCESSED
) b
on
q.Store_Code = b.STORE_CODE   and q.Bank = b.PROCESSED

update dbo.Chalkiadakis_dailyValues
set
       NetValue_MSD=b.net_Value
      ,ComValue_MSD=b.commis_
      ,TotValue_MSD=b.Summary_Amount
      ,CountTrxs_MSD=b.Totals
      
from
dbo.Chalkiadakis_dailyValues q
inner join
(SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
and a.PROCESSED = 'ETHNIKI' and a.Brand = 'MASTER'
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
--order by PROCESSED
) b
on
q.Store_Code = b.STORE_CODE   and q.Bank = b.PROCESSED

update dbo.Chalkiadakis_dailyValues
set
       NetValue_CUP=b.net_Value
      ,ComValue_CUP=b.commis_
      ,TotValue_CUP=b.Summary_Amount
      ,CountTrxs_CUP=b.Totals
      
from
dbo.Chalkiadakis_dailyValues q
inner join
(SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
and a.PROCESSED = 'ETHNIKI' and a.Brand = 'CHINA UNION PAY'
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
--order by PROCESSED
) b
on
q.Store_Code = b.STORE_CODE   and q.Bank = b.PROCESSED

update dbo.Chalkiadakis_dailyValues
set
       NetValue_MAE=b.net_Value
      ,ComValue_MAE=b.commis_
      ,TotValue_MAE=b.Summary_Amount
      ,CountTrxs_MAE=b.Totals
      
from
dbo.Chalkiadakis_dailyValues q
inner join
(SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
and a.PROCESSED = 'ETHNIKI' and a.Brand = 'MAESTRO'
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
--order by PROCESSED
) b
on
q.Store_Code = b.STORE_CODE   and q.Bank = b.PROCESSED

update dbo.Chalkiadakis_dailyValues
set
       NetValue_OTHER1=b.net_Value
      ,ComValue_OTHER1=b.commis_
      ,TotValue_OTHER1=b.Summary_Amount
      ,CountTrxs_OTHER1=b.Totals
      
from
dbo.Chalkiadakis_dailyValues q
inner join
(SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
and a.PROCESSED = 'ETHNIKI' and a.Brand not in ('MASTER','VISA','MAESTRO','CHINA UNION PAY')
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
--order by PROCESSED
) b
on
q.Store_Code = b.STORE_CODE   and q.Bank = b.PROCESSEDs