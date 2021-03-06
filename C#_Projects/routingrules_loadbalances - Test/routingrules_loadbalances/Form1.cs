﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
//using System.Threading;
//using System.Windows.Controls;
using System.Data.SqlClient;


namespace routingrules_loadbalances
{
    public partial class Form1 : Form
    {
        string[,] loadBalArr = new string[5, 7];
        string totals_CntAmnt = " ";
        DialogResult dialogResNew_Inst = new DialogResult();
        DialogResult dialogResNew_BIN = new DialogResult();
        int flag_MID = 0;  
        int flag_TID = 0;
        //ProgressBar pbar = new ProgressBar();
        //Timer timer_1 = new Timer();

        public Form1()
        {
            Form4 FormNew = new Form4();
            FormNew.Initialize();
            DialogResult res = FormNew.ShowDialog();

            //timer_1.Enabled = true;
            //timer_1.Start();
            //timer_1.Interval = 1000;

            //pbar.Value = 0;
            //pbar.Minimum = 0;
            //pbar.Maximum = 5000;

            if (res != DialogResult.OK)
            {

                // Application.Exit();
                exit = true;
                //return;
            }

            if (FormNew.textBoxB1.Text.Length != 0 && FormNew.textBoxB2.Text.Length != 0)
            {
                if (String.Compare(FormNew.textBoxB1.Text, "dsrwer53redfr") == 0 || String.Compare(FormNew.textBoxB2.Text, "fsdfwefwetw54") == 0)
                {
                    ConnectionString = "ZAC_Production___";
                }
                else if (String.Compare(FormNew.textBoxB1.Text, "1") == 0 || String.Compare(FormNew.textBoxB2.Text, "1") == 0)
                {
                    ConnectionString = "ZAC_TEST";
                }
                else
                {
                    MessageBox.Show("Wrong username/password");
                    exit = true;
                    //return;
                }

            }

            conn = new OdbcConnection("DSN=" + ConnectionString);

            try
            {
                conn.Open();
            }
            catch
            {
                MessageBox.Show("Cannot connect to ODBC " + ConnectionString);
                exit = true;
                // return;
            }

            if (exit == true)
                return;

            InitializeComponent();

            listView_MIDs.Columns.Add("MID");
            listView_MIDs.Columns.Add("Route#1");
            listView_MIDs.Columns.Add("Route#2");
            listView_MIDs.Columns.Add("Route#3");
            listView_MIDs.Columns.Add("Route#4");
            listView_MIDs.Columns.Add("Route#5");
            listView_MIDs.Columns.Add("Route#6");

            listView_TIDs.Columns.Add("TID");
            listView_TIDs.Columns.Add("Route#1");
            listView_TIDs.Columns.Add("Route#2");
            listView_TIDs.Columns.Add("Route#3");
            listView_TIDs.Columns.Add("Route#4");
            listView_TIDs.Columns.Add("Route#5");
            listView_TIDs.Columns.Add("Route#6");


            listView1.Columns.Add("Issuing");
            listView1.Columns.Add("Routing");
            listView1.Columns.Add("Instal-From");
            listView1.Columns.Add("Instal-To");
            listView1.Columns.Add("Amount-From");                                             // 09/02/2018  
            listView1.Columns.Add("Amount-To");                                               // 09/02/2018      

            listView2.Columns.Add("Bank");
            listView2.Columns.Add("Load Balance");
            listView2.Columns.Add("GR Value");
            listView2.Columns.Add("EU Value");
            listView2.Columns.Add("Other Value");
            listView2.Columns.Add("Routing Type");

            listView_BINChk.Columns.Add("Destport");
            listView_BINChk.Columns.Add("BIN");
            listView_BINChk.Visible = false;

            textBoxHidden_Route.Visible = false;
            textBoxFr.Text = " ";
            textBoxTo.Text = " ";

            int[] arrBal = new int[4];
            arrBal[0] = 0;
            arrBal[1] = 1;
            arrBal[2] = 50;
            arrBal[3] = 1000000;

            radioButton_Cnt.Checked = true;

            //string[,] loadBalArr = new string[4, 2];
            //Array.Clear(loadBalArr, 4, 2);


            //comboMID
            OdbcCommand comboFillMids = new OdbcCommand(
               "SELECT MID FROM dbo.MERCHANTS_test " +
               "where mid like '00%' and (substring(mid,1,8) = '00000012' or substring(mid,1,8) = '00000011' or substring(mid,1,8) = '00000015'or mid= '000000078000000')" +
               "group by [MID] order by [MID]", conn);
            OdbcDataReader reader = comboFillMids.ExecuteReader();
            while (reader.Read())
            {
                string word = reader.GetString(0);

                this.comboGroup.Items.Add(word);

            }
            reader.Close();

            //comboBnkRoute
            OdbcCommand comboFillBnks = new OdbcCommand(
                 "SELECT BANKDEscr FROM dbo.ROUTEBANK where uploadhostid <> '202'  and uploadhostid <> ' '",
                 conn);
            OdbcDataReader reader2 = comboFillBnks.ExecuteReader();
            while (reader2.Read())
            {
                string BankRout = reader2.GetString(0);

                this.comboBnkRoute.Items.Add(BankRout);
                this.comboBoxLD.Items.Add(BankRout);
            }
            reader2.Close();

            // Combo Box for installemnts
            for (int i = 0; i < 100; i++)
            {
                comboBox_InstalFr.Items.Add(i);                             
                comboBox_InstalTo.Items.Add(i);     
            }
                       
            //Combo box for Load balance Percentage 

            for (int i = 0; i < 4; i++)
            {
                comboBoxPrcntge.Items.Add(arrBal[i]);
            }
            

            // List Box Originator
            OdbcCommand listBoxOrigin = new OdbcCommand(
                 "Select bankdescr_short From dbo.ROUTEBANK where uploadhostid <> '202' order by ORDER_view",
                 conn);
            OdbcDataReader reader3 = listBoxOrigin.ExecuteReader();
            while (reader3.Read())
            {
                string originDescr = reader3.GetString(0);

                this.listBoxOrigin.Items.Add(originDescr);

            }
            reader3.Close();

        }

        private void button1_Click(object sender, EventArgs e) { }

        //Insert records to backup file
        private void button2_Click(object sender, EventArgs e)
        {
            int countMER = 0;
            int countBUP = 0;
            int countMER_LD = 0;
            int countBUP_LD = 0;
            string seleMID;
            seleMID = midDescr.Text;

            // Delete from Backup file
            OdbcCommand delRecsBup = new OdbcCommand("Delete from dbo.MERCHBINS_bu_ln", conn);
            OdbcDataReader reader = delRecsBup.ExecuteReader();
            reader.Close();

            // Inesrt to backup file
            OdbcCommand comIns = new OdbcCommand(
                  "insert into dbo.MERCHBINS_bu_ln (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX)" +
                  "(select TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN,GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from dbo.MERCHBINS_TEST)", conn);
            OdbcDataReader readerIns = comIns.ExecuteReader();
            readerIns.Close();

            // Check records in backup file and original the same
            OdbcCommand comCount_1 = new OdbcCommand("select count(*) from dbo.MERCHBINS_TEST", conn);
            countMER = Convert.ToInt32(comCount_1.ExecuteScalar());

            OdbcCommand comCount_2 = new OdbcCommand("select count(*) from dbo.MERCHBINS_bu_ln", conn);
            countBUP = Convert.ToInt32(comCount_2.ExecuteScalar());

            // Delete from Backup_LD file
            OdbcCommand delRecsBup_LD = new OdbcCommand("Delete from dbo.LOADBALANCE_BU", conn);
            OdbcDataReader reader_LD = delRecsBup_LD.ExecuteReader();
            reader_LD.Close();

            // Inesrt to backup file
            OdbcCommand comIns_LD = new OdbcCommand(" SET IDENTITY_INSERT dbo.LOADBALANCE_BU ON " +
                  "INSERT INTO dbo.LOADBALANCE_BU(ID,BALANCEGROUP,DESTPORT,LOADTYPE,LOADVALUE,SYNC_ID,GR_VAL,EU_VAL,OTHER_VAL)" +
                  "(SELECT ID,BALANCEGROUP,DESTPORT,LOADTYPE,LOADVALUE,SYNC_ID,GR_VAL,EU_VAL,OTHER_VAL FROM dbo.LOADBALANCE_TEST)", conn);
            OdbcDataReader readerIns_LD = comIns_LD.ExecuteReader();
            readerIns_LD.Close();

            // Check records in backup file and original the same
            OdbcCommand comCount_1LD = new OdbcCommand("select count(*) from dbo.LOADBALANCE_TEST", conn);
            countMER_LD = Convert.ToInt32(comCount_1.ExecuteScalar());

            OdbcCommand comCount_2LD = new OdbcCommand("select count(*) from dbo.LOADBALANCE_BU", conn);
            countBUP_LD = Convert.ToInt32(comCount_2.ExecuteScalar());


            if (countMER != countBUP && countMER_LD != countBUP_LD)
                MessageBox.Show("Backup records not equal with initial records");

            //label7.ForeColor = System.Drawing.Color.Red;
            //label7.Text = "DONE";

            seleMID = midDescr.Text;
        }

        private void label4_Click(object sender, EventArgs e) { }

        // MIDs combobox 
        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            Object selectedItem = cmb.SelectedItem;
            midDescr.Text = selectedItem.ToString();
            OdbcCommand listView_MID = conn.CreateCommand();
            string[] hostid_Descr = new string[5];
            int z = 0;
            listView_MID.CommandText = "select case uploadhostid " +
                                        "when 1 then 'PIREOS' " +
                                        "when 6 then 'ETHNIKI' " +
                                        "when 201 then 'ALMOR' " +
                                        "when 302 then 'ALPHA' " +
                                        "when 205 then 'EUROBANK' " +
                                        "when 7 then 'ATTIKA' " +
                                        "when 989 then 'EURMOR' " +
                                        "else ' ' " +
                                        "end as Host_descr " +
                                        "from MERCHANTS_test where mid = ? " +
                                        "and  uploadhostid in ('1','6','302','205','7','201','989')  " + 
                                        "group by uploadhostid ";
            listView_MID.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
            OdbcDataReader reader11 = listView_MID.ExecuteReader();
            while (reader11.Read() && z <= 5)
            {
                hostid_Descr[z] = (reader11.GetString(0)) ?? string.Empty;
                z++;
            }
            reader11.Close();
            listView_MIDs.Items.Add(new ListViewItem(new string[] { cmb.SelectedItem.ToString(), hostid_Descr[0], hostid_Descr[1], hostid_Descr[2], hostid_Descr[3], hostid_Descr[4] }));



            //listView_MIDs.Items.Add(new ListViewItem(new string[] {cmb.SelectedItem.ToString()}));
            listView_MIDs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        private void midDescr_TextChanged(object sender, EventArgs e) { }

        // Delete TIDs for selected MID
        private void button3_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listView_MIDs.Items)
            //{
            //midDescr.Text = item.SubItems[0].Text.ToString();
            //OdbcCommand com = new OdbcCommand("delete from dbo.MERCHBINS_TEST where tid in (select tid from dbo.merchants_test where mid =" + comboGroup.SelectedItem + ")", conn);
            if (flag_TID == 0)    // MIDs are chosen for ruling
            {
                OdbcCommand comdel = new OdbcCommand("delete from dbo.MERCHBINS_TEST where tid in (select tid from dbo.merchants_test where mid ='" + midDescr.Text + "')", conn);
                OdbcDataReader readerDel = comdel.ExecuteReader();
                readerDel.Close();
            }
            else if (flag_TID > 0)
            {
                OdbcCommand comdel = new OdbcCommand("delete from dbo.MERCHBINS_TEST where tid =" + tidDescr.Text, conn);
                OdbcDataReader readerDel = comdel.ExecuteReader();
                readerDel.Close();
            }
            label5.ForeColor = System.Drawing.Color.Red;
            label5.Text = "DONE";
            //}
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void button8_Click(object sender, EventArgs e) { }

