insert into abc096.banks(ID,BANK,DESTCOMID,BID,ApprovalsTel)
select 43, 'EUROBANK ECOMMERCE','NET_EURMOR',null,'EURMOR' from abc096.banks
where DESTCOMID = 'NET_EBLY1'