--epos terminals
select
distinct termid as TID,
case (substring(appnm,1,4))
when 'EPOS' then 'EPOS'
when 'PIRA' then 'PIRAEUS BANK'
else '????????'
end,
--acccode as [S/N-1],
--[S/N-2] =(select serialnumber from vc30.TERMINFO where utid=(select utid from vc30.TERMIDTOUTID where vc30.TERMIDTOUTID.TERMID = vc30.relation.termid and substring(appnm,1,4) in ('PIRA','EPOS') and substring(appnm,9,1) = ('P') and substring(termid,1,1) < '5' and famnm = vc30.relation.famnm)),
--famnm as [TERMINAL TYPE],
[TERMID_NCC] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='TERMID_NCC'),
[TERMID_HOST] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='TERMID_HOST'),
[MID] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MERCHID'),
[ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_1'),
--[ΔΙΕΥΘΥΝΣΗ] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_2'),
--[ΠΟΛΗ] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_3'),
[ΤΗΛΕΦΩΝΟ] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_4')
-- [LOYALTY EUROBANK] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LOYALTY_EUROBANK') = 'TRUE' then 'Yes' else 'No' end,
-- [LOYALTY EUROBANK ELS] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LOYALTY_EUROBANK_ELS') = 'TRUE' then 'Yes' else 'No' end,
-- [LOYALTY CITIBANK] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LOYALTY_CITIBANK') = 'TRUE' then 'Yes' else 'No' end,
-- [LOYALTY ALPHA BONUS] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LOYALTY_BONUS') = 'TRUE' then 'Yes' else 'No' end
-- [LOYALTY ETHNIKI GO4MORE] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LOYALTY_NBG') = 'TRUE' then 'Yes' else 'No' end,
-- [LIFECARD] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='LIFECARD_LTY') = 'TRUE' then 'Yes' else 'No' end,
-- [ACCOUNT SELECTION NBG] = case when (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='ACCTYPE') = 'TRUE' then 'Yes' else 'No' end,
--appnm as [MASKED SW],
--[RUNNING SW] = (select max (g.appnm) from  vc30.TERMLOG as g where g.termid=vc30.relation.termid and (g.appnm like '%PIRA%' or g.appnm like '%EPOS%')and g.message in ('Download Successful','Download Aborted.')),
--[LAST DLD DATE] = (select max (g.evdate) from  vc30.TERMLOG as g where g.termid=vc30.relation.termid and (g.appnm like '%PIRA%' or g.appnm like '%EPOS%') and g.message in ('Download Successful','Download Aborted.'))
--[Media] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MEDIA'),
--[HOST_IP] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP'),
--[HOST_IP2] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP2'),
--[HOST_IP] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP_GPRS'),
--[HOST_IP2] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP_GPRS2')
--[TMS_IP] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='TMS_IP')
-- [Local ip] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='ETH.IPADDR'),
-- [Subnet mask] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='ETH.SUBNET'),
-- [Gateway] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='ETH.GATEWAY')
-- [SERVICES_MID] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_MID'),
-- [SERVICES_TID] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_TID'),
-- [SERVICES_IP] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_IP'),
-- [SERVICES_IP2] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_IP2'),
-- [SERVICES_ENABLED] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_ENABLED'),
-- [DCCCAPABLE] = (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='DCCCAPABLE')
from vc30.relation
where termid in ('520METRO','520METROCC','520METROGAS','520VERO','520VEROREC','675VERO')
order by termid
--CLUSTERID = 'EPOS_VEROPOULOS'
--and
--acccnt = -1
---- and acccode <> ''
---- substring(termid,1,2) in ('73','74')
--and substring(appnm,1,4) in ('EPOS')
--and len(appnm) = 9
--and (
--(
--(select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MEDIA') = 'ETH' and (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP') in ('195.145.098.210','195.226.126.050')
--)
--or
--(
--(select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MEDIA') = 'GPRS' and (select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='HOST_IP_GPRS') in ('195.145.098.210','195.226.126.050')
--)
--)
----and substring(appnm,1,4) in ('PIRA','EPOS')
--and substring(termid,1,1) <> ('T')


select * from vc30.term_dld_files
where termid in ('520METRO','520METROCC','520METROGAS','520VERO','520VEROREC','675VERO')
and (right(serfilenm,3) ='LGO' OR right(serfilenm,3) ='VFT')
order by termid
