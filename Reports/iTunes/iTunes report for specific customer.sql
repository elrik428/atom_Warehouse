SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN]
,AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]
 FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN)
 where customer like '%LIDL%' AND [Category]='Google'
 --where customer like '%LIDL%' AND [Category]='Apple'
 --where customer like '%LIDL%' AND [Category]='TopUp'
ORDER BY [Customer],[TrxDate],[TrxTime] 
