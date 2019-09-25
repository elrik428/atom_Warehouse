select  mid, Merchant,[Group] from abc096.MIDs
where (substring(mid,7,2) = '12'  or substring(mid,7,2) = '11'  or substring(mid,9,2) = '12' or substring(mid,7,2) = '15' ) and substring(mid,1,1) <> 'B' 
order by mid