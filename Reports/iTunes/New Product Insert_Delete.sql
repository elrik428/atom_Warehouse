--- A. INSERT new Product
-- 1. Inserto to itunes.Products
-- 2. Insert to ZACPRT.dbo.SERVICES
-- Copy from a relevant ProductType

--3. Insert to ZACPRT.dbo.SERVICES_MERCHANTS

-- Backup table
select * into [dbo].[SERVICES_MERCHANTS_bupLN] FROM [ZACRPT].[dbo].[SERVICES_MERCHANTS]

-- Insert to table
insert into [ZACRPT].[dbo].[SERVICES_MERCHANTS]
SELECT  [MID]
      ,[TID]
      ,[ProviderID]
      ,'3070'
      ,[ProductType]
      ,[Status]
      ,[SurchargeAmountTotal]
      ,'2019-04-03 00:00:00.000'
      ,'2019-04-03 00:00:00.000'
      ,[AgentID]
      ,[Organismos]
      ,[PaymentMethodsAllowed]
      ,[InstallmentsPCC]
      ,[InstallmentsOCC]
      ,[TERMINAL_TYPE]
      ,[DMID]
      ,[DTID]
      ,[AllowReversal]
      ,[DiasCode]
  FROM [ZACRPT].[dbo].[SERVICES_MERCHANTS]
  where serviceid = '3069'

--- B. DELETE  Product
-- 1.
Delete
From [ZACRPT].[dbo].[SERVICES_MERCHANTS]
Where serviceid = '3012'
-- 2.
Update [ZACRPT].[dbo].[SERVICES]
Set servicestatus = 'N'
Where  servicename  = 'QTEL 10'
