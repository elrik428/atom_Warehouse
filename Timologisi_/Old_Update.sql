update  [abc096].[IMP_TRANSACT_D_monthly]
        set productid=
          case
                  when
                  exists
                      (select * from abc096.products b
                      where
                      (substring(mask,1,6)=cast((substring(b.bin,1,6 )) as dec(22,0)) or
                      substring(mask,1,6)=cast((substring(b.binu,1,6)) as dec(22,0)) or
                      substring(mask,1,6) between cast((substring(b.bin,1,6 )) as dec(22,0)) and
                           cast((substring(b.binu,1,6)) as dec(22,0)) ) )
                  then
                     (select min(b.id) from abc096.products b
                      where
                      productid=cast((substring(b.bin,1,6 )) as dec(22,0)) or
                      productid=cast((substring(b.binu,1,6)) as dec(22,0)) or
                      productid between cast((substring(b.bin,1,6 )) as dec(22,0)) and
                          cast((substring(b.binu,1,6)) as dec(22,0)) )
                      when
                      exists
                          (select * from abc096.products b
                          where
                          (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or
                          substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or
                          substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and
                              cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)
                      then
                          (select min(b.id) from abc096.products b
                          where
                          (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or
                          substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or
                          substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and
                              cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)
                          when
                          exists
                              (select * from abc096.products b
                              where
                              (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or
                              substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or
                              substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and
                                  cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)
                          then
                              (select min(b.id) from abc096.products b
                              where
                              (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or
                              substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or
                              substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and
                                 cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)
                              when
                              exists
                                  (select * from abc096.products b
                                  where
                                  (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or
                                  substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or
                                  substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and
                                      cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)
                              then
                                  (select min(b.id) from abc096.products b
                                  where
                                  (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or
                                  substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or
                                  substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and
                                    cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)
                                  when
                                  exists
                                       (select * from abc096.products b
                                       where
                                       (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or
                                       substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or
                                       substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and
                                         cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)
                                  then
                                       (select min(b.id) from abc096.products b
                                       where
                                       (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or
                                       substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or
                                       substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and
                                          cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)
                                       when
                                       exists
                                            (select * from abc096.products b
                                            where
                                            (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or
                                            substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or
                                            substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and
                                                cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)
                                       then
                                            (select min(b.id) from abc096.products b
                                            where
                                            (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or
                                            substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or
                                            substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and
                                            cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)
                         end
--where productid = 0 
