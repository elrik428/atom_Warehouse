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

SET  @Modems   = 150               /*concarent connections*/
SET         @From     = '23:00'           /*start time*/
SET         @To       = '07:00'           /*end time*/
--SET      @Days     = 4
SET  @Slice    = 2                 /*@Slice should be equal to @ElapsedTime = 2*/
SET  @ElapsedTime   = 2            /*time needs to complete the parameters download in minutes*/
SET  @StartingDate = '2017-12-01'  /*starting date*/

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
 --           substring(appnm,1,4) = 'PIRA' and parnameloc = 'PARAMS_DNLD' and substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8')
substring(appnm,1,4) in ('PIRA', 'EPOS') and parnameloc = 'PARAMS_DNLD' and substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8')

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
             substring(appnm,1,4) in ('PIRA', 'EPOS') and parnameloc = 'PARAMS_DNLD' and substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8')
--          --
            --and partid in ('00000401','00000474','00000576','00000782','00000788','00000914','00000919','00000980','00001073','00001106','00001172','00001342','00001391','00001393','00001407','00001459','00001496','00001504','00001548','00001550','00001614','00001649','00001666','00001706','00001756','00001788','00001804','00001836','00001854','00001859','00001861','00001894','00001901','00001914','00001930','00001988','00001992','00002141','00002144','00002208','00002318','00003164','00003253','00003263','00003322','00003323','00003823','00003824','00003825','00004370','00004412','00004550','00004663','00004814','00006454','00007173','00007174','00007593','00007709','00007742','00007785','00007799','00007866','00007867','00007873','00007888','00008117','00008215','00008319','00008368','00008590','00008614','00008716','00008717','00008731','00008766','00008937','00008948','00008957','00008977','00008978','00009037','00009154','00009164','00009280','00009396','00009716','00009802','00009956','00010204','00010278','00010279','00010404','00010405','00010407','00010408','00010409','00010424','00010447','00010547','00010553','00010704','00010811','00010876','00011053','00011098','00011724','00011976','00011977','00012208','00012210','00012211','00012330','00012491','00012611','00012677','00012767','00012906','00012933','00012995','00013029','00013032','00013033','00013070','00013085','00013087','00013088','00013090','00013171','00013178','00013201','00013203','00013212','00013316','00013326','00013336','00013359','00013404','00013422','00013444','00013445','00013462','00013463','00013487','00013551','00013552','00013602','00013603','00013610','00013611','00013642','00013703','00013872','00013877','00013913','00013935','00014074','00014076','00014115','00014154','00014193','00014222','00014278','00014285','00014308','00014316','00014319','00014338','00014355')
--
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
