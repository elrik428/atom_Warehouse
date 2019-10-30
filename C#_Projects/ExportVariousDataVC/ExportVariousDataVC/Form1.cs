using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;

namespace ExportVariousDataVC
{
      

    public partial class Form1 : Form
    {
        public static string timestamp = DateTime.Now.ToString("yyyyMMdd");
        public static string ExportName = "";
       //public static string OutputDirectory = @"C:\Users\lnestoras\Desktop\";
        public static string OutputDirectory = @"\\GRAT1-FPS-SV2O\Common\POS Support\";
        //public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + ExportName + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";

        public static OleDbConnection VCConn = null;
        //public static string DBName = "vc30";//Production
        public static string DBName = "vc30";//Production
        public static string VCIP = "Provider=SQLOLEDB;Data Source=10.1.45.144;Connect Timeout=30;Initial Catalog=vc30";//Production

        public static string templist = "";

        public Form1()
        {

            connectVC();
            if (VCConn.State != ConnectionState.Open)
            {
                Console.WriteLine("ERROR: Unable to connect to VeriCentre.");
                return;
            }
            InitializeComponent();

            listView_Parm.Columns.Add("Parameter");
            listView_Parm.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            comboBoxClust.Items.Add("EPOS");
            comboBoxClust.Items.Add("EPOS_PIRAEUS");
            comboBoxClust.Items.Add("ALL");
            //comboBoxClust.Items.Add("PIRAUES");

        }

