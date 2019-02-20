/* 
--Create Table
drop table [abc096].[VER_Transactions_Month]
CREATE TABLE [abc096].[VER_Transactions_Month](
     [MID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [TID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [DESTCOMID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [TCODE] [int] NULL ,
     [ORIGINATOR] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [INTERFACE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [DTSTAMP] [datetime] NULL ,
     [MSGID] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [TAMOUNT] [float] NULL ,
     [TAUTHCODE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [MASK] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [PROCCODE] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [INST] [int] NULL ,
     [RESPKIND] [varchar](3) COLLATE Greek_CI_AS NULL ,
     [TRESPONSE] [varchar](4) COLLATE Greek_CI_AS NULL ,
     [USERDATA] [varchar](256) COLLATE Greek_CI_AS NULL ,
     [DMID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [DTID] [varchar](16) COLLATE Greek_CI_AS NULL,
     [PEM] [varchar](5),--20150804
     [MERCHANT_NAME] [varchar](30),
     [ACQUIRING_BANK] [varchar](30),
     [ISSUING_BANK] [nvarchar](50) NULL,
     [BRAND] [varchar](20),--20150629
     [CARD_TYPE] [varchar](100),--20150629
     [GREEK_ISSUER] [varchar](3),
     [ON_US] [varchar](3),
     [ISSUER_BANK_ID] [integer],
     [ACQUIRER_BANK_ID] [integer],
     [STORE_CODE] [varchar](20)
     ) ON [PRIMARY]
*/


Print '------------------';
print '--   S T A R T  --';
Print '------------------';

Print 'Clear File';
delete from [abc096].[VER_Transactions_Month]

--insert data
print '-- Insert Data into VER_Transactions_Month';
insert into [abc096].[VER_Transactions_Month]
select a.*,null,null,null,null,null,null,null,null,null,null
from
dbo.TRANSLOG_TRANSACT a
--dbo.TRANSLOG_TRANSACT_2013 a
WHERE
MID IN ('000000120001700', '000000120001710','000000120001720')
--and PROCCODE<>'5W0000'
--and dtstamp>='2013-01-01 00:00:00.001' and
--dtstamp<='2013-12-31 23:59:59.999'
;
--Update Merchant Name
print '-- Update Merchant Name';
update [abc096].[VER_Transactions_Month] set
MERCHANT_NAME=
--CASE WHEN MID='000000120002800'
case mid
when '000000120001700' THEN 'METRO-MYMARKET-CASH_CARRY'
when '000000120001710' THEN 'METRO-MYMARKET'
when '000000120001720' THEN 'METRO_GAZ'
ELSE 'MYMARKET'
END
;
--Update Store Code
print '-- Update Store Code';
update [abc096].[VER_Transactions_Month] set
[STORE_CODE]= (Select max(a.[STORE_CODE]) from abc096.merchants a
where
[abc096].[VER_Transactions_Month].TID=a.TID and
[abc096].[VER_Transactions_Month].MID=a.MID)
;
--Update ACQUIRING_BANK
print '--Update ACQUIRING_BANK';
update [abc096].[VER_Transactions_Month] set
ACQUIRING_BANK=
(CASE DESTCOMID
WHEN 'NET_ABC'      THEN 'Piraeus Bank'
WHEN 'NET_CITI'     THEN 'CitiBank'
WHEN 'NET_CMBN'     THEN 'Commercial Bank'
WHEN 'NET_AGROTIKI' THEN 'Agrotiki Bank'
WHEN 'NET_NTBN'     THEN 'National Bank of Greece'
WHEN 'NET_ALPHA'    THEN 'Alpha Bank'
WHEN 'NET_EBNK'     THEN 'Eurobank'
WHEN 'NET_GENIKI'   THEN 'Geniki Bank'
WHEN 'NET_EBLY1'    THEN 'Eurobank Loyalty'
WHEN 'NET_EBLY'    THEN 'Eurobank Loyalty'
ELSE 'UNKNOWN BANK'
END)
;

