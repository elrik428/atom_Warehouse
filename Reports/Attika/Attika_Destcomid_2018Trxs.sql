SELECT a.Destcomid as [Bank], a.mid as [Euronet MID], a.dmid as [Bank MID], b.merchant as [Merchant Name],
[TYPE] =
case substring(a.PROCCODE, 1, 2)
when '00' then 'SALES'
when '20' then 'REFUNDS'
when '02' then 'VOIDS-SALES'
when '22' then 'VOIDS-REFUNDS'
else '??????'
end,
--Count ([TAMOUNT]) AS Transactions,
right('           '+convert(varchar(12),REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,count(a.TAMOUNT)),1), '.00','')),12) as 'Transactions Number',
--Sum ([TAMOUNT]) AS Amount
right('           '+convert(varchar(12),REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,sum(a.TAMOUNT)),1), '.00','')),12) as 'Transactions Amount'
--Count(CASE [PROCCODE] when '000000' then [TAMOUNT] when '200000' then -[TAMOUNT] end) AS Transactions,
--Sum(CASE [PROCCODE] when '000000' then [TAMOUNT] when '200000' then -[TAMOUNT] end) AS Amount
FROM dbo.TRANSLOG_TRANSACT_2018 a
join abc096.mids b on a.mid = b.mid
where
-- substring(mask,1,1) = '4' and
-- dtstamp >= '2015-03-01 00:00:00.000'  and
--       dtstamp < '2015-04-01 00:00:00.000'
--And ((INTERFACE)='POS')
((a.MSGID)='0200' Or (a.MSGID)='0220')
And a.TRESPONSE='00' And a.RESPKIND='OK'
and a.destcomid = 'NET_ATTICA'
GROUP BY a.Destcomid,a.mid, a.dmid, b.merchant,a.PROCCODE
ORDER BY a.Destcomid,a.mid, a.dmid, b.merchant,a.PROCCODE
