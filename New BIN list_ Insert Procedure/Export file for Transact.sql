select a.BANKID, b.BANK,a.BIN,a.BINU,a.Product ,a.Brand from abc096.Products a
left join abc096.Banks b on a.BANKID = b.ID
order by len(a.BIN)