--declare  @lstDigiTID as varchar
--set  @lstDigiTID = '9'

select distinct top 10  cast(vc30.relation.TERMID as varchar(30)) as TID,
vc30.relation.CLUSTERID as CLUSTER_DESCR,
cast(a.[value] as varchar(30)) as MID,
b.[value] as [ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ],
c.[value] as [ΔΙΕΥΘΥΝΣΗ],
d.[value] as [ΠΟΛΗ],
e.[value] as [ΤΗΛΕΦΩΝΟ],
o.[value] as [ΣΥΝΔΕΣΗ],
p.[value] as [ΤΥΠΟΣ ΚΑΡΤΑΣ SIM],
case (q.[value]) when '99:99' then 'NO' else q.[value] end  as [ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ],
case (r.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [CONTACTLESS],
x.evdate as [LAST PARAMETER CALL],                                                                             -- vc30.relation.LASTPARAM_DLD_DATE as [LAST PARAMETER CALL]
xx.evdate as [LAST FAILED CALL],
case (t.[value]) when '1' then 'RS232' when '2' then 'TCP' when '3' then 'TCP+RS232' when '4' then 'USB' when '6' then 'TCP+USB' else 'NO CONNECTION' end  as [ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ],
x.late_versio as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ],                                                                    --(Replaced)vc30.relation.appnm as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ]
vc30.relation.famnm as [ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ],
vc30.TERMINFO.datecreated as [BIRTH DATE]
from (vc30.relation
      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC = 'MERCHID'
      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC = 'RECEIPT_LINE_1'
      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC = 'RECEIPT_LINE_2'
      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC = 'RECEIPT_LINE_3'
      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC = 'RECEIPT_LINE_4'
      left join vc30.PARAMETER o on vc30.relation.TERMID=o.PARTID and o.PARNAMELOC = 'MEDIA'
      left join vc30.PARAMETER p on vc30.relation.TERMID=p.PARTID and p.PARNAMELOC = 'CONFIG_APN'
      left join vc30.PARAMETER q on vc30.relation.TERMID=q.PARTID and q.PARNAMELOC = 'AUTOCALL'
      left join vc30.PARAMETER r on vc30.relation.TERMID=r.PARTID and r.PARNAMELOC = 'CONTACTLESS'
      left join vc30.PARAMETER t on vc30.relation.TERMID=t.PARTID and t.PARNAMELOC = 'DLL_CONN'
      left join vc30.TERMINFO    on vc30.relation.TERMID=termid_term
      join  (select e.*, case (substring(e.vers_epos,9,1)) when ',' then substring(e.vers_epos,1,8) + 'P' when 'P' then (e.vers_epos) end as late_versio
			 from
			  (select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('EPOS', a.appnm),9) vers_epos
			   from vc30.termlog a,(select  max(evdate) date_max, termid  
									from vc30.termlog 
			                        where substring(vc30.termlog.termid,1,4) = '7300' 
									-- and  RIGHT(vc30.termlog.TERMID,1) = @lstDigiTID 
									 and message = 'Download Successful' 
									 and status = 'S'
								    group by termid) q
              where  a.evdate = q.date_max 
			     and a.termid = q.termid 
				 and appnm like '%EPOS%' )e
			) x on vc30.relation.TERMID=x.termid
    join (select e.*,case (substring(e.vers_epos,9,1)) when ',' then substring(e.vers_epos,1,8) + 'P' when 'P' then (e.vers_epos) end as late_versio
	      from
           (select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('EPOS', a.appnm),9) vers_epos
            from vc30.termlog a,(select  max(evdate) date_max, termid  
								 from vc30.termlog 
								 where substring(vc30.termlog.termid,1,4) = '7300' 
								 -- and  RIGHT(vc30.termlog.TERMID,1) = @lstDigiTID and 
								 and message <> 'Download Successful' 
								  and status <> 'S'
								 group by termid) q
            where  a.evdate = q.date_max 
			   and a.termid = q.termid 
			   and appnm like '%EPOS%' )e
			) xx on vc30.relation.TERMID=xx.termid
      )
where substring(cast(vc30.relation.appnm as char(10)),9,1) = ('P')
--and  vc30.relation.CLUSTERID not in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP','PIRAEUS','DEFAULT','TIRANA','E360F_UK','E360F_IT','E360F_AU') 
and  vc30.relation.CLUSTERID  in ('EPOS_ATTICA','EPOS_DUTYFREE','EPOS_KOTSOVOLOS','EPOS_CHALKIADAKIS','EPOS_VEROPOULOS','EPOS_VERTICE','EPOS_AB_FRANCHISE','EPOS_LEROY_MERLIN','EPOS_SEVEN','EPOS_NAVY_GREEN','EPOS_ALOUETTE','EPOS_TRADESTATUS','EPOS_POLO_R._LAUREN','EPOS_RL_HELLAS_RESOR','EPOS_RAXEVSKY','EPOS_TSAKIRIS_MALLAS','EPOS_MOTHERCARE','EPOS_AEGEAN','EPOS_AUTOTECHNICA','EPOS_COSMOTE','EPOS_COSMOTE_KIOSK','EPOS_G_TACHIDROMIKI','EPOS_COSMOS_SPORT','EPOS_HERTZ','EPOS_METRO','EPOS_REGENCY','EPOS_FLEXUS','EPOS_PUBLIC','EPOS_FUNKY BUDDHA','EPOS_RVU','EPOS_THANOPOULOS','EPOS_AB_VASILOPOULOS','EPOS_NOTOS','EPOS_FOLLI_FOLLIE','EPOS_INTERSPORT','EPOS_ORIFLAME','EPOS_IKEA','EPOS_LIDL','EPOS_ELL.DIANOMES')
--and substring(vc30.relation.CLUSTERID,1,4) = 'EPOS' 
AND substring(vc30.relation.termid,1,4) = '7300' 
--AND RIGHT(vc30.relation.TERMID,1) = @lstDigiTID ;
