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


-- ALPHA
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460192483'  ,'460192483',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460192484'  ,'460192484',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460195552'  ,'460196009',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460196020'  ,'460196082',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460196084'  ,'460196999',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197001'  ,'460197299',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197301'  ,'460197386',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197388'  ,'460197399',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197401'  ,'460197581',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197583'  ,'460197629',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197631'  ,'460197957',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460197958'  ,'460197999',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460301001'  ,'460301467',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460301469'  ,'460301959',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460301960'  ,'460301970',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460301971'  ,'460301999',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460302001'  ,'460302015',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460302017'  ,'460302999',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460303001'  ,'460303216',0,99,0,99,'Y', 0, 999999999);
insert into merchbins (tid,destport,binlower,binupper,instmin,instmax,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) values ('1111','NET_BICALPHA','460303217'  ,'460303219',0,99,0,99,'Y', 0, 999999999);