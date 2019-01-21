1.
-- delete [dbo].[MERCHBINS_bup3]
-- -------------------------------------------------------------------

2.
-- insert into [dbo].[MERCHBINS_bup3] (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
--   (select TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from merchbins)
-- check the insertion

3.
-- select count(*) from merchbins
-- select count (*) from merchbins_bup3

4.
--select * from merchbins where tid in (select tid from merchants where mid = '000000120005107') /*and binlower = '549804'*/
--select * from merchbins where tid = '73005513' order by destport,binlower --- Use as reference for check

5.
--delete from merchbins where tid in (select tid from merchants where mid = '000000120005107')
----select * from merchbins where tid = '1111    ' and destport = 'NET_NTBN' order by destport,binlower

6.  ---Check Banks
select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120005107' and tid not in
(select tid from merchants where mid = '000000120005107' and uploadhostid = 202)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120005107' and tid not in
(select tid from merchants where mid = '000000120005107' and uploadhostid = 205)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120005107' and tid not in
(select tid from merchants where mid = '000000120005107' and uploadhostid = 6)

select distinct mid,tid,merchtitle,merchaddress from merchants where mid = '000000120005107' and tid not in
(select tid from merchants where mid = '000000120005107' and uploadhostid = 1)


-- uploadhostid
-- 1 = Piraeus , NET_ABC
-- 6 = ETHNIKI, NET_NTBN
-- 202 = ALPHABANK, NET_CLBICALPHA
-- 205 = EUROBANK, NET_CLBICEBNK
--
declare @tid varchar(16)
declare @name varchar(20)

declare merch_cursor cursor for
select  TID, uploadhostname from MERCHANTS a, uploadhosts b
where mid = ('000000120005107')

and a.uploadhostid = '202'
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


--7.1 ---Basic Insert - Change from CUP so to run --
    -- Example for CUP. ALLOWED MUST BE USED. Applies to NET_ABC = 'Y', all others 'N'. Instead of DESTPORT --> NET_XXXXXX must be used
  insert into MERCHBINS
  (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
  (
  select @TID,
  DESTPORT,
  --'NET_CLBICALPHA',
  BINLOWER, BINUPPER,
  --INSTMIN,
  --2
  0,
  --INSTMAX,
  48,
  GRACEMIN, GRACEMAX,
  ALLOWED, ---- Used only for CUP
  --'Y',
  AMOUNTMIN, AMOUNTMAX from MERCHBINS
  where TID = '1111    ' and DESTPORT = 'NET_CLBICEBNK'
  --where tid = '2222    ' and destport = 'NET_CUP'   ----Used only for CUP
  --and binlower <> '549804'
  --    and binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')
  )
--7.2  --kouvas insert
-- --kouvas
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
 --values (@TID, @name, '4', '6', 0, 48, 0, 99, 'Y', 0 , 999999999);
 --insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
 --values (@TID, @name, '9', '9', 0, 48, 0, 99, 'Y', 0 , 999999999);
 --insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
 ---values (@TID, @name, '222100', '272099', 0, 48, 0, 99, 'Y', 0 , 999999999);
--
-- --meals and more
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '502259', '502259', 0, 1, 0, 99, 'Y', 0 , 999999999);
--
-- -- ticket restaurant
-- insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
-- values (@TID, @name, '534228', '534228', 0, 1, 0, 99, 'Y', 0 , 999999999);


  fetch next from merch_cursor
  into @tid, @name
end

CLOSE merch_cursor
deallocate merch_cursor




select gmaxinst, * from merchants where mid = ('000000120005107')
update merchants set gmaxinst=48  where mid = ('000000120005107')

select * from loadbalance where balancegroup in (select 'M'+tid from merchants where mid = '000000120005107')

delete loadbalance where balancegroup in (select 'M'+tid from merchants where mid = '000000120005107')



declare @TID varchar(16), @BALGROUP varchar(20)

DECLARE merch_cursor CURSOR FOR
SELECT distinct(tid) FROM merchants WHERE mid = '000000120005107'

OPEN merch_cursor
if @@ERROR > 0
  return

FETCH NEXT FROM merch_cursor INTO @TID

WHILE @@FETCH_STATUS = 0
        BEGIN
        SET @BALGROUP = 'M'+@TID

       INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
              VALUES (@BALGROUP, 'NET_CLBICALPHA', 'C', '50')
      INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_CLBICEBNK', 'C', '50')
 --     INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
 --             VALUES (@BALGROUP, 'NET_GENIKI', 'C', '00')
      INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
               VALUES (@BALGROUP, 'NET_NTBN', 'C', '50')
       INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
             VALUES (@BALGROUP, 'NET_ABC', 'C', '00')
--         INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE)
--                 VALUES (@BALGROUP, 'NET_CITI', 'T', '0')
        FETCH NEXT FROM merch_cursor
        into @TID
END
CLOSE merch_cursor
DEALLOCATE merch_cursor
GO
