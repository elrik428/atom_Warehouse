using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Ini;
using SMTPClass;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Runtime;

namespace TransactReportImport
{
 
    class Program
    {
        public static string Path;
        private static bool exitFlag = false;
        private static IniFile ini;
        private static int Retries, TimerInterval;
        public static Timer tmr = new Timer();
        public static Timer delayTmr = new Timer();
        public static string mailBody;
        public static string emailTo, emailCC, emailBCC;
        private static int retr;
        public static bool delayTmrFlag;
        public static bool morning=true;

        private static string TimeStamp()
        {
            return (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        private static void delayTmr_Tick(Object myObject, EventArgs myEventArgs)
        {
            delayTmrFlag = true;
        }

        private static void tmr_Tick(Object myObject, EventArgs myEventArgs)
        {
            StreamWriter sw;
            sw = new StreamWriter("Log.txt", true);

            mailBody += "Attempting to process files ";
             //   + Convert.ToString(count++) + " of " + Convert.ToString(retr-1) + "\r\n\r\n\r\n";
            
            if (Retries == 1)
            {
                exitFlag = true;
            }
            Retries--;

          //  tmr.Enabled = false;
            tmr.Interval = 60000 * TimerInterval;

            string OutputDirectory, TransactReportImportPath, UnsettledReportImportPath;
            DateTime dt = DateTime.Now.AddDays(-1);
            

            string year = string.Format("{0:D2}", dt.Year).Substring(2);
            string month = string.Format("{0:D2}", dt.Month);
            string day = string.Format("{0:D2}", dt.Day);
            
            emailTo = ini.IniReadValue("Email", "To Email List");
            emailCC = ini.IniReadValue("Email", "CC Email List");
            emailBCC = ini.IniReadValue("Email", "BCC Email List");
            TransactReportImportPath = ini.IniReadValue("Directories", "TransactReportImportPath");
            UnsettledReportImportPath = ini.IniReadValue("Directories", "UnsettledReportImportPath");
            OutputDirectory = ini.IniReadValue("Directories", "OutputDirectory");

            /****************************************************/

            sw.WriteLine("BEGIN\n");
            
            try
            {
                /*both morning and noon*/

                SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
                sqConTransactB.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

                sqConTransactB.IntegratedSecurity = true;
                sqConTransactB.InitialCatalog = "ZacReporting";
                sqConTransactB.ConnectTimeout = 0;

                SqlConnection transactConn = new SqlConnection(sqConTransactB.ConnectionString);
                transactConn.Open();

                SqlCommand transactEmpty = new SqlCommand();
                transactEmpty.Connection = transactConn;
                transactEmpty.CommandTimeout = 0;
                transactEmpty.CommandText = "Delete from [abc096].[IMP_TRANSACT_D]";
                transactEmpty.ExecuteNonQuery();

                transactConn.Close();
            }
            catch
            {

            }

            try
            {
                if (morning == true)
                {
                    //System.IO.File.Copy(TransactReportImportPath + "report_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);
                   System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);
                  
                    mailBody += "report_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n";

                    sw.WriteLine("report_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n");
                }
                else/*noon*/
                {
                    //System.IO.File.Copy(TransactReportImportPath + "report2_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);
                    System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report2_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);

                    mailBody += "report2_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n";

                    sw.WriteLine("report2_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n");

                }
                
            }
            catch
            {
                if (morning == true)
                {
                    mailBody += "report_" + "20" + year + month + day + ".csv not found. \r\n";
                    Console.WriteLine("report_" + "20" + year + month + day + ".csv not found. \r\n");
                    sw.WriteLine("report_" + "20" + year + month + day + ".csv not found. \r\n");
                }
                else
                {
                    mailBody += "report2_" + "20" + year + month + day + ".csv not found. \r\n";
                    Console.WriteLine("report2_" + "20" + year + month + day + ".csv not found. \r\n");
                    sw.WriteLine("report2_" + "20" + year + month + day + ".csv not found. \r\n");
                }

                try { sw.Close(); }
                catch { }
                return;
            }

            //try move up 21012015
            //{
            //    /*both morning and noon*/

            //    SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
            //    sqConTransactB.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

            //    sqConTransactB.IntegratedSecurity = true;
            //    sqConTransactB.InitialCatalog = "ZacReporting";
            //    sqConTransactB.ConnectTimeout = 0;

            //    SqlConnection transactConn = new SqlConnection(sqConTransactB.ConnectionString);
            //    transactConn.Open();

            //    SqlCommand transactEmpty = new SqlCommand();
            //    transactEmpty.Connection = transactConn;
            //    transactEmpty.CommandTimeout = 0;
            //    transactEmpty.CommandText = "Delete from [abc096].[IMP_TRANSACT_D]";
            //    transactEmpty.ExecuteNonQuery();

            //    transactConn.Close();
            //}
            //catch
            //{

            //}

            try
            {   /*both morning and noon*/
                
                string pkgLocation;
                Package pkg;
                DTSExecResult pkgResults;
                Microsoft.SqlServer.Dts.Runtime.Application app;
                MyEventListener eventListener = new MyEventListener();

                app = new Microsoft.SqlServer.Dts.Runtime.Application();
                pkgLocation = @".\Package.dtsx";
                pkg = app.LoadPackage(pkgLocation, eventListener);
                pkgResults = pkg.Execute(null, null, eventListener, null, null);
     
                mailBody += "transactreport.csv IMPORTED." + pkgResults.ToString()+ "\r\n";
                sw.WriteLine("transactreport.csv IMPORTED." + pkgResults.ToString() + "\r\n");
                

            }
            catch
            {
                mailBody += "transactreport.csv could not be imported. \r\n";
                Console.WriteLine("transactreport.csv could not be imported. \r\n");
                sw.WriteLine("transactreport.csv could not be imported. \r\n");
                try { sw.Close(); }
                catch { }

                return;

            
            }

            try
            {
                /*both morning and noon*/

                SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
                sqConTransactB.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

                sqConTransactB.IntegratedSecurity = true;
                sqConTransactB.InitialCatalog = "ZacReporting";
                sqConTransactB.ConnectTimeout = 0; 

                SqlConnection transactConn = new SqlConnection(sqConTransactB.ConnectionString);
                transactConn.Open();

                SqlCommand transactUpd = new SqlCommand();
                transactUpd.Connection = transactConn;
                transactUpd.CommandTimeout = 0;
                transactUpd.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] " +
                                           "set amount =0 where msgid='0200'  " +
                                           "and REVERSED='F' " +
                                           "and exists " +
                                           "(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid='0400' and amount=b.amount and " +
                                           "mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='A') ";
                transactUpd.ExecuteNonQuery();


                SqlCommand transactDel = new SqlCommand();
                transactDel.Connection = transactConn;
                transactDel.CommandTimeout = 0;
                transactDel.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where msgid='0400'  " +
                                          "and exists " +
                                          "(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid<>'0400' " +
                                          "and mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='F') ";

                transactDel.ExecuteNonQuery();

                mailBody += "Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n";
                Console.WriteLine("Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n");
                sw.WriteLine("Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n");

                
                transactDel.CommandText = "UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] SET [DESTCOMID]='NET_ALPHA' where [DESTCOMID]='NET_CLBICALPHA'";
                transactDel.ExecuteNonQuery();
 
                transactDel.CommandText = "UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] SET [DESTCOMID]='NET_EBNK' where [DESTCOMID]='NET_CLBICEBNK'";
                transactDel.ExecuteNonQuery();
   
                Console.WriteLine("Update BIC ports.\r\n");


                transactDel.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D] where msgid='0100' and proccode ='080000'";
                transactDel.ExecuteNonQuery();
                
