--V1.1 Change the priority of mask-product check based on number of digits
--V1.3 Add UNIONPAY
--v1.4 Add Region EU, ISOCOUNTRY in DF_Transactions_Month. Also added REGIONEU as result in --Select Data to display [Details Per Store]--
       changed abc096 --> dbo for products and banks --> banks_new sto use the xtra bins and banks		
	
/*-- Create Table
--drop table [abc096].[DF_Transactions_Month]*/
/*CREATE TABLE [abc096].[DF_Transactions_Month_tmp](
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
     [PEM] [varchar](5) NULL,--20150804
     [MERCHANT_NAME] [varchar](30),
     [ACQUIRING_BANK] [varchar](30),
     [ISSUING_BANK] [nvarchar](50) NULL,
     [CARD_TYPE] [varchar](15),
     [GREEK_ISSUER] [varchar](3),
     [ON_US] [varchar](3),
     [ISSUER_BANK_ID] [integer],
     [ACQUIRER_BANK_ID] [integer],
	 [ISOCNTRY] [varchar] (64), --20170929
	 [REGIONEUFL] [varchar] (1)	--20170929
     ) ON [PRIMARY]*/

Print 'Clear File';
delete from [abc096].[DF_Transactions_Month]
;

--insert data
-- LN 20170929  Added 2 xtra null for new fields
print '-- Insert Data into DF_Transactions_Month';
insert into [abc096].[DF_Transactions_Month]
select  a.*,null,null,null,null,null,null,null,null, null, null
from
dbo.TRANSLOG_TRANSACT a
--dbo.TRANSLOG_TRANSACT_2016 a

WHERE
MID IN ('000000120002800', '000000120003500','000000120003510')
--and dtstamp>='2013-01-01 00:00:00.001' and
--dtstamp<='2013-12-31 23:59:59.999'
;

--Update Merchant Name
print '-- Update Merchant Name';
update [abc096].[DF_Transactions_Month] set
MERCHANT_NAME=
--CASE WHEN MID='000000120002800'
case mid
when '000000120002800' THEN 'Duty Free         '
when '000000120003500' THEN 'еккгмийес диамолес'
ELSE 'еккгмийес диамолес-пкоиа'
END
;

--Update ACQUIRING_BANK
print '--Update ACQUIRING_BANK';
update [abc096].[DF_Transactions_Month] set
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
print '--Update Card Type';
update [abc096].[DF_Transactions_Month] set
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
WHEN '62' THEN 'UNIONPAY'--20151202
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

print 'Update Discover Card Type';
update [abc096].[df_Transactions_Month] set
CARD_TYPE= 'Discover'
Where SUBSTRING(MASK,1,4)='6011'
;

print 'Update UNIONPAY Card Type 6';--20151202
update [abc096].[df_Transactions_Month] set
CARD_TYPE= 'UNIONPAY'
Where SUBSTRING(MASK,1,6) in
('356390','356391','356392','356827','356828','356829','356830','356833','356835','356839',
'356840','356850','356851','356856','356857','356858','356859','356868','356869','356879','356880',
'356881','356882','356889','356890','356895','360883','360884','370246','370247','370248','370249',
'370267','370285','370286','370287','370289','374738','374739','376966','376968','377152','377153',
'377155','377158','377187','377677','601382','601428','602907','602969','603265','603367','603601',
'603694','603708','632062','685800','900105','900205','940001','940003',
'940010','940013','940015','940018','940020','940021','940023','940027','940028','940030','940031',
'940035','940037','940039','940040','940046','940047','940048','940049','940050','940055','940056',
'940061','940062','940063','940066','940068','940073','940074','955100','955550','955590','955591',
'955592','955593','955880','955881','955882','955888','966666','968807','968808','968809',
'984300','984301','984302','984303','990027','998800','998801','998802','400360','402673','402674',
'402791','403361','403391','403392','403393','404117','404119','404120','404157','404158','404159',
'404171','404172','404173','404738','404739','405512','406252','406254','406365','406366','407405',
'409665','409666','409667','409668','409669','409670','409672','410062','412962','415599','421317',
'421349','421393','421437','421865','421869','421870','421871','422160','422161','425862','427010',
'427018','427019','427020','427029','427030','427039','427570','427571','433666','433667','433670',
'434061','434062','434910','435744','435745','436718','436728','436738','436742','436745','436748',
'438088','438125','438126','438588','439188','439225','439226','439227','442729','442730','451289',
'451290','451804','451810','451811','453242','456351','456418','458071','458123','458124',
'458441','461982','463758','464580','468203','472067','472068','479228','479229','481699','483536',
'486466','486493','486494','486861','487013','489592','489734','489735','489736','491031',
'491032','491035','491037','491038','498451','510529','512315','512316','512411','512412',
'512425','512431','512466','513685','514027','514906','514957','514958','515672','517636','517650',
'518212','518364','518377','518378','518379','518474','518475','518476','518710','518718','519412',
'519413','519498','519961','520082','520083','520108','520131','520152','520169','520194','520382',
'521302','521899','522001','522964','523036','523959','524011','524031','524047','524070','524090',
'524091','524094','524374','524864','524865','525498','525745','525746','525998','526410','526836',
'526855','527414','528020','528057','528708','528709','528856','528931','528948','530970',
'530990','531659','531693','532450','532458','539867','539868','543098',
'543159','544033','544210','544243','544887','545324','545392','545393','545431','545447','547766',
'548478','548738','548838','548844','548943','549633','550213','552245','552288','552398','552534',
'552587','552742','552794','552801','552853','553131','553242','556610','556617','557080','558360',
'558730','558868','558894','558895','558916','559051')
;

