------   To check versions for tid list
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


--- Check version for every TID for specific TIDs
select distinct CLUSTERID,termid, appnm, famnm   from vc30.relation where
TERMID IN
 ('01320840','01320841','01557520','01100352')
AND substring(appnm,1,4) = ('PIRA')
 and substring(appnm,9,1) = ('P')
AND acccnt = -1
and appnm  in ('EPOS01C6P')
and famnm = 'Vx-675'
order by famnm,appnm


-- Check version and libraries for all selected TIDs   -- Run before update and after update to check 
select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
TERMID in
('73001095','73001097','73001098','73001099','73001100','73001101','73000718','73002971','73001071','73003491','73005397','73001073','73001076','73002970','73004807','73001078','73001072','73005393','73005391','73001070','73001068','73001075','73002972','73001069','73001066')
group by CLUSTERID,famnm,appnm
ORDER BY CLUSTERID,famnm,appnm