                Console.WriteLine("Delete inquiries.\r\n");
             
                transactDel.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D] where msgid='0200' and proccode ='890000'";
                transactDel.ExecuteNonQuery();
                
                Console.WriteLine("Delete DCC inquiries.\r\n");

                transactDel.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] set msgid='0200',proccode ='000001' "+
                "where destcomid='NET_NTBNLTY' and msgid='0100' and proccode ='000000'";
                transactDel.ExecuteNonQuery();
              
                transactDel.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] set msgid='0200',proccode ='020001' "+
                "where destcomid='NET_NTBNLTY' and msgid='0100' and proccode ='020000'";
                transactDel.ExecuteNonQuery();
              
                Console.WriteLine("Update Go4More.\r\n");

                SqlCommand transactDelnul = new SqlCommand();
                transactDelnul.Connection = transactConn;
                transactDelnul.CommandTimeout = 0;
                transactDelnul.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where mask is null or mask='' ";

                transactDelnul.ExecuteNonQuery();


                Console.WriteLine("delete null pan \r\n");
                

                SqlCommand transactproduct = new SqlCommand();
                transactproduct.Connection = transactConn;
                transactproduct.CommandTimeout = 0;
                transactproduct.CommandText = "update [abc096].[imp_TRansact_D] " +
                    " set productid= " +
                    " case " +
                    " when " +
                    " exists " +
                       " (select * from abc096.products b " +
                        " where " +
                        " (substring(mask,1,6)=cast((substring(b.bin,1,6 )) as dec(22,0)) or" +
                        " substring(mask,1,6)=cast((substring(b.binu,1,6)) as dec(22,0)) or" +
                        " substring(mask,1,6) between cast((substring(b.bin,1,6 )) as dec(22,0)) and" +
                             " cast((substring(b.binu,1,6)) as dec(22,0)) ) )" +
                    " then " +
                        "(select min(b.id) from abc096.products b" +
                        " where " +
                        " productid=cast((substring(b.bin,1,6 )) as dec(22,0)) or" +
                        " productid=cast((substring(b.binu,1,6)) as dec(22,0)) or" +
                        " productid between cast((substring(b.bin,1,6 )) as dec(22,0)) and" +
                             " cast((substring(b.binu,1,6)) as dec(22,0)) )" +
                        " when " +
                        " exists " +
                            " (select * from abc096.products b" +
                            " where" +
                            " (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or" +
                            " substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or" +
                            " substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and" +
                                " cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)" +
                        " then " +
                            " (select min(b.id) from abc096.products b" +
                            " where " +
                            " (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or" +
                            " substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or" +
                            " substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and" +
                                 " cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)" +
                            " when " +
                            " exists " +
                                " (select * from abc096.products b" +
                                " where" +
                                " (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or" +
                                " substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or" +
                                " substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and" +
                                    " cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)" +
                            " then " +
                                " (select min(b.id) from abc096.products b" +
                                " where" +
                                " (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or" +
                                " substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or" +
                                " substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and" +
                                    " cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)" +
                                " when" +
                                " exists" +
                                    " (select * from abc096.products b" +
                                    " where" +
                                    " (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or" +
                                    " substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or" +
                                    " substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and" +
                                        " cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)" +
                                " then" +
                                    " (select min(b.id) from abc096.products b" +
                                    " where" +
                                    " (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or" +
                                    " substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or" +
                                    " substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and" +
                                        " cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)" +
                                    " when" +
                                    " exists" +
                                         " (select * from abc096.products b" +
                                         " where" +
                                         " (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or" +
                                         " substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or" +
                                         " substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and" +
                                            " cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)" +
                                    " then" +
                                         " (select min(b.id) from abc096.products b" +
                                         " where" +
                                         " (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or" +
                                         " substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or" +
                                         " substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and" +
                                             " cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)" +
                                         " when" +
                                         " exists" +
                                              " (select * from abc096.products b" +
                                              " where" +
                                              " (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or" +
                                              " substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or" +
                                              " substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and" +
                                                   " cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)" +
                                         " then " +
                                              " (select min(b.id) from abc096.products b" +
                                              " where" +
                                              " (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or" +
                                              " substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or" +
                                              " substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and" +
                                              " cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)" +
                           " end";
                transactproduct.ExecuteNonQuery();
                transactConn.Close();

                mailBody += "Fixed product in IMP_TRANSACT_D. \r\n";
                Console.WriteLine("Fixed product in IMP_TRANSACT_D. \r\n");
                sw.WriteLine("Fixed product in IMP_TRANSACT_D. \r\n");

            }
            catch
            {

            }


            try
            {
                if (morning==true)
                {
                    System.IO.File.Move(OutputDirectory + "transactreport.csv", OutputDirectory + "Done\\" + "report_" + "20" + year + month + day + ".csv");
//                    System.IO.File.Move(TransactReportImportPath + "report_" + "20" + year + month + day + ".csv", TransactReportImportPath + "Processed\\" + "report_" + "20" + year + month + day + ".csv");
                    System.IO.File.Move(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report_" + "20" + year + month + day + ".csv", @"\\\\192.168.241.252\\TransactReports\\Daily\\" + "Processed\\" + "report_" + "20" + year + month + day + ".csv");

                    mailBody += "report_" + "20" + year + month + day + ".csv DONE" + "\r\n";                 
 
                }

                else /*noon*/
                {

                    System.IO.File.Move(OutputDirectory + "transactreport.csv", OutputDirectory + "Done\\" + "report2_" + "20" + year + month + day + ".csv");
                   // System.IO.File.Move(TransactReportImportPath + "report2_" + "20" + year + month + day + ".csv", TransactReportImportPath + "Processed\\" + "report2_" + "20" + year + month + day + ".csv");
                    System.IO.File.Move(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report2_" + "20" + year + month + day + ".csv", @"\\\\192.168.241.252\\TransactReports\\Daily\\" + "Processed\\" + "report2_" + "20" + year + month + day + ".csv");

                    mailBody += "report2_" + "20" + year + month + day + ".csv DONE" + "\r\n";
                    sw.WriteLine("report2_" + "20" + year + month + day + ".csv DONE" + "\r\n");
                    exitFlag = true;

                    try { sw.Close(); }
                    catch { }

                    return;
                }
                

            }
            catch
            {   try { sw.Close(); }
                catch { }
                return;
            }

            try
            {
                if(morning==true)
                {
                    DateTime dt1 = DateTime.Now;


                    string year1 = string.Format("{0:D2}", dt1.Year).Substring(2);
                    string month1 = string.Format("{0:D2}", dt1.Month);
                    string day1 = string.Format("{0:D2}", dt1.Day);

                    //System.IO.File.Copy(UnsettledReportImportPath + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", OutputDirectory + "unsettled.csv", true);
                    System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Unsettled\\" + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", OutputDirectory + "unsettled.csv", true);

                    mailBody += "\r\nunsettled_" + "20" + year1 + month1 + day1 + ".csv FOUND.\r\n";
                    sw.WriteLine("\r\nunsettled_" + "20" + year1 + month1 + day1 + ".csv FOUND.\r\n");

                    string pkgLocation;
                    Package pkg;
                    DTSExecResult pkgResults;
                    Microsoft.SqlServer.Dts.Runtime.Application app;
                    MyEventListener eventListener = new MyEventListener();

                    app = new Microsoft.SqlServer.Dts.Runtime.Application();
                    pkgLocation = @".\PackageUnsettled.dtsx";
                    pkg = app.LoadPackage(pkgLocation, eventListener);
                    pkgResults = pkg.Execute(null, null, eventListener, null, null);



                    SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
                    sqConTransactB.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

                    sqConTransactB.IntegratedSecurity = true;
                    sqConTransactB.InitialCatalog = "ZacReporting";
                    sqConTransactB.ConnectTimeout = 0;

                    SqlConnection transactConn = new SqlConnection(sqConTransactB.ConnectionString);
                    transactConn.Open();


                    SqlCommand transactDel = new SqlCommand();
                    transactDel.Connection = transactConn;
                    transactDel.CommandTimeout = 0;
                    transactDel.CommandText = "delete from [ZacReporting].[dbo].[V_TransactUnsettledBatches]  where tid<'14000000' or tid>='90000000'";//donna 14102013 hellaspay unsettled should be processed

                    transactDel.ExecuteNonQuery();


                    SqlCommand transactDel1 = new SqlCommand();
                    transactDel1.Connection = transactConn;
                    transactDel1.CommandTimeout = 0;
                    transactDel1.CommandText = "delete from [ZacReporting].[dbo].[V_TransactUnsettledBatches]  where Bank='NET_ALPMOR'";

                    transactDel1.ExecuteNonQuery();

                    transactConn.Close();


                   mailBody += "unsettled_" + "20" + year1 + month1 + day1 + ".csv IMPORTED." + pkgResults.ToString() + "\r\n";

                   System.IO.File.Move(OutputDirectory + "unsettled.csv", OutputDirectory + "unsettled\\" + "unsettled_" + "20" + year1 + month1 + day1 + ".csv");

 //                  System.IO.File.Move(UnsettledReportImportPath + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", UnsettledReportImportPath + "Processed\\" + "report_" + "20" + year + month + day + ".csv");
                   System.IO.File.Move(@"\\\\192.168.241.252\\TransactReports\\Unsettled\\" + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", @"\\\\192.168.241.252\\TransactReports\\Unsettled\\" + "Processed\\" + "report_" + "20" + year + month + day + ".csv");
                   mailBody += "unsettled_" + "20" + year1 + month1 + day1 + ".csv DONE" + "\r\n";
                   sw.WriteLine("unsettled_" + "20" + year1 + month1 + day1 + ".csv DONE" + "\r\n");

                   

                   SqlConnectionStringBuilder sqConTransactC = new SqlConnectionStringBuilder();
                   sqConTransactC.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

                   sqConTransactC.IntegratedSecurity = true;
                   sqConTransactC.InitialCatalog = "ZacReporting";
                   sqConTransactC.ConnectTimeout = 0;

                   SqlConnection transactConn1 = new SqlConnection(sqConTransactC.ConnectionString);
                   transactConn1.Open();

                   SqlCommand transactEmptyBanks = new SqlCommand();
                   transactEmptyBanks.Connection = transactConn1;
                   transactEmptyBanks.CommandTimeout = 0;
                   transactEmptyBanks.CommandText = "Delete from [abc096].[DEACTIVATED_BANKS]";
                   transactEmptyBanks.ExecuteNonQuery();

                   transactConn1.Close();

                   try
                   {
                       //System.IO.File.Copy(UnsettledReportImportPath + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", OutputDirectory + "unsettled.csv", true);
                       System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\DeactivatedBanks\\" + "deact_banks" + year1 + month1 + day1 + ".csv", OutputDirectory + "deact_banks.csv", true);

                       mailBody += "\r\ndeact_banks" + year1 + month1 + day1 + ".csv FOUND.\r\n";
                       sw.WriteLine("\r\ndeact_banks" + year1 + month1 + day1 + ".csv FOUND.\r\n");

                       string pkgLocation1;
                       Package pkg1;
                       DTSExecResult pkgResults1;
                       Microsoft.SqlServer.Dts.Runtime.Application app1;
                       MyEventListener eventListener1 = new MyEventListener();

                       app1 = new Microsoft.SqlServer.Dts.Runtime.Application();
                       pkgLocation1 = @".\Package_deactbanks.dtsx";
                       pkg1 = app1.LoadPackage(pkgLocation1, eventListener1);
                       pkgResults1 = pkg1.Execute(null, null, eventListener1, null, null);


                       mailBody += "deact_banks" + year1 + month1 + day1 + ".csv IMPORTED." + pkgResults1.ToString() + "\r\n";

                       System.IO.File.Move(OutputDirectory + "deact_banks.csv", OutputDirectory + "DeactivatedBanks\\" + "deact_banks" + year1 + month1 + day1 + ".csv");

                       //                  System.IO.File.Move(UnsettledReportImportPath + "unsettled_" + "20" + year1 + month1 + day1 + ".csv", UnsettledReportImportPath + "Processed\\" + "report_" + "20" + year + month + day + ".csv");
                       System.IO.File.Move(@"\\\\192.168.241.252\\TransactReports\\DeactivatedBanks\\" + "deact_banks" + year1 + month1 + day1 + ".csv", @"\\\\192.168.241.252\\TransactReports\\DeactivatedBanks\\" + "Processed\\" + "deact_banks" + year1 + month1 + day1 + ".csv");
                       mailBody += "deact_banks" + year1 + month1 + day1 + ".csv DONE" + "\r\n";
                       sw.WriteLine("deact_banks" + year1 + month1 + day1 + ".csv DONE" + "\r\n");


                   }
                   catch
                   { }


                   SqlConnectionStringBuilder sqConTransactD = new SqlConnectionStringBuilder();
                   sqConTransactD.DataSource = @"10.7.46.2\sql2k5_dev"; //Production

                   sqConTransactD.IntegratedSecurity = true;
                   sqConTransactD.InitialCatalog = "ZacReporting";
                   sqConTransactD.ConnectTimeout = 0;

                   SqlConnection transactConn2 = new SqlConnection(sqConTransactD.ConnectionString);
                   transactConn2.Open();

                   SqlCommand transactEmptyBinRuling = new SqlCommand();
                   transactEmptyBinRuling.Connection = transactConn2;
                   transactEmptyBinRuling.CommandTimeout = 0;
                   transactEmptyBinRuling.CommandText = "Delete from [abc096].[FAILED_BINRULING]";
                   transactEmptyBinRuling.ExecuteNonQuery();

                   transactConn2.Close();

                   try
                   {
                       System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report4_" + "20" + year + month + day + ".csv", OutputDirectory + "failedbinruling.csv", true);

                       mailBody += "report4_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n";

                       sw.WriteLine("report4_" + "20" + year + month + day + ".csv FOUND.\r\n\r\n");


                       string pkgLocation2;
                       Package pkg2;
                       DTSExecResult pkgResults2;
                       Microsoft.SqlServer.Dts.Runtime.Application app2;
                       MyEventListener eventListener2 = new MyEventListener();

                       app2 = new Microsoft.SqlServer.Dts.Runtime.Application();
                       pkgLocation2 = @".\Package_failedbinruling.dtsx";
                       pkg2 = app2.LoadPackage(pkgLocation2, eventListener2);
                       pkgResults2 = pkg2.Execute(null, null, eventListener2, null, null);


                       mailBody += "report4_" + "20" + year + month + day + ".csv IMPORTED." + pkgResults2.ToString() + "\r\n";

                       System.IO.File.Move(OutputDirectory + "failedbinruling.csv", OutputDirectory + "Done\\" + "report4_" + "20" + year + month + day + ".csv");
                       System.IO.File.Move(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report4_" + "20" + year + month + day + ".csv", @"\\\\192.168.241.252\\TransactReports\\Daily\\" + "Processed\\" + "report4_" + "20" + year + month + day + ".csv");

                       mailBody += "report4_" + "20" + year + month + day + ".csv DONE" + "\r\n";
                       sw.WriteLine("report4_" + "20" + year + month + day + ".csv DONE" + "\r\n");
                   }
                   catch {}

                   exitFlag = true;

                   try { sw.Close(); }
                   catch { }
                   return;

                }
                else /*noon*/
                {
                    
                }


            }
            catch
            {
                

            }

            try { sw.Close(); }
            catch { }
           
        }


        class MyEventListener : DefaultEvents
        {
            public override bool OnError(DtsObject source, int errorCode, string subComponent,
              string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {
                // Add application-specific diagnostics here.
                mailBody += "Error in " + source + " / " + subComponent + " : " + description;
                Console.WriteLine("Error in " + source + " / " + subComponent + " : " + description);
                return false;
            }
        }


        static void Main(string[] args)
        {
            delayTmr.Interval = 15000;

            Path = System.Windows.Forms.Application.ExecutablePath;
            Path = Path.Substring(0, Path.LastIndexOf('\\') + 1);

            if (args.Length < 1)
                morning = false;

            else if (args[0] == "MORNING")
                morning = true;
            
                

            ini = new IniFile(Path + "TransactReportImport.ini");
            tmr.Tick += new EventHandler(tmr_Tick);
            delayTmr.Tick += new EventHandler(delayTmr_Tick);

            Retries = Convert.ToInt32(ini.IniReadValue("Schedule", "Retries")) + 1;
            retr = Retries;
            TimerInterval = Convert.ToInt32(ini.IniReadValue("Schedule", "TimerInterval"));
            tmr.Interval = 1;
            tmr.Start();

            while (exitFlag == false)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            tmr.Stop();

            MailClass.SendMail(Path, "Transact Report Import", mailBody, emailTo, emailCC, emailBCC);
        }
    }
}
