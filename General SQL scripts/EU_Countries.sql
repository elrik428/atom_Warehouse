SELECT [id]
      ,[bin]
      ,[brand]
      ,[bank]
      ,[typeb]
      ,[levelb]
      ,[isocountry]
      ,[isoa2]
      ,[isoa3]
      ,[isonumber]
      ,[www]
      ,[phone]
      ,[regioneu]
  FROM [ZacReporting].[dbo].[binbase]
  where
  -- [regioneu] = 'Y' snd
   isoa2   in ('AT',
'BE',
'CY',
'EE',
'FI',
'FR',
'DE',
'GR',
'IE',
'IT',
'LV',
'LT',
'LU',
'MT',
'NL',
'PT',8
'SK',
'SI',
'ES'
)
