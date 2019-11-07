--DUTY FREE

DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120002800'

UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] set DESTCOMID = NULL where DESTCOMID in ('',' ')

--FFGROUP
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120002300'

--ELLINIKES DIANOMES PLOIA
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120003510'

--ELLINIKES DIANOMES
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID='000000120003500'


--NOTOS
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

INSERT INTO [ZacReporting].[abc096].[IMP_TRANSACT_D]
 SELECT * FROM [ZacReporting].[abc096].[IMP_TRANSACT_D_monthly] WHERE MID like '0000001100002%'
 or MID = '000000110000300' or MID = '000000110000301' or MID = '000000110000302'
 or MID = '000000078000000' --add notos ecommerce


------Insert report11 in [IMP_TRANSACT_D]
--  Run NOTOSMonthTicketRest for monthly


--autotechnica
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

insert into [ZacReporting].[abc096].[IMP_TRANSACT_D]
select * from abc096.IMP_TRANSACT_D_monthly
where MID in ('000000120004700','000000120004800','000000120004750')

--  Run daily for the whole month

--intersport
DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]

insert into [ZacReporting].[abc096].[IMP_TRANSACT_D]
select * from abc096.IMP_TRANSACT_D_monthly
where MID like '0000001200019%'

--  Run INTERSPORTmonthly for monthly
