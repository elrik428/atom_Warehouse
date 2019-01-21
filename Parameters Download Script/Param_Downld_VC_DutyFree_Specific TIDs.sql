SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
-- SET FMTONLY ON
-- GO
-- SET NOCOUNT ON
-- GO
-- Parameters start

DECLARE @Modems as numeric(6,0)
DECLARE @From   as char(5)
DECLARE @To    as char(5)
DECLARE @Days   as numeric(2,0)
DECLARE @Slice  as numeric(3,0)
DECLARE @ElapsedTime  as numeric(2,0)
DECLARE @StartingDate as char (10)

SET  @Modems   = 100               /*concarent connections*/
SET         @From     = '00:30'           /*start time*/
SET         @To       = '01:30'           /*end time*/
--SET      @Days     = 4
SET  @Slice    = 2                 /*@Slice should be equal to @ElapsedTime = 2*/
SET  @ElapsedTime   = 2            /*time needs to complete the parameters download in minutes*/
SET  @StartingDate = '2018-12-06'  /*starting date*/

DECLARE @terminals as numeric(6,0), @WinSlice as int, @TerminalsPerDay as numeric(6,0)
DECLARE @FrmTime as numeric(4,0), @ToTime as numeric(4,0), @Minutes as int

set @FrmTime = cast(substring(@From,1,2) as numeric(2,0))*100 +
                           cast(substring(@From,4,2) as numeric(2,0))

set @ToTime  = cast(substring(@To,1,2) as numeric(2,0))*100 +
                          cast(substring(@To,4,2) as numeric(2,0))

print 'FrmTime  ' + cast(@FrmTime as varchar (20))
print 'ToTime   ' + cast(@ToTime as varchar (20))


-- Find terminals from database (HARD-CODED !!!!)

Select @terminals = count(*)
FROM vc30.parameter
                                WHERE
   PARTID in
('73003416','73003417','73003391','73003420','73003421','73003396','73003397','73003483','73003482','73007849','73003484','73003481','73007753','73003395','73003394','73000299','73000300','73003442','73003445','73003447','73003449','73003450','73003448','73003444','73003446','73003443','73003441','73003347','73003348','73003494','73003493','73003430','73003429','73003350','73003349','73003164','73007893','73003166','73003165','73003160','73007891','73003158','73003163','73003157','73006717','73007892','73003161','73003159','73003167','73003162','73003435','73000296','73000297','73000298','73003406','73003456','73003451','73003453','73003452','73003401','73003399','73003402','73007777','73003486','73003485','73006349','73003357','73003354','73003352','73003355','73003359','73003358','73000302','73000301','73003398','73003377','73003376','73003433','73003432','73003431','73003411','73003413','73003412','73003370','73003368','73003375','73003374','73003373','73003371','73003369','73007422','73003389','73003390','73003353','73006820','73006912','73003405','73003455','73003457','73003454','73003415','73003414','73003463','73003467','73003469','73003471','73003465','73003468','73003466','73003464','73003470','73006350','73003382','73003383','73003381','73003384','73003458','73006819','73003344','73003343','73003400','73003403','73007778','73003393','73003392','73007796','73007858','73003438','73003439','73007786','73005402','73005401','73005945','73003437','73003436','73003462','73003461','73003180','73003183','73003184','73003178','73003179','73003185','73003195','73003181','73003192','73003187','73003175','73003190','73003196','73003191','73003186','73003182','73003188','73003177','73003174','73003176','73003189','73003172','73003173','73003408','73003407','73003059','73003064','73002969','73000279','73003425','73000283','73003060','73000012','73007836','73007736','73000262','73003063','73003379','73003106','73003104','73003138','73007484','73007490','73003409','73000267','73005792','73003062','73006821','73000253','73003428','73000010','73003144','73003110','73000278','73007783','73003422','73003487','73003488','73003067','73003151','73007488','73007851','73003342','73007483','73007739','73000260','73005212','73007487','73000285','73003472','73003171','73007850','73007217','73002550','73007754','73007852','73007485','73007738','73007755','73003108','73000271','73003154','73000258','73003478','73007785','73003419','73003341','73000280','73007737','73007489','73003156','73007834','73003150','73003385','73003474','73003388','73003143','73002968','73003109','73003141','73007894','73007486','73003480','73003146','73003387','73003148','73000256','73007899','73007781','73003105','73005210','73003101','73000282','73006908','73007837','73003139','73007222','73003061','73003476','73003479','73000281','73007835','73003170','73003107','73007853','73000264','73003152','73000263','73003346','73000255','73003142','73000259','73003418','73003345','73006909','73003102','73003155','73005211','73007491','73003423','73003066','73000265','73003100','73003169','73007221','73003168','73000252','73007782','73000284','73000268','73003065','73003099','73000257','73003145','73003473','73003153','73007218','73000261','73003410','73000270','73005514','73007784','73003137','73007898','73003147','73007838','73003149','73000272','73003424','73003386','73002542','73003140','73000269','73003475','73001399','73003378','73000251','73007219','73007220','73007482','73005512','73003477','73005513')
and parnameloc = 'PARAMS_DNLD'

