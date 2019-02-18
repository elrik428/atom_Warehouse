--- Reports from Import monthly.SQL

-- Duty Free
-- i.
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

-- ii.
INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120002800'

-- iii.
Delete from [abc096].[Transactions]

-- iv. 
Insert into [abc096].[Transactions] (TBL, TCODE, MID, TID, MASK, AMOUNT, CURR, 
                               INST, GRACE, ORIGINATOR, DESTCOMID, PROCCODE, 
                               MSGID, RESPKIND, REVERSED, BTBL, BTCODE, PROCBATCH, 
                               ePOSBATCH, POSDATA, BPOSDATA, DTSTAMP, BDTSTAMP, 
                               ORGSYSTAN, DTSTAMP_INSERT, ProductID, 
                               USERDATA,BORGSYSTAN,AUTHCODE,CASHIERINFO,DMID,DTID, 
                               [CASHBACK],[BONUSRED],[PBGAMOUNT],[RRN]) 
select TBL, TCODE, MID, TID, MASK, AMOUNT, CURR, 
                               INST, GRACE, ORIGINATOR, DESTCOMID, PROCCODE, 
                               MSGID, RESPKIND, REVERSED, BTBL, BTCODE, PROCBATCH, 
                               ePOSBATCH, POSDATA, BPOSDATA, DTSTAMP, BDTSTAMP, 
                               ORGSYSTAN, DTSTAMP_INSERT, ProductID, 
                               USERDATA,BORGSYSTAN,AUTHCODE,CASHIERINFO,DMID,DTID, 
                               [CASHBACK],[BONUSRED],[PBGAMOUNT],[RRN] 
from [abc096].[IMP_TRANSACT_D]

-- v.
Delete from [abc096].[IMP_TRANSACT_D]

-- vi.
SELECT  [Group]
      ,[DateST]
      ,[PROCBATCH]
      ,[Category]
      ,[PackageePOS]
      ,[Package]
      ,[CardNbr]
      ,[Keybrd]
      ,[CardType]
      ,[AmountTRx]
      ,[Instals]
      ,[Transaction]
      ,[Response]
      ,[REVERSED]
      ,[TimeST]
      ,[TID]
      ,[PROCESSED]
      ,[Shop]
      ,[MID]
      ,[DMID]
      ,[DTID]
      ,[ePOSBATCH]
      ,[Brand]
      ,[BTCODE]
      ,[ORGSYSTAN]
      ,[DTSTAMP_INSERT]
      ,[BORGSYSTAN]
      ,[AUTHCODE]
      ,[CASHIERINFO]
      ,[TrDMID]
      ,[TrDTID]
      ,[TrPackage]
      ,[PEM_DESC]
      ,[BONUS_Redemption]
      ,[DTSTAMP]
  FROM [ZacReporting].[dbo].[ePOSBatchRep]
  WHERE [Group] = 'DutyFree'