print 'Set ACQUIRER_BANK_ID=0';
update  [abc096].[VER_Transactions_Month] set [ACQUIRER_BANK_ID]=0
;
print 'Set ISSUER_BANK_ID=0';
update  [abc096].[VER_Transactions_Month] set  [ISSUER_BANK_ID]=0
;
print 'Set GREEK_ISSUER=0';
update  [abc096].[VER_Transactions_Month] set  [GREEK_ISSUER]='   '
;
print 'Set ON_US= *Blanks';
update [abc096].[VER_Transactions_Month] set
[ON_US]='   '

--Update Acquirer id
print '--Update Acquirer id';
update a
set  [ACQUIRER_BANK_ID] = id
 from [abc096].[VER_Transactions_Month] as a
  inner join abc096.banks b on a.destcomid=b.destcomid
  ;

--Update Issuer id - 1
print '--Update Issuer id - 1';
update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[VER_Transactions_Month] as a
  inner join zacreporting.abc096.products_old b on substring(a.mask,1,6) = b.BIN /*and b.BANKID <>0*/
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 2
print '--Update Issuer id - 2';
  update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[VER_Transactions_Month] as a
  inner join zacreporting.abc096.products_old b on substring(a.mask,1,5) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 3
print '--Update Issuer id - 3';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[VER_Transactions_Month] as a
  inner join zacreporting.abc096.products_old b on substring(a.mask,1,4) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 4
print '--Update Issuer id - 4';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[VER_Transactions_Month] as a
  inner join zacreporting.abc096.products_old b on substring(a.mask,1,3) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
-- New code for replacement    LN 20171003 Finish


--Update ACQUIRER_BANK_ID=0 where ACQUIRER_BANK_ID is null
print '--Update ACQUIRER_BANK_ID=0 where ACQUIRER_BANK_ID is null';
update  [abc096].[VER_Transactions_Month] set [ACQUIRER_BANK_ID]=0 where [ACQUIRER_BANK_ID] is null
;
--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null
print '--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null';
update  [abc096].[VER_Transactions_Month] set  [ISSUER_BANK_ID]=0 where [ISSUER_BANK_ID] is null

--Update GREEK_ISSUER FLAG
print '--Update GREEK_ISSUER FLAG-1';
update [abc096].[VER_Transactions_Month] set
[GREEK_ISSUER]='Yes' where  [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;

print '--Update GREEK_ISSUER FLAG-4';
update [abc096].[VER_Transactions_Month] set
[GREEK_ISSUER]='No' where  [GREEK_ISSUER]<>'Yes' or [GREEK_ISSUER] is null
;
--Update  [ON_US]  FLAG-1
print '--Update  [ON_US]  FLAG-1';
update [abc096].[VER_Transactions_Month] set
[ON_US]='Yes' where  [ISSUER_BANK_ID]= [ACQUIRER_BANK_ID]
;
--Update  [ON_US]  FLAG-2
print '--Update  [ON_US]  FLAG-2';
update [abc096].[VER_Transactions_Month] set
[ON_US]='No' where  [ISSUER_BANK_ID]<>[ACQUIRER_BANK_ID]
;
--Update ISSUING BANK
print '--Update ISSUING BANK';
update [abc096].[VER_Transactions_Month] set
 [ISSUING_BANK] = (select bank from abc096.banks where [abc096].[VER_Transactions_Month].[ISSUER_BANK_ID]=abc096.banks.id)
where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;

print '--Update Product - 1';
update [abc096].[VER_Transactions_Month] set
 [BRAND] = (select BRAND from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,6) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0),
 [CARD_TYPE] = (select [PRODUCT] from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,6) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0)
where BRAND IS NULL AND CARD_TYPE IS NULL
;
--Update Product - 2 20150629
print '--Update Product - 2';
update [abc096].[VER_Transactions_Month] set
 [BRAND] = (select BRAND from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,5) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0),
 [CARD_TYPE] = (select [PRODUCT] from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,5) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0)
where BRAND IS NULL AND CARD_TYPE IS NULL
;
--Update Product - 3 20150629
print '--Update Product - 3';
update [abc096].[VER_Transactions_Month] set
 [BRAND] = (select BRAND from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,4) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0),
 [CARD_TYPE] = (select [PRODUCT] from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,4) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0)
