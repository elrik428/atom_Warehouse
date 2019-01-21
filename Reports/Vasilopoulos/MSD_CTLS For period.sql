-- IF view 'dbo.Vasilopoulos_MSD_CTLS' doesn't exist use below sql for main table
select  [TBL]
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
      ,[RRN] from abc096.IMP_TRANSACT_D_2017
where DTSTAMP >='2017-12-15 00:00:00.001' and mid = '000000150000001' union
select  [TBL]
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
      ,[RRN] from abc096.IMP_TRANSACT_D_2018
where DTSTAMP <='2018-01-15 23:59:59.999' and mid = '000000150000001'


select  'CTLS:YES /  MSD:YES','15/12/2017 - 15/01/2018',a.MID,b.Merchant, count(*) Transactions_number, sum(amount) Transactions_Amount
from dbo.Vasilopoulos_MSD_CTLS a
join abc096.mids b on a.MID = b.MID
where a.MID in ('000000150000001')
and substring(a.MASK,1,2) between '50' and '59'  and  substring(POSDATA,1,2) in ('07','91') and AMOUNT >= 15
group by a.MID,b.Merchant
