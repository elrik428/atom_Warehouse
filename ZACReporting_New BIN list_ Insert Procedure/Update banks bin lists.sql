--EUROBANK
select b.[Column 0]from [dbo].[EBNK_lnBINs] b
--ALPHA
SELECT [Column 0]
  FROM [ZacReporting].[dbo].[alpha_BINs]

-- ABC
SELECT [Column 0]
  FROM [ZacReporting].[dbo].ABC_bins

-- Exist on both files
select * from abc096.Products a
where BANKID = '3' and  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
order by BIN


-- BINs that exist in new BIN file and on the same time exist in Products table. So they need to be updated to the desired bank
select * from abc096.Products a
where exists
(
select substring(b.[Column 0],1,6)  from [dbo].ABC_bins b where not exists (select a.BIN from abc096.Products a where substring(b.[Column 0],1,6) = a.BIN  and BANKID = '1') 
and a.BIN = substring(b.[Column 0],1,6)
group by substring(b.[Column 0],1,6)
)
order by BIN


--BINs that don't exist and needs to be inserted manually
select * from abc096.Products a
where not exists
(
select substring(b.[Column 0],1,6)  from [dbo].ABC_bins b where not exists (select a.BIN from abc096.Products a where substring(b.[Column 0],1,6) = a.BIN  /*and BANKID = '1'*/)
--and a.BIN = substring(b.[Column 0],1,6)
group by substring(b.[Column 0],1,6)
)

-- To be removed 
select BIN from abc096.Products a
where BANKID = '3' and not  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
order by BIN

-- To insert
select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where not exists (select a.BIN from abc096.Products a where substring(b.[Column 0],1,6) = a.BIN  and BANKID = '3')
group by substring(b.[Column 0],1,6)


-- Update the ones that already exist in Products with desired bank
update abc096.Products
set BANKID = '1'
where bin in(
select BIN from abc096.Products a
where exists
(
select substring(b.[Column 0],1,6)  from [dbo].ABC_bins b where not exists (select a.BIN from abc096.Products a where substring(b.[Column 0],1,6) = a.BIN  and BANKID = '1') 
and a.BIN = substring(b.[Column 0],1,6)
group by substring(b.[Column 0],1,6)
))


-- Update the ones that needs to be removed from updated bank with 0 in BANKID to PRODUCTS
update abc096.Products
set BANKID = '0'
where bin in 
(
select BIN from abc096.Products a
where BANKID = '3' and not  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
) 