        static void connectVC()
        {

            /************CONNECT**************/
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    VCConn = new OleDbConnection(VCIP + ";User Id=vc30;Password=VcEn!@#20130627");//Production
                    // VCConn = new OleDbConnection(VCIP + ";User Id=VeriCentre;Password=VcLe!@#20130423");//Test
                    VCConn.Open();

                    if (VCConn.State == ConnectionState.Open)
                        i = 3;
                }
                catch (Exception exc)
                {


                }

            }
        }

        private void Add_Btn_Click(object sender, EventArgs e)
        {
            int i = 0;
            string parm_temp = "";
            //int z = 0;
            OleDbCommand listView_Parms = VCConn.CreateCommand();
            if (textBox_MultParm.Text.Length > 0 && !listView_Parm.Items.Cast<ListViewItem>().Any(item => item.Text == textBox_MultParm.Text))
            {
                char[] separator = { '\n', '\r', ' ' };
                String[] word = textBox_MultParm.Text.Split(separator);
                for (i = 0; i < word.Length; i++)
                {
                    if (!String.IsNullOrEmpty(word[i]) && !listView_Parm.Items.Cast<ListViewItem>().Any(item => item.Text == word[i]))
                    {
                        listView_Parm.Items.Add(new ListViewItem(new string[] { word[i] }));
                    }

                    listView_Parm.Sort();

                }

                foreach (ListViewItem itemTID in listView_Parm.Items)
                {
                    if(parm_temp != "")
                    {
                    parm_temp = parm_temp + "," + itemTID.SubItems[0].Text.ToString();
                    }
                    else
                    {parm_temp = itemTID.SubItems[0].Text.ToString() ;
                    }
                      
                    
                }
                 templist = string.Join(",", parm_temp.Split(',').Select(x => string.Format("'{0}'", x)).ToList());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "";
            string tmpcondi_Clus = "";
            Object selectedItem = comboBoxClust.SelectedItem;
            string seleClust = selectedItem.ToString();
                       
            switch (seleClust)
            {
                case "PIRAEUS": // For future use
                    tmpcondi_Clus = "and substring(t1.appnm,1,4) = 'PIRA' and substring(t1.termid,1,2) <> '73' ";
                    ExportName = "Piraeus_";
                     connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + ExportName + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
                    break;
                case "EPOS":
                    tmpcondi_Clus = "and substring(t1.appnm,1,4) = 'EPOS' and substring(t1.termid,1,2) = '73' ";
                    ExportName = "EPOS_";
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + ExportName + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
                    break;
                case "EPOS_PIRAEUS":
                    tmpcondi_Clus = "and substring(t1.appnm,1,4) = 'EPOS' and t1.CLUSTERID in ('EPOS_PIRAEUS','EPOS_PIRAEUS_EPP')  ";
                    ExportName = "EPOS_Piraeus_";
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + ExportName + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
                    break;
                case "ALL":
                    tmpcondi_Clus = "and substring(t1.appnm,1,4) = 'EPOS'";
                    ExportName = "ALL_EPOS_";
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + OutputDirectory + ExportName + timestamp + ".xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;';";
                    break;
            }


            Cursor.Current = Cursors.WaitCursor;

            string sqlSelQuery = 
                "SELECT "   +
                "distinct  t1.termid as TID, " +
                "case (substring(t1.appnm,1,4)) " +
                "when 'EPOS' then 'EPOS' " +
                "when 'PIRA' then 'PIRAEUS BANK' " +
                "else '????????' " +
                "end as ClusterType, " +
                "t1.CLUSTERID, " +
                "t1.famnm as [TERMINAL TYPE], " +
                "t2.PARNAMELOC, " +
                "t2.value " +
                "FROM " +
                "vc30.relation t1 FULL OUTER JOIN vc30.PARAMETER t2 ON t1.TERMID = t2.PARTID " +
                "where " +
                "t2.PARNAMELOC in ( " + 
                templist +
                " ) " +
                "and t1.acccnt = -1 " +
                "and len(t1.appnm) = 9 " +
                tmpcondi_Clus +
                //"and substring(t1.appnm,1,4) = 'EPOS' " +
                //"and substring(t1.termid,1,2) = '73' " +
                "and substring(t1.termid,1,1) <> ('T') " +
                "order by t1.termid, t2.PARNAMELOC " ;      

            
            //Select records so to write to excel
            OleDbCommand sqlcmdSel_Excel = new OleDbCommand();
            sqlcmdSel_Excel.Connection = VCConn;
            sqlcmdSel_Excel.CommandTimeout = 0;
            sqlcmdSel_Excel.CommandType = CommandType.Text;
            sqlcmdSel_Excel.CommandText = sqlSelQuery;
            OleDbDataReader rdrSelExcel = sqlcmdSel_Excel.ExecuteReader();


            OleDbCommand dbCmd = new OleDbCommand("CREATE TABLE [VC Report] ([TID] char(255),[ClusterType] char(255),[CLUSTERID] char(255),[TERMINAL TYPE] char(255),[PARNAMELOC] char(255),[VALUE] char(255))");
            OleDbConnection myConnection = new OleDbConnection(connectionString);
            myConnection.Open();
            dbCmd.Connection = myConnection;
            dbCmd.ExecuteNonQuery();

            while (rdrSelExcel.Read())
            {
                OleDbCommand myCommand = new OleDbCommand("Insert into [VC Report] ([TID],[ClusterType],[CLUSTERID],[TERMINAL TYPE],[PARNAMELOC],[VALUE]) " +
              " values ('" + rdrSelExcel.GetValue(0).ToString() + "','" + rdrSelExcel.GetValue(1).ToString() + "','" + rdrSelExcel.GetValue(2).ToString() + "','" + rdrSelExcel.GetValue(3).ToString() + "','"
               + rdrSelExcel.GetValue(4).ToString() + "','" + rdrSelExcel.GetValue(5).ToString().Replace('\'', ' ') + "')");

                myCommand.Connection = myConnection;
                myCommand.ExecuteNonQuery();
            }

            myConnection.Close();
            dbCmd.Dispose();
            rdrSelExcel.Close();

            string spreadSheetVCLocation = @"\\GRAT1-FPS-SV2O\Common\POS Support\"+ExportName + timestamp + ".xlsx";

            Microsoft.Office.Interop.Excel.Application excel_ = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook sheet = excel_.Workbooks.Open(spreadSheetVCLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet x = excel_.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

            // Autofit
            x.Columns.AutoFit();
            
            //Bold
            x.get_Range("A1", "I1").EntireRow.Font.Bold = true;

            sheet.Close(true, Type.Missing, Type.Missing);
            excel_.Quit();

            listView_Parm.Clear();
            listView_Parm.Columns.Add("Parameter");
            MessageBox.Show("Export finished!");
            Cursor.Current = Cursors.Default;



        }

        
        private void button2_Click_1(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listView_Parm.SelectedItems)
            {
                listView_Parm.Items.Remove(eachItem);
            }

        }

        //private void ResizeListViewColumns(ListView listView_Parmlv)
        //{
        //    foreach (ColumnHeader column in listView_Parm.Columns)
        //    {
        //        column.Width = -2;
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            listView_Parm.Clear();
            listView_Parm.Columns.Add("Parameter");
        }
    }
}
