

print '***  START   ***';
-- Steps for report creation
--1.
Print 'Delete temp table'
delete from dbo.IMP_TRANSACT_D_month

--2.
print 'Insert data into temp table'
INSERT INTO [dbo].[IMP_TRANSACT_D_month]
 ([TBL],[TCODE],[MID],[TID],[MASK],[AMOUNT],[CURR],[INST],[GRACE],[ORIGINATOR],[DESTCOMID],[PROCCODE],[MSGID],[RESPKIND],[REVERSED],[BTBL],[BTCODE],[PROCBATCH],[ePOSBATCH],[POSDATA],[BPOSDATA],[DTSTAMP],[BDTSTAMP],[ORGSYSTAN],[DTSTAMP_INSERT],[ProductID],[USERDATA],[BORGSYSTAN],[AUTHCODE],[CASHIERINFO],[DMID],[DTID],[card_type],[ISSUER_BANK_ID],[Merchant_DESCR],[ISSUING_BANK])
select [TBL],[TCODE],[MID],[TID],[MASK],[AMOUNT],[CURR],[INST],[GRACE],[ORIGINATOR],[DESTCOMID],[PROCCODE],[MSGID],[RESPKIND],[REVERSED],[BTBL],[BTCODE],[PROCBATCH],[ePOSBATCH],[POSDATA],[BPOSDATA],[DTSTAMP],[BDTSTAMP],[ORGSYSTAN],[DTSTAMP_INSERT],[ProductID],[USERDATA],[BORGSYSTAN],[AUTHCODE],[CASHIERINFO],[DMID],[DTID],' ',' ' ,' ',' '
 from abc096.IMP_TRANSACT_D_monthly
 --where substring(mid,1,8)= '00000017' and month(DTSTAMP) = '6'
 

--3(All update till end)
print '--Update Card Type';
update dbo.IMP_TRANSACT_D_month set
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
WHEN '62' THEN 'UNIONPAY'
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
END);

print '--Update Merchant Description';
update dbo.IMP_TRANSACT_D_month set
Merchant_DESCR=
(CASE substring(mid,11,2)
WHEN '00' THEN 'OTE OWN'
WHEN '10' THEN 'COSMOTE OWN'
WHEN '11' THEN 'COSMOTE FRANCHISEE'
WHEN '20' THEN 'GERMANOS OWN'
WHEN '21' THEN 'GERMANOS FRANCHISEE'
else 'UNKNOWN'
END);

print '--Update Issuer id - 1';
update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,6) = b.BIN /*and b.BANKID <>0*/
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 2
print '--Update Issuer id - 2';
  update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,5) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 3
print '--Update Issuer id - 3';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,4) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 4
print '--Update Issuer id - 4';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.abc096.products b on substring(a.mask,1,3) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null
print '--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null';
update  dbo.IMP_TRANSACT_D_month set  [ISSUER_BANK_ID]=0 where [ISSUER_BANK_ID] is null
;

--Update ISSUING BANK
print '--Update ISSUING BANK';
update dbo.IMP_TRANSACT_D_month
set ISSUING_BANK = (select substring(bank,1,50) from abc096.banks where  ISSUER_BANK_ID=abc096.banks.id)
where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;
-- END


