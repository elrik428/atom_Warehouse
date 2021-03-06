update zacreporting.dbo.imp_trans_tmp
set productid =
		case
			when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then  '353348'
			when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then  '353358'
			when substring(mask,1,2) = '23'	 then '353401'
			when substring(mask,1,2) = '24'	 then '353402'
			when substring(mask,1,2) = '25'	 then '353403'
			when substring(mask,1,2) = '26'	 then '353404'
			when substring(mask,1,2) = '30'	 then '353345'
			when substring(mask,1,2) = '34'	 then '353383'
			when substring(mask,1,2) = '35'	 then '353384'
			when substring(mask,1,2) = '36'	 then '353361'
			when substring(mask,1,2) = '37'	 then '353359'
			when substring(mask,1,2) = '38'	 then '353346'
			when substring(mask,1,2) = '50'	 then '353354'
			when substring(mask,1,2) = '51'	 then '353349'
			when substring(mask,1,2) = '52'	 then '353350'
			when substring(mask,1,2) = '53'	 then '353351'
			when substring(mask,1,2) = '54'	 then '353352'
			when substring(mask,1,2) = '55'	 then '353353'
			when substring(mask,1,2) = '56'	 then '353355'
			when substring(mask,1,2) = '57'	 then '353356'
 			when substring(mask,1,2) = '58'	 then '353357'
end
where productid = '0'


update zacreporting.dbo.imp_trans_tmp
set productid =
		case
			when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																																							inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																																							group by b.id,b.bin)
			when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																																							inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																																							group by b.id,b.bin)
			when substring(mask,1,2) = '23'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '24'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '25'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '26'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '30'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '34'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '35'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '36'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '37'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '38'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '50'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '51'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '52'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '53'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '54'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '55'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '56'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
			when substring(mask,1,2) = '57'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
 			when substring(mask,1,2) = '58'	 then  (select b.id from zacreporting.dbo.imp_trans_tmp c
																					inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin
																					group by b.id,b.bin)
end
where productid = '0'
