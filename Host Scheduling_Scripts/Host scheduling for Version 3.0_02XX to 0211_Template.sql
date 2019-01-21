
declare @FromModel varchar(20)
declare @ToModel varchar(20)
declare @FromAppnm varchar(10)
declare @ToAppnm varchar(10)
declare @FromAppnm1 varchar(10)
declare @ToAppnm1 varchar(10)
-- -------- version 2.0
 declare @FromOS varchar(10)
 declare @ToOS varchar(10)
 declare @FromEOS varchar(10)
 declare @ToEOS varchar(10)
 declare @FromEMV varchar(10)
 declare @ToEMV varchar(10)
-- -------- version 2.0

--**
--SET @FromModel='Vx-675'
--SET @ToModel='Vx-675'
SET @FromModel='Vx-520'
SET @ToModel='Vx-520'
----**
SET @FromAppnm='EPOS0209P'
SET @ToAppnm='EPOS0211P'
SET @FromAppnm1='EPOS0209'
SET @ToAppnm1='EPOS0211'
--
-- -------- version 2.0
 SET @FromOS='QT000520'
 SET @ToOS='0QT000530'
 SET @FromEOS='EOS020816'
 SET @ToEOS='EOS020816B'
 SET @FromEMV='EMV800'
 SET @ToEMV='EMV8002'
-- --------
--
-- -------- version 2.0 CMA
 insert into vc30.RELATION
 (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
 ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
 FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
 select @ToModel,'CMA',TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
 from vc30.relation
 where famnm = @FromModel and appnm = @FromEOS
 -- for specific terminal
 and TERMID in
('73007492')


-- -------- version 2.0 EMV
 insert into vc30.RELATION
 (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
 ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
 FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
 select @ToModel,@ToEMV,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
 from vc30.relation
 where famnm = @FromModel and appnm = @FromEMV
 -- for specific terminal
 and TERMID in
('73007492')



 -------- version 2.0 EOS
 insert into vc30.RELATION
 (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
 ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
 FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
 select @ToModel,@ToEOS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
 from vc30.relation
 where famnm = @FromModel and appnm = @FromEOS
 -- for specific terminal
 and TERMID in
('73007492')

--   -------- version 2.0 OS
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
--('73007492')

-- --------

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
('73007492')


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
('73007492')



-- 5 Copy parameters from source app to target app
insert into vc30.PARAMETER (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
select @ToModel model,@ToAppnm, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG
from vc30.parameter
where famnm = @FromModel and appnm in (@FromAppnm, @FromAppnm1)
-- for specific terminal
and PARTID in
('73007492')


--
-- update the file paths
update vc30.term_dld_files
set appnm = @ToAppnm
where famnm = @FromModel and appnm = @FromAppnm
AND termid in
('73007492')

--------------------------------------------------

-- 6.1 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in
('73007492')

---
-- 6.2 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in
('73007492')

--
-- -------- version 2.0 EOS
-- -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromEOS
 -- for specific terminal
 and TERMID in
('73007492')

--
-- -------- version 2.0 OS
 -- 6.2 Delete old parameters
-- delete from vc30.RELATION
-- where famnm = @FromModel and appnm = @FromOS
-- -- for specific terminal
-- and TERMID in
--('73007492')

--
-- -------- version 2.0 EMV
-- -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromEMV
 -- for specific terminal
 and TERMID in
('73007492')

--
Print 'Update USES'
update vc30.PARAMETER set value = 'TEPOS0211P' where parnameloc = 'USES' and partid in
('73007492')

Print'Delete UNZIP'
delete from vc30.PARAMETER where parnameloc = '*unzip' and partid in
('73007492')
--

GO





--select * from vc30.term_dld_files where termid in
--('73007492')
--and
-- terfilenm = 'tema.zip'




------ USES
--select value,count(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
--('01779300','01908691')
--group by value

--Print 'Update USES'
--update vc30.PARAMETER set value = 'TPIRA0200P' where parnameloc = 'USES' and partid in
--('01779300','01908691')


--------- UNZIP
--select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
--('01779300','01908691')
--group by value

--Print'Delete UNZIP'
--delete from vc30.PARAMETER where parnameloc = '*unzip' and partid in
--('01779300','01908691')
