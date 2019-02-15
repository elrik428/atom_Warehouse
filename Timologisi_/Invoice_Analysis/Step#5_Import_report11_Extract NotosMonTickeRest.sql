-- Import report11 

-- Clear file
Delete from [abc096].[IMP_TRANSACT_D]

--Updated reversed and deleted reversals from IMP_TRANSACT_D
update [ZacReporting].[abc096].[IMP_TRANSACT_D] 
set amount =0 where msgid='0200'  
and REVERSED='F' 
and exists 
(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid='0400' and amount=b.amount and 
mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='A') 

delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where msgid='0400'  
and exists 
(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid<>'0400' 
and mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='F')

--Update BIC ports
UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] SET [DESTCOMID]='NET_ALPHA' where [DESTCOMID]='NET_BICALPHA'
UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] SET [DESTCOMID]='NET_EBNK' where [DESTCOMID]='NET_CLBICEBNK'

--Delete inquiries
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D] where msgid='0100' and proccode ='080000'

--Delete DCC inquiries
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D] where msgid='0200' and proccode ='890000'

--Update Go4More and Yellow
update [ZacReporting].[abc096].[IMP_TRANSACT_D] set msgid='0200',proccode ='000001' 
where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='000000'

update [ZacReporting].[abc096].[IMP_TRANSACT_D] set msgid='0200',proccode ='020001' 
where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='020000'

--delete null pan
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where mask is null or mask='' 
update [ZacReporting].[abc096].[IMP_TRANSACT_D]  set mask='000000' where productid is null 

--remove AEGEAN current day
delete FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]where mid='000000120004600' and bdtstamp>'"+date+" 23:59:59.000'

--Fix product in IMP_TRANSACT_D
update [ZacReporting].[abc096].[IMP_TRANSACT_D] set productid = 0

update zacreporting.[abc096].[IMP_TRANSACT_D] 
set productid = ( select   top 1 b.id 
from  zacreporting.abc096.Products b   
where substring(mask,1,6) = b.bin )

update zacreporting.[abc096].[IMP_TRANSACT_D]     
             set productid =      
             case     
                 when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                                     inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                                     group by b.id,b.bin)     
                 when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                                     inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                                     group by b.id,b.bin)     
                 when substring(mask,1,2) = '23'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '24'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '25'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '26'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '30'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                              group by b.id,b.bin)     
                 when substring(mask,1,2) = '34'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '35'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '36'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '37'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '38'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '50'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '51'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '52'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '53'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '54'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '55'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '56'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '57'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '58'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,1) = '0'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '1'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '2'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '3'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '4'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '4'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '5'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '6'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '6'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '7'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '8'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '9'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 end     
             where productid = 0   ;


-- SQL for report NOTOSMonthTicketRest export
SELECT 
 QePOSBatchReports.[Group],
 QePOSBatchReports.Ημερομηνία,
 QePOSBatchReports.Κατηγορία,
 QePOSBatchReports.ΠακέτοePOS,
 QePOSBatchReports.Πακέτο, 
 QePOSBatchReports.Κάρτα, 
 QePOSBatchReports.Brand, 
 QePOSBatchReports.Πληκτρ, 
 QePOSBatchReports.Τύπος, 
 QePOSBatchReports.Ποσό, 
 QePOSBatchReports.Δόσεις, 
 QePOSBatchReports.Συναλλαγή, 
 QePOSBatchReports.Απόκριση, 
 QePOSBatchReports.Αντιλογισμός, 
 QePOSBatchReports.PROCESSED, 
 QePOSBatchReports.Shop, 
 QePOSBatchReports.Mid, 
 QePOSBatchReports.TID, 
 QePOSBatchReports.DMID, 
 QePOSBatchReports.DTID, 
 QePOSBatchReports.ePOSBATCH, 
 QePOSBatchReports.PROCBATCH
FROM
(SELECT dataentry,[Group], dtstamp  Ημερομηνία,PROCBATCH,
   case
    when reversed = 'A' or reversed = 'F'
    then 'Εκτός Πακέτου'
    else (case
     when ePOSBATCH IS null
           then
               (case when PROCBATCH is Null
               then'Εκτός Πακέτου'
               else 'Εντός Πακέτου'
         end)
           else 'Εντός Πακέτου' end
  ) end as Κατηγορία,
 case
 when reversed ='A' or reversed ='F'
 then 'Εκτός Πακέτου EURONET'
 else
  (case
    when (ePOSBATCH IS NULL)
    then
        (case
          when (PROCBATCH  IS NULL)
          then 'Εκτός Πακέτου EURONET'
          else 'Εντός Πακέτου EURONET'
          end)
      else  'Εντός Πακέτου EURONET' end)
           + (
		   case when ePOSBATCH IS NULL then ' ' else cast(ePOSBATCH as char) end)

      end as PacakageePOS ,
   dmid + ' / ' + dtid + case
            when reversed = 'A' or reversed ='F'
                         then ' '
                         else
                           (case
               when procbatch is Null
                             then ' '
                             else
                             ' / ' + cast(procbatch as char)  end
                           ) end as   Πακέτο,

   case when dataentry = 'True'
   then 'T' + cast(MASK as char)
   else
   'D' + cast(MASK as char)
   end as  Κάρτα,

   case when cast(dataentry as char) = 'ΠΛΗΚΤΡ'
   then ' '
   end  as Πληκτρ,

   case when bank is Null
   then Brand
   else bank + '/' + product
   end  as Τύπος,

   Amount   Ποσό,
    INST   Δόσεις,
   rtrim (msg + ' ' + act) Συναλλαγή,
    RESPKIND   Απόκριση,
    REVERSED   Αντιλογισμός,
     dtstamp Ώρα,
     TID, PBank  PROCESSED,
      Shop, MID, DMID, DTID, ePOSBATCH, Brand, BTCODE,

      ORGSYSTAN, DTSTAMP_INSERT, BORGSYSTAN, AUTHCODE, CASHIERINFO, TrDMID, TrDTID,
       trdmid + ' / ' + TrDTID + case when reversed ='A' or reversed = 'F'
                              then ' '
                              else (case when procbatch is Null
                                          then ' '
                                        else
                                        ' / ' + cast(procbatch as char)  end  )
                                        end as TrΠακέτο,
  case
        when LEFT(PEM,2)='01' then 'MANUAL ENTRY'
        when LEFT(PEM,2)='02' then 'MAGNETIC STRIPE'
        when LEFT(PEM,2)='80' then 'MAGNETIC STRIPE'
        when LEFT(PEM,2)='05' then 'CHIP CONTACT'
        when LEFT(PEM,2)='07' then 'CONTACTLESS'
        when LEFT(PEM,2)='91' then 'CONTACTLESS'
      end as  PEM_DESC,

      case
        when   BONUSRED > 0 then BONUSRED
        end as BONUS_Redemption

 FROM dbo.VTransactions
 --where [Group] = 'NOTOSMonthTicketRest'
 --ORDER BY dtstamp, PBank
 ) QePOSBatchReports
WHERE QePOSBatchReports.[Group] like "NOTOSMonthTicketRest%"
ORDER BY QePOSBatchReports.Ημερομηνία, QePOSBatchReports.Ώρα