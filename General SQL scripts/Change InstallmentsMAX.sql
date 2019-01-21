1.
select * from merchbins where tid  in (select tid from merchants where mid = ('000000120002100')) and instmax = 24
--group by instmax
update merchbins set instmax = 36 where tid  in (select tid from merchants where mid = ('000000120002100')) and instmax = 24

2.
select gmaxinst, uploadhostid, count(*) from merchants where mid = ('000000120002100')
group by gmaxinst, uploadhostid
update merchants set gmaxinst=36  where mid = ('000000120002100')

3.
select famnm,partid,parnameloc, value from vc30.PARAMETER where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and (parnameloc like ('INSBIN%') or parnameloc like ('INSSCM%'))
and partid = '73002563'
order by partid,parnameloc

select famnm,
--partid,
parnameloc, count(*)--, value
 from vc30.PARAMETER where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and (parnameloc like ('INSBIN%') or parnameloc like ('INSSCM%'))
--and partid = '73002563'
group by famnm,parnameloc
--order by partid,parnameloc


4.

INSSCM4           0002:000000000000:1:01:2                                0 €        100 €                   0
INSSCM5           0002:000000010001:1:01:2:12:8                      100.01 €        300 €                  12
INSSCM6           0002:000000030001:1:01:2:24:8                      300.01 €        500 €                  24
INSSCM7           0002:000000050001:1:01:2:36:8                      500.01 €         ….                    36


update vc30.PARAMETER set value = '0002:000000050001:1:01:2:36:8' where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and parnameloc = 'INSSCM7' and value = ''