print 'Update UNIONPAY Card Type 5';--20151202
update [abc096].[df_Transactions_Month] set
CARD_TYPE= 'UNIONPAY'
Where SUBSTRING(MASK,1,5) in
('69075','90030','90592',
'95599','45806','49102',
'49104','53098','53242',
'53243','53591','53783');
print 'Update UNIONPAY Card Type 4';--20151202
update [abc096].[df_Transactions_Month] set
CARD_TYPE= 'UNIONPAY'
Where SUBSTRING(MASK,1,4) = '9111'
;

print 'Set ACQUIRER_BANK_ID=0';
update  [abc096].[DF_Transactions_Month] set [ACQUIRER_BANK_ID]=0
;
print 'Set ISSUER_BANK_ID=0';
update  [abc096].[DF_Transactions_Month] set  [ISSUER_BANK_ID]=0
;
print 'Set GREEK_ISSUER=0';
update  [abc096].[DF_Transactions_Month] set  [GREEK_ISSUER]='   '
;
print 'Set ON_US= *Blanks';
update [abc096].[DF_Transactions_Month] set
[ON_US]='   '
;

/*
-- Replaced with new code below    LN 20170929
--Update Acquirer id
print '--Update Acquirer id';
update [abc096].[DF_Transactions_Month] set
 [ACQUIRER_BANK_ID] = (select id from abc096.banks where [abc096].[DF_Transactions_Month].destcomid=abc096.banks.destcomid)
;
--Update Issuer id - 1
print '--Update Issuer id - 1';
update [abc096].[DF_Transactions_Month] set
 [ISSUER_BANK_ID] = (select bankid from abc096.products where substring([abc096].[DF_Transactions_Month].mask,1,6) between abc096.products.BIN and abc096.products.BINU and bankid <>0)
where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
--Update Issuer id - 2
print '--Update Issuer id - 2';
update [abc096].[DF_Transactions_Month] set
 [ISSUER_BANK_ID] = (select bankid from abc096.products where substring([abc096].[DF_Transactions_Month].mask,1,5) between abc096.products.BIN and abc096.products.BINU and bankid <>0)
where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
--Update Issuer id - 3
print '--Update Issuer id - 3';
update [abc096].[DF_Transactions_Month] set
 [ISSUER_BANK_ID] = (select bankid from abc096.products where substring([abc096].[DF_Transactions_Month].mask,1,4) between abc096.products.BIN and abc096.products.BINU and bankid <>0)
where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
--Update Issuer id - 4
print '--Update Issuer id - 4';
update [abc096].[DF_Transactions_Month] set
 [ISSUER_BANK_ID] = (select bankid from abc096.products where substring([abc096].[DF_Transactions_Month].mask,1,3) between abc096.products.BIN and abc096.products.BINU and bankid <>0)
where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;
*/

-- New code for replacement    LN 20170929
--Update Acquirer id
print '--Update Acquirer id';update a
set  [ACQUIRER_BANK_ID] = id
 from [abc096].[DF_Transactions_Month] as a
  inner join abc096.banks_new b on a.destcomid=b.destcomid
  ;

--Update Issuer id - 1
print '--Update Issuer id - 1';
update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[DF_Transactions_Month] as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,6) = b.BIN /*and b.BANKID <>0*/
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 2
print '--Update Issuer id - 2';
  update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[DF_Transactions_Month] as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,5) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 3
