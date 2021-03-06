-----

DECLARE
 Modems number(6,0) := 1;
 FromTM char(5) := '06:00';
 ToTM char(5) := '07:00';
 DaysTM number(2,0);
 Slice number(3,0) := 1;
 ElapsedTime number(2,0) := 1;
 --StartingDate char (10);
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
 StartingDate char (10) := '2017/12/11';
 curdate date := to_date(StartingDate,'YYYY/MM/DD') ;
 tempdatemm char(2);
tempdateYY char(2);
tempdatedd char(2);
tempdateHH char(2);
tempdateMN char(2);
termID terminal.TER_TID%type;

     CURSOR  Update_Cursor
    IS
     select distinct TER_TID
FROM TERMINAL where TER_TID not in (select TER_TID from TERMINAL where  TER_START_FILE_DL > '26/06/18' and (TER_UNSUCC_FILE_DL < TER_START_FILE_DL or TER_UNSUCC_FILE_DL is null));


BEGIN

select    count(*) into terminals
 from TERMINAL
 where TER_TID not in
  (select TER_TID from TERMINAL
   where TER_START_FILE_DL > '26/06/18' and (TER_UNSUCC_FILE_DL < TER_START_FILE_DL or TER_UNSUCC_FILE_DL is null));

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
curdate := to_date(StartingDate,'YYYY/MM/DD') ;

OPEN Update_Cursor;
 LOOP
  FETCH Update_Cursor INTO termID;
  EXIT WHEN Update_Cursor%NOTFOUND;

   Modem := Modem +1;

   BaseTime_tm := BaseTime + 10000;
   TmpBaseTime_tm := to_char(BaseTime_tm);
   TmpBaseTime := substr(TmpBaseTime_tm,2,4);

    tempdateYY := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2);
    tempdatemm := substr(('0'+rtrim(to_char(extract(MONTH from curdate)))),-2,2);
    tempdateDD := substr(('0'+rtrim(to_char(extract(DAY from curdate)))),-2,2);
    tempdateHH := substr(('0'+rtrim(to_char(to_number(FrmTime/100)))),-2,2);
    tempdateMN := substr((to_char(mod(((to_number(FrmTime))),100))),-2,2);

    DBMS_OUTPUT.put_line(tempdateYY||tempdatemm||tempdateDD||TmpBaseTime|| '00');
   --DBMS_OUTPUT.put_line(TmpBaseTime);
    DBMS_OUTPUT.PUT_LINE(BaseTime);

     IF Modem = Modems
     then
     BaseTime := Basetime + Winslice;
     Modem := 0;

     --DBMS_OUTPUT.put_line(BaseTime);
     --DBMS_OUTPUT.put_line('Modem = Modems');
     --DBMS_OUTPUT.put_line(TmpBaseTime_1);

     if basetime = 2360
     then
--     DBMS_OUTPUT.put_line('if basetime = 2360  ' ||BaseTime);
--     DBMS_OUTPUT.put_line('curdate  ' ||curdate);
     BaseTime := 0;
     basetime := BaseTime/2401 + mod((BaseTime),2401)  ;
     curdate := curdate + 1;
--     DBMS_OUTPUT.put_line('curdate AFTER  ' ||curdate);
     end if;

     if BaseTime = 60
     then
     Basetime := 100;
     end if;

--      if BaseTime = 700
--     then
--     Basetime := 2300;
--     end if;

      TmpBaseTime_3 := SUBSTR(TmpBaseTime,3,2);
      TmpBaseTime_1 := SUBSTR(TmpBaseTime,1,2);
      --DBMS_OUTPUT.put_line(TmpBaseTime_1);
      --DBMS_OUTPUT.put_line(TmpBaseTime_3);

      If TmpBaseTime_1 <> '23' and  TmpBaseTime_3 = '59'
      then
      BaseTime := (TmpBaseTime_1 * 100) + 100;
      end if;

     ELSE
      basetime := BaseTime;
     --DBMS_OUTPUT.put_line(BaseTime);
     END IF;

     if BaseTime = 701
     then
     Basetime := 2300;
     end if;

 END LOOP;
CLOSE Update_Cursor;

END;
