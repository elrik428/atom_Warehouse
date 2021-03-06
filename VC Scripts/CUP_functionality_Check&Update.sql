-- Check CUP functionality

select a.[APPNM],clusterid,COUNT(*) from vc30.RELATION a join [vc30].[TERM_DLD_FILES] b on a.TERMID=b.[TERMID] and a.[APPNM]=b.[APPNM]
where SERFILENM like '%CUP%' group by a.[APPNM],clusterid order by a.[APPNM],clusterid


select *
--distinct SERFILENM
--CLUSTERID, a.TERMID
from TERM_DLD_FILES a
join RELATION b on a.TERMID=b.[TERMID] and a.[APPNM]=b.[APPNM]
where
SERFILENM   like  '%CUP%' and
b.clusterid  in ('EPOS_PIRAEUS','EPOS_DUTYFREE','EPOS_ELL.DIANOMES','EPOS_NOTOS','EPOS_MOTHERCARE','EPOS_ATTICA','EPOS_INTERSPORT','EPOS_TRADESTATUS') and SERFILENM like'%P.ZIP%' and substring(a.TERMID,1,4) = '7300'
order by CLUSTERID

-- Update CUP
update TERM_DLD_FILES
set SERFILENM = 'ParamFiles\_CUP_NoNCVM_AllCVMs\P.zip'
where TERMID in
('73006908','53535353','73007422','73000254','73006912','B00033333','73000266','73002647','73002652','73002653','73002546','73002547','73002548','73002649','73002651','73007895','73007631','73000846','73000849','73002689','73002682','73000848','73007745','73003004','73003011','73003014','73003020','73003021','73003022','73003032','73005420','73005947','73006154','73007302','73007859','73003012','520TRADE','73003003','73003005','73003007','73003008','73003009','73003010','73003013','73003015','73003023','73003024','73003025','73003026','73003027','73003033','73003034','73003035','73003036','73005421','73005436','73005437','73007301')
--and SERFILENM  like  '%CUP%'
and SERFILENM like'%P.ZIP%'



---- Enable Disable CUP
select substring([value],10,1), *,  substring([value],1,9) + '0' + substring([value],11,50)  -- Disable
--substring([value],1,9) + '1' + substring([value],11,50)  -- Enable
from vc30.PARAMETER
where PARTID in
('01348501','01348511','01348521','01348542','01349142','01349182','01349194','01349235','01349242','01349252','19813238','01349314','01349344','01349362','01349382','01349392','19813884','01348732','01348752','01348784','01348802','01348923','01348952','01349002','01349012','01349053','19812922','01349092','01349103','01349112','01349113')
and PARNAMELOC = 'CARD08'

update vc30.PARAMETER
set [VALUE] = substring([value],1,9) + '1' + substring([value],11,50)
where PARTID in
('01348501','01348511','01348521','01348542','01349142','01349182','01349194','01349235','01349242','01349252','19813238','01349314','01349344','01349362','01349382','01349392','19813884','01348732','01348752','01348784','01348802','01348923','01348952','01349002','01349012','01349053','19812922','01349092','01349103','01349112','01349113')
and PARNAMELOC = 'CARD08'



---- CUP Parameter Insert - Update
--1.
 select * from vc30.term_dld_files
 where termid in ('33333333','520INTER','675INTER','73000850','73000851')
 and SERFILENM = 'ParamFiles\_CUP_NoNCVM_AllCVMs\P.zip'

--2.
 update vc30.term_dld_files 
 set serfilenm = 'ParamFiles\_CUP_NoNCVM_AllCVMs\P.zip'
 where serfilenm = 'ParamFiles\NoNCVM_AllCVMs\P.zip'
 and termid in ('33333333','520INTER','675INTER','73000850','73000851')

--3.
select b.PARTID, b.APPNM, b.FAMNM, PARNAMELOC, [value]   from vc30.vc30.PARAMETER  b
where
 PARTID in ('33333333','520INTER','675INTER','73000850','73000851')
 and  PARnameloc like ('CARD14%')
 and [value] like '%UNION%'

--3a.
select b.PARTID, b.APPNM, b.FAMNM, PARNAMELOC,
[value] ,substring([value],1,9)+'0'+substring([value],11,70)
   from vc30.vc30.PARAMETER  b
where
substring([value],10,1) = '1' and
 --PARTID in
  --('73007242','73007243','73007244','73007245','73007246','73007247','73007248','73007249','73007250','73007251')
  --and
  [value] like '%UNION%'


--4.
-- Use the claasic cursor for insert. Below is the main INSERT
 insert into vc30.PARAMETER
        (FAMNM, APPNM, PARTID, PARNAMELOC, SEQINFO, DLDTYPE,PARINFO, VALUE,PSIZE,FLAG)
        (
        select @FromModel, @FromAppnm, @TID, PARNAMELOC, (select  max(seqinfo) + 1 from vc30.PARAMETER where PARTID = @TID), DLDTYPE,PARINFO,
        'UNIONPAY:1:1:1:0:0:0:0:1:0:2:0:0:000000000000:000000:300000',
        PSIZE,FLAG
        from vc30.parameter where PARTID = 'TEPOS0211P' and PARNAMELOC = 'CARD14' and
		famnm = @FromModel
		        )

--5. update
update vc30.vc30.PARAMETER
set [value] = substring([value],1,9)+'0'+substring([value],11,70)
where
PARTID in
 ('01036650','01036730','01037150')
 and [value] like '%UNION%'



