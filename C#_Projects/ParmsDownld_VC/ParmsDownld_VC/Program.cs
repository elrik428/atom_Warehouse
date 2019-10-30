using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Diagnostics;
using SMTPClass;


namespace ParmsDownld_VC
{
    public partial class Program
    {
        public static OleDbConnection VCConn = null;
        //public static string DBName="VeriCentre";//Test
        //public static string VCIP="Provider=SQLOLEDB;Data Source=10.1.95.32;Connect Timeout=30;Initial Catalog=VeriCentre";//Test
        public static string DBName = "vc30offline";//Production
        public static string VCIP = "Provider=SQLOLEDB;Data Source=10.1.45.144;Connect Timeout=30;Initial Catalog=vc30";//Production
                
        public static string mailBody;
        public static string emailTo, emailCC, emailBCC;
        public static string Path;

        static void Main(string[] args)
        {
            string lstDigiTID = " ";

            Path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Path = Path.Substring(0, Path.LastIndexOf('\\') + 1);
            //emailTo = "GR-POS-Developers-Dl@euronetworldwide.com";
            emailTo = "lnestoras@euronetworldwide.com";


            connectVC();
            if (VCConn.State != ConnectionState.Open)
            {
                Console.WriteLine("ERROR: Unable to connect to VeriCentre.");
                //ErrorFile += "ERROR: Unable to connect to VeriCentre.\r\n";
                //ErrorCount++;
                return;
            }

            //            Stopwatch swra = new Stopwatch();
            //            swra.Start();
            string sqlCursorQry =
"SET QUOTED_IDENTIFIER OFF																																																																																							" +
"GO                                                                                                                                                                                                     " +
"SET ANSI_NULLS OFF                                                                                                                                                                                     " +
"GO                                                                                                                                                                                                     " +
"DECLARE @Modems as numeric(6,0)                                                                                                                                                                        " +
"DECLARE @From   as char(5)                                                                                                                                                                             " +
"DECLARE @To    as char(5)                                                                                                                                                                              " +
"DECLARE @Days   as numeric(2,0)                                                                                                                                                                        " +
"DECLARE @Slice  as numeric(3,0)                                                                                                                                                                        " +
"DECLARE @ElapsedTime  as numeric(2,0)                                                                                                                                                                  " +
"DECLARE @StartingDate as char (10)                                                                                                                                                                     " +
"SET  @Modems   = 150               /*concarent connections*/                                                                                                                                           " +
"SET         @From     = '23:00'           /*start time*/                                                                                                                                               " +
"SET         @To       = '07:00'           /*end time*/                                                                                                                                                 " +
"SET  @Slice    = 2                 /*@Slice should be equal to @ElapsedTime = 2*/                                                                                                                      " +
"SET  @ElapsedTime   = 2            /*time needs to complete the parameters download in minutes*/                                                                                                       " +
"SET  @StartingDate = '2017-12-01'  /*starting date*/                                                                                                                                                   " +
"DECLARE @terminals as numeric(6,0), @WinSlice as int, @TerminalsPerDay as numeric(6,0)                                                                                                                 " +
"DECLARE @FrmTime as numeric(4,0), @ToTime as numeric(4,0), @Minutes as int                                                                                                                             " +
"set @FrmTime = cast(substring(@From,1,2) as numeric(2,0))*100 +                                                                                                                                        " +
"                           cast(substring(@From,4,2) as numeric(2,0))                                                                                                                                  " +
"set @ToTime  = cast(substring(@To,1,2) as numeric(2,0))*100 +                                                                                                                                          " +
"                          cast(substring(@To,4,2) as numeric(2,0))                                                                                                                                     " +
"Select @terminals = count(*)                                                                                                                                                                           " +
"FROM vc30offline.vc30.parameter                                                                                                                                                                                    " +
"WHERE                                                                                                                                                                                                  " +
"substring(appnm,1,4) in ('PIRA', 'EPOS') and parnameloc = 'PARAMS_DNLD' and substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8')                                                         " +
"if (@ToTime-@FrmTime) > 0                                                                                                                                                                              " +
" set @Minutes = cast(((@ToTime - @FrmTime)/100) as decimal(2,0))*60 +                                                                                                                                  " +
" cast((@ToTime-@FrmTime) as int)%100                                                                                                                                                                   " +
"else                                                                                                                                                                                                   " +
" set @Minutes= cast((cast(2400+(@ToTime-@FrmTime) as int)/100) as decimal(2,0))*60+                                                                                                                    " +
" cast(2400+(@ToTime-@FrmTime) as int)%100                                                                                                                                                              " +
"set @TerminalsPerDay = floor(@Minutes /@ElapsedTime * @modems)                                                                                                                                         " +
"if (@Modems = 0 or @TerminalsPerDay = 0)                                                                                                                                                               " +
"set @WinSlice = 0                                                                                                                                                                                      " +
"else                                                                                                                                                                                                   " +
" set @WinSlice = ceiling(@Minutes/floor(@TerminalsPerDay/(@Modems)))                                                                                                                                   " +
"if (@WinSlice > cast(@Slice as int))                                                                                                                                                                   " +
"SET  @Days = floor(@Terminals/@TerminalsPerDay)                                                                                                                                                        " +
"DECLARE @termID  as varchar(15), @modem  as numeric(6,0), @ProcessTerms as numeric(6,0), @NextDay char(1), @curdate as smalldatetime                                                                   " +
"DECLARE @FormatedDTime as char(15),  @BaseTime as numeric(4,0), @NewBaseTime as numeric(4,0), @ParameterValue as varchar(250), @BaseTime# as numeric(4,0)                                              " +
"SET NOCOUNT ON                                                                                                                                                                                         " +
"SET @modem = 0                                                                                                                                                                                         " +
"SET @ProcessTerms = 0                                                                                                                                                                                  " +
"SET @NextDay = '0'                                                                                                                                                                                     " +
"set  @curdate = @StartingDate                                                                                                                                                                          " +
"SET @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+                                                                                                                   " +
"                     right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+                                                                                                                   " +
"                                                substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+                                                                                      " +
"                     right('0'+rtrim(cast(cast(@FrmTime/100 as int) as char(2))),2)+':'+                                                                                                               " +
"                                        right('0'+rtrim(cast(cast(@FrmTime as int)%100 as char(2))),2)                                                                                                 " +
"set @BaseTime = @FrmTime                                                                                                                                                                               " +
"set @NewBaseTime = @FrmTime                                                                                                                                                                            " +
"SET         @Days = floor(@Terminals/@TerminalsPerDay)                                                                                                                                                 " +
" set @BaseTime = (cast(@BaseTime/100 as int) + cast(((cast(@BaseTime as int)%100 + @WinSlice)/60) as int))*100+                                                                                        " +
"                                                   cast((cast(@BaseTime as int)%100 + @WinSlice) as int)%60                                                                                            " +
" set @BaseTime# = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401                                                                                                                            " +
"DECLARE Update_Cursor  CURSOR FOR                                                                                                                                                                      " +
"                SELECT  partid, value                                                                                                                                                                  " +
"                                FROM vc30offline.vc30.parameter                                                                                                                                        " +
"                                WHERE                                                                                                                                                                  " +
"             substring(appnm,1,4) in ('PIRA', 'EPOS') and parnameloc = 'PARAMS_DNLD' and substring(partid,1,1) not in ('T','A','9','C','B','6','5','3','8')                                            " +
"                ORDER BY partid                                                                                                                                                                        " +
"                                                                                                                                                                                                       " +
"OPEN Update_Cursor FETCH NEXT FROM Update_Cursor INTO @termID, @ParameterValue                                                                                                                         " +
"WHILE @@FETCH_STATUS = 0                                                                                                                                                                               " +
"BEGIN                                                                                                                                                                                                  " +
"            SET @modem = @modem + 1                                                                                                                                                                    " +
"            SET @ProcessTerms = @ProcessTerms + 1                                                                                                                                                      " +
"                                                                                                                                                                                                       " +
"                                                                                                                                                                                                       " +
"            UPDATE vc30offline.vc30.parameter  SET Value=@FormatedDTime  WHERE CURRENT OF Update_Cursor                                                                                                " +
"            IF (@modem = @Modems)                                                                                                                                                                      " +
"            BEGIN                                                                                                                                                                                      " +
"                set @BaseTime = (cast(@BaseTime/100 as int) + cast(((cast(@BaseTime as int)%100 + @WinSlice)/60) as int))*100+                                                                         " +
"                                                    cast((cast(@BaseTime as int)%100 + @WinSlice) as int)%60                                                                                           " +
"                if (@BaseTime > 2359)                                                                                                                                                                  " +
"                BEGIN                                                                                                                                                                                  " +
"                    set @BaseTime = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401                                                                                                          " +
"                    if (@BaseTime = 2400)                                                                                                                                                              " +
"                    begin                                                                                                                                                                              " +
"                        set @BaseTime = 0                                                                                                                                                              " +
"                    end                                                                                                                                                                                " +
"                    if (@NextDay = '0')                                                                                                                                                                " +
"                    begin                                                                                                                                                                              " +
"                        set @curdate = dateadd(day,1,@curdate)                                                                                                                                         " +
"                        set @NextDay = '1'                                                                                                                                                             " +
"                    end                                                                                                                                                                                " +
"                    set  @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+                                                                                              " +
"                                          right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+                                                                                              " +
"                                                                  substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+                                                                    " +
"                                          right('0'+rtrim(cast(cast(@BaseTime/100 as int) as char(2))),2)+':'+                                                                                         " +
"                                                                         right('0'+rtrim(cast(cast(@BaseTime as int)%100 as char(2))),2)                                                               " +
"                END                                                                                                                                                                                    " +
"                else                                                                                                                                                                                   " +
"                    set @BaseTime = cast(@BaseTime as int)/2401 + cast(@BaseTime as int)%2401                                                                                                          " +
"                                                                                                                                                                                                       " +
"                set @NewBaseTime = @BaseTime                                                                                                                                                           " +
"                set @modem = 0                                                                                                                                                                         " +
"            END                                                                                                                                                                                        " +
"                set @FormatedDTime = substring(@FormatedDTime,1,9)+                                                                                                                                    " +
"                                                            right('0'+rtrim(cast(cast(@NewBaseTime/100 as int) as char(2))),2)+':'+right('0'+rtrim(cast(cast(@NewBaseTime as int)%100 as char(2))),2)  " +
"                IF (@TerminalsPerDay = @ProcessTerms)                                                                                                                                                  " +
"                BEGIN                                                                                                                                                                                  " +
"                    if (@NextDay = '0')                                                                                                                                                                " +
"                        SET @curdate = dateadd(day,1,@curdate)                                                                                                                                         " +
"                    SET @NextDay = '0'     /* reset value for next day */                                                                                                                              " +
"                    SET @BaseTime  =  @FrmTime                                                                                                                                                         " +
"                    SET @NewBaseTime = @BaseTime                                                                                                                                                       " +
"                    SET @ProcessTerms = 0                                                                                                                                                              " +
"                    SET @modem = 0                                                                                                                                                                     " +
"                    SET  @FormatedDTime = right('0'+rtrim(cast(datepart(dd,@curdate) as char(2))),2)+'.'+                                                                                              " +
"                                          right('0'+rtrim(cast(datepart(mm,@curdate) as char(2))),2)+'.'+                                                                                              " +
"                                                                  substring(rtrim(cast(datepart(yy,@curdate) as char(4))),3,2)+'?'+                                                                    " +
"                                          right('0'+rtrim(cast(cast(@FrmTime/100 as int) as char(2))),2)+':'+                                                                                          " +
"                                                                         right('0'+rtrim(cast(cast(@FrmTime as int)%100 as char(2))),2)                                                                " +
"                END                                                                                                                                                                                    " +
"                                                                                                                                                                                                       " +
"            FETCH NEXT FROM Update_Cursor                                                                                                                                                              " +
"END                                                                                                                                                                                                    " +
"CLOSE Update_Cursor                                                                                                                                                                                    " +
"DEALLOCATE Update_Cursor                                                                                                                                                                               " +
"GO                                                                                                                                                                                                     " +
"SET QUOTED_IDENTIFIER OFF                                                                                                                                                                              " +
"GO                                                                                                                                                                                                     " +
"SET ANSI_NULLS ON                                                                                                                                                                                      " +
"GO                                                                                                                                                                                                     ";

    
            OleDbCommand sqlcrsrQry = new OleDbCommand();
            sqlcrsrQry.Connection = VCConn;
            sqlcrsrQry.CommandTimeout = 0;
            sqlcrsrQry.CommandType = CommandType.Text;
            sqlcrsrQry.CommandText = sqlCursorQry;
            OleDbDataReader cursror_Qry = sqlcrsrQry.ExecuteReader();
            cursror_Qry.Close();
            sqlcrsrQry.Dispose();
            
            VCConn.Close();

            mailBody += "Parameters Download successfully updated";
            MailClass.SendMail(Path, "Parameters Download Monthly task", mailBody, emailTo, "", "");

        }

        static void connectVC()
        {

            /************CONNECT**************/
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    VCConn = new OleDbConnection(VCIP + ";User Id=vc30;Password=VcEn!@#20130627");//Production
                    //VCConn = new OleDbConnection(VCIP + ";User Id=VeriCentre;Password=VcLe!@#20130423");//Test
                    VCConn.Open();

                    if (VCConn.State == ConnectionState.Open)
                        i = 3;
                }
                catch (Exception exc)
                {


                }

            }
        }

    }
}
