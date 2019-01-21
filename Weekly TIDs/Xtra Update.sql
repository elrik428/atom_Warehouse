update [vc30].[tmp_Tms]
set [ETH.DHCP] = case ([value]) when '1' then 'YES' when '0' then 'NO' else 'N/A' end
--select [value]
from vc30.tmp_tms
left join vc30.parameter on PARTID = tid
where parnameloc = 'ETH.DHCP';
update [vc30].[tmp_Tms]
set [Refund VISA] = case (substring([value],10,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end
from vc30.tmp_tms
join vc30.parameter on PARTID = tid
where parnameloc = 'CARD01';
update [vc30].[tmp_Tms]
set [Refund MASTERCARD] = case (substring([value],12,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end
from vc30.tmp_tms
join vc30.parameter on PARTID = tid
where parnameloc = 'CARD03';
update [vc30].[tmp_Tms]
set [Refund MAESTRO] = case (substring([value],13,1)) when '1' then 'YES' when '0' then 'NO' else 'N/A' end
from vc30.tmp_tms
join vc30.parameter on PARTID = tid
where parnameloc = 'CARD04';
update [vc30].[tmp_Tms]
set [EASYPAYPOINT] = case ([value]) when 'TRUE' then 'YES' when 'FALSE' then 'NO' else 'N/A' end
from vc30.tmp_tms
join vc30.parameter on PARTID = tid
where parnameloc = 'SERVICES_ENABLED';
update [vc30].[tmp_Tms]
set [MCC] = [value]
from vc30.tmp_tms
join vc30.parameter on PARTID = tid
where parnameloc = 'MCC';