        //Verify button so to add in listView
        private void button4_Click(object sender, EventArgs e)
        {
            
            if (flag_TID == 0)    // MIDs are chosen for ruling
            {
                string lstVieItmInpu = "";
                string hiddString = "";
                foreach (ListViewItem itemMID in listView_MIDs.Items)
                {
                    int[] cntBnkTid = new int[6];
                    string destChk = " ";
                    string binLoChk = " ";
                    string destChkDescr = " ";
                    string midCheck = "";
                    string flg_MID = "M";
                    int inst_MAX = 0;
                    int inst_MIN = 0;
                    
                    OdbcCommand comCursor_NewBIN_Chk = conn.CreateCommand();

                    if(lstVieItmInpu =="" && hiddString == "" && textBoxFr.Text == " " && textBoxTo.Text== " " && textBoxAmntFr.Text == "" && textBoxAmntTo.Text=="")  // 090218
                    {
                        MessageBox.Show("You haven't made any selection!");
                        button5.Enabled = false;
                            return;
                    }

                    //Check if installs are selected so to proceed
                    if ((textBoxFr.Text.Equals(" ")) || (textBoxTo.Text.Equals(" ")))
                    {
                        MessageBox.Show("Installments are not chosen!");
                        return;
                    }

                    //Check if amounts are filled so to proceed                     09/02/2018
                    if ((textBoxAmntFr.Text.Equals("")) || (textBoxAmntTo.Text.Equals("")))
                    {
                        MessageBox.Show("Amounts fields are not filled!");
                        return;
                    }

                    if (listBoxOrigin.SelectedItem != null && textBox_NEWBIN.Text == "")
                    {
                        //   if (listBoxOrigin.SelectedItem.ToString() == "PIREOS" && listBoxOrigin.SelectedItem.ToString() == "ETHNIKI" && listBoxOrigin.SelectedItem.ToString() == "ALPHA" && listBoxOrigin.SelectedItem.ToString() == "EUROBANK" && listBoxOrigin.SelectedItem.ToString() == "ATTIKA" &&
                        //           listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && listBoxOrigin.SelectedItem.ToString() == "MEALSANDMORE" && listBoxOrigin.SelectedItem.ToString() == "TICKETRESTAURANT" && listBoxOrigin.SelectedItem.ToString() != "AMEXDINERS")
                        //{
                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS")
                        {
                            midCheck = itemMID.SubItems[0].Text.ToString();

                            // Check if kouvas is not routed to a bank that TIDs are not connected to
                            //TID_BankConn.TID_Check(textBox_hiddenCpy.Text.ToString(), cntBnkTid);
                            TID_BankConn.TID_Check(flg_MID, midCheck, cntBnkTid);
                            int countCHK_1 = cntBnkTid[0];
                            int countCHK_6 = cntBnkTid[1];
                            int countCHK_202 = cntBnkTid[2];
                            int countCHK_205 = cntBnkTid[3];
                            int countCHK_7 = cntBnkTid[4];
                            int countCHK_302 = cntBnkTid[5];


                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_1 > 0 && textBoxHidden_Route.Text.ToString() == "PIRAEUS")
                            {
                                string BankDescr = "PIRAEUS";
                                MessageBox.Show("You cannot choose " + BankDescr + " for KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                // button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_6 > 0 && textBoxHidden_Route.Text.ToString() == "ETHNIKI")
                            {
                                string BankDescr = "ETHNIKI";
                                MessageBox.Show("You cannot choose " + BankDescr + " for KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                //button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_202 > 0 && textBoxHidden_Route.Text.ToString() == "ALPMOR")
                            {
                                string BankDescr = "ALMOR";
                                MessageBox.Show("You cannot choose " + BankDescr + " as KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                // button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_202 > 0 && textBoxHidden_Route.Text.ToString() == "EURMOR")
                            {
                                string BankDescr = "EURMOR";
                                MessageBox.Show("You cannot choose " + BankDescr + " as KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                // button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                                                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_302 > 0 && textBoxHidden_Route.Text.ToString() == "ALPHABANK")
                            {
                                string BankDescr = "ALPHA";
                                MessageBox.Show("You cannot choose " + BankDescr + " as KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                // button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_205 > 0 && textBoxHidden_Route.Text.ToString() == "EUROBANK")
                            {
                                string BankDescr = "EUROBANK";
                                MessageBox.Show("You cannot choose " + BankDescr + " as KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                // button6_Click(button6, EventArgs.Empty);

                                return;
                            };

                            if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_7 > 0 && textBoxHidden_Route.Text.ToString() == "ATTIKA")
                            {
                                string BankDescr = "ATTIKA";
                                MessageBox.Show("You cannot choose " + BankDescr + " as KOYBAS as it's not connected to all TIDs for MID " + midCheck + ". " + "\n" + "Please choose another!");
                                //listView1.Clear();
                                //listView1.Columns.Add("Originator");
                                //listView1.Columns.Add("Route");
                                //listView1.Columns.Add("Instal-From");
                                //listView1.Columns.Add("Instal-To");
                                //   button6_Click(button6, EventArgs.Empty);

                                return;
                            };
                        }

                        
                        //Check if a bank is already chosen for ruling so to warn user
                        //if (!!listView1.Items.Cast<ListViewItem>().Any(item => item.Text == listBoxOrigin.SelectedItem.ToString()))// && listBoxOrigin.SelectedItem.ToString() != "KOYBAS")
                        //{
                        //    MessageBox.Show("Bank " + listBoxOrigin.SelectedItem.ToString() + " already chosen for ruling!!");
                        //    return;
                        //}

                        // Check if AMXEDINERS is routed to ALPHA
                        if (listBoxOrigin.SelectedItem.ToString() == "AMEXDINERS" && (textBoxHidden_Route.Text.ToString() != "ALPHABANK" && textBoxHidden_Route.Text.ToString() != "ALPMOR"))
                        {
                            MessageBox.Show("AMEX/DINERS is routed to ALPHA Bank ONLY!! " + "\n" + "You can't choose any other Bank");
                            return;
                        }

                      
                        // Check if CUP is routed to PIRAEUS
                        if (listBoxOrigin.SelectedItem.ToString() == "CUP" && textBoxHidden_Route.Text.ToString() != "PIRAEUS")
                        {
                            MessageBox.Show("CUP is routed to PIRAEUS Bank ONLY!! " + "\n" + "You can't choose any other Bank");
                            return;
                        }

                    
                        // Add to listView so to keep track of what it is done
                        lstVieItmInpu = listBoxOrigin.SelectedItem.ToString();
                        //listView1.Items.Add(new ListViewItem(new string[] { listBoxOrigin.SelectedItem.ToString(), textBoxHidden_Route.Text, textBoxFr.Text, textBoxTo.Text }));
                    }
                    // Multiple checks for xrta BIN addition / Update
                    else
                    {
                        if (radioButton_AddBin.Checked == false && radioButton_UpdBIN.Checked == false)
                        {
                            MessageBox.Show("Please choose action for BIN, Update ruling or Add extra ruling! ");
                            return;
                        }

                        if (textBoxHidden_Route.Text == "")
                        {
                            MessageBox.Show("Please choose route bank! ");
                            return;
                        }

                        comCursor_NewBIN_Chk.CommandText = "select top(1) TID,DESTPORT,BINLOWER,INSTMIN,INSTMAX from dbo.MERCHBINS_TEST where tid in (select tid from dbo.MERCHANTS_test where mid = ?) and binlower = '" + textBox_NEWBIN.Text + "'";
                        //Check if MID was chosen from ComboBox or it was added from TextBox
                        if (flag_MID == 0)
                        {
                            comCursor_NewBIN_Chk.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        }
                        else if (flag_MID == 1)
                        {
                            comCursor_NewBIN_Chk.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = textBox_hiddenCpy.Text;
                        }
                        OdbcDataReader readerCursor6 = comCursor_NewBIN_Chk.ExecuteReader();
                        if (readerCursor6.HasRows)
                        {
                            while (readerCursor6.Read())
                            {
                                destChk = readerCursor6["DESTPORT"].ToString();
                                destChkDescr = "";
                                switch (destChk)
                                {
                                    case "NET_ABC":
                                        destChkDescr = "PIRAEUS";
                                        break;
                                    case "NET_NTBN":
                                        destChkDescr = "ETHNIKI";
                                        break;
                                    case "NET_ALPMOR":
                                        destChkDescr = "ALPMOR";
                                        break;
                                    case "NET_BICALPHA":
                                        destChkDescr = "ALPHABANK";
                                        break;
                                    case "NET_CLBICEBNK":
                                        destChkDescr = "EUROBANK";
                                        break;
                                    case "NET_ATTIKA":
                                        destChkDescr = "ATTIKA";
                                        break;
                                    case "NET_EURMOR":
                                        destChkDescr = "EURMOR";
                                        break;
                                }

                                binLoChk = readerCursor6["BINLOWER"].ToString();
                                inst_MIN = Int32.Parse(readerCursor6["INSTMIN"].ToString());
                                inst_MAX = Int32.Parse(readerCursor6["INSTMAX"].ToString());
                                textBox__CurrMinInst.Text = readerCursor6["INSTMIN"].ToString();
                                textBox_CurrMaxInst.Text = readerCursor6["INSTMAX"].ToString();
                                textBox_CurrRoute.Text = destChkDescr;
                            }
                        }
                        else
                        {
                            binLoChk = textBox_NEWBIN.Text.ToString();
                        }
                        

                        if (radioButton_AddBin.Checked == true)
                        {
                            // Check for BIN ruling wih additional installments range in different Bank
                            if (inst_MAX == Int32.Parse(textBoxFr.Text.ToString()))
                            {
                                MessageBox.Show("A record for BIN " + textBox_NEWBIN.Text.ToString() + " already exists. " + "\n" + " Please check Current Min/Max so to change accordingly. ");
                                //textBox__CurrMinInst.Text = "";
                                //textBox_CurrMaxInst.Text = "";
                                //textBox_CurrRoute.Text = "";
                                return;
                            }
                            else if (inst_MAX + 1 != Int32.Parse(textBoxFr.Text.ToString()))
                            {
                                MessageBox.Show("Range for installments is wrong. 'From' should be +1 from Current Max. ");
                                //textBox__CurrMinInst.Text = "";
                                //textBox_CurrMaxInst.Text = "";
                                //textBox_CurrRoute.Text = "";
                                return;
                            }
                            else if (inst_MAX + 1 == Int32.Parse(textBoxFr.Text.ToString()))
                            {
                                dialogResNew_BIN = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                                    "Should we proceed to addition of ruling ?", "BIN Check for additional ruling", MessageBoxButtons.YesNo);
                            };
                        }

                        if (radioButton_UpdBIN.Checked == true)
                        {
                            // Check for BIN Update in Installments range
                            if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && destChkDescr != textBoxHidden_Route.Text)
                            {
                                dialogResNew_Inst = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                                   "Should we proceed to new ruling apply ?", "BIN Check for new ruling", MessageBoxButtons.YesNo);
                                //MessageBox.Show("Ruling for BIN " + readerCursor6["BINLOWER"].ToString() + " is applied to Destination Port " + readerCursor6["DESPORT"].ToString() +" .","",MessageBoxButtons.YesNo);
                            }
                            else if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && inst_MAX == Int32.Parse(textBoxTo.Text.ToString()) && destChkDescr == textBoxHidden_Route.Text)
                            {
                                MessageBox.Show("You cannot update BIN ruling with same installments range under the same route bank! ");
                                //textBox__CurrMinInst.Text = "";
                                //textBox_CurrMaxInst.Text = "";
                                //textBox_CurrRoute.Text = "";
                                return;
                            };

                        }
                        readerCursor6.Close();
                        lstVieItmInpu = textBox_NEWBIN.Text.ToString();
                        //  listView1.Items.Add(new ListViewItem(new string[] { textBox_NEWBIN.Text.ToString(), textBoxHidden_Route.Text, textBoxFr.Text, textBoxTo.Text }));
                    };
                }
                                                
                  hiddString = textBoxHidden_Route.Text;
                //if (listBoxOrigin.SelectedItem.ToString() == "CUP")
                //    hiddString = " ";
                  if ((lstVieItmInpu != "" || hiddString != "" || textBoxFr.Text != " " || textBoxTo.Text != " " || textBoxAmntFr.Text != "" || textBoxAmntTo.Text != "") && listView_MIDs.Items.Count > 0) // 090218
                  {
                      listView1.Items.Add(new ListViewItem(new string[] { lstVieItmInpu, hiddString, textBoxFr.Text, textBoxTo.Text, textBoxAmntFr.Text, textBoxAmntTo.Text }));   // Added 09/02/2018
                      button5.Enabled = true;             // 090218
                  }
            }
            else    // TIDs are chosen for ruling
            {
                button15_Click(button15, EventArgs.Empty);
            }
        }
        private void ntbnTo_ntbn_CheckedChanged(object sender, EventArgs e) { }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListViewItem item1 = new ListViewItem("Originator", 0);
            //item1.SubItems.Add("1");
            //item1.SubItems.Add("2");
            //item1.SubItems.Add("3");
            //ListViewItem item2 = new ListViewItem("Route", 1);
            //item2.SubItems.Add("1");
            //item2.SubItems.Add("2");
            //item2.SubItems.Add("3");
            //listView1.Items.AddRange(new ListViewItem[] { item1, item2 });

        }

        // Choose from ListBox 
        private void listBoxOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cur_Item = listBoxOrigin.SelectedItem.ToString();
        }

        //Select route from Combo box
        private void comboBnkRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox comboBnkRoute = (System.Windows.Forms.ComboBox)sender;
            int selectedIndex = comboBnkRoute.SelectedIndex;
            Object selectedItem = comboBnkRoute.SelectedItem;

            //MessageBox.Show("Selected Item Text: " + selectedItem.ToString() + "\n" +
            //                "Index: " + selectedIndex.ToString());
            textBoxHidden_Route.Text = selectedItem.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listView1.Clear();
            listView1.Columns.Add("Issuing");
            listView1.Columns.Add("Routing");
            listView1.Columns.Add("Instal-From");
            listView1.Columns.Add("Instal-To");
            listView1.Columns.Add("Amount-From");                   // 09/02/2018
            listView1.Columns.Add("Amount-To");                     // 09/02/2018
        }

        // Proceed to insert of TIDs to MERCHBIN file
        private void button5_Click(object sender, EventArgs e)
        {
            if (flag_TID == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (checkBox1.Checked == false)     // 090218
                {
                    button2_Click(button2, EventArgs.Empty);
                }

                string sublstItm1;
                string alpha_NotosChk = " ";
                string sublstItm2;
                string chkOriginID = " ";
                string route_hostID = " ";
                string chkOriginID2 = " ";
                string route_hostID2 = " ";
                string templKarf = "1111    ";
                int delFlag = 0;

                //Array for CUP insert
                string[,] arrCup = new string[7, 3];
                arrCup[0, 0] = "1";
                arrCup[1, 0] = "6";
                arrCup[2, 0] = "7";
                arrCup[3, 0] = "201";
                arrCup[4, 0] = "205";
                arrCup[5, 0] = "302";
                arrCup[6, 0] = "989";
                arrCup[0, 1] = "NET_ABC";
                arrCup[1, 1] = "NET_NTBN";
                arrCup[2, 1] = "NET_ATTIKA";
                arrCup[3, 1] = "NET_ALPMOR";
                arrCup[4, 1] = "NET_CLBICEBNK";
                arrCup[5, 1] = "NET_BICALPHA";
                arrCup[6, 1] = "NET_EURMOR";
                arrCup[0, 2] = "Y";
                arrCup[1, 2] = "N";
                arrCup[2, 2] = "N";
                arrCup[3, 2] = "N";
                arrCup[4, 2] = "N";
                arrCup[5, 2] = "N";
                arrCup[6, 2] = "N";
                //string destChk= " ";
                //string binLoChk= " ";
                //int inst_MAX = 0;
                //int inst_MIN = 0;

                OdbcCommand comCursor = conn.CreateCommand();
                OdbcCommand comCursor2 = conn.CreateCommand();
                OdbcCommand comCursorKoyvas = conn.CreateCommand();
                OdbcCommand comCursorTic_MMore = conn.CreateCommand();
                OdbcCommand comCursorAMEX_DINERS = conn.CreateCommand();
                OdbcCommand comCursor_NewBIN = conn.CreateCommand();
                OdbcCommand comCursor_NewInst = conn.CreateCommand();
                OdbcCommand comCursor_NewBIN_Chk = conn.CreateCommand();
                OdbcCommand comCursor_UpdGmax = conn.CreateCommand();
                OdbcCommand comCursorCUP = conn.CreateCommand();


                foreach (ListViewItem itemMID in listView_MIDs.Items)
                {
                    midDescr.Text = itemMID.SubItems[0].Text.ToString();
                    // Timer for progress bar      
                    //timer1.Start();
                    //timer1.Interval = 5;

                    foreach (ListViewItem item in listView1.Items)
                    {
                        //for (int i = 0; i < item.SubItems.Count; i++)
                        // {
                        chkOriginID = "0";
                        chkOriginID2 = "0";
                        sublstItm1 = item.SubItems[0].Text.ToString();
                        sublstItm2 = item.SubItems[1].Text.ToString();
                        textBox1.Text = item.SubItems[0].Text.ToString();
                        textBox2.Text = item.SubItems[1].Text.ToString();
                        string textBox_Fr = item.SubItems[2].Text.ToString();
                        string textBox_To = item.SubItems[3].Text.ToString();
                        string textBox_AmntFr = item.SubItems[4].Text.ToString();               // 090218
                        string textBox_AmntTo = item.SubItems[5].Text.ToString();               // 090218
                        //DialogResult dialogResNew_Inst = new DialogResult();
                        //DialogResult dialogResNew_BIN = new DialogResult();

                        //Transform Bank into HostID
                        string chkOrigin = textBox1.Text;
                        switch (chkOrigin)
                        {
                            case "PIRAEUS":
                                chkOriginID = "1";
                                route_hostID = "NET_ABC";
                                break;
                            case "ETHNIKI":
                                chkOriginID = "6";
                                route_hostID = "NET_NTBN";
                                break;
                            case "ALPMOR":
                                chkOriginID = "201";
                                route_hostID = "NET_ALPMOR";
                                break;
                            case "ALPHABANK":
                                chkOriginID = "302";
                                route_hostID = "NET_BICALPHA";
                                break;
                            case "EUROBANK":
                                chkOriginID = "205";
                                route_hostID = "NET_CLBICEBNK";
                                break;
                            case "ATTIKA":
                                chkOriginID = "7";
                                route_hostID = "NET_ATTIKA";
                                break;
                            case "EURMOR":
                                chkOriginID = "989";
                                route_hostID = "NET_EURMOR";
                                break;
                        }

                        //Transform Bank into DEST
                        string chkOrigin2 = textBox2.Text;
                        switch (chkOrigin2)
                        {
                            case "PIRAEUS":
                                chkOriginID2 = "1";
                                route_hostID2 = "NET_ABC";
                                break;
                            case "ETHNIKI":
                                chkOriginID2 = "6";
                                route_hostID2 = "NET_NTBN";
                                break;
                            case "ALPMOR":
                                chkOriginID2 = "201";
                                route_hostID2 = "NET_ALPMOR";
                                alpha_NotosChk = "201";
                                break;
                            case "ALPHABANK":
                                chkOriginID2 = "302";
                                route_hostID2 = "NET_BICALPHA";
                                alpha_NotosChk = "302";
                                break;
                            case "EUROBANK":
                                chkOriginID2 = "205";
                                route_hostID2 = "NET_CLBICEBNK";
                                break;
                            case "ATTIKA":
                                chkOriginID2 = "7";
                                route_hostID2 = "NET_ATTIKA";
                                break;
                            case "EURMOR":
                                chkOriginID2 = "989";
                                route_hostID2 = "NET_EURMOR";
                                alpha_NotosChk = "989";
                                break;
                        }

                        //textBoxFr.Text.ToString();
                        //textBoxTo.Text.ToString();

                        //Check if first time in loop so to delete all records for specific MID
                        if (delFlag == 0 && (chkOriginID == "1" || chkOriginID == "6" || chkOriginID == "201" || chkOriginID == "302" || chkOriginID == "205" || chkOriginID == "7" ||
                            chkOrigin == "KOYBAS" || chkOrigin == "MEALSANDMORE" || chkOrigin == "TICKETRESTAURANT" || chkOrigin == "AMEXDINERS"))
                            //Delete all records for specific MID
                            if(checkBox1.Checked == false)  // 090218
                            {
                            button3_Click(button3, EventArgs.Empty);
                            }

                        if (chkOriginID == "1" || chkOriginID == "6" || chkOriginID == "201" || chkOriginID == "302" || chkOriginID == "205" || chkOriginID == "7")
                        {
                            // Check if trxs will routed to same bank id so to change DESTPORT
                            if (chkOriginID == chkOriginID2)
                            {
                                if (chkOriginID == "201" || chkOriginID == "302")  // Only for Alpha
                                {
                                    comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where mid = ? and " +
                                                    "  a.uploadhostid = ? " +
                                                    "  and a.uploadhostid = b.uploadhostid " +
                                                    " and substring(desttid,1,1) <> 'P' " +
                                                    "  group by mid,tid, uploadhostname), " +
                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                    " from dbo.MERCHBINS_TEST  " +
                                                    " where TID = '1111    ' and DESTPORT = ? and " +
                                                    " binlower not in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                    "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                    "   from  merchtid,merchInfo ) ";
                                }
                                else if (chkOriginID == "6")  // Only for Ethniki
                                {
                                    comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where mid = ? and " +
                                                    "  a.uploadhostid = ? " +
                                                    "  and a.uploadhostid = b.uploadhostid  " +
                                                    " and substring(desttid,1,1) <> 'P' " +
                                                    "  group by mid,tid, uploadhostname), " +
                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                    " from dbo.MERCHBINS_TEST  " +
                                                    " where TID = '1111    ' and DESTPORT = ? and " +
                                                    " binlower not in ('549804')) " +
                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                    "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                    "   from  merchtid,merchInfo ) ";
                                }
                                else if (chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302")
                                {
                                    comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                        " (select mid,TID, uploadhostname " +
                                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                        "  where mid = ? and " +
                                                        "  a.uploadhostid = ? " +
                                                        "  and a.uploadhostid = b.uploadhostid  " +
                                                        " and substring(desttid,1,1) <> 'P' " +
                                                        "  group by mid,tid, uploadhostname), " +
                                                        "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                        " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                        " from dbo.MERCHBINS_TEST  " +
                                                        " where TID = '1111    ' and DESTPORT = ?)  " +
                                                        "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                        " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                            textBox_Fr + "," + textBox_To + "," +
                                                        "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                            textBox_AmntFr + "," + textBox_AmntTo +
                                                        "   from  merchtid,merchInfo ) ";
                                }

                            }
                            else
                            {
                                if (chkOriginID == "201" || chkOriginID == "302")  // Only for Alpha
                                {
                                    comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                          " (select mid,TID, uploadhostname " +
                                                          "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                          "  where mid = " + midDescr.Text +
                                                          "   and a.uploadhostid = " + chkOriginID2 +
                                                          "  and a.uploadhostid = b.uploadhostid  " +
                                                          " and substring(desttid,1,1) <> 'P' " +
                                                          "  group by mid,tid, uploadhostname), " +
                                                          "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                          " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                          "  from dbo.MERCHBINS_TEST  " +
                                                          "  where TID = '1111    ' and DESTPORT = '" + route_hostID + "' and " +
                                                          " binlower not in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                          "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                          "   select #TID, '" +
                                                              route_hostID2 +
                                                          "', #BINLOWER, #BINUPPER, " +
                                                           textBox_Fr + "," + textBox_To + "," +
                                                          "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                              textBox_AmntFr + "," + textBox_AmntTo +
                                                          "   from  merchtid,merchInfo ";

                                }
                                else if (chkOriginID == "6")  // Only for Ethniki
                                {
                                    comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                          " (select mid,TID, uploadhostname " +
                                                          "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                          "  where mid = ? " +  
                                                          "   and a.uploadhostid = ? " +  
                                                          "  and a.uploadhostid = b.uploadhostid  " +
                                                          " and substring(desttid,1,1) <> 'P' " +
                                                          "  group by mid,tid, uploadhostname), " +
                                                          "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                          " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                          "  from dbo.MERCHBINS_TEST  " +
                                                          "  where TID = '1111    ' and DESTPORT = ? " +
                                                          " and binlower not in ('549804')) " +
                                                          "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                          "   select #TID, '" +
                                                              route_hostID2 +
                                                          "', #BINLOWER, #BINUPPER, " +
                                                          textBox_Fr + "," + textBox_To + "," +
                                                          "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                              textBox_AmntFr + "," + textBox_AmntTo +
                                                          "   from  merchtid,merchInfo ";

                                }
                                else if (chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302")
                                {
                                   comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                            " (select mid,TID, uploadhostname " +
                                                            "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                            "  where mid =  ? " + 
                                                            "   and a.uploadhostid = ? " +  
                                                            "  and a.uploadhostid = b.uploadhostid " +
                                                            " and substring(desttid,1,1) <> 'P' " +
                                                            "  group by mid,tid, uploadhostname), " +
                                                            "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                            " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                            "  from dbo.MERCHBINS_TEST  " +
                                                            "  where TID = '1111    ' and DESTPORT = ? ) " +
                                                            "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                            "   select #TID, '" +
                                                                route_hostID2 +
                                                            "', #BINLOWER, #BINUPPER, " +
                                                            textBox_Fr + "," + textBox_To + "," +
                                                            "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                                textBox_AmntFr + "," + textBox_AmntTo +
                                                            "   from  merchtid,merchInfo ";

                                }
                            }

                            if (chkOriginID == chkOriginID2)
                            {
                                try
                                {
                                    comCursor.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                    comCursor.Parameters.Add("@DESTPORT", OdbcType.VarChar, 20).Value = route_hostID;
                                    comCursor.Connection = conn;
                                    comCursor.CommandTimeout = 300;
                                    OdbcDataReader readerCursor1 = comCursor.ExecuteReader();
                                    //readerCursor1.Dispose();
                                    comCursor.Dispose();
                                    readerCursor1.Close();
                                    delFlag++;
                                }
                                catch
                                {
                                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 010. ");
                                    delFlag = 0;
                                    button3_Click(button3, EventArgs.Empty);
                                }
                            }
                            else
                            {
                                try
                                {
                                    comCursor2.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursor2.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                    comCursor2.Parameters.Add("@DESTPORT", OdbcType.VarChar, 20).Value = route_hostID;
                                    comCursor2.Connection = conn;
                                    comCursor2.CommandTimeout = 300;
                                    OdbcDataReader readerCursor2 = comCursor2.ExecuteReader();
                                    comCursor2.Dispose();
                                    readerCursor2.Close();
                                    delFlag++;
                                }
                                catch
                                {
                                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 011. ");
                                    delFlag = 0;
                                    button3_Click(button3, EventArgs.Empty);
                                }
                            }

                        }
                        else if (chkOrigin == "KOYBAS")
                        {
                            comCursorKoyvas.CommandText =
                                            "with merchtid(#mid,#tid,#name) as  " +
                                            " (select mid,TID, uploadhostname " +
                                            "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                            "  where mid = ? and a.uploadhostid = ? " +
                                            "  and a.uploadhostid = b.uploadhostid " +
                                            " and substring(desttid,1,1) <> 'P' " +
                                            "  group by mid,tid, uploadhostname) " +
                                            "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                            " select #TID, #name, '4', '6', " +
                                            textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                            textBox_AmntFr + "," + textBox_AmntTo +
                                            " from  merchtid; " +
                                            "with merchtid(#mid,#tid,#name) as " +
                                            " (select mid,TID, uploadhostname " +
                                            "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                            "  where mid = ? and a.uploadhostid = ? " +
                                            "  and a.uploadhostid = b.uploadhostid " +
                                            " and substring(desttid,1,1) <> 'P' " +
                                            "  group by mid,tid, uploadhostname) " +
                                            "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                            " select #TID, #name, '9', '9', " +
                                            textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                            textBox_AmntFr + "," + textBox_AmntTo +
                                            " from  merchtid; " +
                                            "with merchtid(#mid,#tid,#name) as " +
                                            " (select mid,TID, uploadhostname " +
                                            "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                            "  where mid = ? and a.uploadhostid = ? " +
                                            "  and a.uploadhostid = b.uploadhostid " +
                                            " and substring(desttid,1,1) <> 'P' " +
                                            "  group by mid,tid, uploadhostname) " +
                                            "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                            " select #TID, #name, '222100', '272099', " +
                                            textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                            textBox_AmntFr + "," + textBox_AmntTo +
                                            " from  merchtid; ";
                            try
                            {
                                comCursorKoyvas.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursorKoyvas.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursorKoyvas.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursorKoyvas.Connection = conn;
                                OdbcDataReader readerCursor2 = comCursorKoyvas.ExecuteReader();
                                comCursorKoyvas.Dispose();
                                readerCursor2.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 012. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }

                        }
                        else if (chkOrigin == "MEALSANDMORE")
                        {
                            comCursorTic_MMore.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                    "  where mid = ? and a.uploadhostid = ? " +
                                                    "  and a.uploadhostid = b.uploadhostid " +
                                                    " and substring(desttid,1,1) <> 'P' " +
                                                    "  group by mid,tid, uploadhostname) " +
                                                    "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                    " select #TID, #name, '502259', '502259', 0, 1, 0, 99, 'Y', " +
                                                    textBox_AmntFr + "," + textBox_AmntTo +
                                                    " from  merchtid ; ";
                            try
                            {
                                comCursorTic_MMore.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorTic_MMore.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursorTic_MMore.Connection = conn;
                                OdbcDataReader readerCursor3 = comCursorTic_MMore.ExecuteReader();
                                comCursorTic_MMore.Dispose();
                                readerCursor3.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 013. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }

                        }
                        else if (chkOrigin == "TICKETRESTAURANT")
                        {
                            comCursorTic_MMore.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where mid = ? and a.uploadhostid = ? " +
                                                    "  and a.uploadhostid = b.uploadhostid " +
                                                    " and substring(desttid,1,1) <> 'P' " +
                                                    "  group by mid,tid, uploadhostname) " +
                                                    "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                    " select #TID, #name, '534228', '534228', 0, 1, 0, 99, 'Y', "  +
                                                      textBox_AmntFr + "," + textBox_AmntTo +
                                                    " from  merchtid ; ";
                            try
                            {
                                comCursorTic_MMore.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorTic_MMore.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursorTic_MMore.Connection = conn;
                                OdbcDataReader readerCursor4 = comCursorTic_MMore.ExecuteReader();
                                comCursorTic_MMore.Dispose();
                                readerCursor4.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 014. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }

                        }
                        else if (chkOrigin == "AMEXDINERS" && chkOriginID2 == "201")
                        {
                            comCursorAMEX_DINERS.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                        " (select mid,TID, uploadhostname " +
                                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                        "  where mid = ? and " +
                                                        "  a.uploadhostid = '201'  " +
                                                        "  and a.uploadhostid = b.uploadhostid " +
                                                        " and substring(desttid,1,1) <> 'P' " +
                                                        "  group by mid,tid, uploadhostname), " +
                                                        "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                        " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                        " from dbo.MERCHBINS_TEST  " +
                                                        " where TID = '1111    ' and DESTPORT = 'NET_ALPMOR'  and " +
                                                        " binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                        "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                        " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                            textBox_Fr + "," + textBox_To + "," +
                                                        "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                            textBox_AmntFr + "," + textBox_AmntTo +
                                                        "   from  merchtid,merchInfo ) ";

                            try
                            {
                                comCursorAMEX_DINERS.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorAMEX_DINERS.Connection = conn;
                                OdbcDataReader readerCursor5 = comCursorAMEX_DINERS.ExecuteReader();
                                comCursorAMEX_DINERS.Dispose();
                                readerCursor5.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 015. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }
                        }
                        else if (chkOrigin == "AMEXDINERS" && chkOriginID2 == "302")
                        {
                            comCursorAMEX_DINERS.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                        " (select mid,TID, uploadhostname " +
                                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                        "  where mid = ? and " +
                                                        "  a.uploadhostid = '302' " +
                                                        "  and a.uploadhostid = b.uploadhostid " +
                                                        " and substring(desttid,1,1) <> 'P' " +
                                                        "  group by mid,tid, uploadhostname), " +
                                                        "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                        " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                        " from dbo.MERCHBINS_TEST  " +
                                                        " where TID = '1111    ' and DESTPORT = 'NET_BICALPHA' and " +
                                                        " binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                        "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                        " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                            textBox_Fr + "," + textBox_To + "," +
                                                        "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                            textBox_AmntFr + "," + textBox_AmntTo +
                                                        "   from  merchtid,merchInfo ) ";

                            try
                            {
                                comCursorAMEX_DINERS.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorAMEX_DINERS.Connection = conn;
                                OdbcDataReader readerCursor5 = comCursorAMEX_DINERS.ExecuteReader();
                                comCursorAMEX_DINERS.Dispose();
                                readerCursor5.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 015. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }
                        }
                        else if (chkOrigin == "CUP")
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                comCursorCUP.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                           " (select mid,TID, uploadhostname " +
                                                           "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                           "  where mid = ? " +
                                                           "   and a.uploadhostid = ? " +
                                                           "  and a.uploadhostid = b.uploadhostid " +
                                                           " and substring(desttid,1,1) <> 'P' " +
                                                           "  group by mid,tid, uploadhostname), " +
                                                           "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                           " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                           "  from dbo.MERCHBINS_TEST  " +
                                                           "  where TID = '2222    ' and DESTPORT = 'NET_CUP') " +
                                                           "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                           "   select #TID, '" +
                                                               arrCup[i, 1] +
                                                           "', #BINLOWER, #BINUPPER, 0, 1, " +
                                                           "   #GRACEMIN, #GRACEMAX,'" + arrCup[i, 2] +
                                                           "', " + textBox_AmntFr + "," + textBox_AmntTo +
                                                           "   from  merchtid,merchInfo " +
                                                           "   order by #TID ";
                                try
                                {
                                    comCursorCUP.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursorCUP.Parameters.Add("@uploadhost", OdbcType.Int).Value = arrCup[i, 0];
                                    comCursorCUP.Connection = conn;
                                    comCursorCUP.CommandTimeout = 300;
                                    OdbcDataReader readerCursor10 = comCursorCUP.ExecuteReader();
                                    comCursorCUP.Dispose();
                                    readerCursor10.Close();

                                }
                                catch
                                {
                                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 016C. ");
                                    delFlag = 0;
                                    button3_Click(button3, EventArgs.Empty);
                                    return;
                                }

                            }
                        }
                        else if (chkOriginID != "1" && chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302" && chkOriginID != "205" && chkOriginID != "7" &&
                            chkOrigin != "KOYBAS" && chkOrigin != "MEALSANDMORE" && chkOrigin != "TICKETRESTAURANT" && chkOrigin != "AMEXDINERS")
                        {
                            //comCursor_NewBIN_Chk.CommandText = "select top(1) TID,DESTPORT,BINLOWER,INSTMIN,INSTMAX from dbo.MERCHBINS_TEST where tid in (select tid from dbo.MERCHANTS_test where mid = ?) and binlower = '" + chkOrigin + "'";
                            //comCursor_NewBIN_Chk.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                            //OdbcDataReader readerCursor6 = comCursor_NewBIN_Chk.ExecuteReader();
                            //while (readerCursor6.Read())
                            //{
                            //     destChk = readerCursor6["DESTPORT"].ToString();
                            //     binLoChk = readerCursor6["BINLOWER"].ToString();
                            //     inst_MIN = Int32.Parse(readerCursor6["INSTMIN"].ToString());
                            //     inst_MAX = Int32.Parse(readerCursor6["INSTMAX"].ToString());
                            //}


                            //// Check for BIN ruling wih additional installments range in different Bank
                            //if (inst_MAX == Int32.Parse(textBoxFr.Text.ToString()))
                            //{
                            //    MessageBox.Show("A record for BIN " + chkOrigin.ToString() + " already exists. " + "\n" + " Please change Installments range so to add it. ");
                            //    return;
                            //}
                            //else if (inst_MAX + 1 != Int32.Parse(textBoxFr.Text.ToString()))
                            //{
                            //    MessageBox.Show("Range for installments is wrong. There should be a logical continuity of +1 in installments . ");
                            //        return;
                            //}
                            //else if(inst_MAX + 1 == Int32.Parse(textBoxFr.Text.ToString()))
                            //{
                            //    dialogResNew_BIN = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                            //        "Should we proceed to addition of ruling ?", "BIN Check for additional ruling", MessageBoxButtons.YesNo); 
                            //};


                            //// Check for BIN Update in Installments range
                            //if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && destChk != route_hostID2)
                            //{
                            //    dialogResNew_Inst = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                            //        "Should we proceed to new ruling apply ?", "BIN Check for new ruling", MessageBoxButtons.YesNo);
                            //    //MessageBox.Show("Ruling for BIN " + readerCursor6["BINLOWER"].ToString() + " is applied to Destination Port " + readerCursor6["DESPORT"].ToString() +" .","",MessageBoxButtons.YesNo);
                            //}
                            //else if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && destChk == route_hostID2)
                            //{
                            //    MessageBox.Show("You cannot add same BIN with different installments range under the same Bank! ");
                            //    return;
                            //};

                            //readerCursor6.Close();

                            if (dialogResNew_Inst == DialogResult.Yes)
                            {
                                comCursor_NewInst.CommandText = "delete from dbo.MERCHBINS_TEST where tid in (select tid from dbo.MERCHANTS_test  where mid = ?) and binlower = '" + chkOrigin + "';" +
                                                               "with merchtid(#mid,#tid,#name) as " +
                                                    " (select mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where mid = ? and a.uploadhostid = ? " +
                                                    "  and a.uploadhostid = b.uploadhostid " +
                                                    " and substring(desttid,1,1) <> 'P' " +
                                                    "  group by mid,tid, uploadhostname) " +
                                                    "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                    " select #TID, #name, '" + chkOrigin + "', '" + chkOrigin + "'," +
                                                    textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                                    textBox_AmntFr + "," + textBox_AmntTo +
                                                    " from  merchtid ; ";
                                try
                                {
                                    comCursor_NewInst.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursor_NewInst.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursor_NewInst.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                    comCursor_NewInst.Connection = conn;
                                    OdbcDataReader readerCursor7 = comCursor_NewInst.ExecuteReader();
                                    comCursor_NewInst.Dispose();
                                    readerCursor7.Close();
                                }
                                catch
                                {
                                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 016. ");
                                    delFlag = 0;
                                    button3_Click(button3, EventArgs.Empty);
                                    return;
                                }
                            }

                            if (dialogResNew_BIN == DialogResult.Yes)
                            {
                                comCursor_NewBIN.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                        " (select mid,TID, uploadhostname " +
                                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                        "  where mid = ? and a.uploadhostid = ? " +
                                                        "  and a.uploadhostid = b.uploadhostid " +
                                                        " and substring(desttid,1,1) <> 'P' " +
                                                        "  group by mid,tid, uploadhostname) " +
                                                        "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                        " select #TID, #name, '" + chkOrigin + "', '" + chkOrigin + "'," +
                                                        textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                        " from  merchtid ; ";
                                try
                                {
                                    comCursor_NewBIN.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                    comCursor_NewBIN.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                    comCursor_NewBIN.Connection = conn;
                                    OdbcDataReader readerCursor9 = comCursor_NewBIN.ExecuteReader();
                                    comCursor_NewBIN.Dispose();
                                    readerCursor9.Close();
                                }
                                catch
                                {
                                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 017. ");
                                    delFlag = 0;
                                    button3_Click(button3, EventArgs.Empty);
                                    return;
                                }
                            }
                        }

                    }

                    // Check for Notos MID and insert BIN '9999914'
                    if (midDescr.Text == "000000110000237" || midDescr.Text == "000000110000219" || midDescr.Text == "000000110000286" || midDescr.Text == "000000110000205" || midDescr.Text == "000000110000202" || midDescr.Text == "000000110000236" || midDescr.Text == "000000110000292" || midDescr.Text == "000000110000296" || midDescr.Text == "000000110000299" || midDescr.Text == "000000110000300" || midDescr.Text == "000000110000285" || midDescr.Text == "000000110000291" || midDescr.Text == "000000110000221" || midDescr.Text == "000000110000207" || midDescr.Text == "000000110000212" || midDescr.Text == "000000110000220" || midDescr.Text == "000000110000278" || midDescr.Text == "000000110000234" || midDescr.Text == "000000110000210" || midDescr.Text == "000000110000230" || midDescr.Text == "000000110000274" || midDescr.Text == "000000110000213" || midDescr.Text == "000000110000216" || midDescr.Text == "000000110000217" || midDescr.Text == "000000110000279" || midDescr.Text == "000000110000275" || midDescr.Text == "000000110000288" || midDescr.Text == "000000110000229" || midDescr.Text == "000000110000295" || midDescr.Text == "000000110000235" || midDescr.Text == "000000110000270" || midDescr.Text == "000000110000242" || midDescr.Text == "000000110000297" || midDescr.Text == "000000110000239" || midDescr.Text == "000000110000238" || midDescr.Text == "000000110000243" || midDescr.Text == "000000110000240" || midDescr.Text == "000000110000271" || midDescr.Text == "000000110000241" || midDescr.Text == "000000110000208" || midDescr.Text == "000000110000233" || midDescr.Text == "000000110000298" || midDescr.Text == "000000110000206" || midDescr.Text == "000000110000226" || midDescr.Text == "000000110000201" || midDescr.Text == "000000110000224" || midDescr.Text == "000000110000289" || midDescr.Text == "000000110000203")
                    {
                        OdbcCommand comCount_NotosChk = new OdbcCommand("select count(*) from MERCHbins_test where mid = ? and tid not in " +
                                                              "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 302) and binlower = '999914'", conn);
                        comCount_NotosChk.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCount_NotosChk.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        int countCHK_Notos = Convert.ToInt32(comCount_NotosChk.ExecuteScalar());

                        if (countCHK_Notos > 0)
                        {
                            OdbcCommand comCursorNotos = new OdbcCommand("with merchtid(#mid,#tid,#name) as " +
                                                       " (select mid,TID, uploadhostname " +
                                                       "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                       "  where mid = ? and a.uploadhostid = '" + alpha_NotosChk + "'" +
                                                       "  and a.uploadhostid = b.uploadhostid) " +
                                                       "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                       " select #TID, #name, '999914', '999914', 0, 1, 0, 99, 'Y',  0 , 999999999 " +
                                                       " from  merchtid ; ", conn);
                            try
                            {
                                comCursorNotos.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                                comCursorNotos.Connection = conn;
                                OdbcDataReader readerCursor10 = comCursorNotos.ExecuteReader();
                                comCursorNotos.Dispose();
                                readerCursor10.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 018. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }

                        }
                    };

                    // Update GMAXINST from Merchants file
                    comCursor_UpdGmax.CommandText = "with  fndMaxPBnk (tid_, maxinst,desport) as " +
                                                    "(select tid,max(instmax) max_inst, " +
                                                     "case destport " +
                                                     "when 'NET_ABC'  THEN '1' " +
                                                     "when 'NET_NTBN' THEN '6' " +
                                                     "when 'NET_ALPMOR' THEN '201' " +
                                                     "when 'NET_BICALPHA' THEN '302' " +
                                                     "when 'NET_CLBICEBNK'  THEN '205' " +
                                                     "when 'NET_ATTICA' THEN '7' " +
                                                     "when 'NET_EURMOR' THEN '989' " +
                                                     "end des_port " +
                                                     "from dbo.MERCHBINS_TEST where tid in(select tid from dbo.MERCHANTS_test where mid = ?) " +
                                                     "group by tid,destport) " +
                                                    "update MERCHANTS_test " +
                                                    "set gmaxinst = maxinst " +
                                                    "from fndMaxPBnk " +
                                                    "where tid_ = tid and uploadhostid = desport and substring(desttid,1,1) <> 'P' ";
                    try
                    {
                        comCursor_UpdGmax.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCursor_UpdGmax.Connection = conn;
                        OdbcDataReader readerCursor8 = comCursor_UpdGmax.ExecuteReader();
                        comCursor_UpdGmax.Dispose();
                        readerCursor8.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 019. ");
                        delFlag = 0;
                        button3_Click(button3, EventArgs.Empty);
                        return;
                    }

                    //MessageBox.Show("Insert completed for MID: " + midDescr.Text);
                    //timer1.Stop();
                   // label8.Text = "Insert completed for MID: " + midDescr.Text;
                    delFlag = 0;
                    textBox__CurrMinInst.Text = "";
                    textBox_CurrMaxInst.Text = "";
                    textBox_CurrRoute.Text = "";
                    textBox_NEWBIN.Text = "";
                    comboBox_InstalFr.SelectedText = "0";
                    comboBox_InstalTo.SelectedText = "0";
                }

                textBox_NEWBIN.Enabled = true;
                button11.Enabled = true;
                Cursor.Current = Cursors.Default;

                //listView_MIDs.Clear();
                //listView_MIDs.Columns.Add("MID/TID");
                //listView_MIDs.Columns.Add("Route#1");
                //listView_MIDs.Columns.Add("Route#2");
                //listView_MIDs.Columns.Add("Route#3");
                //listView_MIDs.Columns.Add("Route#4");
                //listView_MIDs.Columns.Add("Route#5");

                listView1.Clear();
                listView1.Columns.Add("Issuing");
                listView1.Columns.Add("Routing");
                listView1.Columns.Add("Instal-From");
                listView1.Columns.Add("Instal-To");
                listView1.Columns.Add("Amount-From");               // 090218
                listView1.Columns.Add("Amount-To");                 // 090218

                //label7.ForeColor = System.Drawing.Color.Black;
                //label7.Text = "FOR PROCESS";
                label5.ForeColor = System.Drawing.Color.Black;
                label5.ResetText();
                //label8.ForeColor = System.Drawing.Color.Black;
                //label8.Text = "READY";
                delFlag = 0;

                return;
            }
            else    // TIDs are chosen for ruling
            {
                button18_Click(button18, EventArgs.Empty);
            }
        }

        private void comboBox_InstalFr_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            Object selectedItem = cmb.SelectedItem;
            textBoxFr.Text = selectedItem.ToString();
        }

