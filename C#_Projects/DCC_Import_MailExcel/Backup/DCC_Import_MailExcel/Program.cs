using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using Ini;
using Microsoft.SqlServer.Dts.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using SMTPClass;
using System.Drawing;




namespace DCC_Import_MailExcel
{
    class Program
    {
        class MyEventListener : DefaultEvents
        {
            public override bool OnError(DtsObject source, int errorCode, string subComponent,
              string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {
                // Add application-specific diagnostics here.
                Console.WriteLine("Error in " + source + " / " + subComponent + " : " + description);
                return false;
            }
        }

        //public static string OutputDirectory = @"C:\Users\epos_support\Desktop\";
        //public static string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + OutputDirectory + "Attika_XXXX.xls;";
        //connectionString += "Extended Properties=Excel 8.0;";
        //public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "Attika_XXXX.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";

        public static string emailTo, emailCC, emailBCC;

        public static string mailBodyAT, subjectMailAT, emailToAtt, emailCCAtt;
        public static string mailBodyDF, subjectMailDF, emailToDF, emailCCDF;
        public static string mailBodyECS, subjectMailECS,emailToECS, emailCCECS ;
        public static string mailBodyRE, subjectMailRE, emailToREG, emailCCREG;
        public static string mailBodyRV, subjectMailRV, emailToRVU, emailCCRVU;
        public static string mailBodyDF_FTP, subjectMailDF_FTP, emailToDF_FTP, emailCCDF_FTP;
        public static string mailBodyOPT,subjectMailOPT,emailTo_OPT, emailCC_OPT;

        public static string pathmail, sqlSelectAtt, sqlSelectAtt1, sqlSelectAtt2, sqlSelectReg, sqlSelectRvu;
        public static string pathIni = @"\\grat1-dev-ap2t\d$\Reporting\DCC_Import_MailExcel\DCC_Import_MailExcel\bin\x86\Debug\DCC_Import.ini";
        //public static string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        public static string timestamp = DateTime.Now.ToString("yyyyMMdd");
        public static string data_MonthCurr = DateTime.Now.Month.ToString();
        public static int ErrorCnt = 0;

