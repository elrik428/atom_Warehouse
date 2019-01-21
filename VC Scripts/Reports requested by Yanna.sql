select a.FAMNM, a.PARTID,  a.VALUE, b.VALUE from
(select * from vc30.PARAMETER
where PARNAMELOC = 'MEDIA' and [value] in ('ETH','WIFI','GPRS')
and substring (PARTID,1,1) in ('0','1')) a
join vc30.PARAMETER b on a.PARTID = b.PARTID
where b.PARNAMELOC = 'HOST_IP' and b.value in ('195.145.098.210','195.226.126.050')
group by a.FAMNM, a.PARTID,  a.VALUE, b.VALUE
order by a.PARTID
--having count(*) >1


select z.FAMNM ,z.PARTID, z.valIP HOSTIP1 , d.VALUE HOSTIP2 from
 (select a.FAMNM, a.PARTID,  a.VALUE valMED, b.VALUE valIP from
 (select * from vc30.PARAMETER
 where PARNAMELOC = 'MEDIA' and [value] in ('ETH','WIFI','GPRS')
 and substring (PARTID,1,1) in ('0','1')) a
 join vc30.PARAMETER b on a.PARTID = b.PARTID
 where b.PARNAMELOC = 'HOST_IP'
 group by a.FAMNM, a.PARTID,  a.VALUE, b.VALUE)  z
 join vc30.PARAMETER d on z.PARTID = d.PARTID
 where d.PARNAMELOC = 'HOST_IP2'  and d.VALUE = z.valIP
 order by d.PARTID
--having count(*) >1
