-- Update regioneu
    with bingroup (binmask) as(
     select (substring(a.mask,1,6))
     from zacreporting.[abc096].[DF_Transactions_Month] a
     group by  (substring(a.mask,1,6)) ),
   totupdbins (bin, reg_eu ) as(
     select b.bin, b.regioneu
     from bingroup a
     inner join  zacreporting.dbo.binbase b on  a.binmask = b.bin   )

      update zacreporting.[abc096].[DF_Transactions_Month]
          set REGIONEUFL = (select reg_eu from totupdbins where bin =(substring(mask,1,6)) )





          with noissueIdUpd(binISS) as(
select substring(mask,1,6)    from [abc096].[DF_Transactions_Month] a
where ISSUER_BANK_ID =  ' '
group by substring(mask,1,6)),

 amexbinupd(binamex) as(
select b.bin from noissueIdUpd a
inner join  dbo.binbase b on a.binISS = b.bin
where b.isoa3 = 'usa' and brand = 'american express')

update v
set v.bankid = 73
from dbo.products  as v
inner join amexbinupd g on g.binISS = v.bin
