USE [iTunes]
GO

INSERT INTO [dbo].[REPORTS_ALL]
           ([Category]
           ,[CustomerName]
           ,[Customer]
           ,[TerminalID]
           ,[Storeno]
           ,[TrxDate]
           ,[TrxTime]
           ,[EAN]
           ,[Amount]
           ,[Receipt]
           ,[Trace]
           ,[Serial]
           ,[Type]
           ,[UserName]
           ,[Product]
           ,[RetailerCommission]
           ,[VATonCommission]
           ,[RetailerFunds]
           ,[PayabletoEuronet])
     VALUES
           ('Sony'
           ,'Seven GR'
           ,'Seven GR '
           ,'GRE00525'
           ,'12'
           ,'2019-07-15'
           ,'13:54:03'
           ,'4251604134714'
           ,-59.99
           ,'699'
           ,'714'
           ,'05272705-000285'
           ,'Deactivation'
           ,'SUP (00)'
           ,'Sony PS+ PoR 12 months'
           ,-4.7992
		   ,-1.1518
           ,-5.951
           ,-54.039)
GO


