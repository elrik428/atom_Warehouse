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
using System.Data.Odbc;
using System.Data.OleDb;
using System.Threading;

namespace iTunes_Auto_Reports
{

    class Program
    {
        public static string Path;
        private static bool exitFlag = false;
        private static IniFile ini;
        private static int Retries, TimerInterval;
        public static System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer delayTmr = new System.Windows.Forms.Timer();
        public static string mailBody;
        public static string emailTo, emailCC, emailBCC;
        private static int retr;
        public static bool delayTmrFlag;
        public static bool morning=true;

        //public static System.Data.Odbc.OdbcConnection conn=new OdbcConnection("DSN=iTunes");

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

            string OutputDirectory;//, iTunesReportImportPath;

            DateTime dt_1 = DateTime.Now.AddDays(-1);

            string year_1 = string.Format("{0:D2}", dt_1.Year).Substring(2);
            string month_1 = string.Format("{0:D2}", dt_1.Month);
            string day_1 = string.Format("{0:D2}", dt_1.Day);

            DateTime dt_7 = DateTime.Now.AddDays(-7);

            string year_7 = string.Format("{0:D2}", dt_7.Year).Substring(2);
            string month_7 = string.Format("{0:D2}", dt_7.Month);
            string day_7 = string.Format("{0:D2}", dt_7.Day);

            //if (String.Compare(month_1, month_7) != 0)
            //{
            //    year_7 = String.Copy(year_1);
            //    month_7 = String.Copy(month_1);
            //    day_7 = "01";
            //}

            emailTo = ini.IniReadValue("Email", "To Email List");
            emailCC = ini.IniReadValue("Email", "CC Email List");
            emailBCC = ini.IniReadValue("Email", "BCC Email List");
            //iTunesReportImportPath = ini.IniReadValue("Directories", "iTunesReportImportPath");
            OutputDirectory = ini.IniReadValue("Directories", "OutputDirectory");

            /****************************************************/

            sw.WriteLine("BEGIN\n");
            SqlConnection transactConn;

            try
            {
                SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
                //sqConTransactB.DataSource = @"grat1-mis-ap1p"; //Production
                sqConTransactB.DataSource = @"10.7.17.11"; //Production 20170531

                
                sqConTransactB.IntegratedSecurity = true; //20170531
                /*sqConTransactB.IntegratedSecurity =false; windows auth does not work well on this server
                sqConTransactB.UserID="EPOS_Support_SQL";
                sqConTransactB.Password="1q2w3e4r5t6y!@#";*/
                sqConTransactB.InitialCatalog = "iTunes";
                sqConTransactB.ConnectTimeout = 0;

                transactConn = new SqlConnection(sqConTransactB.ConnectionString);
                transactConn.Open();

                if(transactConn.State!=System.Data.ConnectionState.Open)
                {
                     exitFlag=true;
                     mailBody+="Connection Error!";
                     sw.WriteLine("Connection Error!\n");
                     return;
                }
            }
            catch
            {
                 exitFlag=true;
                 mailBody+="Connection Error!";
                 sw.WriteLine("Connection Error!\n");
                 return;
            }

            try
            {
  //              System.IO.File.Copy(@"\\\\192.168.241.252\\iTunes\\Weekly\\" + "greece_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv", OutputDirectory + "greece_transaction_list.csv", true);
                System.IO.File.Copy(@"\\\\10.7.17.11\\Storage\\TransactReports\\iTunes\\Weekly\\" + "greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv", OutputDirectory + "greece_transaction_list.csv", true);

                SqlCommand transactEmpty = new SqlCommand();
                transactEmpty.Connection = transactConn;
                transactEmpty.CommandTimeout = 0;
                transactEmpty.CommandText = "Delete from [dbo].[TRANSACTIONS]";
                transactEmpty.ExecuteNonQuery();

                //transactConn.Close();

                string pkgLocation;
                Package pkg;
                DTSExecResult pkgResults;
                Microsoft.SqlServer.Dts.Runtime.Application app;
                MyEventListener eventListener = new MyEventListener();

                app = new Microsoft.SqlServer.Dts.Runtime.Application();
                pkgLocation = @"D:\\Reporting\\DTSX Packages\\iTunes_Import\\iTunes_Import\\bin\\Package.dtsx";
                pkg = app.LoadPackage(pkgLocation, eventListener);
                pkgResults = pkg.Execute(null, null, eventListener, null, null);

                System.IO.File.Delete(OutputDirectory + "greece_transaction_list.csv");

                mailBody += "greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv FOUND.\r\n\r\n";

                sw.WriteLine("greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv FOUND.\r\n\r\n");

                mailBody += "greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv IMPORTED." + pkgResults.ToString() + "\r\n";
                    
            }
            catch   
            {
                mailBody += "greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv PROBLEM.\r\n\r\n";
                Console.WriteLine("greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv PROBLEM.\r\n\r\n");
                sw.WriteLine("greece_weekly_transaction_list_" + "20" + year_7 + month_7 + day_7 + "_" + "20" + year_1 + month_1 + day_1 + ".csv PROBLEM.\r\n\r\n");


                try { sw.Close();transactConn.Close(); }
                catch { }
                return;
            }



