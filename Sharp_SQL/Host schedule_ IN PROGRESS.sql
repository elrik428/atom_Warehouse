------------     In progress work      (START)  Test server
--------------------------------------------------------------
DECLARE
 Modems number(6,0) := 1;
 FromTM char(5) := '23:00';
 ToTM char(5) := '07:00';
 DaysTM number(2,0);
 Slice number(3,0) := 1;
 ElapsedTime number(2,0) := 1;
 StartingDate char (10) := '2017-12-11';
 terminals numeric(6,0);
 WinSlice  int;
 Minutes int;
 TerminalsPerDay  numeric(6,0);
 FrmTime  number(4,0):=  cast(SUBSTRr(FromTM,1,2) as number)*100 + cast(SUBSTRr(FromTM,4,2) as number);
 ToTime  number(4,0) := (cast(SUBSTRr(ToTM,1,2) as number)*100) + (cast(SUBSTRr(ToTM,4,2) as number));
 Modem numeric(6,0);
 BaseTime numeric(4,0);
 BaseTime_tm numeric(6,0);
 NewBaseTime numeric(4,0);
 termID char(15);
 ProcessTerms numeric(6.0) := 0;
 NextDay char(1) := '0';
 FormatedDTime char(15);
 BaseTime numeric(4,0);
 ParameterValue char(250);
 --BaseTime# numeric(4,0);
 curdate date := StartingDate ;


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

DaysTM := (terminals/TerminalsPerDay);

--DBMS_OUTPUT.put_line(FrmTime);
--DBMS_OUTPUT.put_line(ToTime);
--DBMS_OUTPUT.put_line(Minutes);
--DBMS_OUTPUT.put_line(TerminalsPerDay);
--DBMS_OUTPUT.put_line(WinSlice);
--DBMS_OUTPUT.put_line(' ');


Modem := 0;
ProcessTerms := 0;
NextDay := '0';
curdate := StartingDate;

FormatedDTime := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2) +
                 substr(('0'+rtrim(to_char(extract(MONTH from curdate)))),-2,2)+
                 substr(('0'+rtrim(to_char(extract(DAY from curdate)))),-2,2) +
                 substr(('0' +rtrim(to_char((cast(FrmTime/100) as int)))),-2,2) +
                 substr(('0'+rtrim(to_char(mod(((cast(FrmTime as int))),100)))),-2,2);

--
BaseTime:= FrmTime;
NewBaseTime := FrmTime;
DaysTM := (terminals/TerminalsPerDay);

 BaseTime := (BaseTime/100)  +  ((mod(((BaseTime)),100) + WinSlice)/60)*100+  mod(((mod(((BaseTime)),100) + WinSlice)),60);

OPEN Update_Cursor;
 LOOP
  FETCH Update_Cursor INTO TermId_Prm;
  EXIT WHEN Update_Cursor%NOTFOUND;

   Modem := Modem +1;
   ProcessTerms := ProcessTerms + 1;

   DBMS_OUTPUT.put_line(BaseTime);

   BaseTime_tm := BaseTime + 10000;

   --DBMS_OUTPUT.put_line(TmpBaseTime);
   --DBMS_OUTPUT.PUT_LINE(TmpBaseTime_tm);


     IF Modem = Modems
     then
     BaseTime := Basetime + Winslice;
     Modem := 0;

--     DBMS_OUTPUT.put_line(BaseTime);
--     DBMS_OUTPUT.put_line('Modem = Modems');

     BaseTime := (BaseTime/100)  +  ((mod(((BaseTime)),100) + WinSlice)/60)*100+  mod(((mod(((BaseTime)),100) + WinSlice)),60);

     if basetime > 2359
     then
     BaseTime := 0;
     basetime := BaseTime/2401 + mod((BaseTime),2401)  ;


     if BaseTime = 2400
     then
     Basetime := 0;
     end if;

     if NextDay= '0'
     then
     curdate := curdate + INTERVAL '1' DAY;
     NextDay := '1';
     end if;

     FormatedDTime := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),-2,2) +
                 substr(('0'+rtrim(to_char(extract(MONTH from curdate)))),-2,2)+
                 substr(('0'+rtrim(to_char(extract(DAY from curdate)))),-2,2) +
                 substr(('0'+rtrim(to_char(mod(((cast(FrmTime as int))),100)))),-2,2);


      --DBMS_OUTPUT.put_line(TmpBaseTime_1);
      --DBMS_OUTPUT.put_line(TmpBaseTime_3);

      end if;

     ELSE
      basetime := BaseTime/2401 + mod((BaseTime),2401);
      NewBaseTime := basetime;
      modem := 0;
     --DBMS_OUTPUT.put_line(BaseTime);
     END IF;

--     FormatedDTime := SUBSTR(FormatedDTime,1,6)  +		SUBSTR('0'+rtrim(cast(cast(NewBaseTime/100 as int) as char(2))),-2,2)  +
--                     SUBSTR('0'+rtrim(cast(mod(((cast(NewBaseTime as int))),100) as char(2)),-2,2)) + '00'

FormatedDTime := SUBSTR(FormatedDTime,1,6)  +
                 SUBSTR(('0'+rtrim(to_char(cast(NewBaseTime/100 as int)))),-2,2)  +
                 SUBSTR(('0'+rtrim(to_char(mod(((cast(NewBaseTime as int))),100)))),-2,2) + '00';


if  (TerminalsPerDay =  ProcessTerms)
then
    if   NextDay = '0'
      then
      curdate := curdate + INTERVAL '1' DAY;
      NextDay := '0';
      BaseTime := FrmTime;
      NewBaseTime := BaseTime;
      ProcessTerms := 0;
      modem := 0;
  FormatedDTime := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2) +
                   substr(('0'+rtrim(to_char(extract(MONTH from curdate)))),-2,2)+
                   substr(('0'+rtrim(to_char(extract(DAY from curdate)))),-2,2) +
                   substr(('0'+rtrim(to_char(mod(((cast(FrmTime as int))),100)))),-2,2);

endif;
Endif;

 END LOOP;
CLOSE Update_Cursor;

END;
COMMIT;
-------------------   **********************    ------------
