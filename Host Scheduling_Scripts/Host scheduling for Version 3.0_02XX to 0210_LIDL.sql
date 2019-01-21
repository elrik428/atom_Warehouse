
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
SET @FromModel='Vx-820'
SET @ToModel='Vx-820'
----**
SET @FromAppnm='EPOS0201P'
SET @ToAppnm='EPOS0210P'
SET @FromAppnm1='EPOS0201'
SET @ToAppnm1='EPOS0210'
--
-- -------- version 2.0
 SET @FromEMV='EMV800'
 SET @ToEMV='EMV8002'
 SET @FromEOS='EOS020816'
 SET @ToEOS='EOS020816B'
 SET @FromOS='QT000520'
 SET @ToOS='0QT000530'

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
 where famnm = @FromModel and appnm = @FromOS
 -- for specific terminal
 and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')


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
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')



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
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--   -------- version 2.0 OS
 insert into vc30.RELATION
 (FAMNM,APPNM,TERMID,CLUSTERID,ACCCNT,LASTFULL,LASTPAR,ACCCODE,VIOLATIONCOUNT,LOCKED,MODON,MODBY,LOCKTIMESTAMP,EPROMID,DESCRIPTION,DLD_STATUS,
 ISAUTODOWNLOAD,LAST_ATTEMPTED_DLD_DATE,VERSION,LASTPARAM_DLD_DATE,LASTFILE_DLD_DATE,FORUSES,FORMVIEWTYPE,SERVERID,TERM_FILE_UPDATES,
 FORCEFILEDLD,FORCEPARAMDLD,FORCETERMFILEDLD)
 select @ToModel,@ToOS,TERMID,CLUSTERID,ACCCNT,NULL,NULL,ACCCODE,0,LOCKED,getdate(),
 'SCRIPT1',NULL,NULL,DESCRIPTION,NULL,'N',NULL,NULL,NULL,NULL,FORUSES,FORMVIEWTYPE,NULL,TERM_FILE_UPDATES,'D','D','D'
 from vc30.relation
 where famnm = @FromModel and appnm = @FromOS
 -- for specific terminal
 and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

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
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')


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
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')



-- 5 Copy parameters from source app to target app
insert into vc30.PARAMETER (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
select @ToModel model,@ToAppnm, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG
from vc30.parameter
where famnm = @FromModel and appnm in (@FromAppnm, @FromAppnm1)
-- for specific terminal
and PARTID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')


--
-- update the file paths
update vc30.term_dld_files
set appnm = @ToAppnm
where famnm = @FromModel and appnm = @FromAppnm
AND termid in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--------------------------------------------------

-- 6.1 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm
-- for specific terminal
and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

---
-- 6.2 Delete old parameters
delete from vc30.RELATION
where famnm = @FromModel and appnm = @FromAppnm1
-- for specific terminal
and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--
-- -------- version 2.0 EOS
-- -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromEOS
 -- for specific terminal
 and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--
-- -------- version 2.0 OS
 -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromOS
 -- for specific terminal
 and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--
-- -------- version 2.0 EMV
-- -- 6.2 Delete old parameters
 delete from vc30.RELATION
 where famnm = @FromModel and appnm = @FromEMV
 -- for specific terminal
 and TERMID in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

--
Print 'Update USES'
update vc30.PARAMETER set value = 'TEPOS0210P' where parnameloc = 'USES' and partid in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')

Print'Delete UNZIP'
delete from vc30.PARAMETER where parnameloc = '*unzip' and partid in
('73004373','73004374','73005937','73004372','73004371','73004552','73004554','73004553','73004636','73004551','73004739','73004637','73004558','73004559','73004561','73005936','73004560','73004034','73004033','73004031','73004032','73004547','73004548','73004549','73004550','73004573','73004571','73004570','73004572','73004745','73004750','73004749','73004748','73004747','73004746','73006177','73004564','73004563','73004562','73004117','73004120','73004118','73004121','73004119','73004688','73004617','73004616','73004614','73004613','73004615','73004660','73007553','73004658','73004659','73006017','73004657','73004669','73004670','73005201','73004672','73004671','73004676','73004677','73004674','73004675','73005941','73004673','73004666','73004662','73004664','73004665','73004663','73004661','73006364','73004654','73004655','73004652','73004656','73004653','73004703','73004702','73003753','73004701','73004700','73006718','73004400','73004402','73004404','73004403','73004401','73004685','73004684','73004686','73004683','73004704','73004705','73004707','73004744','73004706','73007568','73007564','73007567','73007565','73007566','73007569','73007433','73007435','73007429','73007430','73007431','73007432','73007434','73005896','73005892','73005890','73005894','73005895','73005891','73005893','73006740','73006738','73006741','73006736','73006742','73006737','73006739','73006747','73006744','73006749','73006746','73006750','73006748','73006745','73006757','73006753','73006758','73006756','73006759','73006752','73006755','73006754','73004056','73006173','73004058','73004060','73004057','73004059','73004295','73004296','73005879','73004297','73004298','73004354','73004352','73004355','73004356','73004353')
--

GO

------ USES
--select value,count(*) from vc30.PARAMETER where parnameloc = 'USES' and partid in
--('01779300','01908691')
--group by value

--------- UNZIP
--select value,count(*) from vc30.PARAMETER where parnameloc = '*unzip' and partid in
--('01779300','01908691')
--group by value
