
----- MAIN SQL for TOTALS per SHOP per BANK per BRAND

SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED,
ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, 
ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_,
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
--and a.PROCESSED = 'ALPHA'
 --and a.PROCESSED = 'ETHNIKI'
GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION
order by PROCESSED


---- Insert data to amounts table
insert into dbo.Chalkiadakis_dailyValues
SELECT v.MERCHTITLE,v.MERCHADDRESS,a.Shop,v.STORE_CODE,v.BANK,v.DMID,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,
--'','','','','','','','','','','','','','','','','','','','',
--'','','','','','','','','','','','','','','','','','','','',
--'','','','','','','',
sum(a.Ποσό) Summary_Amount,
count(*) Totals
FROM [ZacReporting].[dbo].[ePOSBatchRep] a
--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
join (select DMID, MERCHADDRESS,MERCHTITLE, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS,MERCHTITLE, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
and a.DMID = v.DMID
where a.MID = '000000120004000' and 
--shop like ('%KALOKAIRI%') and 
a.Κατηγορία= 'Εντός Πακέτου' 
--and a.PROCESSED = 'ALPHA'
 GROUP BY v.STORE_CODE, a.Shop,v.STORE_CODE,v.MERCHTITLE,v.MERCHADDRESS,v.DMID,v.BANK




--- TOTALS
select r.Shop, sum(r.net_Value), sum(r.commis_),sum(r.Summary_Amount) , sum(r.Totals) from 
   (SELECT  v.STORE_CODE, a.Shop,a.Brand,a.PROCESSED, count(*) Totals, sum(a.Ποσό) Summary_Amount,ROUND(((sum(a.Ποσό) - ((sum(a.Ποσό)*v.COMMISION)/100))) ,2 ) net_Value, ROUND(((sum(a.Ποσό)*v.COMMISION)/100),2) commis_
	FROM [ZacReporting].[dbo].[ePOSBatchRep] a
	--JOIN (select max(dmid) DMID, MERCHADDRESS, STORE_CODE from abc096.MERCHANTS_COMM group by MERCHADDRESS, STORE_CODE) b  on a.DMID = b.DMID 
	join (select DMID, MERCHADDRESS, BANK,brand,COMMISION, STORE_CODE from abc096.MERCHANTS_COMM group by DMID, MERCHADDRESS, BANK,brand,COMMISION,STORE_CODE) v on a.Brand = v.BRAND and a.PROCESSED = v.BANK 
	and a.DMID = v.DMID
	where a.MID = '000000120004000' and 
	shop like ('%KALOKAIRI%') and	
	a.Κατηγορία= 'Εντός Πακέτου'	 
	and a.PROCESSED = 'ETHNIKI'
	--and a.PROCESSED = 'ETHNIKI'
	GROUP BY v.STORE_CODE, a.Shop, a.Brand,a.PROCESSED,v.COMMISION) r
	group by r.Shop
