44444444 --820
00033333 --520
TESTCTLS --675

--1. Check version

select distinct CLUSTERID,termid, appnm  from vc30.relation where CLUSTERID = 'EPOS_TRADESTATUS'
and substring(appnm,1,4) = ('EPOS') and substring(appnm,9,1) = ('P') and acccnt = -1
and /*appnm NOT in ('EPOS01D0P') and*/ termid in
('73003004','73003007','73005420')
--('520GLEROY','520LEROY','675LEROY','73002563','73002564','73002569','73002570','73002571','73002572','73002573','73002574','73002575','73002578','73002579','73002580','73002581','73002582','73002583','73002584','73002585','73002586','73002589','73002590','73002591','73002593','73002594','73002595','73002596','73002597','73002598','73002599','73002600','73002601','73002602','73002603','73002604','73002605','73002606','73002607','73002608','73002609','73002610','73002611','73002612','73002614','73002615','73002616','73002617','73002619','73002620','73002621','73002624','73002625','73002626','73002627','73002629','73002630','73002631','73002632','73002634','73002635','73002636','73002637','73002638','73002639','73002641','73002642','73002643','73002644','73002645','73004818','73005503','73005504','73005542','73005543','73006018','73006019','73006020','73006021','73006022','73006023','73006024','73006025','73006026','73006027','73006028','73006029','73006030','73006031','73006032','73006033','73006034','73006035','73006036','73006037','73006038','73006039','73006040','73006041','73006042','73006043','73006356','73006357','73006358','73006359','73006360','73006361','73006362')

--2. Amount of versions

select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
--
substring(appnm,1,4) = ('EPOS') and substring(appnm,9,1) = 'P' and
--
TERMID in
('73003004','73003007','73005420')
group by CLUSTERID,famnm,appnm

--3.Replace TIDs with new at all places and also check declarations regadinf terminal type and application version(look for double **)

-- 1 *NOTE: Ensure that the new app is created first in the target Model using Model/App Manager

declare @FromModel varchar(20)
declare @ToModel varchar(20)
declare @FromAppnm varchar(10)
declare @ToAppnm varchar(10)
declare @FromAppnm1 varchar(10)
declare @ToAppnm1 varchar(10)
-- -------- version 2.0
-- declare @FromCLA varchar(10)
-- declare @ToCLA varchar(10)
-- declare @FromCTLS varchar(10)
-- declare @ToCTLS varchar(10)
-- declare @FromOS varchar(10)
-- declare @ToOS varchar(10)
-- declare @FromEOS varchar(10)
-- declare @ToEOS varchar(10)
-- -------- version 2.0

-- 2 Modify the 4 variables below accordingly

--
  SET @FromModel='Vx-520'
  SET @ToModel='Vx-520'
  --SET @FromModel='Vx-675'
  --SET @ToModel='Vx-675'
  --SET @FromModel='Vx-820'
  --SET @ToModel='Vx-820'
--
SET @FromAppnm='EPOS01B7P'
SET @ToAppnm='EPOS01D0P'
SET @FromAppnm1='EPOS01B7'
SET @ToAppnm1='EPOS01D0'

