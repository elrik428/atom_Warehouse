--- sql for selecting which groups have pdf reports
select * from abc096.[GroupReports]
join abc096.[Groups] on abc096.[Groups].[Group] = abc096.[GroupReports].[Group]
where abc096.[GroupReports].Query = '0' and abc096.[GroupReports].Cycle = '3' and abc096.[Groups].run = 1

-- All INVOICE pdf are exported from below query for each GROUP + NOTOSBONUS
-- Basic sql for most reports
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
  WHERE [Group] = 'ABFR0802'


---   Exceptions       //////      --------
---  Where [GROUP] = 'ALL'

-- ePOSMonthTransactions_180101_180131 / pdf 
--
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

-- ePOSMonthTransactions_180101_180131 / csv
-- 
SELECT ePOSMonthTrx5.[Group], ePOSMonthTrx5.Descr, ePOSMonthTrx5.Terminals, ePOSMonthTrx5.MonthRent, ePOSMonthTrx5.TotRent, ePOSMonthTrx5.TrxNo, ePOSMonthTrx5.TrxCost, ePOSMonthTrx5.TotTrxCost, ePOSMonthTrx5.TotCost, ePOSMonthTrx5.Period
FROM ePOSMonthTrx5
WHERE ePOSMonthTrx5.[Group] Not In ("VIVA Tickets","VIVA eCommerce","VIVA Travel","VIVA Retail","VIVA Services","VIVA Telecoms","VIVA Non Profit","VIVA Utility Bills","VIVA Mobile","VIVA Taxis","VIVA Partners","VIVA Gaming","VIVA Card Present","VIVA Hotels");


-- 
-- ALPHA TIDs & ALPHA TRXs / pfd,csv
-- 
-- ALPHATIDs / csv
SELECT ePOSMonthBankTID.BANK, ePOSMonthBankTID.[Group], ePOSMonthBankTID.Shop, ePOSMonthBankTID.DMID, ePOSMonthBankTID.DTID
FROM
(SELECT Banks.BANK, MIDs.[Group], TIDs.Shop, abc096.MERCHANTS.DMID, abc096.MERCHANTS.DTID
FROM (abc096.TIDs RIGHT JOIN (abc096.MIDs INNER JOIN abc096.MERCHANTS ON (MIDs.MID = abc096.MERCHANTS.MID) AND (MIDs.MID = abc096.MERCHANTS.MID)) ON TIDs.TID = abc096.MERCHANTS.TID) INNER JOIN abc096.Banks ON abc096.MERCHANTS.UPLOADHOSTNAME = abc096.Banks.DESTCOMID) ePOSMonthBankTID
WHERE (((ePOSMonthBankTID.BANK)='ALPHA'))
ORDER BY ePOSMonthBankTID.[Group], ePOSMonthBankTID.Shop, ePOSMonthBankTID.DMID, ePOSMonthBankTID.DTID;

-- ALPHATIDs / pdf
SELECT Banks.BANK, MIDs.[Group], TIDs.Shop, abc096.MERCHANTS.DMID, abc096.MERCHANTS.DTID
FROM (abc096.TIDs RIGHT JOIN (abc096.MIDs INNER JOIN abc096.MERCHANTS ON (MIDs.MID = abc096.MERCHANTS.MID) AND (MIDs.MID = abc096.MERCHANTS.MID)) ON TIDs.TID = abc096.MERCHANTS.TID) INNER JOIN abc096.Banks ON abc096.MERCHANTS.UPLOADHOSTNAME = abc096.Banks.DESTCOMID


-- ALPHATrxs / csv
SELECT ePOSMonthBankTrx.MonthGroup, ePOSMonthBankTrx.BANK, ePOSMonthBankTrx.OnUs, ePOSMonthBankTrx.Brand, ePOSMonthBankTrx.TrxNo, ePOSMonthBankTrx.Amount
FROM
(SELECT dbo.VTrxMonthAlpha.PROCESSED, dbo.VTrxMonthAlpha.MonthGroup, dbo.VTrxMonthAlpha.Period, dbo.VTrxMonthAlpha.BANK,
case when (bank) is null then 'Other'
	 else (case 
			when bank  = 'ALPHA' then 'Onus' 
				else 'Other' end) end as OnUs
 ,dbo.VTrxMonthAlpha.Brand, dbo.VTrxMonthAlpha.TrxNo, dbo.VTrxMonthAlpha.Amount
FROM dbo.VTrxMonthAlpha
) ePOSMonthBankTrx
ORDER BY ePOSMonthBankTrx.MonthGroup, ePOSMonthBankTrx.BANK;

---  ALPHATrxs / pdf
SELECT dbo.VTrxMonthAlpha.PROCESSED, dbo.VTrxMonthAlpha.MonthGroup, dbo.VTrxMonthAlpha.Period, dbo.VTrxMonthAlpha.BANK,
case when (bank) is null then 'Other'
	 else (case 
			when bank  = 'ALPHA' then 'Onus' 
				else 'Other' end) end as OnUs
 ,dbo.VTrxMonthAlpha.Brand, dbo.VTrxMonthAlpha.TrxNo, dbo.VTrxMonthAlpha.Amount
FROM dbo.VTrxMonthAlpha


