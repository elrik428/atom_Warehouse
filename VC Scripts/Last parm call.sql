select * from vc30.relation where acccnt=-1 and appnm like 'pira%P'
and (lastfile_dld_date〈'2018-07-05 16:00:00.000' or dld_status〈〉'SUCCESS')
and termid in
(select partid from vc30.parameter where parnameloc='SERVICES_ENABLED' and [value]='TRUE')