where BRAND IS NULL AND CARD_TYPE IS NULL
;
--Update Product - 4 20150629
print '--Update Product - 4';
update [abc096].[VER_Transactions_Month] set
 [BRAND] = (select BRAND from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,3) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0),
 [CARD_TYPE] = (select [PRODUCT] from abc096.products_old where substring([abc096].[VER_Transactions_Month].mask,1,3) between abc096.products_old.BIN and abc096.products_old.BINU and bankid <>0)
where BRAND IS NULL AND CARD_TYPE IS NULL
;
--Update Card Type
print '--Update Card Type';
update [abc096].[VER_Transactions_Month] set
CARD_TYPE=
(CASE SUBSTRING(MASK,1,2)
WHEN '40' THEN 'VISA'
WHEN '41' THEN 'VISA'
WHEN '42' THEN 'VISA'
WHEN '43' THEN 'VISA'
WHEN '44' THEN 'VISA'
WHEN '45' THEN 'VISA'
WHEN '46' THEN 'VISA'
WHEN '47' THEN 'VISA'
WHEN '48' THEN 'VISA'
WHEN '49' THEN 'VISA'
WHEN '50' THEN 'MasterCard'
WHEN '51' THEN 'MasterCard'
WHEN '52' THEN 'MasterCard'
WHEN '53' THEN 'MasterCard'
WHEN '54' THEN 'MasterCard'
WHEN '55' THEN 'MasterCard'
WHEN '56' THEN 'MasterCard'
WHEN '57' THEN 'MasterCard'
WHEN '58' THEN 'MasterCard'
WHEN '59' THEN 'MasterCard'
WHEN '60' THEN 'Maestro'
WHEN '61' THEN 'Maestro'
WHEN '62' THEN 'Maestro'
WHEN '63' THEN 'Maestro'
WHEN '64' THEN 'Maestro'
WHEN '65' THEN 'Maestro'
WHEN '66' THEN 'Maestro'
WHEN '67' THEN 'Maestro'
WHEN '68' THEN 'Maestro'
WHEN '69' THEN 'Maestro'
WHEN '34' THEN 'AMEX'
WHEN '37' THEN 'AMEX'
WHEN '30' THEN 'Diners'
WHEN '36' THEN 'Diners'
WHEN '38' THEN 'Diners'
ELSE 'UNKNOWN TYPE'
END)
;
update [abc096].[VER_Transactions_Month] set
CARD_TYPE=
(CASE SUBSTRING(MASK,1,4)
WHEN '6011' THEN 'Discover'
END)
WHERE SUBSTRING(MASK,1,4)='6011'
;
update [abc096].[VER_Transactions_Month] set
[BRAND]= [CARD_TYPE],
[ISSUING_BANK]='UNKNOWN'
where [ISSUER_BANK_ID]= 0  OR  [ISSUER_BANK_ID] is null
;

update [abc096].[VER_Transactions_Month] set
[BRAND]='MASTER'
where [BRAND]='MasterCard'
;
update [abc096].[VER_Transactions_Month] set
[BRAND]='MAESTRO'
where [BRAND]='Maestro'
;


--- Select Data to display 
SELECT
MERCHANT_NAME,
MID,
[DTSTAMP],
[Store_Code],
TRN_TYPE =
case left(PROCCODE,2)--Account selection changes 3rd byte. 2 first bytes are transaction type 150813
when '00' then 'SALE'
when '20' then 'REFUND'
when '02' then 'VOID-SALE'
when '22' then 'VOID-REFUND'
when '2S' then 'LOYALTY SALE'
when '5W' then 'LOYALTY BALANCE INQUIRY'
when '1U' then 'LOYALTY REDEMPTION'
when '2T' then 'LOYALTY VOID-SALE'
when '3W' then 'LOYALTY BALANCE INQUIRY'
when '2U' then 'LOYALTY VOID-REDEMPTION'
when '1S' then 'LOYALTY REFUND'
else PROCCODE
end,
ACQUIRING_BANK,
Cast(TAMOUNT as dec(15,2)) as Transaction_Amount,
BRAND,
[ISSUING_BANK]--+'/'+[CARD_TYPE] 150813
from [abc096].[VER_Transactions_Month]
order by MERCHANT_NAME,[Store_Code],[DTSTAMP]
