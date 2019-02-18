SELECT TOP 10 MIDs.[Group], 
Banks.BANK, 
TIDs.Shop,
 DATENAME(MONTH,(DATEADD(m,-1,GETDATE()))) + ' ' + DATENAME(YEAR,(DATEADD(m,-1,GETDATE()))) AS [day],
count(case
		when LEFT(TRANSLOG_TRANSACT.PROCCODE,2)='00' then  +(TRANSLOG_TRANSACT.TAMOUNT) 
		when LEFT(TRANSLOG_TRANSACT.PROCCODE,2)='20' then  -(TRANSLOG_TRANSACT.TAMOUNT)  
		end )
-Count(case
	   when LEFT(TRANSLOG_TRANSACT.PROCCODE,2)='22' then +(TRANSLOG_TRANSACT.TAMOUNT) 
	   when LEFT(TRANSLOG_TRANSACT.PROCCODE,2)='02' then -(TRANSLOG_TRANSACT.TAMOUNT)
	   end
	   ) AS Transactions, 
Sum(case 
	when Left(TRANSLOG_TRANSACT.PROCCODE,2) In ('00','22') then +(TRANSLOG_TRANSACT.TAMOUNT) 
	when Left(TRANSLOG_TRANSACT.PROCCODE,2) In ('02','20') then -(TRANSLOG_TRANSACT.TAMOUNT)end ) AS NetAmount
FROM abc096.TIDs INNER JOIN ((dbo.TRANSLOG_TRANSACT INNER JOIN abc096.MIDs ON dbo.TRANSLOG_TRANSACT.MID = MIDs.MID) INNER JOIN  abc096.Banks ON dbo.TRANSLOG_TRANSACT.DESTCOMID = Banks.DESTCOMID) ON TIDs.TID = dbo.TRANSLOG_TRANSACT.TID
WHERE (
		((dbo.TRANSLOG_TRANSACT.DTSTAMP) Between '2019-01-01 00:00:00.001' And '2019-02-01 00:00:00.001') AND 
		((dbo.TRANSLOG_TRANSACT.INTERFACE)='POS') AND
		((dbo.TRANSLOG_TRANSACT.DESTCOMID)<>'ZACHELPER') AND
		((dbo.TRANSLOG_TRANSACT.MSGID)='0200' Or (dbo.TRANSLOG_TRANSACT.MSGID)='0220') AND 
		((dbo.TRANSLOG_TRANSACT.TRESPONSE)='00') AND
		((dbo.TRANSLOG_TRANSACT.RESPKIND)='OK') AND 
		((dbo.TRANSLOG_TRANSACT.TAMOUNT)<>0)
	   )
GROUP BY MIDs.[Group], Banks.BANK, TIDs.Shop
HAVING (((MIDs.[Group]) Like '%NOTOS%'))
ORDER BY MIDs.[Group], Banks.BANK;
