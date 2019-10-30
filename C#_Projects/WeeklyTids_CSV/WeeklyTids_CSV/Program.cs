using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
//using System.Linq;
using System.Diagnostics;
using SMTPClass;


namespace WeeklyTids_CSV
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            string path = " ";
            string inputDirectory = @"C:\Users\lnestoras\Desktop\testfile_banks.csv";
            string toEmailList = "lnestoras@euronetworldwide.com";
            string ccEmailList = "lnestoras@euronetworldwide.com";
            string bccEmailList = "lnestoras@euronetworldwide.com";
            string subjectMail = "Data  TIDs for Piraeus & Euronet";

            Stopwatch swra = new Stopwatch();
            swra.Start();

            SqlConnectionStringBuilder sqConTransactB = new SqlConnectionStringBuilder();
            sqConTransactB.DataSource = @"grat1-dev-ap2t"; //Production
            sqConTransactB.IntegratedSecurity = true;
            sqConTransactB.InitialCatalog = "ZacReporting";
            sqConTransactB.ConnectTimeout = 0;

            StreamWriter CsvfileWriter = new StreamWriter(@"C:\Users\lnestoras\Desktop\testfile_banks.csv");
            string sqlselectQuery = "select * from abc096.Banks_new";

            SqlCommand sqlcmd = new SqlCommand();
 
            SqlConnection spContentConn = new SqlConnection(sqConTransactB.ConnectionString);
            sqlcmd.Connection = spContentConn;
            sqlcmd.CommandTimeout = 0;
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandText = sqlselectQuery;
            spContentConn.Open();

            using (spContentConn)
            {
                using (SqlDataReader sdr = sqlcmd.ExecuteReader())
                using (CsvfileWriter)
                {
                    // Table Headers
                   DataTable Tablecolumns = new DataTable();

                    for (int i = 0; i < sdr.FieldCount; i++)
                    {
                        Tablecolumns.Columns.Add(sdr.GetName(i));
                    }
                     CsvfileWriter.WriteLine(string.Join(",", Tablecolumns.Columns.Cast<DataColumn>().Select(csvfile => csvfile.ColumnName)));
                     
                    // Write detail lines tp csv 
                    while (sdr.Read())
                    CsvfileWriter.WriteLine(sdr[0].ToString() + "," + sdr[1].ToString() + "," + sdr[2].ToString() + "," + sdr[3].ToString() + "," + sdr[4].ToString() + ",");
                    //+ sdr[5].ToString() + "," + sdr[6].ToString() + "," + sdr[7].ToString() + "," + sdr[8].ToString() + "," + sdr[9].ToString() + "," + sdr[10].ToString() + "," + sdr[11].ToString() + ",");
                       
                }
            }
            
            MailClass.SendMail(path, subjectMail,inputDirectory, toEmailList, ccEmailList, bccEmailList);
            Console.WriteLine("File has been processed and mail is sent");
            Console.Write('\r');
            swra.Stop();
            Console.WriteLine(swra.ElapsedMilliseconds);
            Console.Write('\r');
            Console.WriteLine("Export finished");
            Console.Write('\r');
            Console.ReadLine();

     }
   }
}

