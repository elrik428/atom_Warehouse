declare  @lstDigiTID as varchar
set  @lstDigiTID = '8'

select  distinct  cast(vc30.relation.TERMID as varchar(30)) as TID,
cast(a.[value] as varchar(30)) as MID,
b.[value] as [ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ],
c.[value] as [ΔΙΕΥΘΥΝΣΗ],
d.[value] as [ΠΟΛΗ],
e.[value] as [ΤΗΛΕΦΩΝΟ],
case (f.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΔΟΣΕΙΣ],
cast(g.[value] as varchar(30)) as [ΠΛΗΘΟΣ ΔΟΣΕΙΩΝ],
h.[value] as [ΓΛΩΣΣΑ],
i.[value] as [ΠΛΗΚΤΡΟΛΟΓΗΣΗ],
case (j.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [CVV],
case (substring(k.[value],12,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [ΠΡΟΕΓΚΡΙΣΗ],
case (substring(k.[value],20,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [COMPLETION],
case (l.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [DCC],
case (substring(m.[value],10,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [CUP],
case (substring(n.[value],6,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [ΚΑΡΤΑ ΣΥΜΒΟΛΑΙΑΚΗΣ],
o.[value] as [ΣΥΝΔΕΣΗ],
p.[value] as [ΤΥΠΟΣ ΚΑΡΤΑΣ SIM],
case (q.[value]) when '99:99' then 'NO' else q.[value] end  as [ΑΥΤΟΜΑΤΗ ΑΠΟΣΤΟΛΗ ΠΑΚΕΤΟΥ],
case (r.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [CONTACTLESS],
case (s.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΦΟΡΟΚΑΡΤΑ],
x.evdate as [LAST PARAMETER CALL],                                                                             -- vc30.relation.LASTPARAM_DLD_DATE as [LAST PARAMETER CALL]
case (t.[value]) when '1' then 'RS232' when '2' then 'TCP' when '3' then 'TCP+RS232' when '4' then 'USB' when '6' then 'TCP+USB' else 'NO CONNECTION' end  as [ΣΥΝΔΕΣΗ ΤΑΜΕΙΑΚΗΣ],
case (u.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [TIP],
case (v.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [ΣΥΝΔΕΣΗ PINPAD],
x.late_versio as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ],                                                                    --(Replaced)vc30.relation.appnm as [ΕΚΔΟΣΗ ΕΦΑΡΜΟΓΗΣ]
vc30.relation.famnm as [ΜΟΝΤΕΛΟ ΤΕΡΜΑΤΙΚΟΥ],
'VeriFone' as [ΚΑΤΑΣΚΕΥΑΣΤΗΣ],
case (w.[value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end  as [LOYALTY],
vc30.TERMINFO.datecreated as [BIRTH DATE],
y.[value] as [MCC],
z.[value] as [ETH.DHCP],
case (substring(aa.[value],10,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [Refund VISA],
case (substring(ab.[value],12,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [Refund MASTERCARD],
case (substring(ac.[value],13,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end  as [Refund MAESTRO]
from (vc30.relation
      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC = 'MERCHID'
      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC = 'RECEIPT_LINE_1'
      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC = 'RECEIPT_LINE_2'
      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC = 'RECEIPT_LINE_3'
      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC = 'RECEIPT_LINE_4'
      left join vc30.PARAMETER f on vc30.relation.TERMID=f.PARTID and f.PARNAMELOC = 'INSTALMENT'
      left join vc30.PARAMETER g on vc30.relation.TERMID=g.PARTID and g.PARNAMELOC = 'MAXINST'
      left join vc30.PARAMETER h on vc30.relation.TERMID=h.PARTID and h.PARNAMELOC = 'LANGUAGE'
      left join vc30.PARAMETER i on vc30.relation.TERMID=i.PARTID and i.PARNAMELOC = 'MANUALENTRY'
      left join vc30.PARAMETER j on vc30.relation.TERMID=j.PARTID and j.PARNAMELOC = 'ASKFORCVV2'
      left join vc30.PARAMETER k on vc30.relation.TERMID=k.PARTID and k.PARNAMELOC = 'CARD01'
      left join vc30.PARAMETER l on vc30.relation.TERMID=l.PARTID and l.PARNAMELOC = 'DCCCAPABLE'
      left join vc30.PARAMETER m on vc30.relation.TERMID=m.PARTID and m.PARNAMELOC = 'CARD08'
      left join vc30.PARAMETER n on vc30.relation.TERMID=n.PARTID and n.PARNAMELOC = 'CARD07'
      left join vc30.PARAMETER o on vc30.relation.TERMID=o.PARTID and o.PARNAMELOC = 'MEDIA'
      left join vc30.PARAMETER p on vc30.relation.TERMID=p.PARTID and p.PARNAMELOC = 'CONFIG_APN'
      left join vc30.PARAMETER q on vc30.relation.TERMID=q.PARTID and q.PARNAMELOC = 'AUTOCALL'
      left join vc30.PARAMETER r on vc30.relation.TERMID=r.PARTID and r.PARNAMELOC = 'CONTACTLESS'
      left join vc30.PARAMETER s on vc30.relation.TERMID=s.PARTID and s.PARNAMELOC = 'FOROKARTA'
      left join vc30.PARAMETER t on vc30.relation.TERMID=t.PARTID and t.PARNAMELOC = 'DLL_CONN'
      left join vc30.PARAMETER u on vc30.relation.TERMID=u.PARTID and u.PARNAMELOC = 'TIP_ENABLED'
      left join vc30.PARAMETER v on vc30.relation.TERMID=v.PARTID and v.PARNAMELOC = 'EXTPINPAD'
      left join vc30.PARAMETER w on vc30.relation.TERMID=w.PARTID and w.PARNAMELOC = 'LOYALTY_PBG'
	    left join vc30.PARAMETER y on vc30.relation.TERMID=w.PARTID and y.PARNAMELOC = 'MCC'
	    left join vc30.PARAMETER z on vc30.relation.TERMID=w.PARTID and z.PARNAMELOC = 'ETH.DHCP'
	    left join vc30.PARAMETER aa on vc30.relation.TERMID=m.PARTID and m.PARNAMELOC = 'CARD01'
	    left join vc30.PARAMETER ab on vc30.relation.TERMID=m.PARTID and m.PARNAMELOC = 'CARD03'
	    left join vc30.PARAMETER ac on vc30.relation.TERMID=m.PARTID and m.PARNAMELOC = 'CARD04'
      left join vc30.TERMINFO    on vc30.relation.TERMID=termid_term
      join  (select e.*,
case (substring(e.vers_pir,9,1)) when ',' then substring(e.vers_pir,1,8) + 'P' when 'P' then (e.vers_pir) end as late_versio
 from
 (select a.evdate, a.termid, a.appnm , substring(a.appnm,charindex('PIRA', a.appnm),9) vers_pir
 from vc30.termlog a,(select  max(evdate) date_max, termid  from vc30.termlog where
 RIGHT(vc30.termlog.TERMID,1) = @lstDigiTID
and message = 'Download Successful' and status = 'S'
 group by termid) q
 where  a.evdate = q.date_max and a.termid = q.termid and appnm like '%PIRA%')e) x on vc30.relation.TERMID=x.termid
               )
where substring(cast(vc30.relation.appnm as char(10)),9,1) = ('P')
and  vc30.relation.CLUSTERID in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP','PIRAEUS')
AND RIGHT(vc30.relation.TERMID,1) = @lstDigiTID ;
