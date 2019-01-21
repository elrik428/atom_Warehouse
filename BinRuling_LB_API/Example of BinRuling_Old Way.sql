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
where tid = '78000000'

and a.uploadhostid = '205'    ---Goes to Eurobank
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
    -- Example for CUP. ALLOWED MUST BE USED. Applies to NET_ABC = 'Y', all others 'N'
  insert into MERCHBINS
  (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
  (
  select @TID,
  --DESTPORT,
  'NET_CLBICEBNK',  ------ Goes to Eurobank
  BINLOWER, BINUPPER,
  --INSTMIN,
  --2
  0,
  --INSTMAX,
  36,
  GRACEMIN, GRACEMAX,
  ALLOWED, ---- Used only for CUP
  --'Y',
  AMOUNTMIN, AMOUNTMAX from MERCHBINS
  where TID = '1111    ' and DESTPORT = 'NET_ABC'    -------- Issue bank card
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