            try
            {
                String timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                //DateTime result = dateTimePicker1.Value.Date;
                //String from = result.ToString("yyyy-MM-dd");
                //result = dateTimePicker2.Value.Date;
                //String to = result.ToString("yyyy-MM-dd");
                string mailBodydefault = "Σας επισυνάπτουμε την αναφορά " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1 + ".\n\r" +
                    "Παρακαλούμε όπως εκδώσετε Τιμολόγιο Παροχής Υπηρεσιών με το ποσό που αναφέρεται στην συνημμένη αναφορά  (προμήθειά σας - σύμφωνα με την μεταξύ μας σύμβαση)," +
                    "και πληρωμή του ποσού της Euronet (Euronet Payable) στον παρακάτω λογαριασμό: \n\r\n\r" +
                    "Τράπεζα Πειραιώς - IBAN GR81 0172 0110 0050 1102 4189 937\n\r\n\r" +//GR 70 0172 0110 0050 1103 5043 853\n\r\n\r" +
                    "Με εκτίμηση,\n\r" +

                    "Euronet Card Services A.E.\n\r" +
                    "Yπηρεσίες Πληροφορικής\n\r" +
                    "ΣΑΧΤΟΥΡΗ 1 ΚΑΛΛΙΘΕΑ Τ.Κ. 176 74\n\r" +
                    "ΑΦΜ : 999311933 ΔΟΥ :  ΦΑΕ ΠΕΙΡΑΙΑ\n\r" +
                    "ΤΗΛ: 2109478478\n\r" +
                    "ΦΑΞ: 2109478500\n\r";

                    /*"EURONET ΠΡΟΠΛΗΡΩΜΗ ΕΛΛΑΣ ΕΠΕ\n\r" +
                    "EURONET PREPAID GREECE LTD\n\r" +
                    "ΥΠΗΡΕΣΙΕΣ ΤΗΛΕΠΙΚΟΙΝΩΝΙΩΝ\n\r" +
                    "ΣΑΧΤΟΥΡΗ 1 ΚΑΛΛΙΘΕΑ Τ.Κ. 176 74\n\r" +
                    "ΑΦΜ: 998254125 ΔΟΥ: Α’ (Β’-Α’) ΚΑΛΛΙΘΕΑΣ\n\r" +
                    "ΤΗΛ: 2109478478\n\r" +
                    "ΦΑΞ: 2109478500\n\r";*/

                int i = 0;
                decimal[] VAT = new Decimal[300];
                decimal[] Margin = new Decimal[300];
                decimal[] MarginexVAT = new Decimal[300];

                String[] customer =new String[300];
                String[] email =new String[300];
                String[] category =new String[300];

                SqlCommand com1= new SqlCommand();
                com1.Connection = transactConn;
                com1.CommandTimeout = 0;
                com1.CommandText = "SELECT [Name],[VAT],[Margin],[MarginexVAT],[Recipients],[Category] "/*[rcp],*/+
                    "FROM [iTunes].[dbo].[CUSTOMERS]";
                

                //OdbcCommand com1 = new OdbcCommand("SELECT [Name],[VAT],[Margin],[MarginexVAT],[Recipients],[Category] "/*[rcp],*/+
                //   "FROM [iTunes].[dbo].[CUSTOMERS]", conn);

                //OdbcDataReader reader1 = com1.ExecuteReader();
                SqlDataReader reader1 = com1.ExecuteReader();

                while (reader1.Read())
                {
                    VAT[i] = reader1.GetDecimal(1);
                    Margin[i] = reader1.GetDecimal(2);
                    MarginexVAT[i] = reader1.GetDecimal(3);
                    customer[i] = reader1.GetValue(0).ToString();
                    email[i] = reader1.GetValue(4).ToString();
                    category[i] = reader1.GetValue(5).ToString();
                    i++;
                }

                reader1.Close();
               
                
                for (int j = 0; j < i; j++)
                {
                    if(String.Compare(category[j],"Apple")==0)
                    {  // string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + OutputDirectory + customer[j] + "_iTunes_" + timestamp + ".xls;";
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_iTunes_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Apple' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='Apple' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //    //"WHERE Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)>='" + from + "' AND Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)<='" + to +
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                        SqlDataReader reader = com.ExecuteReader();

                        //OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                        //" Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        //" Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,2), " +
                        //" VATonCommission DECIMAL(10,2),RetailerFunds DECIMAL(10,2),PayabletoEuronet DECIMAL(10,2))");

                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");



                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                //decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;
                                    }
                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="Apple")
                                       break;                                    
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="Apple")
                                        break; 
                                }
                                //int myIndex = Array.IndexOf(customer, reader.GetValue(0).ToString().Trim());

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                //decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_iTunes_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_iTunes_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " iTunes " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_iTunes_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " iTunes " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " iTunes " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_iTunes_" + timestamp + ".xls");
                        }


                    }
                    else if(String.Compare(category[j],"iTunesPass")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_iTunesPass_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='iTunesPass' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='iTunesPass' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //    //"WHERE Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)>='" + from + "' AND Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)<='" + to +
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                        SqlDataReader reader = com.ExecuteReader();

                        //OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                        //" Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        //" Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,2), " +
                        //" VATonCommission DECIMAL(10,2),RetailerFunds DECIMAL(10,2),PayabletoEuronet DECIMAL(10,2))");


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");



                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                //decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;
                                    }
                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="iTunesPass")
                                       break;                                    
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="iTunesPass")
                                        break; 
                                }
                                //int myIndex = Array.IndexOf(customer, reader.GetValue(0).ToString().Trim());

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                //decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_iTunesPass_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_iTunesPass_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " iTunesPass " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_iTunesPass_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " iTunesPass " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " iTunesPass " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_iTunesPass_" + timestamp + ".xls");
                        }


                    }
                    else if(String.Compare(category[j],"Google")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Google_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Google' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='Google' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //    //"WHERE Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)>='" + from + "' AND Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)<='" + to +
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                        SqlDataReader reader = com.ExecuteReader();


                        //OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                        //" Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        //" Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,2), " +
                        //" VATonCommission DECIMAL(10,2),RetailerFunds DECIMAL(10,2),PayabletoEuronet DECIMAL(10,2))");


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Google Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                //decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Google Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;

                                    }

                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="Google")
                                         break;
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="Google")
                                       break;
                                }
                              //  myIndex = Array.IndexOf(customer, reader.GetValue(0).ToString().Trim());

                              

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                //decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 2);
                                //decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 2);
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Google Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_Google_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Google_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Google " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Google_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Google " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Google " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Google_" + timestamp + ".xls");
                        }


                    }
                    else if(String.Compare(category[j],"TopUp")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_TopUp_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='TopUp' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='TopUp' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //    //"WHERE Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)>='" + from + "' AND Left([TOPUP].[dbo].[SOLD].SaleDateTime,10)<='" + to +
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                       SqlDataReader reader = com.ExecuteReader();



                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [TopUp Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255))");




                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                
                                OleDbCommand myCommand = new OleDbCommand("Insert into [TopUp Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;

                                    }

                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="TopUp")
                                        break;
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="TopUp")
                                        break;
                                }

                                //int myIndex = Array.IndexOf(customer, reader.GetValue(0).ToString().Trim());

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }

                                OleDbCommand myCommand = new OleDbCommand("Insert into [TopUp Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_TopUp_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_TopUp_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " TopUp " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                 " TopUp " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_TopUp_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " TopUp " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " TopUp " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_TopUp_" + timestamp + ".xls");
                        }

                    }
                    else if(String.Compare(category[j],"LeagueOfLegends")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_LeagueOfLegends_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='LeagueOfLegends' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='LeagueOfLegends' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [LeagueOfLegends Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [LeagueOfLegends Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;

                                    }

                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="LeagueOfLegends")
                                         break;
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="LeagueOfLegends")
                                       break;
                                }
                                                            

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [LeagueOfLegends Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_LeagueOfLegends_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_LeagueOfLegends_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);

                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " League Of Legends " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_LeagueOfLegends_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " League Of Legends " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " League Of Legends " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_LeagueOfLegends_" + timestamp + ".xls");
                        }


                    }
                    else if(String.Compare(category[j],"Blizzard")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Blizzard_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Blizzard' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='Blizzard' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Blizzard Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Blizzard Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;

                                    }

                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="Blizzard")
                                         break;
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="Blizzard")
                                       break;
                                }
                                                            

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Blizzard Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_Blizzard_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Blizzard_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);

                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Blizzard " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Blizzard_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Blizzard " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Blizzard " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Blizzard_" + timestamp + ".xls");
                        }


                    }
                    else if (String.Compare(category[j], "Netflix") == 0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Netflix_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where = "";

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            where = " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Netflix' ";
                        }
                        else if (String.Compare(customer[j], "ALL") == 0)
                        {
                            where = " WHERE [Category]='Netflix' ";
                        }

                        SqlCommand com = new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Netflix Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Netflix Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex = 0;

                                for (myIndex = 0; myIndex < customer.Length; myIndex++)
                                {
                                    if (String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex = -1;
                                        break;

                                    }

                                    if (customer[myIndex] == reader.GetValue(0).ToString().Trim() && category[myIndex] == "Netflix")
                                        break;
                                    if (customer[myIndex].Length < reader.GetValue(0).ToString().Length &&
                                       customer[myIndex] == reader.GetValue(0).ToString().Trim().Substring(0, customer[myIndex].Length) && category[myIndex] == "Netflix")
                                        break;
                                }


                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Netflix Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n" + customer[j] + "_Netflix_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Netflix_" + timestamp + ".xls CREATED.\r\n");

                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);

                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Netflix " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Netflix_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Netflix " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Netflix " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Netflix_" + timestamp + ".xls");
                        }


                    }
                    else if(String.Compare(category[j],"Avira")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Avira_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Avira' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='Avira' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +//,[Price]
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";


                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Avira Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255))"); //,Margin DECIMAL(10,2)
                       


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
      

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Avira Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +//,Margin
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString()+ "')");//"','" +reader.GetValue(12).ToString() + 

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Avira Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +//,Margin
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString()+ "')");//"','" +reader.GetValue(12).ToString()+  

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_Avira_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Avira_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Avira " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                 " Avira " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Avira_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Avira " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Avira " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Avira_" + timestamp + ".xls");
                        }
                        
                    }
                    else if(category[j].Length>=9 && String.Compare(category[j].Substring(0,9),"Microsoft")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND left([Category],9)='Microsoft' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE left([Category],9)='Microsoft' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        //OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        //",[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        //" FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        //where +
                        //" ORDER BY [Customer],[TrxDate],[TrxTime] "
                        //, conn);

                        //OdbcDataReader reader = com.ExecuteReader();
                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Microsoft Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255) )"); 
                       


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
      

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Microsoft Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Microsoft Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() +  "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_Microsoft_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Microsoft_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                 " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls");
                        }
                        
                    }
                    else if (String.Compare(category[j],"McAfee")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_McAfee_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where = "";

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            where = " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='McAfee' ";
                        }
                        else if (String.Compare(customer[j], "ALL") == 0)
                        {
                            where = " WHERE [Category]='McAfee' ";
                        }

                        SqlCommand com = new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";


                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [McAfee Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255) )");



                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {


                                OleDbCommand myCommand = new OleDbCommand("Insert into [McAfee Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {


                                OleDbCommand myCommand = new OleDbCommand("Insert into [McAfee Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n" + customer[j] + "_McAfee_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_McAfee_" + timestamp + ".xls CREATED.\r\n");

                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " McAfee " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                 " McAfee " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_McAfee_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " McAfee " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " McAfee " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_McAfee_" + timestamp + ".xls");
                        }

                    }
                    else if (String.Compare(category[j], "Kaspersky") == 0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Kaspersky_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where = "";

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            where = " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Kaspersky' ";
                        }
                        else if (String.Compare(customer[j], "ALL") == 0)
                        {
                            where = " WHERE [Category]='Kaspersky' ";
                        }

                        SqlCommand com = new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";


                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Kaspersky Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255) )");



                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {


                                OleDbCommand myCommand = new OleDbCommand("Insert into [Kaspersky Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {


                                OleDbCommand myCommand = new OleDbCommand("Insert into [Kaspersky Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                                    + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(6) + "','"
                                    + reader.GetValue(7).ToString() + "','" + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10) + "','"
                                    + reader.GetValue(11).ToString() + "')");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n" + customer[j] + "_Kaspersky_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Kaspersky_" + timestamp + ".xls CREATED.\r\n");

                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);
                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Kaspersky " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                 " Kaspersky " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Kaspersky_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Kaspersky " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Kaspersky " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Kaspersky_" + timestamp + ".xls");
                        }

                    }
                    else if(String.Compare(category[j],"Xbox")==0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Xbox_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where="";

                        if(String.Compare(customer[j],"ALL")!=0)
                        {
                           where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Xbox' ";
                        }
                        else if(String.Compare(customer[j],"ALL")==0)
                        {
                           where= " WHERE [Category]='Xbox' ";
                        }

                        SqlCommand com= new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        SqlDataReader reader = com.ExecuteReader();

                        OleDbCommand dbCmd;
                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            dbCmd = new OleDbCommand("CREATE TABLE [Xbox Giftcards Report] (Customer char(255),TerminalID char(255), " +
                            " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                            " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                            " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");
                        }
                        else//20170424 remove columns with amounts from XBOX ALL report
                        {
                            dbCmd = new OleDbCommand("CREATE TABLE [Xbox Giftcards Report] (Customer char(255),TerminalID char(255), " +
                            " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                            " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255))");
                        }

                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Xbox Giftcards Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex=0;

                                for(myIndex=0;myIndex<customer.Length;myIndex++)
                                {
                                    if(String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex=-1;
                                        break;

                                    }

                                    if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="Xbox")
                                         break;
                                    if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                                       customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="Xbox")
                                       break;
                                }
                                                            

                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Xbox Giftcards Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product)"+//,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    //+ reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");
                                    + reader.GetValue(12).ToString() + "' )"); //20170424 remove columns with amounts from XBOX ALL report
                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n"+customer[j] + "_Xbox_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Xbox_" + timestamp + ".xls CREATED.\r\n");
                        
                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);

                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Xbox " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Xbox_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Xbox " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Xbox " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Xbox_" + timestamp + ".xls");
                        }


                    }
                    else if (String.Compare(category[j], "Sony") == 0)
                    {
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + customer[j] + "_Sony_" + timestamp + ".xls;";
                        connectionString += "Extended Properties=Excel 8.0;";

                        string where = "";

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            where = " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Sony' ";
                        }
                        else if (String.Compare(customer[j], "ALL") == 0)
                        {
                            where = " WHERE [Category]='Sony' ";
                        }

                        SqlCommand com = new SqlCommand();
                        com.Connection = transactConn;
                        com.CommandTimeout = 0;
                        com.CommandText = "SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                        " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                        where +
                        " ORDER BY [Customer],[TrxDate],[TrxTime] ";

                        SqlDataReader reader = com.ExecuteReader();


                        OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [Sony Report] (Customer char(255),TerminalID char(255), " +
                        " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Amount DECIMAL(10,2),Receipt char(255), " +
                        " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),RetailerCommission DECIMAL(10,4), " +
                        " VATonCommission DECIMAL(10,4),RetailerFunds DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                        OleDbConnection myConnection = new OleDbConnection(connectionString);

                        myConnection.Open();


                        dbCmd.Connection = myConnection;

                        dbCmd.ExecuteNonQuery();

                        if (String.Compare(customer[j], "ALL") != 0)
                        {
                            while (reader.Read())
                            {
                                decimal Commission = Decimal.Round(MarginexVAT[j] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[j]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Sony Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                int myIndex = 0;

                                for (myIndex = 0; myIndex < customer.Length; myIndex++)
                                {
                                    if (String.IsNullOrEmpty(customer[myIndex]))
                                    {
                                        myIndex = -1;
                                        break;

                                    }

                                    if (customer[myIndex] == reader.GetValue(0).ToString().Trim() && category[myIndex] == "Sony")
                                        break;
                                    if (customer[myIndex].Length < reader.GetValue(0).ToString().Length &&
                                       customer[myIndex] == reader.GetValue(0).ToString().Trim().Substring(0, customer[myIndex].Length) && category[myIndex] == "Sony")
                                        break;
                                }


                                if (myIndex < 0)
                                {
                                    myIndex = Array.IndexOf(customer, "ALL");
                                }
                                decimal Commission = Decimal.Round(MarginexVAT[myIndex] * Decimal.Parse(reader.GetValue(6).ToString()), 4);
                                decimal VATonCommission = Decimal.Round((Commission * VAT[myIndex]), 4);
                                decimal Funds = Commission + VATonCommission;
                                decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) - Funds;

                                OleDbCommand myCommand = new OleDbCommand("Insert into [Sony Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Amount,Receipt, " +
                                " Trace ,Serial,Type,UserName,Product,RetailerCommission,VATonCommission,RetailerFunds,PayabletoEuronet) " +
                                    "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','" + reader.GetValue(3).ToString() + "','"
                                    + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "'," + reader.GetValue(6).ToString() + ",'" + reader.GetValue(7) + "','"
                                    + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                                    + reader.GetValue(12).ToString() + "'," + Commission + "," + VATonCommission + "," + Funds + "," + Payable + ")");

                                myCommand.Connection = myConnection;
                                myCommand.ExecuteNonQuery();
                            }
                        }

                        mailBody += "\r\n" + customer[j] + "_Sony_" + timestamp + ".xls CREATED.\r\n";
                        sw.WriteLine("\r\n" + customer[j] + "_Sony_" + timestamp + ".xls CREATED.\r\n");

                        myConnection.Close();
                        reader.Close();

                        Thread.Sleep(2000);

                        if (String.Compare(customer[j], "ALL") != 0)
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Sony " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                mailBodydefault, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Sony_" + timestamp + ".xls");
                        }
                        else
                        {

                            //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                            MailClass.SendMailAttach(Path, customer[j] + " Sony " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                                " Sony " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Sony_" + timestamp + ".xls");
                        }


                    }
                    //else if(String.Compare(category[j],"Microsoft")==0)
                    //{   string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls;";
                    //    connectionString += "Extended Properties=Excel 8.0;";

                    //    string where="";

                    //    if(String.Compare(customer[j],"ALL")!=0)
                    //    {
                    //       where= " WHERE [Customer] like '" + customer[j] + "%' AND [Category]='Microsoft' ";
                    //    }
                    //    else if(String.Compare(customer[j],"ALL")==0)
                    //    {
                    //       where= " WHERE [Category]='Microsoft' ";
                    //    }

                    //   SqlCommand com= new SqlCommand();
                    //   com.Connection = transactConn;
                    //   com.CommandTimeout = 0;
                    //    com.CommandText ="SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                    //    ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Price] WHEN 'A' THEN [Price] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                    //    " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                    //    where +
                    //    " ORDER BY [Customer],[TrxDate],[TrxTime] "

                    ////    OdbcCommand com = new OdbcCommand("SELECT [Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN] " +
                    ////    ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Price] WHEN 'A' THEN [Price] END,[Receipt],[Trace],[Serial],TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[Name]" +
                    ////    " FROM [iTunes].[dbo].[TRANSACTIONS] LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN) " +
                    ////    where +
                    ////    " ORDER BY [Customer],[TrxDate],[TrxTime] "
                    ////    , conn);

                    ////    OdbcDataReader reader = com.ExecuteReader();
                    //    SqlDataReader reader = com.ExecuteReader();


                    //    OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [iTunes Report] (Customer char(255),TerminalID char(255), " +
                    //    " Storeno char(255),TrxDate char(255),TrxTime char(255),EAN char(255),Receipt char(255), " +
                    //    " Trace char(255),Serial char(255),Type char(255),UserName char(255),Product char(255),Amount DECIMAL(10,2), " +
                    //    " VATonAmount DECIMAL(10,4),PayabletoEuronet DECIMAL(10,4))");


                    //    OleDbConnection myConnection = new OleDbConnection(connectionString);

                    //    myConnection.Open();


                    //    dbCmd.Connection = myConnection;

                    //    dbCmd.ExecuteNonQuery();

                    //    if (String.Compare(customer[j], "ALL") != 0)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            decimal AmountexVAT = Decimal.Round(Decimal.Parse(reader.GetValue(6).ToString()), 4);
                    //            decimal VATonAmount = Decimal.Round((AmountexVAT * VAT[j]), 4);
                    //            decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) + VATonAmount;

                    //            OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                    //            " Trace ,Serial,Type,UserName,Product,Amount,VATonAmount,PayabletoEuronet) " +
                    //                "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                    //                + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(7) + "','"
                    //                + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                    //                + reader.GetValue(12).ToString() + "'," + reader.GetValue(6).ToString() + "," + VATonAmount + "," + Payable + ")");

                    //            myCommand.Connection = myConnection;
                    //            myCommand.ExecuteNonQuery();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            int myIndex=0;

                    //            for(myIndex=0;myIndex<customer.Length;myIndex++)
                    //            {
                    //                if(String.IsNullOrEmpty(customer[myIndex]))
                    //                {
                    //                    myIndex=-1;
                    //                    break;

                    //                }

                    //                if(customer[myIndex]== reader.GetValue(0).ToString().Trim() && category[myIndex]=="Microsoft")
                    //                     break;
                    //                if(customer[myIndex].Length<reader.GetValue(0).ToString().Length &&
                    //                   customer[myIndex]== reader.GetValue(0).ToString().Trim().Substring(0,customer[myIndex].Length) && category[myIndex]=="Microsoft")
                    //                   break;
                    //            }
                                                            

                    //            if (myIndex < 0)
                    //            {
                    //                myIndex = Array.IndexOf(customer, "ALL");
                    //            }

                    //            decimal AmountexVAT = Decimal.Round(Decimal.Parse(reader.GetValue(6).ToString()), 4);
                    //            decimal VATonAmount = Decimal.Round((AmountexVAT * VAT[myIndex]), 4);
                    //            decimal Payable = Decimal.Parse(reader.GetValue(6).ToString()) + VATonAmount;

                    //            OleDbCommand myCommand = new OleDbCommand("Insert into [iTunes Report] (Customer,TerminalID,Storeno,TrxDate,TrxTime,EAN,Receipt, " +
                    //            " Trace ,Serial,Type,UserName,Product,Amount,VATonAmount,PayabletoEuronet) " +
                    //                "values ('" + reader.GetValue(0).ToString() + "','" + reader.GetValue(1).ToString() + "','" + reader.GetValue(2).ToString() + "','"
                    //                + reader.GetValue(3).ToString() + "','" + reader.GetValue(4).ToString() + "','" + reader.GetValue(5).ToString() + "','" + reader.GetValue(7) + "','"
                    //                + reader.GetValue(8).ToString() + "','" + reader.GetValue(9).ToString() + "','" + reader.GetValue(10).ToString() + "','" + reader.GetValue(11) + "','"
                    //                + reader.GetValue(12).ToString() + "'," + reader.GetValue(6).ToString() + "," + VATonAmount + "," + Payable + ")");

                    //            myCommand.Connection = myConnection;
                    //            myCommand.ExecuteNonQuery();
                    //        }
                    //    }

                    //    mailBody += "\r\n"+customer[j] + "_Microsoft_" + timestamp + ".xls CREATED.\r\n";
                    //    sw.WriteLine("\r\n" + customer[j] + "_Microsoft_" + timestamp + ".xls CREATED.\r\n");
                        
                    //    myConnection.Close();
                    //    reader.Close();

                    //  Thread.Sleep(2000);
                    //    if (String.Compare(customer[j], "ALL") != 0)
                    //    {

                    //        //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                    //        MailClass.SendMailAttach(Path, customer[j] + " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                    //             " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], emailCC, emailBCC, OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls");
                    //    }
                    //    else
                    //    {

                    //        //MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, emailCC, emailBCC);
                    //        MailClass.SendMailAttach(Path, customer[j] + " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1,
                    //            " Microsoft " + day_7 + "-" + month_7 + "-20" + year_7 + " έως " + day_1 + "-" + month_1 + "-20" + year_1, email[j], "", "", OutputDirectory + customer[j] + "_Microsoft_" + timestamp + ".xls");
                    //    }
                        
                    //}

                }

                /*try 20150722 transactions are now loaded daily
                {
                    
                    SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
                    sqConTransactB.DataSource = @"grat1-mis-ap1p"; //Production

                    sqConTransactB.IntegratedSecurity = true;
                    sqConTransactB.InitialCatalog = "iTunes";
                    sqConTransactB.ConnectTimeout = 0;

                    SqlConnection transactConn = new SqlConnection(sqConTransactB.ConnectionString);
                    transactConn.Open();

                    SqlCommand comSQL = new SqlCommand();
                    comSQL.Connection = transactConn;
                    comSQL.CommandTimeout = 0;
                    comSQL.CommandText = "Delete from [dbo].[TRANSACTIONS_ALL] "+
                        "WHERE TRXDATE>=(SELECT MIN(TRXDATE) FROM [TRANSACTIONS]) "+
                        "and TRXDATE<=(SELECT MAX(TRXDATE) FROM [TRANSACTIONS]) ";
                    comSQL.ExecuteNonQuery();

                    comSQL.CommandText = "UPDATE [TRANSACTIONS] "+
                        "SET Amount=(SELECT [PRODUCTS].PRICE from PRODUCTS where [TRANSACTIONS].EAN=[PRODUCTS].EAN) "+
                        "where [TRANSACTIONS].EAN in (select EAN FROM PRODUCTS WHERE PRICE is not null) ";
                    comSQL.ExecuteNonQuery();

                    comSQL.CommandText = "INSERT INTO [TRANSACTIONS_ALL] SELECT * FROM [TRANSACTIONS] ";
                    comSQL.ExecuteNonQuery();


                    transactConn.Close();

                    mailBody +=  "\r\nUpdate TRANSACTIONS_ALL.\r\n";
                    sw.WriteLine("\r\nUpdate TRANSACTIONS_ALL.\r\n");

                }
                catch
                {
                    mailBody +=  "\r\nProblem Updating TRANSACTIONS_ALL.\r\n";
                    sw.WriteLine("\r\nProblem Updating TRANSACTIONS_ALL.\r\n");

                }*/

                try//20181003
                {
                    SqlCommand comSQL = new SqlCommand();
                    comSQL.Connection = transactConn;
                    comSQL.CommandTimeout = 0;
                    comSQL.CommandText = "Delete from [dbo].[REPORTS_ALL] " +
                        "WHERE TRXDATE>=(SELECT MIN(TRXDATE) FROM [TRANSACTIONS]) " +
                        "and TRXDATE<=(SELECT MAX(TRXDATE) FROM [TRANSACTIONS]) ";
                    comSQL.ExecuteNonQuery();

                    comSQL.CommandText = "INSERT INTO REPORTS_ALL "+
                        "SELECT [iTunes].[dbo].[PRODUCTS].Category,[iTunes].[dbo].[CUSTOMERS].Name,[Customer],[TerminalID],[Storeno],[TrxDate],[TrxTime],[iTunes].[dbo].[TRANSACTIONS].[EAN]  "+
                        ",AAmount=CASE [Type]  WHEN 'D' THEN '-'+[Amount] WHEN 'A' THEN [Amount] END,[Receipt],[Trace],[Serial] "+
                        ",TType = CASE [Type]  WHEN 'D' THEN 'Deactivation' WHEN 'A' THEN 'Activation' END,[User Name],[iTunes].[dbo].[PRODUCTS].[Name] " +
                        ",Commission=CASE [Type]  WHEN 'D' THEN -1 WHEN 'A' THEN 1 END * case Margin WHEN 0 THEN NULL ELSE ROUND (MarginExVAT*Amount,4) END "+
                        ",VATonCommission=CASE [Type]  WHEN 'D' THEN -1 WHEN 'A' THEN 1 END * case Margin WHEN 0 THEN NULL ELSE ROUND((MarginExVAT*Amount)*VAT,4) END "+
                        ",Funds=CASE [Type]  WHEN 'D' THEN -1 WHEN 'A' THEN 1 END * case Margin WHEN 0 THEN NULL ELSE ROUND(MarginExVAT*Amount,4)+ ROUND((MarginExVAT*Amount)*VAT,4) END "+
                        ",Payable=CASE [Type]  WHEN 'D' THEN -1 WHEN 'A' THEN 1 END * case Margin WHEN 0 THEN NULL ELSE (Amount-(ROUND(MarginExVAT*Amount,4)+(ROUND((MarginExVAT*Amount)*VAT,4)))) END "+
                        "FROM [iTunes].[dbo].[TRANSACTIONS]  "+
	                    "    LEFT JOIN [iTunes].[dbo].[PRODUCTS] ON ([iTunes].[dbo].[PRODUCTS].EAN=[iTunes].[dbo].[TRANSACTIONS].EAN)  "+
                        "LEFT JOIN [iTunes].[dbo].[CUSTOMERS] ON ([iTunes].[dbo].[CUSTOMERS].Name=SUBSTRING([iTunes].[dbo].[TRANSACTIONS].Customer,1,LEN([iTunes].[dbo].[CUSTOMERS].Name)) and  "+
                        "[iTunes].[dbo].[CUSTOMERS].category=[iTunes].[dbo].[PRODUCTS].category) "+
                        "where [iTunes].[dbo].[TRANSACTIONS].ean in (select ean from [iTunes].[dbo].[PRODUCTS])";
                    comSQL.ExecuteNonQuery();

                    mailBody += "\r\nUpdating REPORTS_ALL OK.\r\n";

                }
                catch
                {
                    mailBody += "\r\nProblem Updating REPORTS_ALL.\r\n";
                }

                exitFlag = true;
                mailBody +=  "\r\nDONE.\r\n";
                sw.WriteLine("\r\nDONE.\r\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                transactConn.Close();
            }

            
            try { sw.Close(); transactConn.Close();}
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

            //try
            //{
            //    conn.Open();
            //}
            //catch
            //{
            //    exitFlag=true;
            //    mailBody+="Connection Error!";
            //    // return;

            //}
                

            ini = new IniFile(Path + "iTunes_Auto_Reports.ini");
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

            MailClass.SendMail(Path, "iTunes Auto Reports", mailBody, emailTo, "", "");

            //if (conn.State != System.Data.ConnectionState.Closed)
            //    conn.Close();

        }
    }
}
