--1. Delete
        delete from dbo.binbase_forTransfer
 -- & Insert BINs to temp BIN file, [dbo].[binbase_forTransfer]
 -- Use below insert for columns
INSERT INTO dbo.binbase_forTransfer(id, bin, brand, bank, typeb, levelb, isocountry, isoa2, isoa3, isonumber, www, phone) 
VALUES (3094186, 100001, 'LOCAL BRAND', 'STATE BANK OF INDIA', 'DEBIT', 'CLASSIC', 'INDIA', 'IN', 'IND', 356, '', '')
-- ETC............    

--2. Backup table abc096.products to dbo.products_cpyofabc
DELETE from dbo.products_cpyofabc;

/*  
    insert into dbo.products_cpyofabc
    select * from abc096.Products;
*/

INSERT INTO [ZacReporting].[dbo].[products_cpyofabc]
([BIN],[BINU],[BANKID],[ExBankID],[Proprietary],[Product],[Exclude],[CARDID],[Brand])
SELECT [BIN],[BINU],[BANKID],[ExBankID],[Proprietary],[Product],[Exclude],[CARDID],[Brand]
FROM [ZacReporting].[abc096].[Products]


--3. Insert NON Greek BINs to table
DELETE from dbo.binbase_forTr_NoGR

INSERT INTO dbo.binbase_forTr_NoGR
SELECT * FROM dbo.binbase_forTransfer  WHERE isocountry <> 'GREECE'


-- 4. INsert BINS to middle table so to update bankID
DELETE FROM [dbo].[binbase_mid]

INSERT INTO [dbo].[binbase_mid]
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
           ,[regioneu]
           ,[bankid_tmp])
SELECT a.*,' ',(CAST(Rank() OVER( ORDER BY bank ) AS int)) + 100   FROM dbo.binbase_forTr_NoGR a      

-- 5. Update REGIONEU to mid table
UPDATE [dbo].[binbase_mid]
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


--6. Update *blank BANK
UPDATE [dbo].[binbase_mid]
SET bankid_tmp= '0'
WHERE bank =' ' 

UPDATE [dbo].[binbase_mid]
SET bank= 'UNKNOWN'
WHERE bankid_tmp = '0'  

--7. Select Scripts for PRODUCTS  
-- i.
-- Scripts for BINs that already exist to products and should not be replaced oi updated - For check purpose
select * from [dbo].[binbase_mid] a
where  exists (select bin from dbo.Products b where a.bin = b.bin );

select * from dbo.Products b
where exists (select bin from [dbo].[binbase_mid] a where a.bin = b.bin )

-- ii. 
-- BINs to be deleted so to INSERT new from Russian site
DELETE FROM dbo.Products
where BANKID > 99 or BANKID=0

    select * from dbo.Products
    where BANKID > 99 or BANKID=0

-- iii. 
-- Insert data to PRODUCTS table
INSERT INTO [ZacReporting].dbo.[products]
           ([BIN]
           ,[BINU]
           ,[BANKID]
           ,[ExBankID]
           ,[Proprietary]
           ,[Product]
           ,[Exclude]
           ,[CARDID]
           ,[Brand])
select bin, bin, 0, 0, cast(0 as bit) ,substring(brand,1,40)+ ' ' + substring(levelb,1,40), cast(0 as bit), 0, substring(brand,1,20)   from dbo.[binbase_mid] 
where bin not in 
(
    select bin from dbo.Products b
	where exists (select bin from [dbo].[binbase_mid] a where a.bin = b.bin )
)
	

-- XTRA section
-- BINS that are NOT updated from the Russian site!!!!!!!!! ATTENTION
SELECT * FROM dbo.Products
WHERE bankid  IN (1,2,3,6,8)

-- BINS outside big five banks that are NOT updated ever!!!! ATTENTION
SELECT * FROM dbo.Products
WHERE BANKID < 44 AND BANKID  NOT IN (1,2,3,6,8) AND BANKID<>0




-- 8. Scripts for BANKS table
-- i. 
-- BANKIDs to be deletd from BANKS table
DELETE from DBO.BANKS_TEST
where id >99
    
    SELECT * FROM abc096.banks
    WHERE id >99

-- ii. 
-- Insert data
INSERT INTO DBO.BANKS_TEST
SELECT bankid_tmp, bank  , ' ',  ' ', bank
FROM zacreporting.[dbo].[binbase_mid]
WHERE ID <> 0
GROUP BY bankid_tmp,bank
