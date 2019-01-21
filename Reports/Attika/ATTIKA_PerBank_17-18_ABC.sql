-- B
----  DESTCOMID = ABC     CUP trxs
  select destcomid, 'CUP' ,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  where RESPKIND = 'OK' and substring(mask,1,6) in ('622300','622321','622397','622428','622441','622462','622466','622476','622481','622603','622902','622922','625001','625101','628214','628230','628285','628381') and DESTCOMID = 'NET_ABC'
  group by DESTCOMID



  ----  DESTCOMID = ABC   No GREEK trxs
  select destcomid, ' ABC NO GREEK' , b.isocountry,
  sum (case substring(PROCCODE, 1, 2)
  when '00' then +AMOUNT
  when '20' then -AMOUNT
  when '02' then -AMOUNT
  when '22' then -AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from [abc096].[IMP_TRANSACT_D_tempLN]
  join dbo.binbase b on substring(mask,1,6) = bin and isocountry <> 'GREECE'
  where RESPKIND = 'OK'  and DESTCOMID = 'NET_ABC'
  group by DESTCOMID,b.isocountry
  order by b.isocountry



  ----  DESTCOMID = ABC   OTHER BANKS CARDS
  select destcomid, 'OTHER BANKS CARDS - NOT ABC' ,
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
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ABC') z
   where (z.sixplace  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode <> 1)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 1 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 1 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode <> 1 )
			 )
			group by DESTCOMID


----  DESTCOMID = ABC   ABC trxs NOT LTY
  --select destcomid, 'ABC - NOT LTY' ,
  --sum (case substring(z.PROCCODE, 1, 2)
  --when '00' then +AMOUNT
  --when '20' then -AMOUNT
  --when '02' then -AMOUNT
  --when '22' then -AMOUNT
  --end) Total_amount,
  --count(*)  Trxs
  --from
  --(select DESTCOMID,AMOUNT, substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace, PROCCODE
  -- from [abc096].[IMP_TRANSACT_D_tempLN]
  -- where RESPKIND = 'OK'  and DESTCOMID = 'NET_ABC') z
  -- where (z.sixplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1)
		--	or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
		--	or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
		--	or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )  )
		--	group by DESTCOMID


----  DESTCOMID = ABC   ABC trxs NOT LTY
  select z.DESTCOMID, 'ABC - ABC cards - NOT LTY ' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +z.AMOUNT
  when '20' then -z.AMOUNT
  when '02' then -z.AMOUNT
  when '22' then -z.AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT, PROCCODE, mask, DTSTAMP, TID, ORGSYSTAN
  --substring(mask,1,2) twoplace, substring(mask,1,3) threeplace,substring(mask,1,4) fourplace,substring(mask,1,6) sixplace
     from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ABC'
    and		 (substring(mask,1,6)  in (select SUBSTRING(binlower_m,1,6) from [dbo].[Merchbins_ln_1111] where  destcode = 1)
			 --or substring(mask,1,4) in (select SUBSTRING(binlower_m,1,4) from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			 --or substring(mask,1,3) in (select SUBSTRING(binlower_m,1,3) from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			 --or substring(mask,1,2)  in (select SUBSTRING(binlower_m,1,2) from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			 )
			 ) z
left join [abc096].[IMP_TRANSACT_D_tempLN] b on z.MASK = b.mask and z.AMOUNT = b.AMOUNT and b.DESTCOMID = 'NET_PBGLTY' and z.TID = b.TID and z.ORGSYSTAN = b.ORGSYSTAN
where b.mask is null and b.AMOUNT is null and b.TID is null and b.ORGSYSTAN is null and  b.DESTCOMID is null
 group by z.DESTCOMID


 ----  DESTCOMID = ABC   ABC trxs  LTY

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
		where RESPKIND = 'OK'  and DESTCOMID = 'NET_PBGLTY' and PROCCODE = '000001') z
  join [abc096].[IMP_TRANSACT_D_tempLN] b on z.MASK = b.mask and z.AMOUNT = b.AMOUNT and b.DESTCOMID = 'NET_ABC' and z.TID = b.TID and z.ORGSYSTAN = b.ORGSYSTAN
  --and z.DTSTAMP = b.DTSTAMP
  --where
 -- (z.sixplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1)
			--or  Z.fourplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--or z.threeplace in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--or z.twoplace  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			--) z
  group by z.DESTCOMID


--- ΕΞΑΡΓΥΡΩΣΗ ABC

  select z.DESTCOMID, 'ABC - ΕΞΑΡΓΥΡΩΣΗ ABC ' ,
  sum (case substring(z.PROCCODE, 1, 2)
  when '00' then +z.AMOUNT
  when '20' then -z.AMOUNT
  when '02' then -z.AMOUNT
  when '22' then -z.AMOUNT
  end) Total_amount,
  count(*)  Trxs
  from
  (select DESTCOMID,AMOUNT,
   PROCCODE
   from [abc096].[IMP_TRANSACT_D_tempLN]
   where RESPKIND = 'OK'  and DESTCOMID = 'NET_ABC' and PBGAMOUNT is not null and PBGAMOUNT <> 0
   -- and		 (substring(mask,1,6)  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1)
			-- or substring(mask,1,4) in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			-- or substring(mask,1,3) in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 )
			-- or substring(mask,1,2)  in (select binlower_m from [dbo].[Merchbins_ln_1111] where  destcode = 1 ) )
			) z
 group by z.DESTCOMID


 --NET_PBGLTY	ABC - ABC cards -  LTY	6724941,93000054		65341
 --NET_ABC	ABC - ΕΞΑΡΓΥΡΩΣΗ ABC 		7225409,37000059		63499
