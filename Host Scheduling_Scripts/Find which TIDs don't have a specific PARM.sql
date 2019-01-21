--- 1.
select CLUSTERID, TERMID
from
(select a.partid from vc30.PARAMETER a
where not exists
	(select partid from vc30.PARAMETER b
	where
	b.PARNAMELOC = 'LOCK_TRX_TIME'
	AND substring(b.appnm,1,4) = ('PIRA')
	and substring(b.appnm,9,1) = ('P')
	and substring(b.appnm,6,1) = ('2')
	and substring(b.appnm,8,1) = ('3')
	and b.partid = a.partid
	group by b.partid)
 and
 substring(a.appnm,1,4) = ('PIRA')
 and substring(a.appnm,9,1) = ('P')
 and substring(a.appnm,6,1) = ('2')
 and substring(a.appnm,8,1) = ('3')
 group by a.partid) q
 join vc30.relation c  on q.partid = c.termid
 where clusterid = 'PIRAEUS'
 group by CLUSTERID, TERMID



 select c.CLUSTERID, TERMID, datecreated
 from
 (select a.partid from vc30.PARAMETER a
 where not exists
        (select partid from vc30.PARAMETER b
        where
        b.PARNAMELOC = 'LOCK_TRX_TIME'
        AND substring(b.appnm,1,4) = ('PIRA')
        and substring(b.appnm,9,1) = ('P')
        and substring(b.appnm,6,1) = ('2')
        and substring(b.appnm,8,1) = ('3')
        and b.partid = a.partid
        group by b.partid)
 and substring(a.appnm,1,4) = ('PIRA')
 and substring(a.appnm,9,1) = ('P')
 and substring(a.appnm,6,1) = ('2')
 and substring(a.appnm,8,1) = ('3')

 group by a.partid) q
 join vc30.relation c  on q.partid = c.termid
 join vc30.TERMINFO on c.TERMID=termid_term
 where c.clusterid = 'PIRAEUS'
 group by c.CLUSTERID, TERMID, datecreated


-- 2. 
 select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
 TERMID in
 ('01416930')
 group by CLUSTERID,famnm,appnm
 ORDER BY CLUSTERID,famnm,appnm

 select FAMNM, PARTID, [value] from vc30.PARAMETER where PARTID in
 ('01416930')
 and (PARNAMELOC = 'MCC'
 OR PARNAMELOC = 'LOCK_TRX_TIME')
 ORDER BY FAMNM ,[value]

 select FAMNM, PARTID, [value] from vc30.PARAMETER where PARTID in
 ('01416930')
 and PARNAMELOC = 'LOCK_TRX_TIME'
 ORDER BY FAMNM ,[value]
