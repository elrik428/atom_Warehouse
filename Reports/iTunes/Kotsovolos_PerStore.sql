--- Totals per StoreNo
SELECT   storeno, sum(cast(([Amount]) as dec(15,2))), count(*)
FROM [iTunes].[dbo].[TRANSACTIONS_ALL] a
--join [dbo].[PRODUCTS] b on a.[EAN] = b.[EAN]
where customer = 'Kotsovolos Dixons GR'
and [Type]= 'A'
and [TrxDate] > '2018-10-01'
group by [Customer],storeno
order by storeno


----- Totals per StoreNo + Product
SELECT   storeno,b.[Name],a.EAN
       , sum(cast(([Amount]) as dec(15,2))) as 'Amount', count(*) as 'Totals'
FROM [iTunes].[dbo].[TRANSACTIONS_ALL] a
join [dbo].[PRODUCTS] b on a.[EAN] = b.[EAN]
where customer = 'Kotsovolos Dixons GR'
and [Type]= 'A'
and [TrxDate] >= '2018-10-01'
--and amount = '0.00'
--and storeno ='11'
group by [Customer],storeno,a.EAN,b.[Name]
order by storeno,b.[Name]
