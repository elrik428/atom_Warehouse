--- E

----  DESTCOMID = NBG      NBG  trxs NOT LTY
  select z.DESTCOMID, 'NBG - NOT LTY TRXS' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +z.AMOUNT
  when '20' then -z.AMOUNT
  when '02' then -z.AMOUNT
  when '22' then -z.AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, MASK, TID
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_NTBN') z
   left join [abc096].[IMP_TRANSACT_D_tempLN] b on z.MASK = b.mask and z.AMOUNT = b.AMOUNT and b.DESTCOMID = 'NET_PBGLTY' and z.TID = b.TID
where b.mask is null and b.AMOUNT is null and b.TID is null and b.ORGSYSTAN is null and  b.DESTCOMID is null
  and  (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode = 6) )
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 6 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 6 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 6 )

 group by z.DESTCOMID



----  DESTCOMID = NBG      NBG  trxs  LTY
select z.DESTCOMID, 'ABC - ABC cards -  LTY' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +z.AMOUNT
  when '20' then -z.AMOUNT
  when '02' then -z.AMOUNT
  when '22' then -z.AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from (select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE, mask, DTSTAMP, TID, ORGSYSTAN
		from [abc096].[IMP_TRANSACT_D_tempLN]
		where RESPKIND = 'OK'  and DESTCOMID = 'NET_NTBNLTY' and PROCCODE = '000001') z
  join [abc096].[IMP_TRANSACT_D_tempLN] b on z.MASK = b.mask and z.AMOUNT = b.AMOUNT and b.DESTCOMID = 'NET_NTBN' and z.TID = b.TID
  --and z.DTSTAMP = b.DTSTAMP
  --where
 -- (z.sixplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--) z
  group by z.DESTCOMID



  ----  DESTCOMID = NBG     No GREEK trxs
  select destcomid, 'NBG -  NO GREEK CARDS' , b.isocountry,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  join dbo.binbase b on substring(mask,1,6) = bin and isocountry <> 'GREECE'
  where RESPKIND = 'OK'  and DESTCOMID = 'NET_NTBN'
  group by DESTCOMID,b.isocountry
  order by b.isocountry



  ----  DESTCOMID = NBG     OTHER BANKS CARDS - Not NBG trxs
  select destcomid, 'OTHER BANKS CARDS - NOT NBG' ,
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
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_NTBN') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 6)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 6 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 6 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 6 )
			)
			group by DESTCOMID
			group by DESTCOMID
