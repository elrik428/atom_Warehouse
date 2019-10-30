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
        public static bool weekend=false;
        public static bool run=false;

        private static string TimeStamp()
        {
            return (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        private static void delayTmr_Tick(Object myObject, EventArgs myEventArgs)
        {
            delayTmrFlag = true;
        }

        static void LaunchCommandLineApp(String Date)
        {
            // For the example
            const string ex1 = "D:\\Reporting\\ePOSReports\\AutoReports\\AutoReports\\bin\\Debug\\AutoReports.exe";

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex1;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "ALL "+Date;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
        }

        private static void Import_transactreport(bool onboard,bool casino)
        {   
            SqlConnectionStringBuilder sqCon = new SqlConnectionStringBuilder();
            sqCon.DataSource = @"grat1-dev-ap2t"; //Production

            sqCon.IntegratedSecurity = true;
            sqCon.InitialCatalog = "ZacReporting";
            sqCon.ConnectTimeout = 0;

            SqlConnection Conn = new SqlConnection(sqCon.ConnectionString);

 
            try
            {   /*both morning and noon*/
                
                string pkgLocation;
                Package pkg;
                DTSExecResult pkgResults;
                Microsoft.SqlServer.Dts.Runtime.Application app;
                MyEventListener eventListener = new MyEventListener();

                app = new Microsoft.SqlServer.Dts.Runtime.Application();
                pkgLocation = @"D:\Reporting\\DTSX Packages\\ImportDailyDTSx\\Integration Services Project1\\bin\\Package.dtsx";
                pkg = app.LoadPackage(pkgLocation, eventListener);
                pkgResults = pkg.Execute(null, null, eventListener, null, null);

                mailBody += "transactreport.csv IMPORTED." + pkgResults.ToString() + "\r\n\r\n";
               // sw.WriteLine("transactreport.csv IMPORTED." + pkgResults.ToString() + "\r\n");
                
                if(onboard==true)
                {
                    try //20170609 AEGEAN
                    {  

                        Conn.Open();

                        SqlCommand transactUpd = new SqlCommand();
                        transactUpd.Connection = Conn;
                        transactUpd.CommandTimeout = 0;
                        transactUpd.CommandText = "delete FROM [ZacReporting].[abc096].[IMP_TRANSACT_D] where mid='000000120004600'";
                        transactUpd.ExecuteNonQuery();

                        Conn.Close();

                        app = new Microsoft.SqlServer.Dts.Runtime.Application();
                        pkgLocation = @"D:\\Reporting\\DTSX Packages\\ImportOnboard\\Integration Services Project1\\bin\\Package.dtsx";
                        pkg = app.LoadPackage(pkgLocation, eventListener);
                        pkgResults = pkg.Execute(null, null, eventListener, null, null);
                        mailBody += "onboard transactreport.csv IMPORTED." + pkgResults.ToString() + "\r\n\r\n";

               

                    }
                    catch
                    {
                            mailBody += "onboard_transactreport.csv PROBLEM!!!\r\n";

                    }
                }

                if (casino == true)
                {
                    try //20170831 casino
                    {

                        Conn.Open();

                        SqlCommand transactUpd = new SqlCommand();
                        transactUpd.Connection = Conn;
                        transactUpd.CommandTimeout = 0;
                        transactUpd.CommandText = "delete FROM [ZacReporting].[abc096].[IMP_TRANSACT_D] where mid='000000120005300'";
                        transactUpd.ExecuteNonQuery();

                        Conn.Close();

                        app = new Microsoft.SqlServer.Dts.Runtime.Application();
                        pkgLocation = @"D:\\Reporting\\DTSX Packages\\ImportCasino\\Integration Services Project1\\bin\\Package.dtsx";
                        pkg = app.LoadPackage(pkgLocation, eventListener);
                        pkgResults = pkg.Execute(null, null, eventListener, null, null);
                        mailBody += "casino transactreport.csv IMPORTED." + pkgResults.ToString() + "\r\n\r\n";



                    }
                    catch
                    {
                        mailBody += "casino_transactreport.csv PROBLEM!!!\r\n";

                    }
                }

                try
                {
                    /*both morning and noon*/


                    Conn.Open();

                    SqlCommand transactUpd = new SqlCommand();
                    transactUpd.Connection = Conn;
                    transactUpd.CommandTimeout = 0;
                    transactUpd.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] " +
                                               "set amount =0 where msgid='0200'  " +
                                               "and REVERSED='F' " +
                                               "and exists " +
                                               "(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid='0400' and amount=b.amount and " +
                                               "mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='A') ";
                    transactUpd.ExecuteNonQuery();


                    SqlCommand transactDel = new SqlCommand();
                    transactDel.Connection = Conn;
                    transactDel.CommandTimeout = 0;
                    transactDel.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where msgid='0400'  " +
                                              "and exists " +
                                              "(select * from [ZacReporting].[abc096].[IMP_TRANSACT_D]  B where b.msgid<>'0400' " +
                                              "and mask=b.mask and mid=b.mid and tid=b.tid and DESTCOMID=B.DESTCOMID AND B.REVERSED='F') ";

                    transactDel.ExecuteNonQuery();

                    mailBody += "Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n";
                    Console.WriteLine("Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n");
                   // sw.WriteLine("Updated reversed and deleted reversals from IMP_TRANSACT_D. \r\n");


                    transactDel.CommandText = "UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] SET [DESTCOMID]='NET_ALPHA' where [DESTCOMID]='NET_CLBICALPHA' or [DESTCOMID]='NET_BICALPHA'";
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
                    "where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='000000'";
                    transactDel.ExecuteNonQuery();
                  
                    transactDel.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] set msgid='0200',proccode ='020001' "+
                    "where (destcomid='NET_NTBNLTY' or destcomid='NET_PBGLTY') and msgid='0100' and proccode ='020000'";
                    transactDel.ExecuteNonQuery();
                  
                    Console.WriteLine("Update Go4More and Yellow.\r\n");

                    SqlCommand transactDelnul = new SqlCommand();
                    transactDelnul.Connection = Conn;
                    transactDelnul.CommandTimeout = 0;
                    transactDelnul.CommandText = "delete from [ZacReporting].[abc096].[IMP_TRANSACT_D]  where mask is null or mask='' ";

                    transactDelnul.ExecuteNonQuery();

                    transactDelnul.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D]  set mask='000000' where productid is null ";

                    transactDelnul.ExecuteNonQuery();

                    Console.WriteLine("delete null pan \r\n");
                    
                    // ln 20170922      ----------    start    ---------
                            SqlCommand UpdatePrdctID = new SqlCommand();
                            UpdatePrdctID.Connection = Conn;
                            UpdatePrdctID.CommandTimeout = 0;
                            UpdatePrdctID.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D] set productid = 0 ";
                            UpdatePrdctID.ExecuteNonQuery();
                            Console.WriteLine("Update productid = 0 \r\n");
                   // ln 20170922      ----------     end     ---------

                    SqlCommand transactproduct = new SqlCommand();
                    transactproduct.Connection = Conn;
                    transactproduct.CommandTimeout = 0;

                    // ln 20170922      ----------    start    ---------
                    transactproduct.CommandText = "update zacreporting.[abc096].[imp_TRansact_D] " +
                                                    "set productid = ( select   top 1 b.id " +
                                                    "from  zacreporting.abc096.Products b   " +
                                                    "where substring(mask,1,6) = b.bin ); ";

                    // ln 20170922      ----------     end     ---------

                    //     LN 20170922 Commented     ----------    start    ---------
                    //transactproduct.CommandText = "update [abc096].[imp_TRansact_D] " +
                    //    " set productid= " +
                    //    " case " +
                    //    " when " +
                    //    " exists " +
                    //       " (select * from abc096.products b " +
                    //        " where " +
                    //        " (substring(mask,1,6)=cast((substring(b.bin,1,6 )) as dec(22,0)) or" +
                    //        " substring(mask,1,6)=cast((substring(b.binu,1,6)) as dec(22,0)) or" +
                    //        " substring(mask,1,6) between cast((substring(b.bin,1,6 )) as dec(22,0)) and" +
                    //             " cast((substring(b.binu,1,6)) as dec(22,0)) ) )" +
                    //    " then " +
                    //        "(select min(b.id) from abc096.products b" +
                    //        " where " +
                    //        " substring(mask,1,6)=cast((substring(b.bin,1,6 )) as dec(22,0)) or" +
                    //        " substring(mask,1,6)=cast((substring(b.binu,1,6)) as dec(22,0)) or" +
                    //        " substring(mask,1,6) between cast((substring(b.bin,1,6 )) as dec(22,0)) and" +
                    //             " cast((substring(b.binu,1,6)) as dec(22,0)) )" +
                    //        " when " +
                    //        " exists " +
                    //            " (select * from abc096.products b" +
                    //            " where" +
                    //            " (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or" +
                    //            " substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or" +
                    //            " substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and" +
                    //                " cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)" +
                    //        " then " +
                    //            " (select min(b.id) from abc096.products b" +
                    //            " where " +
                    //            " (substring(mask,1,5)=cast((substring(b.bin,1,5 )) as dec(22,0)) or" +
                    //            " substring(mask,1,5)=cast((substring(b.binu,1,5)) as dec(22,0)) or" +
                    //            " substring(mask,1,5) between cast((substring(b.bin,1,5)) as dec(22,0)) and" +
                    //                 " cast((substring(b.binu,1,5)) as dec(22,0))) and len(b.bin)=5)" +
                    //            " when " +
                    //            " exists " +
                    //                " (select * from abc096.products b" +
                    //                " where" +
                    //                " (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or" +
                    //                " substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or" +
                    //                " substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and" +
                    //                    " cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)" +
                    //            " then " +
                    //                " (select min(b.id) from abc096.products b" +
                    //                " where" +
                    //                " (substring(mask,1,4)=cast((substring(b.bin,1,4 )) as dec(22,0)) or" +
                    //                " substring(mask,1,4)=cast((substring(b.binu,1,4)) as dec(22,0)) or" +
                    //                " substring(mask,1,4) between cast((substring(b.bin,1,4)) as dec(22,0)) and" +
                    //                    " cast((substring(b.binu,1,4)) as dec(22,0))) and len(b.bin)=4)" +
                    //                " when" +
                    //                " exists" +
                    //                    " (select * from abc096.products b" +
                    //                    " where" +
                    //                    " (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or" +
                    //                    " substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or" +
                    //                    " substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and" +
                    //                        " cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)" +
                    //                " then" +
                    //                    " (select min(b.id) from abc096.products b" +
                    //                    " where" +
                    //                    " (substring(mask,1,3)=cast((substring(b.bin,1,3 )) as dec(22,0)) or" +
                    //                    " substring(mask,1,3)=cast((substring(b.binu,1,3)) as dec(22,0)) or" +
                    //                    " substring(mask,1,3) between cast((substring(b.bin,1,3)) as dec(22,0)) and" +
                    //                        " cast((substring(b.binu,1,3)) as dec(22,0))) and len(b.bin)=3)" +
                    //                    " when" +
                    //                    " exists" +
                    //                         " (select * from abc096.products b" +
                    //                         " where" +
                    //                         " (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or" +
                    //                         " substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or" +
                    //                         " substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and" +
                    //                            " cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)" +
                    //                    " then" +
                    //                         " (select min(b.id) from abc096.products b" +
                    //                         " where" +
                    //                         " (substring(mask,1,2)=cast((substring(b.bin,1,2 )) as dec(22,0)) or" +
                    //                         " substring(mask,1,2)=cast((substring(b.binu,1,2)) as dec(22,0)) or" +
                    //                         " substring(mask,1,2) between cast((substring(b.bin,1,2)) as dec(22,0)) and" +
                    //                             " cast((substring(b.binu,1,2)) as dec(22,0))) and len(b.bin)=2)" +
                    //                         " when" +
                    //                         " exists" +
                    //                              " (select * from abc096.products b" +
                    //                              " where" +
                    //                              " (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or" +
                    //                              " substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or" +
                    //                              " substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and" +
                    //                                   " cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)" +
                    //                         " then " +
                    //                              " (select min(b.id) from abc096.products b" +
                    //                              " where" +
                    //                              " (substring(mask,1,1)=cast((substring(b.bin,1,1 )) as dec(22,0)) or" +
                    //                              " substring(mask,1,1)=cast((substring(b.binu,1,1)) as dec(22,0)) or" +
                    //                              " substring(mask,1,1) between cast((substring(b.bin,1,1)) as dec(22,0)) and" +
                    //                              " cast((substring(b.binu,1,1)) as dec(22,0))) and len(b.bin)=1)" +
                    //           " end";
                    //     LN 20170922 Commented     ----------    end    ---------

                    transactproduct.ExecuteNonQuery();

                    // ln 20170922      ----------    start    ---------
                    SqlCommand UpdZeroPrdctID = new SqlCommand();
                    UpdZeroPrdctID.Connection = Conn;
                    UpdZeroPrdctID.CommandTimeout = 0;
                    UpdZeroPrdctID.CommandText = "update zacreporting.[abc096].[imp_TRansact_D] " +
                    "set productid =  " +
                    "case " +
                        "when substring(mask,1,2) >= '40' and substring(mask,1,2) <= '49' then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                            "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                            "group by b.id,b.bin) " +
                        "when substring(mask,1,2) >= '60' and substring(mask,1,2) <= '69' then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                            "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                            "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '23'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '24'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '25'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '26'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '30'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    " group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '34'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '35'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '36'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '37'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '38'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '50'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '51'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '52'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '53'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '54'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '55'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '56'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '57'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "when substring(mask,1,2) = '58'	 then  (select b.id from zacreporting.[abc096].[imp_TRansact_D] c " +
                                                                    "inner join zacreporting.dbo.Products b on substring(c.mask,1,2) = b.bin " +
                                                                    "group by b.id,b.bin) " +
                        "end " +
                    "where productid = '0' ";
                    UpdZeroPrdctID.ExecuteNonQuery();
                    Console.WriteLine("Update productid for '0' \r\n");
                    // ln 20170922      ----------    end    ---------
                
                    Conn.Close();

                    mailBody += "Fixed product in IMP_TRANSACT_D. \r\n";
                    Console.WriteLine("Fixed product in IMP_TRANSACT_D. \r\n");
                   // sw.WriteLine("Fixed product in IMP_TRANSACT_D. \r\n");

                }
                catch
                {   mailBody += "transactreport.csv could not be imported. \r\n";
                    Console.WriteLine("transactreport.csv could not be imported. \r\n");
                   // sw.WriteLine("transactreport.csv could not be imported. \r\n");
                   // try { sw.Close(); }
                   // catch { }
                }
            }
            catch
            {       mailBody += "transactreport.csv could not be imported. \r\n";
                    Console.WriteLine("transactreport.csv could not be imported. \r\n");
                    //sw.WriteLine("transactreport.csv could not be imported. \r\n");
                    //try { sw.Close(); }
                    //catch { }
            }
        }

        private static void tmr_Tick(Object myObject, EventArgs myEventArgs)
        {
            StreamWriter sw;
            sw = new StreamWriter("Log.txt", true);

            mailBody += "Attempting to process files ";
             //   + Convert.ToString(count++) + " of " + Convert.ToString(retr-1) + "\r\n\r\n\r\n";
            
            //if (Retries == 1)
            //{
                exitFlag = true;
            //}
            //Retries--;

          //  tmr.Enabled = false;
            tmr.Interval = 60000 * TimerInterval;

            string OutputDirectory, TransactReportImportPath, UnsettledReportImportPath;

            
            emailTo = ini.IniReadValue("Email", "To Email List");
            emailCC = ini.IniReadValue("Email", "CC Email List");
            emailBCC = ini.IniReadValue("Email", "BCC Email List");
            TransactReportImportPath = ini.IniReadValue("Directories", "TransactReportImportPath");
            UnsettledReportImportPath = ini.IniReadValue("Directories", "UnsettledReportImportPath");
            OutputDirectory = ini.IniReadValue("Directories", "OutputDirectory");

            SqlConnectionStringBuilder sqCon = new SqlConnectionStringBuilder();
            sqCon.DataSource = @"grat1-dev-ap2t"; //Production

            sqCon.IntegratedSecurity = true;
            sqCon.InitialCatalog = "ZacReporting";
            sqCon.ConnectTimeout = 0;

            SqlConnection Conn = new SqlConnection(sqCon.ConnectionString);

            /****************************************************/

            sw.WriteLine("BEGIN\n");
            DateTime dt_today = DateTime.Now;

            string year_r5 = string.Format("{0:D2}", dt_today.Year).Substring(2);
            string month_r5  = string.Format("{0:D2}", dt_today.Month);
            string day_r5  = string.Format("{0:D2}", dt_today.Day);
            
            try
            {

                //System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv", OutputDirectory + "transactreport.csv", true);
                System.IO.File.Copy(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv", OutputDirectory + "transactreport.csv", true);
                  
                mailBody += "report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv FOUND.\r\n";

                sw.WriteLine("report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv FOUND.\r\n");

                
            }
            catch
            {
                mailBody += "report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv  not found. \r\n";
                Console.WriteLine("report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv  not found. \r\n");
                sw.WriteLine("report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv  not found. \r\n");
                sw.Close();
                return;
            }
            
            Import_transactreport(false,false);

            try
            {
               
                    Conn.Open();

                    SqlCommand InsertUPD = new SqlCommand();
                    InsertUPD.Connection = Conn;
                    InsertUPD.CommandTimeout = 0;
                    InsertUPD.CommandText = "INSERT INTO [abc096].[IMP_TRANSACT_D_upd] SELECT * FROM [abc096].[IMP_TRANSACT_D] ";
                    InsertUPD.ExecuteNonQuery();

                    mailBody += "report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv inserted into IMP_TRANSACT_D_upd. \r\n";
                    Console.WriteLine("report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv inserted into IMP_TRANSACT_D_upd. \r\n");
                    sw.WriteLine("report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv inserted into IMP_TRANSACT_D_upd. \r\n");
                    

                    Conn.Close();
                
                    System.IO.File.Delete(OutputDirectory + "transactreport.csv");
                    System.IO.File.Delete(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report5_" + "20" + year_r5  + month_r5  + day_r5  + ".csv");


            }
            catch
            {
                    mailBody += "report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv problem. \r\n";
                    Console.WriteLine("report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv problem. \r\n");
                    sw.WriteLine("report5_" + "20" + year_r5 + month_r5 + day_r5 + ".csv problem. \r\n");

            }   

            int i=1;
            /*weekend*/
            do
            {
                DateTime dt_i = DateTime.Now.AddDays(-i);//yesterday
                string hdate = dt_i.ToString("dd-MM-yyyy");            
                string rdate = dt_i.ToString("dd/MM/yyyy");            

                DateTime dt_ri = DateTime.Now.AddDays(-i-1);//2 days ago

                string year = string.Format("{0:D2}", dt_ri.Year).Substring(2);
                string month = string.Format("{0:D2}", dt_ri.Month);
                string day = string.Format("{0:D2}", dt_ri.Day);
 
                Conn.Open();

                SqlCommand IsHoliday = new SqlCommand();
                IsHoliday.Connection = Conn;
                IsHoliday.CommandTimeout = 0;
                IsHoliday.CommandText = "Select * from [dbo].[BankHolidays] where [Holiday]='"+ hdate +"'";
                SqlDataReader reader= IsHoliday.ExecuteReader();

                if(reader.Read())
                {
                    weekend=true;
                }
                else if(dt_i.DayOfWeek==System.DayOfWeek.Saturday || dt_i.DayOfWeek==System.DayOfWeek.Sunday) 
                {

                    weekend=true;
                }
                else 
                    weekend=false;

                reader.Close();
                Conn.Close();

                if(weekend==true)
                {
                    run=true;
                    try
                    {

                    //System.IO.File.Copy(@"\\\\192.168.241.252\\TransactReports\\Daily\\" + "report2_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);
                    System.IO.File.Copy(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report2_" + "20" + year + month + day + ".csv", OutputDirectory + "transactreport.csv", true);

                    mailBody += "report2_" + "20" + year + month + day + ".csv FOUND.\r\n";

                    sw.WriteLine("report2_" + "20" + year + month + day + ".csv FOUND.\r\n");
                    
                    }
                    catch
                    {
                    
                            mailBody += "report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D NOT ready. \r\n";
                            Console.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");
                            sw.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");

                    }

                    try //20170609 AEGEAN
                    {
                        Console.WriteLine("--ONBOARD--\r\n");
                        mailBody += "--ONBOARD--\r\n";

                        try
                        {
                            System.IO.File.Delete(OutputDirectory + "onboard_transactreport.csv");
                        }
                        catch
                        {
                        }

                    System.IO.File.Copy(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report_onboard_" + "20" + year + month + day + ".csv", OutputDirectory + "onboard_transactreport.csv", true);

                    mailBody += "report_onboard_" + "20" + year + month + day + ".csv FOUND.\r\n";

                    sw.WriteLine("report_onboard_" + "20" + year + month + day + ".csv FOUND.\r\n");
                    
                    }
                    catch
                    {
                    
                            mailBody += "report_onboard_" + "20" + year + month + day + ".csv IMP_TRANSACT_D NOT ready. \r\n";
                            Console.WriteLine("report_onboard_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");
                            sw.WriteLine("report_onboard_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");

                    }

                    try //20170831 casino
                    {
                        Console.WriteLine("--CASINO--\r\n");
                        mailBody += "--CASINO--\r\n";

                        try
                        {
                            System.IO.File.Delete(OutputDirectory + "casino_transactreport.csv");
                        }
                        catch
                        {
                        }

                        System.IO.File.Copy(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report_casino_" + "20" + year + month + day + ".csv", OutputDirectory + "casino_transactreport.csv", true);

                        mailBody += "report_casino_" + "20" + year + month + day + ".csv FOUND.\r\n";

                        sw.WriteLine("report_casino_" + "20" + year + month + day + ".csv FOUND.\r\n");

                    }
                    catch
                    {

                        mailBody += "report_casino_" + "20" + year + month + day + ".csv IMP_TRANSACT_D NOT ready. \r\n";
                        Console.WriteLine("report_casino_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");
                        sw.WriteLine("report_casino_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");

                    }

                    try
                    {
                        Conn.Open();
                        //clean imp_transact_d from previous loop

                        SqlCommand InsertKEEP = new SqlCommand();
                        InsertKEEP.Connection = Conn;
                        InsertKEEP.CommandTimeout = 0;

                        InsertKEEP.CommandText ="DELETE FROM [ZacReporting].[abc096].[IMP_TRANSACT_D] ";
                        InsertKEEP.ExecuteNonQuery();

                        
                        Conn.Close();


                    }
                    catch
                    {

                    }
                               
                    Import_transactreport(true,true);

                    try
                    {
                   
                        Conn.Open();

                        SqlCommand UpdateUPD = new SqlCommand();
                        UpdateUPD.Connection = Conn;
                        UpdateUPD.CommandTimeout = 0;

                        //commented 20170609 AEGEAN
                        //UpdateUPD.CommandText = "delete FROM [ZacReporting].[abc096].[IMP_TRANSACT_D]where mid='000000120004600' and bdtstamp>'20"+year+"-"+month+"-"+day+" 23:59:59.000'";
                        //UpdateUPD.ExecuteNonQuery();
                        //Console.WriteLine("remove AEGEAN current day \r\n");

                        UpdateUPD.CommandText = "update [ZacReporting].[abc096].[IMP_TRANSACT_D_upd] "+
                        "SET TCODE =(SELECT TCODE from [ZacReporting].[abc096].[IMP_TRANSACT_D] WHERE "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].MID=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].MID and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].TID=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].TID and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].MASK=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].MASK and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].AMOUNT=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].AMOUNT and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].PROCCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].PROCCODE and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].ORGSYSTAN=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].ORGSYSTAN and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].DESTCOMID=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].DESTCOMID and "+
                        "[ZacReporting].[abc096].[IMP_TRANSACT_D].AUTHCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].AUTHCODE and "+
					    "[ZacReporting].[abc096].[IMP_TRANSACT_D].MSGID IN ('0200','0220')) "+
                        "where [ZacReporting].[abc096].[IMP_TRANSACT_D_upd].MSGID='0320' ";
                        UpdateUPD.ExecuteNonQuery();

                        UpdateUPD.CommandText = "UPDATE [ZacReporting].[abc096].[IMP_TRANSACT_D] "+
                        "SET BTBL='FRWDAR', "+
                        "BTCODE=TCODE, "+
                        "PROCBATCH=(SELECT PROCBATCH from [ZacReporting].[abc096].[IMP_TRANSACT_D_upd] WHERE [ZacReporting].[abc096].[IMP_TRANSACT_D].TCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].TCODE), "+
                        "ePOSBATCH=(SELECT ePOSBATCH from [ZacReporting].[abc096].[IMP_TRANSACT_D_upd] WHERE [ZacReporting].[abc096].[IMP_TRANSACT_D].TCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].TCODE), "+
                        "BPOSDATA=(SELECT BPOSDATA from [ZacReporting].[abc096].[IMP_TRANSACT_D_upd] WHERE [ZacReporting].[abc096].[IMP_TRANSACT_D].TCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].TCODE), "+
                        "DTSTAMP_INSERT=(SELECT DTSTAMP_INSERT from [ZacReporting].[abc096].[IMP_TRANSACT_D_upd] WHERE [ZacReporting].[abc096].[IMP_TRANSACT_D].TCODE=[ZacReporting].[abc096].[IMP_TRANSACT_D_upd].TCODE) "+
                        "where tcode in (select tcode from [ZacReporting].[abc096].[IMP_TRANSACT_D_upd]) ";
                        UpdateUPD.ExecuteNonQuery();

                        mailBody += "report2_" + "20" + year+ month + day + ".csv IMP_TRANSACT_D updated. \r\n";
                        Console.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D updated. \r\n");
                        sw.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D updated. \r\n");

     
                        Conn.Close();

                        System.IO.File.Delete(OutputDirectory + "transactreport.csv");
                        System.IO.File.Delete(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report2_" + "20" + year + month + day + ".csv");
 
                        System.IO.File.Delete(OutputDirectory + "onboard_transactreport.csv");
                        System.IO.File.Delete(@"\\\\grat1-dev-ap2t\\d$\\TransactReports\\Daily\\" + "report_onboard_" + "20" + year + month + day + ".csv");

                    }
                    catch
                    {
                            mailBody += "report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D NOT ready. \r\n";
                            Console.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");
                            sw.WriteLine("report2_" + "20" + year + month + day + ".csv IMP_TRANSACT_D  NOT ready. \r\n");

                    }   

                    //Date="11/10/2015";//Day that the report would run-it was weekend
                    LaunchCommandLineApp(rdate);

                    i++;
                   
                }


            }
            while(weekend==true);
            /*...weekend*/

            if(run==true)
            {

                DateTime dt_2 = DateTime.Now.AddDays(-2);//2 day ago-last run
                string dt=dt_2.ToString("yyyy/MM/dd 00:00:00.000");

                Conn.Open();
                SqlCommand UpdateCCL = new SqlCommand();
                UpdateCCL.Connection = Conn;
                UpdateCCL.CommandTimeout = 0;
                UpdateCCL.CommandText ="UPDATE [abc096].[ReportCycles] SET DateF='"+dt+"', DateT='"+dt+"' WHERE ID=1";
                UpdateCCL.ExecuteNonQuery();
                Conn.Close();
            }

            exitFlag = true;
            sw.Close();
            return;
           
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

            SqlConnectionStringBuilder sqCon = new SqlConnectionStringBuilder();
            sqCon.DataSource = @"grat1-dev-ap2t"; //Production
            sqCon.IntegratedSecurity = true;
            sqCon.InitialCatalog = "ZacReporting";
            sqCon.ConnectTimeout = 0;

            SqlConnection Conn = new SqlConnection(sqCon.ConnectionString);


            try
            {
                /*both morning and noon*/

                Conn.Open();

                SqlCommand transactEmpty = new SqlCommand();
                transactEmpty.Connection = Conn;
                transactEmpty.CommandTimeout = 0;
                transactEmpty.CommandText = "Delete from [abc096].[IMP_TRANSACT_D]";
                transactEmpty.ExecuteNonQuery();

                SqlCommand transactEmptyBanks = new SqlCommand();
                transactEmptyBanks.Connection = Conn;
                transactEmptyBanks.CommandTimeout = 0;
                transactEmptyBanks.CommandText = "Delete from [abc096].[DEACTIVATED_BANKS]";
                transactEmptyBanks.ExecuteNonQuery();

                SqlCommand transactEmptyBinRuling = new SqlCommand();
                transactEmptyBinRuling.Connection = Conn;
                transactEmptyBinRuling.CommandTimeout = 0;
                transactEmptyBinRuling.CommandText = "Delete from [abc096].[FAILED_BINRULING]";
                transactEmptyBinRuling.ExecuteNonQuery();

                /*weekend*/
                transactEmpty.CommandText = "Delete from [abc096].[IMP_TRANSACT_D_upd]";
                transactEmpty.ExecuteNonQuery();
                /*...weekend*/

                Conn.Close();

            }
            catch
            {   mailBody+="Unable to connect to ZacReporting database";
                exitFlag=true;
            }
                        
            try
            {
                DateTime dt_today = DateTime.Now;
                string hdate = dt_today.ToString("dd-MM-yyyy");

                Conn.Open();


                SqlCommand IsHoliday = new SqlCommand();
                IsHoliday.Connection = Conn;
                IsHoliday.CommandTimeout = 0;
                IsHoliday.CommandText = "Select * from [dbo].[BankHolidays] where [Holiday]='"+ hdate +"'";
                SqlDataReader reader= IsHoliday.ExecuteReader();

                if(reader.Read())
                {
                    weekend=true;
                }
                else if(dt_today.DayOfWeek==System.DayOfWeek.Saturday || dt_today.DayOfWeek==System.DayOfWeek.Sunday) 
                {

                    weekend=true;
                }

                reader.Close();
                Conn.Close();

                if(weekend==true)//do not run anything on weekend
                    return;
            }
            catch
            {
                mailBody+="Unable to connect to ZacReporting database";
                exitFlag =true;
            }
            /*...weekend*/

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

            if(run==true)
                MailClass.SendMail(Path, "Transact Report Import", mailBody, emailTo, emailCC, emailBCC);
        }
    }
}
