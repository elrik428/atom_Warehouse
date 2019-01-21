select b.CLUSTERID, count(*)
from
(select  CLUSTERID,TERMID  from vc30.relation
group by CLUSTERID, TERMID) b
group by b.CLUSTERID
