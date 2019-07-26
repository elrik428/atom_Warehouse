
print '--Total Sum';
select
substring(mid,11,2) merchant_code,
cashierinfo,
destcomid,
card_type,
ISSUER_BANK_ID,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where  reversed not in ('F','A') and respkind = 'OK'
group by substring(mid,11,2), cashierinfo, destcomid, card_type,ISSUER_BANK_ID
order by substring(mid,11,2), cashierinfo, destcomid, card_type,ISSUER_BANK_ID
;

print '--Loyalty Report';
select
substring(mid,11,2) merchant_code,
destcomid,
card_type,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where destcomid in('NET_EBLY1', 'NET_NTBNLTY', 'NET_PPGLTY') and respkind = 'OK'
group by substring(mid,11,2), destcomid, card_type
order by substring(mid,11,2), destcomid, card_type
;

print '--PPC Prepaid Total Sum';
select
substring(mid,11,2) merchant_code,
destcomid,
card_type,
ISSUER_BANK_ID,
SUM(CASE PROCCODE
WHEN '200000' THEN -AMOUNT
WHEN '020000' THEN -AMOUNT
ELSE AMOUNT
END) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from dbo.IMP_TRANSACT_D_month
where  reversed not in ('F','A') and respkind = 'OK' and substring(cashierinfo,2,3) = 'PPC'
group by substring(mid,11,2), destcomid, card_type,ISSUER_BANK_ID
order by substring(mid,11,2), destcomid, card_type,ISSUER_BANK_ID
;


alter table dbo.IMP_TRANSACT_D_month add   card_type varchar(15)
alter table dbo.IMP_TRANSACT_D_month add   ISSUER_BANK_ID int
alter table dbo.IMP_TRANSACT_D_month add   ISSUING_BANK varchar(50)
alter table dbo.IMP_TRANSACT_D_month add   Merchant_DESCR  varchar(50)


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

print '--Update Issuer id - 1';
update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,6) = b.BIN /*and b.BANKID <>0*/
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 2
print '--Update Issuer id - 2';
  update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,5) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 3
print '--Update Issuer id - 3';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,4) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 4
print '--Update Issuer id - 4';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from dbo.IMP_TRANSACT_D_month as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,3) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null
print '--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null';
update  dbo.IMP_TRANSACT_D_month set  [ISSUER_BANK_ID]=0 where [ISSUER_BANK_ID] is null
;

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

--Update ISSUING BANK
print '--Update ISSUING BANK';
update dbo.IMP_TRANSACT_D_month
set ISSUING_BANK = (select substring(bank,1,50) from abc096.banks_new where  ISSUER_BANK_ID=abc096.banks_new.id)
where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;
