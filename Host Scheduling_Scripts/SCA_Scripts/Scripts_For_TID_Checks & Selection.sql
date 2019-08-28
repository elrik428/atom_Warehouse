-- Split per version - Type - Count
select appnm, famnm, count(*)   from vc30.relation where
CLUSTERID IN ('PIRAEUS')
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
--and appnm  in ('EPOS01C6P')
--and famnm = 'Vx-520'
--and famnm = 'Vx-675'
--and famnm = 'Vx-675WiFi'
group by appnm, famnm
order by  famnm  ,appnm

-- Check version for every TID for whole cluster
select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
CLUSTERID IN ('PIRAEUS')
AND substring(appnm,1,4) = ('PIRA')
AND substring(appnm,9,1) = ('P')
AND acccnt = -1
and appnm  in ('PIRA01A4P')
--and appnm   in ('EPOS0201P')
--and famnm = 'Vx-675'
order by famnm,appnm


--- Check version for every TID for specific TIDs
select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
TERMID IN
 ('01320840','01320841','01557520','01100352')
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
--and appnm  in ('EPOS01C6P')
--and famnm = 'Vx-675'
order by famnm,appnm


-- Check version and libraries for all selected TIDs
select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
('73001095','73001097','73001098','73001099','73001100','73001101','73000718','73002971','73001071','73003491','73005397','73001073','73001076','73002970','73004807','73001078','73001072','73005393','73005391','73001070','73001068','73001075','73002972','73001069','73001066')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm


-- Find TIDs w/o CTLS library
select a.TERMID from vc30.vc30.RELATION a
where not exists (
select distinct b.CLUSTERID,b.TERMID, b.APPNM, b.FAMNM   from vc30.vc30.relation  b
where
b.TERMID IN
(
	 select distinct termid 
	 from vc30.relation 
	 where CLUSTERID IN ('PIRAEUS')
	  and substring(appnm,1,4) = ('PIRA')
      and substring(appnm,9,1) = ('P')
	  and acccnt = -1
	 -- and appnm  in ('PIRA01A4P')
)
AND b.ACCCNT = -1
and b.APPNM like ('CTLS%') and a.TERMID = b.TERMID)
and a.TERMID IN 
(
	select distinct termid 
	 from vc30.relation 
	 where CLUSTERID IN ('PIRAEUS')
	  and substring(appnm,1,4) = ('PIRA')
      and substring(appnm,9,1) = ('P')
	  and acccnt = -1
	 -- and appnm  in ('PIRA01A4P')
)
group by TERMID
order by TERMID






