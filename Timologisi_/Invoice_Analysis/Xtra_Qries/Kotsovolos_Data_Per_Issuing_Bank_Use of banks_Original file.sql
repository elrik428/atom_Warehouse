--------------------------
--      START           --
--------------------------
print '***  START   ***';
print 'Clear file';
delete from [abc096].[KOTS_Transactions_Month]
;
--insert data
print 'Insert Data';
insert into [abc096].[KOTS_Transactions_Month]
select a.*,null,null,null,null,null,null,null,null
from
 dbo.TRANSLOG_TRANSACT a
WHERE
 MID IN ('000000001100009') and
-- dtstamp>='2014-01-01 00:00:00.001' and
-- dtstamp<='2014-01-31 23:59:59.999'
-- dtstamp>=::FROMDAT  and
-- dtstamp<= ::TODAT
respkind='OK' and DESTCOMID<>'NET_NTBNLTY'
;
--Update Amount for Void and refund
update [abc096].[KOTS_Transactions_Month] set tamount= tamount*(-1) where proccode in ('200000','020000')
;
--Update Merchant Name
update [abc096].[KOTS_Transactions_Month] set
MERCHANT_NAME=
CASE WHEN MID='000000001100009'
THEN 'Dixons'
ELSE ' '
END
;
--Update ACQUIRING_BANK
update [abc096].[KOTS_Transactions_Month] set
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
ELSE 'UNKNOWN BANK'
END)
;
--Update Card Type
update [abc096].[KOTS_Transactions_Month] set
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
ELSE 'UNKNOWN'
END)
;
print 'Update Discover Card Type';
update [abc096].[KOTS_Transactions_Month] set
CARD_TYPE= 'Discover'
Where SUBSTRING(MASK,1,4)='6011'
;
update  [abc096].[KOTS_Transactions_Month] set [ACQUIRER_BANK_ID]=0
;
update  [abc096].[KOTS_Transactions_Month] set  [ISSUER_BANK_ID]=0
;
update  [abc096].[KOTS_Transactions_Month] set  [GREEK_ISSUER]='   '
;
update [abc096].[KOTS_Transactions_Month] set
[ON_US]='   '

--Update Acquirer id
print '--Update Acquirer id';
update a
set  [ACQUIRER_BANK_ID] = id
 from [abc096].[KOTS_Transactions_Month] as a
  inner join abc096.banks_Original b on a.destcomid=b.destcomid
  ;

--Update Issuer id - 1
print '--Update Issuer id - 1';
update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[KOTS_Transactions_Month] as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,6) = b.BIN /*and b.BANKID <>0*/
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 2
print '--Update Issuer id - 2';
  update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[KOTS_Transactions_Month] as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,5) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 3
print '--Update Issuer id - 3';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[KOTS_Transactions_Month] as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,4) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 4
print '--Update Issuer id - 4';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[KOTS_Transactions_Month] as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,3) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
-- New code for replacement    LN 20171003 Finish


update  [abc096].[KOTS_Transactions_Month] set [ACQUIRER_BANK_ID]=0 where [ACQUIRER_BANK_ID] is null
;
update  [abc096].[KOTS_Transactions_Month] set  [ISSUER_BANK_ID]=0 where [ISSUER_BANK_ID] is null

--Update GREEK_ISSUER FLAG
update [abc096].[KOTS_Transactions_Month] set
[GREEK_ISSUER]='Yes'
where [ISSUER_BANK_ID]< 42 and  [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;

update [abc096].[KOTS_Transactions_Month] set
[GREEK_ISSUER]='No' where  [GREEK_ISSUER]<>'Yes' or [GREEK_ISSUER] is null
;
--Update  [ON_US]  FLAG
update [abc096].[KOTS_Transactions_Month] set
[ON_US]='Yes' where  [ISSUER_BANK_ID]= [ACQUIRER_BANK_ID]
;
--Update  [ON_US]  FLAG
update [abc096].[KOTS_Transactions_Month] set
[ON_US]='No' where  [ISSUER_BANK_ID]<>[ACQUIRER_BANK_ID]
;

--Update ISSUING BANK
update [abc096].[KOTS_Transactions_Month] set
 [ISSUING_BANK] = cast((select bank from abc096.banks_Original where [abc096].[KOTS_Transactions_Month].[ISSUER_BANK_ID]=abc096.banks_Original.id) as nvarchar(50))
where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;

--Update user data with card type
print '--Update Userdata DINERS';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Diners' where substring([abc096].[KOTS_Transactions_Month].mask,1,2) in ('30','36','38')
 ;
 print '--Update Userdata AMEX';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='AMEX' where substring([abc096].[KOTS_Transactions_Month].mask,1,2) in ('34','37')
;
print '--Update Userdata Debit';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Debit' where substring([abc096].[KOTS_Transactions_Month].mask,1,6)='375593'
;
print '--Update Userdata CARD TYPE';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] =CARD_TYPE where  [UserData]='' or  [UserData] is null
 ;
 print '--Update Userdata Credit for UserData=MasterCard';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Credit' where  [UserData]='MasterCard'
