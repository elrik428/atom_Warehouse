--1.
-- delete [dbo].[MERCHBINS_bup3]
-- -------------------------------------------------------------------
--2.
-- insert into [dbo].[MERCHBINS_bup3] (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
--   (select TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from merchbins)
-- -------------------------------------------------------check the insertion
--3.
-- select count(*) from merchbins
--4.
-- select count (*) from merchbins_bup2
--select * from merchbins where tid in ('73003497','73003515','73003516','73003517','73003753','73003763','73003764','73003765','73003766','73003767','73003768','73003769','73003770','73003771','73003772','73003773','73003774','73003775','73003776','73003777','73003778','73003779','73003780','73003781','73003782','73003783','73003787','73003788','73003789','73003790','73003799','73003800','73003801','73003802','73003803','73003804','73003809','73003810','73003811','73003812','73003813','73003814','73003815','73003816','73003817','73003818','73003843','73003844','73003845','73003846','73003847','73003848','73003849','73003850','73003851','73003852','73003853','73003854','73003855','73003856','73003857','73003858','73003859','73003860','73003861','73003862','73003863','73003864','73003865','73004052','73004053','73004054','73004055','73004056','73004057','73004058','73004059','73004060','73004061','73004062','73004063','73004064','73004111','73004112','73004113','73004114','73004115','73004116','73004117','73004118','73004119','73004120','73004121','73004122','73004123','73004124','73004125','73004135','73004136','73004137','73004138','73004139','73004140','73004141','73004142','73004143','73004149','73004150','73004151','73004152','73004157','73004158','73004159','73004160','73004183','73004184','73004185','73004186','73004187','73004188','73004189','73004190','73004191','73004192','73004193','73004194','73004195','73004196','73004197','73004198','73004199','73004200','73004201','73004202','73004203','73004204','73004205','73004206','73004207','73004208','73004213','73004214','73004215','73004216','73004217','73004218','73004219','73004220','73004221','73004235','73004236','73004237','73004238','73004239','73004240','73004269','73004270','73004271','73004272','73004273','73004279','73004280','73004281','73004282','73004295','73004296','73004297','73004298','73004341','73004342','73004343','73004344','73004345','73004367','73004368','73004369','73004370','73004371','73004372','73004373','73004374','73004375','73004376','73004377','73004378','73004379','73004380','73004381','73004382','73004383','73004384','73004385','73004386','73004387','73004388','73004389','73004390','73004395','73004396','73004397','73004398','73004399','73004400','73004401','73004402','73004403','73004404','73004423','73004424','73004425','73004426','73004427','73004428','73004429','73004430','73004431','73004432','73004433','73004434','73004476','73004477','73004478','73004479','73004480','73004485','73004486','73004487','73004488','73004558','73004559','73004560','73004561','73004562','73004563','73004564','73004587','73004588','73004589','73004590','73004613','73004614','73004615','73004616','73004617','73004635','73004639','73004652','73004653','73004654','73004655','73004656','73004657','73004658','73004659','73004660','73004661','73004662','73004663','73004664','73004665','73004666','73004673','73004674','73004675','73004676','73004677','73004687','73004688','73004689','73004690','73004691','73004700','73004701','73004702','73004703','73004713','73004723','73004736','73004737','73004741','73004752','73004756','73004759','73005198','73005199','73005368','73005369','73005403','73005428','73005429','73005430','73005782','73005789','73005790','73005853','73005854','73005856','73005859','73005860','73005861','73005862','73005879','73005890','73005891','73005892','73005893','73005894','73005895','73005896','73005897','73005898','73005900','73005901','73005936','73005941','73006004','73006173','73006752','73006753','73006754','73006755','73006756','73006757','73006758','73006759','73007429','73007430','73007431','73007432','73007433','73007434','73007435')

--5. delete old bins
--delete from merchbins where tid in ('73003497','73003515','73003516','73003517','73003753','73003763','73003764','73003765','73003766','73003767','73003768','73003769','73003770','73003771','73003772','73003773','73003774','73003775','73003776','73003777','73003778','73003779','73003780','73003781','73003782','73003783','73003787','73003788','73003789','73003790','73003799','73003800','73003801','73003802','73003803','73003804','73003809','73003810','73003811','73003812','73003813','73003814','73003815','73003816','73003817','73003818','73003843','73003844','73003845','73003846','73003847','73003848','73003849','73003850','73003851','73003852','73003853','73003854','73003855','73003856','73003857','73003858','73003859','73003860','73003861','73003862','73003863','73003864','73003865','73004052','73004053','73004054','73004055','73004056','73004057','73004058','73004059','73004060','73004061','73004062','73004063','73004064','73004111','73004112','73004113','73004114','73004115','73004116','73004117','73004118','73004119','73004120','73004121','73004122','73004123','73004124','73004125','73004135','73004136','73004137','73004138','73004139','73004140','73004141','73004142','73004143','73004149','73004150','73004151','73004152','73004157','73004158','73004159','73004160','73004183','73004184','73004185','73004186','73004187','73004188','73004189','73004190','73004191','73004192','73004193','73004194','73004195','73004196','73004197','73004198','73004199','73004200','73004201','73004202','73004203','73004204','73004205','73004206','73004207','73004208','73004213','73004214','73004215','73004216','73004217','73004218','73004219','73004220','73004221','73004235','73004236','73004237','73004238','73004239','73004240','73004269','73004270','73004271','73004272','73004273','73004279','73004280','73004281','73004282','73004295','73004296','73004297','73004298','73004341','73004342','73004343','73004344','73004345','73004367','73004368','73004369','73004370','73004371','73004372','73004373','73004374','73004375','73004376','73004377','73004378','73004379','73004380','73004381','73004382','73004383','73004384','73004385','73004386','73004387','73004388','73004389','73004390','73004395','73004396','73004397','73004398','73004399','73004400','73004401','73004402','73004403','73004404','73004423','73004424','73004425','73004426','73004427','73004428','73004429','73004430','73004431','73004432','73004433','73004434','73004476','73004477','73004478','73004479','73004480','73004485','73004486','73004487','73004488','73004558','73004559','73004560','73004561','73004562','73004563','73004564','73004587','73004588','73004589','73004590','73004613','73004614','73004615','73004616','73004617','73004635','73004639','73004652','73004653','73004654','73004655','73004656','73004657','73004658','73004659','73004660','73004661','73004662','73004663','73004664','73004665','73004666','73004673','73004674','73004675','73004676','73004677','73004687','73004688','73004689','73004690','73004691','73004700','73004701','73004702','73004703','73004713','73004723','73004736','73004737','73004741','73004752','73004756','73004759','73005198','73005199','73005368','73005369','73005403','73005428','73005429','73005430','73005782','73005789','73005790','73005853','73005854','73005856','73005859','73005860','73005861','73005862','73005879','73005890','73005891','73005892','73005893','73005894','73005895','73005896','73005897','73005898','73005900','73005901','73005936','73005941','73006004','73006173','73006752','73006753','73006754','73006755','73006756','73006757','73006758','73006759','73007429','73007430','73007431','73007432','73007433','73007434','73007435')

select * from merchbins where tid = '1111    ' and destport = 'NET_NTBN' order by destport,binlower
--6.
select * from merchbins where tid = '73006044' order by destport,binlower

select * from merchants where tid = '73006044'

-- ----------------------------------------------------------------------------
-- 7.
-- uploadhostid
-- 1 = Piraeus , NET_ABC
-- 6=ETHNIKI, NET_NTBN
-- 202=ALPHABANK, NET_CLBICALPHA
-- 205=EUROBANK, NET_CLBICEBNK
--
declare @tid varchar(16)
declare @name varchar(20)

declare merch_cursor cursor for
select  TID, uploadhostname from MERCHANTS a, uploadhosts b
where  tid in ('73003497','73003515','73003516','73003517','73003753','73003763','73003764','73003765','73003766','73003767','73003768','73003769','73003770','73003771','73003772','73003773','73003774','73003775','73003776','73003777','73003778','73003779','73003780','73003781','73003782','73003783','73003787','73003788','73003789','73003790','73003799','73003800','73003801','73003802','73003803','73003804','73003809','73003810','73003811','73003812','73003813','73003814','73003815','73003816','73003817','73003818','73003843','73003844','73003845','73003846','73003847','73003848','73003849','73003850','73003851','73003852','73003853','73003854','73003855','73003856','73003857','73003858','73003859','73003860','73003861','73003862','73003863','73003864','73003865','73004052','73004053','73004054','73004055','73004056','73004057','73004058','73004059','73004060','73004061','73004062','73004063','73004064','73004111','73004112','73004113','73004114','73004115','73004116','73004117','73004118','73004119','73004120','73004121','73004122','73004123','73004124','73004125','73004135','73004136','73004137','73004138','73004139','73004140','73004141','73004142','73004143','73004149','73004150','73004151','73004152','73004157','73004158','73004159','73004160','73004183','73004184','73004185','73004186','73004187','73004188','73004189','73004190','73004191','73004192','73004193','73004194','73004195','73004196','73004197','73004198','73004199','73004200','73004201','73004202','73004203','73004204','73004205','73004206','73004207','73004208','73004213','73004214','73004215','73004216','73004217','73004218','73004219','73004220','73004221','73004235','73004236','73004237','73004238','73004239','73004240','73004269','73004270','73004271','73004272','73004273','73004279','73004280','73004281','73004282','73004295','73004296','73004297','73004298','73004341','73004342','73004343','73004344','73004345','73004367','73004368','73004369','73004370','73004371','73004372','73004373','73004374','73004375','73004376','73004377','73004378','73004379','73004380','73004381','73004382','73004383','73004384','73004385','73004386','73004387','73004388','73004389','73004390','73004395','73004396','73004397','73004398','73004399','73004400','73004401','73004402','73004403','73004404','73004423','73004424','73004425','73004426','73004427','73004428','73004429','73004430','73004431','73004432','73004433','73004434','73004476','73004477','73004478','73004479','73004480','73004485','73004486','73004487','73004488','73004558','73004559','73004560','73004561','73004562','73004563','73004564','73004587','73004588','73004589','73004590','73004613','73004614','73004615','73004616','73004617','73004635','73004639','73004652','73004653','73004654','73004655','73004656','73004657','73004658','73004659','73004660','73004661','73004662','73004663','73004664','73004665','73004666','73004673','73004674','73004675','73004676','73004677','73004687','73004688','73004689','73004690','73004691','73004700','73004701','73004702','73004703','73004713','73004723','73004736','73004737','73004741','73004752','73004756','73004759','73005198','73005199','73005368','73005369','73005403','73005428','73005429','73005430','73005782','73005789','73005790','73005853','73005854','73005856','73005859','73005860','73005861','73005862','73005879','73005890','73005891','73005892','73005893','73005894','73005895','73005896','73005897','73005898','73005900','73005901','73005936','73005941','73006004','73006173','73006752','73006753','73006754','73006755','73006756','73006757','73006758','73006759','73007429','73007430','73007431','73007432','73007433','73007434','73007435')

and a.uploadhostid = '205'
and a.uploadhostid = b.uploadhostid

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid, @name

while @@FETCH_STATUS = 0
begin
 -- delete from MERCHBINS where TID = @tid and destport = 'NET_CLBICEBNK'
  --and len(binlower) > 3 and destport <> 'NET_CITI'

-- --kouvas
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '4', '6', 0, 1, 0, 99, 'Y', 0 , 999999999);
 --insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '9', '9', 0, 1, 0, 99, 'Y', 0 , 999999999);
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '222100', '272099', 0, 1, 0, 99, 'Y', 0 , 999999999);
--
-- --meals and more
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '502259', '502259', 0, 1, 0, 99, 'Y', 0 , 999999999);
--
-- -- ticket restaurant
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '534228', '534228', 0, 1, 0, 99, 'Y', 0 , 999999999);



