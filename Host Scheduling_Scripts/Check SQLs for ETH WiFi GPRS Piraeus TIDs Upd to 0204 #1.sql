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
