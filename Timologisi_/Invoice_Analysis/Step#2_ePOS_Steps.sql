-- ePOS application

-- Fill TRXMONTH
-- 1.
SELECT MID, TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
FROM dbo.TRANSLOG_TRANSACT
WHERE (LEFT(DESTCOMID,4) = 'NET_') AND (ORIGINATOR NOT IN ('NET_DCEB','NET_DCAB', 'DIAL_03')) AND INTERFACE = 'POS'    
AND DESTCOMID <> 'NET_NTBNLTY' --//do not include ntbnlty 150503
AND DESTCOMID <> 'NET_PBGLTY' --//do not include pbglty 161125
AND (DTSTAMP BETWEEN @fdate AND @tdate)
GROUP BY MID, TID, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 2. 
SELECT ORIGINATOR AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT
 WHERE (LEFT(DESTCOMID,4) = 'NET_')
 AND (ORIGINATOR IN ('NET_DCEB','NET_DCAB', 'DIAL_03'))
 AND (DTSTAMP BETWEEN @fdate AND @tdate)
 AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
 UNION ALL SELECT ORIGINATOR AS MID, ''
 AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT WHERE (LEFT(DESTCOMID,4) = 'NET_') AND
 (ORIGINATOR IN ('NET_PIR')) AND MSGID = '0220' AND TID not like 'DOY%' AND TID not like 'TAX%' AND
 (DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 3. DOY TRANSACTIONS Invoice
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT 'NET_DOY' AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT WHERE (LEFT(DESTCOMID,4) = 'NET_') AND
 (ORIGINATOR IN ('NET_PIR')) AND MSGID = '0220' AND (TID like 'DOY%' OR TID like 'TAX%') AND
 (DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 4.BONUS TRANSACTIONS Invoice
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT 'B'+right(MID,14), TID, DESTCOMID, Count(TCODE), @period, @fdate, @tdate 
FROM dbo.TRANSLOG_TRANSACT 
where [userdata] is not null and [userdata] <> '' and [userdata] <> '000000000000000' 
and mid<>'000000150000001' and mid<>'000000150000003' and mid<>'000000150000011'
GROUP BY MID, TID,DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 5. NTBNLTY TRANSACTIONS
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT MID, 
TID, DESTCOMID, Count(TCODE), @period, @fdate, @tdate 
FROM dbo.TRANSLOG_TRANSACT 
where DESTCOMID = 'NET_NTBNLTY'
 AND ((PROCCODE IN ('400000','300000') AND MID NOT LIKE '00000017%') OR
(PROCCODE NOT IN ('400000','300000') AND MID LIKE '00000017%'))
GROUP BY MID, TID,DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 6. PBGLTY TRANSACTIONS
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT MID, 
TID, DESTCOMID, Count(TCODE), @period, @fdate, @tdate 
FROM dbo.TRANSLOG_TRANSACT 
where DESTCOMID = 'NET_PBGLTY'
AND PROCCODE ='400000' -- invoice only balance inquiry for all customers
GROUP BY MID, TID,DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 7. LIFECARD TRANSACTIONS
-- i.
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT MID, TID, 'NET_LIFE', Count(*), @period, @fdate, @tdate 
FROM abc096.LIFECARD 
GROUP BY MID, TID,DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- ii.
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT 'LIFECARD' AS MID, '' AS TID,'ALL', Count(*) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
FROM abc096.LIFECARD 
GROUP BY DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 8. 
INSERT INTO [abc096].[TrxMonth]
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT)
SELECT MID, TID, 'ALL', 0, @period, @fdate, @tdate
FROM [abc096].[MERCHANTS]
GROUP BY MID, TID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 9. 
DELETE FROM [abc096].[TrxMonthPeriod]
INSERT INTO [abc096].[TrxMonthPeriod]
(Period, DateF, DateT)
SELECT @period, @fdate, @tdate

-- 10.  MEL ALT TRANSACTIONS
-- i.
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT 'MEL_PIR' AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT WHERE
 (ORIGINATOR IN ('MEL')) AND DESTCOMID='NET_ABC' AND 
 (DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- ii. 
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT 'ALT_PIR' AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT WHERE
 (ORIGINATOR IN ('ALT')) AND DESTCOMID='NET_ABC' AND
 (DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 11. CLX TRANSACTIONS
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT 'CLX_PIR' AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT WHERE
 (ORIGINATOR IN ('CLX')) AND DESTCOMID='NET_ABC' AND 
 (DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
 GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 12. FDH TRANSACTIONS
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
SELECT 'FDH_PIR' AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
FROM dbo.TRANSLOG_TRANSACT WHERE
(ORIGINATOR IN ('FDH')) AND DESTCOMID='NET_ABC' AND 
(DTSTAMP BETWEEN @fdate AND @tdate) AND INTERFACE = 'POS'
GROUP BY ORIGINATOR, DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 13. VIVA PAYMENTS TRANSACTIONS 
-- i.
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT left(MONTHGROUP,15) AS MID, '' AS TID,DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
 FROM dbo.TRANSLOG_TRANSACT JOIN abc096.MIDs on dbo.TRANSLOG_TRANSACT.MID=abc096.MIDs.MID 
 WHERE (dbo.TRANSLOG_TRANSACT.MID like '00000008380%') AND  INTERFACE = 'POS' 
GROUP BY MONTHGROUP,DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- ii. 
 INSERT INTO [abc096].[TrxMonth] 
 (MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
  SELECT left([GROUP],15) AS MID, '' AS TID, DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
  FROM dbo.TRANSLOG_TRANSACT JOIN abc096.MIDs on dbo.TRANSLOG_TRANSACT.MID=abc096.MIDs.MID 
  WHERE (dbo.TRANSLOG_TRANSACT.MID ='000000160000001') AND  INTERFACE = 'POS' 
 GROUP BY [GROUP],DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- 14. MEALS & MORE
INSERT INTO [abc096].[TrxMonth] 
(MID, TID, DESTCOMID, TrxNo, Period, DateF, DateT) 
 SELECT 'MEALS&MORE' AS MID, '' AS TID,DESTCOMID, Count(TCODE) AS TrxNo, @period AS Period, @fdate AS DateF, @tdate1 AS DateT
-- // " FROM dbo.TRANSLOG_TRANSACT " +//Vasilopoulos is not invoiced for meals and more transactions
FROM (SELECT * FROM dbo.VASILOPOULOS_MEALS_TEMP UNION ALL SELECT * FROM dbo.TRANSLOG_TRANSACT) all_trx
WHERE (MASK like '502259%') AND  INTERFACE = 'POS' 
GROUP BY DESTCOMID
-- @period, cycle.Period(), -- value : '01/01/2019 - 31/01/2019' 
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate1, cycle.DateT, value : '2019-01-31 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00


-- 15. KOTSOVOLOS
-- Insert into [abc096].[TrxMonthKotsovolos]
SELECT CONVERT(varchar(10), DTSTAMP, 102) AS [Date],
 DESTCOMID, TID, Count(TCODE) AS TrxNo, '{0}' AS Period, cycle.Period()
FROM dbo.TRANSLOG_TRANSACT
WHERE  (LEFT(DESTCOMID,4) = 'NET_') AND (ORIGINATOR NOT IN ('NET_DCEB','NET_DCAB', 'DIAL_03')) AND INTERFACE = 'POS' 
AND DESTCOMID <> 'NET_NTBNLTY' -- +//do not include ntbnlty 150503
AND DESTCOMID <> 'NET_PBGLTY' -- +//do not include pbglty 161125
AND MID='000000001100009'
AND (DTSTAMP BETWEEN @fdate AND @tdate)
GROUP BY CONVERT(varchar(10), DTSTAMP, 102), DESTCOMID, TID
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- Index table
CREATE UNIQUE INDEX TrxMonthKotsovolos_ix
ON [abc096].[TrxMonthKotsovolos]
([DATE], DESTCOMID, TID)


-- 16. KOTSOVOLOS BONUS
-- Insert into [abc096].[TrxMonthKotsovolosBONUS]
SELECT CONVERT(varchar(10), DTSTAMP, 102) AS [Date],
DESTCOMID, TID, Count(TCODE) AS TrxNo, '{0}' AS Period,cycle.Period()
 FROM dbo.TRANSLOG_TRANSACT
 WHERE  (LEFT(DESTCOMID,4) = 'NET_') AND (ORIGINATOR NOT IN ('NET_DCEB','NET_DCAB', 'DIAL_03')) AND INTERFACE = 'POS' 
 AND DESTCOMID <> 'NET_NTBNLTY' --//do not include ntbnlty 150503
 AND DESTCOMID <> 'NET_PBGLTY' -- //do not include pbglty 161125
 AND MID='000000001100009'
 AND (DTSTAMP BETWEEN @fdate AND @tdate)
 AND (USERDATA is not NULL AND USERDATA<>'' AND USERDATA<>'000000000000000') 
 GROUP BY CONVERT(varchar(10), DTSTAMP, 102), DESTCOMID, TID
 -- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
 -- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

 -- Imdex table
 CREATE UNIQUE INDEX TrxMonthKotsovolosBONUS_ix
 ON [abc096].[TrxMonthKotsovolosBONUS]
 ([DATE], DESTCOMID, TID)


 -- 17. KOTSOVOLOS NTBNLTY
 -- Insert into [abc096].[TrxMonthKotsovolosNTBNLTY]
SELECT CONVERT(varchar(10), DTSTAMP, 102) AS [Date], DESTCOMID, TID, Count(TCODE) AS TrxNo, '{0}' AS Period, cycle.Period()
FROM dbo.TRANSLOG_TRANSACT 
WHERE DESTCOMID = 'NET_NTBNLTY' -- //include ntbnlty 150503
AND MID='000000001100009'
AND (DTSTAMP BETWEEN @fdate AND @tdate)
AND (PROCCODE IN ('400000','300000')) 
GROUP BY CONVERT(varchar(10), DTSTAMP, 102), DESTCOMID, TID
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- Index table
CREATE UNIQUE INDEX TrxMonthKotsovolosNTBNLTY_ix
 ON [abc096].[TrxMonthKotsovolosNTBNLTY]
([DATE], DESTCOMID, TID)


-- 18. VASILOPOULOS
-- Insert into [abc096].[TrxMonthVasilopoulos]
SELECT CONVERT(varchar(10), DTSTAMP, 102) AS [Date], DESTCOMID, TID, Count(TCODE) AS TrxNo, '{0}' AS Period ,cycle.Period()
FROM dbo.TRANSLOG_TRANSACT
 WHERE (LEFT(DESTCOMID,4) = 'NET_') AND (ORIGINATOR NOT IN ('NET_DCEB','NET_DCAB', 'DIAL_03')) AND INTERFACE = 'POS' 
 AND MID='000000150000001' OR MID='000000150000011'
 AND (DTSTAMP BETWEEN @fdate AND @tdate)
 GROUP BY CONVERT(varchar(10), DTSTAMP, 102), DESTCOMID, TID
-- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
-- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

-- Index table
CREATE UNIQUE INDEX TrxMonthVasilopoulos_ix
 ON [abc096].[TrxMonthVasilopoulos]
([DATE], DESTCOMID, TID)


-- 19. ALPHA
-- Insert into [abc096].[TrxMonthAlpha]

--declare Transact2 = " MID, BIN, PROCCODE "
--declare Transact3 = " MID, CAST(LEFT(MASK, 6) AS INT) AS BIN, PROCCODE, TAMOUNT, TCODE "
SELECT  MID, BIN, PROCCODE, Sum(TAMOUNT) AS AMOUNT, Count(TCODE) AS TrxNo, cycle.Period()
 FROM
  (SELECT  MID, CAST(LEFT(MASK, 6) AS INT) AS BIN, PROCCODE, TAMOUNT, TCODE, cycle.Period() 
   FROM dbo.TRANSLOG_TRANSACT 
   WHERE DESTCOMID = 'NET_ALPHA'
   AND (DTSTAMP BETWEEN @fdate AND @tdate)
  ) all_trx2
 -- @fdate, cycle.DateF, -- value : '2019-01-01 00:00:00.000'
 -- @tdate, cycle.DateT.AddDays(1)); value : 1/2/2019 00:00:00

 -- Run function for removing BIN field

 --Index table
 CREATE INDEX TrxMonthAlpha_ix
 ON [abc096].[TrxMonthAlpha]
 (MID, ProductID)

 -- 20. Earned Transactions per customer
 -- Insert into table [abc096].[MIDTRXALL]

 -- declare Transact4 = " MID, DTSTAMP, DESTCOMID "
SELECT MID, YEAR(DTSTAMP) AS YY, MONTH(DTSTAMP) AS MM,COUNT(*)AS TRX
FROM (SELECT MID, DTSTAMP, DESTCOMID FROM dbo.TRANSLOG_TRANSACT) all_trx4
       WHERE DTSTAMP >= @fdate AND LEFT(DESTCOMID,4)='NET_'
GROUP BY MID, YEAR(DTSTAMP), MONTH(DTSTAMP) 
--@fdate", new DateTime(cycle.DateF.Year, 1, 1)

--Index table
CREATE UNIQUE INDEX MIDTRXALL_ix
 ON [abc096].[MIDTRXALL]
(YY, MM, MID)
