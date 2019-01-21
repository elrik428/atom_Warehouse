-- 1. Insert Data

USE [ZacReporting]
GO

delete from [abc096].[IMP_TRANSACT_D_tempLN]

INSERT INTO [abc096].[IMP_TRANSACT_D_tempLN]
           ([TBL]
           ,[TCODE]
           ,[MID]
           ,[TID]
           ,[MASK]
           ,[AMOUNT]
           ,[CURR]
           ,[INST]
           ,[GRACE]
           ,[ORIGINATOR]
           ,[DESTCOMID]
           ,[PROCCODE]
           ,[MSGID]
           ,[RESPKIND]
           ,[REVERSED]
           ,[BTBL]
           ,[BTCODE]
           ,[PROCBATCH]
           ,[ePOSBATCH]
           ,[POSDATA]
           ,[BPOSDATA]
           ,[DTSTAMP]
           ,[BDTSTAMP]
           ,[ORGSYSTAN]
           ,[DTSTAMP_INSERT]
           ,[ProductID]
           ,[USERDATA]
           ,[BORGSYSTAN]
           ,[AUTHCODE]
           ,[CASHIERINFO]
           ,[DMID]
           ,[DTID]
           ,[CASHBACK]
           ,[BONUSRED]
           ,[PBGAMOUNT]
           ,[RRN]
           ,[DCC_CURRENCY]
           ,[DCC_AMOUNT]
           ,[DCCCHOSEN_DCCELIGIBLE])
    select [TBL]
           ,[TCODE]
           ,[MID]
           ,[TID]
           ,[MASK]
           ,[AMOUNT]
           ,[CURR]
           ,[INST]
           ,[GRACE]
           ,[ORIGINATOR]
           ,[DESTCOMID]
           ,[PROCCODE]
           ,[MSGID]
           ,[RESPKIND]
           ,[REVERSED]
           ,[BTBL]
           ,[BTCODE]
           ,[PROCBATCH]
           ,[ePOSBATCH]
           ,[POSDATA]
           ,[BPOSDATA]
           ,[DTSTAMP]
           ,[BDTSTAMP]
           ,[ORGSYSTAN]
           ,[DTSTAMP_INSERT]
           ,[ProductID]
           ,[USERDATA]
           ,[BORGSYSTAN]
           ,[AUTHCODE]
           ,[CASHIERINFO]
           ,[DMID]
           ,[DTID]
           ,[CASHBACK]
           ,[BONUSRED]
           ,[PBGAMOUNT]
           ,[RRN]
           ,[DCC_CURRENCY]
           ,[DCC_AMOUNT]
           ,[DCCCHOSEN_DCCELIGIBLE]
		   from abc096.IMP_TRANSACT_D_2018
		   where mid in ('000000001100001','000000001100010','000000001100011','000000001100013','000000001100014','000000001100015','000000001100016','000000001100017','B00000001100001','B00000001100010','B00000001100011','B00000001100013','B00000001100014','B00000001100015','B00000001100016','B00000001100017')
		   and dtstamp>='2018-04-10 00:00:00.001' and dtstamp <'2018-05-07 00:00:00.001' and substring(MASK,1,2) between '50' and '59'
GO



---- 2. Export report

SELECT        TOP (100) PERCENT [Group], CONVERT(nvarchar(10), DTSTAMP, 103) AS DateST, PROCBATCH, CASE WHEN reversed = 'A' OR
                         reversed = 'F' THEN 'Εκτός Πακέτου' ELSE (CASE WHEN ePOSBATCH IS NULL THEN (CASE WHEN PROCBATCH IS NULL
                         THEN 'Εκτός Πακέτου' ELSE 'Εντός Πακέτου' END) ELSE 'Εντός Πακέτου' END) END AS Category, CASE WHEN reversed = 'A' OR
                         reversed = 'F' THEN 'Εκτός Πακέτου EURONET' ELSE (CASE WHEN (ePOSBATCH IS NULL) THEN (CASE WHEN (PROCBATCH IS NULL)
                         THEN 'Εκτός Πακέτου EURONET' ELSE 'Εντός Πακέτου EURONET' END) ELSE 'Εντός Πακέτου EURONET' END) + (CASE WHEN ePOSBATCH IS NULL
                         THEN ' ' ELSE CAST(ePOSBATCH AS char) END) END AS PackageePOS, DMID + ' / ' + DTID + CASE WHEN reversed = 'A' OR
                         reversed = 'F' THEN ' ' ELSE (CASE WHEN procbatch IS NULL THEN ' ' ELSE ' / ' + CAST(procbatch AS char) END) END AS Package,
                         CASE WHEN dataentry = 'True' THEN 'T' + CAST(MASK AS char) ELSE 'D' + CAST(MASK AS char) END AS CardNbr, CASE WHEN CAST(dataentry AS char)
                         = 'ΠΛΗΚΤΡ' THEN ' ' END AS Keybrd, CASE WHEN bank IS NULL THEN Brand ELSE bank + '/' + product END AS CardType, Amount AS AmountTRx, INST AS Instals,
                         RTRIM(MSG + ' ' + ACT) AS [Transaction], RESPKIND AS Response, REVERSED, CONVERT(time(0), DTSTAMP) AS TimeST, TID, PBank AS PROCESSED, Shop, MID,
                         DMID, DTID, ePOSBATCH, Brand, BTCODE, ORGSYSTAN, DTSTAMP_INSERT, BORGSYSTAN, AUTHCODE, CASHIERINFO, TrDMID, TrDTID,
                         TrDMID + ' / ' + TrDTID + CASE WHEN reversed = 'A' OR
                         reversed = 'F' THEN ' ' ELSE (CASE WHEN procbatch IS NULL THEN ' ' ELSE ' / ' + CAST(procbatch AS char) END) END AS TrPackage, CASE WHEN LEFT(PEM, 2)
                         = '01' THEN 'MANUAL ENTRY' WHEN LEFT(PEM, 2) = '02' THEN 'MAGNETIC STRIPE' WHEN LEFT(PEM, 2) = '80' THEN 'MAGNETIC STRIPE' WHEN LEFT(PEM, 2)
                         = '05' THEN 'CHIP CONTACT' WHEN LEFT(PEM, 2) = '07' THEN 'CONTACTLESS' WHEN LEFT(PEM, 2) = '91' THEN 'CONTACTLESS' END AS PEM_DESC,
                         CASE WHEN BONUSRED > 0 THEN BONUSRED END AS BONUS_Redemption, DTSTAMP
