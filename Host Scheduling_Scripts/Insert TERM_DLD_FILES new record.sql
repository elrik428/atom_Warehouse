
--Procedure for insert
declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

SET @FromModel='Vx-520'
SET @FromAppnm='TIRA0101P'


declare merch_cursor cursor for

select DISTINCT TERMID from vc30.relation
where
TERMID in
('70103164','70104001','70104002','70103110','70103104','70103105','70103106','70103056','70103057','70103058','70100760','70103152','520GTIRA','70103149','70103151','70103232','70103182','70103211','70103228','70103233','70103249','70100999','70100739','80000009','70103001','70103032','70103036','70103038','70103046','70103100','70103051','01234507','70103101','70103098','70103099','70103078','70103082','70103153','70100761','70103091','70103094','70103127','70103138','70103139','70103144','70103166','70103174','70103176','70103178','70103189','70103199','70103207','70103205','70103225','70103227','70103302','70103229','70103239','70103244','70103258','70103260','70103261')
and FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0
begin

        insert into vc30.TERM_DLD_FILES
        ([FAMNM],[APPNM],[TERMID],[SEQINFO],[SERFILENM],[TERFILENM],[GID],[DRIVE],[DLDTYPE],[FILETYPE])        
        VALUES
           ('Vx-520','TIRA0101P',@tid,'0',
           'Currency\EUR\C.zip',
           --'Currency\LEK\C.zip',
           'C.zip','1','I','FP','5')
		        

  fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor
