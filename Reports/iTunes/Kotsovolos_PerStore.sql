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

-- Totals per store with totals for Activation + Cancellations
SELECT   storeno,b.[Name],a.EAN,
sum(case when [Type] ='A' then +(cast(([Amount]) as dec(15,2))) else 0 end ) as Activation_Totals,
sum(case when [Type] ='A' then +1 else 0 end ) as Activation_Sum,
sum(case when [Type] ='D' then +1 else 0 end ) as Cancelled_Sum
      -- , sum(cast(([Amount]) as dec(15,2))) as 'Amount', count(*) as 'Totals'
FROM [iTunes].[dbo].[TRANSACTIONS_ALL] a
join [dbo].[PRODUCTS] b on a.[EAN] = b.[EAN]
where customer = 'Kotsovolos Dixons GR'
--and [Type]= 'A'
and [TrxDate] >= '2018-10-01'
--and amount = '0.00'
--and storeno ='11'
group by [Customer],storeno,a.EAN,b.[Name]
order by storeno,b.[Name]
