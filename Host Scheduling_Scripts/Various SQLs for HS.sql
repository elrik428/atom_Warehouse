select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
('73001095','73001097','73001098','73001099','73001100','73001101','73000718','73002971','73001071','73003491','73005397','73001073','73001076','73002970','73004807','73001078','73001072','73005393','73005391','73001070','73001068','73001075','73002972','73001069','73001066')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm

select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
--substring(appnm,1,4) = ('EPOS') and substring(appnm,9,1) = 'P' and
--famnm = 'Vx-520' and
--famnm = 'Vx-675' and
TERMID in
('01117742','01238251','01238261','01239591','01246191','01313630','01389560','01499340','01616730')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm


select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
TERMID IN
 ('01320840','01320841','01557520','01100352')
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
--and appnm  in ('EPOS01C6P')
--and appnm   in ('EPOS0201P')
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
order by famnm,appnm


select distinct CLUSTERID,termid, appnm   from vc30.relation where
CLUSTERID = 'EPOS_VEROPOULOS'
AND substring(appnm,1,4) = ('EPOS')and
substring(appnm,9,1) = ('P') and
acccnt = -1
--and appnm  in ('EPOS01C6P')
--and appnm   in ('EPOS0201P')X
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
order by appnm

------   Group by FAMNM  & Version
select appnm, famnm, count(*)   from vc30.relation where
TERMID IN
('00001548','00007005','00014113','00015208','00017872')
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
--and appnm  in ('EPOS01C6P')
--and famnm = 'Vx-520'
--and famnm = 'Vx-675'
--and famnm = 'Vx-675WiFi'
group by appnm, famnm
order by  famnm  ,appnm


-----   Appnm fragmenation in clusters
select distinct CLUSTERID, appnm, count(*)   from vc30.relation where
CLUSTERID like 'EPOS_%'
AND substring(appnm,1,4) = ('EPOS')and
substring(appnm,9,1) = ('P') and
acccnt = -1
--and appnm  in ('EPOS01C6P')
--and appnm   in ('EPOS0201P')X
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
group by CLUSTERID, appnm
order by CLUSTERID, appnm



select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
substring(appnm,1,4) = ('EPOS') and substring(appnm,9,1) = 'P' and
TERMID in
('73000004','73000026','73000088','73000058','73000064','73000069','73005506','73000037','73000068','73000072','73000039','73000031','73000086','73000033','73000003','73000050','73000062','73000022','73000092','73000036','73000030','73000081','73000097','73000049','73000043','73000070','73000071','73000057','73000090','73000095','73000093','73000038','73000066','73000020','73000083','73000048','73000080','73000034','73000063','73000060','73000084','73000021','73000082','73000051','73000029','73000005')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm


-- Find NO CTLS Tids
select a.TERMID from vc30offline.vc30.RELATION a
where not exists (
select distinct b.CLUSTERID,b.TERMID, b.APPNM, b.FAMNM   from vc30offline.vc30.relation  b
where
b.TERMID IN
('01958321','01002601','00020892','00008717','00004550','00001861','00014735')
AND b.ACCCNT = -1
and b.APPNM like ('CTLS%') and a.TERMID = b.TERMID)
and a.TERMID IN ('01958321','01002601','00020892','00008717','00004550','00001861','00014735')
group by TERMID
order by TERMID


-- Find Version from CTLS tids
select distinct a.CLUSTERID,a.TERMID, a.APPNM, a.FAMNM   from vc30.vc30.relation a where
a.TERMID IN
 (		select distinct b.TERMID from vc30.vc30.relation  b
		where
		b.TERMID IN
		('01958321','01002601','00020892','00008717','00004550','00001861','00014735')
		AND b.ACCCNT = -1
		and b.APPNM like ('CTLS%')
)
AND substring(a.APPNM,1,4) = ('PIRA')
 and substring(a.APPNM,9,1) = ('P')
AND a.ACCCNT = -1
order by a.FAMNM,a.APPNM


----- Find TIDs that don't have specific PARNAMELOC   #1
select partid from vc30.parameter a
where not exists (
select b.partid  from vc30.PARAMETER b
where b.partid in
('02167135','01281541','01083341','01265671','01010341')
and b.parnameloc ='LOCK_TRX_TIME' and a.partid = b.partid )
and A.partid in
('02167135','01281541','01083341','01265671','01010341')
group by PARTID



