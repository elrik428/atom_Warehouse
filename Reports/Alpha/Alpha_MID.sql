[‎20/‎Φεβ/‎2018 10:19 πμ]  Despoina Evgenidou:
SELECT [MID]
      ,[TID]
      ,[UPLOADHOSTNAME]
      ,[DMID]
      ,[DTID]
      ,[MERCHTITLE]
      ,[MERCHADDRESS]
      ,[STORE_CODE]
      ,[BINRULING]
  FROM [ZacReporting].[abc096].[MERCHANTS]
where uploadhostname='NET_ALPHA' and mid not in ('000000011111111',
'1111','000000012345678')
order by tid