        private void comboBox_InstalTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            Object selectedItem = cmb.SelectedItem;
            textBoxTo.Text = selectedItem.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
                listView1.Items.RemoveAt(listView1.Items.Count - 1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listView2.Clear();
            listView2.Columns.Add("Bank");
            listView2.Columns.Add("Load Balance");
            listView2.Columns.Add("GR Value");
            listView2.Columns.Add("EU Value");
            listView2.Columns.Add("Other Value");
            listView2.Columns.Add("Routing Type");
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
       //     if ((comboBoxLD.Text.Equals("")) || (textBox_lbNbr.Text.Equals("")))
           if ((comboBoxLD.Text.Equals("")))
            {
                MessageBox.Show("Please choose Bank");
                return;
            }

            if (radioButton_Amnt.Checked == true)
            {
                totals_CntAmnt = "T";
                textBoxlb_other.Text = "0";
                textBoxlb_gr.Text = "0";
                textBoxlb_eu.Text = "0";
            }
            if (radioButton_Cnt.Checked == true)
            {
                totals_CntAmnt = "C";
                textBoxlb_other.Text = "0";
                textBoxlb_gr.Text = "0";
                textBoxlb_eu.Text = "0";
            }
            if (radioButton_Pri.Checked == true)
            {
                totals_CntAmnt = "P";
                textBox_lbNbr.Text = "1";
            }


            if (listView2.Items.Cast<ListViewItem>().Any(item => item.Text == comboBoxLD.Text))
            {
                MessageBox.Show("Chosen bank is already added in list!");
            }
            else
            {
                listView2.Items.Add(new ListViewItem(new string[] { comboBoxLD.Text, textBox_lbNbr.Text, textBoxlb_gr.Text, textBoxlb_eu.Text, textBoxlb_other.Text, totals_CntAmnt }));
                button9.Enabled = true;  // 090218
                textBoxlb_other.Text = "0";
                textBoxlb_gr.Text = "0";
                textBoxlb_eu.Text = "0";
                textBox_lbNbr.Text = "0";
            }
            // Add to listView so to keep track of what it is done
            //listView2.Items.Add(new ListViewItem(new string[] { comboBoxLD.Text, comboBoxPrcntge.Text, totals_CntAmnt }));
            //listView2.Items.Add(new ListViewItem(new string[] { comboBoxLD.Text, textBox_lbNbr.Text, totals_CntAmnt }));
        }