print '@terminals  ' + cast(@terminals as varchar (20))


--============================****ANTONIS****==================================
-- Find number of served terminals per day
--set @TerminalsPerDay = floor(@terminals/@Days)
--set @TerminalsPerDay = @TerminalsPerDay + (@Modems - (cast(@TerminalsPerDay as int)%cast(@Modems as int)))


-- Find new WinSlice

if (@ToTime-@FrmTime) > 0

   set @Minutes = cast(((@ToTime - @FrmTime)/100) as decimal(2,0))*60 +
                                  cast((@ToTime-@FrmTime) as int)%100
else
    set @Minutes= cast((cast(2400+(@ToTime-@FrmTime) as int)/100) as decimal(2,0))*60+
                             cast(2400+(@ToTime-@FrmTime) as int)%100

print 'Minutes   ' + cast(@Minutes as varchar (20))

--============================****ANTONIS****=============================-=====
-- Find number of served terminals per day
set @TerminalsPerDay = floor(@Minutes /@ElapsedTime * @modems)
print '@TerminalsPerDay  ' + cast(@TerminalsPerDay as varchar (20))
--==============================================================

if (@Modems = 0 or @TerminalsPerDay = 0)
    set @WinSlice = 0
else
    set @WinSlice = ceiling(@Minutes/floor(@TerminalsPerDay/(@Modems)))

if (@WinSlice > cast(@Slice as int))
Print 'WINSLICE IS SMALL!!!'


print 'WinSlice  ' + cast(@WinSlice as varchar (20))

SET  @Days = floor(@Terminals/@TerminalsPerDay)
print 'Days  ' + cast(@Days as varchar (20))
--exec SCHEDULE_SW_UPDATE315  @Modems, @FrmTime, @Days, @WinSlice, @Elapsed, @TerminalsPerDay, @StartingDate


DECLARE @termID  as varchar(15), @modem  as numeric(6,0), @ProcessTerms as numeric(6,0), @NextDay char(1), @curdate as smalldatetime
DECLARE @FormatedDTime as char(15),  @BaseTime as numeric(4,0), @NewBaseTime as numeric(4,0), @ParameterValue as varchar(250), @BaseTime# as numeric(4,0)


SET NOCOUNT ON

SET @modem = 0
SET @ProcessTerms = 0
SET @NextDay = '0'

-- get current date
--set  @curdate =  GETDATE()
-- get the starting date
set  @curdate = @StartingDate

SET @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+
                     right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+
                                                substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+
                     right('0'+rtrim(cast(cast(@FrmTime/100 as int) as char(2))),2)+':'+
                                        right('0'+rtrim(cast(cast(@FrmTime as int)%100 as char(2))),2)



set @BaseTime = @FrmTime
set @NewBaseTime = @FrmTime

