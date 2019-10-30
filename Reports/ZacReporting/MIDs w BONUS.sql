select * from abc096.MIDs a
where    exists (select '0'+substring(b.MID,2,20) from abc096.MIDs  b
where substring(b.MID,1,1) <> '0' and substring(b.MID,1,1) = 'B' and a.MID ='0'+substring(b.MID,2,20) and b.mid <> '000000078000000' ) 
and a.mid <> '000000078000000'