        //private void button9_Click(object sender, EventArgs e)   /// OLD way
        //{
        //    if (flag_TID == 0)
        //    {
        //        Array.Clear(loadBalArr, 5, 3);
        //        //int i = 0;

        //        foreach (ListViewItem itemMID2 in listView_MIDs.Items)
        //        {
        //            midDescr.Text = itemMID2.SubItems[0].Text.ToString();
        //            int i = 0;
        //            foreach (ListViewItem item in listView2.Items)
        //            {
        //                string tmpLD_Descr = item.SubItems[0].Text.ToString();
        //                switch (tmpLD_Descr)
        //                {
        //                    case "PIRAEUS":
        //                        loadBalArr[i, 0] = "NET_ABC";
        //                        break;
        //                    case "ETHNIKI":
        //                        loadBalArr[i, 0] = "NET_NTBN";
        //                        break;
        //                    case "ALPHABANK":
        //                        loadBalArr[i, 0] = "NET_CLBICALPHA";
        //                        break;
        //                    case "EUROBANK":
        //                        loadBalArr[i, 0] = "NET_CLBICEBNK";
        //                        break;
        //                    case "ATTIKA":
        //                        loadBalArr[i, 0] = "NET_ATTIKA";
        //                        break;
        //                }

        //                loadBalArr[i, 1] = item.SubItems[1].Text.ToString();
        //                loadBalArr[i, 2] = item.SubItems[2].Text.ToString();
        //                i++;
        //            }

        //            OdbcCommand comLdBal_Del = conn.CreateCommand();
        //            OdbcCommand comLdBal_Cursor = conn.CreateCommand();

        //            // Delete LB from LoadBal file so to insert new selections
        //            comLdBal_Del.CommandText = "delete dbo.LOADBALANCE_test where balancegroup in (select 'M'+tid from dbo.MERCHANTS_test where mid = ?)";
        //            comLdBal_Del.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
        //            OdbcDataReader readerCursor5 = comLdBal_Del.ExecuteReader();
        //            readerCursor5.Close();

        //            string ldBl0_0 = loadBalArr[0, 0].ToString();
        //            string ldBl0_1 = loadBalArr[0, 1].ToString();
        //            string ldBl0_2 = loadBalArr[0, 2].ToString();
        //            string ldBl1_0 = loadBalArr[1, 0].ToString();
        //            string ldBl1_1 = loadBalArr[1, 1].ToString();
        //            string ldBl1_2 = loadBalArr[1, 2].ToString();
        //            string ldBl2_0 = loadBalArr[2, 0].ToString();
        //            string ldBl2_1 = loadBalArr[2, 1].ToString();
        //            string ldBl2_2 = loadBalArr[2, 2].ToString();
        //            string ldBl3_0 = loadBalArr[3, 0].ToString();
        //            string ldBl3_1 = loadBalArr[3, 1].ToString();
        //            string ldBl3_2 = loadBalArr[3, 2].ToString();
        //            string ldBl4_0 = loadBalArr[4, 0].ToString();
        //            string ldBl4_1 = loadBalArr[4, 1].ToString();
        //            string ldBl4_2 = loadBalArr[4, 2].ToString();


        //            comLdBal_Cursor.CommandText =
        //                                    "drop TABLE [zacrpt_test].[dbo].[tmp_LB]; " +
        //                                    "CREATE TABLE [zacrpt_test].[dbo].[tmp_LB] " +
        //                                    "(ID int IDENTITY(1,1) PRIMARY KEY, TID nvarchar(16)); " +
        //                                    "declare @NumberRecords int, @RowCount int " +
        //                                    "declare @TID varchar(16) " +
        //                                    "declare @BALGROUP varchar(16) " +
        //                                        "Insert into [zacrpt_test].[dbo].[tmp_LB](tid) " +
        //                                          "Select distinct('M'+TID) FROM dbo.MERCHANTS_test WHERE mid = ? " +
        //                                    "set @NumberRecords = (select count(*) from [zacrpt_test].[dbo].[tmp_LB]) " +
        //                                    "set @RowCount = 1 " +
        //                                    "While @RowCount <= @NumberRecords " +
        //                                        "Begin " +
        //                                            "select distinct @tid=tid " +
        //                                            "from [zacrpt_test].[dbo].[tmp_LB] " +
        //                                            "where id = @RowCount " +
        //                                              "set @BALGROUP = @tid " +
        //                                              " INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
        //                                              " VALUES (@BALGROUP, '" +
        //                                               ldBl0_0 + "' , '" + ldBl0_2 + "','" + ldBl0_1 + "') " +
        //                                              " INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
        //                                              " VALUES (@BALGROUP, '" +
        //                                                ldBl1_0 + "','" + ldBl1_2 + "','" + ldBl1_1 + "') " +
        //                                              " INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
        //                                              " VALUES (@BALGROUP, '" +
        //                                               ldBl2_0 + "','" + ldBl2_2 + "','" + ldBl2_1 + "') " +
        //                                              " INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
        //                                              " VALUES (@BALGROUP, '" +
        //                                                ldBl3_0 + "','" + ldBl3_2 + "','" + ldBl3_1 + "') " +
        //                                              " INSERT INTO [zacrpt_test].[dbo].[LOADBALANCE_test] (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
        //                                              " VALUES (@BALGROUP, '" +
        //                                                ldBl4_0 + "','" + ldBl4_2 + "','" + ldBl4_1 + "') " +
        //                                              "set @rowcount = @rowcount + 1  " +
        //                                              "end";

        //            comLdBal_Cursor.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
        //            comLdBal_Cursor.Connection = conn;
        //            OdbcDataReader readerCursor6 = comLdBal_Cursor.ExecuteReader();
        //            comLdBal_Cursor.Dispose();
        //            readerCursor6.Close();

        //            MessageBox.Show("Load Balance Completed for MID: " + midDescr.Text);
        //        }
        //        listView2.Clear();
        //        listView2.Columns.Add("Bank");
        //        listView2.Columns.Add("Load Balance");
        //        listView2.Columns.Add("Totals By");

        //        return;
        //    }
        //    else
        //    {
        //        button19_Click(button19, EventArgs.Empty);
        //    }
        //}