--
  /* insert into MERCHBINS
  (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
  (
  select @TID, DESTPORT, BINLOWER, BINUPPER,
  --INSTMIN,
  --2
  0,
  --INSTMAX,
  1,
  GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from MERCHBINS
  where TID = '1111    ' and DESTPORT = 'NET_CLBICEBNK'
  --and binlower <> '549804'
  --    and binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')
)*/

  fetch next from merch_cursor
  into @tid, @name
end

CLOSE merch_cursor
deallocate merch_cursor

------------
-- find maxinst
select * from merchbins where tid  in (select tid from merchants where mid = ('000000120002500'))
--8.
-- maxinst = 1
select gmaxinst, * from merchants where mid = ('000000120002500')
update merchants set gmaxinst=1  where mid = ('000000120002500')
------------
---------------------------------------------------------------------
select * from merchbins where tid in (select tid from merchants where mid = '000000120002500') and tid ='73000228'
---------------------------------------------------------------------

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120002500' and tid not in
(select tid from merchants where mid = '000000120002500' and uploadhostid = 202)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120002500' and tid not in
(select tid from merchants where mid = '000000120002500' and uploadhostid = 205)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120002500' and tid not in
(select tid from merchants where mid = '000000120002500' and uploadhostid = 6)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120002500' and tid not in
(select tid from merchants where mid = '000000120002500' and uploadhostid = 1)

