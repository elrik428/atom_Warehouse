declare @tid varchar(16)
declare @binlower varchar(22)

declare merch_cursor cursor for
 select [column 0] from [dbo].[binlist - Copy]


open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @binlower

while @@FETCH_STATUS = 0
begin

 insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX)
 values ('2222    ', 'NET_CUP', @BINLOWER, @BINLOWER, 0, 1, 0, 99, 'Y', 0 , 999999999);
  fetch next from merch_cursor
  into @binlower
end

CLOSE merch_cursor
deallocate merch_cursor
