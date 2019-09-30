select a.BANKID, b.BANK,a.BIN,a.BINU,
case 
when a.BANKID <> '0' then b.bank +' - ' +   a.Product 
else a.Product
end
,a.Brand from abc096.Products a
left join abc096.Banks b on a.BANKID = b.ID
--where a.BANKID = '0'
order by len(a.BIN)