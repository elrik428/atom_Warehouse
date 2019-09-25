SELECT dataentry,[Group], dtstamp  Ημερομηνία,PROCBATCH,
   case
    when reversed = 'A' or reversed = 'F'
    then 'Εκτός Πακέτου'
    else (case
     when ePOSBATCH IS null
           then
               (case when PROCBATCH is Null
               then'Εκτός Πακέτου'
               else 'Εντός Πακέτου'
         end)
           else 'Εντός Πακέτου' end
  ) end as Κατηγορία,
case
when reversed ='A' or reversed ='F'
then 'Εκτός Πακέτου EURONET'
else
  (case
    when (ePOSBATCH IS NULL)
    then
        (case
          when (PROCBATCH  IS NULL)
          then 'Εκτός Πακέτου EURONET'
          else 'Εντός Πακέτου EURONET'
          end)
      else  'Εντός Πακέτου EURONET' end)
           + (
		   case when ePOSBATCH IS NULL then ' ' else cast(ePOSBATCH as char) end)

      end as PacakageePOS ,
   dmid + ' / ' + dtid + case
            when reversed = 'A' or reversed ='F'
                         then ' '
                         else
                           (case
               when procbatch is Null
                             then ' '
                             else
                             ' / ' + cast(procbatch as char)  end
                           ) end as   Πακέτο,

   case when dataentry = 'True'
   then 'T' + cast(MASK as char)
   else
   'D' + cast(MASK as char)
   end as  Κάρτα,

   case when cast(dataentry as char) = 'ΠΛΗΚΤΡ'
   then ' '
   end  as Πληκτρ,

   case when bank is Null
   then Brand
   else bank + '/' + product
   end  as Τύπος,

   Amount   Ποσό,
    INST   Δόσεις,
   rtrim (msg + ' ' + act) Συναλλαγή,
    RESPKIND   Απόκριση,
    REVERSED   Αντιλογισμός,
     dtstamp Ώρα,
     TID, PBank  PROCESSED,
      Shop, MID, DMID, DTID, ePOSBATCH, Brand, BTCODE,

      ORGSYSTAN, DTSTAMP_INSERT, BORGSYSTAN, AUTHCODE, CASHIERINFO, TrDMID, TrDTID,
       trdmid + ' / ' + TrDTID + case when reversed ='A' or reversed = 'F'
                              then ' '
                              else (case when procbatch is Null
                                          then ' '
                                        else
                                        ' / ' + cast(procbatch as char)  end  )
                                        end as TrΠακέτο,
  case
        when LEFT(PEM,2)='01' then 'MANUAL ENTRY'
        when LEFT(PEM,2)='02' then 'MAGNETIC STRIPE'
        when LEFT(PEM,2)='80' then 'MAGNETIC STRIPE'
        when LEFT(PEM,2)='05' then 'CHIP CONTACT'
        when LEFT(PEM,2)='07' then 'CONTACTLESS'
        when LEFT(PEM,2)='91' then 'CONTACTLESS'
      end as  PEM_DESC,

      case
        when   BONUSRED > 0 then BONUSRED
        end as BONUS_Redemption

FROM dbo.VTransactions
where mask = '516732xxxxxx1362'
ORDER BY dtstamp, PBank;