--- Find the non exist
select distinct termid from vc30.relation a where
TERMID in
('675CTLSTRA-CO','675CTLSTRA','675CTLSSUP-CO','675CTLSSUP','675CTLSRES-CO','675CTLSSAL-CO','675CTLSSAL','675HOTEL','675RES','675SALES','675CTLSRES','675TRAVEL','675SUPER','675INSTANT','675INST','675CAR','675CTLSCAR-CO','675CTLSCAR','675CTLSINS','675CTLSHOT','675CTLSHOT-CO','675CTLSINS-CO')
and not  exists (select termid from vc30.RELATION b where termid in ('675CTLSCAR','675CTLSCAR-CO','675CTLSHOT','675CTLSHOT-CO','675CTLSINS','675CTLSINS-CO','675CTLSRES','675CTLSRES-CO','675CTLSSAL','675CTLSSAL-CO','675CTLSSUP','675CTLSSUP-CO','675CTLSTRA','675CTLSTRA-CO','675INSTANT') and a.TERMID = b.TERMID)


------- Piraeus MAsks
select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
--termid like 'COMBO%'
(TERMID like '675%' or termid like '520G%' or termid like 'COMBO%' or termid like '520INSTA%' or termid like '690%' )
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
--and appnm  in ('EPOS01C6P')
--and appnm   in ('EPOS0201P')
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
order by famnm,appnm


----- Find duplicates of aapnm
select top 2500 TERMID, FAMNM, count(*)   from vc30offline.vc30.relation where
CLUSTERID = 'PIRAEUS'
--('01982700','01109163','01109164','01109165','01109166','01109167','01109168','01109169','01109170','01109171','03109150','03109152','03109153','03109154','03109155','03109156','03109157','03109158','03109159','04109151','04109152','04109154','01318811','01544020','01816610','00001861','01308650','01328200','01329370','01375720','01468060','01640490','01690810','01993081','01701060','01548280','01025311','01555600','01466880','01367932','01462911')
--substring(appnm,1,4) = ('PIRA')
-- and substring(appnm,9,1) = ('P')
AND
 acccnt = -1
and appnm  like 'EOS%'
--and appnm   in ('EPOS0201P')
--and appnm = 'EOS011103' and
--and appnm = 'EOS010802'
--and famnm = 'Vx-675'
group by TERMID,   FAMNM
having count(*) > 1



-- Find if PIRA0204 tids have old logo parm value
select * from vc30.PARAMETER where
PARNAMELOC = 'LOGO_DISP_EN'
and appnm = 'PIRA0204P'
and [value] = 'F:PIR_EN.VFT'

update vc30.PARAMETER
set [value] = 'F:KATASTASHA_D.VFT'
where PARNAMELOC = 'LOGO_DISP_EN' and PARTID  = '73003919'


-- Check if a group of TIDs have a specific parm
select * from vc30.PARAMETER where PARTID in
('73003919','73003921','73003918','73003917')
and PARNAMELOC = 'AUTO_HOSTSCH'

update vc30.PARAMETER
set [value] = '22:19'
where PARNAMELOC = 'AUTO_HOSTSCH' and PARTID in
('73001291','73001357','73001358','73001359','73001360','73001361','73001362','73005966','73005978','73005981','73005992','73007492')

-- Check times to be equally split
select PARNAMELOC, [value], count(*) from vc30.PARAMETER
where PARNAMELOC = 'AUTO_HOSTSCH'
group by PARNAMELOC, [value]


select * from vc30.PARAMETER where PARTID in
--('73004784')
('675KOTFR','73001478','73001482','KOTSO675')
and PARNAMELOC = 'LOTTERYMC'




-- Find TIDs with RESTAURANT and LOCK_TRX_TIME <> 06:00 from InstantPOS procedure
select  * from
(select * from vc30.PARAMETER where PARTID like '1981%'
	and PARNAMELOC in( 'MCC')
	and [value] = 'RESTAURANT') q
join vc30.PARAMETER b on q.PARTID = b.PARTID
and b.PARNAMELOC = 'LOCK_TRX_TIME' and b.[VALUE] <> '06:00'



select * from vc30.PARAMETER where
PARTID in
('00001548','00007005','00014113','00015208')
 and PARNAMELOC = 'MCC'
and [value] = 'RESTAURANT'
and appnm = 'PIRA0204P'
--and FAMNM = 'Vx-520'
--and FAMNM = 'Vx-675'
and FAMNM = 'Vx-675WiFi'



-- various Update
update vc30.PARAMETER
set [value] = 'RESTAURANT'
where PARNAMELOC = 'MCC' and PARTID in
('00079963')
and [VALUE] <> 'RESTAURANT'

update vc30.PARAMETER
set [value] = '06:00'
where PARNAMELOC = 'LOCK_TRX_TIME' and PARTID in
('00079963')


   -----   Check PIRAEUS for version < 0200

