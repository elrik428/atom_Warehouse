--ATTIKA insert
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ATTICA','415511'    ,'415511'  ,0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ATTICA','432496'    ,'432496'  ,0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ATTICA','526841'    ,'526841'  ,0,99,0,99,'Y', 0, 999999999);

-- Removed from NET_ABC and updated to ATTICA
update dbo.merchbins set destport = 'NET_ATTICA' where  binlower in ('432495', '540186') and tid = '1111    ';

-- ABC insert
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ABC','41595809'    ,'41595809'      ,0,99,0,99,'Y' ,0 ,999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ABC','55848325'    ,'55848325'      ,0,99,0,99,'Y' ,0 ,999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_ABC','9300601080'    ,'9300601082'      ,0,99,0,99,'Y' ,0 ,999999999);

-- EBNK insert
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_CLBICEBNK','45503900'    , '45503900'    ,0,99,0,99,'Y'  ,0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_CLBICEBNK','45503901'    , '45503901'    ,0,99,0,99,'Y'  ,0, 999999999);


