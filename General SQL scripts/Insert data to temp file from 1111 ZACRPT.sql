1. Run SQL and save to csv file

select destport,binlower,
case destport
when 'NET_ABC' then '1'
when 'NET_CLBICALPHA' then '202'
when 'NET_CLBICEBNK' then '205'
when 'NET_NTBN' then '6'
end
 from  dbo.merchbins
where tid = '1111    '
and destport in ( 'NET_ABC','NET_CLBICALPHA','NET_CLBICEBNK','NET_NTBN')
order by destport
--oup by destport

2. Insert csv to table  [Zacreporting].[dbo].[Merchbins_ln_1111]
