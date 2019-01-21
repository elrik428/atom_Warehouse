SELECT *
FROM   TERMINAL
ORDER BY TER_TID
OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY;




------------------------------------------------------------------------------------------------------------------------------------------------------------------------
select   TER_TID, TER_DL_TIME, substr(TER_TID,6,3) from TERMINAL where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
/*csv*/
select   TER_TID from TERMINAL where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
select   count(*) from TERMINAL                                  where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
select   TER_DL_TIME, count(*) from TERMINAL                                  where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9') group by TER_DL_TIME
------------------------------------------------------------------------------------------------------------------------------------------------------------------------


------------     In progress work      (START)  Test server
--------------------------------------------------------------
DECLARE
 Modems number(6,0) := 150;
 FromTM char(5) := '23:00';
 ToTM char(5) := '07:00';
 DaysTM number(2,0);
 Slice number(3,0) := 2;
 ElapsedTime number(2,0) := 2;
 StartingDate char (10);
 terminals numeric(6,0);
 WinSlice  int;
 Minutes int;
 TerminalsPerDay  numeric(6,0);
 TmpBaseTime char(4);
 TmpBaseTime_1 char(2);
 TmpBaseTime_3 char(2);
 TmpBaseTime_tm char(6);
 FrmTime  number(4,0):=  cast(substr(FromTM,1,2) as number)*100 + cast(substr(FromTM,4,2) as number);
 ToTime  number(4,0) := (cast(substr(ToTM,1,2) as number)*100) + (cast(substr(ToTM,4,2) as number));
 Modem numeric(6,0);
 BaseTime numeric(4,0);
 BaseTime_tm numeric(6,0);
 NewBaseTime numeric(4,0);

   CURSOR  Update_Cursor
    IS
      select   TER_TID from TERMINAL where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
      --and rownum <200
      order by TER_TID  FOR UPDATE;
  --CURSOR Update_Cursor RETURN TermId_Prm%ROWTYPE;
  TermId_Prm Update_Cursor%ROWTYPE;

BEGIN
select    count(*) into terminals
 from TERMINAL
 where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9');
  --DBMS_OUTPUT.put_line(FrmTime);
  --DBMS_OUTPUT.put_line(ToTime);
  --DBMS_OUTPUT.put_line(terminals);
  --DBMS_OUTPUT.put_line(Modems);


if (ToTime-FrmTime) > 0
then
   Minutes := (((ToTime - FrmTime)/100))*60 +
                                 mod(((ToTime-FrmTime)),100);
else
   Minutes := (((2400+(ToTime-FrmTime))/100))*60+
                             mod((2400+(ToTime-FrmTime)),100);
end if;

TerminalsPerDay := (Minutes/ElapsedTime * Modems);

if (Modems = 0) or (TerminalsPerDay = 0)
then
Winslice := 0;
else
WinSlice := (Minutes/(TerminalsPerDay/Modems));
end if;

--DBMS_OUTPUT.put_line(FrmTime);
--DBMS_OUTPUT.put_line(ToTime);
--DBMS_OUTPUT.put_line(Minutes);
--DBMS_OUTPUT.put_line(TerminalsPerDay);
--DBMS_OUTPUT.put_line(WinSlice);
--DBMS_OUTPUT.put_line(' ');

BaseTime:= FrmTime;
Modem := 0;

OPEN Update_Cursor;
 LOOP
  FETCH Update_Cursor INTO TermId_Prm;
  EXIT WHEN Update_Cursor%NOTFOUND;

   Modem := Modem +1;

  -- DBMS_OUTPUT.put_line(BaseTime);

   BaseTime_tm := BaseTime + 10000;
   TmpBaseTime_tm := to_char(BaseTime_tm);
   TmpBaseTime := substr(TmpBaseTime_tm,2,4);

   --DBMS_OUTPUT.put_line(TmpBaseTime);
   --DBMS_OUTPUT.PUT_LINE(TmpBaseTime_tm);
     UPDATE  TERMINAL set TER_DL_TIME = BaseTime
      WHERE CURRENT OF Update_Cursor;

     IF Modem = Modems
     then
     BaseTime := Basetime + Winslice;
     Modem := 0;

     --DBMS_OUTPUT.put_line(BaseTime);
     --DBMS_OUTPUT.put_line('Modem = Modems');
     --DBMS_OUTPUT.put_line(TmpBaseTime_1);

     if basetime = 2360
     then
     BaseTime := 0;
     basetime := BaseTime/2401 + mod((BaseTime),2401)  ;
     end if;

     if BaseTime = 60
     then
     Basetime := 100;
     end if;

      TmpBaseTime_3 := SUBSTR(TmpBaseTime,3,2);
      TmpBaseTime_1 := SUBSTR(TmpBaseTime,1,2);
      --DBMS_OUTPUT.put_line(TmpBaseTime_1);
      --DBMS_OUTPUT.put_line(TmpBaseTime_3);

      If TmpBaseTime_1 <> '23' and  TmpBaseTime_3 = '58'
      then
      BaseTime := (TmpBaseTime_1 * 100) + 100;
      end if;

     ELSE
      basetime := BaseTime;
     --DBMS_OUTPUT.put_line(BaseTime);
     END IF;

 END LOOP;
