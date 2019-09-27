INSERT INTO [dbo].[PRODUCTS]
           ([Name]
           ,[EAN]
           ,[Category]
           ,[Price]
           ,[Active])
select 'Sony PSN PoR 10 EUR GR','4251604188731',[Category],[Price],[Active]
 from dbo.products
where EAN = '4251604134714'
     