select   distinct   cast(vc30.relation.TERMID as varchar(30)) as TID,
o.[value] as [ΣΥΝΔΕΣΗ],
x.late_versio as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ],
vc30.relation.appnm as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ MASKED]
from (vc30.relation
left join vc30.PARAMETER o on vc30.relation.TERMID=o.PARTID and o.PARNAMELOC = 'MEDIA'
left join vc30.TERMINFO    on vc30.relation.TERMID=termid_term
join  (select e.*,
	   case (substring(e.vers_pir,9,1)) when ',' then substring(e.vers_pir,1,8) + 'P' when 'P' then (e.vers_pir) end as late_versio
	   from
		(select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('PIRA', a.appnm),9) vers_pir
		 from vc30.termlog a,(select  max(evdate) date_max, termid  from vc30.termlog where message = 'Download Successful' and status = 'S'
		 group by termid) q
	   where  a.evdate = q.date_max and a.termid = q.termid and appnm like '%PIRA%')e) x on vc30.relation.TERMID=x.termid)
where substring(cast(vc30.relation.appnm as char(10)),9,1) = ('P')
and  vc30.relation.CLUSTERID in ('PIRAEUS')
and vc30.relation.appnm not in ('PIRA0201P','PIRA0202P','PIRA0203P','PIRA0204P')
and o.[value] in ('ETH','WIFI','GPRS')
--and o.[value] = 'ETH'
--and o.[value] = 'WIFI'
--and o.[value] = 'GPRS'
and substring (vc30.relation.TERMID,1,1) in ('0','1')

--AND (vc30.relation.TERMID NOT like '675%'
--	or vc30.relation.TERMID not like '520G%'
--	or vc30.relation.TERMID not like 'COMBO%'
--	or vc30.relation.TERMID NOT like '520INSTA%'
--	or vc30.relation.TERMID not like '520%')


---- Verification SQL triplet
select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
('00001548','00007005','00014113')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm

select count(*) from vc30.PARAMETER
where partid in
('00001548','00007005','00014113','00015208')
and parnameloc ='LOCK_TRX_TIME'

select value, COUNT(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
('00001548','00007005','00014113','00015208')
group by value


---- CUP Parameter Insert - Update
--1.
 select * from vc30.term_dld_files
 where termid in ('33333333','520INTER','675INTER','73000850','73000851')
 and SERFILENM = 'ParamFiles\_CUP_NoNCVM_AllCVMs\P.zip'

--2.
 update vc30.term_dld_files set serfilenm = 'ParamFiles\_CUP_NoNCVM_AllCVMs\P.zip'
 where serfilenm = 'ParamFiles\NoNCVM_AllCVMs\P.zip'
 and termid in ('33333333','520INTER','675INTER','73000850','73000851')

--3.
select b.PARTID, b.APPNM, b.FAMNM, PARNAMELOC, [value]   from vc30.vc30.PARAMETER  b
where
 PARTID in ('33333333','520INTER','675INTER','73000850','73000851')
 and  PARnameloc like ('CARD14%')
 and [value] like '%UNION%'

--3a.
select b.PARTID, b.APPNM, b.FAMNM, PARNAMELOC,
[value] ,substring([value],1,9)+'0'+substring([value],11,70)
   from vc30.vc30.PARAMETER  b
where
substring([value],10,1) = '1' and
 --PARTID in
  --('73007242','73007243','73007244','73007245','73007246','73007247','73007248','73007249','73007250','73007251')
  --and
  [value] like '%UNION%'


--4.
-- Use the claasic cursor for insert. Below is the main INSERT
 insert into vc30.PARAMETER
        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
        (
        select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
        'UNIONPAY:1:1:1:0:0:0:0:1:0:2:0:0:000000000000:000000:300000',
        PSIZE,FLAG
        from vc30.parameter where PARTID = 'TEPOS0211P' and PARNAMELOC = 'CARD14' and
		famnm = @FromModel
		        )

--5. update
update vc30.vc30.PARAMETER
set [value] = substring([value],1,9)+'0'+substring([value],11,70)
where
PARTID in
 ('01036650','01036730','01037150')
 and [value] like '%UNION%'


-- Update HOST_IP,SERVICE_IP, BONUS_IP etc.
select * from vc30.PARAMETER where PARTID in
--('73004784')
('73007855','73007787','73007791','73007413','73007420','73007412','73007479','73007411','73007407','73007410','73007416','73007523','73007525','73007524','73007528','73000885','73000886','73007402','73007401')
and PARNAMELOC in ('HOST_IP2','BONUS_IP2','SERVICES_IP2')

update vc30.PARAMETER
set value = '195.226.126.050'
where PARTID in
('73007855','73007787','73007791','73007413','73007420','73007412','73007479','73007411','73007407','73007410','73007416','73007523','73007525','73007524','73007528','73000885','73000886','73007402','73007401')
and PARNAMELOC in ('HOST_IP2','BONUS_IP2','SERVICES_IP2')
