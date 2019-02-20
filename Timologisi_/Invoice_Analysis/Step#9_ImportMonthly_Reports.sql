--- Reports from Import monthly.SQL

-- DutyFree_190101_190131.xls

DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120002800'

Delete from [abc096].[Transactions]

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

Delete from [abc096].[IMP_TRANSACT_D]


SELECT  [Group]
      ,[Date]
      ,[PROCBATCH]
      ,[Category]
      ,[BatchePOS]
      ,[Batch]
      ,[Card]
      ,[ManEntry]
      ,[Type]
      ,[Amount]
      ,[Installments]
      ,[Transaction]
      ,[Response]
      ,[Reversed]
      ,[Time]
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
FROM [ZacReporting].[dbo].[ePOSBatchRep_En]
WHERE [Group]='DutyFree'
ORDER BY [Date],[Time]

-------         ***    //////    ****

----FFGROUP_190101_190131.xls
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120002300'

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

SELECT  [Group]
      ,[Date]
      ,[PROCBATCH]
      ,[Category]
      ,[BatchePOS]
      ,[Batch]
      ,[Card]
      ,[ManEntry]
      ,[Type]
      ,[Amount]
      ,[Installments]
      ,[Transaction]
      ,[Response]
      ,[Reversed]
      ,[Time]
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
  FROM [ZacReporting].[dbo].[ePOSBatchRep_En]
  WHERE [Group] = 'FFGROUP' 
  ORDER BY [Date],[Time]

-------         ***    //////    ****

-- Ellinikes_Dianomes.xls  /  PLOIA
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120003510'

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

SELECT  [Group]
      ,[Date]
      ,[PROCBATCH]
      ,[Category]
      ,[BatchePOS]
      ,[Batch]
      ,[Card]
      ,[ManEntry]
      ,[Type]
      ,[Amount]
      ,[Installments]
      ,[Transaction]
      ,[Response]
      ,[Reversed]
      ,[Time]
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
FROM [ZacReporting].[dbo].[ePOSBatchRep_En]
WHERE [Group]='Ellinikes_Dianomes'
ORDER BY [Date],[Time]

-------         ***    //////    ****

-- Ellinikes_Dianomes.xls   
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120003500'

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

SELECT  [Group]
      ,[Date]
      ,[PROCBATCH]
      ,[Category]
      ,[BatchePOS]
      ,[Batch]
      ,[Card]
      ,[ManEntry]
      ,[Type]
      ,[Amount]
      ,[Installments]
      ,[Transaction]
      ,[Response]
      ,[Reversed]
      ,[Time]
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
FROM [ZacReporting].[dbo].[ePOSBatchRep_En]
WHERE [Group]='Ellinikes_Dianomes'
ORDER BY [Date],[Time]

-------         ***    //////    ****

-- NOTOSMonth.xlsx + NOTOSMonthECOMMERCE.xlsx
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID like '0000001100002%'
 or MID = '000000110000300' 
 or MID = '000000078000000' --add notos ecommerce

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

--- NOTOSMonth.xlsx 
SELECT ePOSBatchRep.[Group], ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Κατηγορία, ePOSBatchRep.ΠακέτοePOS, ePOSBatchRep.Πακέτο, ePOSBatchRep.Κάρτα, ePOSBatchRep.Brand, ePOSBatchRep.Πληκτρ, ePOSBatchRep.Τύπος, ePOSBatchRep.Ποσό, ePOSBatchRep.Δόσεις, ePOSBatchRep.Συναλλαγή, ePOSBatchRep.Απόκριση, ePOSBatchRep.Αντιλογισμός, ePOSBatchRep.PROCESSED, ePOSBatchRep.Shop, ePOSBatchRep.Mid, ePOSBatchRep.TID, ePOSBatchRep.DMID, ePOSBatchRep.DTID, ePOSBatchRep.ePOSBATCH, ePOSBatchRep.PROCBATCH
FROM ePOSBatchRep
WHERE ePOSBatchRep.[Group]='NOTOSKENTRIKA' OR ePOSBatchRep.[Group]='NOTOSRetail' OR ePOSBatchRep.[Group]='NOTOSCOSMETICS'
ORDER BY ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα;