        static void Main(string[] args)
        {
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //Get sqlSelect for Attica/Regency from ini file
            IniFile ini = new IniFile(pathIni);
            sqlSelectAtt1 = ini.IniReadValue("SqlScripts", "Attika_1");
            sqlSelectAtt2 = ini.IniReadValue("SqlScripts", "Attika_2");
            sqlSelectAtt = sqlSelectAtt1 + sqlSelectAtt2;
            sqlSelectReg = ini.IniReadValue("SqlScripts", "Regency");
            sqlSelectRvu = ini.IniReadValue("SqlScripts", "RVU");
            // *** ---- ***

            string OutputDirectory = @"D:\TransactReports\DCCReports\";
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "AtticaDCCYTD_" + timestamp + ".xlsx;";
            connectionString += "Extended Properties=Excel 12.0 Xml;";
            //connectionString += "Extended Properties=Excel 8.0;";

            string connectionStringDF = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "DutyFreeDCCYTD_" + timestamp + ".xlsx;";
            connectionStringDF += "Extended Properties=Excel 12.0 Xml;";
            //connectionStringDF += "Extended Properties=Excel 8.0;";

            string connectionStringECS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "DutyFreeDCCYTD_" + timestamp + "_ECS.xlsx;";
            connectionStringECS += "Extended Properties=Excel 12.0 Xml;";

            string connectionStringRE = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "RegencyDCCYTD_" + timestamp + ".xlsx;";
            connectionStringRE += "Extended Properties=Excel 12.0 Xml;";

            string connectionStringRV = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "RVUDCCYTD_" + timestamp + ".xlsx;";
            connectionStringRV += "Extended Properties=Excel 12.0 Xml;";

            string connectionStringOPT = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + "DCC_OPTIN_" + timestamp + ".xlsx;";
            connectionStringOPT += "Extended Properties=Excel 12.0 Xml;";

            string[] file_Arr = new string[3];
            string[] file_ArrDf = new string[3];
            string[] file_ArrRe = new string[3];
            string[] file_ArrRv = new string[3];
            Console.Write("Process Started.\r\n");

            string[] resultsAtti = new string[1];
            string resultsAtti_fn;

            string[] resultsDF = new string[1];
            string resultsDF_fn;

            string[] resultsRE = new string[1];
            string resultsRE_fn;

            string[] resultsRV = new string[1];
            string resultsRV_fn;

            string path_Srch = @"\\10.7.17.11\Storage\TransactReports\DCCReports\";

            try
            {
                //resultsAtti = Path.GetFileName(path_Srch);

                resultsAtti = Directory.GetFiles(path_Srch, "*ATTICA*", SearchOption.TopDirectoryOnly);
                resultsAtti_fn = Path.GetFileName(resultsAtti[0]);
                //resultsAtti = Directory.EnumerateFiles(path, "*ATTIKA*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).First();
                //Select(Path.GetFileName).First();
                file_Arr[0] = resultsAtti_fn.Substring(0, 33);
                file_Arr[1] = resultsAtti_fn.Substring(33, 4);
                file_Arr[2] = resultsAtti_fn.Substring(37, 2);
                System.IO.File.Copy(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsAtti_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\ATTICA_Monthly_Report.csv");
                System.IO.File.Delete(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsAtti_fn);
                //System.IO.File.Replace(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsAtti_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\ATTICA_Monthly_Report.csv", @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\ATTICA_Monthly_Report_old.csv");
                Console.Write("Attika file copied. \r\n");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex + "\n\r" + "Error with Attika" );
                ErrorCnt++;
            }

            try
            {
                resultsDF = Directory.GetFiles(path_Srch, "*DUTYFREE*", SearchOption.TopDirectoryOnly);
                resultsDF_fn = Path.GetFileName(resultsDF[0]);
                //resultsDF = Directory.EnumerateFiles(path, "*DUTYFREE*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).First();
                file_ArrDf[0] = resultsDF_fn.Substring(0, 35);
                file_ArrDf[1] = resultsDF_fn.Substring(35, 4);
                file_ArrDf[2] = resultsDF_fn.Substring(39, 2);
                System.IO.File.Copy(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsDF_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report.csv");
                System.IO.File.Delete(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsDF_fn);
                //System.IO.File.Replace(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsDF_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report.csv", @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report_old.csv");
                Console.Write("Duty Free file copied. \r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n\r" + "Error with Duty Free");
                ErrorCnt++;
            }

            try
            {
                resultsRE = Directory.GetFiles(path_Srch, "*REGENCY*", SearchOption.TopDirectoryOnly);
                resultsRE_fn = Path.GetFileName(resultsRE[0]);
                file_ArrRe[0] = resultsRE_fn.Substring(0, 35);
                file_ArrRe[1] = resultsRE_fn.Substring(35, 4);
                file_ArrRe[2] = resultsRE_fn.Substring(39, 2);
                System.IO.File.Copy(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsRE_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\REGENCY_Monthly_Report.csv");
                System.IO.File.Delete(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsRE_fn);
                //System.IO.File.Replace(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsDF_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report.csv", @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report_old.csv");
                Console.Write("Regency file copied. \r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n\r" + "Error with Regency");
                ErrorCnt++;
            }

            try
            {
                resultsRV = Directory.GetFiles(path_Srch, "*RVU*", SearchOption.TopDirectoryOnly);
                resultsRV_fn = Path.GetFileName(resultsRV[0]);
                file_ArrRv[0] = resultsRV_fn.Substring(0, 31);
                file_ArrRv[1] = resultsRV_fn.Substring(31, 4);
                file_ArrRv[2] = resultsRV_fn.Substring(35, 2);
                System.IO.File.Copy(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsRV_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RVU_Monthly_Report.csv");
                System.IO.File.Delete(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsRV_fn);
                //System.IO.File.Replace(@"\\10.7.17.11\Storage\TransactReports\DCCReports\" + resultsDF_fn, @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report.csv", @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report_old.csv");
                Console.Write("RVU file copied. \r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n\r" + "Error with RVU");
                ErrorCnt++;
            }

            // Check for errors so to proceed
            if (ErrorCnt == 0)
            {

                //Working OK_Checked
                string connstring = "Provider=SQLOLEDB;Data Source=grat1-dev-ap2t;Initial Catalog=ZacReporting;Integrated Security=SSPI";
                OleDbConnection transactConn = new OleDbConnection(connstring);
                transactConn.Open();

                // Insert dat to BUP files
                try
                {
                    OleDbCommand BUP_Files = new OleDbCommand();
                    BUP_Files.Connection = transactConn;
                    BUP_Files.CommandTimeout = 0;
                    BUP_Files.CommandText = " delete from dbo.AttikaW_BUP     " +
                                            " insert into dbo.AttikaW_BUP     " +
                                            " select * from dbo.AttikaW       " +
                                            " delete from dbo.DutyFreeW_BUP   " +
                                            " insert into dbo.DutyFreeW_BUP   " +
                                            " select * from dbo.DutyFreeW     " +
                                            " delete from dbo.RegencyW_BUP    " +
                                            " insert into dbo.RegencyW_BUP    " +
                                            " select * from dbo.RegencyW      " +
                                            " delete from dbo.RVUW_BUP        " +
                                            " insert into dbo.RVUW_BUP        " +
                                            " select * from dbo.RVUW		  ";

                    OleDbDataReader rdr_ForBUPfiles_Ins = BUP_Files.ExecuteReader();
                    rdr_ForBUPfiles_Ins.Close();
                    Console.Write("Data reloaded to BUP files. \r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex + "\n\r" + "Error with BUP files");                    
                }

                //OdbcConnection transactConn = new OdbcConnection();
                //transactConn.ConnectionTimeout = 60;
                //transactConn.ConnectionString = "DSN=ZacReporting";
                //transactConn.Open();

                //OleDbConnectionStringBuilder sqConTransactA = new OleDbConnectionStringBuilder();
                //sqConTransactA.DataSource = @"grat1-dev-ap2t"; //Production
                //sqConTransactA.IntegratedSecurity = true;
                //sqConTransactA.InitialCatalog = "ZacReporting";
                //sqConTransactA.ConnectTimeout = 0;
                //OleDbConnection transactConn = new OleDbConnection(sqConTransactA.ConnectionString);
                //transactConn.Open();


                string pkgLocation;
                Package pkg;
                DTSExecResult pkgResults;
                Microsoft.SqlServer.Dts.Runtime.Application app;
                MyEventListener eventListener = new MyEventListener();

                app = new Microsoft.SqlServer.Dts.Runtime.Application();
                pkgLocation = @"d:\Reporting\DTSX Packages\Import_DCC\Import_DCC\Package.dtsx";
                pkg = app.LoadPackage(pkgLocation, eventListener);
                pkgResults = pkg.Execute(null, null, eventListener, null, null);
                Console.Write("Package execution successful. \r\n");

                try
                {
                    System.IO.File.Delete(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DF_Monthly_Report.csv");
                    System.IO.File.Delete(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\ATTICA_Monthly_Report.csv");
                    System.IO.File.Delete(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\REGENCY_Monthly_Report.csv");
                    System.IO.File.Delete(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RVU_Monthly_Report.csv");
                    Console.WriteLine("Temp csv deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                // Connection for excel
                OleDbConnection xlcONN = new OleDbConnection(connectionString);
                OleDbConnection xlcONNDF = new OleDbConnection(connectionStringDF);
                OleDbConnection xlcONNECS = new OleDbConnection(connectionStringECS);
                OleDbConnection xlcONNRE = new OleDbConnection(connectionStringRE);
                OleDbConnection xlcONNRV = new OleDbConnection(connectionStringRV);
                OleDbConnection xlcONNOPT = new OleDbConnection(connectionStringOPT);


                //  - - - - - - - - - - - - - - - -    ATTIKA    - - - - - - - - - - - - - - - -
                xlcONN.Open();


                OleDbCommand YTD_Report_Attika = new OleDbCommand();
                YTD_Report_Attika.Connection = transactConn;
                YTD_Report_Attika.CommandTimeout = 0;
                YTD_Report_Attika.CommandText = "select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2), " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                                                "from attikaw a " +
                                                "where " +
                                                "( " +
                                                  sqlSelectAtt +
                                                " ) " +
                                                " group by left(datum,10) " +
                                                " order by left(datum,10) ";

                OleDbDataReader rdr_ForExcel = YTD_Report_Attika.ExecuteReader();

                OleDbCommand xlCmd_YTD_Att = new OleDbCommand("CREATE TABLE [YTD] ([Date] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                //  OleDbConnection xlcONN = new OleDbConnection(connectionString);
                // xlcONN.Open();
                xlCmd_YTD_Att.Connection = xlcONN;
                xlCmd_YTD_Att.ExecuteNonQuery();

                while (rdr_ForExcel.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel.GetValue(0).ToString() + "','" + rdr_ForExcel.GetValue(1).ToString() + "','" + rdr_ForExcel.GetValue(2).ToString() + "','" + rdr_ForExcel.GetValue(3).ToString()
                   + "','" + rdr_ForExcel.GetValue(4).ToString() + "','" + rdr_ForExcel.GetValue(5).ToString() + "','" + rdr_ForExcel.GetValue(6).ToString() + "','" + rdr_ForExcel.GetValue(7).ToString()
                   + "','" + rdr_ForExcel.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel.Close();

                OleDbCommand YTD_Report_Attika_Tot = new OleDbCommand();
                YTD_Report_Attika_Tot.Connection = transactConn;
                YTD_Report_Attika_Tot.CommandTimeout = 0;
                YTD_Report_Attika_Tot.CommandText = "select 'Totals',    " +
    "replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,  " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,        " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,      " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,  " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                "from attikaw a	" +
                "where " +
                "( " +
                    sqlSelectAtt +
                " ) ";
                OleDbDataReader rdr_ForExcel_Total = YTD_Report_Attika_Tot.ExecuteReader();

                while (rdr_ForExcel_Total.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Total.GetValue(0).ToString() + "','" + rdr_ForExcel_Total.GetValue(1).ToString() + "','" + rdr_ForExcel_Total.GetValue(2).ToString() + "','" + rdr_ForExcel_Total.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Total.GetValue(4).ToString() + "','" + rdr_ForExcel_Total.GetValue(5).ToString() + "','" + rdr_ForExcel_Total.GetValue(6).ToString() + "','" + rdr_ForExcel_Total.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Total.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Total.Close();


                OleDbCommand YTD_Report_Attika_Perce = new OleDbCommand();
                YTD_Report_Attika_Perce.Connection = transactConn;
                YTD_Report_Attika_Perce.CommandTimeout = 0;
                YTD_Report_Attika_Perce.CommandText = "select  ' ',' ' ,																																																    " +
    "convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' , " +
    "convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%'," +
               "' ',' ',                                                                                           " +
    "convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%', " +
    "convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' ' " +
               "from                                                                                                                 " +
               "(                                                                                                                    " +
               "	select                                                                                                              " +
               "	count(*) as All_trxs,                                                                                               " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,                               " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,                               " +
               "	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,                                                          " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then                                                              " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,                           " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then                                                                " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt                        " +
               "	from attikaw a                                                                                                      " +
               "	where (                                                                                                             " +
                    sqlSelectAtt +
               "	)                                                                                                                   " +
               ")q  ";
                OleDbDataReader rdr_ForExcel_Perce = YTD_Report_Attika_Perce.ExecuteReader();

                while (rdr_ForExcel_Perce.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Perce.GetValue(0).ToString() + "','" + rdr_ForExcel_Perce.GetValue(1).ToString() + "','" + rdr_ForExcel_Perce.GetValue(2).ToString() + "','" + rdr_ForExcel_Perce.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Perce.GetValue(4).ToString() + "','" + rdr_ForExcel_Perce.GetValue(5).ToString() + "','" + rdr_ForExcel_Perce.GetValue(6).ToString() + "','" + rdr_ForExcel_Perce.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Perce.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Perce.Close();


                OleDbCommand PerTid_Report_Attika = new OleDbCommand();
                PerTid_Report_Attika.Connection = transactConn;
                PerTid_Report_Attika.CommandTimeout = 0;
                PerTid_Report_Attika.CommandText = "select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency, " +
                                                    "count(*) as ALL_TRN, " +
                                                    "sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted, " +
                                                    "(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,  " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted  " +
                                                    "from attikaw a " +
                                                    "where  " +
                                                    "datum > '2019-" + file_Arr[2] + "-01 00:00:00' and " +
                                                    "( " +
                                                        sqlSelectAtt +
                                                    " ) " +
                                                    "group by  right(tid,8),left(datum,10),dcc_currency " +
                                                    "order by  right(tid,8),left(datum,10),dcc_currency ";


                rdr_ForExcel = PerTid_Report_Attika.ExecuteReader();

                OleDbCommand xlCmd_PerTid_Att = new OleDbCommand("CREATE TABLE [Per TID-Day-Currency] ([TID] char(255),[Date] char(255),[DCC CURRENCY] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_PerTid_Att.Connection = xlcONN;
                xlCmd_PerTid_Att.ExecuteNonQuery();

                while (rdr_ForExcel.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per TID-Day-Currency] ([TID],[Date],[DCC CURRENCY],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel.GetValue(0).ToString() + "','" + rdr_ForExcel.GetValue(1).ToString() + "','" + rdr_ForExcel.GetValue(2).ToString() + "','" + rdr_ForExcel.GetValue(3).ToString()
                   + "','" + rdr_ForExcel.GetValue(4).ToString() + "','" + rdr_ForExcel.GetValue(5).ToString() + "','" + rdr_ForExcel.GetValue(6).ToString() + "','" + rdr_ForExcel.GetValue(7).ToString()
                   + "','" + rdr_ForExcel.GetValue(8).ToString() + "','" + rdr_ForExcel.GetValue(9).ToString() + "','" + rdr_ForExcel.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel.Close();



                OleDbCommand details_Report_Attika = new OleDbCommand();
                details_Report_Attika.Connection = transactConn;
                details_Report_Attika.CommandTimeout = 0;
                details_Report_Attika.CommandText = "select substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) as Transaction_Date, " +
                                                    "substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) +' ' + substring(RIGHT(datum,8),1,5) As Transaction_TimeStamp, " +
                                                    "right(TID,8) as TID_, vispan as PAN, " +
                                                    "(replace((cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2))),'.',',')) as  Original_Amount, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,   " +
                                                     "(replace((case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(dcc_amount as dec(15,0))/100) as dec(15,2)) else 0 end),'.',',')) as DCC_AMOUNT, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN, " +
                                                     "left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN " +
                                                     " from attikaw a " +
                                                     " where  " +
                                                     "datum > '2019-" + file_Arr[2] + "-01 00:00:00' and " +
                                                     "( " +
                                                        sqlSelectAtt +
                                                     " ) " +
                                                     " order by left(datum,10),Datum,TID ";

                rdr_ForExcel = details_Report_Attika.ExecuteReader();

                OleDbCommand xlCmd_details_Att = new OleDbCommand("CREATE TABLE [Details] ([Transaction_Date] char(255),[Transaction_TimeStamp] char(255),[TID_] char(255) ,[PAN] char(255),[Original_Amount] char(255)," +
                    "[dcc__currency] char(255),[DCC_AMOUNT] char(255),[Eligible_YN] char(255),[DCC_Accepted_YN] char(255))");

                xlCmd_details_Att.Connection = xlcONN;
                xlCmd_details_Att.ExecuteNonQuery();

                while (rdr_ForExcel.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Details] ([Transaction_Date],[Transaction_TimeStamp],[TID_],[PAN],[Original_Amount],[dcc__currency],[DCC_AMOUNT], " +
                   " [Eligible_YN],[DCC_Accepted_YN])" +
                   "values ('" + rdr_ForExcel.GetValue(0).ToString() + "','" + rdr_ForExcel.GetValue(1).ToString() + "','" + rdr_ForExcel.GetValue(2).ToString() + "','" + rdr_ForExcel.GetValue(3).ToString()
                   + "','" + rdr_ForExcel.GetValue(4).ToString() + "','" + rdr_ForExcel.GetValue(5).ToString() + "','" + rdr_ForExcel.GetValue(6).ToString() + "','" + rdr_ForExcel.GetValue(7).ToString()
                   + "','" + rdr_ForExcel.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel.Close();




                OleDbCommand PerAreaD_report_Attika = new OleDbCommand();
                PerAreaD_report_Attika.Connection = transactConn;
                PerAreaD_report_Attika.CommandTimeout = 0;
                PerAreaD_report_Attika.CommandText = "select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,10) as Date, " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Accepted," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Not_Accepted " +
                                                      "from attikaw a, abc096.merchants b " +
                                                      "where " +
                                                      "datum > '2019-" + file_Arr[2] + "-01 00:00:00' and " +
                                                      "right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and " +
                                                      "( " +
                                                        sqlSelectAtt +
                                                      " ) " +
                                                      "group by  merchaddress,left(datum,10) " +
                                                      "order by  merchaddress,left(datum,10) ";

                rdr_ForExcel = PerAreaD_report_Attika.ExecuteReader();

                OleDbCommand xlCmd_PerAreaD_Att = new OleDbCommand("CREATE TABLE [Per Day Per Area] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255) ,[ALL_TRN] char(255),[Eligible] char(255)," +
                    "[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[ALL_AMNT] char(255),[Eligible_] char(255), [DCC_Accepted_] char(255),[DCC_Not_Accepted_] char(255))");

                xlCmd_PerAreaD_Att.Connection = xlcONN;
                xlCmd_PerAreaD_Att.ExecuteNonQuery();

                while (rdr_ForExcel.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per Day Per Area] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [ALL_AMNT],[Eligible_],[DCC_Accepted_],[DCC_Not_Accepted_])" +
                   "values ('" + rdr_ForExcel.GetValue(0).ToString() + "','" + rdr_ForExcel.GetValue(1).ToString() + "','" + rdr_ForExcel.GetValue(2).ToString() + "','" + rdr_ForExcel.GetValue(3).ToString()
                   + "','" + rdr_ForExcel.GetValue(4).ToString() + "','" + rdr_ForExcel.GetValue(5).ToString() + "','" + rdr_ForExcel.GetValue(6).ToString() + "','" + rdr_ForExcel.GetValue(7).ToString()
                   + "','" + rdr_ForExcel.GetValue(8).ToString() + "','" + rdr_ForExcel.GetValue(9).ToString() + "','" + rdr_ForExcel.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONN;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel.Close();
                xlcONN.Close();

                Console.Write("Excel for ATTICA exported.  \r\n");







                //  - - - - - - - - - - - - - - - -    DUTY FREE    - - - - - - - - - - - - - - - -
                xlcONNDF.Open();


                OleDbCommand YTD_Report_DF = new OleDbCommand();
                YTD_Report_DF.Connection = transactConn;
                YTD_Report_DF.CommandTimeout = 0;
                YTD_Report_DF.CommandText = "select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2), " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                        "from dutyfreew " +
                        "group by left(datum,10) " +
                        "order by left(datum,10) ";

                YTD_Report_DF.CommandTimeout = 500;
                OleDbDataReader rdr_ForExcelDF = YTD_Report_DF.ExecuteReader();

                OleDbCommand xlCmd_YTD_DF = new OleDbCommand("CREATE TABLE [YTD] ([Date] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                //  OleDbConnection xlcONN = new OleDbConnection(connectionString);
                // xlcONN.Open();
                xlCmd_YTD_DF.Connection = xlcONNDF;
                xlCmd_YTD_DF.ExecuteNonQuery();

                while (rdr_ForExcelDF.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelDF.GetValue(0).ToString() + "','" + rdr_ForExcelDF.GetValue(1).ToString() + "','" + rdr_ForExcelDF.GetValue(2).ToString() + "','" + rdr_ForExcelDF.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(4).ToString() + "','" + rdr_ForExcelDF.GetValue(5).ToString() + "','" + rdr_ForExcelDF.GetValue(6).ToString() + "','" + rdr_ForExcelDF.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelDF.Close();



                OleDbCommand YTD_Report_DF_Tot = new OleDbCommand();
                YTD_Report_DF_Tot.Connection = transactConn;
                YTD_Report_DF_Tot.CommandTimeout = 0;
                YTD_Report_DF_Tot.CommandText = "select 'Totals',    " +
    "replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,  " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,        " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,      " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,  " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                "from dutyfreew	";

                OleDbDataReader rdr_ForExcelDF_Total = YTD_Report_DF_Tot.ExecuteReader();

                while (rdr_ForExcelDF_Total.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelDF_Total.GetValue(0).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(1).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(2).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF_Total.GetValue(4).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(5).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(6).ToString() + "','" + rdr_ForExcelDF_Total.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF_Total.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelDF_Total.Close();



                OleDbCommand YTD_Report_DF_Perce = new OleDbCommand();
                YTD_Report_DF_Perce.Connection = transactConn;
                YTD_Report_DF_Perce.CommandTimeout = 0;
                YTD_Report_DF_Perce.CommandText = "select  ' ',' ' ,																																																    " +
    "convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' , " +
    "convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%'," +
                "' ',' ',                                                                                           " +
    "convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%', " +
    "convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' ' " +
               "from                                                                                                                 " +
               "(                                                                                                                    " +
               "	select                                                                                                              " +
               "	count(*) as All_trxs,                                                                                               " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,                               " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,                               " +
               "	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,                                                          " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then                                                              " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,                           " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then                                                                " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt                        " +
               "	from dutyfreew                                                                                                        " +
               ")q  ";
                OleDbDataReader rdr_ForExcelDF_Perce = YTD_Report_DF_Perce.ExecuteReader();

                while (rdr_ForExcelDF_Perce.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelDF_Perce.GetValue(0).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(1).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(2).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF_Perce.GetValue(4).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(5).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(6).ToString() + "','" + rdr_ForExcelDF_Perce.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF_Perce.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelDF_Perce.Close();



                OleDbCommand PerTid_Report_DF = new OleDbCommand();
                PerTid_Report_DF.Connection = transactConn;
                PerTid_Report_DF.CommandTimeout = 0;
                PerTid_Report_DF.CommandText =
                "select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency, " +
                                                    "count(*) as ALL_TRN, " +
                                                    "sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted, " +
                                                    "(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as Eligible, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,  " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted  " +
                    "from dutyfreew " +
                    "where " +
                    "datum > '2019-" + file_ArrDf[2] + "-01 00:00:00' " +
                    "group by  right(tid,8),left(datum,10),dcc_currency " +
                    "order by  right(tid,8),left(datum,10),dcc_currency ";

                rdr_ForExcelDF = PerTid_Report_DF.ExecuteReader();

                OleDbCommand xlCmd_PerTid_DF = new OleDbCommand("CREATE TABLE [Per TID-Day-Currency] ([TID] char(255),[Date] char(255),[DCC CURRENCY] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_PerTid_DF.Connection = xlcONNDF;
                xlCmd_PerTid_DF.ExecuteNonQuery();

                while (rdr_ForExcelDF.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per TID-Day-Currency] ([TID],[Date],[DCC CURRENCY],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelDF.GetValue(0).ToString() + "','" + rdr_ForExcelDF.GetValue(1).ToString() + "','" + rdr_ForExcelDF.GetValue(2).ToString() + "','" + rdr_ForExcelDF.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(4).ToString() + "','" + rdr_ForExcelDF.GetValue(5).ToString() + "','" + rdr_ForExcelDF.GetValue(6).ToString() + "','" + rdr_ForExcelDF.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(8).ToString() + "','" + rdr_ForExcelDF.GetValue(9).ToString() + "','" + rdr_ForExcelDF.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcelDF.Close();


                OleDbCommand details_Report_DF = new OleDbCommand();
                details_Report_DF.Connection = transactConn;
                details_Report_DF.CommandTimeout = 0;
                details_Report_DF.CommandText = "select substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) as Transaction_Date, " +
                                                    "substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) +' ' + substring(RIGHT(datum,8),1,5) As Transaction_TimeStamp, " +
                                                    "right(TID,8) as TID_, vispan as PAN, " +
                                                    "(replace((cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2))),'.',',')) as  Original_Amount, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,   " +
                                                     "(replace((case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(dcc_amount as dec(15,0))/100) as dec(15,2)) else 0 end),'.',',')) as DCC_AMOUNT, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN, " +
                                                     "left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN " +
                    " from dutyfreew " +
                    " where  " +
                    "datum > '2019-" + file_ArrDf[2] + "-01 00:00:00' " +
                    " order by left(datum,10),Datum,TID ";

                rdr_ForExcelDF = details_Report_DF.ExecuteReader();

                OleDbCommand xlCmd_details_DF = new OleDbCommand("CREATE TABLE [Details] ([Transaction_Date] char(255),[Transaction_TimeStamp] char(255),[TID_] char(255) ,[PAN] char(255),[Original_Amount] char(255)," +
                    "[dcc__currency] char(255),[DCC_AMOUNT] char(255),[Eligible_YN] char(255),[DCC_Accepted_YN] char(255))");

                xlCmd_details_DF.Connection = xlcONNDF;
                xlCmd_details_DF.ExecuteNonQuery();

                while (rdr_ForExcelDF.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Details] ([Transaction_Date],[Transaction_TimeStamp],[TID_],[PAN],[Original_Amount],[dcc__currency],[DCC_AMOUNT], " +
                   " [Eligible_YN],[DCC_Accepted_YN])" +
                   "values ('" + rdr_ForExcelDF.GetValue(0).ToString() + "','" + rdr_ForExcelDF.GetValue(1).ToString() + "','" + rdr_ForExcelDF.GetValue(2).ToString() + "','" + rdr_ForExcelDF.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(4).ToString() + "','" + rdr_ForExcelDF.GetValue(5).ToString() + "','" + rdr_ForExcelDF.GetValue(6).ToString() + "','" + rdr_ForExcelDF.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcelDF.Close();




                OleDbCommand PerAreaD_report_DF = new OleDbCommand();
                PerAreaD_report_DF.Connection = transactConn;
                PerAreaD_report_DF.CommandTimeout = 0;
                PerAreaD_report_DF.CommandText = "select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,10) as Date, " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Accepted," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Not_Accepted " +
                                            "from dutyfreew a, abc096.merchants b " +
                                            "where " +
                                            "datum > '2019-" + file_ArrDf[2] + "-01 00:00:00' and " +
                                            "right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC'  " +
                                            "group by  merchaddress,left(datum,10) " +
                                            "order by  merchaddress,left(datum,10)";

                rdr_ForExcelDF = PerAreaD_report_DF.ExecuteReader();

                OleDbCommand xlCmd_PerAreaD_DF = new OleDbCommand("CREATE TABLE [Per Day Per Area] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255) ,[ALL_TRN] char(255),[Eligible] char(255)," +
                    "[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[ALL_AMNT] char(255),[Eligible_] char(255), [DCC_Accepted_] char(255),[DCC_Not_Accepted_] char(255))");

                xlCmd_PerAreaD_DF.Connection = xlcONNDF;
                xlCmd_PerAreaD_DF.ExecuteNonQuery();

                while (rdr_ForExcelDF.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per Day Per Area] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [ALL_AMNT],[Eligible_],[DCC_Accepted_],[DCC_Not_Accepted_])" +
                   "values ('" + rdr_ForExcelDF.GetValue(0).ToString() + "','" + rdr_ForExcelDF.GetValue(1).ToString() + "','" + rdr_ForExcelDF.GetValue(2).ToString() + "','" + rdr_ForExcelDF.GetValue(3).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(4).ToString() + "','" + rdr_ForExcelDF.GetValue(5).ToString() + "','" + rdr_ForExcelDF.GetValue(6).ToString() + "','" + rdr_ForExcelDF.GetValue(7).ToString()
                   + "','" + rdr_ForExcelDF.GetValue(8).ToString() + "','" + rdr_ForExcelDF.GetValue(9).ToString() + "','" + rdr_ForExcelDF.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNDF;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcelDF.Close();

                xlcONNDF.Close();

                Console.Write("Excel for DutyFree exported.  \r\n");



                //  - - - - - - - - - - - - - - - -    DUTY FREE  ECS  - - - - - - - - - - - - - - - -
                xlcONNECS.Open();


                OleDbCommand YTD_Report_ECS = new OleDbCommand();
                YTD_Report_ECS.Connection = transactConn;
                YTD_Report_ECS.CommandTimeout = 0;
                YTD_Report_ECS.CommandText = "select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2), " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                        "from dutyfreew " +
                        "group by left(datum,10) " +
                        "order by left(datum,10) ";

                YTD_Report_ECS.CommandTimeout = 500;
                OleDbDataReader rdr_ForExcelECS = YTD_Report_ECS.ExecuteReader();

                OleDbCommand xlCmd_YTD_ECS = new OleDbCommand("CREATE TABLE [YTD] ([Date] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                //  OleDbConnection xlcONN = new OleDbConnection(connectionString);
                // xlcONN.Open();
                xlCmd_YTD_ECS.Connection = xlcONNECS;
                xlCmd_YTD_ECS.ExecuteNonQuery();

                while (rdr_ForExcelECS.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelECS.GetValue(0).ToString() + "','" + rdr_ForExcelECS.GetValue(1).ToString() + "','" + rdr_ForExcelECS.GetValue(2).ToString() + "','" + rdr_ForExcelECS.GetValue(3).ToString()
                   + "','" + rdr_ForExcelECS.GetValue(4).ToString() + "','" + rdr_ForExcelECS.GetValue(5).ToString() + "','" + rdr_ForExcelECS.GetValue(6).ToString() + "','" + rdr_ForExcelECS.GetValue(7).ToString()
                   + "','" + rdr_ForExcelECS.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNECS;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelECS.Close();



                OleDbCommand YTD_Report_ECS_Tot = new OleDbCommand();
                YTD_Report_ECS_Tot.Connection = transactConn;
                YTD_Report_ECS_Tot.CommandTimeout = 0;
                YTD_Report_ECS_Tot.CommandText = "select 'Totals',    " +
    "replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,  " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,        " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,      " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,  " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                "from dutyfreew	";

                OleDbDataReader rdr_ForExcelECS_Total = YTD_Report_ECS_Tot.ExecuteReader();

                while (rdr_ForExcelECS_Total.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelECS_Total.GetValue(0).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(1).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(2).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(3).ToString()
                   + "','" + rdr_ForExcelECS_Total.GetValue(4).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(5).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(6).ToString() + "','" + rdr_ForExcelECS_Total.GetValue(7).ToString()
                   + "','" + rdr_ForExcelECS_Total.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNECS;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelECS_Total.Close();



                OleDbCommand YTD_Report_ECS_Perce = new OleDbCommand();
                YTD_Report_ECS_Perce.Connection = transactConn;
                YTD_Report_ECS_Perce.CommandTimeout = 0;
                YTD_Report_ECS_Perce.CommandText = "select  ' ',' ' ,																																																    " +
    "convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' , " +
    "convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%'," +
                "' ',' ',                                                                                           " +
    "convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%', " +
    "convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' ' " +
               "from                                                                                                                 " +
               "(                                                                                                                    " +
               "	select                                                                                                              " +
               "	count(*) as All_trxs,                                                                                               " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,                               " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,                               " +
               "	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,                                                          " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then                                                              " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,                           " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then                                                                " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt                        " +
               "	from dutyfreew                                                                                                        " +
               ")q  ";
                OleDbDataReader rdr_ForExcelECS_Perce = YTD_Report_ECS_Perce.ExecuteReader();

                while (rdr_ForExcelECS_Perce.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcelECS_Perce.GetValue(0).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(1).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(2).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(3).ToString()
                   + "','" + rdr_ForExcelECS_Perce.GetValue(4).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(5).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(6).ToString() + "','" + rdr_ForExcelECS_Perce.GetValue(7).ToString()
                   + "','" + rdr_ForExcelECS_Perce.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNECS;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcelECS_Perce.Close();
                xlcONNECS.Close();
                Console.Write("Excel for DutyFree_ECS exported.  \r\n");
                
                

                //  - - - - - - - - - - - - - - - -    REGENCY    - - - - - - - - - - - - - - - -
                xlcONNRE.Open();


                OleDbCommand YTD_Report_RE = new OleDbCommand();
                YTD_Report_RE.Connection = transactConn;
                YTD_Report_RE.CommandTimeout = 0;
                YTD_Report_RE.CommandText = "select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2), " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                                                "from RegencyW a " +
                                                "where ( " +
                                                    sqlSelectReg +
                                                " )" +
                                                " group by left(datum,10) " +
                                                " order by left(datum,10) ";

                OleDbDataReader rdr_ForExcel_RE = YTD_Report_RE.ExecuteReader();

                OleDbCommand xlCmd_YTD_RE = new OleDbCommand("CREATE TABLE [YTD] ([Date] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_YTD_RE.Connection = xlcONNRE;
                xlCmd_YTD_RE.ExecuteNonQuery();

                while (rdr_ForExcel_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_RE.Close();



                OleDbCommand YTD_Report_RE_Tot = new OleDbCommand();
                YTD_Report_RE_Tot.Connection = transactConn;
                YTD_Report_RE_Tot.CommandTimeout = 0;
                YTD_Report_RE_Tot.CommandText = "select 'Totals',    " +
    "replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,  " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,        " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,      " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,  " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                "from RegencyW a	" +
                "where ( " +
                sqlSelectReg +
                " ) ";
                OleDbDataReader rdr_ForExcel_Total_RE = YTD_Report_RE_Tot.ExecuteReader();

                while (rdr_ForExcel_Total_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Total_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Total_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_Total_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Total_RE.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Total_RE.Close();



                OleDbCommand YTD_Report_RE_Perce = new OleDbCommand();
                YTD_Report_RE_Perce.Connection = transactConn;
                YTD_Report_RE_Perce.CommandTimeout = 0;
                YTD_Report_RE_Perce.CommandText = "select  ' ',' ' ,																																																    " +
    "convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' , " +
    "convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%'," +
               "' ',' ',                                                                                           " +
    "convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%', " +
    "convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' ' " +
               "from                                                                                                                 " +
               "(                                                                                                                    " +
               "	select                                                                                                              " +
               "	count(*) as All_trxs,                                                                                               " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,                               " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,                               " +
               "	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,                                                          " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then                                                              " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,                           " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then                                                                " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt                        " +
               "	from RegencyW a                                                                                                      " +
               "	where (  " +
                sqlSelectReg +
               " ) " +
               ")q  ";
                OleDbDataReader rdr_ForExcel_Perce_RE = YTD_Report_RE_Perce.ExecuteReader();

                while (rdr_ForExcel_Perce_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Perce_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Perce_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_Perce_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Perce_RE.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Perce_RE.Close();


                OleDbCommand PerTid_Report_RE = new OleDbCommand();
                PerTid_Report_RE.Connection = transactConn;
                PerTid_Report_RE.CommandTimeout = 0;
                PerTid_Report_RE.CommandText = "select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency, " +
                                                    "count(*) as ALL_TRN, " +
                                                    "sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted, " +
                                                    "(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,  " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted  " +
                                                    "from RegencyW a " +
                                                    "where  " +
                                                    "datum > '2019-" + file_ArrRe[2] + "-01 00:00:00' and " +
                                                    " ( " +
                                                        sqlSelectReg +
                                                    " ) " +
                                                    "group by  right(tid,8),left(datum,10),dcc_currency " +
                                                    "order by  right(tid,8),left(datum,10),dcc_currency ";


                rdr_ForExcel_RE = PerTid_Report_RE.ExecuteReader();

                OleDbCommand xlCmd_PerTid_RE = new OleDbCommand("CREATE TABLE [Per TID-Day-Currency] ([TID] char(255),[Date] char(255),[DCC CURRENCY] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_PerTid_RE.Connection = xlcONNRE;
                xlCmd_PerTid_RE.ExecuteNonQuery();

                while (rdr_ForExcel_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per TID-Day-Currency] ([TID],[Date],[DCC CURRENCY],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(8).ToString() + "','" + rdr_ForExcel_RE.GetValue(9).ToString() + "','" + rdr_ForExcel_RE.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RE.Close();



                OleDbCommand details_Report_RE = new OleDbCommand();
                details_Report_RE.Connection = transactConn;
                details_Report_RE.CommandTimeout = 0;
                details_Report_RE.CommandText = "select substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) as Transaction_Date, " +
                                                    "substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) +' ' + substring(RIGHT(datum,8),1,5) As Transaction_TimeStamp, " +
                                                    "right(TID,8) as TID_, vispan as PAN, " +
                                                    "(replace((cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2))),'.',',')) as  Original_Amount, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,   " +
                                                     "(replace((case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(dcc_amount as dec(15,0))/100) as dec(15,2)) else 0 end),'.',',')) as DCC_AMOUNT, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN, " +
                                                     "left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN " +
                                                     " from RegencyW a " +
                                                     " where  " +
                                                     "datum > '2019-" + file_ArrRe[2] + "-01 00:00:00' and " +
                                                     "( " +
                                                      sqlSelectReg+
                                                     " ) " +
                                                     " order by left(datum,10),Datum,TID ";

                rdr_ForExcel_RE = details_Report_RE.ExecuteReader();

                OleDbCommand xlCmd_details_RE = new OleDbCommand("CREATE TABLE [Details] ([Transaction_Date] char(255),[Transaction_TimeStamp] char(255),[TID_] char(255) ,[PAN] char(255),[Original_Amount] char(255)," +
                    "[dcc__currency] char(255),[DCC_AMOUNT] char(255),[Eligible_YN] char(255),[DCC_Accepted_YN] char(255))");

                xlCmd_details_RE.Connection = xlcONNRE;
                xlCmd_details_RE.ExecuteNonQuery();

                while (rdr_ForExcel_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Details] ([Transaction_Date],[Transaction_TimeStamp],[TID_],[PAN],[Original_Amount],[dcc__currency],[DCC_AMOUNT], " +
                   " [Eligible_YN],[DCC_Accepted_YN])" +
                   "values ('" + rdr_ForExcel_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RE.Close();




                OleDbCommand PerAreaD_report_RE = new OleDbCommand();
                PerAreaD_report_RE.Connection = transactConn;
                PerAreaD_report_RE.CommandTimeout = 0;
                PerAreaD_report_RE.CommandText = "select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,10) as Date, " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Accepted," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Not_Accepted " +
                                                      "from RegencyW a, abc096.merchants b " +
                                                      "where " +
                                                      "datum > '2019-" + file_ArrRe[2] + "-01 00:00:00' and " +
                                                      "right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and " +
                                                      "( " +
                                                         sqlSelectReg +
                                                      " ) " +
                                                      "group by  merchaddress,left(datum,10) " +
                                                      "order by  merchaddress,left(datum,10) ";

                rdr_ForExcel_RE = PerAreaD_report_RE.ExecuteReader();

                OleDbCommand xlCmd_PerAreaD_RE = new OleDbCommand("CREATE TABLE [Per Day Per Area] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255) ,[ALL_TRN] char(255),[Eligible] char(255)," +
                    "[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[ALL_AMNT] char(255),[Eligible_] char(255), [DCC_Accepted_] char(255),[DCC_Not_Accepted_] char(255))");

                xlCmd_PerAreaD_RE.Connection = xlcONNRE;
                xlCmd_PerAreaD_RE.ExecuteNonQuery();

                while (rdr_ForExcel_RE.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per Day Per Area] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [ALL_AMNT],[Eligible_],[DCC_Accepted_],[DCC_Not_Accepted_])" +
                   "values ('" + rdr_ForExcel_RE.GetValue(0).ToString() + "','" + rdr_ForExcel_RE.GetValue(1).ToString() + "','" + rdr_ForExcel_RE.GetValue(2).ToString() + "','" + rdr_ForExcel_RE.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(4).ToString() + "','" + rdr_ForExcel_RE.GetValue(5).ToString() + "','" + rdr_ForExcel_RE.GetValue(6).ToString() + "','" + rdr_ForExcel_RE.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RE.GetValue(8).ToString() + "','" + rdr_ForExcel_RE.GetValue(9).ToString() + "','" + rdr_ForExcel_RE.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNRE;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RE.Close();
                xlcONNRE.Close();

                Console.Write("Excel for Regency exported.  \r\n");



                //  - - - - - - - - - - - - - - - -     RVU      - - - - - - - - - - - - - - - - 
                
                xlcONNRV.Open();


                OleDbCommand YTD_Report_RV = new OleDbCommand();
                YTD_Report_RV.Connection = transactConn;
                YTD_Report_RV.CommandTimeout = 0;
                YTD_Report_RV.CommandText = "select substring(LEFT(datum,10),1,4)+substring(LEFT(datum,10),6,2)+ substring(LEFT(datum,10),9,2), " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                                                "from RVUW a " +
                                                "where ( " +
                                                    sqlSelectRvu +
                                                " )" +
                                                " group by left(datum,10) " +
                                                " order by left(datum,10) ";

                OleDbDataReader rdr_ForExcel_RV = YTD_Report_RV.ExecuteReader();

                OleDbCommand xlCmd_YTD_RV = new OleDbCommand("CREATE TABLE [YTD] ([Date] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_YTD_RV.Connection = xlcONNRV;
                xlCmd_YTD_RV.ExecuteNonQuery();

                while (rdr_ForExcel_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_RV.Close();



                OleDbCommand YTD_Report_RV_Tot = new OleDbCommand();
                YTD_Report_RV_Tot.Connection = transactConn;
                YTD_Report_RV_Tot.CommandTimeout = 0;
                YTD_Report_RV_Tot.CommandText = "select 'Totals',    " +
    "replace((replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as ALL_TRN,  " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as Eligible,        " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Accepted,      " +
    "replace((replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','')),',','.') as DCC_Not_Accepted,  " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€' as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as Eligible, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.')) + '€'  as DCC_Not_Accepted " +
                "from RVUW a	" +
                "where ( " +
                sqlSelectRvu +
                " ) ";
                OleDbDataReader rdr_ForExcel_Total_RV = YTD_Report_RV_Tot.ExecuteReader();

                while (rdr_ForExcel_Total_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Total_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Total_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_Total_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Total_RV.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Total_RV.Close();



                OleDbCommand YTD_Report_RV_Perce = new OleDbCommand();
                YTD_Report_RV_Perce.Connection = transactConn;
                YTD_Report_RV_Perce.CommandTimeout = 0;
                YTD_Report_RV_Perce.CommandText = "select  ' ',' ' ,																																																    " +
    "convert(varchar, (cast(((cast((q.Eligible_Cnt) as dec(15,2))  / All_trxs) * 100) as dec(15,2)))) + '%' , " +
    "convert(varchar, (cast(((cast((q.DCC_Accepted_Cnt ) as dec(15,2)) / cast((q.Eligible_Cnt) as dec(15,2))) * 100) as dec(15,2)))) + '%'," +
               "' ',' ',                                                                                           " +
    "convert(varchar,(cast(((Eligible_Amnt / ALL_AMNT) * 100) as dec(15,2)))) + '%', " +
    "convert(varchar,(cast(((DCC_Accepted_Amnt / Eligible_Amnt) * 100) as dec(15,2)))) + '%', ' ' " +
               "from                                                                                                                 " +
               "(                                                                                                                    " +
               "	select                                                                                                              " +
               "	count(*) as All_trxs,                                                                                               " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible_Cnt,                               " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)  DCC_Accepted_Cnt,                               " +
               "	sum(cast((merchant_amount/100) as dec(15,2))) as ALL_AMNT,                                                          " +
               "	sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then                                                              " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as Eligible_Amnt,                           " +
               "	sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then                                                                " +
               "	cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2)) else 0 end) as DCC_Accepted_Amnt                        " +
               "	from RVUW a                                                                                                      " +
               "	where (  " +
                sqlSelectRvu +
               " ) " +
               ")q  ";
                OleDbDataReader rdr_ForExcel_Perce_RV = YTD_Report_RV_Perce.ExecuteReader();

                while (rdr_ForExcel_Perce_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [YTD] ([Date],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_Perce_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_Perce_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_Perce_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_Perce_RV.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }
                rdr_ForExcel_Perce_RV.Close();


                OleDbCommand PerTid_Report_RV = new OleDbCommand();
                PerTid_Report_RV.Connection = transactConn;
                PerTid_Report_RV.CommandTimeout = 0;
                PerTid_Report_RV.CommandText = "select right(tid,8),substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4),dcc_currency, " +
                                                    "count(*) as ALL_TRN, " +
                                                    "sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted, " +
                                                    "sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted, " +
                                                    "(replace((replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),'.00',' ')),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible, " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Accepted,  " +
                                                    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))   as DCC_Not_Accepted  " +
                                                    "from RVUW a " +
                                                    "where  " +
                                                    "datum > '2019-" + file_ArrRv[2] + "-01 00:00:00' and " +
                                                    " ( " +
                                                        sqlSelectRvu +
                                                    " ) " +
                                                    "group by  right(tid,8),left(datum,10),dcc_currency " +
                                                    "order by  right(tid,8),left(datum,10),dcc_currency ";


                rdr_ForExcel_RV = PerTid_Report_RV.ExecuteReader();

                OleDbCommand xlCmd_PerTid_RV = new OleDbCommand("CREATE TABLE [Per TID-Day-Currency] ([TID] char(255),[Date] char(255),[DCC CURRENCY] char(255),[All] char(255),[DCC ELIGIBLE] char(255),[DCC ACCEPTED] char(255),[DCC NOT ACCEPTED] char(255),[All_] char(255), " +
     " [DCC ELIGIBLE_] char(255),[DCC ACCEPTED_] char(255),[DCC NOT ACCEPTED_] char(255))");

                xlCmd_PerTid_RV.Connection = xlcONNRV;
                xlCmd_PerTid_RV.ExecuteNonQuery();

                while (rdr_ForExcel_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per TID-Day-Currency] ([TID],[Date],[DCC CURRENCY],[All],[DCC ELIGIBLE],[DCC ACCEPTED],[DCC NOT ACCEPTED], " +
                   " [All_],[DCC ELIGIBLE_],[DCC ACCEPTED_],[DCC NOT ACCEPTED_])" +
                   "values ('" + rdr_ForExcel_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(8).ToString() + "','" + rdr_ForExcel_RV.GetValue(9).ToString() + "','" + rdr_ForExcel_RV.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RV.Close();



                OleDbCommand details_Report_RV = new OleDbCommand();
                details_Report_RV.Connection = transactConn;
                details_Report_RV.CommandTimeout = 0;
                details_Report_RV.CommandText = "select substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) as Transaction_Date, " +
                                                    "substring(LEFT(datum,10),9,2) + '/' +substring(LEFT(datum,10),6,2)+ '/' +substring(LEFT(datum,10),1,4) +' ' + substring(RIGHT(datum,8),1,5) As Transaction_TimeStamp, " +
                                                    "right(TID,8) as TID_, vispan as PAN, " +
                                                    "(replace((cast((Cast(merchant_amount as dec(15,0))/100) as dec(15,2))),'.',',')) as  Original_Amount, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then dcc_currency else ' ' end) dcc__currency ,   " +
                                                     "(replace((case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then cast((Cast(dcc_amount as dec(15,0))/100) as dec(15,2)) else 0 end),'.',',')) as DCC_AMOUNT, " +
                                                     "(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 'Y' else 'N' end) as Eligible_YN, " +
                                                     "left(DCCCHOSEN_DCCELIGIBLE,1) DCC_Accepted_YN " +
                                                     " from RVUW a " +
                                                     " where  " +
                                                     "datum > '2019-" + file_ArrRv[2] + "-01 00:00:00' and " +
                                                     "( " +
                                                      sqlSelectRvu +
                                                     " ) " +
                                                     " order by left(datum,10),Datum,TID ";

                rdr_ForExcel_RV = details_Report_RV.ExecuteReader();

                OleDbCommand xlCmd_details_RV = new OleDbCommand("CREATE TABLE [Details] ([Transaction_Date] char(255),[Transaction_TimeStamp] char(255),[TID_] char(255) ,[PAN] char(255),[Original_Amount] char(255)," +
                    "[dcc__currency] char(255),[DCC_AMOUNT] char(255),[Eligible_YN] char(255),[DCC_Accepted_YN] char(255))");

                xlCmd_details_RV.Connection = xlcONNRV;
                xlCmd_details_RV.ExecuteNonQuery();

                while (rdr_ForExcel_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Details] ([Transaction_Date],[Transaction_TimeStamp],[TID_],[PAN],[Original_Amount],[dcc__currency],[DCC_AMOUNT], " +
                   " [Eligible_YN],[DCC_Accepted_YN])" +
                   "values ('" + rdr_ForExcel_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(8).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RV.Close();




                OleDbCommand PerAreaD_report_RV = new OleDbCommand();
                PerAreaD_report_RV.Connection = transactConn;
                PerAreaD_report_RV.CommandTimeout = 0;
                PerAreaD_report_RV.CommandText = "select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,10) as Date, " +
    "replace((convert(varchar,(convert(money,(CAST((count(*)) as dec(10,0))/1),0)),1)),'.00','') as ALL_TRN,  " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as Eligible,       " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Accepted,     " +
    "replace((convert(varchar,(convert(money,(CAST((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end)) as dec(10,0))/1),0)),1)),'.00','') as DCC_Not_Accepted, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(cast((merchant_amount/100) as dec(15,2))))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as ALL_AMNT, " +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as Eligible," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Accepted," +
    "(replace((replace((replace((convert(nvarchar(15),(cast((sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then (Cast(merchant_amount as dec(15,2))/100) else 0 end))as money)),1)),','  ,'_' )),'.',',')),'_','.'))  as DCC_Not_Accepted " +
                                                      "from RVUW a, abc096.merchants b " +
                                                      "where " +
                                                      "datum > '2019-" + file_ArrRv[2] + "-01 00:00:00' and " +
                                                      "right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and " +
                                                      "( " +
                                                         sqlSelectRvu +
                                                      " ) " +
                                                      "group by  merchaddress,left(datum,10) " +
                                                      "order by  merchaddress,left(datum,10) ";

                rdr_ForExcel_RV = PerAreaD_report_RV.ExecuteReader();

                OleDbCommand xlCmd_PerAreaD_RV = new OleDbCommand("CREATE TABLE [Per Day Per Area] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255) ,[ALL_TRN] char(255),[Eligible] char(255)," +
                    "[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[ALL_AMNT] char(255),[Eligible_] char(255), [DCC_Accepted_] char(255),[DCC_Not_Accepted_] char(255))");

                xlCmd_PerAreaD_RV.Connection = xlcONNRV;
                xlCmd_PerAreaD_RV.ExecuteNonQuery();

                while (rdr_ForExcel_RV.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Per Day Per Area] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [ALL_AMNT],[Eligible_],[DCC_Accepted_],[DCC_Not_Accepted_])" +
                   "values ('" + rdr_ForExcel_RV.GetValue(0).ToString() + "','" + rdr_ForExcel_RV.GetValue(1).ToString() + "','" + rdr_ForExcel_RV.GetValue(2).ToString() + "','" + rdr_ForExcel_RV.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(4).ToString() + "','" + rdr_ForExcel_RV.GetValue(5).ToString() + "','" + rdr_ForExcel_RV.GetValue(6).ToString() + "','" + rdr_ForExcel_RV.GetValue(7).ToString()
                   + "','" + rdr_ForExcel_RV.GetValue(8).ToString() + "','" + rdr_ForExcel_RV.GetValue(9).ToString() + "','" + rdr_ForExcel_RV.GetValue(10).ToString() + "')");

                    excelWrite.Connection = xlcONNRV;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_RV.Close();
                xlcONNRV.Close();

                Console.Write("Excel for RVU exported.  \r\n");



                //  - - - - - - - - - - - - - - - -    OPTIN     - - - - - - - - - - - - - - - -
                xlcONNOPT.Open();

               OleDbCommand OPTIN_Report_ATT = new OleDbCommand();
                OPTIN_Report_ATT.Connection = transactConn;
                OPTIN_Report_ATT.CommandTimeout = 0;
                OPTIN_Report_ATT.CommandText =
" select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date,  	 " +
" count(*) as ALL_TRN,                                                                                                                                                                                                                                                   " +
" sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,                                                                                                                                                                                      " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,                                                                                                                                                                                    " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,                                                                                                                                                                                " +
" convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'    " +
" from attikaw a, abc096.merchants b                                                                                                                                                                                                                                     " +
" where datum > '2017-10-01 00:00:00' and                                                                                                                                                                                                                                " +
" right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and                                                                                                                                                                                                                " +
" (                                                                                                                                                                                                                                                                      " +
    sqlSelectAtt +
" )                                                                                                                                                                                                                                                                      " +
" group by  merchaddress,left(datum,7)                                                                                                                                                                                                                                   " +
" order by  merchaddress,left(datum,7)                                                                                                                                                                                                                                   ";

                OleDbDataReader rdr_ForExcel_OPT = OPTIN_Report_ATT.ExecuteReader();

                OleDbCommand xlCmd_OPT_ATT = new OleDbCommand("CREATE TABLE [Attica] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255),[ALL_TRN] char(255),[Eligible] char(255),[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[OPTIN] char(255)) ");


                xlCmd_OPT_ATT.Connection = xlcONNOPT;
                xlCmd_OPT_ATT.ExecuteNonQuery();

                while (rdr_ForExcel_OPT.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Attica] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [OPTIN])" +
                   "values ('" + rdr_ForExcel_OPT.GetValue(0).ToString() + "','" + rdr_ForExcel_OPT.GetValue(1).ToString() + "','" + rdr_ForExcel_OPT.GetValue(2).ToString() + "','" + rdr_ForExcel_OPT.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_OPT.GetValue(4).ToString() + "','" + rdr_ForExcel_OPT.GetValue(5).ToString() + "','" + rdr_ForExcel_OPT.GetValue(6).ToString() + "','" + rdr_ForExcel_OPT.GetValue(7).ToString() + "')");


                    excelWrite.Connection = xlcONNOPT;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_OPT.Close();



                OleDbCommand OPTIN_Report_DF = new OleDbCommand();
                OPTIN_Report_DF.Connection = transactConn;
                OPTIN_Report_DF.CommandTimeout = 0;
                OPTIN_Report_DF.CommandText =
" select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date,	" +
" count(*) as ALL_TRN,   " +
" sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible, " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted, " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted, " +
" convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'   " +
" from dutyfreew a, abc096.merchants b  " +
" where datum > '2017-01-01 00:00:00' and   " +
" right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC'   " +
" group by  merchaddress,left(datum,7)   " +
" order by  merchaddress,left(datum,7)" ;

                rdr_ForExcel_OPT = OPTIN_Report_DF.ExecuteReader();

                OleDbCommand xlCmd_OPT_DF = new OleDbCommand("CREATE TABLE [Duty Free] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255),[ALL_TRN] char(255),[Eligible] char(255),[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[OPTIN] char(255)) ");


                xlCmd_OPT_DF.Connection = xlcONNOPT;
                xlCmd_OPT_DF.ExecuteNonQuery();

                while (rdr_ForExcel_OPT.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Duty Free] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [OPTIN])" +
                   "values ('" + rdr_ForExcel_OPT.GetValue(0).ToString() + "','" + rdr_ForExcel_OPT.GetValue(1).ToString() + "','" + rdr_ForExcel_OPT.GetValue(2).ToString() + "','" + rdr_ForExcel_OPT.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_OPT.GetValue(4).ToString() + "','" + rdr_ForExcel_OPT.GetValue(5).ToString() + "','" + rdr_ForExcel_OPT.GetValue(6).ToString() + "','" + rdr_ForExcel_OPT.GetValue(7).ToString() + "')");


                    excelWrite.Connection = xlcONNOPT;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_OPT.Close();


                OleDbCommand OPTIN_Report_RE = new OleDbCommand();
                OPTIN_Report_RE.Connection = transactConn;
                OPTIN_Report_RE.CommandTimeout = 0;
                OPTIN_Report_RE.CommandText = 
" select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date, " +
" count(*) as ALL_TRN,                                                                                                                                                                                                                                                 " +
" sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,                                                                                                                                                                                    " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,                                                                                                                                                                                  " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,                                                                                                                                                                              " +
" convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'  " +
" from regencyw a, abc096.merchants b                                                                                                                                                                                                                                  " +
" where datum > '2018-01-01 00:00:00' and                                                                                                                                                                                                                              " +
" right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and                                                                                                                                                                                                              " +
" (     " +
     sqlSelectReg +
" )   " +
" group by  merchaddress,left(datum,7)                                                                                                                                                                                                                                 " +
" order by  merchaddress,left(datum,7)                                                                                                                                                                                                                                 ";

                 rdr_ForExcel_OPT = OPTIN_Report_RE.ExecuteReader();

                OleDbCommand xlCmd_OPT_RE = new OleDbCommand("CREATE TABLE [Regency] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255),[ALL_TRN] char(255),[Eligible] char(255),[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[OPTIN] char(255)) ");


                xlCmd_OPT_RE.Connection = xlcONNOPT;
                xlCmd_OPT_RE.ExecuteNonQuery();

                while (rdr_ForExcel_OPT.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [Regency] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [OPTIN])" +
                   "values ('" + rdr_ForExcel_OPT.GetValue(0).ToString() + "','" + rdr_ForExcel_OPT.GetValue(1).ToString() + "','" + rdr_ForExcel_OPT.GetValue(2).ToString() + "','" + rdr_ForExcel_OPT.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_OPT.GetValue(4).ToString() + "','" + rdr_ForExcel_OPT.GetValue(5).ToString() + "','" + rdr_ForExcel_OPT.GetValue(6).ToString() + "','" + rdr_ForExcel_OPT.GetValue(7).ToString() + "')");
                   

                    excelWrite.Connection = xlcONNOPT;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_OPT.Close();
                

                OleDbCommand OPTIN_Report_RV = new OleDbCommand();
                OPTIN_Report_RV.Connection = transactConn;
                OPTIN_Report_RV.CommandTimeout = 0;
                OPTIN_Report_RV.CommandText = 
" select merchaddress, count(distinct right(a.TID,8)) As Number_Of_Active_Terminals, left(datum,7) as Date, " +
" count(*) as ALL_TRN,                                                                                                                                                                                                                                                 " +
" sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end) as Eligible,                                                                                                                                                                                    " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end) as DCC_Accepted,                                                                                                                                                                                  " +
" sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='N' then 1 else 0 end) as DCC_Not_Accepted,                                                                                                                                                                              " +
" convert(varchar,(cast( (cast( (sum(case when left(DCCCHOSEN_DCCELIGIBLE,1)='Y' then 1 else 0 end))  as decimal(5,0)) / nullif( (cast( (sum(case when right(DCCCHOSEN_DCCELIGIBLE,2)='EG' then 1 else 0 end))  as decimal(5,0))),0) * 100) as decimal(6,2)))) + ' %'  " +
" from RVUW a, abc096.merchants b                                                                                                                                                                                                                                  " +
" where datum > '2019-04-01 00:00:00' and                                                                                                                                                                                                                              " +
" right(a.TID,8)=b.TID and b.uploadhostname='NET_ABC' and                                                                                                                                                                                                              " +
" (     " +
     sqlSelectRvu +
" )   " +
" group by  merchaddress,left(datum,7)                                                                                                                                                                                                                                 " +
" order by  merchaddress,left(datum,7)                                                                                                                                                                                                                                 ";

                 rdr_ForExcel_OPT = OPTIN_Report_RV.ExecuteReader();

                OleDbCommand xlCmd_OPT_RV = new OleDbCommand("CREATE TABLE [RVU] ([merchaddress] char(255),[Number_Of_Active_Terminals] char(255),[Date] char(255),[ALL_TRN] char(255),[Eligible] char(255),[DCC_Accepted] char(255),[DCC_Not_Accepted] char(255),[OPTIN] char(255)) ");


                xlCmd_OPT_RV.Connection = xlcONNOPT;
                xlCmd_OPT_RV.ExecuteNonQuery();

                while (rdr_ForExcel_OPT.Read())
                {
                    OleDbCommand excelWrite = new OleDbCommand("Insert into [RVU] ([merchaddress],[Number_Of_Active_Terminals],[Date],[ALL_TRN],[Eligible],[DCC_Accepted],[DCC_Not_Accepted], " +
                   " [OPTIN])" +
                   "values ('" + rdr_ForExcel_OPT.GetValue(0).ToString() + "','" + rdr_ForExcel_OPT.GetValue(1).ToString() + "','" + rdr_ForExcel_OPT.GetValue(2).ToString() + "','" + rdr_ForExcel_OPT.GetValue(3).ToString()
                   + "','" + rdr_ForExcel_OPT.GetValue(4).ToString() + "','" + rdr_ForExcel_OPT.GetValue(5).ToString() + "','" + rdr_ForExcel_OPT.GetValue(6).ToString() + "','" + rdr_ForExcel_OPT.GetValue(7).ToString() + "')");
                   

                    excelWrite.Connection = xlcONNOPT;
                    excelWrite.ExecuteNonQuery();
                }

                rdr_ForExcel_OPT.Close();

                xlcONNOPT.Close();

                Console.Write("Excel for OPTIN exported.  \r\n");

                
                //Close connection
                transactConn.Close();



                // - - - - - - - - - -  - - - -  - - - - -- - -  -    Edit Excels

                string spreadSheetAttLocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\AtticaDCCYTD_" + timestamp + ".xlsx";
                string spreadSheetDFLocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".xlsx";
                string spreadSheetECSLocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + "_ECS.xlsx";
                string spreadSheetRELocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RegencyDCCYTD_" + timestamp + ".xlsx";
                string spreadSheetRVLocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RVUDCCYTD_" + timestamp + ".xlsx";
                string spreadSheetOPTLocation = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DCC_OPTIN_" + timestamp + ".xlsx";

                //string spreadSheetLocation = @"\\grat1-dev-ap2t\d$\Test\Attica DCC YTD_20180514.xlsx";
                Microsoft.Office.Interop.Excel.Application excel_ = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheet = excel_.Workbooks.Open(spreadSheetAttLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet x = excel_.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xx = excel_.Sheets.get_Item(2) as Microsoft.Office.Interop.Excel.Worksheet;


                // Autofit
                x.Columns.AutoFit();

                // Insert Blank row at beginning
                x.get_Range("A1", "I1").Insert(Type.Missing, Type.Missing);

                // Add value to specific Cell
                x.get_Range("B1", "B1").Value2 = "Transaction Count";
                x.get_Range("B1", "B1").Font.Bold = true;

                x.get_Range("F1", "F1").Value2 = "Transaction Amount";
                x.get_Range("F1", "F1").Font.Bold = true;

                //Bold
                x.get_Range("A2", "I2").EntireRow.Font.Bold = true;
                xx.get_Range("A1", "K1").EntireRow.Font.Bold = true;

                // Merge
                x.get_Range(x.Cells[1, 2], x.Cells[1, 5]).Merge(Type.Missing);
                x.get_Range(x.Cells[1, 6], x.Cells[1, 9]).Merge(Type.Missing);

                // Add Border
                Excel.Range last = x.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX = x.get_Range("A1", last);
                Excel.Borders xBorders = rangeX.Borders;

                xBorders.LineStyle = Excel.XlLineStyle.xlContinuous;
                xBorders.Weight = 2d;

                // Freeze Panes
                x.Activate();
                x.Application.ActiveWindow.SplitRow = 2;
                x.Application.ActiveWindow.FreezePanes = true;

                // Find last row & column
                Excel.Range lastX2 = x.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX2 = x.get_Range("B1", lastX2);

                int lastUsedRow = lastX2.Row;
                int lastUsedCol = lastX2.Column;

                //Change color in specific row
                string lastCol_B = "B" + (lastUsedRow - 1);
                string lastCol_C = "C" + (lastUsedRow - 1);
                string lastCol_D = "D" + (lastUsedRow - 1);
                string lastCol_E = "E" + (lastUsedRow - 1);

                x.get_Range("B1", lastCol_B).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                x.get_Range("C1", lastCol_C).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                x.get_Range("D1", lastCol_D).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                x.get_Range("E1", lastCol_E).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));

                x.get_Range("A" + (lastUsedRow - 1), "I" + (lastUsedRow - 1)).Font.Bold = true;
                x.get_Range("A" + (lastUsedRow), "I" + (lastUsedRow)).Font.Bold = true;


                sheet.Close(true, Type.Missing, Type.Missing);
                excel_.Quit();


                // - - - - - - -  - -  Duty Free   - - - - - - - - - - -
                Microsoft.Office.Interop.Excel.Application excel_DF = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheetDF = excel_DF.Workbooks.Open(spreadSheetDFLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xDF = excel_DF.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxDF = excel_DF.Sheets.get_Item(2) as Microsoft.Office.Interop.Excel.Worksheet;

                xDF.Columns.AutoFit();
                xDF.get_Range("A1", "I1").Insert(Type.Missing, Type.Missing);

                xDF.get_Range("B1", "B1").Value2 = "Transaction Count";
                xDF.get_Range("B1", "B1").Font.Bold = true;

                xDF.get_Range("F1", "F1").Value2 = "Transaction Amount";
                xDF.get_Range("F1", "F1").Font.Bold = true;
                xxDF.get_Range("A1", "K1").EntireRow.Font.Bold = true;

                xDF.get_Range("A2", "I2").EntireRow.Font.Bold = true;

                xDF.get_Range(xDF.Cells[1, 2], xDF.Cells[1, 5]).Merge(Type.Missing);
                xDF.get_Range(xDF.Cells[1, 6], xDF.Cells[1, 9]).Merge(Type.Missing);

                Excel.Range lastDF = xDF.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeXDF = xDF.get_Range("A1", lastDF);
                Excel.Borders xBordersDF = rangeXDF.Borders;

                xBordersDF.LineStyle = Excel.XlLineStyle.xlContinuous;
                xBordersDF.Weight = 2d;

                xDF.Activate();
                xDF.Application.ActiveWindow.SplitRow = 2;
                xDF.Application.ActiveWindow.FreezePanes = true;

                // Find last row & column
                Excel.Range lastX3 = xDF.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX3 = xDF.get_Range("B1", lastX3);

                int lastUsedRowDF = lastX3.Row;
                int lastUsedColDF = lastX3.Column;

                //Change color in specific row
                string lastCol_B_DF = "B" + (lastUsedRowDF - 1);
                string lastCol_C_DF = "C" + (lastUsedRowDF - 1);
                string lastCol_D_DF = "D" + (lastUsedRowDF - 1);
                string lastCol_E_DF = "E" + (lastUsedRowDF - 1);

                xDF.get_Range("B1", lastCol_B_DF).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xDF.get_Range("C1", lastCol_C_DF).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xDF.get_Range("D1", lastCol_D_DF).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xDF.get_Range("E1", lastCol_E_DF).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));

                xDF.get_Range("A" + (lastUsedRowDF - 1), "I" + (lastUsedRowDF - 1)).Font.Bold = true;
                xDF.get_Range("A" + (lastUsedRowDF), "I" + (lastUsedRowDF)).Font.Bold = true;

                sheetDF.Close(true, Type.Missing, Type.Missing);
                excel_DF.Quit();

                //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;


                // - - - - - - -  - -  Duty Free ECS  - - - - - - - - - - -
                Microsoft.Office.Interop.Excel.Application excel_ECS = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheetECS = excel_ECS.Workbooks.Open(spreadSheetECSLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xECS = excel_ECS.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            
                xECS.Columns.AutoFit();
                xECS.get_Range("A1", "I1").Insert(Type.Missing, Type.Missing);

                xECS.get_Range("B1", "B1").Value2 = "Transaction Count";
                xECS.get_Range("B1", "B1").Font.Bold = true;

                xECS.get_Range("F1", "F1").Value2 = "Transaction Amount";
                xECS.get_Range("F1", "F1").Font.Bold = true;

                xECS.get_Range("A2", "I2").EntireRow.Font.Bold = true;

                xECS.get_Range(xECS.Cells[1, 2], xECS.Cells[1, 5]).Merge(Type.Missing);
                xECS.get_Range(xECS.Cells[1, 6], xECS.Cells[1, 9]).Merge(Type.Missing);

                Excel.Range lastECS = xECS.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangexECS = xECS.get_Range("A1", lastECS);
                Excel.Borders xBordersECS = rangexECS.Borders;

                xBordersECS.LineStyle = Excel.XlLineStyle.xlContinuous;
                xBordersECS.Weight = 2d;

                xECS.Activate();
                xECS.Application.ActiveWindow.SplitRow = 2;
                xECS.Application.ActiveWindow.FreezePanes = true;

                // Find last row & column
                Excel.Range lastX3ECS = xECS.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX3ECS = xECS.get_Range("B1", lastX3ECS);

                int lastUsedRowECS = lastX3ECS.Row;
                int lastUsedColECS = lastX3ECS.Column;

                //Change color in specific row
                string lastCol_B_ECS = "B" + (lastUsedRowECS - 1);
                string lastCol_C_ECS = "C" + (lastUsedRowECS - 1);
                string lastCol_D_ECS = "D" + (lastUsedRowECS - 1);
                string lastCol_E_ECS = "E" + (lastUsedRowECS - 1);

                xECS.get_Range("B1", lastCol_B_ECS).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xECS.get_Range("C1", lastCol_C_ECS).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xECS.get_Range("D1", lastCol_D_ECS).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xECS.get_Range("E1", lastCol_E_ECS).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));

                xECS.get_Range("A" + (lastUsedRowECS - 1), "I" + (lastUsedRowECS - 1)).Font.Bold = true;
                xECS.get_Range("A" + (lastUsedRowECS), "I" + (lastUsedRowECS)).Font.Bold = true;

                sheetECS.Close(true, Type.Missing, Type.Missing);
                excel_ECS.Quit();


                // - - - - - - -  - -  Regency   - - - - - - - - - - -
                Microsoft.Office.Interop.Excel.Application excel_RE = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheetRE = excel_RE.Workbooks.Open(spreadSheetRELocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xRE = excel_RE.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxRE = excel_RE.Sheets.get_Item(2) as Microsoft.Office.Interop.Excel.Worksheet;

                xRE.Columns.AutoFit();
                xRE.get_Range("A1", "I1").Insert(Type.Missing, Type.Missing);

                xRE.get_Range("B1", "B1").Value2 = "Transaction Count";
                xRE.get_Range("B1", "B1").Font.Bold = true;

                xRE.get_Range("F1", "F1").Value2 = "Transaction Amount";
                xRE.get_Range("F1", "F1").Font.Bold = true;
                xxRE.get_Range("A1", "K1").EntireRow.Font.Bold = true;

                xRE.get_Range("A2", "I2").EntireRow.Font.Bold = true;

                xRE.get_Range(xRE.Cells[1, 2], xRE.Cells[1, 5]).Merge(Type.Missing);
                xRE.get_Range(xRE.Cells[1, 6], xRE.Cells[1, 9]).Merge(Type.Missing);

                Excel.Range lastRE = xRE.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeXRE = xRE.get_Range("A1", lastRE);
                Excel.Borders xBordersRE = rangeXRE.Borders;

                xBordersRE.LineStyle = Excel.XlLineStyle.xlContinuous;
                xBordersRE.Weight = 2d;

                xRE.Activate();
                xRE.Application.ActiveWindow.SplitRow = 2;
                xRE.Application.ActiveWindow.FreezePanes = true;

                // Find last row & column
                Excel.Range lastX3RE = xRE.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX3RE = xRE.get_Range("B1", lastX3RE);

                int lastUsedRowRE = lastX3RE.Row;
                int lastUsedColRE = lastX3RE.Column;

                //Change color in specific row
                string lastCol_B_RE = "B" + (lastUsedRowRE - 1);
                string lastCol_C_RE = "C" + (lastUsedRowRE - 1);
                string lastCol_D_RE = "D" + (lastUsedRowRE - 1);
                string lastCol_E_RE = "E" + (lastUsedRowRE - 1);

                xRE.get_Range("B1", lastCol_B_RE).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRE.get_Range("C1", lastCol_C_RE).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRE.get_Range("D1", lastCol_D_RE).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRE.get_Range("E1", lastCol_E_RE).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));

                xRE.get_Range("A" + (lastUsedRowRE - 1), "I" + (lastUsedRowRE - 1)).Font.Bold = true;
                xRE.get_Range("A" + (lastUsedRowRE), "I" + (lastUsedRowRE)).Font.Bold = true;

                sheetRE.Close(true, Type.Missing, Type.Missing);
                excel_RE.Quit();


                // - - - - - - - - - - -   RVU   - - - - - - - - - - 
                Microsoft.Office.Interop.Excel.Application excel_RV = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheetRV = excel_RV.Workbooks.Open(spreadSheetRVLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xRV = excel_RV.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxRV = excel_RV.Sheets.get_Item(2) as Microsoft.Office.Interop.Excel.Worksheet;

                xRV.Columns.AutoFit();
                xRV.get_Range("A1", "I1").Insert(Type.Missing, Type.Missing);

                xRV.get_Range("B1", "B1").Value2 = "Transaction Count";
                xRV.get_Range("B1", "B1").Font.Bold = true;

                xRV.get_Range("F1", "F1").Value2 = "Transaction Amount";
                xRV.get_Range("F1", "F1").Font.Bold = true;
                xxRV.get_Range("A1", "K1").EntireRow.Font.Bold = true;

                xRV.get_Range("A2", "I2").EntireRow.Font.Bold = true;

                xRV.get_Range(xRV.Cells[1, 2], xRV.Cells[1, 5]).Merge(Type.Missing);
                xRV.get_Range(xRV.Cells[1, 6], xRV.Cells[1, 9]).Merge(Type.Missing);

                Excel.Range lastRV = xRV.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeXRV = xRV.get_Range("A1", lastRV);
                Excel.Borders xBordersRV = rangeXRV.Borders;

                xBordersRV.LineStyle = Excel.XlLineStyle.xlContinuous;
                xBordersRV.Weight = 2d;

                xRV.Activate();
                xRV.Application.ActiveWindow.SplitRow = 2;
                xRV.Application.ActiveWindow.FreezePanes = true;

                // Find last row & column
                Excel.Range lastX3RV = xRV.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range rangeX3RV = xRV.get_Range("B1", lastX3RV);

                int lastUsedRowRV = lastX3RV.Row;
                int lastUsedColRV = lastX3RV.Column;

                //Change color in specific row
                string lastCol_B_RV = "B" + (lastUsedRowRV - 1);
                string lastCol_C_RV = "C" + (lastUsedRowRV - 1);
                string lastCol_D_RV = "D" + (lastUsedRowRV - 1);
                string lastCol_E_RV = "E" + (lastUsedRowRV - 1);

                xRV.get_Range("B1", lastCol_B_RV).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRV.get_Range("C1", lastCol_C_RV).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRV.get_Range("D1", lastCol_D_RV).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));
                xRV.get_Range("E1", lastCol_E_RV).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(242, 242, 242));

                xRV.get_Range("A" + (lastUsedRowRV - 1), "I" + (lastUsedRowRV - 1)).Font.Bold = true;
                xRV.get_Range("A" + (lastUsedRowRV), "I" + (lastUsedRowRV)).Font.Bold = true;

                sheetRV.Close(true, Type.Missing, Type.Missing);
                excel_RV.Quit();


                 // - - - - - - -  - -  OPTIN   - - - - - - - - - - -
                Microsoft.Office.Interop.Excel.Application excel_OP = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook sheetOP = excel_OP.Workbooks.Open(spreadSheetOPTLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xOP = excel_OP.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxOP_1 = excel_OP.Sheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxOP_2 = excel_OP.Sheets.get_Item(2) as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet xxOP_3 = excel_OP.Sheets.get_Item(3) as Microsoft.Office.Interop.Excel.Worksheet;

                xOP.Columns.AutoFit();
                xxOP_1.get_Range("A1", "H1").EntireRow.Font.Bold = true;
                xxOP_2.get_Range("A1", "H1").EntireRow.Font.Bold = true;
                xxOP_3.get_Range("A1", "H1").EntireRow.Font.Bold = true;

                sheetOP.Close(true, Type.Missing, Type.Missing);
                excel_OP.Quit();
                

                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;


                // - - - - - - - - -  - - -  -  -  -  -  - - -  - - - - ZIP Excel
                // 1. Attika
                string zFile = @"D:\TransactReports\DCCReports\AtticaDCCYTD_" + timestamp + ".zip";
                string iFile = @"D:\TransactReports\DCCReports\AtticaDCCYTD_" + timestamp + ".xlsx";
                string fname = zFile + " " + iFile;
                WaitProcess(zip(iFile, zFile, false), 120);

                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\AtticaDCCYTD_" + timestamp + ".xlsx", @"\\grat1-fps-sv2o\DutyFreeDCCReport\AtticaDCCYTD_" + timestamp + ".xlsx");
                // System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\AtticaDCCYTD_" + timestamp + ".xlsx", @"\\grat1-dev-ap2t\d$\Test\AtticaDCCYTD_" + timestamp + ".xlsx");

                zFile = " ";
                iFile = " ";

                // 2. Duty Free
                zFile = @"D:\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".zip";
                iFile = @"D:\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".xlsx";

                WaitProcess(zip(iFile, zFile, false), 120);

                //System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".xlsx", @"\\grat1-dev-ap2t\d$\Test\DutyFreeDCCYTD_" + timestamp + ".xlsx");
                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".zip", @"\\grat1-dev-ap2t\d$\DutyFreeW\DutyFreeDCCYTD_" + timestamp + ".zip");
                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DutyFreeDCCYTD_" + timestamp + ".xlsx", @"\\grat1-fps-sv2o\DutyFreeDCCReport\DutyFreeDCCYTD_" + timestamp + ".xlsx");
                

                // 3. Regency
                zFile = @"D:\TransactReports\DCCReports\RegencyDCCYTD_" + timestamp + ".zip";
                iFile = @"D:\TransactReports\DCCReports\RegencyDCCYTD_" + timestamp + ".xlsx";

                WaitProcess(zip(iFile, zFile, false), 120);

                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RegencyDCCYTD_" + timestamp + ".xlsx", @"\\grat1-fps-sv2o\DutyFreeDCCReport\RegencyDCCYTD_" + timestamp + ".xlsx");

                // 4. RVU
                zFile = @"D:\TransactReports\DCCReports\RVUDCCYTD_" + timestamp + ".zip";
                iFile = @"D:\TransactReports\DCCReports\RVUDCCYTD_" + timestamp + ".xlsx";

                WaitProcess(zip(iFile, zFile, false), 120);

                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\RVUDCCYTD_" + timestamp + ".xlsx", @"\\grat1-fps-sv2o\DutyFreeDCCReport\RVUDCCYTD_" + timestamp + ".xlsx");


                // 5. OPTIN
                zFile = @"D:\TransactReports\DCCReports\DCC_OPTIN_" + timestamp + ".zip";
                iFile = @"D:\TransactReports\DCCReports\DCC_OPTIN_" + timestamp + ".xlsx";
                
                WaitProcess(zip(iFile, zFile, false), 120);

                System.IO.File.Move(@"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\DCC_OPTIN_" + timestamp + ".xlsx", @"\\grat1-fps-sv2o\DutyFreeDCCReport\DCC_OPTIN_" + timestamp + ".xlsx");


                // - - - - - - - - - - - - - -  - - - -  -    Mails
                string pathFolder = @"D:\TransactReports\DCCReports";
                string[] resultsAtt = Directory.GetFiles(pathFolder, @"AtticaDCCYTD_" + timestamp + ".zip", SearchOption.TopDirectoryOnly);
                string[] resultsDFr = Directory.GetFiles(pathFolder, @"DutyFreeDCCYTD_" + timestamp + ".zip", SearchOption.TopDirectoryOnly);
                string[] resultsECS = Directory.GetFiles(pathFolder, @"DutyFreeDCCYTD_" + timestamp + "_ECS.xlsx", SearchOption.TopDirectoryOnly);
                string[] resultsReg = Directory.GetFiles(pathFolder, @"RegencyDCCYTD_" + timestamp + ".zip", SearchOption.TopDirectoryOnly);
                string[] resultsRvu = Directory.GetFiles(pathFolder, @"RVUDCCYTD_" + timestamp + ".zip", SearchOption.TopDirectoryOnly);
                string[] resultsOpt = Directory.GetFiles(pathFolder, @"DCC_OPTIN_" + timestamp + ".zip", SearchOption.TopDirectoryOnly);

                pathmail = System.Reflection.Assembly.GetExecutingAssembly().Location;
                pathmail = pathmail.Substring(0, pathmail.LastIndexOf('\\') + 1);

                // ATTIKA
                subjectMailAT  = "Attica DCC report";
                mailBodyAT = "Καλημέρα σας, " + "\n" + "Σας αποστέλλουμε το Attica DCC YTD report το οποίο περιέχει στοιχεία DCC συναλλαγών έως 4/" + data_MonthCurr + "/2019." +
                                "\n\r" +
                                "\n\r" +
                                "--This is an automated message --" + "\n\r" +
                                "-- Euronet Card Services S.A.  --" + "\n\r" +
                                "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                "--      176 74, Kallithea      --" + "\n\r" +
                                "--      Athens, Greece         --" + "\n\r" +
                                "--     tel.+30-210-9478478     --" + "\n\r";
                emailToAtt = "ferlas@atticadps.gr";
                emailCCAtt = "dtiganis@euronetworldwide.com; gathineos@euronetworldwide.com; ikrikelli@euronetworldwide.com; economou@atticadps.gr; paltadakis@atticadps.gr; yfertakis@euronetworldwide.com; ysachami@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToAtt = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCCAtt = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";

                // DUTY FREE - ECS
                subjectMailECS = "DUTY FREE REPORT";
                mailBodyECS = "Hi all, " + "\n" + "The new file DutyFreeDCCYTD_" + timestamp + @".xlsx has been placed in \\grat1-fps-sv2o\DutyFreeDCCReport." +
                                "\n\r" +
                                "\n\r" +
                                "--This is an automated message --" + "\n\r" +
                                "-- Euronet Card Services S.A.  --" + "\n\r" +
                                "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                "--      176 74, Kallithea      --" + "\n\r" +
                                "--      Athens, Greece         --" + "\n\r" +
                                "--     tel.+30-210-9478478     --" + "\n\r";
                emailToECS = "ikrikelli@euronetworldwide.com";
                emailCCECS = "dtiganis@euronetworldwide.com; yfertakis@euronetworldwide.com; ysachami@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToECS = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCCECS = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";

                // DUTY FREE - FTP
                subjectMailDF_FTP = "Euronet ftp account";
                mailBodyDF_FTP = "Hello, " + "\n" + "Could you please upload the file “DutyFreeDCCYTD_" + timestamp + ".zip” to their sftp server?" + "\n\r" + @"You can find it In Grat1-dev-ap2t  in folder D:\DutyFreeW." +
                                 "\n\r" +
                                 "\n\r" +
                                 "--This is an automated message --" + "\n\r" +
                                 "-- Euronet Card Services S.A.  --" + "\n\r" +
                                 "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                 "--      176 74, Kallithea      --" + "\n\r" +
                                 "--      Athens, Greece         --" + "\n\r" +
                                 "--     tel.+30-210-9478478     --" + "\n\r";
                emailToDF_FTP = "mtsafos@euronetworldwide.com; kpapargiriou@euronetworldwide.com; istavropoulos@euronetworldwide.com";
                emailCCDF_FTP = "dtiganis@euronetworldwide.com; devgenidou@euronetworldwide.com; ldorse@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToDF_FTP = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCCDF_FTP = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";

                // DUTY FREE
                subjectMailDF = "DUTY FREE REPORT";
                mailBodyDF = "Dear Mr Kondylis,, " + "\n" + "We have created the Duty Free DCC YTD report until 4/" + data_MonthCurr + "/2019." + "\n" + "File is located in your ftp server “ftp://ftp.dutyfreeshops.gr” in folder Euronet." +
                                "\n\r" +
                                "\n\r" +
                                "--This is an automated message --" + "\n\r" +
                                "-- Euronet Card Services S.A.  --" + "\n\r" +
                                "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                "--      176 74, Kallithea      --" + "\n\r" +
                                "--      Athens, Greece         --" + "\n\r" +
                                "--     tel.+30-210-9478478     --" + "\n\r";
                emailToDF = "lkondylis@DUTYFREESHOPS.GR";
                emailCCDF = "mioannidou@DUTYFREESHOPS.GR; dtiganis@euronetworldwide.com; gathineos@euronetworldwide.com; ikrikelli@euronetworldwide.com; yfertakis@euronetworldwide.com; ysachami@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToDF = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCCDF = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";


                // REGENCY
                subjectMailRE  = "Regency Casino DCC report";
                mailBodyRE = "Καλημέρα σας, " + "\n" + "Σας αποστέλλουμε το Regency DCC YTD report το οποίο περιέχει στοιχεία DCC συναλλαγών έως 4/" + data_MonthCurr + "/2019." +
                                "\n\r" +
                                "\n\r" +
                                "--This is an automated message --" + "\n\r" +
                                "-- Euronet Card Services S.A.  --" + "\n\r" +
                                "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                "--      176 74, Kallithea      --" + "\n\r" +
                                "--      Athens, Greece         --" + "\n\r" +
                                "--     tel.+30-210-9478478     --" + "\n\r";
                emailToREG = "MAngelis@rcmp.regency.gr";
                emailCCREG = "dtiganis@euronetworldwide.com; gathineos@euronetworldwide.com; ikrikelli@euronetworldwide.com; yfertakis@euronetworldwide.com; ysachami@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToREG = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailToREG = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";


                // RVU
                subjectMailRV  = "RVU DCC report";
                mailBodyRV = "Καλημέρα σας, " + "\n" + "Σας αποστέλλουμε το RVU DCC YTD report το οποίο περιέχει στοιχεία DCC συναλλαγών έως 4/" + data_MonthCurr + "/2019." +
                                "\n\r" +
                                "\n\r" +
                                "--This is an automated message --" + "\n\r" +
                                "-- Euronet Card Services S.A.  --" + "\n\r" +
                                "-- Sachtouri 1 & Posidonos Ave.--" + "\n\r" +
                                "--      176 74, Kallithea      --" + "\n\r" +
                                "--      Athens, Greece         --" + "\n\r" +
                                "--     tel.+30-210-9478478     --" + "\n\r";
                emailToRVU = "cmarkopoulou@rvudistribution.com; dimitris@cmdelta.com";
                emailCCRVU = "dtiganis@euronetworldwide.com; gathineos@euronetworldwide.com; ikrikelli@euronetworldwide.com; yfertakis@euronetworldwide.com; ysachami@euronetworldwide.com; akantzos@euronetworldwide.com; lnestoras@euronetworldwide.com";
                //emailToRVU = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCCRVU = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";


                // DCC OPTIN
                subjectMailOPT = "DCC OPTIN report";
                mailBodyOPT = "Dear all, " + "\n" + "Please find attached the monthly report for DCC optin. 4 sheets included (DF, Attica, Regency & RVU). The last month includes transactions only for the first 4 days (1-4)." +
                             "\n\r" + "Regards," + "\n\r" + "Lambros Nestoras"  ;
                emailTo_OPT = "yfertakis@euronetworldwide.com;ysachami@euronetworldwide.com";
                emailCC_OPT = "dtiganis@euronetworldwide.com;akantzos@euronetworldwide.com;devgenidou@euronetworldwide.com";
                //emailTo_OPT = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                //emailCC_OPT = "lnestoras@euronetworldwide.com;lnestoras@euronetworldwide.com";
                
                
               

                emailTo = "lnestoras@euronetworldwide.com;akantzos@euronetworldwide.com";
                //emailTo = "lnestoras@euronetworldwide.com";

                MailClass.SendMailAttach(pathmail, subjectMailAT, mailBodyAT, emailToAtt, emailCCAtt, "", resultsAtt);
                MailClass.SendMailAttach(pathmail, subjectMailECS, mailBodyECS, emailToECS, emailCCECS, "", resultsECS);
                MailClass.SendMail(pathmail, subjectMailDF_FTP, mailBodyDF_FTP, emailToDF_FTP, emailCCDF_FTP, "");
                MailClass.SendMail(pathmail, subjectMailDF, mailBodyDF, emailToDF, emailCCDF, "");
                MailClass.SendMailAttach(pathmail, subjectMailRE, mailBodyRE, emailToREG, emailCCREG, "", resultsReg);
                MailClass.SendMailAttach(pathmail, subjectMailRV, mailBodyRV, emailToRVU, emailCCRVU, "", resultsRvu);
                MailClass.SendMailAttach(pathmail, subjectMailOPT, mailBodyOPT, emailTo_OPT, emailCC_OPT, "", resultsOpt);
                Console.WriteLine("Mail sent. ");


                // Move + Delete files
                string sourceDirectory = @"\\grat1-dev-ap2t\d$\TransactReports\DCCReports\";
                string targetDirectory = @"\\grat1-dev-ap2t\d$\Test\";
                //string targetDirectory = @"\\grat1-fps-sv2o\DutyFreeDCCReport\";

                DirectoryInfo dccSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo dccTarget = new DirectoryInfo(targetDirectory);
                foreach (FileInfo fi in dccSource.GetFiles())
                {
                    //fi.CopyTo(Path.Combine(dccTarget.FullName, fi.Name), true);
                    fi.Delete();
                    //Console.WriteLine("File {0} succesfully copied. ", fi.Name);

                }
            }
            else
            {
                Console.WriteLine("Process aborted due to import file error.");
            }


        }
        public static System.Diagnostics.Process zip(string iFile, string zFile, bool add)
        {
            return zip(iFile, zFile, add, false);
        }
        public static System.Diagnostics.Process zip(string iFile, string zFile, bool add, bool pause)
        {
            if (!add)
            {
                System.IO.FileInfo zipF = new System.IO.FileInfo(zFile);
                if (zipF.Exists)
                    zipF.Delete();
            }
            string zipperArgs = zFile + " " + iFile;
            zipperArgs = zipperArgs.Substring(0, zipperArgs.Length);
            return System.Diagnostics.Process.Start("wzzip", zipperArgs);

        }

        public static int WaitProcess(System.Diagnostics.Process proc, int seconds)
        {
            while ((!proc.HasExited) & (seconds-- > 0))
                System.Threading.Thread.Sleep(1000);
            if (!proc.HasExited)
                throw new Exception(string.Format(
                    "Process {0} has not ended after {1} seconds !!!",
                    proc.ProcessName, seconds));
            return proc.ExitCode;
        }
    }
}