FROM            (SELECT        abc096.MIDs.[Group], abc096.TIDs.Shop, abc096.IMP_TRANSACT_D_tempLN.MID, abc096.IMP_TRANSACT_D_tempLN.TID, abc096.IMP_TRANSACT_D_tempLN.BTCODE, abc096.IMP_TRANSACT_D_tempLN.ePOSBATCH,
                         PBanks.BANK AS PBank, abc096.MERCHANTS.DMID, abc096.MERCHANTS.DTID, abc096.IMP_TRANSACT_D_tempLN.PROCBATCH,
                         abc096.amDataEntry(abc096.IMP_TRANSACT_D_tempLN.POSDATA) AS DataEntry, abc096.IMP_TRANSACT_D_tempLN.MASK, abc096.Products.Brand, CBanks.BANK, abc096.Products.Product,
                         abc096.amProcodeSign(abc096.IMP_TRANSACT_D_tempLN.PROCCODE) * abc096.IMP_TRANSACT_D_tempLN.AMOUNT AS Amount, abc096.IMP_TRANSACT_D_tempLN.INST,
                         abc096.amMSGID(abc096.IMP_TRANSACT_D_tempLN.MSGID) AS MSG, abc096.amProcodeAction(abc096.IMP_TRANSACT_D_tempLN.PROCCODE) AS ACT, abc096.IMP_TRANSACT_D_tempLN.RESPKIND,
                         abc096.IMP_TRANSACT_D_tempLN.REVERSED, abc096.IMP_TRANSACT_D_tempLN.DTSTAMP, abc096.IMP_TRANSACT_D_tempLN.ORIGINATOR, abc096.IMP_TRANSACT_D_tempLN.PROCCODE,
                         abc096.IMP_TRANSACT_D_tempLN.MSGID, abc096.IMP_TRANSACT_D_tempLN.BDTSTAMP, abc096.IMP_TRANSACT_D_tempLN.TCODE, abc096.IMP_TRANSACT_D_tempLN.ORGSYSTAN,
                         abc096.IMP_TRANSACT_D_tempLN.DTSTAMP_INSERT, abc096.IMP_TRANSACT_D_tempLN.BORGSYSTAN, abc096.IMP_TRANSACT_D_tempLN.AUTHCODE, abc096.IMP_TRANSACT_D_tempLN.CASHIERINFO,
                         abc096.IMP_TRANSACT_D_tempLN.DMID AS TrDMID, abc096.IMP_TRANSACT_D_tempLN.DTID AS TrDTID, abc096.IMP_TRANSACT_D_tempLN.POSDATA AS PEM, abc096.IMP_TRANSACT_D_tempLN.BONUSRED,
                         abc096.IMP_TRANSACT_D_tempLN.DCC_CURRENCY, abc096.IMP_TRANSACT_D_tempLN.DCC_AMOUNT, abc096.IMP_TRANSACT_D_tempLN.DCCCHOSEN_DCCELIGIBLE
FROM            abc096.MIDs INNER JOIN
                         abc096.Groups ON abc096.MIDs.[Group] = abc096.Groups.[Group] RIGHT OUTER JOIN
                         abc096.IMP_TRANSACT_D_tempLN INNER JOIN
                         abc096.Banks AS PBanks ON abc096.IMP_TRANSACT_D_tempLN.DESTCOMID = PBanks.DESTCOMID LEFT OUTER JOIN
                         abc096.Banks AS CBanks RIGHT OUTER JOIN
                         abc096.Products ON CBanks.ID = abc096.Products.BANKID ON abc096.IMP_TRANSACT_D_tempLN.ProductID = abc096.Products.ID LEFT OUTER JOIN
                         abc096.MERCHANTS ON abc096.IMP_TRANSACT_D_tempLN.DESTCOMID = abc096.MERCHANTS.UPLOADHOSTNAME AND abc096.IMP_TRANSACT_D_tempLN.TID = abc096.MERCHANTS.TID AND
                          abc096.IMP_TRANSACT_D_tempLN.MID = abc096.MERCHANTS.MID LEFT OUTER JOIN
                         abc096.TIDs ON abc096.IMP_TRANSACT_D_tempLN.TID = abc096.TIDs.TID ON abc096.MIDs.MID = abc096.IMP_TRANSACT_D_tempLN.MID) q
ORDER BY DateST, PROCESSED, DTSTAMP
