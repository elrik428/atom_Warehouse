
UPDATE [VC30].[VC30].[PARAMETER]
SET [VALUE] = '516297     :533824     :416596     :416581     :535143     ?08'
WHERE PARNAMELOC = 'LTYBIN082'



select * from vc30.RELATION
where TERMID in (
  select distinct PARTID from vc30.PARAMETER
  where appnm like  '%EPOS%'
  and partid not in (select partid from vc30.PARAMETER where PARNAMELOC = 'LTYBIN081') and PARTID like  '73%'
  )
  and ACCCNT = -1



516297     :533824     :416596     :416581     :535143     ?08