-- Summary SHEET
select
Merchant_DESCR,
--substring(cashierinfo,2,4),
(case substring(cashierinfo,2,4)
when 'PPC0' then 'PPC Financial'
when 'PPC1' then 'PPC Bill Payment'
when 'PPC2' then 'PPC Prepaid card charge'
when 'PPC3' then 'PPC Protergia'
when 'PPC ' then 'PPC'
when 'SRS'  then 'SRS'
when 'SRS0' then 'SRS'
when ' '  then 'LOYALTY'
else 'UNKNOWN'
end) CashierNFO,
--destcomid,
(case DESTCOMID
when 'NET_ALPHA'   then 'ALPHA BANK'
when 'NET_EBNK'    then 'EUROBANK'
when 'NET_NTBN'    then 'ETHNIKI'
when 'NET_ABC'     then 'PIRAEUS'
when 'NET_EBLY1'   then 'EUROBANK LOYALTY'
when 'NET_NTBNLTY' then 'ETHNIKI LOYALTY'
else 'UNKNOWN'
end) DestinationBank,
card_type,
--ISSUER_BANK_ID,
case ISSUING_BANK
when ' ' then 'UNKNOWN'
else ISSUING_BANK
end,
SUM(CASE substring(PROCCODE,1,2)
WHEN '20' THEN -AMOUNT
WHEN '02' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where  reversed not in ('F','A') and respkind = 'OK'
group by substring(mid,11,2),Merchant_DESCR, substring(cashierinfo,2,4), destcomid, card_type,ISSUER_BANK_ID,ISSUING_BANK
order by substring(mid,11,2), substring(cashierinfo,2,4), destcomid, card_type,ISSUER_BANK_ID
;

-- Loyalty SHEET
-- Fill the same sheet
-- i.
select
Merchant_DESCR,
(case DESTCOMID
when 'NET_EBLY1' then 'EUROBANK LOYALTY'
when 'NET_NTBNLTY' then 'ETHNIKI LOYALTY'
else 'UNKNOWN'
end )  DestinationBank,
card_type,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where destcomid in('NET_NTBNLTY')
--- 'NET_PPGLTY')  Not used
--and proccode = '5W0000' for NET_EBLY1 only
and PROCCODE NOT IN ('400000','300000') -- FOR NET_NTBNLTY ONLY
and respkind = 'OK'
group by substring(mid,11,2),Merchant_DESCR,destcomid, card_type
order by substring(mid,11,2),destcomid, card_type
;
-- ii.
select
Merchant_DESCR,
(case DESTCOMID
when 'NET_EBLY1' then 'EUROBANK LOYALTY'
when 'NET_NTBNLTY' then 'ETHNIKI LOYALTY'
else 'UNKNOWN'
end )  DestinationBank,
card_type,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where destcomid in('NET_EBLY1')
-- 'NET_PPGLTY')  Not used
and proccode = '5W0000' --for NET_EBLY1 only
--and PROCCODE NOT IN ('400000','300000') -- FOR NET_NTBNLTY ONLY
and respkind = 'OK'
group by substring(mid,11,2),Merchant_DESCR,destcomid, card_type
order by substring(mid,11,2),destcomid, card_type

-- Prepaid mastercard-PPC  Sheet
select
Merchant_DESCR,
--DESTCOMID,
(case DESTCOMID
when 'NET_ALPHA' then 'ALPHA BANK'
when 'NET_EBNK' then 'EUROBANK'
when 'NET_NTBN' then 'ETHNIKI'
else 'UNKNOWN'
end) DestinationBank,
card_type,
--ISSUER_BANK_ID,
case ISSUING_BANK
when ' ' then 'UNKNOWN'
else ISSUING_BANK
end,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where  reversed not in ('F','A') and respkind = 'OK' and substring(cashierinfo,2,3) = 'PPC' and substring(cashierinfo,5,1) = '2'
group by substring(mid,11,2),Merchant_DESCR, destcomid, card_type,ISSUER_BANK_ID,ISSUING_BANK
order by substring(mid,11,2), destcomid, card_type,ISSUER_BANK_ID;


-- Protergia Sheet
select
Merchant_DESCR,
--DESTCOMID,
(case DESTCOMID
when 'NET_ALPHA' then 'ALPHA BANK'
when 'NET_EBNK' then 'EUROBANK'
when 'NET_NTBN' then 'ETHNIKI'
else 'UNKNOWN'
end) DestinationBank,
card_type,
--ISSUER_BANK_ID,
case ISSUING_BANK
when ' ' then 'UNKNOWN'
else ISSUING_BANK
end,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where  reversed not in ('F','A') and respkind = 'OK' and substring(cashierinfo,2,3) = 'PPC' and substring(cashierinfo,5,1) = '3'
group by substring(mid,11,2),Merchant_DESCR, destcomid, card_type,ISSUER_BANK_ID,ISSUING_BANK
order by substring(mid,11,2), destcomid, card_type,ISSUER_BANK_ID;