--9.
-------------------------------------
delete loadbalance where balancegroup in ('M73003511','M73003564','M73003565','M73003566','M73003567','M73003568','M73003569','M73003570','M73003571','M73003572','M73003573','M73003574','M73003580','M73003581','M73003582','M73003583','M73003584','M73003585','M73003586','M73003587','M73003588','M73003589','M73003590','M73003591','M73003592','M73003593','M73003594','M73003595','M73003596','M73003597','M73003598','M73003599','M73003600','M73003601','M73003602','M73003603','M73003604','M73003605','M73003606','M73003641','M73003642','M73003643')
---(select 'M'+tid from merchants where mid = '000000120002500')
--and destport = 'NET_GENIKI'
select * from loadbalance where balancegroup in ('M73003511','M73003564','M73003565','M73003566','M73003567','M73003568','M73003569','M73003570','M73003571','M73003572','M73003573','M73003574','M73003580','M73003581','M73003582','M73003583','M73003584','M73003585','M73003586','M73003587','M73003588','M73003589','M73003590','M73003591','M73003592','M73003593','M73003594','M73003595','M73003596','M73003597','M73003598','M73003599','M73003600','M73003601','M73003602','M73003603','M73003604','M73003605','M73003606','M73003641','M73003642','M73003643')
--(select 'M'+tid from merchants where mid = '000000120002500')
--and balancegroup ='M73000243'
order by balancegroup, DESTPORT
update loadbalance set LOADVALUE = 250000 where balancegroup in (select 'M'+tid from merchants where mid = '000000120002500') and destport = 'NET_NTBN'
update loadbalance set LOADVALUE = 1 where balancegroup in (select 'M'+tid from merchants where mid = '000000120002500') and destport = 'NET_ABC'
update loadbalance set LOADVALUE = 1 where balancegroup in (select 'M'+tid from merchants where mid = '000000120002500') and destport = 'NET_CLBICALPHA'
update loadbalance set LOADVALUE = 750000 where balancegroup in (select 'M'+tid from merchants where mid = '000000120002500') and destport = 'NET_CLBICEBNK'
-------------------------------------
--10.
declare @TID varchar(16), @BALGROUP varchar(20)

