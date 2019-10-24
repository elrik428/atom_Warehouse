--// 1. Backup BANKS table
delete from abc096.banks_bup;

insert into zacreporting.abc096.banks_bup
select * from zacreporting.abc096.banks;

--// 2. Insert Bank & BINs to file 
delete from zacreporting.dbo.banknbins;

-- Keep in mind to change increment number
-- To do so run below sql stmnt and add 1 
select top 1 * from abc096.Banks a
order by a.ID desc

with trxbins as (
  select  bin binsix  from dbo.binbase#2 
    )
insert into zacreporting.dbo.banknbins
    select (cast(Rank() OVER( ORDER BY bank ) AS int)) + 176019 'rownumber' ,bank, bin  from  dbo.binbase#2 a
     group  by bank, bin;

-- XTRA 
-- In the case to refill abc096.BANKS from begin
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


--// 3. Insert banks from Temp file into  banks 

insert into zacreporting.abc096.banks
  select bankid, bankdescr  , ' ',  ' ', bankdescr
  from zacreporting.dbo.banknbins
  group by bankid, bankdescr;


--// 4. Update products file with bankid for existing BINS in both abc096, dbo tables

  update  a
  set a.bankid = b.bankid
  from zacreporting.abc096.products as a
  inner join zacreporting.dbo.banknbins b on b.binnumber = a.bin;