---- NOTOSMonthECOMMERCE.xlsx
SELECT ePOSBatchRep.[Group], ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Κατηγορία, ePOSBatchRep.ΠακέτοePOS, ePOSBatchRep.Πακέτο, ePOSBatchRep.Κάρτα, ePOSBatchRep.Brand, ePOSBatchRep.Πληκτρ, ePOSBatchRep.Τύπος, ePOSBatchRep.Ποσό, ePOSBatchRep.Δόσεις, ePOSBatchRep.Συναλλαγή, ePOSBatchRep.Απόκριση, ePOSBatchRep.Αντιλογισμός, ePOSBatchRep.PROCESSED, ePOSBatchRep.Shop, ePOSBatchRep.Mid, ePOSBatchRep.TID, ePOSBatchRep.DMID, ePOSBatchRep.DTID, ePOSBatchRep.ePOSBATCH, ePOSBatchRep.PROCBATCH
FROM ePOSBatchRep
WHERE ePOSBatchRep.[Group]='NOTOSECOMMERCE'
ORDER BY ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα;


-------         ***    //////    ****

-- AUTOTECHNICA.xlsx
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

insert into [ZacReporting].[abc096].[IMP_TRANSACT_D]
select * from abc096.IMP_TRANSACT_D_monthly
where MID in ('000000120004700','000000120004800','000000120004750')

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

SELECT ePOSBatchRep.[Group], ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα, ePOSBatchRep.Κατηγορία, ePOSBatchRep.ΠακέτοePOS, ePOSBatchRep.Πακέτο, ePOSBatchRep.Κάρτα, ePOSBatchRep.Brand, ePOSBatchRep.Πληκτρ, ePOSBatchRep.Τύπος, ePOSBatchRep.Ποσό, ePOSBatchRep.Δόσεις, ePOSBatchRep.Συναλλαγή, ePOSBatchRep.Απόκριση, ePOSBatchRep.Αντιλογισμός, ePOSBatchRep.PROCESSED, ePOSBatchRep.Shop, ePOSBatchRep.Mid, ePOSBatchRep.TID, ePOSBatchRep.DMID, ePOSBatchRep.DTID, ePOSBatchRep.ePOSBATCH, ePOSBatchRep.PROCBATCH
FROM ePOSBatchRep
WHERE ePOSBatchRep.[Group]='AUTOTECHNICA'
ORDER BY ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα;


-------         ***    //////    ****

---- INTERSPORTmonthly.xlsx
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

insert into [ZacReporting].[abc096].[IMP_TRANSACT_D]
select * from abc096.IMP_TRANSACT_D_monthly
where MID like '0000001200019%'

Delete from [abc096].[Transactions]
 
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

Delete from [abc096].[IMP_TRANSACT_D]

SELECT ePOSBatchRep.[Group], ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα, ePOSBatchRep.Κατηγορία, ePOSBatchRep.ΠακέτοePOS, ePOSBatchRep.Πακέτο, ePOSBatchRep.Κάρτα, ePOSBatchRep.Brand, ePOSBatchRep.Πληκτρ, ePOSBatchRep.Τύπος, ePOSBatchRep.Ποσό, ePOSBatchRep.Δόσεις, ePOSBatchRep.Συναλλαγή, ePOSBatchRep.Απόκριση, ePOSBatchRep.Αντιλογισμός, ePOSBatchRep.PROCESSED, ePOSBatchRep.Shop, ePOSBatchRep.Mid, ePOSBatchRep.TID, ePOSBatchRep.DMID, ePOSBatchRep.DTID, ePOSBatchRep.ePOSBATCH, ePOSBatchRep.PROCBATCH, ePOSBatchRep.BONUS_Redemption
FROM ePOSBatchRep
WHERE ePOSBatchRep.[Group] like 'INTERSPORT%'
ORDER BY ePOSBatchRep.Ημερομηνία, ePOSBatchRep.Ώρα;
