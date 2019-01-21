      with bingroup (binmask) as(
         select (substring(a.mask,1,6))
         from zacreporting.[abc096].[DF_Transactions_Month] a
         group by  (substring(a.mask,1,6)) ),
      totupdbins (bin, cntry_fnl, reg_eu) as(
         select b.bin, b.isocountry, b.regioneu
         from bingroup a
         inner join  zacreporting.dbo.binbase b on  a.binmask = b.bin   )

         select * from totupdbins
