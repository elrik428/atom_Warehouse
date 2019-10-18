--EUROBANK
select b.[Column 0]from [dbo].[EBNK_lnBINs] b
--ALPHA
SELECT [Column 0]
  FROM [ZacReporting].[dbo].[alpha_BINs]

-- Exist on both files
select * from abc096.Products a
where BANKID = '3' and  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
order by BIN

-- To be removed 
select BIN from abc096.Products a
where BANKID = '3' and not  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
order by BIN

-- To insert
select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where not exists (select a.BIN from abc096.Products a where substring(b.[Column 0],1,6) = a.BIN  and BANKID = '3')
group by substring(b.[Column 0],1,6)

-- Update non exist with 0 in BANKID to PRODUCTS
update abc096.Products
set BANKID = '0'
where bin in 
(
select BIN from abc096.Products a
where BANKID = '3' and not  exists(select substring(b.[Column 0],1,6)  from [dbo].[EBNK_lnBINs] b where a.BIN = substring(b.[Column 0],1,6) group by substring(b.[Column 0],1,6))
) 