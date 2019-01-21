
declare @FromModel varchar(20)
declare @ToModel varchar(20)
declare @FromAppnm varchar(10)
declare @ToAppnm varchar(10)
declare @FromAppnm1 varchar(10)
declare @ToAppnm1 varchar(10)

-- 2. Modify the 4 variables below accordingly
--

--SET @FromModel='Vx-520'
--SET @ToModel='Vx-520'
 SET @FromModel='Vx-675'
 SET @ToModel='Vx-675'
-- SET @FromModel='Vx-675WiFi'
-- SET @ToModel='Vx-675WiFi'
SET @FromAppnm='PIRA01A6P'
SET @ToAppnm='PIRA0204P'
SET @FromAppnm1='PIRA01A6'
SET @ToAppnm1='PIRA0204'


-- 1 Create new app on all TIDs in the target model based on the TIDs found in the source model and app name combination.

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

('01228761')

-- 2 Create new parameters app on all TIDs in the target model based on the TIDs found in the source model and app name combination.
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

('01228761')

-- 3 Copy parameters from source app to target app
insert into vc30.PARAMETER (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
select @ToModel model,@ToAppnm, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG
from vc30.parameter
where famnm = @FromModel and appnm in (@FromAppnm, @FromAppnm1)
-- for specific terminal
and PARTID in

('01228761')

-- Copy Parameters from one app to another app
-- 1 *NOTE: Ensure that the new app is created first in the target Model using Model/App Manager


-- 4  update the file paths

update vc30.term_dld_files
set appnm = @ToAppnm
where famnm = @FromModel and appnm = @FromAppnm
AND termid in

('01228761')

--------------------------------------------------
-- 5  Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in

('01228761')

-- 6 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in
('01228761')

 -- 7 Update USES
update vc30.PARAMETER set value = 'TPIRA0204P' where parnameloc = 'USES' and partid in
('01228761')

-- Update LOGO parm
update vc30.PARAMETER
set [value] = 'F:KATASTASHA_D.VFT'
where PARNAMELOC = 'LOGO_DISP_EN' and PARTID in
('01228761')


GO



------- UNZIP
--select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
--('01228761')
--group by value

---- USES
--select value,count(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
--('01228761')
--group by value
