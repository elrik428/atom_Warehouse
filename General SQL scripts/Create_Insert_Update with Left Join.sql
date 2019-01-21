create table dbo.imp_trans##  (
codeid int, idnbr int, binnbr nvarchar(22)
)

insert into dbo.imp_trans##
select  c.tcode codeid, max(b.id) idmax, max(b.bin) binmax
from  dbo.imp_trans_tmp c
left join zacreporting.dbo.Products b on c.mask like b.bin + '%'
group  by  c.tcode

insert into dbo.imp_trans##
select  c.tcode codeid, max(b.id) idmax, max(b.bin) binmax, max(len(b.bin))
from  dbo.imp_trans_tmp c
left join zacreporting.dbo.Products b on c.mask like b.bin + '%'
group  by  c.tcode
order by c.tcode


update f
set f.ProductID = v.idmax
from zacreporting.dbo.imp_trans_tmp  as f
inner join
(select  c.tcode codeid, max(b.id) idmax, max(b.bin) binmax, max(len(b.bin)) mxlen
from  zacreporting.dbo.imp_trans_tmp c
left join zacreporting.dbo.Products b on c.mask like b.bin + '%'
group  by  c.tcode) v  on v.codeid = f.TCODE
