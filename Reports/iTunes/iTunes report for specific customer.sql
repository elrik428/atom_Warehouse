SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN]
,AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]
 FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN)
 where customer like '%LIDL%' AND [Category]='Google' and trxdate > '2019-04-14' and trxdate < '2019-04-22'
 --where customer like '%LIDL%' AND [Category]='Apple'
 --where customer like '%LIDL%' AND [Category]='TopUp'
ORDER BY [Customer],[TrxDate],[TrxTime]

SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS_ALL].[EAN],
 AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]
FROM [iTunes].[dbo].[TRANSACTIONS_ALL] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS_ALL].EAN)
WHERE
  customer like '%CY%' AND [Category]='Xbox'
 --customer like '%LIDL%' AND [Category]='Apple'
 --customer like '%LIDL%' AND [Category]='TopUp'
 and trxdate > '2019-05-05' and trxdate < '2019-05-13'
ORDER BY [Customer],[TrxDate],[TrxTime]
