--1. Delete
        delete from dbo.binbase_forTransfer
 -- & Insert BINs to temp BIN file, [dbo].[binbase_forTransfer]
 -- Use below insert for columns
 insert into dbo.binbase_forTransfer(id, bin, brand, bank, typeb, levelb, isocountry, isoa2, isoa3, isonumber, www, phone) VALUES
--2. Run sql statement so find which BINs doesn't exist in Products from new file

    select * from [dbo].[binbase_forTransfer] a
    where not exists (select bin from abc096.Products b where a.bin = b.bin );
-- and vice versa
    select * from abc096.Products b
	where not exists (select bin from [dbo].[binbase_forTransfer] a where a.bin = b.bin )

--3. Delete table dbo.products_cpyofabc and then Insert sql

    delete from dbo.products_cpyofabc;

  /*  insert into dbo.products_cpyofabc
    select * from abc096.Products;*/

    INSERT INTO [ZacReporting].[dbo].[products_cpyofabc]
               ([BIN]
               ,[BINU]
               ,[BANKID]
               ,[ExBankID]
               ,[Proprietary]
               ,[Product]
               ,[Exclude]
               ,[CARDID]
               ,[Brand])
         SELECT [BIN]
          ,[BINU]
          ,[BANKID]
          ,[ExBankID]
          ,[Proprietary]
          ,[Product]
          ,[Exclude]
          ,[CARDID]
          ,[Brand]
      FROM [ZacReporting].[abc096].[Products]

--4. Delete [dbo].[binbase#2] and Insert not exist BINs of new file to BINBASE#2
  DELETE from [dbo].[binbase#2]

  INSERT INTO [ZacReporting].[dbo].[binbase#2]
       ([id]
       ,[bin]
       ,[brand]
       ,[bank]
       ,[typeb]
       ,[levelb]
       ,[isocountry]
       ,[isoa2]
       ,[isoa3]
       ,[isonumber]
       ,[www]
       ,[phone]
       ,[regioneu])
       select a.*,' '   from dbo.[binbase_forTransfer] a
       where not exists (select bin from abc096.Products b where a.bin = b.bin )

--5. Update regioneu
  update dbo.binbase#2
    set  regioneu = 'Y'
    where  isocountry in (
     'Russia'
    ,'Germany'
    ,'Turkey'
    ,'France'
    ,'United Kingdom'
    ,'Italy'
    ,'Spain'
    ,'Ukraine'
    ,'Poland'
    ,'Romania'
    ,'Netherlands'
    ,'Belgium'
    ,'Greece'
    ,'Czech Republic'
    ,'Portugal'
    ,'Sweden'
    ,'Hungary'
    ,'Azerbaijan'
    ,'Belarus'
    ,'Austria'
    ,'Switzerland'
    ,'Bulgaria'
    ,'Serbia'
    ,'Denmark'
    ,'Finland'
    ,'Slovakia'
    ,'Norway'
    ,'Ireland'
    ,'Croatia'
    ,'Bosnia and Herzegovina'
    ,'Georgia'
    ,'Moldova'
    ,'Armenia'
    ,'Lithuania'
    ,'Albania'
    ,'Macedonia'
    ,'Slovenia'
    ,'Latvia'
    ,'Kosovo'
    ,'Estonia'
    ,'Cyprus'
    ,'Montenegro'
    ,'Luxembourg'
    ,'Malta'
    ,'Iceland'
    ,'Jersey'
    ,'Isle of Man'
    ,'Andorra'
    ,'Guernsey'
    ,'Faroe Islands'
    ,'Liechtenstein'
    ,'Monaco'
    ,'Gibraltar'
    ,'San Marino'
    ,'Aland Islands'
    ,'Svalbard'
    ,'Vatican City')

--6.  Insert new BINs to products tables, abc096 & dbo,

--INSERT INTO [ZacReporting].[dbo].[products]
INSERT INTO [ZacReporting].[abc096].[products]
           ([BIN]
           ,[BINU]
           ,[BANKID]
           ,[ExBankID]
           ,[Proprietary]
           ,[Product]
           ,[Exclude]
           ,[CARDID]
           ,[Brand])
select bin, bin, 0, 0, cast(0 as bit) ,substring(brand,1,40)+ ' ' + substring(levelb,1,40), cast(0 as bit), 0, substring(brand,1,20)   from dbo.binbase#2

--7. Run BIN_Procedure#4 so to update banks in new products table

-- Xtra query so to check detail of BINs inserted
--2. Run sql statement so find which BINs doesn't exist in Products from new file

 	  select * from [dbo].[binbase_forTransfer] a
    where not exists (select bin from dbo.products_cpyofabc b where a.bin = b.bin );


-- XTRA SQLs
-- 1. Insert data to table with unique ID
  SET IDENTITY_INSERT DBO.PRODUCTS ON
  GO
  DELETE from dbo.Products
  INSERT into dbo.Products ([ID],[BIN],[BINU],[BANKID],[ExBankID],[Proprietary],[Product],[Exclude],[CARDID],[Brand])
  SELECT * from abc096.Products


-- 2. Revert BANKS to original form
delete from abc096.Banks 
where ID >=42

insert into abc096.Banks
select * from abc096.banks_new a
where not  exists(select b.ID from abc096.Banks b where a.ID=b.ID)

UPDATE ABC096.Banks_new
SET DESTCOMID = NULL,BID = NULL
WHERE ID > 42
