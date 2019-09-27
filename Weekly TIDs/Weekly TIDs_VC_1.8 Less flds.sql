declare  @lstDigiTID as varchar
set  @lstDigiTID = '8'

select  distinct  cast(vc30.relation.TERMID as varchar(30)) as TID,
cast(a.[value] as varchar(30)) as MID,
o.[value] as [ΣΥΝΔΕΣΗ]
from vc30.relation
left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC = 'MERCHID'
left join vc30.PARAMETER o on vc30.relation.TERMID=o.PARTID and o.PARNAMELOC = 'MEDIA'
where substring(cast(vc30.relation.appnm as char(10)),9,1) = ('P')
and  vc30.relation.CLUSTERID in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP','PIRAEUS')
AND RIGHT(vc30.relation.TERMID,1) = @lstDigiTID ;
