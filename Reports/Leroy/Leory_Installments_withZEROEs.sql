select shop,month(DTSTAMP) as 'MONTH',count(*) as Transactions_Number,
sum(case when PROCCODE ='020000' then Tamount*(-1)
         when PROCCODE ='200000' then Tamount*(-1)
         else Tamount end) as Transactions_Amount,
(case  when Tamount=0 then '1. 0 Euro'
 when Tamount>0 and tamount<=100  then '2. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '3. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '4. 300.01-500 Euro'
		      when Tamount>500  then '5. >500 Euro'  end) as range,
inst as Installments_Number
from dbo.TRANSLOG_TRANSACT_2018, abc096.TIDs
where
dbo.TRANSLOG_TRANSACT_2018.mid='000000120002100' and
[abc096].[TIDs].TID=dbo.TRANSLOG_TRANSACT_2018.TID and
TAMOUNT>=0 and
Tresponse='00' and Shop= 'Leroy Merlin ΜΑΡΟΥΣΙ'
group by shop,month(DTSTAMP),(case  when Tamount=0 then '1. 0 Euro'
 when Tamount>0 and tamount<=100  then '2. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '3. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '4. 300.01-500 Euro'
		      when Tamount>500  then '5. >500 Euro'  end), inst
order by shop,month(DTSTAMP),(case  when Tamount=0 then '1. 0 Euro'
 when Tamount>0 and tamount<=100  then '2. 0-100 Euro'
		      when Tamount>100 and tamount<=300  then '3. 100.01-300 Euro'
		      when Tamount>300 and tamount<=500  then '4. 300.01-500 Euro'
		      when Tamount>500  then '5. >500 Euro'  end), inst