print '--Update Issuer id - 3';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[DF_Transactions_Month] as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,4) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update Issuer id - 4
print '--Update Issuer id - 4';
    update  a
  set a.[ISSUER_BANK_ID] = b.bankid
  from [abc096].[DF_Transactions_Month] as a
  inner join zacreporting.dbo.products b on substring(a.mask,1,3) = b.BIN and b.BANKID <>0
  where [ISSUER_BANK_ID]= 0  or  [ISSUER_BANK_ID] is null
;

--Update ACQUIRER_BANK_ID=0 where ACQUIRER_BANK_ID is null
print '--Update ACQUIRER_BANK_ID=0 where ACQUIRER_BANK_ID is null';
update  [abc096].[DF_Transactions_Month] set [ACQUIRER_BANK_ID]=0 where [ACQUIRER_BANK_ID] is null
;

--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null
print '--Update ISSUER_BANK_ID=0 where ISSUER_BANK_ID is null';
update  [abc096].[DF_Transactions_Month] set  [ISSUER_BANK_ID]=0 where [ISSUER_BANK_ID] is null
;

-- LN 20170929 Added xtra condition for Greek Issuer update, --[ISSUER_BANK_ID]< 42--
--Update GREEK_ISSUER FLAG
print '--Update GREEK_ISSUER FLAG-1';
update [abc096].[DF_Transactions_Month] set
[GREEK_ISSUER]='Yes' where [ISSUER_BANK_ID]< 42 and   [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;
/*
-- Removed as all BINs are uploaded in Products table      LN20170929
print '--Update GREEK_ISSUER FLAG-2';
update [abc096].[DF_Transactions_Month] set
[GREEK_ISSUER]='Yes' where substring([abc096].[DF_Transactions_Month].mask,1,6) in (select BIN from visa_BINS_201311 where country ='Greece')
and [GREEK_ISSUER]='' or [GREEK_ISSUER] is null
;
print '--Update GREEK_ISSUER FLAG-3';
update [abc096].[DF_Transactions_Month] set
[GREEK_ISSUER]='Yes' where substring([abc096].[DF_Transactions_Month].mask,1,6) in (select BIN from [dbo].[MC_GREEK_BINS] where country ='Greece')
and [GREEK_ISSUER]='' or [GREEK_ISSUER] is null
;
*/

print '--Update GREEK_ISSUER FLAG-4';
update [abc096].[DF_Transactions_Month] set
[GREEK_ISSUER]='No' where  [GREEK_ISSUER]<>'Yes' or [GREEK_ISSUER] is null
;

--Update  [ON_US]  FLAG-1
print '--Update  [ON_US]  FLAG-1';
update [abc096].[DF_Transactions_Month] set
[ON_US]='Yes' where  [ISSUER_BANK_ID]= [ACQUIRER_BANK_ID]
;

--Update  [ON_US]  FLAG-2
print '--Update  [ON_US]  FLAG-2';
update [abc096].[DF_Transactions_Month] set
[ON_US]='No' where  [ISSUER_BANK_ID]<>[ACQUIRER_BANK_ID]
;

--Update ISSUING BANK
print '--Update ISSUING BANK';
update [abc096].[DF_Transactions_Month] set
 [ISSUING_BANK] = cast((select bank from abc096.banks_new where [abc096].[DF_Transactions_Month].[ISSUER_BANK_ID]=abc096.banks_new.id) as nvarchar(50))
where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
;

--Update REGION EU
print '----Update REGION EU';
with bingroup (binmask) as(
   select (substring(a.mask,1,6))
   from zacreporting.[abc096].[DF_Transactions_Month] a
   group by  (substring(a.mask,1,6)) ),
 totupdbins (bin, reg_eu ) as(
   select b.bin, b.regioneu
   from bingroup a
   inner join  zacreporting.dbo.binbase b on  a.binmask = b.bin   )
    --select * from totupdbins
update a
  set REGIONEUFL = reg_eu
  from zacreporting.[abc096].[DF_Transactions_Month] as a
  inner join totupdbins on bin =(substring(mask,1,6))
  where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
  ;

--Update ISOCOUNTRY
print '----Update ISO COUNTRY';
with bingroup (binmask) as(
   select (substring(a.mask,1,6))
   from zacreporting.[abc096].[DF_Transactions_Month] a
   group by  (substring(a.mask,1,6)) ),
 totupdbins (bin, cntry_fnl ) as(
   select b.bin, b.isocountry
   from bingroup a
   inner join  zacreporting.dbo.binbase b on  a.binmask = b.bin   )
    --select * from totupdbins
update a
  set ISOCNTRY = cntry_fnl
  from zacreporting.[abc096].[DF_Transactions_Month] as a
  inner join totupdbins on bin =(substring(mask,1,6))
  where [ISSUER_BANK_ID]<> 0  and  [ISSUER_BANK_ID] is not null
  ;


---Select Data to display [Summary]
print '---Select Data to display [Summary]';
SELECT
MERCHANT_NAME,
MID,
ACQUIRING_BANK,[GREEK_ISSUER],
ON_US,
CARD_TYPE,
SUM(CASE PROCCODE
WHEN '200000' THEN -TAMOUNT
WHEN '020000' THEN -TAMOUNT
ELSE TAMOUNT
END) as TOTAL_AMOUNT,
--SUM(TAMOUNT) as TOTAL_AMOUNT,
COUNT(*) TOTAL_TRANSACTIONS
from
 [abc096].[DF_Transactions_Month]
GROUP BY
MERCHANT_NAME,
MID,
ACQUIRING_BANK,[GREEK_ISSUER],
ON_US,
CARD_TYPE
order by
MERCHANT_NAME,
MID,
ACQUIRING_BANK,[GREEK_ISSUER],
ON_US,
CARD_TYPE


  ---Select Data to display [Details Per Store]
  -- LN 20170929   Added new field for Region EU
  print '--Select Data to display [Details Per Store]';
  SELECT
  MERCHANT_NAME,
  MID,
  abc096.tids.shop as Store_Name,
  --dbo.VTIDsPeriod.shop as Store_Name, 20150804
  ACQUIRING_BANK,[GREEK_ISSUER],
  REGIONEUFL,
  DMID,
  ON_US,
  CARD_TYPE,
  SUM(CASE PROCCODE
  WHEN '200000' THEN -TAMOUNT
  WHEN '020000' THEN -TAMOUNT
  ELSE TAMOUNT
  END) as TOTAL_AMOUNT,
  --SUM(TAMOUNT) as TOTAL_AMOUNT,
  COUNT(*) TOTAL_TRANSACTIONS
  from  [abc096].[DF_Transactions_Month]
  --LEFT JOIN dbo.VTIDsPeriod ON  [abc096].[DF_Transactions_Month].TID = dbo.VTIDsPeriod.TID 20150804
  LEFT JOIN abc096.tids ON  [abc096].[DF_Transactions_Month].TID = abc096.tids.TID
  GROUP BY
  MERCHANT_NAME,
  MID,
  --dbo.VTIDsPeriod.shop, 20150804
  abc096.tids.shop,
  ACQUIRING_BANK,[GREEK_ISSUER],
  REGIONEUFL,
  DMID,
  ON_US,
  CARD_TYPE
  order by
  MERCHANT_NAME,
  MID,
  --dbo.VTIDsPeriod.shop, 20150804
  abc096.tids.shop,
  ACQUIRING_BANK,[GREEK_ISSUER],
  DMID,
  ON_US,
  CARD_TYPE
  

--Select Data to display [Details Per Store]
print '--Select Data to display [Details Per Store]';
SELECT
     [MERCHANT_NAME] ,
     [MID]  ,
     [abc096].[DF_Transactions_Month].[TID]  ,
     --dbo.VTIDsPeriod.shop as Store_Name, 20150804
     abc096.tids.shop as Store_Name,
     [MSGID],
     [TAMOUNT],
     [DTSTAMP],
     [TAUTHCODE],
     [MASK] ,
     [CARD_TYPE],
     [ISSUING_BANK],
     [PROCCODE],
     [INST] ,
     [RESPKIND],
     [TRESPONSE],
     [ACQUIRING_BANK],
     [DMID],
     [DTID] ,
     [ON_US]
from  [abc096].[DF_Transactions_Month]
--LEFT JOIN dbo.VTIDsPeriod ON  [abc096].[DF_Transactions_Month].TID = dbo.VTIDsPeriod.TID 20150804
LEFT JOIN abc096.tids ON  [abc096].[DF_Transactions_Month].TID = abc096.tids.TID
where
[GREEK_ISSUER]='Yes'
order by
    [MERCHANT_NAME] ,
     [MID]  ,
     [abc096].[DF_Transactions_Month].[TID]  ,
     --dbo.VTIDsPeriod.shop , 20150804
	 abc096.tids.shop,
     [ACQUIRING_BANK],
     [ISSUING_BANK]


