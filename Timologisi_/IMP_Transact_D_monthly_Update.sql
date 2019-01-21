
update [abc096].[IMP_TRANSACT_D_monthly]
set productid= 0


update zacreporting.[abc096].[IMP_TRANSACT_D_monthly]
set productid = ( select b.id from  zacreporting.abc096.Products b  where substring(mask,1,6) = b.bin )




update zacreporting.[abc096].[IMP_TRANSACT_D_monthly]
            set productid =
            case
                --when substring(mask,1,2) in ('40','41','42','43','44','45','46','47','48','49') then
				when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then (select top 1 b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                                   inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                                   group by b.id,b.bin)
                ---when substring(mask,1,2) in ('60','61','62','63','64','65','66','67','68','69') then
				when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then (select top 1 b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                                   inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                                   group by b.id,b.bin)
                when substring(mask,1,2) = '23'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '24'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '25'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '26'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '30'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                            group by b.id,b.bin)
                when substring(mask,1,2) = '34'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '35'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '36'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '37'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '38'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '50'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '51'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '52'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '53'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '54'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '55'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '56'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '57'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
                                                           inner join zacreporting.abc096.Products b on substring(c.mask,1,2) = b.bin
                                                           group by b.id,b.bin)
                when substring(mask,1,2) = '58'	 then  (select top(1) b.id from zacreporting.[abc096].IMP_TRANSACT_D_monthly c
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
                where productid is null

				
