-- Backup BANKS table

delete from abc096.banks_bup;

insert into zacreporting.abc096.banks_bup
select * from zacreporting.abc096.banks;


--// 1.  Delete and then Insert banks into Banks_new
delete from abc096.banks_new;

insert into zacreporting.abc096.banks_new
select * from zacreporting.abc096.banks;


--// 2.  Delete and then insert records of BINs,Ranking, bankdescr to temp file(zacreporting.dbo.banknbins)
delete from zacreporting.dbo.banknbins;

with trxbins as (
  select   substring(mask,1,6) binsix  from zacreporting.abc096.IMP_TRANSACT_D_monthly
  group by substring(mask,1,6)
  having count(*) > 50
  )
insert into zacreporting.dbo.banknbins
    select (cast(Rank() OVER( ORDER BY bank ) AS int)) + 41 'rownumber' ,bank, bin  from  zacreporting.dbo.binbase a
    inner join trxbins on binsix = a.bin
    where a.isocountry <> 'greece' and bank <> ' '
    group  by bank, bin;
  --order by a.isocountry     (only for select!!)


--// 3. Insert banks from Temp file into  banks_new

insert into zacreporting.abc096.banks_new
  select bankid, bankdescr  , ' ',  ' ', bankdescr
  from zacreporting.dbo.banknbins
  group by bankid, bankdescr;


--// 4. Update products file with bankid for existing BINS in both abc096, dbo tables

  update  a
  set a.bankid = b.bankid
  from zacreporting.dbo.products as a
  inner join zacreporting.dbo.banknbins b on b.binnumber = a.bin;
