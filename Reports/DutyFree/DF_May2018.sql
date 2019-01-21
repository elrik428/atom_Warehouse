select  substring(mask,1,6) from abc096.IMP_TRANSACT_D_2018
where substring(mask,1,6) in ('406560','406561','406562','439539','456100','456103','456111','456112','456121','415055','415095','456158','406572','455670')
and month(dtstamp) = '5' and MID = '000000120002800'
group by  substring(mask,1,6)
