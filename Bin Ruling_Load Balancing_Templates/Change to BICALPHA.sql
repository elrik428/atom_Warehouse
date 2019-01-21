-- 1. Merchbins
select * from merchbins
where tid in ('73002786','73002744','73002668','73002672','73002680','73005877','73001461')
and destport = 'NET_CLBICALPHA'

select * from merchbins
where tid in (select tid from merchants where mid in ('000000110000200','000000110000201','000000110000202','000000110000225','000000110000203','000000110000207','000000110000214','000000110000219','000000110000220','000000110000221','000000110000223','000000110000224','000000110000229','000000110000230','000000110000235','000000110000231','000000110000232','000000110000271','000000110000272','000000110000273','000000110000274','000000110000275','000000110000218','000000110000237','000000110000276','000000110000277','000000110000278','000000110000279','000000110000280','000000110000281','000000110000282','000000110000283','000000110000284','000000110000290','000000110000285','000000110000286','000000110000287','000000110000289','000000110000288','000000110000291','000000110000239','000000110000240','000000110000238','000000110000241','000000110000242','000000110000243','000000110000244','000000110000292','000000110000296','000000110000297','000000110000299','000000110000298','000000110000300','000000110000295') )
and destport = 'NET_CLBICALPHA'

update merchbins
set destport = 'NET_BICALPHA'
where tid in ('73002786','73002744','73002668','73002672','73002680','73005877','73001461')
and destport = 'NET_CLBICALPHA'


-- 2. Loadbalance
select * from loadbalance
where balancegroup in ('M73002786','M73002744','M73002668','M73002672','M73002680','M73005877','M73001461')
and destport = 'NET_CLBICALPHA'

select * from loadbalance
where balancegroup in (select 'M'+tid from merchants where mid in ('000000110000200','000000110000201','000000110000202','000000110000225','000000110000203','000000110000207','000000110000214','000000110000219','000000110000220','000000110000221','000000110000223','000000110000224','000000110000229','000000110000230','000000110000235','000000110000231','000000110000232','000000110000271','000000110000272','000000110000273','000000110000274','000000110000275','000000110000218','000000110000237','000000110000276','000000110000277','000000110000278','000000110000279','000000110000280','000000110000281','000000110000282','000000110000283','000000110000284','000000110000290','000000110000285','000000110000286','000000110000287','000000110000289','000000110000288','000000110000291','000000110000239','000000110000240','000000110000238','000000110000241','000000110000242','000000110000243','000000110000244','000000110000292','000000110000296','000000110000297','000000110000299','000000110000298','000000110000300','000000110000295') )
and destport = 'NET_CLBICALPHA'

update loadbalance
set destport = 'NET_BICALPHA'
where balancegroup in ('M73002786','M73002744','M73002668','M73002672','M73002680','M73005877','M73001461')
and destport = 'NET_CLBICALPHA'


 -- 3. Merchants
select * from merchants
where tid  in ('73002786','73002744','73002668','73002672','73002680','73005877','73001461')
and uploadhostid = '202'

select * from merchants
where tid in (select tid from merchants where mid in ('000000110000200','000000110000201','000000110000202','000000110000225','000000110000203','000000110000207','000000110000214','000000110000219','000000110000220','000000110000221','000000110000223','000000110000224','000000110000229','000000110000230','000000110000235','000000110000231','000000110000232','000000110000271','000000110000272','000000110000273','000000110000274','000000110000275','000000110000218','000000110000237','000000110000276','000000110000277','000000110000278','000000110000279','000000110000280','000000110000281','000000110000282','000000110000283','000000110000284','000000110000290','000000110000285','000000110000286','000000110000287','000000110000289','000000110000288','000000110000291','000000110000239','000000110000240','000000110000238','000000110000241','000000110000242','000000110000243','000000110000244','000000110000292','000000110000296','000000110000297','000000110000299','000000110000298','000000110000300','000000110000295') )
and uploadhostid = '202'

update merchants
set uploadhostid = '302'
where tid  in ('73002786','73002744','73002668','73002672','73002680','73005877','73001461')
and uploadhostid = '202'