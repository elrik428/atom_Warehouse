--- C

----  DESTCOMID = EUROBANK   EUROBANK trxs NOT LTY
  select destcomid, 'EUROBANK - NOT LTY' ,
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
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_EBNK') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode = 205)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			 )
			group by DESTCOMID



----  DESTCOMID = EUROBANK   --- ΕΞΑΡΓΥΡΩΣΗ EUROBANK
  select z.DESTCOMID, 'ΕΞΑΡΓΥΡΩΣΗ EUROBANK' ,
  sum (z.AMOUNT) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, mask, DTSTAMP, TID, ORGSYSTAN
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_EBLY1' and PROCCODE = '1U0000') z
   --where (z.sixplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			 --)
			group by z.DESTCOMID

----  DESTCOMID = EUROBANK   EUROBANK trxs  LTY
 select z.DESTCOMID, 'LTY EUROBANK' ,
  sum (z.AMOUNT) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, mask, DTSTAMP, TID, ORGSYSTAN
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_EBLY1' and PROCCODE = '2S0000') z
  -- where (z.sixplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 205 )
			 --)
			group by z.DESTCOMID


  ----  DESTCOMID = EUROBANK   No GREEK trxs
  select destcomid, 'EUROBANK NO GREEK' , b.isocountry,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  join dbo.binbase b on substring(mask,1,6) = bin and isocountry <> 'GREECE'
  where RESPKIND = 'OK'  and DESTCOMID = 'NET_EBNK'
  group by DESTCOMID,b.isocountry
  order by b.isocountry



  ----  DESTCOMID = EUROBANK  OTHER BANKS CARDS - Not EUROBANK trxs
  select destcomid, 'OTHER BANKS CARDS - NOT EUROBANK' ,
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
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_EBNK') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 205)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 205 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 205 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 205 )
			 )
			group by DESTCOMID
