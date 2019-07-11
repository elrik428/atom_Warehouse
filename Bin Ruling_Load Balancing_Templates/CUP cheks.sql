--1  cup bins in tid:2222
select  binlower from dbo.merchbins where  tid = '2222    '

--2 cup bins to net_abc allowed = Y
select binlower, count(*) from dbo.merchbins
where substring(tid,1,4) = '7300' and  
allowed = 'Y' and destport = 'NET_ABC' and binlower in (select  binlower from dbo.merchbins where  tid = '2222    ') 
group by binlower

--3 cup bins not allowed to other than NET_ABC banks
select binlower, count(*) from dbo.merchbins
where substring(tid,1,4) = '7300' and  
allowed = 'N' and destport <> 'NET_ABC' and binlower not in (select  binlower from dbo.merchbins where  tid = '2222    ') 
group by binlower