SET         @Days = floor(@Terminals/@TerminalsPerDay)

 set @BaseTime = (cast(@BaseTime/100 as int) + cast(((cast(@BaseTime as int)%100 + @WinSlice)/60) as int))*100+
                                                    cast((cast(@BaseTime as int)%100 + @WinSlice) as int)%60
 set @BaseTime# = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401

PRINT 'terminals :'  + cast(cast(@terminals as numeric(6,0)) as char)
PRINT 'Estimated window Slice (in minutes) :'  + cast(cast(@WinSlice as numeric(6,0)) as char)
PRINT 'Terminals per Day :'  + cast(cast(@TerminalsPerDay as numeric(6,0)) as char)
PRINT 'Minutes :'  + cast(cast(@Minutes as numeric(12,0)) as char)
PRINT 'FormatedDTime :'  + @FormatedDTime
PRINT 'BaseTime :'  + cast(cast(@BaseTime as numeric(12,0)) as char)
PRINT 'NewBaseTime :'  + cast(cast(@NewBaseTime as numeric(12,0)) as char)
PRINT 'modem :'  + cast(cast(@modem as numeric(6,0)) as char)
PRINT 'Modems :'  + cast(cast(@Modems as numeric(6,0)) as char)
PRINT 'DAYS :'  + cast(cast(@days as numeric(2,0)) as char)
PRINT '@BaseTime :'  + cast(@BaseTime as varchar(20))
PRINT '@BaseTime# :'  + cast(@BaseTime# as varchar(20))



------------------------------------- Update all terminals with Parameter value--------------------------------------------
DECLARE Update_Cursor  CURSOR FOR
                SELECT  partid, value
                                FROM vc30.parameter
                                WHERE
  PARTID in
('73003416','73003417','73003391','73003420','73003421','73003396','73003397','73003483','73003482','73007849','73003484','73003481','73007753','73003395','73003394','73000299','73000300','73003442','73003445','73003447','73003449','73003450','73003448','73003444','73003446','73003443','73003441','73003347','73003348','73003494','73003493','73003430','73003429','73003350','73003349','73003164','73007893','73003166','73003165','73003160','73007891','73003158','73003163','73003157','73006717','73007892','73003161','73003159','73003167','73003162','73003435','73000296','73000297','73000298','73003406','73003456','73003451','73003453','73003452','73003401','73003399','73003402','73007777','73003486','73003485','73006349','73003357','73003354','73003352','73003355','73003359','73003358','73000302','73000301','73003398','73003377','73003376','73003433','73003432','73003431','73003411','73003413','73003412','73003370','73003368','73003375','73003374','73003373','73003371','73003369','73007422','73003389','73003390','73003353','73006820','73006912','73003405','73003455','73003457','73003454','73003415','73003414','73003463','73003467','73003469','73003471','73003465','73003468','73003466','73003464','73003470','73006350','73003382','73003383','73003381','73003384','73003458','73006819','73003344','73003343','73003400','73003403','73007778','73003393','73003392','73007796','73007858','73003438','73003439','73007786','73005402','73005401','73005945','73003437','73003436','73003462','73003461','73003180','73003183','73003184','73003178','73003179','73003185','73003195','73003181','73003192','73003187','73003175','73003190','73003196','73003191','73003186','73003182','73003188','73003177','73003174','73003176','73003189','73003172','73003173','73003408','73003407','73003059','73003064','73002969','73000279','73003425','73000283','73003060','73000012','73007836','73007736','73000262','73003063','73003379','73003106','73003104','73003138','73007484','73007490','73003409','73000267','73005792','73003062','73006821','73000253','73003428','73000010','73003144','73003110','73000278','73007783','73003422','73003487','73003488','73003067','73003151','73007488','73007851','73003342','73007483','73007739','73000260','73005212','73007487','73000285','73003472','73003171','73007850','73007217','73002550','73007754','73007852','73007485','73007738','73007755','73003108','73000271','73003154','73000258','73003478','73007785','73003419','73003341','73000280','73007737','73007489','73003156','73007834','73003150','73003385','73003474','73003388','73003143','73002968','73003109','73003141','73007894','73007486','73003480','73003146','73003387','73003148','73000256','73007899','73007781','73003105','73005210','73003101','73000282','73006908','73007837','73003139','73007222','73003061','73003476','73003479','73000281','73007835','73003170','73003107','73007853','73000264','73003152','73000263','73003346','73000255','73003142','73000259','73003418','73003345','73006909','73003102','73003155','73005211','73007491','73003423','73003066','73000265','73003100','73003169','73007221','73003168','73000252','73007782','73000284','73000268','73003065','73003099','73000257','73003145','73003473','73003153','73007218','73000261','73003410','73000270','73005514','73007784','73003137','73007898','73003147','73007838','73003149','73000272','73003424','73003386','73002542','73003140','73000269','73003475','73001399','73003378','73000251','73007219','73007220','73007482','73005512','73003477','73005513')
and parnameloc = 'PARAMS_DNLD'
--          --
                ORDER BY partid

OPEN Update_Cursor FETCH NEXT FROM Update_Cursor INTO @termID, @ParameterValue

WHILE @@FETCH_STATUS = 0
BEGIN
            SET @modem = @modem + 1
            SET @ProcessTerms = @ProcessTerms + 1

-- print @modem
-- print @ProcessTerms
-- print @FormatedDTime

            UPDATE vc30.parameter  SET Value=@FormatedDTime  WHERE CURRENT OF Update_Cursor

            -- Find Base Time for terminals hiting the modems in parallel
            IF (@modem = @Modems)
            BEGIN
                set @BaseTime = (cast(@BaseTime/100 as int) + cast(((cast(@BaseTime as int)%100 + @WinSlice)/60) as int))*100+
                                                    cast((cast(@BaseTime as int)%100 + @WinSlice) as int)%60
PRINT '@BaseTime :'  + cast(@BaseTime as varchar(20))
                -- round time around the clock on a 24:00 hour basis
--============================****ANTONIS****==================================  if (@BaseTime > 2400)
                if (@BaseTime > 2359)
                BEGIN
                    set @BaseTime = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401
--============================****ANTONIS****==================================
                    if (@BaseTime = 2400)
                    begin
                        set @BaseTime = 0
                    end
--============================****ANTONIS****==================================
                    if (@NextDay = '0')
                    begin
                        set @curdate = dateadd(day,1,@curdate)
                        set @NextDay = '1'
                    end
                    -- set next date & time
                    set  @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+
                                          right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+
                                                                  substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+
                                          right('0'+rtrim(cast(cast(@BaseTime/100 as int) as char(2))),2)+':'+
                                                                         right('0'+rtrim(cast(cast(@BaseTime as int)%100 as char(2))),2)
                END
                else
                    set @BaseTime = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401

                set @NewBaseTime = @BaseTime
                set @modem = 0
            END
--============================****ANTONIS****==================================
                set @FormatedDTime = substring(@FormatedDTime,1,9)+
                                                            right('0'+rtrim(cast(cast(@NewBaseTime/100 as int) as char(2))),2)+':'+right('0'+rtrim(cast(cast(@NewBaseTime as int)%100 as char(2))),2)

                -- Next day, if all terminals for this day are proccesed.
                IF (@TerminalsPerDay = @ProcessTerms)
                BEGIN
                    -- Next day
                    if (@NextDay = '0')
                        SET @curdate = dateadd(day,1,@curdate)
                    SET @NextDay = '0'     /* reset value for next day */
                    -- reset the time to beginning time (@From) for the next day
                    SET @BaseTime  =  @FrmTime
                    SET @NewBaseTime = @BaseTime
                    SET @ProcessTerms = 0
                    SET @modem = 0
                    -- set next date & time
                    SET  @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+
                                          right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+
                                                                  substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+
                                          right('0'+rtrim(cast(cast(@FrmTime/100 as int) as char(2))),2)+':'+
                                                                         right('0'+rtrim(cast(cast(@FrmTime as int)%100 as char(2))),2)
                END

            -- read next terminal
            FETCH NEXT FROM Update_Cursor
END

CLOSE Update_Cursor
DEALLOCATE Update_Cursor

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
