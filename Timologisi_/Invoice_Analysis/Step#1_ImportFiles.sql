-- IMPORT Pgms

-- Import Translog Transact
--Clear files
Delete from [dbo].[TRANSLOG_TRANSACT]
Delete from [dbo].[VASILOPOULOS_CITYLTY_TEMP]
Delete from [dbo].[VEROPOULOS_EBNKLTY_TEMP]
Delete from [dbo].[VASILOPOULOS_MEALS_TEMP]
Delete from [dbo].[CHALKIADAKIS_EXCLUDED]

--Delete loyalty transactions for Veropoulos
UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET MID='E'+right(MID,14)
where [mid] like '0000001200038%' and DESTCOMID='NET_EBLY1'

--Delete meals & more transactions for Vasilopoulos
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET MID='E'+right(MID,14)
 where [mid]='000000150000001' and mask like '502259%'

--Delete excluded transactions for Chalkiadakis
UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET MID='E'+right(MID,14)
where mid='000000120004000' and  [DESTCOMID]<>'NET_PBGLTY' and ([DESTCOMID]='NET_EBLY1' or tamount=0 or respkind<>'OK')

--Delete excluded transactions for NOTOS (1 lty per transaction)
UPDATE dbo.TRANSLOG_TRANSACT SET MID='E'+right(MID,14) 
where mid like '0000001100002%' and destcomid='NET_EBLY1' and proccode<>'5W0000'

--Delete excluded transactions for OTE (1 lty per transaction)
UPDATE dbo.TRANSLOG_TRANSACT SET MID='E'+right(MID,14) 
where mid like '00000017%' and destcomid='NET_EBLY1' and proccode<>'5W0000'

--Delete excluded transactions for AB.
UPDATE dbo.TRANSLOG_TRANSACT SET MID='E'+right(MID,14) --20160801 AB exclude declined transaction, not apply to franchisees
where mid in ('000000150000001','000000150000011') and (tamount=0 or respkind<>'OK')

--Update OPAP
UPDATE dbo.TRANSLOG_TRANSACT --20161003 OPAP
SET MID='OPAP' where DTID like 'O%'

--Delete reversals
delete from [ZacReporting].[dbo].[TRANSLOG_TRANSACT] where msgid like '04%'

--Delete inquiries.
delete from [ZacReporting].[dbo].[TRANSLOG_TRANSACT] where msgid='0100' and proccode ='080000'

--Delete DCC inquiries.
delete from [ZacReporting].[dbo].[TRANSLOG_TRANSACT] where msgid='0200' and proccode ='890000'

--Fix Hellaspay tids
UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET [TID] = right (MID,8)  WHERE mid like '0000000838%'

--Fix Originator
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set originator='DIAL_03' where originator='win'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set originator='DIAL_01' where originator='pos'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set originator='NET_DCEB' where originator='cle'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set originator='NET_DCAB' where originator='cla'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set originator='NET_PIR' where originator='clp'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set tid =dtid where originator='NET_DCAB'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set tid =dtid where originator='NET_DCEB'

--Fix MID TID for cardlink
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set mid =right(dmid,9) where originator='NET_DCAB'
update [ZacReporting].[dbo].[TRANSLOG_TRANSACT] set mid =right(dmid,9) where originator='NET_DCEB'

--Update BIC ports.
UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET [DESTCOMID]='NET_ALPHA' where [DESTCOMID]='NET_BICALPHA'
UPDATE [ZacReporting].[dbo].[TRANSLOG_TRANSACT] SET [DESTCOMID]='NET_EBNK' where [DESTCOMID]='NET_CLBICEBNK'


-- Import IMP_MONTHLY
-- Clear file
Delete from [abc096].[IMP_TRANSACT_D_monthly]

--Updated reversed and deleted reversals from IMP_TRANSACT_D.
update [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] 
set amount =0 where msgid='0200'  
and REVERSED='F' 
and exists 
(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]  B where b.msgid='0400' and amount=b.amount and 
mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='A') 

delete from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]  where msgid='0400'  
and exists 
(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]  B where b.msgid<>'0400' 
and mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='F')

--Update BIC ports
UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] SET [DESTCOMID]='NET_ALPHA' where [DESTCOMID]='NET_BICALPHA'
UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] SET [DESTCOMID]='NET_EBNK' where [DESTCOMID]='NET_CLBICEBNK'

--Delete inquiries
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] where msgid='0100' and proccode ='080000'

--Delete DCC inquiries
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] where msgid='0200' and proccode ='890000'

--Update Go4More and Yellow
update [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] set msgid='0200',proccode ='000001' 
where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='000000'

update [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] set msgid='0200',proccode ='020001' 
where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='020000'

--delete null pan
delete from [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]  where mask is null or mask='' 
update [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly]  set mask='000000' where productid is null 

--Fixed product in IMP_TRANSACT_D
update [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] set productid = 0

update zacreporting.[abc096].[IMP_TRANSACT_D_monthly] 
set productid = ( select   top 1 b.id 
from  zacreporting.abc096.Products b   
where substring(mask,1,6) = b.bin )

update zacreporting.[abc096].[IMP_TRANSACT_D_monthly]     
             set productid =      
             case     
                 when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                                     inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                                     group by b.id,b.bin)     
                 when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                                     inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                                     group by b.id,b.bin)     
                 when substring(mask,1,2) = '23'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '24'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '25'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '26'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '30'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                              group by b.id,b.bin)     
                 when substring(mask,1,2) = '34'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '35'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '36'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '37'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '38'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '50'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '51'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '52'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '53'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '54'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '55'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '56'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '57'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,2) = '58'	 then  (select b.id from zacreporting.[abc096].[IMP_TRANSACT_D_monthly] c     
                                                             inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin     
                                                             group by b.id,b.bin)     
                 when substring(mask,1,1) = '0'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '1'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '2'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '3'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '4'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '4'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '5'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '6'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '6'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '7'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '8'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 when substring(mask,1,1) = '9'   then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c      
                                                            inner join zacreporting.abc096.Products b on b.bin = '0'         
                                                            group by b.id,b.bin)                                             
                 end     
             where productid = 0   ;



-- Import LIFECARD 

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

-- Insert table to abc096.LIFECARD after above steps