        private void button9_Click(object sender, EventArgs e)
        {
            if (flag_TID == 0)
            {
                Array.Clear(loadBalArr, 5, 7);
                //int i = 0;
                string sqlCrtQry = "CREATE TABLE [zacrpt_test].[dbo].[tmp_LB]  (ID int IDENTITY(1,1) PRIMARY KEY, TID nvarchar(16), uploadhost_id varchar(1)) ";   // 090218
                string sqlDelTmpQry = "DELETE from [zacrpt_test].[dbo].[tmp_LB] ";          // 090218

                // Check for table [zacrpt_test].[dbo].[tmp_LB] if exists so to create or not    090218
                bool isExists;
                const string sqlStmnt = @"Select count(*) from [zacrpt_test].[dbo].[tmp_LB]";

                try
                {
                    using (OdbcCommand chkExistance = new OdbcCommand(sqlStmnt, conn))
                    {
                        chkExistance.ExecuteScalar();
                        isExists = true;
                    }
                }
                catch
                {
                    isExists = false;
                }
                
                if (isExists == false)
                {
                    OdbcCommand sqlcmdCrt = new OdbcCommand();
                    sqlcmdCrt.Connection = conn;
                    sqlcmdCrt.CommandTimeout = 0;
                    sqlcmdCrt.CommandType = CommandType.Text;
                    //sqlcmdDel.CommandText = sqlDelQry;
                    sqlcmdCrt.CommandText = sqlCrtQry;
                    OdbcDataReader rdrCrt = sqlcmdCrt.ExecuteReader();
                    rdrCrt.Close();
                    sqlcmdCrt.Dispose();
                }
                else
                {
                    OdbcCommand sqlcmdDel = new OdbcCommand();
                    sqlcmdDel.Connection = conn;
                    sqlcmdDel.CommandTimeout = 0;
                    sqlcmdDel.CommandType = CommandType.Text;
                    //sqlcmdDel.CommandText = sqlDelQry;
                    sqlcmdDel.CommandText = sqlDelTmpQry;
                    OdbcDataReader rdrDel = sqlcmdDel.ExecuteReader();
                    rdrDel.Close();
                    sqlcmdDel.Dispose();
                }

              

                foreach (ListViewItem itemMID2 in listView_MIDs.Items)
                {
                    midDescr.Text = itemMID2.SubItems[0].Text.ToString();

                    // Delete LB from LoadBal file so to insert new selections   090218
                    OdbcCommand comLdBal_Del = conn.CreateCommand();
                    comLdBal_Del.CommandText = "delete dbo.LOADBALANCE_test where balancegroup in (select 'M'+tid from dbo.MERCHANTS_test where mid = ?)";
                    comLdBal_Del.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
                    OdbcDataReader readerCursor5 = comLdBal_Del.ExecuteReader();
                    readerCursor5.Close();


                    
                    //Insert data for mid to temp file      090218
                    OdbcCommand lbInsert_ = conn.CreateCommand();
                    lbInsert_.CommandText  = "delete from [zacrpt_test].[dbo].[tmp_LB]; " +
                                             "Insert into [zacrpt_test].[dbo].[tmp_LB](tid, uploadhost_id) " +
                                             "Select distinct('M'+TID), uploadhostid FROM dbo.MERCHANTS_test WHERE mid = ? ";
                    lbInsert_.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
                    OdbcDataReader readerLb_Temp = lbInsert_.ExecuteReader();
                    readerLb_Temp.Close();
                        


                    int i = 0;
                    foreach (ListViewItem item in listView2.Items)
                    {
                        string tmpLD_Descr = item.SubItems[0].Text.ToString();
                        switch (tmpLD_Descr)
                        {
                            case "PIRAEUS":
                                loadBalArr[i, 0] = "NET_ABC";
                                loadBalArr[i, 3] = "1";
                                break;
                            case "ETHNIKI":
                                loadBalArr[i, 0] = "NET_NTBN";
                                loadBalArr[i, 3] = "6";
                                break;
                            case "ALPMOR":
                                loadBalArr[i, 0] = "NET_ALPMOR";
                                loadBalArr[i, 3] = "201";
                                break;
                            case "ALPHABANK":
                                loadBalArr[i, 0] = "NET_BICALPHA";
                                loadBalArr[i, 3] = "302";
                                break;
                            case "EUROBANK":
                                loadBalArr[i, 0] = "NET_CLBICEBNK";
                                loadBalArr[i, 3] = "205";
                                break;
                            case "ATTIKA":
                                loadBalArr[i, 0] = "NET_ATTIKA";
                                loadBalArr[i, 3] = "7";
                                break;
                            case "EURMOR":
                                loadBalArr[i, 0] = "NET_EURMOR";
                                loadBalArr[i, 3] = "989";
                                break;

                        }

                        //loadvalue
                        loadBalArr[i, 1] = item.SubItems[1].Text.ToString();
                        //Type
                        loadBalArr[i, 2] = item.SubItems[5].Text.ToString();
                        //gr
                        loadBalArr[i, 4] = item.SubItems[2].Text.ToString();
                        //eu
                        loadBalArr[i, 5] = item.SubItems[3].Text.ToString();
                        //other
                        loadBalArr[i, 6] = item.SubItems[4].Text.ToString();
                        //i++;     090218

                                                
                        OdbcCommand comLdBal_Cursor = conn.CreateCommand();
                                               

                        string ldBl0_0 = loadBalArr[0, 0].ToString();
                        string ldBl0_1 = loadBalArr[0, 1].ToString();
                        string ldBl0_2 = loadBalArr[0, 2].ToString();
                        string ldBl0_4 = loadBalArr[0, 4].ToString();
                        string ldBl0_5 = loadBalArr[0, 5].ToString();
                        string ldBl0_6 = loadBalArr[0, 6].ToString();
                        string ldBl0_Host = loadBalArr[0, 3].ToString();

                        // 090218
                        comLdBal_Cursor.CommandText = "with LBTEmp(mTid, UploadID) as " +
                                                       "(select tid, uploadhost_id from [dbo].[tmp_LB]) " +
                                                        " insert into loadbalance_test(BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE, GR_VAL, EU_VAL, OTHER_VAL) " +
                                                        " select mTid, '" + ldBl0_0 + "' , '" + ldBl0_2 + "','" + ldBl0_1 + "','" + ldBl0_4 + "','" + ldBl0_5 + "','" +ldBl0_6 + "'" +
                                                       " from LBTemp " +
                                                       " where UploadID = ? ";

                        comLdBal_Cursor.Parameters.Add("uploadhostid", OdbcType.Char).Value = ldBl0_Host;
                        comLdBal_Cursor.Connection = conn;
                        OdbcDataReader readerCursor6 = comLdBal_Cursor.ExecuteReader();
                        comLdBal_Cursor.Dispose();
                        readerCursor6.Close();
                    }
                    MessageBox.Show("Load Balance Completed for MID: " + midDescr.Text);
                
                }
                listView2.Clear();
                listView2.Columns.Add("Bank");
                listView2.Columns.Add("Load Balance");
                listView2.Columns.Add("GR Value");
                listView2.Columns.Add("EU Value");
                listView2.Columns.Add("Other Value");
                listView2.Columns.Add("Routing Type");

                return;
            }
            else
            {
                button19_Click(button19, EventArgs.Empty);
            }
        }

        private void button16_Click(object sender, EventArgs e)  // Not used
        {
            foreach (ListViewItem item in listView_MIDs.Items)
            {
                int countCHK_1 = 0;
                int countCHK_6 = 0;
                int countCHK_202 = 0;
                int countCHK_205 = 0;
                int countCHK_7 = 0;

                // Check connected banks
                OdbcCommand comCount_1 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                                          "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 1)", conn);
                comCount_1.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                comCount_1.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                countCHK_1 = Convert.ToInt32(comCount_1.ExecuteScalar());
                if (countCHK_1 > 0)
                {
                    textBoxABC.BackColor = System.Drawing.Color.Red;
                    textBoxABC.Text = countCHK_1.ToString();
                }
                else
                {
                    textBoxABC.Text = countCHK_1.ToString();
                }


                OdbcCommand comCount_6 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                                           "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 6)", conn);
                comCount_6.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                comCount_6.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                countCHK_6 = Convert.ToInt32(comCount_6.ExecuteScalar());
                if (countCHK_6 > 0)
                {
                    textBoxNTBN.BackColor = System.Drawing.Color.Red;
                    textBoxNTBN.Text = countCHK_6.ToString();
                }
                else
                {
                    textBoxNTBN.Text = countCHK_6.ToString();
                }