-- SET @FromAppnm='EPOS01C3P'
-- SET @ToAppnm='EPOS0200P'
-- SET @FromAppnm1='EPOS01C3'
-- SET @ToAppnm1='EPOS0200'
--
-- -------- version 2.0
-- SET @FromCLA='CLA013645'
-- SET @ToCLA='CLA013646'
-- SET @FromCTLS='CTLS11114'
-- SET @ToCTLS='CTLS20116'
-- -- 0QT5G0111/QT650240/QT5G240/QT650240/QT520112
-- SET @FromOS='QT520112'
-- SET @ToOS='QT000520'
-- -- EOS010802/EOS011103/EOS020101
-- SET @FromEOS='EOS010802'
-- SET @ToEOS='EOS020816'
-- --------
--
-- -------- version 2.0 EMV
-- insert into vc30.RELATION
-- (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
-- ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
-- FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
-- select @ToModel,'EMV800',TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
-- 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
-- from vc30.relation
-- where famnm = @FromModel and appnm = @FromOS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
-- -------- version 2.0 EOS
-- insert into vc30.RELATION
-- (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
-- ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
-- FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
-- select @ToModel,@ToEOS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
-- 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
-- from vc30.relation
-- where famnm = @FromModel and appnm = @FromEOS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
-- --------
--
-- -------- version 2.0 OS
-- insert into vc30.RELATION
-- (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
-- ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
-- FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
-- select @ToModel,@ToOS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
-- 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
-- from vc30.relation
-- where famnm = @FromModel and appnm = @FromOS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
-- --------
--
-- -------- version 2.0 CTLS
-- insert into vc30.RELATION
-- (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
-- ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
-- FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
-- select @ToModel,@ToCTLS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
-- 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
-- from vc30.relation
-- where famnm = @FromModel and appnm = @FromCTLS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
-- --------
--
-- -------- version 2.0 CLA
-- insert into vc30.RELATION
-- (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
-- ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
-- FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
-- select @ToModel,@ToCLA,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
-- 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
-- from vc30.relation
-- where famnm = @FromModel and appnm = @FromCLA
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
-- --------
--



insert into vc30.RELATION
(FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
select @ToModel,@ToAppnm,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
from vc30.relation
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in

('73005493','73005477','73005498')


-- 4 Create new parameters app on all TIDs in the target model based on the TIDs found in the source model and app name combination.

insert into vc30.RELATION
(FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
select @ToModel,@ToAppnm1,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
from vc30.relation
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in

('73005493','73005477','73005498')


-- 5 Copy parameters from source app to target app
insert into vc30.PARAMETER (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
select @ToModel model,@ToAppnm, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG
from vc30.parameter
where famnm = @FromModel and appnm in (@FromAppnm, @FromAppnm1)
-- for specific terminal
and PARTID in

('73005493','73005477','73005498')


-- Copy Parameters from one app to another app
-- 1 *NOTE: Ensure that the new app is created first in the target Model using Model/App Manager


-- update the file paths
-- update vc30.term_dld_files
-- set appnm = @ToAppnm, serfilenm = replace(serfilenm,'\\HUBP1-POS-MG1P.commonpr.eeft.com\VCApps\LiveEvoApps\EPOS\EPOS_01A5','\\HUBP1-POS-MG1P.commonpr.eeft.com\VCApps\LiveEvoApps\EPOS\EPOS_01A6')
-- where serfilenm like '\\HUBP1-POS-MG1P.commonpr.eeft.com\VCApps\LiveEvoApps\EPOS\EPOS_01A5%'
-- and famnm = @FromModel and appnm = @FromAppnm
-- -- -- for specific terminal
-- AND termid in
--
-- ('73000010','73000011','73000012','73000251','73000252','73000253','73000255','73000256','73000257','73000258','73000259','73000260','73000261','73000262','73000263','73000264','73000265','73000267','73000268','73000269','73000270','73000271','73000272','73000273','73000274','73000275','73000276','73000277','73000278','73000279','73000280','73000281','73000282','73000283','73000284','73000285','73001399','73001457','73002541','73002542','73002550','73002967','73002968','73002969','73003099','73003100','73003101','73003102','73003103','73003104','73003105','73003106','73003107','73003341','73003342')
update vc30.term_dld_files
set appnm = @ToAppnm
where famnm = @FromModel and appnm = @FromAppnm
AND termid in

('73005493','73005477','73005498')


--------------------------------------------------

-- 6.1 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in

('73005493','73005477','73005498')


-- 6.2 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in

('73005493','73005477','73005498')


--
-- -------- version 2.0 EOS
-- -- 6.2 Delete old parameters
-- delete from vc30.RELATION
-- where famnm = @FromModel and appnm = @FromEOS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
-- --------
--
-- -------- version 2.0 OS
-- -- 6.2 Delete old parameters
-- delete from vc30.RELATION
-- where famnm = @FromModel and appnm = @FromOS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
-- --------
--
-- -------- version 2.0 CTLS
-- -- 6.2 Delete old parameters
-- delete from vc30.RELATION
-- where famnm = @FromModel and appnm = @FromCTLS
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
-- --------
--
--
-- -------- version 2.0 CLA
-- -- 6.2 Delete old parameters
-- delete from vc30.RELATION
-- where famnm = @FromModel and appnm = @FromCLA
-- -- for specific terminal
-- and TERMID in
--
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
-- ------
--

GO

--'TEPOS0200P'
update vc30.PARAMETER set value = 'TEPOS01D0P' where parnameloc = 'USES' and partid in
('73005493','73005477','73005498')
--and value = 'TEPOS01C2P'
--------------------------change *unzip parm
-- select * from vc30.PARAMETER where parnameloc = '*unzip' and partid in
-- ('520THAN','675THAN','73000007','73000008','73000009','73000226','73000227','73000228','73000229','73000230','73000231','73000232','73000233','73000234','73000235','73000236','73000237','73000238','73000239','73000240','73000241','73000242','73000243','73000244','73000245','73000246','73000247','73000248','73000249','73000250','73001400')
-- and value = 'Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01B4P.ZIP,F:EPOS01B4.ZIP,F:TEMA.ZIP,P.ZIP,F:C1.ZIP,F:C2.ZIP'
-- order by partid

select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
('73003004','73003007','73005420')
group by value

Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01B7P.ZIP,F:EPOS01B7.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP
Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01B7P.ZIP,F:EPOS01B7.ZIP,F:TEMA.ZIP,P.ZIP
-- -- WARNING for version 0200 remove unzip.
--  delete vc30.PARAMETER where parnameloc = '*unzip' and partid in
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
--520
update vc30.PARAMETER set value = 'Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01D0P.ZIP,F:EPOS01D0.ZIP,F:TEMA.ZIP,P.ZIP' where parnameloc = '*unzip' and partid in
('73003004','73003007','73005420')
and value = 'Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01B7P.ZIP,F:EPOS01B7.ZIP,F:TEMA.ZIP,P.ZIP'

--675
update vc30.PARAMETER set value = 'QT520112.ZIP,VX011-EOS.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01D0P.ZIP,F:EPOS01D0.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP' where parnameloc = '*unzip' and partid in
('73003004','73003007','73005420')
and value = 'QT520112.ZIP,VX011-EOS.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01C3P.ZIP,F:EPOS01C3.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP'

-------------change port from 8501 to 8508 ***************************
select * from vc30.PARAMETER where value = '8501' and partid in ('73005493','73005477','73005498')
update vc30.PARAMETER set value = '8508' where value = '8501' and partid in ('73005493','73005477','73005498')
select * from vc30.PARAMETER where value = '8508' and partid in ('73005493','73005477','73005498')
------------------------------------------------------------------------



4. rerun below sql for check
select CLUSTERID,famnm,appnm,count(*) from vc30.relation where
--
substring(appnm,1,4) = ('EPOS') and substring(appnm,9,1) = 'P' and
--
TERMID in
('73003004','73003007','73005420')
group by CLUSTERID,famnm,appnm

5.
--------------------------change uses parm
select value,count(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
('73003004','73003007','73005420')
group by value

--'TEPOS0200P'
update vc30.PARAMETER set value = 'TEPOS01D0P' where parnameloc = 'USES' and partid in
('73003004','73003007','73005420')

6.

select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
('73003004','73003007','73005420')
group by value

Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01B9P.ZIP,F:EPOS01B9.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP
Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01C0P.ZIP,F:EPOS01C0.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP

-- -- WARNING for version 0200 remove unzip.
--  delete vc30.PARAMETER where parnameloc = '*unzip' and partid in
-- ('73000904','73000905','73000906','73000907','73000908','73004787','73004788','73007343','73004836','73006005','73006006','73006007','73006008','73006009','73006010','73006011','73006012','73006013','73006014','73006015','73006016')
--
--520
update vc30.PARAMETER set value = 'Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01D0P.ZIP,F:EPOS01D0.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP' where parnameloc = '*unzip' and partid in
('73003004','73003007','73005420')
and value = 'Q.ZIP,E.ZIP,ACT.ZIP,CLA.ZIP,VVMAC132.ZIP,VMACIF105.ZIP,F:EPOS01C0P.ZIP,F:EPOS01C0.ZIP,F:C1.ZIP,F:C2.ZIP,P.ZIP'


7.
-------------change port from 8501 to 8508 ***************************
select * from vc30.PARAMETER where value = '8501' and partid in ('73003004','73003007','73005420')
update vc30.PARAMETER set value = '8508' where value = '8501' and partid in ('73003004','73003007','73005420')
select * from vc30.PARAMETER where value = '8508' and partid in ('73003004','73003007','73005420')
------------------------------------------------------------------------

8. INSERT NEW Parameters

-- qUERY FOR check
select * from vc30.PARAMETER where PARTID in
('73003004','73003007','73005420')
and (PARNAMELOC = 'MENUPREAUTH'
OR PARNAMELOC = 'DBLHEIGHT_UPPER'
OR PARNAMELOC = 'COMPACTEELS'
OR PARNAMELOC = 'SENDOFFL')
ORDER BY PARNAMELOC

--Procedure for insert
declare @tid varchar(15)
declare @FromModel varchar(20)
declare @FromAppnm varchar(10)

 --SET @FromModel='Vx-675'
 SET @FromModel='Vx-520'
--SET @FromModel='Vx-820'
SET @FromAppnm='EPOS01D0P'
--SET @FromAppnm='PIRA01A9P'

declare merch_cursor cursor for

select DISTINCT TERMID from vc30.relation
where
--famnm = @FromModel and appnm = @FromAppnm
 --CLUSTERID = 'EPOS_DUTYFREE' and

TERMID in

('73003004','73003007','73005420')



 --and TERMID <> '73004875'
--and TERMID not in (select partid from vc30.PARAMETER  where PARTID=termid and PARNAMELOC='ACCTYPE' and value = 'TRUE')

and FAMNM = @FromModel

open merch_cursor
if @@ERROR > 0
  return

fetch next from merch_cursor
into @tid

while @@FETCH_STATUS = 0_
begin
--  delete from vc30.PARAMETER where PARTID = @tid

        insert into vc30.PARAMETER
        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
        (
        select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
        --VALUE,
        --'FALSE',
        --'NO_PP1000',
        'TRUE',
        --'3',
        --'1',
        --'*',
        PSIZE,FLAG
        from vc30.parameter where PARTID = 'TEPOS01D0P' and PARNAMELOC = 'COMPACTEELS' and famnm = 'Vx-520'
        )

  fetch next from merch_cursor
  into @tid
end

CLOSE merch_cursor
deallocate merch_cursor

9.
prepare xls file.
Watch so every TID that is copied to xls is with its original format(a green triangle should appear in the upper left corner of the cell)
Export to csv

10. Upload to Transact
Open transact
Batch processing
Upload

11.
Reply to email
