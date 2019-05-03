DELETE from [dbo].[BP_TRANSACTIONS]

UPDATE [dbo].[BP_TRANSACTIONS] set [TerminalID]=right([TerminalID],8)
UPDATE [dbo].[BP_TRANSACTIONS] set [Status]='' WHERE [Type]='V'
UPDATE [dbo].[BP_TRANSACTIONS] set [Response]='DECLINE'
UPDATE [dbo].[BP_TRANSACTIONS] set [Response]='APPROVAL' WHERE [Result]='0'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='OK' WHERE [Result]='0'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='Terminal unknown / setup issue' WHERE [Result]='03'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='Provider not available or article not allowed' WHERE [Result]='91'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='Format error' WHERE [Result]='12'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='Transaction not found' WHERE [Result]='21'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='Declined from Host' WHERE [Result]='85'
UPDATE [dbo].[BP_TRANSACTIONS] set [RC Descr]='No answer' WHERE [Result]='200'

SELECT [Customer] as 'Κατάστημα'
      ,right([TerminalID],8) as 'Κωδικός τερματικού'
	  ,[ZacReporting].[abc096].[MERCHANTS].[MID] as 'Κωδικός Εμπόρου'
	  ,[ZacReporting].[abc096].[MERCHANTS].[DMID] as 'Agent'
      ,[TrxDate] as 'Ημερομηνία Συναλλαγής'
      ,[TrxTime] as 'Ώρα Συναλλαγής'
	  ,[dbo].[SERVICES].[ServiceName] as 'Οργανισμός'
      ,[Amount] as 'Ποσό πληρωμής'
      ,[Trace] as 'Αριθμός Συναλλαγής'
      ,[Serial] as 'Κωδικός Πληρωμής'
      ,[Exp.Date] as 'Ημερομηνία Λήξης'
      ,(CASE [Type] WHEN 'V' THEN 'Reversal'	ELSE ''	END)as 'Τύπος'
      ,[Telephone] as 'Τηλέφωνο'
      ,[Result] as 'Απόκριση'
      ,[Response] as 'Είδος Απόκρισης'
      ,[RC Descr] as 'Περιγραφή Απόκρισης'
      ,[ErrorCode] as 'Κωδικός Απόρριψης'
	  ,(CASE [Status] WHEN 'VOI' THEN 'Reversed' ELSE '' END)	as 'Κατάσταση'
	    ,[SecondaryPaymentCode] as '2ος Κωδικός Πληρωμής'
	    ,[Reference] as 'Reference',
      (case  when [PAN financial] <> '' then 'CARD'  when [PAN financial] = '' then  'CASH' end) as 'Τρόπος Πληρωμής'


FROM ([ZacReporting].[dbo].[BP_Transactions] LEFT JOIN [ZacReporting].[abc096].[MERCHANTS]
ON [ZacReporting].[dbo].[BP_Transactions].[TerminalID]=[ZacReporting].[abc096].[MERCHANTS].[DTID]
AND [ZacReporting].[abc096].[MERCHANTS].[UPLOADHOSTNAME]='NET_EPA') LEFT JOIN [dbo].[SERVICES] ON [dbo].[SERVICES].[ProductServiceBarcode]=[ZacReporting].[dbo].[BP_Transactions].[EAN]
order by [TerminalID],[TrxDate],[TrxTime]
