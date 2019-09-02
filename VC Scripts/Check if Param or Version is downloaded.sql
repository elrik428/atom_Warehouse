
---   DOWNLOADED
select distinct vc30.relation.termid as TID, a.[value], b.[value] , c.[value] , d.[value], e.[value] ,
vc30.TERMLOG.EVDATE as DOWNLOADED,
vc30.TERMLOG.famnm,vc30.TERMLOG.message,vc30.TERMLOG.status,vc30.TERMLOG.appnm, vc30.TERMLOG.duration
from (vc30.relation left join vc30.TERMLOG on
      vc30.relation.TERMID=vc30.TERMLOG.TERMID
      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC ='MERCHID'
      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC='RECEIPT_LINE_1'
      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC='RECEIPT_LINE_2'
      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC='RECEIPT_LINE_3'
      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC='RECEIPT_LINE_4'
      )
where
      (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
      vc30.TERMLOG.appnm like '%PIRA0203P%' and
      vc30.TERMLOG.message = 'Download Successful' and
      vc30.TERMLOG.EVDATE > '2018-05-04 00:00:00.000' and
      vc30.TERMLOG.TERMID in  ('00011977')



   ---  NOT DOWNLOADED
   select distinct vc30.relation.termid as TID
   from  vc30.relation
   where vc30.relation.clusterid = 'EPOS_DUTYFREE'
   and vc30.relation.termid not in ( select vc30.TERMLOG.TERMID from vc30.TERMLOG where
         (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         vc30.TERMLOG.appnm like '%EPOS0213P%' and
         vc30.TERMLOG.message = 'Download Successful' and
         vc30.TERMLOG.EVDATE > '2018-09-24 16:00:00.000')


-- For Donna
         select distinct q.PARTID as TID
            from  (SELECT [PARTID]  FROM [vc30].[vc30].[PARAMETER]
         		  where parnameloc='services_enabled' and [value]='TRUE' and partid not like '73%'
         		   and partid not in ('520CHAL','520CHALREC','520KOTSO','675CHALREC','KOTSMCCT','KOTSO675','KOTSTST','8508TEST','TESTDONN','520EPPCAR','520EPPHOT','520EPPINST','520EPPRES','520EPPSAL','520EPPSUP','520EPPTRA','520GSERV','675EPPCAR','675EPPHOT','675EPPINST','675EPPRES','675EPPSAL','675EPPSUP','675EPPTRA','COMBOSERV','TESTDON2')) q
            where q.PARTID not in
            (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         	where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         	 vc30.TERMLOG.message = 'Download Successful' and
         	 vc30.TERMLOG.EVDATE > '2018-11-10 11:00:00.000')


--For Antonis
select distinct q.PARTID as TID
from  (select PARTID from vc30.PARAMETER where
		PARNAMELOC = 'SERVICES_ENABLED' and [value] = 'TRUE' and substring(partid,1,3) not in ('520','675','820','690','COM','KOT','TES') and substring(partid,1,5) not in ('8508T')) q
where q.PARTID not in  (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         				where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         				vc30.TERMLOG.message = 'Download Successful' and
         				vc30.TERMLOG.EVDATE > '2018-12-11 11:00:00.000')


--

select distinct q.PARTID as TID
   from  (SELECT [PARTID]  FROM [vc30].[vc30].[PARAMETER]
     where parnameloc='services_enabled' and [value]='TRUE' and partid not like '73%'
      and partid not in ('520CHAL','520CHALREC','520KOTSO','675CHALREC','KOTSMCCT','KOTSO675','KOTSTST','8508TEST','TESTDONN','520EPPCAR','520EPPHOT','520EPPINST','520EPPRES','520EPPSAL','520EPPSUP','520EPPTRA','520GSERV','675EPPCAR','675EPPHOT','675EPPINST','675EPPRES','675EPPSAL','675EPPSUP','675EPPTRA','COMBOSERV','TESTDON2')) q
--(select distinct partid from  [vc30].[vc30].[PARAMETER]
--where partid in
-- ('00916484','01031332')) q
 where q.PARTID not  in
   (select vc30.TERMLOG.TERMID from vc30.TERMLOG
 where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
  vc30.TERMLOG.message = 'Download Successful' and
  vc30.TERMLOG.EVDATE > '2019-03-18 11:00:00.000')