                OdbcCommand comCount_202 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                               "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 201)", conn);
                comCount_202.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                comCount_202.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                countCHK_202 = Convert.ToInt32(comCount_202.ExecuteScalar());

                if (countCHK_202 > 0)
                {
                    textBoxALPHA.BackColor = System.Drawing.Color.Red;
                    textBoxALPHA.Text = countCHK_202.ToString();
                }
                else
                {
                    textBoxALPHA.Text = countCHK_202.ToString();
                }


                OdbcCommand comCount_205 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                               "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 205)", conn);
                comCount_205.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                comCount_205.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                countCHK_205 = Convert.ToInt32(comCount_205.ExecuteScalar());
                if (countCHK_205 > 0)
                {
                    textBoxEBNK.BackColor = System.Drawing.Color.Red;
                    textBoxEBNK.Text = countCHK_205.ToString();
                }
                else
                {
                    textBoxEBNK.Text = countCHK_205.ToString();
                }

                OdbcCommand comCount_7 = new OdbcCommand("select count(*) from MERCHANTS_test where mid = ? and tid not in " +
                                               "(select tid from MERCHANTS_test where mid = ? and uploadhostid = 7)", conn);
                comCount_7.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                comCount_7.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = item.SubItems[0].Text.ToString();
                countCHK_7 = Convert.ToInt32(comCount_7.ExecuteScalar());
                if (countCHK_7 > 0)
                {
                    textBoxEBNK.BackColor = System.Drawing.Color.Red;
                    textBoxEBNK.Text = countCHK_7.ToString();
                }
                else
                {
                    textBoxEBNK.Text = countCHK_205.ToString();
                }

                if (countCHK_1 > 0 || countCHK_6 > 0 || countCHK_202 > 0 || countCHK_205 > 0 || countCHK_7 > 0)
                {
                    MessageBox.Show("If bank is not Kouvas it's ok. Otherwise call Support Dept");
                }
            }
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            listView_MIDs.Clear();
            listView_MIDs.Columns.Add("MID/TID");
            listView_MIDs.Columns.Add("Route#1");
            listView_MIDs.Columns.Add("Route#2");
            listView_MIDs.Columns.Add("Route#3");
            listView_MIDs.Columns.Add("Route#4");
            listView_MIDs.Columns.Add("Route#5");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button4_Click(button4, EventArgs.Empty);
            //OdbcCommand comdBin_Chk = conn.CreateCommand();
            //listBoxOrigin.Items.Add(textBox_NEWBIN.Text);
            //MessageBox.Show("New bin added");

            ////Check for New BIN connection with Bank
            //comdBin_Chk.CommandText = "select DESTPORT, BINLOWER "+
            //                          "from MERCHBINS_test " +
            //                          "where TID = '1111    '  and binlower = ?";
            //comdBin_Chk.Parameters.Add("@binlower", OdbcType.VarChar, 22).Value = textBox_NEWBIN.Text;
            //OdbcDataReader readerBIN = comdBin_Chk.ExecuteReader();
            //while(readerBIN.Read())
            //{
            //     listView_BINChk.Items.Add(new ListViewItem(new string[] {readerBIN["DESTPORT"].ToString(), readerBIN["BINLOWER"].ToString()}));
            //}
            //readerBIN.Close();
            //textBox_NEWBIN.Clear();
            //listView_BINChk.Visible = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            flag_MID = 0;
            //string[] hostid_Descr = new string[5];
            int z = 0;
            int i = 0;
            OdbcCommand listView_MID = conn.CreateCommand();
            if (textBox_MultMid.Text.Length > 0 && !listView_MIDs.Items.Cast<ListViewItem>().Any(item => item.Text == textBox_MultMid.Text))
            {
                char[] separator = { '\n', '\r', ' ' };
                String[] word = textBox_MultMid.Text.Split(separator);
                for (i = 0; i < word.Length; i++)
                {
                    if (!String.IsNullOrEmpty(word[i]) && !listView_MIDs.Items.Cast<ListViewItem>().Any(item => item.Text == word[i]))
                    {
                        string[] hostid_Descr = new string[5];
                        z = 0;
                        listView_MID.CommandText = "select case uploadhostid " +
                                                    "when 1 then 'PIRAEUS' " +
                                                    "when 6 then 'ETHNIKI' " +
                                                    "when 201 then 'ALPMOR' " +
                                                    "when 302 then 'ALPHA' " +
                                                    "when 205 then 'EUROBANK' " +
                                                    "when 7 then 'ATTIKA' " +
                                                    "when 989 then 'EURMOR' " +
                                                    "else ' ' " +
                                                    "end as Host_descr " +
                                                    "from MERCHANTS_test where mid = ?  " +
                                                    "and  uploadhostid in ('1','6','302','205','201') " +
                                                    "group by uploadhostid ";
                        listView_MID.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = word[i];
                        listView_MID.Connection = conn;
                        OdbcDataReader reader11 = listView_MID.ExecuteReader();
                        while (reader11.Read() && z <= 5)
                        {
                            hostid_Descr[z] = (reader11.GetString(0)) ?? string.Empty;
                            z++;
                        }
                        listView_MIDs.Items.Add(new ListViewItem(new string[] { word[i], hostid_Descr[0], hostid_Descr[1], hostid_Descr[2], hostid_Descr[3], hostid_Descr[4] }));
                        listView_MIDs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        reader11.Close();
                        listView_MID.Dispose();
                        flag_MID = 1;
                    }
                }

                ////for (i = 0; i < word.Length; i++)
                ////    if (!String.IsNullOrEmpty(word[i]) && !listView_MIDs.Items.Cast<ListViewItem>().Any(item => item.Text == word[i]))
                ////        listView_MIDs.Items.Add(word[i]);
                ////listView_MIDs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                ////flag_MID = 1;
            }
            textBox_hiddenCpy.Text = textBox_MultMid.Text;
            textBox_MultMid.Text = "";
            z = 0;
        }

        private void progressBar1_Click(object sender, EventArgs e) { }

        private void button14_Click(object sender, EventArgs e)
        {
            int r = 0;
            int i = 0;
            flag_TID++;
            OdbcCommand listView_TID = conn.CreateCommand();
            if (textBox_MultTid.Text.Length > 0 && !listView_TIDs.Items.Cast<ListViewItem>().Any(item => item.Text == textBox_MultTid.Text))
            {
                char[] separator = { '\n', '\r', ' ' };
                String[] word = textBox_MultTid.Text.Split(separator);
                for (i = 0; i < word.Length; i++)
                {
                    if (!String.IsNullOrEmpty(word[i]) && !listView_TIDs.Items.Cast<ListViewItem>().Any(item => item.Text == word[i]))
                    {
                        string[] hostid_Descr = new string[5];
                        r = 0;
                        listView_TID.CommandText = "select case uploadhostid " +
                                                    "when 1 then 'PIRAEUS' " +
                                                    "when 6 then 'ETHNIKI' " +
                                                    "when 201 then 'ALPMOR' " +
                                                    "when 302 then 'ALPHA' " +
                                                    "when 205 then 'EUROBANK' " +
                                                    "when 7 then 'ATTIKA' " +
                                                    "when 989 then 'EURMOR' " +
                                                    "else ' ' " +
                                                    "end as Host_descr " +
                                                    "from dbo.MERCHANTS_test where tid = ?  " +
                                                    "and  uploadhostid in ('1','6','302','205','7','201') " +
                                                    "group by uploadhostid ";
                        listView_TID.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = word[i];
                        listView_TID.Connection = conn;
                        OdbcDataReader reader12 = listView_TID.ExecuteReader();
                        while (reader12.Read() && r <= 5)
                        {
                            hostid_Descr[r] = (reader12.GetString(0)) ?? string.Empty;
                            r++;
                        }
                        listView_TIDs.Items.Add(new ListViewItem(new string[] { word[i], hostid_Descr[0], hostid_Descr[1], hostid_Descr[2], hostid_Descr[3], hostid_Descr[4] }));
                        listView_TIDs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        reader12.Close();
                        listView_TID.Dispose();

                    }
                }

                ////for (i = 0; i < word.Length; i++)
                ////    if (!String.IsNullOrEmpty(word[i]) && !listView_MIDs.Items.Cast<ListViewItem>().Any(item => item.Text == word[i]))
                ////        listView_MIDs.Items.Add(word[i]);
                ////listView_MIDs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                ////flag_MID = 1;
            }
            textBox_hiddenCpy.Text = textBox_MultMid.Text;
            textBox_MultMid.Text = "";
            r = 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listView_TIDs.Clear();
            listView_TIDs.Columns.Add("TID");
            listView_TIDs.Columns.Add("Route#1");
            listView_TIDs.Columns.Add("Route#2");
            listView_TIDs.Columns.Add("Route#3");
            listView_TIDs.Columns.Add("Route#4");
            listView_TIDs.Columns.Add("Route#5");
            flag_TID = 0;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string lstVieItmInpu = "";
            string hiddString = "";
            foreach (ListViewItem itemTID in listView_TIDs.Items)
            {
                int[] cntBnkTid = new int[6];
                string destChk = " ";
                string binLoChk = " ";
                string destChkDescr = " ";
                string tidCheck = "";
                int inst_MAX = 0;
                int inst_MIN = 0;
                string flg_TID = "T";
                OdbcCommand comCursor_NewBIN_Chk = conn.CreateCommand();

                if (lstVieItmInpu == "" && hiddString == "" && textBoxFr.Text == " " && textBoxTo.Text == " " && textBoxAmntFr.Text == "" && textBoxAmntTo.Text == "")  // 090218
                {
                    MessageBox.Show("You haven't made any selection!");
                    button5.Enabled = false;
                    return;
                }

                //Check if installs are selected so to proceed
                if ((textBoxFr.Text.Equals(" ")) || (textBoxTo.Text.Equals(" ")))
                {
                    MessageBox.Show("Installments are not chosen!");
                    return;
                }

                //Check if amounts are filled so to proceed     090218
                if ((textBoxAmntFr.Text.Equals(" ")) || (textBoxAmntTo.Text.Equals(" ")))
                {
                    MessageBox.Show("Amounts fields are not filled!");
                    return;
                }

                if (listBoxOrigin.SelectedItem != null && textBox_NEWBIN.Text == "")
                {

                    if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" || listBoxOrigin.SelectedItem.ToString() == "CUP")
                    {
                        tidCheck = itemTID.SubItems[0].Text.ToString();
                        // Check if kouvas is not routed to a bank that TIDs are not connected to
                        TID_BankConn.TID_Check(flg_TID, tidCheck, cntBnkTid);
                        int countCHK_1 = cntBnkTid[0];
                        int countCHK_6 = cntBnkTid[1];
                        int countCHK_202 = cntBnkTid[2];
                        int countCHK_205 = cntBnkTid[3];
                        int countCHK_7 = cntBnkTid[4];
                        int countCHK_302 = cntBnkTid[5];


                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_1 > 0 && textBoxHidden_Route.Text.ToString() == "PIRAEUS")
                        {
                            string BankDescr = "PIRAEUS";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //button6_Click(button6, EventArgs.Empty);
                            return;
                        };

                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_6 > 0 && textBoxHidden_Route.Text.ToString() == "ETHNIKI")
                        {
                            string BankDescr = "ETHNIKI";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //button6_Click(button6, EventArgs.Empty);
                            return;
                        };

                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_202 > 0 && textBoxHidden_Route.Text.ToString() == "ALPMOR") 
                        {
                            string BankDescr = "ALPMOR";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //button6_Click(button6, EventArgs.Empty);
                            return;
                        };

                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_202 > 0 && textBoxHidden_Route.Text.ToString() == "EURMOR")
                        {
                            string BankDescr = "ALPMOR";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //button6_Click(button6, EventArgs.Empty);
                            return;
                        };


                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_302 > 0 &&   textBoxHidden_Route.Text.ToString() == "ALPHABANK")
                        {
                            string BankDescr = "ALPHA";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //button6_Click(button6, EventArgs.Empty);
                            return;
                        };


                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_205 > 0 && textBoxHidden_Route.Text.ToString() == "EUROBANK")
                        {
                            string BankDescr = "EUROBANK";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            // button6_Click(button6, EventArgs.Empty);
                            return;
                        };

                        if (listBoxOrigin.SelectedItem.ToString() == "KOYBAS" && countCHK_7 > 0 && textBoxHidden_Route.Text.ToString() == "ATTIKA")
                        {
                            string BankDescr = "ATTIKA";
                            MessageBox.Show("You cannot choose " + BankDescr + " as bucket as it's not connected to TID " + tidCheck + " . " + "\n" + "Please choose another!");
                            listView1.Clear();
                            //listView1.Clear();
                            //listView1.Columns.Add("Originator");
                            //listView1.Columns.Add("Route");
                            //listView1.Columns.Add("Instal-From");
                            //listView1.Columns.Add("Instal-To");
                            //  button6_Click(button6, EventArgs.Empty);
                            return;
                        };
                    }

                    int countCHK_CUP = cntBnkTid[0];

                    //Check if a bank is already chosen for ruling so to warn user
                    if (!!listView1.Items.Cast<ListViewItem>().Any(item => item.Text == listBoxOrigin.SelectedItem.ToString()) && listBoxOrigin.SelectedItem.ToString() != "KOYBAS")
                    {
                        MessageBox.Show("Bank " + listBoxOrigin.SelectedItem.ToString() + " already chosen for ruling!!");
                        return;
                    }

                    // Check if AMXEDINERS is routed to ALPHA
                    if (listBoxOrigin.SelectedItem.ToString() == "AMEXDINERS" && (textBoxHidden_Route.Text.ToString() != "ALPHABANK" && textBoxHidden_Route.Text.ToString() != "ALPMOR"))
                    {
                        MessageBox.Show("AMEX/DINERS is routed to ALPHA Bank ONLY!! " + "\n" + "You can't choose any other Bank");
                        return;
                    }

                    // Check if PIRAEUS is connected to TID so to route CUP trxs
                    if (listBoxOrigin.SelectedItem.ToString() == "CUP" && countCHK_CUP > 0)
                    {
                        MessageBox.Show("PIRAEUS Bank is not connected to TID! " + "\n" + "You can't route CUP!");
                        return;
                    }


                    // Check if CUP is routed to PIRAEUS
                    if (listBoxOrigin.SelectedItem.ToString() == "CUP" && textBoxHidden_Route.Text.ToString() != "PIRAEUS")
                    {
                        MessageBox.Show("CUP is routed to PIRAEUS Bank ONLY!! " + "\n" + "You can't choose any other Bank");
                        return;
                    }

                    // Add to listView so to keep track of what it is done
                    lstVieItmInpu = listBoxOrigin.SelectedItem.ToString();
                    //listView1.Items.Add(new ListViewItem(new string[] { listBoxOrigin.SelectedItem.ToString(), textBoxHidden_Route.Text, textBoxFr.Text, textBoxTo.Text }));
                }
                // Multiple checks for xrta BIN addition / Update
                else
                {
                    if (radioButton_AddBin.Checked == false && radioButton_UpdBIN.Checked == false)
                    {
                        MessageBox.Show("Please choose action for BIN, Update ruling or Add extra ruling! ");
                        return;
                    }

                    if (textBoxHidden_Route.Text == "")
                    {
                        MessageBox.Show("Please choose route bank! ");
                        return;
                    }

                    comCursor_NewBIN_Chk.CommandText = "select top(1) TID,DESTPORT,BINLOWER,INSTMIN,INSTMAX from dbo.MERCHBINS_TEST where tid = ? and binlower = '" + textBox_NEWBIN.Text + "'";
                    //Check if MID was chosen from ComboBox or it was added from TextBox
                    comCursor_NewBIN_Chk.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                    OdbcDataReader readerCursor6 = comCursor_NewBIN_Chk.ExecuteReader();
                    if(readerCursor6.HasRows)
                    {
                        while (readerCursor6.Read())
                        {
                            destChk = readerCursor6["DESTPORT"].ToString();
                            destChkDescr = "";
                            switch (destChk)
                            {
                                case "NET_ABC":
                                    destChkDescr = "PIRAEUS";
                                    break;
                                case "NET_NTBN":
                                    destChkDescr = "ETHNIKI";
                                    break;
                                case "NET_ALPMOR":
                                    destChkDescr = "ALPMOR";
                                    break;
                                case "NET_BICALPHA":
                                    destChkDescr = "ALPHABANK";
                                    break;
                                case "NET_CLBICEBNK":
                                    destChkDescr = "EUROBANK";
                                    break;
                                case "NET_ATTIKA":
                                    destChkDescr = "ATTIKA";
                                    break;
                                case "NET_EURMOR":
                                    destChkDescr = "EURMOR";
                                    break;
                            }

                            binLoChk = readerCursor6["BINLOWER"].ToString();
                            inst_MIN = Int32.Parse(readerCursor6["INSTMIN"].ToString());
                            inst_MAX = Int32.Parse(readerCursor6["INSTMAX"].ToString());
                            textBox__CurrMinInst.Text = readerCursor6["INSTMIN"].ToString();
                            textBox_CurrMaxInst.Text = readerCursor6["INSTMAX"].ToString();
                            textBox_CurrRoute.Text = destChkDescr;
                        }
                    }
                        else
                        {
                            binLoChk = textBox_NEWBIN.Text.ToString();
                        }
                    

                    if (radioButton_AddBin.Checked == true)
                    {
                        // Check for BIN ruling wih additional installments range in different Bank
                        if (inst_MAX == Int32.Parse(textBoxFr.Text.ToString()))
                        {
                            MessageBox.Show("A record for BIN " + textBox_NEWBIN.Text.ToString() + " already exists. " + "\n" + " Please check Current Min/Max so to change accordingly. ");
                            //textBox__CurrMinInst.Text = "";
                            //textBox_CurrMaxInst.Text = "";
                            //textBox_CurrRoute.Text = "";
                            return;
                        }
                        else if (inst_MAX + 1 != Int32.Parse(textBoxFr.Text.ToString()))
                        {
                            MessageBox.Show("Range for installments is wrong. 'From' should be +1 from Current Max. ");
                            //textBox__CurrMinInst.Text = "";
                            //textBox_CurrMaxInst.Text = "";
                            //textBox_CurrRoute.Text = "";
                            return;
                        }
                        else if (inst_MAX + 1 == Int32.Parse(textBoxFr.Text.ToString()))
                        {
                            dialogResNew_BIN = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                                "Should we proceed to addition of ruling ?", "BIN Check for additional ruling", MessageBoxButtons.YesNo);
                        };
                    }

                    if (radioButton_UpdBIN.Checked == true)
                    {
                        // Check for BIN Update in Installments range
                        if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && destChkDescr != textBoxHidden_Route.Text)
                        {
                            dialogResNew_Inst = MessageBox.Show("Ruling for BIN : " + binLoChk + " is applied to Destination Port : " + destChk + " ." + "\n" +
                               "Should we proceed to new ruling apply ?", "BIN Check for new ruling", MessageBoxButtons.YesNo);
                            //MessageBox.Show("Ruling for BIN " + readerCursor6["BINLOWER"].ToString() + " is applied to Destination Port " + readerCursor6["DESPORT"].ToString() +" .","",MessageBoxButtons.YesNo);
                        }
                        else if (inst_MIN == Int32.Parse(textBoxFr.Text.ToString()) && inst_MAX == Int32.Parse(textBoxTo.Text.ToString()) && destChkDescr == textBoxHidden_Route.Text)
                        {
                            MessageBox.Show("You cannot update BIN ruling with same installments range under the same route bank! ");
                            //textBox__CurrMinInst.Text = "";
                            //textBox_CurrMaxInst.Text = "";
                            //textBox_CurrRoute.Text = "";
                            return;
                        };

                    }
                    readerCursor6.Close();
                    lstVieItmInpu = textBox_NEWBIN.Text.ToString();
                    //listView1.Items.Add(new ListViewItem(new string[] { textBox_NEWBIN.Text.ToString(), textBoxHidden_Route.Text, textBoxFr.Text, textBoxTo.Text }));
                };
            }
            hiddString = textBoxHidden_Route.Text;
            //if (listBoxOrigin.SelectedItem.ToString() == "CUP")
            //    hiddString = " ";
            if ((lstVieItmInpu != "" || hiddString != "" || textBoxFr.Text != " " || textBoxTo.Text != " " || textBoxAmntFr.Text != "" || textBoxAmntTo.Text != "") && listView_TIDs.Items.Count > 0) // 090218
            {
                listView1.Items.Add(new ListViewItem(new string[] { lstVieItmInpu, hiddString, textBoxFr.Text, textBoxTo.Text, textBoxAmntFr.Text, textBoxAmntTo.Text }));   // Added 09/02/2018
                button5.Enabled = true;             // 090218
            }
             
        }

        private void button17_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = 5;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            pbar1.Minimum = 0;
            pbar1.Maximum = 2000;

            if (pbar1.Value < pbar1.Maximum)
            {
                pbar1.Value = pbar1.Value + 5;
            }
            if (pbar1.Value == 2000)
            {
                //MessageBox.Show("DONE");
                pbar1.Value = 0;
                timer1.Stop();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (checkBox1.Checked == false)     // 090218
            {
                button2_Click(button2, EventArgs.Empty);
            }

            string sublstItm1;
            string sublstItm2;
            string alpha_NotosChk = " ";
            string chkOriginID = " ";
            string route_hostID = " ";
            string chkOriginID2 = " ";
            string route_hostID2 = " ";
            //string templKarf = "1111    ";
            int delFlag = 0;

            //Array for CUP insert
            string[,] arrCup = new string[7, 3];
            arrCup[0, 0] = "1";
            arrCup[1, 0] = "6";
            arrCup[2, 0] = "7";
            arrCup[3, 0] = "201";
            arrCup[4, 0] = "205";
            arrCup[5, 0] = "302";
            arrCup[6, 0] = "989";
            arrCup[0, 1] = "NET_ABC";
            arrCup[1, 1] = "NET_NTBN";
            arrCup[2, 1] = "NET_ATTIKA";
            arrCup[3, 1] = "NET_ALPMOR";
            arrCup[4, 1] = "NET_CLBICEBNK";
            arrCup[5, 1] = "NET_BICALPHA";
            arrCup[6, 1] = "NET_EURMOR";
            arrCup[0, 2] = "Y";
            arrCup[1, 2] = "N";
            arrCup[2, 2] = "N";
            arrCup[3, 2] = "N";
            arrCup[4, 2] = "N";
            arrCup[5, 2] = "N";
            arrCup[6, 2] = "N";



            OdbcCommand comCursor = conn.CreateCommand();
            OdbcCommand comCursor2 = conn.CreateCommand();
            OdbcCommand comCursorKoyvas = conn.CreateCommand();
            OdbcCommand comCursorTic_MMore = conn.CreateCommand();
            OdbcCommand comCursorAMEX_DINERS = conn.CreateCommand();
            OdbcCommand comCursor_NewBIN = conn.CreateCommand();
            OdbcCommand comCursor_NewInst = conn.CreateCommand();
            OdbcCommand comCursor_NewBIN_Chk = conn.CreateCommand();
            OdbcCommand comCursor_UpdGmax = conn.CreateCommand();
            OdbcCommand comCursorCUP = conn.CreateCommand();


            foreach (ListViewItem itemTID in listView_TIDs.Items)
            {
                tidDescr.Text = itemTID.SubItems[0].Text.ToString();
                // Timer for progress bar      
                timer1.Start();
                timer1.Interval = 5;

                foreach (ListViewItem item in listView1.Items)
                {
                    //for (int i = 0; i < item.SubItems.Count; i++)
                    // {
                    chkOriginID = "0";
                    chkOriginID2 = "0";
                    sublstItm1 = item.SubItems[0].Text.ToString();
                    sublstItm2 = item.SubItems[1].Text.ToString();
                    textBox1.Text = item.SubItems[0].Text.ToString();
                    textBox2.Text = item.SubItems[1].Text.ToString();
                    string textBox_Fr = item.SubItems[2].Text.ToString();
                    string textBox_To = item.SubItems[3].Text.ToString();
                    string textBox_AmntFr = item.SubItems[4].Text.ToString();                   // 090218
                    string textBox_AmntTo = item.SubItems[5].Text.ToString();                   // 090218
                    //DialogResult dialogResNew_Inst = new DialogResult();
                    //DialogResult dialogResNew_BIN = new DialogResult();

                    //Transform Bank into HostID
                    string chkOrigin = textBox1.Text;
                    switch (chkOrigin)
                    {
                        case "PIRAEUS":
                            chkOriginID = "1";
                            route_hostID = "NET_ABC";
                            break;
                        case "ETHNIKI":
                            chkOriginID = "6";
                            route_hostID = "NET_NTBN";
                            break;
                        case "ALPMOR":
                            chkOriginID = "201";
                            route_hostID = "NET_ALPMOR";
                            break;
                        case "ALPHABANK":
                            chkOriginID = "302";
                            route_hostID = "NET_BICALPHA";
                            break;
                        case "EUROBANK":
                            chkOriginID = "205";
                            route_hostID = "NET_CLBICEBNK";
                            break;
                        case "ATTIKA":
                            chkOriginID = "7";
                            route_hostID = "NET_ATTIKA";
                            break;
                        case "EURMOR":
                            chkOriginID = "989";
                            route_hostID = "NET_EURMOR";
                            break;
                    }

                    //Transform Bank into DEST
                    string chkOrigin2 = textBox2.Text;
                    switch (chkOrigin2)
                    {
                        case "PIRAEUS":
                            chkOriginID2 = "1";
                            route_hostID2 = "NET_ABC";
                            break;
                        case "ETHNIKI":
                            chkOriginID2 = "6";
                            route_hostID2 = "NET_NTBN";
                            break;
                        case "ALPMOR":
                            chkOriginID2 = "201";
                            route_hostID2 = "NET_ALPMOR";
                            alpha_NotosChk = "201";
                            break;
                        case "ALPHABANK":
                            chkOriginID2 = "302";
                            route_hostID2 = "NET_BICALPHA";
                            alpha_NotosChk = "302";
                            break;
                        case "EUROBANK":
                            chkOriginID2 = "205";
                            route_hostID2 = "NET_CLBICEBNK";
                            break;
                        case "ATTIKA":
                            chkOriginID2 = "7";
                            route_hostID2 = "NET_ATTIKA";
                            break;
                        case "EURMOR":
                            chkOriginID2 = "989";
                            route_hostID2 = "NET_EURMOR";
                            alpha_NotosChk = "989";
                            break;
                    }

                    //textBoxFr.Text.ToString();
                    //textBoxTo.Text.ToString();

                    //Check if first time in loop so to delete all records for specific MID
                    if (delFlag == 0 && (chkOriginID == "1" || chkOriginID == "6" || chkOriginID == "201" || chkOriginID == "302" || chkOriginID == "205" || chkOriginID == "7" ||
                        chkOrigin == "KOYBAS" || chkOrigin == "MEALSANDMORE" || chkOrigin == "TICKETRESTAURANT" || chkOrigin == "AMEXDINERS"))
                        //Delete all records for specific TID
                        if (checkBox1.Checked == false)  // 090218
                        {
                            button3_Click(button3, EventArgs.Empty);
                        }

                    if (chkOriginID == "1" || chkOriginID == "6" || chkOriginID == "201" || chkOriginID == "302" || chkOriginID == "205" || chkOriginID == "7")
                    {
                        // Check if trxs will routed to same bank id so to change DESTPORT
                        if (chkOriginID == chkOriginID2)
                        {
                            if (chkOriginID == "201" || chkOriginID == "302")  // Only for Alpha
                            {
                                comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                " (select top 1 mid,TID, uploadhostname " +
                                                "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                "  where tid = ? and " +
                                                "  a.uploadhostid = ? " +
                                                "  and substring(desttid,1,1) <> 'P' " +
                                                "  and a.uploadhostid = b.uploadhostid), " +
                                                "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                " from dbo.MERCHBINS_TEST  " +
                                                " where TID = '1111    ' and DESTPORT = ? and " +
                                                " binlower not in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                    textBox_Fr + "," + textBox_To + "," +
                                                "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                    textBox_AmntFr + "," + textBox_AmntTo +
                                                "   from  merchtid,merchInfo ) ";
                            }
                            else if (chkOriginID == "6")  // Only for Ethniki
                            {
                                comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                " (select top 1 mid,TID, uploadhostname " +
                                                "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                "  where tid = ? and " +
                                                "  a.uploadhostid = ? " +
                                                "  and substring(desttid,1,1) <> 'P' " +
                                                "  and a.uploadhostid = b.uploadhostid), " +
                                                "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                " from dbo.MERCHBINS_TEST  " +
                                                " where TID = '1111    ' and DESTPORT = ? and " +
                                                " binlower not in ('549804')) " +
                                                "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                    textBox_Fr + "," + textBox_To + "," +
                                                "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                    textBox_AmntFr + "," + textBox_AmntTo +
                                                "   from  merchtid,merchInfo ) ";
                            }
                            else if (chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302")
                            {
                                comCursor.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select top 1 mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where tid = ? and " +
                                                    "  a.uploadhostid = ? " +
                                                    "  and substring(desttid,1,1) <> 'P' " +
                                                    "  and a.uploadhostid = b.uploadhostid), " +
                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                    " from dbo.MERCHBINS_TEST  " +
                                                    " where TID = '1111    ' and DESTPORT = ?)  " +
                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                    "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                    "   from  merchtid,merchInfo ) ";
                            }

                        }
                        else
                        {
                            if (chkOriginID == "201" || chkOriginID == "302")  // Only for Alpha
                            {
                                comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                      " (select top 1  mid,TID, uploadhostname " +
                                                      "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                      "  where tid =  ? "  +
                                                      "   and a.uploadhostid = ?  " +
                                                      "  and substring(desttid,1,1) <> 'P' " +
                                                      "  and a.uploadhostid = b.uploadhostid), " +
                                                      "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                      " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                      "  from dbo.MERCHBINS_TEST  " +
                                                      "  where TID = '1111    ' and DESTPORT = ? " +
                                                      " and binlower not in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                      "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                      "   select #TID, '" +
                                                          route_hostID2 +
                                                      "', #BINLOWER, #BINUPPER, " +
                                                      textBox_Fr + "," + textBox_To + "," +
                                                      "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                          textBox_AmntFr + "," + textBox_AmntTo +
                                                      "   from  merchtid,merchInfo " +
                                                      "   order by #TID ";
                            }
                            else if (chkOriginID == "6")  // Only for Ethniki
                            {
                                comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                      " (select top 1  mid,TID, uploadhostname " +
                                                      "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                      "  where tid = ? " +
                                                      "   and a.uploadhostid = ? " +
                                                      "  and substring(desttid,1,1) <> 'P' " +
                                                      "  and a.uploadhostid = b.uploadhostid), " +
                                                      "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                      " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                      "  from dbo.MERCHBINS_TEST  " +
                                                      "  where TID = '1111    ' and DESTPORT = ? " +
                                                      " and binlower not in ('549804')) " +
                                                      "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                      "   select #TID, '" +
                                                          route_hostID2 +
                                                      "', #BINLOWER, #BINUPPER, " +
                                                      textBox_Fr + "," + textBox_To + "," +
                                                      "   #GRACEMIN, #GRACEMAX, #ALLOWED, "+
                                                          textBox_AmntFr + "," + textBox_AmntTo +
                                                      "   from  merchtid,merchInfo " +
                                                      "   order by #TID ";
                            }
                            else if (chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302")
                            {
                                comCursor2.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                        " (select top 1  mid,TID, uploadhostname " +
                                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                        "  where tid = ? " + 
                                                        "   and a.uploadhostid = ? " +
                                                        "  and substring(desttid,1,1) <> 'P' " +
                                                        "  and a.uploadhostid = b.uploadhostid), " +
                                                        "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                        " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                        "  from dbo.MERCHBINS_TEST  " +
                                                        "  where TID = '1111    ' and DESTPORT = ? ) " +
                                                        "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                        "   select #TID, '" +
                                                            route_hostID2 +
                                                        "', #BINLOWER, #BINUPPER, " +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                        "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                            textBox_AmntFr + "," + textBox_AmntTo +
                                                        "   from  merchtid,merchInfo " +
                                                        "   order by #TID ";
                            }
                        }
                        if (chkOriginID == chkOriginID2)
                        {
                            try
                            {
                                comCursor.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursor.Parameters.Add("@DESTPORT", OdbcType.VarChar, 20).Value = route_hostID;
                                comCursor.Connection = conn;
                                comCursor.CommandTimeout = 300;
                                OdbcDataReader readerCursor1 = comCursor.ExecuteReader();
                                //readerCursor1.Dispose();
                                comCursor.Dispose();
                                readerCursor1.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 020. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }
                        }
                        else
                        {
                            try
                            {
                                comCursor2.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursor2.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursor2.Parameters.Add("@DESTPORT", OdbcType.VarChar, 20).Value = route_hostID;
                                comCursor2.Connection = conn;
                                //comCursor2.CommandTimeout = 300;
                                OdbcDataReader readerCursor2 = comCursor2.ExecuteReader();
                                comCursor2.Dispose();
                                readerCursor2.Close();
                                delFlag++;
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 020B. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }
                        }

                    }
                    else if (chkOrigin == "KOYBAS")
                    {
                        comCursorKoyvas.CommandText =
                                        "with merchtid(#mid,#tid,#name) as  " +
                                        " (select top 1  mid,TID, uploadhostname " +
                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                        "  where tid = ? and a.uploadhostid = ? " +
                                        "  and substring(desttid,1,1) <> 'P' " +
                                        "  and a.uploadhostid = b.uploadhostid) " +
                                        "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                        " select #TID, #name, '4', '6', " +
                                        textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                        textBox_AmntFr + "," + textBox_AmntTo +
                                        " from  merchtid; " +
                                        "with merchtid(#mid,#tid,#name) as " +
                                        " (select top 1  mid,TID, uploadhostname " +
                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                        "  where tid = ? and a.uploadhostid = ? " +
                                        "  and substring(desttid,1,1) <> 'P' " +
                                        "  and a.uploadhostid = b.uploadhostid) " +
                                        "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                        " select #TID, #name, '9', '9', " +
                                        textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', "  +
                                        textBox_AmntFr + "," + textBox_AmntTo +
                                        " from  merchtid; " +
                                        "with merchtid(#mid,#tid,#name) as " +
                                        " (select top 1  mid,TID, uploadhostname " +
                                        "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                        "  where tid = ? and a.uploadhostid = ? " +
                                        "  and substring(desttid,1,1) <> 'P' " +
                                        "  and a.uploadhostid = b.uploadhostid) " +
                                        "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                        " select #TID, #name, '222100', '272099', " +
                                        textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                        textBox_AmntFr + "," + textBox_AmntTo +
                                        " from  merchtid; ";
                        try
                        {
                            comCursorKoyvas.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                            comCursorKoyvas.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                            comCursorKoyvas.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorKoyvas.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                            comCursorKoyvas.Connection = conn;
                            OdbcDataReader readerCursor2 = comCursorKoyvas.ExecuteReader();
                            comCursorKoyvas.Dispose();
                            readerCursor2.Close();
                            delFlag++;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 021. ");
                            delFlag = 0;
                            button3_Click(button3, EventArgs.Empty);
                            return;
                        }

                    }
                    else if (chkOrigin == "MEALSANDMORE")
                    {
                        comCursorTic_MMore.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                " (select top 1  mid,TID, uploadhostname " +
                                                "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                "  where tid = ? and a.uploadhostid = ? " +
                                                "  and substring(desttid,1,1) <> 'P' " +
                                                "  and a.uploadhostid = b.uploadhostid) " +
                                                "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                " select #TID, #name, '502259', '502259', 0, 1, 0, 99, 'Y', " +
                                                  textBox_AmntFr + "," + textBox_AmntTo +
                                                " from  merchtid ; ";
                        try
                        {
                            comCursorTic_MMore.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorTic_MMore.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                            comCursorTic_MMore.Connection = conn;
                            OdbcDataReader readerCursor3 = comCursorTic_MMore.ExecuteReader();
                            comCursorTic_MMore.Dispose();
                            readerCursor3.Close();
                            delFlag++;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 022. ");
                            delFlag = 0;
                            button3_Click(button3, EventArgs.Empty);
                            return;
                        }

                    }
                    else if (chkOrigin == "TICKETRESTAURANT")
                    {
                        comCursorTic_MMore.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                " (select top 1  mid,TID, uploadhostname " +
                                                "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                "  where tid = ? and a.uploadhostid = ? " +
                                                "  and substring(desttid,1,1) <> 'P' " +
                                                "  and a.uploadhostid = b.uploadhostid) " +
                                                "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                " select #TID, #name, '534228', '534228', 0, 1, 0, 99, 'Y', " +
                                                  textBox_AmntFr + "," + textBox_AmntTo +
                                                " from  merchtid ; ";
                        try
                        {
                            comCursorTic_MMore.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorTic_MMore.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                            comCursorTic_MMore.Connection = conn;
                            OdbcDataReader readerCursor4 = comCursorTic_MMore.ExecuteReader();
                            comCursorTic_MMore.Dispose();
                            readerCursor4.Close();
                            delFlag++;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 023. ");
                            delFlag = 0;
                            button3_Click(button3, EventArgs.Empty);
                            return;
                        }

                    }
                    else if (chkOrigin == "AMEXDINERS" && chkOriginID2 == "201")
                    {
                        comCursorAMEX_DINERS.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select top 1  mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where tid = ? and " +
                                                    "  a.uploadhostid = '201'  " +
                                                    "  and substring(desttid,1,1) <> 'P' " +
                                                    "  and a.uploadhostid = b.uploadhostid), " +
                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                    " from dbo.MERCHBINS_TEST  " +
                                                    " where TID = '1111    ' and DESTPORT = 'NET_ALPMOR'  and " +
                                                    " binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                    "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                    "   from  merchtid,merchInfo ) ";

                        try
                        {
                            comCursorAMEX_DINERS.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorAMEX_DINERS.Connection = conn;
                            OdbcDataReader readerCursor5 = comCursorAMEX_DINERS.ExecuteReader();
                            comCursorAMEX_DINERS.Dispose();
                            readerCursor5.Close();
                            delFlag++;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 024. ");
                            delFlag = 0;
                            button3_Click(button3, EventArgs.Empty);
                            return;
                        }
                       }
                    else if (chkOrigin == "AMEXDINERS" && chkOriginID2 == "302")
                    {
                        comCursorAMEX_DINERS.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select top 1  mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                    "  where tid = ? and " +
                                                    "   a.uploadhostid = '302' " +
                                                    "  and substring(desttid,1,1) <> 'P' " +
                                                    "  and a.uploadhostid = b.uploadhostid), " +
                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX  " +
                                                    " from dbo.MERCHBINS_TEST  " +
                                                    " where TID = '1111    ' and  DESTPORT = 'NET_BICALPHA' and " +
                                                    " binlower in ('30','36','38','34','37','6011','644','645','646','647','648','649','650','651','652','653','654','655','656','657','658','659')) " +
                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER,INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    " (select #tid, #name,  #BINLOWER, #BINUPPER," +
                                                        textBox_Fr + "," + textBox_To + "," +
                                                    "   #GRACEMIN, #GRACEMAX, #ALLOWED, " +
                                                        textBox_AmntFr + "," + textBox_AmntTo +
                                                    "   from  merchtid,merchInfo ) ";

                        try
                        {
                            comCursorAMEX_DINERS.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                            comCursorAMEX_DINERS.Connection = conn;
                            OdbcDataReader readerCursor5 = comCursorAMEX_DINERS.ExecuteReader();
                            comCursorAMEX_DINERS.Dispose();
                            readerCursor5.Close();
                            delFlag++;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 024. ");
                            delFlag = 0;
                            button3_Click(button3, EventArgs.Empty);
                            return;
                        }
                    }
                    else if (chkOrigin == "CUP")
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            comCursorCUP.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                                                    " (select top 1  mid,TID, uploadhostname " +
                                                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                                                    "  where tid = ? " +
                                                                                    "   and a.uploadhostid = ? " +
                                                                                    "  and substring(desttid,1,1) <> 'P' " +
                                                                                    "  and a.uploadhostid = b.uploadhostid), " +
                                                                                    "merchInfo(#DESTPORT, #BINLOWER, #BINUPPER, #GRACEMIN, #GRACEMAX, #ALLOWED, #AMOUNTMIN, #AMOUNTMAX ) as  " +
                                                                                    " (select DESTPORT, BINLOWER, BINUPPER, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX " +
                                                                                    "  from dbo.MERCHBINS_TEST  " +
                                                                                    "  where TID = '2222    ' and DESTPORT = 'NET_CUP') " +
                                                                                    "insert into  dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                                                    "   select #TID, '" +
                                                                                        arrCup[i, 1] +
                                                                                    "', #BINLOWER, #BINUPPER, 0, 1, " +
                                                                                    "   #GRACEMIN, #GRACEMAX,'" + arrCup[i, 2] +
                                                                                    "'," + textBox_AmntFr + "," + textBox_AmntTo +
                                                                                    "   from  merchtid,merchInfo " +
                                                                                    "   order by #TID ";
                            try
                            {
                                comCursorCUP.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursorCUP.Parameters.Add("@uploadhost", OdbcType.Int).Value = arrCup[i, 0];
                                comCursorCUP.Connection = conn;
                                OdbcDataReader readerCursor10 = comCursorCUP.ExecuteReader();
                                comCursorCUP.Dispose();
                                readerCursor10.Close();

                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 026. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }

                        }
                    }
                    else if (chkOriginID != "1" && chkOriginID != "6" && chkOriginID != "201" && chkOriginID != "302" && chkOriginID != "205" && chkOriginID != "7" &&
                        chkOrigin != "KOYBAS" && chkOrigin != "MEALSANDMORE" && chkOrigin != "TICKETRESTAURANT" && chkOrigin != "AMEXDINERS")
                    {
                        if (dialogResNew_Inst == DialogResult.Yes)
                        {
                            comCursor_NewInst.CommandText = "delete from dbo.MERCHBINS_TEST where tid = ? and binlower = '" + chkOrigin + "';" +
                                                           "with merchtid(#mid,#tid,#name) as " +
                                                " (select top 1 mid,TID, uploadhostname " +
                                                "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                                "  where tid = ? and a.uploadhostid = ? " +
                                                "  and substring(desttid,1,1) <> 'P' " +
                                                "  and a.uploadhostid = b.uploadhostid) " +
                                                "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                " select #TID, #name, '" + chkOrigin + "', '" + chkOrigin + "'," +
                                                textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                                textBox_AmntFr + "," + textBox_AmntTo +
                                                " from  merchtid ; ";
                            try
                            {
                                comCursor_NewInst.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursor_NewInst.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursor_NewInst.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursor_NewInst.Connection = conn;
                                OdbcDataReader readerCursor7 = comCursor_NewInst.ExecuteReader();
                                comCursor_NewInst.Dispose();
                                readerCursor7.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 025. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }
                        }

                        if (dialogResNew_BIN == DialogResult.Yes)
                        {
                            comCursor_NewBIN.CommandText = "with merchtid(#mid,#tid,#name) as " +
                                                    " (select top 1  mid,TID, uploadhostname " +
                                                    "  from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                    "  where tid = ? and a.uploadhostid = ? " +
                                                    "  and substring(desttid,1,1) <> 'P' " +
                                                    "  and a.uploadhostid = b.uploadhostid) " +
                                                    "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                    " select #TID, #name, '" + chkOrigin + "', '" + chkOrigin + "'," +
                                                    textBox_Fr + "," + textBox_To + ", 0, 99, 'Y', " +
                                                    textBox_AmntFr + "," + textBox_AmntTo +
                                                    " from  merchtid ; ";
                            try
                            {
                                comCursor_NewBIN.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                                comCursor_NewBIN.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                                comCursor_NewBIN.Connection = conn;
                                OdbcDataReader readerCursor9 = comCursor_NewBIN.ExecuteReader();
                                comCursor_NewBIN.Dispose();
                                readerCursor9.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 026. ");
                                delFlag = 0;
                                button3_Click(button3, EventArgs.Empty);
                                return;
                            }


                        }
                    }

                }

                // Check if TID belomgs to one of NOTOS mids
                OdbcCommand comCount_NotosChkTid = new OdbcCommand("select count(*) from MERCHANTS_test where mid in ('000000110000237','000000110000219','000000110000286','000000110000205','000000110000202','000000110000236','000000110000292','000000110000296','000000110000299','000000110000300','000000110000285','000000110000291','000000110000221','000000110000207','000000110000212','000000110000220','000000110000278','000000110000234','000000110000210','000000110000230','000000110000274','000000110000213','000000110000216','000000110000217','000000110000279','000000110000275','000000110000288','000000110000229','000000110000295','000000110000235','000000110000270','000000110000242','000000110000297','000000110000239','000000110000238','000000110000243','000000110000240','000000110000271','000000110000241','000000110000208','000000110000233','000000110000298','000000110000206','000000110000226','000000110000201','000000110000224','000000110000289','000000110000203') " +
                                                                    "and tid = ? ", conn);
                comCount_NotosChkTid.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                int countCHK_Notos = Convert.ToInt32(comCount_NotosChkTid.ExecuteScalar());

                // If tid belogs to one of NOTOS MIDs, insert xtra BIN 999914
                if (countCHK_Notos > 0)
                {
                    OdbcCommand comCursorNotos = new OdbcCommand("with merchtid(#mid,#tid,#name) as " +
                                               " (select top 1 mid,TID, uploadhostname " +
                                               "  from dbo.MERCHANTS_test a, dbo.uploadhosts b  " +
                                               "  where tid = ? and a.uploadhostid = '" + alpha_NotosChk + "'" +
                                               "  and substring(desttid,1,1) <> 'P' " +
                                               "  and a.uploadhostid = b.uploadhostid) " +
                                               "insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                               " select #TID, #name, '999914', '999914', 0, 1, 0, 99, 'Y', 0 , 999999999 " +
                                               " from  merchtid ; ", conn);
                    try
                    {
                        comCursorNotos.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                        comCursorNotos.Connection = conn;
                        OdbcDataReader readerCursor10 = comCursorNotos.ExecuteReader();
                        comCursorNotos.Dispose();
                        readerCursor10.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 027. ");
                        delFlag = 0;
                        button3_Click(button3, EventArgs.Empty);
                        return;
                    }

                };

                // Update GMAXINST from Merchants file
                comCursor_UpdGmax.CommandText = "with  fndMaxPBnk (tid_, maxinst,desport) as " +
                                                "(select tid,max(instmax) max_inst, " +
                                                 "case destport " +
                                                 "when 'NET_ABC'  THEN '1' " +
                                                 "when 'NET_NTBN' THEN '6' " +
                                                 "when 'NET_ALPMOR' THEN '201' " +
                                                 "when 'NET_BICALPHA' THEN '302' " +
                                                 "when 'NET_CLBICEBNK'  THEN '205' " +
                                                 "when 'NET_ATTICA' THEN '7' " +
                                                 "when 'NET_EURMOR' THEN '989' " +
                                                 "end des_port " +
                                                 "from dbo.MERCHBINS_TEST where tid = ? " +
                                                 "group by tid,destport) " +
                                                "update MERCHANTS_test " +
                                                "set gmaxinst = maxinst " +
                                                "from fndMaxPBnk " +
                                                "where tid_ = tid and uploadhostid = desport and substring(desttid,1,1) <> 'P' ";
                try
                {
                    comCursor_UpdGmax.Parameters.Add("@tid", OdbcType.VarChar, 16).Value = tidDescr.Text;
                    comCursor_UpdGmax.Connection = conn;
                    OdbcDataReader readerCursor8 = comCursor_UpdGmax.ExecuteReader();
                    comCursor_UpdGmax.Dispose();
                    readerCursor8.Close();
                }
                catch
                {
                    MessageBox.Show("Unexpected error has occured. Please start process from begin. " + "\n" + "Error code 027. ");
                    delFlag = 0;
                    button3_Click(button3, EventArgs.Empty);
                    return;
                }
               // label8.Text ="Insert completed for TID: " + tidDescr.Text;
                //MessageBox.Show("Insert completed for TID: " + tidDescr.Text);
                //timer1.Stop();
                delFlag = 0;
                textBox__CurrMinInst.Text = "";
                textBox_CurrMaxInst.Text = "";
                textBox_CurrRoute.Text = "";
                textBox_NEWBIN.Text = "";
                comboBox_InstalFr.SelectedText = "0";
                comboBox_InstalTo.SelectedText = "0";
            }

            Cursor.Current = Cursors.Default;
            textBox_NEWBIN.Enabled = true;
            button11.Enabled = true;

            //listView_TIDs.Clear();
            //listView_TIDs.Columns.Add("TID");
            //listView_TIDs.Columns.Add("Route#1");
            //listView_TIDs.Columns.Add("Route#2");
            //listView_TIDs.Columns.Add("Route#3");
            //listView_TIDs.Columns.Add("Route#4");
            //listView_TIDs.Columns.Add("Route#5");

            listView1.Clear();
            listView1.Columns.Add("Issuing");
            listView1.Columns.Add("Routing");
            listView1.Columns.Add("Instal-From");
            listView1.Columns.Add("Instal-To");
            listView1.Columns.Add("Amount-From");                   // 090218
            listView1.Columns.Add("Amount-To");                     // 090218

            //label7.ForeColor = System.Drawing.Color.Black;
            //label7.Text = "FOR PROCESS";
            label5.ForeColor = System.Drawing.Color.Black;
            label5.ResetText();
            //label8.ForeColor = System.Drawing.Color.Black;
            //label8.Text = "READY";
            delFlag = 0;

            return;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Array.Clear(loadBalArr, 5, 7);
            //int i = 0;

            string sqlCrtQry = "CREATE TABLE [zacrpt_test].[dbo].[tmp_LB]  (ID int IDENTITY(1,1) PRIMARY KEY, TID nvarchar(16), uploadhost_id varchar(1)) ";   // 090218
            string sqlDelTmpQry = "DELETE from [zacrpt_test].[dbo].[tmp_LB] ";          // 090218

            // Check for table [zacrpt_test].[dbo].[tmp_LB] if exists so to create or not    090218
            bool isExists;
            const string sqlStmnt = @"Select count(*) from [zacrpt_test].[dbo].[tmp_LB]";

            try
            {
                using (OdbcCommand chkExistance = new OdbcCommand(sqlStmnt, conn))
                {
                    chkExistance.ExecuteScalar();
                    isExists = true;
                }
            }
            catch
            {
                isExists = false;
            }

            if (isExists == false)
            {
                OdbcCommand sqlcmdCrt = new OdbcCommand();
                sqlcmdCrt.Connection = conn;
                sqlcmdCrt.CommandTimeout = 0;
                sqlcmdCrt.CommandType = CommandType.Text;
                //sqlcmdDel.CommandText = sqlDelQry;
                sqlcmdCrt.CommandText = sqlCrtQry;
                OdbcDataReader rdrCrt = sqlcmdCrt.ExecuteReader();
                rdrCrt.Close();
                sqlcmdCrt.Dispose();
            }
            else
            {
                OdbcCommand sqlcmdDel = new OdbcCommand();
                sqlcmdDel.Connection = conn;
                sqlcmdDel.CommandTimeout = 0;
                sqlcmdDel.CommandType = CommandType.Text;
                //sqlcmdDel.CommandText = sqlDelQry;
                sqlcmdDel.CommandText = sqlDelTmpQry;
                OdbcDataReader rdrDel = sqlcmdDel.ExecuteReader();
                rdrDel.Close();
                sqlcmdDel.Dispose();
            }

              


            foreach (ListViewItem itemTID2 in listView_TIDs.Items)
            {
                tidDescr.Text = itemTID2.SubItems[0].Text.ToString();

                // Delete LB from LoadBal file so to insert new selections   090218
                OdbcCommand comLdBal_Del = conn.CreateCommand();
                comLdBal_Del.CommandText = "delete dbo.LOADBALANCE_test where balancegroup in (select 'M'+tid from dbo.MERCHANTS_test where tid = ?)";
                comLdBal_Del.Parameters.Add("mid", OdbcType.Char).Value = tidDescr.Text;
                OdbcDataReader readerCursorDelLb = comLdBal_Del.ExecuteReader();
                readerCursorDelLb.Close();



                //Insert data for mid to temp file      090218
                OdbcCommand lbInsert_ = conn.CreateCommand();
                lbInsert_.CommandText = "delete from [zacrpt_test].[dbo].[tmp_LB]; " +
                                         "Insert into [zacrpt_test].[dbo].[tmp_LB](tid, uploadhost_id) " +
                                         "Select distinct('M'+TID), uploadhostid FROM dbo.MERCHANTS_test WHERE tid = ? ";
                lbInsert_.Parameters.Add("mid", OdbcType.Char).Value = tidDescr.Text;
                OdbcDataReader readerLb_Temp = lbInsert_.ExecuteReader();
                readerLb_Temp.Close();
                        



                int i = 0;
                foreach (ListViewItem item in listView2.Items)
                {
                    string tmpLD_Descr = item.SubItems[0].Text.ToString();
                    switch (tmpLD_Descr)
                    {
                        case "PIRAEUS":
                            loadBalArr[i, 0] = "NET_ABC";
                            loadBalArr[i, 3] = "1";
                            break;
                        case "ETHNIKI":
                            loadBalArr[i, 0] = "NET_NTBN";
                            loadBalArr[i, 3] = "6";
                            break;
                        case "ALPMOR":
                            loadBalArr[i, 0] = "NET_ALPMOR";
                            loadBalArr[i, 3] = "201";
                            break;
                        case "ALPHABANK":
                            loadBalArr[i, 0] = "NET_BICALPHA";
                            loadBalArr[i, 3] = "302";
                            break;
                        case "EUROBANK":
                            loadBalArr[i, 0] = "NET_CLBICEBNK";
                            loadBalArr[i, 3] = "205";
                            break;
                        case "ATTIKA":
                            loadBalArr[i, 0] = "NET_ATTIKA";
                            loadBalArr[i, 3] = "7";
                            break;
                        case "EURMOR":
                            loadBalArr[i, 0] = "NET_EURMOR";
                            loadBalArr[i, 3] = "989";
                            break;

                    }

                    //loadvalue
                    loadBalArr[i, 1] = item.SubItems[1].Text.ToString();
                    //Type
                    loadBalArr[i, 2] = item.SubItems[5].Text.ToString();
                    //gr
                    loadBalArr[i, 4] = item.SubItems[2].Text.ToString();
                    //eu
                    loadBalArr[i, 5] = item.SubItems[3].Text.ToString();
                    //other
                    loadBalArr[i, 6] = item.SubItems[4].Text.ToString();
                    //i++;   090218
                

                OdbcCommand comLdBal_Cursor = conn.CreateCommand();

              
                string ldBl0_0 = loadBalArr[0, 0].ToString();
                string ldBl0_1 = loadBalArr[0, 1].ToString();
                string ldBl0_2 = loadBalArr[0, 2].ToString();
                string ldBl0_4 = loadBalArr[0, 4].ToString();
                string ldBl0_5 = loadBalArr[0, 5].ToString();
                string ldBl0_6 = loadBalArr[0, 6].ToString();
                string ldBl0_Host = loadBalArr[0, 3].ToString();
              

                // 090218
                comLdBal_Cursor.CommandText =      "with LBTEmp(mTid, UploadID) as " +
                                                   "(select tid, uploadhost_id from [dbo].[tmp_LB]) " +
                                                    " insert into loadbalance_test(BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE, GR_VAL, EU_VAL, OTHER_VAL) " +
                                                    " select mTid, '" + ldBl0_0 + "' , '" + ldBl0_2 + "','" + ldBl0_1 + "','" + ldBl0_4 + "','" + ldBl0_5 + "','" + ldBl0_6 + "'" +
                                                    " from LBTemp " +
                                                    " where UploadID = ? ";                                                       

                        comLdBal_Cursor.Parameters.Add("uploadhostid", OdbcType.Char).Value = ldBl0_Host;
                        comLdBal_Cursor.Connection = conn;
                        OdbcDataReader readerCursor6 = comLdBal_Cursor.ExecuteReader();
                        comLdBal_Cursor.Dispose();
                        readerCursor6.Close();
            }
                MessageBox.Show("Load Balance Completed for TID: " + tidDescr.Text);
        }
            listView2.Clear();
            listView2.Columns.Add("Bank");
            listView2.Columns.Add("Load Balance");
            listView2.Columns.Add("GR Value");
            listView2.Columns.Add("EU Value");
            listView2.Columns.Add("Other Value");
            listView2.Columns.Add("Routing Type");

            return;
        
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void radioButton_Cnt_CheckedChanged(object sender, EventArgs e)
        {
            textBox_lbNbr.Visible = true;
            textBoxlb_gr.Visible = false;
            textBoxlb_other.Visible = false;
            textBoxlb_eu.Visible = false;
        }

        private void radioButton_Amnt_CheckedChanged(object sender, EventArgs e)
        {
            textBox_lbNbr.Visible = true;
            textBoxlb_gr.Visible = false;
            textBoxlb_other.Visible = false;
            textBoxlb_eu.Visible = false;
        }

        private void radioButton_Pri_CheckedChanged(object sender, EventArgs e)
        {
            textBox_lbNbr.Visible = false;
            textBoxlb_gr.Visible = true;
            textBoxlb_other.Visible = true;
            textBoxlb_eu.Visible = true;
        }
    }

}

