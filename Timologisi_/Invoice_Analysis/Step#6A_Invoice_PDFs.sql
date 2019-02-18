-- All INVOICE pdf are exported from below query for each GROUP
-- XXXXXXXXInvoice_180101_180131.PDF

SELECT [Group]
      ,[Descr]
      ,[Terminals]
      ,[MonthRent]
      ,[TotRent]
      ,[TrxNo]
      ,[TrxCost]
      ,[TotTrxCost]
      ,[TotCost]
      ,[Period]
      ,[DateF]
      ,[DateT]
  FROM [ZacReporting].[dbo].[ePOSMonthTrx5]
  --WHERE [Group] = 'ABFR0802'


---   Exceptions       //////      --------

-- ePOSMonthTransactions_180101_180131
SELECT [Group]
      ,[Descr]
      ,[Terminals]
      ,[MonthRent]
      ,[TotRent]
      ,[TrxNo]
      ,[TrxCost]
      ,[TotTrxCost]
      ,[TotCost]
      ,[Period]
      ,[DateF]
      ,[DateT]
  FROM [ZacReporting].[dbo].[ePOSMonthTrx5]
  WHERE [Group] Not In ("VIVA Tickets","VIVA eCommerce","VIVA Travel","VIVA Retail","VIVA Services","VIVA Telecoms","VIVA Non Profit","VIVA Utility Bills","VIVA Mobile","VIVA Taxis","VIVA Partners","VIVA Gaming","VIVA Card Present","VIVA Hotels")

  -- 

