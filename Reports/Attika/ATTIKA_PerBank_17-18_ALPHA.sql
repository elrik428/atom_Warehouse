
--- D

----  DESTCOMID = ALPHA      ALPHA  trxs NOT LTY -   VISA & MASTERCARD  ONLY
  select destcomid, 'ALPHA - NOT LTY - VISA & MASTERCARD  ONLY' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, USERDATA
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode = 202)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 202 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 202 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 202 )
			  )
  and SUBSTRING(sixplace,1,1) in ('4','5')
  and z.USERDATA is  null or z.USERDATA = ''
			group by DESTCOMID

			----  DESTCOMID = ALPHA     AMEX trxs
  select destcomid, 'AMEX' ,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  where RESPKIND = 'OK' and
  (substring(mask,1,3) in ('644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')  or substring(mask,1,4) in ('6011') )
   and DESTCOMID = 'NET_ALPHA'
  group by DESTCOMID


    ----  DESTCOMID = ALPHA      DINERS - NOT LTY
  select destcomid, ' APLHA - DINERS - NOT LTY' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, USERDATA, MASK
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA') z
   --where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 202)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )

			where z.USERDATA is  null or z.USERDATA = '' and substring(z.mask,1,2) in ('30','36','38','34','37')
			group by DESTCOMID


    ----  DESTCOMID = ALPHA     DINERS - LTY
  select destcomid, ' APLHA - DINERS - LTY' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, USERDATA,MASK
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA') z
   --where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 202)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )

			where z.USERDATA is not null and z.USERDATA  <> '' and substring(z.mask,1,2) in ('30','36','38','34','37')
			group by DESTCOMID


    ----  DESTCOMID = ALPHA     DINERS - ΕΞΑΡΓΥΡΩΣΕΙΣ
  select destcomid, ' APLHA - DINERS - ΕΞΑΡΓΥΡΩΣΕΙΣ' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, USERDATA,BONUSRED, MASK
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA') z
   --where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 202)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			where BONUSRED is  null and USERDATA is not null and userdata  <> '' and substring(z.mask,1,2) in ('30','36','38','34','37')
			group by DESTCOMID



  ----  DESTCOMID = ALPHA     No GREEK trxs
  select destcomid, 'ALPHA NO GREEK' , b.isocountry,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  join dbo.binbase b on substring(mask,1,6) = bin and isocountry <> 'GREECE'
  where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA'
  group by DESTCOMID,b.isocountry
  order by b.isocountry



  ----  DESTCOMID = ALPHA     OTHER BANKS CARDS - Not ALPHA trxs
  select destcomid, 'OTHER BANKS CARDS - NOT ALPHA CARDS' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ALPHA') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 202)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 202 )
			)
			group by DESTCOMID