public class Form4 : Form
{   /*User Login*/
    System.Windows.Forms.Label labelB1;
    System.Windows.Forms.Label labelB2;

    public System.Windows.Forms.TextBox textBoxB1;
    public System.Windows.Forms.TextBox textBoxB2;

    System.Windows.Forms.Button buttonB1;
    System.Windows.Forms.Button buttonB2;

    public void Initialize()
    {
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(270, 270);
        this.Name = "Form4";
        this.Text = "Login";
        this.SuspendLayout();

        this.labelB1 = new System.Windows.Forms.Label();
        this.labelB2 = new System.Windows.Forms.Label();

        this.Controls.Add(labelB1);
        this.Controls.Add(labelB2);

        this.labelB1.AutoSize = true;
        this.labelB1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
        this.labelB1.Location = new System.Drawing.Point(20, 50);
        this.labelB1.Name = "labelB1";
        this.labelB1.Size = new System.Drawing.Size(63, 16);
        this.labelB1.TabIndex = 0;
        this.labelB1.Text = "Username";

        this.labelB2.AutoSize = true;
        this.labelB2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
        this.labelB2.Location = new System.Drawing.Point(20, 100);
        this.labelB2.Name = "labelB2";
        this.labelB2.Size = new System.Drawing.Size(63, 16);
        this.labelB2.TabIndex = 1;
        this.labelB2.Text = "Password";

        this.textBoxB1 = new System.Windows.Forms.TextBox();
        this.textBoxB2 = new System.Windows.Forms.TextBox();

        this.Controls.Add(textBoxB1);
        this.Controls.Add(textBoxB2);

        this.textBoxB1.Location = new System.Drawing.Point(100, 50);
        this.textBoxB1.Name = "textBoxB1";
        this.textBoxB1.Size = new System.Drawing.Size(150, 20);
        this.textBoxB1.TabIndex = 4;

        this.textBoxB2.Location = new System.Drawing.Point(100, 100);
        this.textBoxB2.Name = "textBoxB2";
        this.textBoxB2.Size = new System.Drawing.Size(150, 20);
        this.textBoxB2.TabIndex = 5;
        this.textBoxB2.PasswordChar = '*';

        this.buttonB1 = new System.Windows.Forms.Button();
        this.buttonB2 = new System.Windows.Forms.Button();

        this.Controls.Add(buttonB1);
        this.Controls.Add(buttonB2);

        this.buttonB1.Location = new System.Drawing.Point(20, 200);
        this.buttonB1.Name = "buttonB1";
        this.buttonB1.Size = new System.Drawing.Size(80, 30);
        this.buttonB1.TabIndex = 6;
        this.buttonB1.Text = "OK";
        this.buttonB1.UseVisualStyleBackColor = true;
        this.buttonB1.Click += new System.EventHandler(this.buttonB1_Click);
        this.AcceptButton = this.buttonB1;

        this.buttonB2.Location = new System.Drawing.Point(170, 200);
        this.buttonB2.Name = "buttonB2";
        this.buttonB2.Size = new System.Drawing.Size(80, 30);
        this.buttonB2.TabIndex = 7;
        this.buttonB2.Text = "Cancel";
        this.buttonB2.UseVisualStyleBackColor = true;
        this.buttonB2.Click += new System.EventHandler(this.buttonB2_Click);


    }
    private void buttonB1_Click(object sender, EventArgs e)
    {   /*OK*/
        if (this.textBoxB1.Text == "" || this.textBoxB2.Text == "")
        { }
        //MessageBox.Show("Empty TextBox!");
        else
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
    private void buttonB2_Click(object sender, EventArgs e)
    {   /*Cancel*/
        this.textBoxB1.Text = "";
        this.textBoxB2.Text = "";

        this.Close();
    }
}
