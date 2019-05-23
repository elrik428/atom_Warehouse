--- parnameloc name
INSSCM4     0002:000000000000:1:01:2
INSSCM5     0002:000000010000:1:01:2:12:8
INSSCM6     0002:000000050001:1:01:2:24:8 ->36

-- Update value of parameter
update vc30.PARAMETER set value = '0002:000000010000:1:01:2:12:8' where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and parnameloc = 'INSSCM5' and value = '0002:000000010001:1:01:2:06:8'
--and partid = '73002563'
--
update vc30.PARAMETER set value = '0002:000000050001:1:01:2:24:8' where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and parnameloc = 'INSSCM6' and value = '0002:000000030001:1:01:2:12:8'
--and partid = '73002563'
--
update vc30.PARAMETER set value = '' where partid in
(select termid from vc30.relation where clusterid = 'EPOS_LEROY_MERLIN')
and parnameloc = 'INSSCM7' and value = '0002:000000100001:1:01:2:24:8'
--and partid = '73002563'