---- KOTSOVOLOSInvoiceBONUS / pdf
-- 
SELECT 'KOTSOVOLOS' AS [Group], Banks.BANK, abc096.TrxMonthKotsovolosBONUS.Date, TIDs.Shop, Sum(abc096.TrxMonthKotsovolosBONUS.TrxNo) AS SumOfTrxNo, abc096.TrxMonthKotsovolosBONUS.Period
FROM (abc096.TIDs RIGHT JOIN abc096.TrxMonthKotsovolosBONUS ON TIDs.TID=abc096.TrxMonthKotsovolosBONUS.TID) INNER JOIN abc096.Banks ON abc096.TrxMonthKotsovolosBONUS.DESTCOMID=Banks.DESTCOMID
GROUP BY Banks.BANK, abc096.TrxMonthKotsovolosBONUS.Date, TIDs.Shop, abc096.TrxMonthKotsovolosBONUS.Period;


--- KOTSOVOLOSInvoiceNTBNLTY / pdf
--
SELECT 'KOTSOVOLOS' AS [Group], Banks.BANK, abc096.TrxMonthKotsovolosNTBNLTY.Date, TIDs.Shop, Sum(abc096.TrxMonthKotsovolosNTBNLTY.TrxNo) AS SumOfTrxNo, abc096.TrxMonthKotsovolosNTBNLTY.Period
FROM (abc096.TIDs RIGHT JOIN abc096.TrxMonthKotsovolosNTBNLTY ON TIDs.TID=abc096.TrxMonthKotsovolosNTBNLTY.TID) INNER JOIN abc096.Banks ON abc096.TrxMonthKotsovolosNTBNLTY.DESTCOMID=Banks.DESTCOMID
GROUP BY Banks.BANK, abc096.TrxMonthKotsovolosNTBNLTY.Date, TIDs.Shop, abc096.TrxMonthKotsovolosNTBNLTY.Period;


--- KOTSOVOLOSTerminals /  pdf
--
SELECT 'KOTSOVOLOS' AS [Group], TIDs.TID, TIDs.Shop, Max(TIDs.MonthRent) AS MaxOfMonthRent, DATENAME(MONTH,(DATEADD(m,-1,GETDATE()))) + ' ' + DATENAME(YEAR,(DATEADD(m,-1,GETDATE()))) AS Period
FROM abc096.MERCHANTS INNER JOIN abc096.TIDs ON abc096.MERCHANTS.TID=TIDs.TID
WHERE (((TIDs.MonthRent)<>0) And ((abc096.MERCHANTS.MID)='000000001100009'))
GROUP BY TIDs.TID, TIDs.Shop
ORDER BY TIDs.Shop;


--- KOTSOVOLOSInvoice / pdf
SELECT 'KOTSOVOLOS' AS [Group], Banks.BANK, abc096.TrxMonthKotsovolos.Date, TIDs.Shop, Sum(abc096.TrxMonthKotsovolos.TrxNo) AS SumOfTrxNo, abc096.TrxMonthKotsovolos.Period
FROM (abc096.TIDs RIGHT JOIN abc096.TrxMonthKotsovolos ON TIDs.TID = abc096.TrxMonthKotsovolos.TID) INNER JOIN abc096.Banks ON abc096.TrxMonthKotsovolos.DESTCOMID = Banks.DESTCOMID
GROUP BY Banks.BANK, abc096.TrxMonthKotsovolos.Date, TIDs.Shop, abc096.TrxMonthKotsovolos.Period;


--- COSMOTEFR_PPC_Invoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='COSMOTE FR' AND Descr='COSMOTE FR-PPC'


  --- COSMOTEOwnInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='COSMOTE'


  ---- GERMANOSFR_PPC_Invoice   /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='GERMANOS FR' AND Descr='GERMANOS FR-PPC'


  ---- GERMANOSOwnInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='GERMANOS' 
  

  ---- IKEAECOMMERCEInvoice  /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='IKEA ECOMMERCE' 
  

  ---- IKEAInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='IKEA' 


  ---  INTERSPORTInvoice  / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='INTERSPORT' 


  --- LeroyMerlinInvoice  /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='LeroyMerlin' 
  

  ----  LIDLInvoice  /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group]='LIDL' 
  
  
  ---- LIFECARDInvoice  /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group] In ('LIFECARD')


  ---- NOTOSBONUS /  pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group] In ('NOTOS BONUS')
  

  --- NOTOSInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group] In ('NOTOS', 'NOTOSRetail')
  


--- OTEOwnInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group] In ('OTE')
  

  --- RaxevskyInvoice / pdf
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
  FROM [ZacReporting].[dbo].[ePOSMonthTrx]
  where [Group] In ('Raxevsky')
  

  ---  VASILOPOULOSInvoiceDetail / pdf
SELECT 'VASILOPOULOS Detailed Invoice' AS Descr, TIDs.Shop, Sum(abc096.TrxMonthVasilopoulos.TrxNo) AS SumOfTrxNo, abc096.TrxMonthVasilopoulos.Period, 'VASILOPOULOS' AS [Group]
FROM AB_Stores RIGHT JOIN ((abc096.TrxMonthVasilopoulos LEFT JOIN abc096.TIDs ON abc096.TrxMonthVasilopoulos.TID = TIDs.TID) INNER JOIN abc096.Banks ON abc096.TrxMonthVasilopoulos.DESTCOMID = Banks.DESTCOMID) ON AB_Stores.Store = TIDs.Shop
GROUP BY TIDs.Shop, abc096.TrxMonthVasilopoulos.Period
ORDER BY TIDs.Shop;


  
  