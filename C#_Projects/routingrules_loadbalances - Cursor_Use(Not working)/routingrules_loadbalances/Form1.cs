using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
//using System.Windows.Controls;
using System.Data.SqlClient;


namespace routingrules_loadbalances
{
    public partial class Form1 : Form
    {
        string[,] loadBalArr = new string[4, 3];
        string totals_CntAmnt = " ";

        public Form1()
        {
              
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
                                  
            listView1.Columns.Add("Originator");
            listView1.Columns.Add("Route");
            listView1.Columns.Add("Instal-From");
            listView1.Columns.Add("Instal-To");

            listView2.Columns.Add("Bank");
            listView2.Columns.Add("Load Balance");
            listView2.Columns.Add("Totals By");

            textBoxHidden_Route.Visible = false;
            textBoxFr.Text = " ";
            textBoxTo.Text = " ";

            int[] arrBal =new int[4];
            arrBal[0] = 0;
            arrBal[1] = 1;
            arrBal[2] = 50;
            arrBal[3] = 1000000;

             //string[,] loadBalArr = new string[4, 2];
            //Array.Clear(loadBalArr, 4, 2);


            //comboMID
            OdbcCommand comboFillMids = new OdbcCommand(
               "SELECT MID FROM dbo.MERCHANTS_test " +
                "where mid like '00%' group by [MID]", conn);
            OdbcDataReader reader = comboFillMids.ExecuteReader();
            while (reader.Read())
            {
                string word = reader.GetString(0);

                this.comboGroup.Items.Add(word);

            }
            reader.Close();

            //comboBnkRoute
            OdbcCommand comboFillBnks = new OdbcCommand(
                 "SELECT BANKDEscr FROM dbo.ROUTEBANK where uploadhostid <> ' '",
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
            for (int i = 0; i<100; i++)
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
                 "Select bankdescr_short From dbo.ROUTEBANK",
                 conn);
            OdbcDataReader reader3 = listBoxOrigin.ExecuteReader();
            while (reader3.Read())
            {
                string originDescr = reader3.GetString(0);

                this.listBoxOrigin.Items.Add(originDescr);

            }
            reader3.Close();

        }

        private void button1_Click(object sender, EventArgs e)   {}

        //Insert records to backup file
        private void button2_Click(object sender, EventArgs e)
        {
            int countMER = 0;
            int countBUP = 0;
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
               
          
            if (countMER != countBUP)
               MessageBox.Show("Backup records not equal with initial records");

            label7.ForeColor = System.Drawing.Color.Red;
            label7.Text = "DONE";

            seleMID = midDescr.Text;
        }
        
        private void label4_Click(object sender, EventArgs e) {}

        // MIDs combobox 
        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;
            int selectedIndex = cmb.SelectedIndex;
            Object selectedItem = cmb.SelectedItem;

            //MessageBox.Show("Selected Item Text: " + selectedItem.ToString() + "\n" +
            //                "Index: " + selectedIndex.ToString());
            midDescr.Text = selectedItem.ToString();
            label8.ForeColor = System.Drawing.Color.Red;
            label8.Text = "DONE";
         }
        
        private void midDescr_TextChanged(object sender, EventArgs e) {}

        // Delete TIDs for selected MID
        private void button3_Click(object sender, EventArgs e)
        {
            OdbcCommand com = new OdbcCommand("delete from dbo.MERCHBINS_TEST where tid in (select tid from dbo.merchants_test where mid =" + comboGroup.SelectedItem +")" , conn);
            OdbcDataReader readerIns = com.ExecuteReader();
            readerIns.Close();
            label5.ForeColor = System.Drawing.Color.Red;
            label5.Text = "DONE";
        }
        
        private void radioButton6_CheckedChanged(object sender, EventArgs e) {}
        private void Form1_Load(object sender, EventArgs e) {}
        private void button8_Click(object sender, EventArgs e) {}

