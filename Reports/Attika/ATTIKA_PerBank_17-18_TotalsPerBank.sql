-- A

select DESTCOMID,sum(amount), count(*)
from abc096.IMP_TRANSACT_D_tempLN
where RESPKIND = 'OK'
group by DESTCOMID
