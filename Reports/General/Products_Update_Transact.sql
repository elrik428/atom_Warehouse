
--- Curret products file
select
case
when a.BANKID in (1,2,3,4,5,6,7,9,34,15,16,27) then a.BANKID
else 0
end bankid_nbr
,b.BANK, a.BIN,a.BINU,
case
when a.BANKID <> 0  then a.Product +'-'+ b.BANK
when a.BANKID = 0 then a.Product
end as Prodname,
a.Brand,' ' as mark_end

 from abc096.Products a
join abc096.Banks b on  a.BANKID = b.ID
where a.BANKID <> 0


-- New products file with xtra Banks
select a.BANKID
,case
when a.BANKID in (1,2,3,4,5,6,7,9,34,15,16,27) then a.BANKID
else 0
end bankid_nbr
,b.BANK, a.BIN,a.BINU,
case
when a.BANKID <> 0  then a.Product +'-'+ b.BANK
when a.BANKID = 0 then a.Product
end as Prodname,
a.Brand,' ' as mark_end
from dbo.Products_Bup a
join abc096.Banks_new b on  a.BANKID = b.ID