;
print '--Update Userdata Credit for UserData=Diners';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Credit' where  [UserData]='Diners'
;
print '--Update Userdata Credit for UserData=AMEX';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Credit' where  [UserData]='AMEX'
;
print '--Update Userdata Credit for UserData=Maestro';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Debit' where  [UserData]='Maestro'
;
print '--Update Userdata Credit for UserData=Deferred Debit';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] ='Debit' where  [UserData]='Deferred Debit'
;
print '--Update Userdata from subselect #1 ';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] = (select [Card Type] from [dbo].[MC_GREEK_BINS] where substring([abc096].[KOTS_Transactions_Month].mask,1,6)=[dbo].[MC_GREEK_BINS].bin)
where  substring([abc096].[KOTS_Transactions_Month].mask,1,6) in (select [dbo].[MC_GREEK_BINS].bin from [dbo].[MC_GREEK_BINS])
;
print '--Update Userdata from subselect #2 ';
update [abc096].[KOTS_Transactions_Month] set
 [UserData] = (select ACCT_FUNDG_SRCE_NM from visa_BINS_201311 where substring([abc096].[KOTS_Transactions_Month].mask,1,6)=visa_BINS_201311.bin)
where  substring([abc096].[KOTS_Transactions_Month].mask,1,6) in (select visa_BINS_201311.bin from visa_BINS_201311)
;


---Summary SHEET
SELECT
(case month(dtstamp)
when '01' then 'January'
when '02' then 'February'
when '03' then 'March'
when '04' then 'April'
when '05' then 'May'
when '06' then 'June'
when '07' then 'July'
when '08' then 'August'
when '09' then 'September'
when '10' then 'October'
when '11' then 'November'
when '12' then 'December'
ELSE 'UNKNOWN'
END) as Month,
Year(dtstamp) as Year,
[GREEK_ISSUER],
[ISSUING_BANK],
SUM(case when UserData = 'Credit' then [TAMOUNT] else 0 end) [Credit],
SUM(case when UserData like '%Debit%' then [TAMOUNT] else 0 end) [Debit],
SUM(case when UserData = 'Prepaid' then [TAMOUNT] else 0 end) [Prepaid],
SUM(case when (UserData = 'UNKNOWN' or UserData = 'VISA' )then [TAMOUNT] else 0 end) [UNKNOWN],
SUM(TAMOUNT) as [In Value Total],
'|',
--[ISSUING_BANK],
SUM(case when UserData = 'Credit' then 1 else 0 end) [Credit_],
SUM(case when UserData like '%Debit%' then 1 else 0 end) [Debit_],
SUM(case when UserData = 'Prepaid' then 1 else 0 end) [Prepaid_],
SUM(case when (UserData = 'UNKNOWN' or UserData = 'VISA' )then 1 else 0 end) [UNKNOWN_],
COUNT(*) as [In Volume Total]
from
 [abc096].[KOTS_Transactions_Month] a
GROUP BY
(case month(dtstamp)
when '01' then 'January'
when '02' then 'February'
when '03' then 'March'
when '04' then 'April'
when '05' then 'May'
when '06' then 'June'
when '07' then 'July'
when '08' then 'August'
when '09' then 'September'
when '10' then 'October'
when '11' then 'November'
when '12' then 'December'
ELSE 'UNKNOWN'
END),
Year(dtstamp),
[GREEK_ISSUER],
[ISSUING_BANK]
order by
(case month(dtstamp)
when '01' then 'January'
when '02' then 'February'
when '03' then 'March'
when '04' then 'April'
when '05' then 'May'
when '06' then 'June'
when '07' then 'July'
when '08' then 'August'
when '09' then 'September'
when '10' then 'October'
when '11' then 'November'
when '12' then 'December'
ELSE 'UNKNOWN'
END),
Year(dtstamp),
[GREEK_ISSUER],
[ISSUING_BANK]

-- Split in 3 xls so to be able to send them
-- 1. 
select
DTSTAMP as Transaction_Date,
TID,
TAMOUNT as AMOUNT,
TAUTHCODE as AUTH_CODE,
INST AS Installment_Number,
MASK as MASKED_PAN,
USERDATA as Card_Type,
CARD_TYPE As Card_Brand,
ISSUING_BANK,
GREEK_ISSUER,
ACQUIRING_BANK,
ON_US
from
 [abc096].[KOTS_Transactions_Month] a
 where dtstamp <= '2019-08-10 23:59:59.999'
 order by dtstamp,TID

-- 2.
select
DTSTAMP as Transaction_Date,
TID,
TAMOUNT as AMOUNT,
TAUTHCODE as AUTH_CODE,
INST AS Installment_Number,
MASK as MASKED_PAN,
USERDATA as Card_Type,
CARD_TYPE As Card_Brand,
ISSUING_BANK,
GREEK_ISSUER,
ACQUIRING_BANK,
ON_US
from
 [abc096].[KOTS_Transactions_Month] a
 where dtstamp >= '2019-08-11 00:00:00.001' and dtstamp <= '2019-08-20 23:59:59.999'
 order by dtstamp,TID

-- 3.
select
DTSTAMP as Transaction_Date,
TID,
TAMOUNT as AMOUNT,
TAUTHCODE as AUTH_CODE,
INST AS Installment_Number,
MASK as MASKED_PAN,
USERDATA as Card_Type,
CARD_TYPE As Card_Brand,
ISSUING_BANK,
GREEK_ISSUER,
ACQUIRING_BANK,
ON_US
from
 [abc096].[KOTS_Transactions_Month] a
 where dtstamp > '2019-08-20 23:59:59.999'
 order by dtstamp,TID


--Detail SHEET
select
DTSTAMP as Transaction_Date,
TID,
TAMOUNT as AMOUNT,
TAUTHCODE as AUTH_CODE,
INST AS Installment_Number,
MASK as MASKED_PAN,
USERDATA as Card_Type,
CARD_TYPE As Card_Brand,
ISSUING_BANK,
GREEK_ISSUER,
ACQUIRING_BANK,
ON_US
from
 [abc096].[KOTS_Transactions_Month] a
 order by dtstamp,TID