        //Verify button so to add in listView
        private void button4_Click(object sender, EventArgs e) 
        {
            //Check if installs are selected so to proceed
            if ((textBoxFr.Text.Equals(" ")) || (textBoxTo.Text.Equals(" ")))
            {
                MessageBox.Show("Installments are not chosen!");
                return;
            }

          // Add to listView so to keep track of what it is done
            listView1.Items.Add(new ListViewItem(new string[] { listBoxOrigin.SelectedItem.ToString(), textBoxHidden_Route.Text,textBoxFr.Text,textBoxTo.Text}));
        }

        private void ntbnTo_ntbn_CheckedChanged(object sender, EventArgs e) {}

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
            listView1.Columns.Add("Originator");
            listView1.Columns.Add("Route");
            listView1.Columns.Add("Instal-From");
            listView1.Columns.Add("Instal-To");
        }

        // Proceed to insert of TIDs to MERCHBIN file
        private void button5_Click(object sender, EventArgs e)
        {
            string sublstItm1;
            string sublstItm2;
            string chkOriginID= " ";
            string route_hostID = " ";
            string chkOriginID2 = " ";
            string route_hostID2 = " ";
            string templKarf = "1111    ";
            
            OdbcCommand comCursor = conn.CreateCommand();
            //OdbcDataReader readerCursor = new OdbcDataReader();


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
                        case "ALPHABANK":
                            chkOriginID = "202";
                            route_hostID = "NET_CLBICALPHA";
                            break;
                        case "EUROBANK":
                            chkOriginID = "205";
                            route_hostID = "NET_CLBICEBNK";
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
                        case "ALPHABANK":
                            chkOriginID2 = "202";
                            route_hostID2 = "NET_CLBICALPHA";
                            break;
                        case "EUROBANK":
                            chkOriginID2 = "205";
                            route_hostID2 = "NET_CLBICEBNK";
                            break;
                    }

                    textBoxFr.Text.ToString();
                    textBoxTo.Text.ToString();

                    //OdbcCommand comCursor = conn.CreateCommand();

                    if (chkOriginID == "1" || chkOriginID == "6" || chkOriginID == "202" || chkOriginID == "205")
                    {
                        // Check if trxs will routed to same bank id so to change DESTPORT
                        if (chkOriginID == chkOriginID2)
                        {

                            comCursor.CommandText = "declare @tid varchar(16) " +
                                                    "declare @name varchar(20) " +
                                                    "declare @mid varchar(16) " +
                                                    "declare @uploadhost int " +
                                                    "declare @DESTPORT varchar(20) " +
                                                    "declare merch_cursor cursor for " +
                                                    "    select  TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                    "    where mid =  ? " +
                                                    "     and a.uploadhostid = ?  " +
                                                    "     and a.uploadhostid = b.uploadhostid " +
                                                    "open merch_cursor; " +
                                                    " if @@ERROR > 0 " +
                                                    " return; " +
                                                    "   fetch next from merch_cursor into @tid, @name " +
                                                    "    while @@FETCH_STATUS = 0 " +
                                                    "   begin " +
                                                    "    insert into dbo.MERCHBINS_TEST " +
                                                    "     (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                                                    "     (select @TID, DESTPORT, BINLOWER, BINUPPER," +
                                                           textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                    "      GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from dbo.MERCHBINS_TEST " +
                                                    "       where TID = " + templKarf + " and DESTPORT =  ?) " +
                                                    "   fetch next from merch_cursor " +
                                                    "   into @tid, @name " +
                                                    "   end " +
                                                    " CLOSE merch_cursor " +
                                                    " deallocate merch_cursor ";
                        }
                        else
                        {
                            comCursor.CommandText = "declare @tid varchar(16) " +
                         "declare @name varchar(20) " +
                         "declare @mid varchar(16) " +
                         "declare @uploadhost int " +
                         "declare @DESTPORT varchar(20) " +
                         "declare merch_cursor cursor for " +
                         "    select  TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                         "    where mid = " + midDescr.Text +   
                         "     and a.uploadhostid = " + chkOriginID +
                         "     and a.uploadhostid = b.uploadhostid " +
                         "open merch_cursor " +
                         " if @@ERROR > 0 " +
                         " return; " +
                         "   fetch next from merch_cursor into @tid, @name " +
                         "    while @@FETCH_STATUS = 0 " +
                         "   begin " +
                         "    insert into dbo.MERCHBINS_TEST " +
                         "     (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                         "     (select @TID, '" + route_hostID2 + "', BINLOWER, BINUPPER, " +
                                textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                         "      GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from dbo.MERCHBINS_TEST " +
                         "       where TID = " + templKarf + " and DESTPORT = '" + route_hostID  +"') " +
                         "   fetch next from merch_cursor " +
                         "   into @tid, @name " +
                         "   end " +
                         " CLOSE merch_cursor " +
                         " deallocate merch_cursor ";

                            //comCursor.CommandText = "declare @tid varchar(16) " +
                            //                         "declare @name varchar(20) " +
                            //                         "declare @mid varchar(16) " +
                            //                         "declare @uploadhost int " +
                            //                         "declare @DESTPORT varchar(20) " +
                            //                         "declare merch_cursor cursor for " +
                            //                         "    select  TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                            //                         "    where mid =  ? " +
                            //                         "     and a.uploadhostid = ? " +
                            //                         "     and a.uploadhostid = b.uploadhostid " +
                            //                         "open merch_cursor " +
                            //                         " if @@ERROR > 0 " +
                            //                         " return; " +
                            //                         "   fetch next from merch_cursor into @tid, @name " +
                            //                         "    while @@FETCH_STATUS = 0 " +
                            //                         "   begin " +
                            //                         "    insert into dbo.MERCHBINS_TEST " +
                            //                         "     (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX, GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX) " +
                            //                         "     (select @TID, '" + route_hostID2 + "', BINLOWER, BINUPPER, " +
                            //                                textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                            //                         "      GRACEMIN, GRACEMAX, ALLOWED, AMOUNTMIN, AMOUNTMAX from dbo.MERCHBINS_TEST " +
                            //                         "       where TID = " + templKarf + " and DESTPORT =  ?) " +
                            //                         "   fetch next from merch_cursor " +
                            //                         "   into @tid, @name " +
                            //                         "   end " +
                            //                         " CLOSE merch_cursor " +
                            //                         " deallocate merch_cursor ";
                        }
                        
                        comCursor.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID;
                        comCursor.Parameters.Add("@DESTPORT", OdbcType.VarChar, 20).Value = route_hostID;

                        OdbcDataReader readerCursor1 = comCursor.ExecuteReader();
                        readerCursor1.Close();

                    }
                    else if (chkOrigin == "KOYBAS")
                    {
                        comCursor.CommandText = "declare @tid varchar(16) " +
                                                "declare @name varchar(20) " +
                                                "declare @mid varchar(16) " +
                                                "declare @uploadhost int " +
                                                "declare merch_cursor cursor for " +
                                                "    select  TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                "    where mid =  ? " +
                                                "     and a.uploadhostid = ? " +
                                                "     and a.uploadhostid = b.uploadhostid " +
                                                "open merch_cursor " +
                                                " if @@ERROR > 0 " +
                                                " return; " +
                                                "   fetch next from merch_cursor into @tid, @name " +
                                                "    while @@FETCH_STATUS = 0 " +
                                                "   begin " +
                                                " insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                "    values (@TID, @name, '4', '6', " +
                                                     textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                "     0, 99, 'Y', 0 , 999999999) " +
                                                " insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                "    values (@TID, @name, '9', '9', " +
                                                     textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                "    0, 99, 'Y', 0 , 999999999) " +
                                                " insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                "    values (@TID, @name, '222100', '272099', " +
                                                     textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                "    0, 99, 'Y', 0 , 999999999) " +
                                                "   fetch next from merch_cursor " +
                                                "   into @tid, @name " +
                                                "   end " +
                                                " CLOSE merch_cursor " +
                                                " deallocate merch_cursor ";

                        comCursor.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                        OdbcDataReader readerCursor2 = comCursor.ExecuteReader();
                        readerCursor2.Close();

                    }
                    else if (chkOrigin == "MEALSANDMORE")
                    {
                        comCursor.CommandText = "declare @tid varchar(16) " +
                                                "declare @name varchar(20) " +
                                                "declare @mid varchar(16) " +
                                                "declare @uploadhost int " +
                                                "declare merch_cursor cursor for " +
                                                "    select  TID, uploadhostname from dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                "    where mid =  ? " +
                                                "     and a.uploadhostid = ? " +
                                                "     and a.uploadhostid = b.uploadhostid " +
                                                "open merch_cursor " +
                                                " if @@ERROR > 0 " +
                                                " return " +
                                                "   fetch next from merch_cursor into @tid, @name " +
                                                "    while @@FETCH_STATUS = 0 " +
                                                "   begin " +
                                                " insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                "  values (@TID, @name, '502259', '502259', " +
                                                     textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                "   0, 99, 'Y', 0 , 999999999) " +
                                                "   fetch next from merch_cursor " +
                                                "   into @tid, @name " +
                                                "   end " +
                                                " CLOSE merch_cursor " +
                                                " deallocate merch_cursor ";

                        comCursor.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                        OdbcDataReader readerCursor3 = comCursor.ExecuteReader();
                        readerCursor3.Close();

                    }
                    else if (chkOrigin == "TICKETRESTAURANT")
                    {
                        comCursor.CommandText = "declare @tid varchar(16) " +
                                                "declare @name varchar(20) " +
                                                "declare @mid varchar(16) " +
                                                "declare @uploadhost int " +
                                                "declare merch_cursor cursor for " +
                                                "    select  TID, uploadhostname from  dbo.MERCHANTS_test a, dbo.uploadhosts b " +
                                                "    where @mid =  ? " +
                                                "     and @a.uploadhostid = ? " +
                                                "     and a.uploadhostid = b.uploadhostid " +
                                                "open merch_cursor " +
                                                " if @@ERROR > 0 " +
                                                " return " +
                                                "   fetch next from merch_cursor into @tid, @name " +
                                                "    while @@FETCH_STATUS = 0 " +
                                                "   begin " +
                                                " insert into dbo.MERCHBINS_TEST (TID, DESTPORT, BINLOWER, BINUPPER, INSTMIN, INSTMAX,gracemin,gracemax,allowed, AMOUNTMIN, AMOUNTMAX) " +
                                                "  values (@TID, @name, '534228', '534228', " +
                                                     textBoxFr.Text.ToString() + "," + textBoxTo.Text.ToString() + "," +
                                                "   0, 99, 'Y', 0 , 999999999) " +
                                                "   fetch next from merch_cursor " +
                                                "   into @tid, @name " +
                                                "   end " +
                                                " CLOSE merch_cursor " +
                                                " deallocate merch_cursor ";

                        comCursor.Parameters.Add("@mid", OdbcType.VarChar, 16).Value = midDescr.Text;
                        comCursor.Parameters.Add("@uploadhost", OdbcType.Int).Value = chkOriginID2;
                        OdbcDataReader readerCursor4 = comCursor.ExecuteReader();
                        readerCursor4.Close();

                    }
                //}
            }
             
            MessageBox.Show("Insert completed!");
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
         listView2.Columns.Add("Totals By");
         }

        private void button8_Click_1(object sender, EventArgs e)
        {
         if ((comboBoxLD.Text.Equals("")) || (comboBoxPrcntge.Text.Equals("")))
           {
                MessageBox.Show("Please choose Bank or Load Balance % ");
                return;
            }

       if (checkBox1.Checked == true)
           {
              totals_CntAmnt = "T";
           }
        else
           {
             totals_CntAmnt = "C";
           }
                
            // Add to listView so to keep track of what it is done
            listView2.Items.Add(new ListViewItem(new string[] { comboBoxLD.Text, comboBoxPrcntge.Text, totals_CntAmnt }));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Array.Clear(loadBalArr, 4, 3);
            int i = 0;
            foreach (ListViewItem item in listView2.Items)
            {
                string tmpLD_Descr = item.SubItems[0].Text.ToString();
                switch (tmpLD_Descr)
                {
                 case "PIRAEUS":
                     loadBalArr[i, 0] = "NET_ABC";
                     break;
                 case "ETHNIKI":
                     loadBalArr[i, 0] = "NET_NTBN";
                     break;
                 case "ALPHABANK":
                     loadBalArr[i, 0] = "NET_CLBICALPHA";
                     break;
                 case "EUROBANK":
                     loadBalArr[i, 0] = "NET_CLBICEBNK";
                     break;
                }
                
             loadBalArr[i, 1] = item.SubItems[1].Text.ToString();
             loadBalArr[i, 2] = item.SubItems[2].Text.ToString();
             i++;
            }

            OdbcCommand comLdBal_Cursor = conn.CreateCommand();

            // Delete LB from LoadBal file so to insert new selections
            comLdBal_Cursor.CommandText = "delete dbo.LOADBALANCE_test where balancegroup in (select 'M'+tid from dbo.MERCHANTS_test where mid = ?)";
            comLdBal_Cursor.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
            OdbcDataReader readerCursor5 = comLdBal_Cursor.ExecuteReader();
            readerCursor5.Close();
            
            comLdBal_Cursor.CommandText =
                "declare @TID varchar(16), @BALGROUP varchar(20)" +
                "DECLARE merch_cursor CURSOR FOR " +
                "  SELECT distinct(tid) FROM dbo.MERCHANTS_test WHERE mid = ? " +
                "  OPEN merch_cursor " +
                " if @@ERROR > 0 " +
                "  return " +
                "  FETCH NEXT FROM merch_cursor INTO @TID " +
                "  WHILE @@FETCH_STATUS = 0 " +
                "  BEGIN " +
                "  SET @BALGROUP = 'M'+@TID " +
                "   INSERT INTO dbo.LOADBALANCE_test (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
                "        VALUES (@BALGROUP, " +
                    loadBalArr[0, 0] + "," + loadBalArr[0, 2] + loadBalArr[0, 1] + ")" +
                "   INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
                "        VALUES (@BALGROUP, " +
                    loadBalArr[1, 0] + "," + loadBalArr[1, 2] + loadBalArr[1, 1] + ")" +
                "   INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
                "        VALUES (@BALGROUP, " +
                    loadBalArr[2, 0] + "," + loadBalArr[2, 2] + loadBalArr[2, 1] + ")" +
                "   INSERT INTO LOADBALANCE (BALANCEGROUP, DESTPORT, LOADTYPE, LOADVALUE) " +
                "        VALUES (@BALGROUP, " +
                    loadBalArr[3, 0] + "," + loadBalArr[3, 2] + loadBalArr[3, 1] + ")" +
                "        FETCH NEXT FROM merch_cursor " +
                "        into @TID " +
                "   END " +
                "   CLOSE merch_cursor " +
                "  DEALLOCATE merch_cursor " +
                " GO ";

            comLdBal_Cursor.Parameters.Add("mid", OdbcType.Char).Value = midDescr.Text;
            OdbcDataReader readerCursor6 = comLdBal_Cursor.ExecuteReader();
            readerCursor6.Close();

        }

    }
}
