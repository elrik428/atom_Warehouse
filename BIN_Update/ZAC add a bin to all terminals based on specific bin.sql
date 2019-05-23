-- delete [dbo].[MERCHBINS_bup2]
-- -------------------------------------------------------------------
-- drop table [dbo].[MERCHBINS_bup2]
-- 
-- CREATE TABLE [dbo].[MERCHBINS_bup2](
--      [ID] [int] IDENTITY(3300000,1)  NOT NULL ,
--      [TID] [varchar](16) COLLATE Greek_CI_AS NOT NULL ,
--      [MID] [varchar](16) COLLATE Greek_CI_AS NULL ,
--      [DESTPORT] [varchar](20) COLLATE Greek_CI_AS NOT NULL ,
--      [BINLOWER] [varchar](22) COLLATE Greek_CI_AS NOT NULL ,
--      [BINUPPER] [varchar](22) COLLATE Greek_CI_AS NOT NULL ,
--      [INSTMIN] [int] NULL ,
--      [INSTMAX] [int] NULL ,
--      [GRACEMIN] [int] NULL ,
--      [GRACEMAX] [int] NULL ,
--      [ALLOWED] [varchar](1) COLLATE Greek_CI_AS NULL ,
--      [SYNC_ID] [int] NULL ,
--      [AMOUNTMIN] [varchar](10) COLLATE Greek_CI_AS DEFAULT ((0))  NULL ,
--      [AMOUNTMAX] [varchar](10) COLLATE Greek_CI_AS DEFAULT ((999999999))  NULL 
-- ) ON [PRIMARY]
-- go

--  insert into [dbo].[MERCHBINS_bup2] (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
--    (select TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from merchbins)
-- -------------------------------------------------------check the insertion
-- select count(*) from merchbins
-- select count (*) from merchbins_bup2
-- -------------------------------------------------------------------

select count(*) from MERCHBINS where (binlower = '4' and binupper = '6') or (binlower = '5' and binupper = '5') or (binlower = '5' and binupper = '6')
select count(*) from MERCHBINS where (binlower = '222100' and binupper = '272099')
select * from MERCHBINS where (binlower = '222100' and binupper = '272099')


select count(*) from MERCHBINS where (binlower = '589242' and binupper = '589242') --3319
select count(*) from MERCHBINS where (binlower = '516297' and binupper = '516297') --3319
select * from MERCHBINS where (binlower = '516297' and binupper = '516297') and tid = '1111    '
select * from MERCHBINS where (binlower = '589242' and binupper = '589242')
517179
527802
554272

-----------------------------------------------------------------------------------------------


declare @tid varchar(16)

declare merch_cursor cursor for
select DISTINCT TID from MERCHBINS
where (binlower = '589242' and binupper = '589242')
and tid <> '1111    '

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin
--  delete from MERCHBINS where TID = @tid and DESTPORT = 'NET_ALPHA'

--         insert into MERCHBINS (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed)
--         values (@TID, 'NET_NTBN', '442317', '442317', 0, 1, 0, 99, 'Y');

  insert into MERCHBINS
  (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)
  (
  select @TID, DESTPORT, '537440', '537440', INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from MERCHBINS
  where TID = @TID and (binlower = '557378' and binupper = '557378')
  )

  fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
 
