using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;

namespace TransactReports_OnDemand
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            string results;
            string path = @"C:\Users\lnestoras\Desktop\ExportReport\Source file\";
            string[] file_Arr = new string[3];

            try
            {
                results = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).First();
                file_Arr[0] = results.Substring(0, 19);
                file_Arr[1] = results.Substring(20, 15);
                file_Arr[2] = results.Substring(36, 10);
                System.IO.File.Copy(@"C:\Users\lnestoras\Desktop\ExportReport\Source file\" + results, @"\\grat1-dev-ap2t\D$\Reporting\Import\report.csv");

            }
            catch
            {
                MessageBox.Show("File is not placed in folder c:\\Users\\lnestoras\\Desktop\\Extractreport\\Source");
                return;
            }

            
            InitializeComponent();

            SqlConnectionStringBuilder sqConTransactB1 = new SqlConnectionStringBuilder();
            sqConTransactB1.DataSource = @"grat1-dev-ap2t"; //Production
            sqConTransactB1.IntegratedSecurity = true;
            sqConTransactB1.InitialCatalog = "ZacReporting";
            sqConTransactB1.ConnectTimeout = 0;
            SqlConnection transactConn1 = new SqlConnection(sqConTransactB1.ConnectionString);
            transactConn1.Open();

            SqlCommand transactMid = new SqlCommand();
            transactMid.Connection = transactConn1;
            transactMid.CommandTimeout = 0;
            transactMid.CommandText = "select mid, merchant, [Group] from abc096.MIDS  where mid= '" + file_Arr[1].ToString() + "'";

            if (file_Arr[1].ToLower().Contains('%') )
            {
                file_Arr[0] = results.Substring(0, 19);
                file_Arr[1] = results.Substring(20, 14);
                file_Arr[2] = results.Substring(35, 10);
                transactMid.CommandText = "select top(1) mid, merchant, [Group] from abc096.MIDS  where mid like '" + file_Arr[1].ToString() + "'";
            }

            SqlDataReader transactMidRd = transactMid.ExecuteReader();

            while(transactMidRd.Read())
            {                               
                transactMidRd[0].ToString();
                textBoxDescrMid.Text = transactMidRd[1].ToString();
                textBoxGroup.Text = transactMidRd[2].ToString();
            }
            textBoxMid.Text = file_Arr[1];
            textBoxDate.Text = file_Arr[2];

            transactMidRd.Dispose();
            transactConn1.Close();

            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\lnestoras\Documents\Visual Studio 2013\Projects\ePOS\ePOS_DR\ePOS\bin\Debug\ePOS.exe");
            //label4.Text = "Export completed.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\lnestoras\Documents\Visual Studio 2013\Projects\Import_csv_to_IMP_TRANSACT_D_OnDemand\bin\Debug\Import_csv_to_IMP_TRANSACT_D.exe");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
