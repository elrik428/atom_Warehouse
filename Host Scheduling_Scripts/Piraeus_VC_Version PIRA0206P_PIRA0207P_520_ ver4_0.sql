
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
 declare @FromCTLS varchar(10)
 declare @ToCTLS varchar(10)
-- -------- version 2.0

--**
SET @FromModel='Vx-520'
SET @ToModel='Vx-520'
----**
SET @FromAppnm='PIRA0206P'
SET @ToAppnm='PIRA0207P'
SET @FromAppnm1='PIRA0206'
SET @ToAppnm1='PIRA0207'
--
-- -------- version 2.0
 SET @FromCTLS='CTLS20116'
 SET @ToCTLS='CTLS20116B'
-- --------
--
-- -------- version 2.0 CTLS
 insert into vc30.RELATION
 (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
 ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
 FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
 select @ToModel,@ToCTLS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
 from vc30.relation
 where famnm = @FromModel and appnm = @FromCTLS
 -- for specific terminal
 and TERMID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

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
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')


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
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')



-- 5 Copy parameters from source app to target app
insert into vc30.PARAMETER (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
select @ToModel model,@ToAppnm, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG
from vc30.parameter
where famnm = @FromModel and appnm in (@FromAppnm, @FromAppnm1)
-- for specific terminal
and PARTID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')


--
-- update the file paths
update vc30.term_dld_files
set appnm = @ToAppnm
where famnm = @FromModel and appnm = @FromAppnm
AND termid in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

--------------------------------------------------

-- 6.1 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

---
-- 6.2 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

--
-- -------- version 2.0 CTLS
-- -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromCTLS
 -- for specific terminal
 and TERMID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')
----
------

Print 'Update USES'
update vc30.PARAMETER set value = 'TPIRA0207P' where parnameloc = 'USES' and partid in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

Print'Delete UNZIP'
delete from vc30.PARAMETER where parnameloc = '*unzip' and partid in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')

--
-- Update LOGO parm
update vc30.PARAMETER
set [value] = 'F:KATASTASHA_D.VFT'
where PARNAMELOC = 'LOGO_DISP_EN' and PARTID in
('01004782','01067821','01267161','01318520','01368810','01419940','01497250','01542640','01547320','01589480','01647510','01835030')



GO









GO



------- UNZIP
--select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
--('01228761')
--group by value

---- USES
--select value,count(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
--('01228761')
--group by value
