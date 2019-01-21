select e.*,
case (substring(e.vers_pir,9,1)) when ',' then substring(e.vers_pir,1,8) + 'P' when 'P' then (e.vers_pir) end as late_versio
case (substring(e.vers_pir,9,1)) when ',' then e.vers_pir + replace(substring(e.vers_pir,9,1), ',', 'P') when 'P' then (e.vers_pir + '') end as late_versio
from
(select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('PIRA', a.appnm),9) vers_pir
from vc30.termlog a,(select  max(evdate) date_max, termid  from vc30.termlog
where RIGHT(vc30.termlog.TERMID,1) = '8' and message = 'Download Successful' and status = 'S'
and termid =  '00000788'
group by termid) q
where  a.evdate = q.date_max and a.termid = q.termid
and appnm like '%PIRA%'

and a.termid = '00000788') e
