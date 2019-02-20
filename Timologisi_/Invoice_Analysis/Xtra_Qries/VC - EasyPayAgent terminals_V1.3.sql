select distinct termid as [VeriCentre TID],
--acccode as [S/N],
[S/N] =(select serialnumber from vc30.TERMINFO where utid=(select utid from vc30.TERMIDTOUTID where vc30.TERMIDTOUTID.TERMID = vc30.relation.termid and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(termid,1,1) < '5' and famnm = vc30.relation.famnm)),
[диайяитийос титкос] = (select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_1' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm ),
[диеухумсг] = (select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_2' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm ),
[покг] = (select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_3' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm ),
[тгкежымо] = (select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='RECEIPT_LINE_4' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm ),
--[Financial Merchant ID]                 =(select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MERCHID' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm),
--[Financial Terminal ID]                 =(select value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='TERMID_HOST' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm),
[Bill Payment_Merchant ID]              =(select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_MID' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm),
[Bill Payment_Terminal ID]              =(select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_TID' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm),
FAMNM as Terminal_Type,
APPNM as Application_Version,
MCC                                   =(select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='MCC' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm),
[Payment Application Availability]      =
case when
(select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='FINANCIAL' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm)='FALSE' then 'No'
else 'Yes'
end,
[Bill Payment Application Availability] =
case when
(select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_ENABLED' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm)='TRUE' then 'Yes'
else 'No'
end
from vc30.relation
where
CLUSTERID = 'PIRAEUS'
and acccnt = -1
--and acccode <> ''
and substring(appnm,1,4) = ('PIRA')
and substring(appnm,9,1) = ('P')
and substring(termid,1,1) < '5'
and (select distinct value from vc30.PARAMETER b where PARTID=termid and PARNAMELOC='SERVICES_ENABLED' and substring(appnm,1,4) = ('PIRA') and substring(appnm,9,1) = ('P') and substring(PARTID,1,1) < '5' and famnm = vc30.relation.famnm)='TRUE'
order by termid
