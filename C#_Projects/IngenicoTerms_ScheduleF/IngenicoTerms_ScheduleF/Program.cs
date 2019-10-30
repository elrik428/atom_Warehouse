using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;// ODP.NET Oracle managed provider
using Oracle.ManagedDataAccess.Types;
using SMTPClass;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace IngenicoTerms_ScheduleF
{
    class Program
    {

        public static string OutputDirectory = @"C:\Users\lnestoras\Desktop\";
        //////public static string timestamp = DateTime.Now.ToShortDateString();
        public static string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        public static string ErrorFile = "";
        public static int ErrorCount = 0;
        public static OracleConnection SHARPconn = null;
        //public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "ppirdf0p_NewKernel_" + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
        public static string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + OutputDirectory + "ppirdf0p_NewKernel_" + timestamp + ".xls;Extended Properties=Excel 8.0;";
        public static string connectionString2 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "Ingenico_LastCall_" + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
        public static string connectionString3 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "Ingenico_MinMaxDate_" + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";

        public static string csvExport = OutputDirectory + "TidsToFile.txt";
                
        public static string mailBody_1, mailBody, subjectMail;
        public static string emailTo_1, emailTo, emailCC, emailBCC;
        public static string Path;

        public static string  connstrZACrep = "Provider=SQLOLEDB;Data Source=grat1-dev-ap2t;Initial Catalog=ZacReporting;Integrated Security=SSPI";
        
        
        static void Main(string[] args)
        {
            bool exit = false;
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            Path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Path = Path.Substring(0, Path.LastIndexOf('\\') + 1);
            //emailTo = "GR-POS-Developers-Dl@euronetworldwide.com";
            emailTo_1 = "lnestoras@euronetworldwide.com";
            emailTo = "lnestoras@euronetworldwide.com;";
            emailCC = "lnestoras@euronetworldwide.com;";
            //subjectMail = "ΠΡΟΣΦΟΡΑ: Urgent αρχείο για τα TID από τα TMS Πειραιώς και Εuronet (epos) ";

            
            ConnectSHARPdb();
            if (SHARPconn.State != ConnectionState.Open)
            {
                Console.WriteLine("ERROR: Unable to connect to SHARP.");
                return;
            }


            OleDbConnection connZACRep = new OleDbConnection(connstrZACrep);
            try
            {
                connZACRep.Open();
            }
            catch
            {
                Console.WriteLine("Cannot connect to OLEDB " + connstrZACrep);
                exit = true;
            }

            if (exit == true)
                return;


            int TotalPir = 0;
            int TotalPirRem = 0;

            // Total Piraeus Terminals
            OracleCommand sqlTotPi = new OracleCommand(" select count (*) FROM TERMINAL ", SHARPconn);
            TotalPir = Convert.ToInt32(sqlTotPi.ExecuteScalar());

            // Total remaining terminals with no latest fix
            OracleCommand sqlTotRe = new OracleCommand(" select count(*)			" +
                                                       " FROM TERMINAL    " +
                                                       " where (TER_SUCC_FILE_DL is null or  " +
                                                       " TO_CHAR(TER_SUCC_FILE_DL, 'YYYY/MM/DD')<='2018/06/26') " +
                                                       " and ter_tid not in (select tsa_terminal from term_stat_act where tsa_version like 'PR5.1.%.0021' and tsa_version>='PR5.1.10.0021')", SHARPconn);
            TotalPirRem = Convert.ToInt32(sqlTotRe.ExecuteScalar());

            // Clear temp file
            OracleCommand sqlDltQry = new OracleCommand("TRUNCATE TABLE HOSTBASE_TEMP", SHARPconn);
            sqlDltQry.ExecuteNonQuery();
            sqlDltQry.Dispose();

            OracleCommand sqlCursorFill = new OracleCommand(
" DECLARE																																												  " +
" Modems number(6,0) := 1;                                                                                                                                                                " +
" FromTM char(5) := '23:00';                                                                                                                                                              " +
" ToTM char(5) := '07:00';                                                                                                                                                                " +
" DaysTM number(2,0);                                                                                                                                                                     " +
" Slice number(3,0) := 1;                                                                                                                                                                 " +
" ElapsedTime number(2,0) := 1;                                                                                                                                                           " +
"  terminals numeric(6,0);                                                                                                                                                                " +
" WinSlice  int;                                                                                                                                                                          " +
" Minutes int;                                                                                                                                                                            " +
" TerminalsPerDay  numeric(6,0);                                                                                                                                                          " +
" TmpBaseTime char(4);                                                                                                                                                                    " +
" TmpBaseTime_1 char(2);                                                                                                                                                                  " +
" TmpBaseTime_3 char(2);                                                                                                                                                                  " +
" TmpBaseTime_tm char(6);                                                                                                                                                                 " +
" FrmTime  number(4,0):=  cast(substr(FromTM,1,2) as number)*100 + cast(substr(FromTM,4,2) as number);                                                                                    " +
" ToTime  number(4,0) := (cast(substr(ToTM,1,2) as number)*100) + (cast(substr(ToTM,4,2) as number));                                                                                     " +
" Modem numeric(6,0);                                                                                                                                                                     " +
" BaseTime numeric(4,0);                                                                                                                                                                  " +
" BaseTime_tm numeric(6,0);                                                                                                                                                               " +
" NewBaseTime numeric(4,0);                                                                                                                                                               " +
" StartingDate char (10) := '" + todayDate + "' ;         " +
" curdate date := to_date(StartingDate,'YYYY/MM/DD') ;                                                                                                                                    " +
" tempdatemm char(2);                                                                                                                                                                   " +
" tempdatemm2 char(2);                                                                                                                                                                    " +
" tempdateYY char(2);                                                                                                                                                                      " +
" tempdateYY_Check char(2);                                                                                                                                                                " +
" tempdateYY_CheckTmp char(2);                                                                                                                                                             " +
" tempdatedd char(2);                                                                                                                                                                      " +
" tempdatedd2 char(2);                                                                                                                                                                     " +
" tempdateHH char(2);                                                                                                                                                                      " +
" tempdateMN char(2);                                                                                                                                                                      " +
" termID terminal.TER_TID%type;                                                                                                                                                            " +
" tempdate#1 char(12);                                                                                                                                                                     " +
" tempdate#2 char(6);                                                                                                                                                                      " +
" tempdate#3 char(7);                                                                                                                                                                      " +
" tempdatCreat char(10);                                                                                                                                                                   " +
"     CURSOR  Update_Cursor                                                                                                                                                               " +
"    IS                                                                                                                                                                                   " +
"     select  distinct TER_TID                                                                                                                                                            " +
" FROM TERMINAL where TER_TID not in (select TER_TID from TERMINAL where  TER_START_FILE_DL > '26/06/18' and (TER_UNSUCC_FILE_DL < TER_START_FILE_DL or TER_UNSUCC_FILE_DL is null))  ; 	" +
" BEGIN                                                                                                                                                                                    " +
" select    count(*) into terminals                                                                                                                                                        " +
" from TERMINAL                                                                                                                                                                           " +
" where TER_TID not in                                                                                                                                                                    " +
"  (select TER_TID from TERMINAL                                                                                                                                                          " +
"   where TER_START_FILE_DL > '26/06/18' and (TER_UNSUCC_FILE_DL < TER_START_FILE_DL or TER_UNSUCC_FILE_DL is null));                                                                     " +
" if (ToTime-FrmTime) > 0                                                                                                                                                                  " +
" then                                                                                                                                                                                     " +
"   Minutes := (((ToTime - FrmTime)/100))*60 +                                                                                                                                            " +
"     mod(((ToTime-FrmTime)),100);                                                                                                                                                        " +
" else                                                                                                                                                                                     " +
"   Minutes := (((2400+(ToTime-FrmTime))/100))*60+                                                                                                                                        " +
"     mod((2400+(ToTime-FrmTime)),100);                                                                                                                                                   " +
" end if;                                                                                                                                                                                  " +
" TerminalsPerDay := (Minutes/ElapsedTime * Modems);                                                                                                                                       " +
" if (Modems = 0) or (TerminalsPerDay = 0)                                                                                                                                                 " +
" then                                                                                                                                                                                     " +
" Winslice := 0;                                                                                                                                                                           " +
" else                                                                                                                                                                                     " +
" WinSlice := (Minutes/(TerminalsPerDay/Modems));                                                                                                                                          " +
" end if;                                                                                                                                                                                  " +
" BaseTime:= FrmTime;                                                                                                                                                                      " +
" Modem := 0;                                                                                                                                                                              " +
" curdate := to_date(StartingDate,'YYYY/MM/DD') ;                                                                                                                                          " +
" tempdateYY_Check := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2);                                                                                                        " +
" OPEN Update_Cursor;                                                                                                                                                                      " +
" LOOP                                                                                                                                                                                    " +
"  FETCH Update_Cursor INTO termID;                                                                                                                                                       " +
"  EXIT WHEN Update_Cursor%NOTFOUND;                                                                                                                                                      " +
"   Modem := Modem +1;                                                                                                                                                                    " +
"   BaseTime_tm := BaseTime + 10000;                                                                                                                                                      " +
"   TmpBaseTime_tm := to_char(BaseTime_tm);                                                                                                                                               " +
"   TmpBaseTime := substr(TmpBaseTime_tm,2,4);                                                                                                                                            " +
"    tempdateYY := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2);                                                                                                          " +
"    tempdatemm :=substr('0'||(to_char(extract(MONTH from curdate))),-2,2);                                                                                                               " +
"    tempdateDD :=substr('0' ||(to_char(extract(DAY from curdate))),-2,2);                                                                                                                " +
"    tempdateHH := substr(('0'+rtrim(to_char(to_number(FrmTime/100)))),-2,2);                                                                                                             " +
"    tempdateMN := substr((to_char(mod(((to_number(FrmTime))),100))),-2,2);                                                                                                               " +
"    tempdate#1 := tempdateYY||tempdatemm||tempdateDD||TmpBaseTime || '00';                                                                                                               " +
"    tempdate#2 := tempdateYY||tempdatemm||tempdateDD;                                                                                                                                    " +
"    tempdate#3 := '1' || tempdateYY||tempdatemm||tempdateDD;                                                                                                                             " +
"    INSERT INTO HOSTBASE_TEMP  (F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15)                                                                                                      " +
"    VALUES ('LIP', termID , '*APPL',tempdate#1, '02', '2119894412FFFFFFFFFFFFFF',tempdate#2,'220000', 'N', '0','0', tempdate#3,'220000', 'TIGANIS', 'QPADEV000P');                       " +
"     IF Modem = Modems                                                                                                                                                                   " +
"     then                                                                                                                                                                                " +
"     BaseTime := Basetime + Winslice;                                                                                                                                                    " +
"     Modem := 0;                                                                                                                                                                         " +
"     if basetime = 2360                                                                                                                                                                  " +
"     then                                                                                                                                                                                " +
"     BaseTime := 0;                                                                                                                                                                      " +
"     basetime := BaseTime/2401 + mod((BaseTime),2401)  ;                                                                                                                                 " +
"     curdate := curdate + 1;                                                                                                                                                             " +
"     tempdateDD2 :=substr('0' ||(to_char(extract(DAY from curdate))),-2,2);                                                                                                              " +
"     tempdateYY_CheckTmp := substr(('0'+rtrim(to_char(extract(YEAR from curdate)))),3,2);                                                                                                " +
"      if tempdateYY_Check <> tempdateYY_CheckTmp                                                                                                                                         " +
"      then                                                                                                                                                                               " +
"      tempdatCreat :='20'|| tempdateYY_CheckTmp||'/'||'01'||'/'||'01';                                                                                                                   " +
"      curdate := to_date(tempdatCreat,'YYYY/MM/DD');                                                                                                                                     " +
"      tempdateYY_Check := tempdateYY_CheckTmp;                                                                                                                                           " +
"      end if;                                                                                                                                                                            " +
"     end if;                                                                                                                                                                             " +
"     if BaseTime = 60                                                                                                                                                                    " +
"     then                                                                                                                                                                                " +
"     Basetime := 100;                                                                                                                                                                    " +
"     end if;                                                                                                                                                                             " +
"      TmpBaseTime_3 := SUBSTR(TmpBaseTime,3,2);                                                                                                                                          " +
"      TmpBaseTime_1 := SUBSTR(TmpBaseTime,1,2);                                                                                                                                          " +
"      If TmpBaseTime_1 <> '23' and  TmpBaseTime_3 = '59'                                                                                                                                 " +
"      then                                                                                                                                                                               " +
"      BaseTime := (TmpBaseTime_1 * 100) + 100;                                                                                                                                           " +
"      end if;                                                                                                                                                                            " +
"     ELSE                                                                                                                                                                                " +
"      basetime := BaseTime;                                                                                                                                                              " +
"     END IF;                                                                                                                                                                             " +
"     if BaseTime = 701                                                                                                                                                                   " +
"     then                                                                                                                                                                                " +
"     Basetime := 2300;                                                                                                                                                                   " +
"     end if;                                                                                                                                                                             " +
" END LOOP;                                                                                                                                                                               " +
" CLOSE Update_Cursor;                                                                                                                                                                     " +
" END;                                                                                                                                                                                     ", SHARPconn);

            ///sqlCursorFill.Parameters.Add("StartDate", OracleDbType.TimeStamp, todayDate, ParameterDirection.Input);
            OracleDataReader rdrCursorFill = null;
            rdrCursorFill = sqlCursorFill.ExecuteReader();

            // Select data so to create host schdule excel
            string sqlHostSele = " select * FROM HOSTBASE_TEMP order by f4 " ;
            OracleCommand sqlcmdHost_Excel = new OracleCommand();
            sqlcmdHost_Excel.Connection = SHARPconn;
            //////sqlcmdSel_Excel.CommandTimeout = 0;
            sqlcmdHost_Excel.CommandType = CommandType.Text;
            sqlcmdHost_Excel.CommandText = sqlHostSele;
            OracleDataReader rdrHostToExcel = null;
            rdrHostToExcel = sqlcmdHost_Excel.ExecuteReader();

            //OleDbCommand xlCrtCmd = new OleDbCommand("CREATE TABLE [HostBaseSchedule] ([F1] char(255),[F2] char(255),[F3] char(255),[F4] char(255),[F5] char(255), " +
            //" [F6] char(255),[F7] char(255),[F8] char(255),[F9] char(255),[F10] char(255),[F11] char(255),[F12] char(255), [F13] char(255),[F14] char(255),[F15] char(255) )");
            OleDbCommand xlCrtCmd = new OleDbCommand("CREATE TABLE [HostBaseSchedule] ([F1] char(255),[F2] char(255),[F3] char(255),[F4] char(255),[F5] char(255), " +
            " [F6] char(255),[F7] char(255),[F8] int,[F9] char(255),[F10] int,[F11] int,[F12] char(255), [F13] int,[F14] char(255),[F15] char(255) )");

            
            OleDbConnection myConnection = new OleDbConnection(connectionString);
            myConnection.Open();
            xlCrtCmd.Connection = myConnection;
            xlCrtCmd.ExecuteNonQuery();

            while (rdrHostToExcel.Read())
            {
               // OleDbCommand myCommand = new OleDbCommand("Insert into [HostBaseSchedule] ([F1],[F2],[F3],[F4],[F5],[F6],[F7],[F8],[F9],[F10],[F11],[F12],[F13],[F14],[F15]) " +
               //" values ('" + rdrHostToExcel.GetValue(0).ToString() + "','" + rdrHostToExcel.GetValue(1).ToString() + "','" + rdrHostToExcel.GetValue(2).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(3).ToString().Replace('\'', ' ') + "','"
               // + rdrHostToExcel.GetValue(4).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(5).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(6).ToString() + "','" + rdrHostToExcel.GetValue(7).ToString() + "','"
               // + rdrHostToExcel.GetValue(8).ToString() + "','" + rdrHostToExcel.GetValue(9).ToString() + "','" + rdrHostToExcel.GetValue(10).ToString() + "','" + rdrHostToExcel.GetValue(11).ToString() + "','"
               // + rdrHostToExcel.GetValue(12).ToString() + "','" + rdrHostToExcel.GetValue(13).ToString() + "','" + rdrHostToExcel.GetValue(14).ToString() + "')");
                OleDbCommand myCommand = new OleDbCommand("Insert into [HostBaseSchedule] ([F1],[F2],[F3],[F4],[F5],[F6],[F7],[F8],[F9],[F10],[F11],[F12],[F13],[F14],[F15]) " +
              " values ('" + rdrHostToExcel.GetValue(0).ToString() + "','" + rdrHostToExcel.GetValue(1).ToString() + "','" + rdrHostToExcel.GetValue(2).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(3).ToString().Replace('\'', ' ') + "','"
               + rdrHostToExcel.GetValue(4).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(5).ToString().Replace('\'', ' ') + "','" + rdrHostToExcel.GetValue(6).ToString() + "','" + Convert.ToInt32(rdrHostToExcel.GetValue(7)) + "','"
               + rdrHostToExcel.GetValue(8).ToString() + "','" + Convert.ToInt32(rdrHostToExcel.GetValue(9)) + "','" + Convert.ToInt32(rdrHostToExcel.GetValue(10)) + "','" + rdrHostToExcel.GetValue(11).ToString() + "','"
               + Convert.ToInt32(rdrHostToExcel.GetValue(12)) + "','" + rdrHostToExcel.GetValue(13).ToString() + "','" + rdrHostToExcel.GetValue(14).ToString() + "')");

                myCommand.Connection = myConnection;
                myCommand.ExecuteNonQuery();

            }

            myConnection.Close();
            xlCrtCmd.Dispose();
            rdrHostToExcel.Close();
            Console.WriteLine("Schedule file Exported");


            // Select data so to create Ingenico Last Call
            string sqlHostSele2 =
                    " select TER_TID,			" +
                    "  TO_CHAR(TER_START_PARAM_DL, 'YYYY/MM/DD HH:mm:ss') START_PARAM_DL,    " +
                    "   TO_CHAR(TER_UNSUCC_PARAM_DL, 'YYYY/MM/DD HH:mm:ss') UNSUCC_PARAM_DL, " +
                    "   TO_CHAR(TER_START_FILE_DL, 'YYYY/MM/DD HH:mm:ss') START_FILE_DL,     " +
                    "   TO_CHAR(TER_UNSUCC_FILE_DL, 'YYYY/MM/DD HH:mm:ss') UNSUCC_FILE_DL,   " +
                    "  TO_CHAR(TER_SUCC_FILE_DL, 'YYYY/MM/DD HH:mm:ss') SUCC_FILE_DL,        " +
                    "   (select tsa_version from TERM_STAT_ACT where tsa_terminal = TERMINAL.ter_tid)  running_ver,             " +
                    "   (select tsa_timestamp from TERM_STAT_ACT where tsa_terminal = TERMINAL.ter_tid)  time_stamp_upload_stat " +
                    " FROM TERMINAL    " +
                    " where (TER_SUCC_FILE_DL is null or  " +
                    " TO_CHAR(TER_SUCC_FILE_DL, 'YYYY/MM/DD')<='2018/06/26') " +
                    " and ter_tid not in (select tsa_terminal from term_stat_act where tsa_version like 'PR5.1.%.0021' and tsa_version>='PR5.1.10.0021')   ";

            //string sqlHostSele2 =
            //    "	SELECT ter_tid,TER_START_FILE_DL,TER_UNSUCC_FILE_DL, TER_SUCC_FILE_DL   " +
            //    " 	,TER_START_PARAM_DL, TER_UNSUCC_PARAM_DL, TER_SUCC_PARAM_DL         " +
            //    "	FROM TERMINAL                                                           " +
            //    "	WHERE TERMINAL.TER_TID NOT IN                                           " +
            //    "  	(SELECT TERMINAL.TER_TID                                              " +
            //    "  	 FROM TERMINAL                                                        " +
            //    "  	 WHERE TERMINAL.TER_START_FILE_DL > '26/06/18'                        " +
            //    "  	AND (TERMINAL.TER_UNSUCC_FILE_DL < TERMINAL.TER_START_FILE_DL         " +
            //    "  	OR TERMINAL.TER_UNSUCC_FILE_DL  IS NULL)                              " +
            //    "   )			";
            OracleCommand sqlcmdHost_Excel2 = new OracleCommand();
            sqlcmdHost_Excel2.Connection = SHARPconn;
            sqlcmdHost_Excel2.CommandType = CommandType.Text;
            sqlcmdHost_Excel2.CommandText = sqlHostSele2;
            OracleDataReader rdrHostToExcel2 = null;
            rdrHostToExcel2 = sqlcmdHost_Excel2.ExecuteReader();

            OleDbCommand xlCrtCmd2 = new OleDbCommand("CREATE TABLE [IngenicoLastCall] ([TID] char(255),[START_PARAM_DL] char(255),[UNSUCC_PARAM_DL] char(255),[START_FILE_DL] char(255),[UNSUCC_FILE_DL] char(255), " +
               " [SUCC_FILE_DL] char(255),[running_ver] char(255),[time_stamp_upload_stat] char(255) )");

            OleDbConnection myConnection2 = new OleDbConnection(connectionString2);
            myConnection2.Open();
            xlCrtCmd2.Connection = myConnection2;
            xlCrtCmd2.ExecuteNonQuery();

            while (rdrHostToExcel2.Read())
            {
                OleDbCommand myCommand2 = new OleDbCommand("Insert into [IngenicoLastCall] ([TID] ,[START_PARAM_DL] ,[UNSUCC_PARAM_DL],[START_FILE_DL] ,[UNSUCC_FILE_DL] , [SUCC_FILE_DL] ,[running_ver] ,[time_stamp_upload_stat]   ) " +
               " values ('" + rdrHostToExcel2.GetValue(0).ToString() + "','" + rdrHostToExcel2.GetValue(1).ToString() + "','" + rdrHostToExcel2.GetValue(2).ToString() + "','" + rdrHostToExcel2.GetValue(3).ToString() + "','"
                + rdrHostToExcel2.GetValue(4).ToString() + "','" + rdrHostToExcel2.GetValue(5).ToString() + "','" + rdrHostToExcel2.GetValue(6).ToString() + "','" + rdrHostToExcel2.GetValue(7).ToString() + "')");

                myCommand2.Connection = myConnection2;
                myCommand2.ExecuteNonQuery();

            }
                        
            ////Console.Write('\r');
            myConnection2.Close();
            xlCrtCmd2.Dispose();
            rdrHostToExcel2.Close();
            Console.WriteLine("Ingenico Last Call file exported");


            // Ingenico_MinMaxDate
            // Export to txt file so import to ZACReporting/dbo.temp_tid file 
            string sqlHostSeleTxt =
                "	SELECT ter_tid  " +
                "	FROM TERMINAL                                                           " +
                "	WHERE TERMINAL.TER_TID NOT IN                                           " +
                "  	(SELECT TERMINAL.TER_TID                                              " +
                "  	 FROM TERMINAL                                                        " +
                "  	 WHERE TERMINAL.TER_START_FILE_DL > '26/06/18'                        " +
                "  	AND (TERMINAL.TER_UNSUCC_FILE_DL < TERMINAL.TER_START_FILE_DL         " +
                "  	OR TERMINAL.TER_UNSUCC_FILE_DL  IS NULL)                              " +
                "   )			";
            OracleCommand txtExp = new OracleCommand();
            txtExp.Connection = SHARPconn;
            txtExp.CommandType= CommandType.Text;
            txtExp.CommandText = sqlHostSeleTxt;
            OracleDataReader txtExpRdr = txtExp.ExecuteReader();

            //using (System.IO.StreamWriter writer = new StreamWriter(csvExport,false, Encoding.Unicode))
            using (System.IO.StreamWriter writer = new StreamWriter(csvExport, false))
            {
                while(txtExpRdr.Read())
                {
                    //writer.WriteLine(txtExpRdr["ter_tid"] + "\t");
                    writer.WriteLine(txtExpRdr["ter_tid"]);
                }

            }
            Console.WriteLine("File with TIDs exported");

            //Clear temp file
            OleDbCommand clearTmpFile = new OleDbCommand();
            clearTmpFile.Connection = connZACRep;
            clearTmpFile.CommandTimeout = 0;
            clearTmpFile.CommandText = "delete from abc096.temp_tid";
            clearTmpFile.ExecuteNonQuery();

            // Import csv to zacreporting/dbo.temp_tid file
            OleDbCommand inserTempTid = new OleDbCommand();
            inserTempTid.Connection = connZACRep;
            inserTempTid.CommandTimeout = 0;
            inserTempTid.CommandText = "BULK INSERT  [abc096].[temp_tid] " +
                                          "FROM '" + @"\\grat1-dev-ap2t\d$\TransactReports\HostSchedule_Ingenico\TidsToFile.txt" + "' " +
                                          "WITH " +
                                            "( " +
                                            " CODEPAGE = 1252, " +
                                            " FIRSTROW = 1, " +
                                            " FIELDTERMINATOR = '', " +
                                            //" ROWTERMINATOR = '\n', " +
                                            " TABLOCK " +
                                            ")";
            try
            {
                inserTempTid.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                Console.WriteLine("Error description " + ex);
            }

            // Select data so to create Ingenico_MinMaxDate
            string sqlHostSele3 =
                " select  tid, min(a.dtstamp) as Date_Min, max(a.dtstamp) as Date_Max, count(*) as Trx_Total  " +
                " from (select tid, DTSTAMP from abc096.IMP_TRANSACT_D_2018 a union select tid, DTSTAMP from abc096.IMP_TRANSACT_D_2018_B) a " +
                " where  exists(select TID from abc096.temp_tid b where a.tid = b.TID ) " +
                " group by TID ";

            OleDbCommand sqlcmdHost_Excel3 = new OleDbCommand();
            sqlcmdHost_Excel3.Connection = connZACRep;
            sqlcmdHost_Excel3.CommandTimeout = 3000;
            sqlcmdHost_Excel3.CommandType = CommandType.Text;
            sqlcmdHost_Excel3.CommandText = sqlHostSele3;
            OleDbDataReader rdrHostToExcel3 = null;
            rdrHostToExcel3 = sqlcmdHost_Excel3.ExecuteReader();

            OleDbCommand xlCrtCmd3 = new OleDbCommand("CREATE TABLE [Ingenico_MinMaxDate] ([TID] char(255), [Min_date] char(255), [Max_date] char(255), [Trx_count] char(255))");
            OleDbConnection myConnection3 = new OleDbConnection(connectionString3);
            myConnection3.Open();
            xlCrtCmd3.Connection = myConnection3;
            xlCrtCmd3.ExecuteNonQuery();

            while (rdrHostToExcel3.Read())
            {
                OleDbCommand myCommand3 = new OleDbCommand("Insert into [Ingenico_MinMaxDate] ([TID], [Min_date], [Max_date], [Trx_count]) " +
               " values ('" + rdrHostToExcel3.GetValue(0).ToString() + "','" + rdrHostToExcel3.GetValue(1).ToString() + "','" + rdrHostToExcel3.GetValue(2).ToString() + "','" + rdrHostToExcel3.GetValue(3).ToString() + "')");

                myCommand3.Connection = myConnection3;
                myCommand3.ExecuteNonQuery();

            }
            myConnection3.Close();
            xlCrtCmd3.Dispose();
            rdrHostToExcel3.Close();
            Console.WriteLine("Ingenico_MinMaxDate file exported");


            SHARPconn.Close();
            connZACRep.Close();


            string pathFolder = @"C:\Users\lnestoras\Desktop\";
            ////string[] hostBaseExcel = Directory.GetFiles(pathFolder, @"ppirdf0p_NewKernel_" + timestamp + ".xlsx", SearchOption.TopDirectoryOnly);
            string[] hostBaseExcel = Directory.GetFiles(pathFolder, "*", SearchOption.TopDirectoryOnly);

            subjectMail = "Ingenico Host scheduling terminals";
            mailBody += "Dimitri, " + "\n\r" +
                        "Below are the requested info, " + "\n\r" +
                        "Total Piraeus Bank Ingenico terminals: " + TotalPir + "\n\r" +
                        "Remaining (not updated) Piraeus Bank Ingenico terminals: " + TotalPirRem + "\n\r" +
                        "Attaching file for host base schedule. " + "\n\r" +
                        "Regards " ;

            MailClass.SendMailAttach(Path, subjectMail, mailBody, emailTo, emailCC,"",hostBaseExcel);

            // Move files to Archive
            string sourceDirectory = @"C:\Users\lnestoras\Desktop\Test\";
            string targetDirectory = @"C:\Users\lnestoras\Desktop\Test\Archive\";

            DirectoryInfo ingenicoSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo ingenicoTarget = new DirectoryInfo(targetDirectory);
            foreach (FileInfo fi in ingenicoSource.GetFiles())
            {
                //fi.CopyTo(Path.Combine(dccTarget.FullName, fi.Name), true);
                
                fi.MoveTo(targetDirectory + fi.Name);
                //Console.WriteLine("File {0} succesfully copied. ", fi.Name);
            }



        }


        static void ConnectSHARPdb()
        {

            /************CONNECT**************/
            string oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = hubp1-stm-db1p.commonpr.eeft.com)(PORT = 1521))" +
        "(CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = tmsdb.commonpr.eeft.com)));User Id=eutms2;Password=EUTMS;";//pira prod

            /************CONNECT**************/
            for (int i = 0; i < 3; i++)
            {
                try
                {

                    SHARPconn = new OracleConnection(oradb);  // C#
                    SHARPconn.Open();
                    if (SHARPconn.State == ConnectionState.Open)
                        i = 3;
                }
                catch (Exception exc)
                {



                }

            }
        }


    }
}
