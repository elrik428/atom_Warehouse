DELETE fROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]

INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
select * from [ZacReporting].[dbo].[CHALKIADAKIS_EXCLUDED]

INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
select * from [ZacReporting].[dbo].[VEROPOULOS_EBNKLTY_TEMP]



INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE DESTCOMID='NET_PBGLTY'

INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE DESTCOMID='NET_NTBNLTY' AND PROCCODE IN ('000000','020000')

INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE DESTCOMID='NET_EBLY1' OR DESTCOMID='NET_EBLY'

INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE USERDATA IS NOT NULL AND USERDATA<>'000000000000000' AND USERDATA<>''


-- VIVA
INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE left(MID,11)='00000008380' OR MID='000000160000001'

-- AUTOTECHNICA
INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE MID IN ('000000120004700','000000120004800')
and tcode not in (select tcode from [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp])

-- RVU DISTRIBUTION
INSERT INTO [ZacReporting].[dbo].[TRANSLOG_TRANSACT_temp]
SELECT *  FROM [ZacReporting].[dbo].[TRANSLOG_TRANSACT]
WHERE MID IN ('000000120005700','000000120005710','000000120005720','000000120005730')