DECLARE merch_cursor CURSOR FOR
SELECT distinct(tid) FROM merchants WHERE  tid in ('73003497','73003515','73003516','73003517','73003753','73003763','73003764','73003765','73003766','73003767','73003768','73003769','73003770','73003771','73003772','73003773','73003774','73003775','73003776','73003777','73003778','73003779','73003780','73003781','73003782','73003783','73003787','73003788','73003789','73003790','73003799','73003800','73003801','73003802','73003803','73003804','73003809','73003810','73003811','73003812','73003813','73003814','73003815','73003816','73003817','73003818','73003843','73003844','73003845','73003846','73003847','73003848','73003849','73003850','73003851','73003852','73003853','73003854','73003855','73003856','73003857','73003858','73003859','73003860','73003861','73003862','73003863','73003864','73003865','73004052','73004053','73004054','73004055','73004056','73004057','73004058','73004059','73004060','73004061','73004062','73004063','73004064','73004111','73004112','73004113','73004114','73004115','73004116','73004117','73004118','73004119','73004120','73004121','73004122','73004123','73004124','73004125','73004135','73004136','73004137','73004138','73004139','73004140','73004141','73004142','73004143','73004149','73004150','73004151','73004152','73004157','73004158','73004159','73004160','73004183','73004184','73004185','73004186','73004187','73004188','73004189','73004190','73004191','73004192','73004193','73004194','73004195','73004196','73004197','73004198','73004199','73004200','73004201','73004202','73004203','73004204','73004205','73004206','73004207','73004208','73004213','73004214','73004215','73004216','73004217','73004218','73004219','73004220','73004221','73004235','73004236','73004237','73004238','73004239','73004240','73004269','73004270','73004271','73004272','73004273','73004279','73004280','73004281','73004282','73004295','73004296','73004297','73004298','73004341','73004342','73004343','73004344','73004345','73004367','73004368','73004369','73004370','73004371','73004372','73004373','73004374','73004375','73004376','73004377','73004378','73004379','73004380','73004381','73004382','73004383','73004384','73004385','73004386','73004387','73004388','73004389','73004390','73004395','73004396','73004397','73004398','73004399','73004400','73004401','73004402','73004403','73004404','73004423','73004424','73004425','73004426','73004427','73004428','73004429','73004430','73004431','73004432','73004433','73004434','73004476','73004477','73004478','73004479','73004480','73004485','73004486','73004487','73004488','73004558','73004559','73004560','73004561','73004562','73004563','73004564','73004587','73004588','73004589','73004590','73004613','73004614','73004615','73004616','73004617','73004635','73004639','73004652','73004653','73004654','73004655','73004656','73004657','73004658','73004659','73004660','73004661','73004662','73004663','73004664','73004665','73004666','73004673','73004674','73004675','73004676','73004677','73004687','73004688','73004689','73004690','73004691','73004700','73004701','73004702','73004703','73004713','73004723','73004736','73004737','73004741','73004752','73004756','73004759','73005198','73005199','73005368','73005369','73005403','73005428','73005429','73005430','73005782','73005789','73005790','73005853','73005854','73005856','73005859','73005860','73005861','73005862','73005879','73005890','73005891','73005892','73005893','73005894','73005895','73005896','73005897','73005898','73005900','73005901','73005936','73005941','73006004','73006173','73006752','73006753','73006754','73006755','73006756','73006757','73006758','73006759','73007429','73007430','73007431','73007432','73007433','73007434','73007435')

OPEN merch_cursor
if @@ERROR > 0
  return

FETCH NEXT FROM merch_cursor INTO @TID

WHILE @@FETCH_STATUS = 0
        BEGIN
        SET @BALGROUP = 'M'+@TID

       INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
              VALUES (@BALGROUP, 'NET_CLBICALPHA', 'C', '01')
      INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_CLBICEBNK', 'C', '00')
 --     INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
 --             VALUES (@BALGROUP, 'NET_GENIKI', 'C', '00')
      INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_NTBN', 'C', '1000000')
       INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
             VALUES (@BALGROUP, 'NET_ABC', 'C', '0')
--         INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
--                 VALUES (@BALGROUP, 'NET_CITI', 'T', '0')
        FETCH NEXT FROM merch_cursor
        into @TID
END
CLOSE merch_cursor
DEALLOCATE merch_cursor
GO