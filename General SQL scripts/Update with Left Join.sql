update mytable t
set z = (
  with comp as (
    select b.*, 42 as computed
    from mytable t
    where bs_id = 1
  )
  select c.computed
  from  comp c
  where c.id = t.id
)


update imp_trans_tmp c
set productid = (
with tmpid as (
      select  c.tcode codeid, max(b.id) idmax, max(b.bin) binmax, max(len(b.bin))
      from  dbo.imp_trans_tmp c
      left join zacreporting.dbo.Products b on c.mask like b.bin + '%'
      group  by  c.tcode
      order by c.tcode
      )

select c.idmax
from tmpid e
where c.tcode = e.codeid
)



update dbo.merchants_test
   set merchtitle = b.[Name] , merchaddress = b.[Adress], merchname = b.[doy]
 from dbo.merchants_test a
 inner join  [ZACRPT_Test].[dbo].[lidl_ln] b on a.tid = b.tid
   where a.tid ='73004035'



transactproduct.CommandText = "update f " +
"set f.ProductID = v.idmax " +
"from zacreporting.dbo.imp_trans_tmp  as f " +
"inner join " +
	"(select  c.tcode codeid, max(b.id) idmax, max(b.bin) binmax, max(len(b.bin)) mxlen " +
      "from  zacreporting.dbo.imp_trans_tmp c " +
      "left join zacreporting.dbo.Products b on c.mask like b.bin + '%' " +
      "group  by  c.tcode) v  on v.codeid = f.TCODE "
