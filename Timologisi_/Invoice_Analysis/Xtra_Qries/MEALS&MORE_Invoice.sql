
select a.mid,
--a.dmid, a.DTID,
b.Merchant,
--DTSTAMP,
AMOUNT,DESTCOMID,AUTHCODE  from abc096.IMP_TRANSACT_D_monthly a
join abc096.MIDs b on a.MID = b.MID
where substring(mask,1,6) = '502259'
