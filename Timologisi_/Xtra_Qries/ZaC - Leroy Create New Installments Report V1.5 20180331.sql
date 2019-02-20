----------------------------------------------------------------------------------------
-- V1.1 Replace file [abc096].[My_Month_TIDs_Det] with [abc096].[TIDs]                --
-- V1.0 Create New Installments Report.                                               --
----------------------------------------------------------------------------------------

--select shop,count(*) as Transactions_Number,
--sum(case when PROCCODE ='020000' then Tamount*(-1)
--        when PROCCODE ='200000' then Tamount*(-1)
--         else Tamount end) as Transactions_Amount,
----sum(case when PROCCODE ='020000' then Tamount*(-1)
----         when PROCCODE ='200000' then Tamount*(-1)
----         else 0 end) as NEgative,
----sum(case when PROCCODE ='000000' then Tamount
----         else 0 end) as Positive,
--(case when Tamount>100 and tamount<=300  then '1. 100-300 Euro'
--      when Tamount>300 and tamount<=1000 then '2. 300-1000 Euro'
--      when Tamount>1000 then '3. 1000>' end) as range,
--inst as Installments_Number
--from dbo.TRANSLOG_transact, [abc096].[My_Month_TIDs_Det]
--where
--dbo.TRANSLOG_transact.mid='000000120002100' and
--[abc096].[My_Month_TIDs_Det].MID=dbo.TRANSLOG_transact.mid and
--[abc096].[My_Month_TIDs_Det].TID=dbo.TRANSLOG_transact.Tid and
--TAMOUNT>100 and Tresponse='00'
--group by shop,(case when Tamount>100 and tamount<=300  then '1. 100-300 Euro'
--      when Tamount>300 and tamount<=1000 then '2. 300-1000 Euro'
--      when Tamount>1000 then '3. 1000>' end), inst
--order by shop,(case when Tamount>100 and tamount<=300  then '1. 100-300 Euro'
--      when Tamount>300 and tamount<=1000 then '2. 300-1000 Euro'
--     when Tamount>1000 then '3. 1000>' end), inst



select shop,count(*) as Transactions_Number,
sum(case when PROCCODE ='020000' then Tamount*(-1)
         when PROCCODE ='200000' then Tamount*(-1)
         else Tamount end) as Transactions_Amount,
(case   when Tamount>0 and tamount<=100  then '1. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '2. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '2. 300.01-500 Euro'
		      when Tamount>500  then '2. >500 Euro'  end) as range,
inst as Installments_Number
from dbo.TRANSLOG_transact, abc096.TIDs
where
dbo.TRANSLOG_transact.mid='000000120002100' and
[abc096].[TIDs].TID=dbo.TRANSLOG_transact.Tid and
TAMOUNT>=100 and
Tresponse='00'
group by shop,(case   when Tamount>0 and tamount<=100  then '1. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '2. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '2. 300.01-500 Euro'
		      when Tamount>500  then '2. >500 Euro'
end), inst
order by shop,(case   when Tamount>0 and tamount<=100  then '1. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '2. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '2. 300.01-500 Euro'
		      when Tamount>500  then '2. >500 Euro'
end), inst