CLOSE Update_Cursor;

END;

-------------------   **********************    ------------



------------------    Production machine       --------------

DECLARE
 Modems number(6,0) := 150;
 FromTM char(5) := '23:00';
 ToTM char(5) := '07:00';
 DaysTM number(2,0);
 Slice number(3,0) := 2;
 ElapsedTime number(2,0) := 2;
 StartingDate char (10);
 terminals numeric(6,0);
 WinSlice  int;
 Minutes int;
 TerminalsPerDay  numeric(6,0);
 TmpBaseTime char(4);
 TmpBaseTime_1 char(2);
 TmpBaseTime_3 char(2);
 TmpBaseTime_tm char(6);
 FrmTime  number(4,0):=  cast(substr(FromTM,1,2) as number)*100 + cast(substr(FromTM,4,2) as number);
 ToTime  number(4,0) := (cast(substr(ToTM,1,2) as number)*100) + (cast(substr(ToTM,4,2) as number));
 Modem numeric(6,0);
 BaseTime numeric(4,0);
 BaseTime_tm numeric(6,0);
 NewBaseTime numeric(4,0);

   CURSOR  Update_Cursor
    IS
      select   TER_TID from TERMINAL  where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
      --and rownum <200
      order by TER_TID  FOR UPDATE;
  --CURSOR Update_Cursor RETURN TermId_Prm%ROWTYPE;
  TermId_Prm Update_Cursor%ROWTYPE;

BEGIN
select    count(*) into terminals
 from TERMINAL
 where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9');
  --DBMS_OUTPUT.put_line(FrmTime);
  --DBMS_OUTPUT.put_line(ToTime);
  --DBMS_OUTPUT.put_line(terminals);
  --DBMS_OUTPUT.put_line(Modems);


if (ToTime-FrmTime) > 0
then
   Minutes := (((ToTime - FrmTime)/100))*60 +
                                 mod(((ToTime-FrmTime)),100);
else
   Minutes := (((2400+(ToTime-FrmTime))/100))*60+
                             mod((2400+(ToTime-FrmTime)),100);
end if;

TerminalsPerDay := (Minutes/ElapsedTime * Modems);

if (Modems = 0) or (TerminalsPerDay = 0)
then
Winslice := 0;
else
WinSlice := (Minutes/(TerminalsPerDay/Modems));
end if;

--DBMS_OUTPUT.put_line(FrmTime);
--DBMS_OUTPUT.put_line(ToTime);
--DBMS_OUTPUT.put_line(Minutes);
--DBMS_OUTPUT.put_line(TerminalsPerDay);
--DBMS_OUTPUT.put_line(WinSlice);
--DBMS_OUTPUT.put_line(' ');

BaseTime:= FrmTime;
Modem := 0;

OPEN Update_Cursor;
 LOOP
  FETCH Update_Cursor INTO TermId_Prm;
  EXIT WHEN Update_Cursor%NOTFOUND;

   Modem := Modem +1;

  -- DBMS_OUTPUT.put_line(BaseTime);

   BaseTime_tm := BaseTime + 10000;
   TmpBaseTime_tm := to_char(BaseTime_tm);
   TmpBaseTime := substr(TmpBaseTime_tm,2,4);

   --DBMS_OUTPUT.put_line(TmpBaseTime);
   --DBMS_OUTPUT.PUT_LINE(TmpBaseTime_tm);
     UPDATE  TERMINAL  set TER_DL_TIME = BaseTime
      WHERE CURRENT OF Update_Cursor;

     IF Modem = Modems
     then
     BaseTime := Basetime + Winslice;
     Modem := 0;

     --DBMS_OUTPUT.put_line(BaseTime);
     --DBMS_OUTPUT.put_line('Modem = Modems');
     --DBMS_OUTPUT.put_line(TmpBaseTime_1);

     if basetime = 2360
     then
     BaseTime := 0;
     basetime := BaseTime/2401 + mod((BaseTime),2401)  ;
     end if;

     if BaseTime = 60
     then
     Basetime := 100;
     end if;

      TmpBaseTime_3 := SUBSTR(TmpBaseTime,3,2);
      TmpBaseTime_1 := SUBSTR(TmpBaseTime,1,2);
      --DBMS_OUTPUT.put_line(TmpBaseTime_1);
      --DBMS_OUTPUT.put_line(TmpBaseTime_3);

      If TmpBaseTime_1 <> '23' and  TmpBaseTime_3 = '58'
      then
      BaseTime := (TmpBaseTime_1 * 100) + 100;
      end if;

     ELSE
      basetime := BaseTime;
     --DBMS_OUTPUT.put_line(BaseTime);
     END IF;

 END LOOP;
CLOSE Update_Cursor;

END;

COMMIT



-----   Chek sql

select
--TER_TID,
TER_DL_TIME, COUNT(*)
--substr(TER_TID,6,3)
from TERMINAL where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
GROUP BY TER_DL_TIME
ORDER BY TER_DL_TIME;
