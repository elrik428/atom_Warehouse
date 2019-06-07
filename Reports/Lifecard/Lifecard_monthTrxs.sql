select a.DTSTAMP,a.mid,b.Merchant,c.MERCHADDRESS,a.TID,mask,AMOUNT from abc096.LIFECARD_2019 a
join abc096.MIDs  b on a.MID= b.MID
join (select mid, MERCHTITLE, MERCHADDRESS,TID from abc096.MERCHANTS group by MID,MERCHTITLE, MERCHADDRESS, TID ) c on a.MID= c.MID and a.TID= c.TID
where month(DTSTAMP) = '5'
group by a.DTSTAMP,a.mid,b.Merchant,c.MERCHADDRESS,a.TID,mask,AMOUNT
