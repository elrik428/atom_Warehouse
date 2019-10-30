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



namespace VeriCentre_Requested_Data_Export
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public static OleDbConnection VCConn = null;
        //public static string DBName="VeriCentre";//Test
        //public static string VCIP="Provider=SQLOLEDB;Data Source=10.1.95.32;Connect Timeout=30;Initial Catalog=VeriCentre";//Test
        public static string DBName = "vc30";//Production
        public static string VCIP = "Provider=SQLOLEDB;Data Source=10.1.45.144;Connect Timeout=30;Initial Catalog=vc30";//Production

        public Form1()
        {
            InitializeComponent();

            connectVC();
            if (VCConn.State != ConnectionState.Open)
            {
                Console.WriteLine("ERROR: Unable to connect to VeriCentre.");
                //ErrorFile += "ERROR: Unable to connect to VeriCentre.\r\n";
                //ErrorCount++;
                return;
            }

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
