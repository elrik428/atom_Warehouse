namespace routingrules_loadbalances
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public static System.Data.Odbc.OdbcConnection conn;
        public System.String ConnectionString = "ZAC_TEST";
        public bool exit = false;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (conn.State != System.Data.ConnectionState.Closed)
                conn.Close();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboGroup = new System.Windows.Forms.ComboBox();
            this.midDescr = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.button16 = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.listBoxOrigin = new System.Windows.Forms.ListBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.comboBnkRoute = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label24 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxHidden_Route = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox_InstalFr = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_InstalTo = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.textBoxFr = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxAmntTo = new System.Windows.Forms.TextBox();
            this.textBoxAmntFr = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_CurrRoute = new System.Windows.Forms.TextBox();
            this.textBox__CurrMinInst = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_CurrMaxInst = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton_UpdBIN = new System.Windows.Forms.RadioButton();
            this.radioButton_AddBin = new System.Windows.Forms.RadioButton();
            this.button11 = new System.Windows.Forms.Button();
            this.textBox_NEWBIN = new System.Windows.Forms.TextBox();
            this.pbar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxlb_other = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBoxlb_eu = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.textBoxlb_gr = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.radioButton_Pri = new System.Windows.Forms.RadioButton();
            this.textBox_lbNbr = new System.Windows.Forms.TextBox();
            this.radioButton_Amnt = new System.Windows.Forms.RadioButton();
            this.radioButton_Cnt = new System.Windows.Forms.RadioButton();
            this.button9 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.comboBoxLD = new System.Windows.Forms.ComboBox();
            this.comboBoxPrcntge = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxABC = new System.Windows.Forms.TextBox();
            this.textBoxNTBN = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxALPHA = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxEBNK = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.listView_MIDs = new System.Windows.Forms.ListView();
            this.button10 = new System.Windows.Forms.Button();
            this.listView_BINChk = new System.Windows.Forms.ListView();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxATTIKA = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_MultMid = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_hiddenCpy = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView_TIDs = new System.Windows.Forms.ListView();
            this.label7 = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.textBox_MultTid = new System.Windows.Forms.TextBox();
            this.button13 = new System.Windows.Forms.Button();
            this.tidDescr = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button19 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(1062, 67);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Insert records to Backup";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "1.";
            // 
            // comboGroup
            // 
            this.comboGroup.FormattingEnabled = true;
            this.comboGroup.Location = new System.Drawing.Point(11, 37);
            this.comboGroup.Name = "comboGroup";
            this.comboGroup.Size = new System.Drawing.Size(224, 21);
            this.comboGroup.TabIndex = 5;
            this.comboGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // midDescr
            // 
            this.midDescr.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.midDescr.Location = new System.Drawing.Point(1111, 40);
            this.midDescr.Name = "midDescr";
            this.midDescr.Size = new System.Drawing.Size(103, 20);
            this.midDescr.TabIndex = 7;
            this.midDescr.Visible = false;
            this.midDescr.TextChanged += new System.EventHandler(this.midDescr_TextChanged);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(1062, 100);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1059, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Delete records for selected MID";
            this.label5.Visible = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button4.Location = new System.Drawing.Point(256, 251);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 22);
            this.button4.TabIndex = 92;
            this.button4.Text = "Add selection";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(12, 293);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(16, 13);
            this.label39.TabIndex = 104;
            this.label39.Text = "2.";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(1061, 394);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(155, 23);
            this.button16.TabIndex = 105;
            this.button16.Text = "Check TIDs";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Visible = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(1049, 420);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(167, 13);
            this.label40.TabIndex = 106;
            this.label40.Text = "Check TIDs for banks connection";
            this.label40.Visible = false;
            // 
            // listBoxOrigin
            // 
            this.listBoxOrigin.FormattingEnabled = true;
            this.listBoxOrigin.Location = new System.Drawing.Point(19, 43);
            this.listBoxOrigin.Name = "listBoxOrigin";
            this.listBoxOrigin.Size = new System.Drawing.Size(206, 199);
            this.listBoxOrigin.TabIndex = 107;
            this.listBoxOrigin.SelectedIndexChanged += new System.EventHandler(this.listBoxOrigin_SelectedIndexChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(17, 22);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(67, 13);
            this.label41.TabIndex = 108;
            this.label41.Text = "Issuing Bank";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(17, 248);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(105, 13);
            this.label23.TabIndex = 109;
            this.label23.Text = "Τράπεζα προορισμού";
            // 
            // comboBnkRoute
            // 
            this.comboBnkRoute.FormattingEnabled = true;
            this.comboBnkRoute.Location = new System.Drawing.Point(20, 267);
            this.comboBnkRoute.Name = "comboBnkRoute";
            this.comboBnkRoute.Size = new System.Drawing.Size(121, 21);
            this.comboBnkRoute.TabIndex = 110;
            this.comboBnkRoute.SelectedIndexChanged += new System.EventHandler(this.comboBnkRoute_SelectedIndexChanged);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(256, 43);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(360, 199);
            this.listView1.TabIndex = 111;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(253, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 13);
            this.label24.TabIndex = 112;
            this.label24.Text = "Προβολή συνδυασμού";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(487, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 25);
            this.button1.TabIndex = 113;
            this.button1.Text = "Clear selections";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBoxHidden_Route
            // 
            this.textBoxHidden_Route.Location = new System.Drawing.Point(1098, 142);
            this.textBoxHidden_Route.Name = "textBoxHidden_Route";
            this.textBoxHidden_Route.Size = new System.Drawing.Size(118, 20);
            this.textBoxHidden_Route.TabIndex = 114;
            this.textBoxHidden_Route.Visible = false;
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(622, 89);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(96, 65);
            this.button5.TabIndex = 115;
            this.button5.Text = "Proceed ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1116, 194);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 116;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1116, 168);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 117;
            this.textBox2.Visible = false;
            // 
            // comboBox_InstalFr
            // 
            this.comboBox_InstalFr.FormattingEnabled = true;
            this.comboBox_InstalFr.Location = new System.Drawing.Point(25, 333);
            this.comboBox_InstalFr.Name = "comboBox_InstalFr";
            this.comboBox_InstalFr.Size = new System.Drawing.Size(39, 21);
            this.comboBox_InstalFr.TabIndex = 118;
            this.comboBox_InstalFr.SelectedIndexChanged += new System.EventHandler(this.comboBox_InstalFr_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 119;
            this.label6.Text = "Installments 0 - 99";
            // 
            // comboBox_InstalTo
            // 
            this.comboBox_InstalTo.FormattingEnabled = true;
            this.comboBox_InstalTo.Location = new System.Drawing.Point(77, 333);
            this.comboBox_InstalTo.Name = "comboBox_InstalTo";
            this.comboBox_InstalTo.Size = new System.Drawing.Size(39, 21);
            this.comboBox_InstalTo.TabIndex = 120;
            this.comboBox_InstalTo.SelectedIndexChanged += new System.EventHandler(this.comboBox_InstalTo_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(23, 316);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(31, 13);
            this.label25.TabIndex = 121;
            this.label25.Text = "From";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(77, 316);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(19, 13);
            this.label26.TabIndex = 122;
            this.label26.Text = "To";
            // 
            // textBoxFr
            // 
            this.textBoxFr.Location = new System.Drawing.Point(1116, 220);
            this.textBoxFr.Name = "textBoxFr";
            this.textBoxFr.Size = new System.Drawing.Size(39, 20);
            this.textBoxFr.TabIndex = 123;
            this.textBoxFr.Visible = false;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(1161, 220);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(39, 20);
            this.textBoxTo.TabIndex = 124;
            this.textBoxTo.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(487, 279);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 24);
            this.button6.TabIndex = 125;
            this.button6.Text = "Delete last combination";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.textBoxAmntTo);
            this.groupBox1.Controls.Add(this.textBoxAmntFr);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_CurrRoute);
            this.groupBox1.Controls.Add(this.textBox__CurrMinInst);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_CurrMaxInst);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.radioButton_UpdBIN);
            this.groupBox1.Controls.Add(this.radioButton_AddBin);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox_NEWBIN);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.listBoxOrigin);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.comboBnkRoute);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.comboBox_InstalTo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBox_InstalFr);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(739, 449);
            this.groupBox1.TabIndex = 126;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Κανόνες Δρομολόγησης";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(623, 161);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 155;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(644, 157);
            this.label21.Name = "label21";
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label21.Size = new System.Drawing.Size(89, 33);
            this.label21.TabIndex = 154;
            this.label21.Text = "Additional \r\nInstallment ruling";
            // 
            // textBoxAmntTo
            // 
            this.textBoxAmntTo.Location = new System.Drawing.Point(255, 332);
            this.textBoxAmntTo.Name = "textBoxAmntTo";
            this.textBoxAmntTo.Size = new System.Drawing.Size(92, 21);
            this.textBoxAmntTo.TabIndex = 141;
            // 
            // textBoxAmntFr
            // 
            this.textBoxAmntFr.Location = new System.Drawing.Point(173, 332);
            this.textBoxAmntFr.Name = "textBoxAmntFr";
            this.textBoxAmntFr.Size = new System.Drawing.Size(75, 21);
            this.textBoxAmntFr.TabIndex = 140;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(252, 316);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(19, 13);
            this.label19.TabIndex = 139;
            this.label19.Text = "To";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(171, 316);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(31, 13);
            this.label20.TabIndex = 138;
            this.label20.Text = "From";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(171, 300);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(117, 13);
            this.label18.TabIndex = 137;
            this.label18.Text = "Amount 0 - 999999999";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 375);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 136;
            this.label2.Text = "Current Route";
            // 
            // textBox_CurrRoute
            // 
            this.textBox_CurrRoute.Location = new System.Drawing.Point(446, 367);
            this.textBox_CurrRoute.Name = "textBox_CurrRoute";
            this.textBox_CurrRoute.Size = new System.Drawing.Size(100, 21);
            this.textBox_CurrRoute.TabIndex = 135;
            // 
            // textBox__CurrMinInst
            // 
            this.textBox__CurrMinInst.Location = new System.Drawing.Point(447, 392);
            this.textBox__CurrMinInst.Name = "textBox__CurrMinInst";
            this.textBox__CurrMinInst.Size = new System.Drawing.Size(36, 21);
            this.textBox__CurrMinInst.TabIndex = 134;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(357, 424);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 133;
            this.label4.Text = "Current Max Inst";
            // 
            // textBox_CurrMaxInst
            // 
            this.textBox_CurrMaxInst.Location = new System.Drawing.Point(447, 416);
            this.textBox_CurrMaxInst.Name = "textBox_CurrMaxInst";
            this.textBox_CurrMaxInst.Size = new System.Drawing.Size(37, 21);
            this.textBox_CurrMaxInst.TabIndex = 132;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(356, 400);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 131;
            this.label3.Text = "Current Min Inst";
            // 
            // radioButton_UpdBIN
            // 
            this.radioButton_UpdBIN.AutoSize = true;
            this.radioButton_UpdBIN.Location = new System.Drawing.Point(255, 373);
            this.radioButton_UpdBIN.Name = "radioButton_UpdBIN";
            this.radioButton_UpdBIN.Size = new System.Drawing.Size(92, 17);
            this.radioButton_UpdBIN.TabIndex = 130;
            this.radioButton_UpdBIN.TabStop = true;
            this.radioButton_UpdBIN.Text = "Update Ruling";
            this.radioButton_UpdBIN.UseVisualStyleBackColor = true;
            // 
            // radioButton_AddBin
            // 
            this.radioButton_AddBin.AutoSize = true;
            this.radioButton_AddBin.Location = new System.Drawing.Point(157, 373);
            this.radioButton_AddBin.Name = "radioButton_AddBin";
            this.radioButton_AddBin.Size = new System.Drawing.Size(99, 17);
            this.radioButton_AddBin.TabIndex = 129;
            this.radioButton_AddBin.TabStop = true;
            this.radioButton_AddBin.Text = "Add xtra Ruling";
            this.radioButton_AddBin.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(25, 397);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(129, 21);
            this.button11.TabIndex = 128;
            this.button11.Text = "Add / Update Ruling";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // textBox_NEWBIN
            // 
            this.textBox_NEWBIN.Location = new System.Drawing.Point(25, 370);
            this.textBox_NEWBIN.Name = "textBox_NEWBIN";
            this.textBox_NEWBIN.Size = new System.Drawing.Size(129, 21);
            this.textBox_NEWBIN.TabIndex = 127;
            // 
            // pbar1
            // 
            this.pbar1.Location = new System.Drawing.Point(34, 809);
            this.pbar1.Maximum = 5000;
            this.pbar1.Name = "pbar1";
            this.pbar1.Size = new System.Drawing.Size(756, 11);
            this.pbar1.TabIndex = 137;
            this.pbar1.Visible = false;
            this.pbar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.textBoxlb_other);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.textBoxlb_eu);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.textBoxlb_gr);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.radioButton_Pri);
            this.groupBox2.Controls.Add(this.textBox_lbNbr);
            this.groupBox2.Controls.Add(this.radioButton_Amnt);
            this.groupBox2.Controls.Add(this.radioButton_Cnt);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.listView2);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.comboBoxLD);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(566, 354);
            this.groupBox2.TabIndex = 127;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Balance";
            // 
            // textBoxlb_other
            // 
            this.textBoxlb_other.Location = new System.Drawing.Point(325, 92);
            this.textBoxlb_other.Name = "textBoxlb_other";
            this.textBoxlb_other.Size = new System.Drawing.Size(98, 20);
            this.textBoxlb_other.TabIndex = 147;
            this.textBoxlb_other.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(218, 75);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(52, 13);
            this.label29.TabIndex = 146;
            this.label29.Text = "EU Value";
            // 
            // textBoxlb_eu
            // 
            this.textBoxlb_eu.Location = new System.Drawing.Point(221, 91);
            this.textBoxlb_eu.Name = "textBoxlb_eu";
            this.textBoxlb_eu.Size = new System.Drawing.Size(98, 20);
            this.textBoxlb_eu.TabIndex = 145;
            this.textBoxlb_eu.Visible = false;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(322, 75);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(63, 13);
            this.label28.TabIndex = 144;
            this.label28.Text = "Other Value";
            // 
            // textBoxlb_gr
            // 
            this.textBoxlb_gr.Location = new System.Drawing.Point(117, 91);
            this.textBoxlb_gr.Name = "textBoxlb_gr";
            this.textBoxlb_gr.Size = new System.Drawing.Size(98, 20);
            this.textBoxlb_gr.TabIndex = 143;
            this.textBoxlb_gr.Visible = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(114, 75);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(53, 13);
            this.label27.TabIndex = 142;
            this.label27.Text = "GR Value";
            // 
            // radioButton_Pri
            // 
            this.radioButton_Pri.AutoSize = true;
            this.radioButton_Pri.Location = new System.Drawing.Point(289, 50);
            this.radioButton_Pri.Name = "radioButton_Pri";
            this.radioButton_Pri.Size = new System.Drawing.Size(56, 17);
            this.radioButton_Pri.TabIndex = 136;
            this.radioButton_Pri.TabStop = true;
            this.radioButton_Pri.Text = "Priority";
            this.radioButton_Pri.UseVisualStyleBackColor = true;
            this.radioButton_Pri.CheckedChanged += new System.EventHandler(this.radioButton_Pri_CheckedChanged);
            // 
            // textBox_lbNbr
            // 
            this.textBox_lbNbr.Location = new System.Drawing.Point(14, 92);
            this.textBox_lbNbr.Name = "textBox_lbNbr";
            this.textBox_lbNbr.Size = new System.Drawing.Size(98, 20);
            this.textBox_lbNbr.TabIndex = 134;
            // 
            // radioButton_Amnt
            // 
            this.radioButton_Amnt.AutoSize = true;
            this.radioButton_Amnt.Location = new System.Drawing.Point(211, 50);
            this.radioButton_Amnt.Name = "radioButton_Amnt";
            this.radioButton_Amnt.Size = new System.Drawing.Size(63, 17);
            this.radioButton_Amnt.TabIndex = 133;
            this.radioButton_Amnt.TabStop = true;
            this.radioButton_Amnt.Text = "Amoumt";
            this.radioButton_Amnt.UseVisualStyleBackColor = true;
            this.radioButton_Amnt.CheckedChanged += new System.EventHandler(this.radioButton_Amnt_CheckedChanged);
            // 
            // radioButton_Cnt
            // 
            this.radioButton_Cnt.AutoSize = true;
            this.radioButton_Cnt.Location = new System.Drawing.Point(141, 50);
            this.radioButton_Cnt.Name = "radioButton_Cnt";
            this.radioButton_Cnt.Size = new System.Drawing.Size(53, 17);
            this.radioButton_Cnt.TabIndex = 132;
            this.radioButton_Cnt.TabStop = true;
            this.radioButton_Cnt.Text = "Count";
            this.radioButton_Cnt.UseVisualStyleBackColor = true;
            this.radioButton_Cnt.CheckedChanged += new System.EventHandler(this.radioButton_Cnt_CheckedChanged);
            // 
            // button9
            // 
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(447, 185);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(85, 52);
            this.button9.TabIndex = 131;
            this.button9.Text = "Proceed";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 13);
            this.label11.TabIndex = 129;
            this.label11.Text = "Load Balance %";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 128;
            this.label10.Text = "Bank";
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(11, 118);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(412, 173);
            this.listView2.TabIndex = 126;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(11, 297);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(121, 24);
            this.button8.TabIndex = 4;
            this.button8.Text = "Add selections";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click_1);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(138, 297);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(114, 24);
            this.button7.TabIndex = 3;
            this.button7.Text = "Clear selections";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // comboBoxLD
            // 
            this.comboBoxLD.FormattingEnabled = true;
            this.comboBoxLD.Location = new System.Drawing.Point(14, 46);
            this.comboBoxLD.Name = "comboBoxLD";
            this.comboBoxLD.Size = new System.Drawing.Size(105, 21);
            this.comboBoxLD.TabIndex = 0;
            // 
            // comboBoxPrcntge
            // 
            this.comboBoxPrcntge.FormattingEnabled = true;
            this.comboBoxPrcntge.Location = new System.Drawing.Point(1093, 14);
            this.comboBoxPrcntge.Name = "comboBoxPrcntge";
            this.comboBoxPrcntge.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPrcntge.TabIndex = 1;
            this.comboBoxPrcntge.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1113, 440);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 128;
            this.label9.Text = "ΠΕΙΡΑΙΩΣ: ";
            this.label9.Visible = false;
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // textBoxABC
            // 
            this.textBoxABC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxABC.Location = new System.Drawing.Point(1171, 437);
            this.textBoxABC.Name = "textBoxABC";
            this.textBoxABC.ReadOnly = true;
            this.textBoxABC.Size = new System.Drawing.Size(39, 20);
            this.textBoxABC.TabIndex = 129;
            this.textBoxABC.Visible = false;
            // 
            // textBoxNTBN
            // 
            this.textBoxNTBN.Location = new System.Drawing.Point(1171, 463);
            this.textBoxNTBN.Name = "textBoxNTBN";
            this.textBoxNTBN.ReadOnly = true;
            this.textBoxNTBN.Size = new System.Drawing.Size(39, 20);
            this.textBoxNTBN.TabIndex = 131;
            this.textBoxNTBN.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1125, 467);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 130;
            this.label12.Text = "ΕΘΝΙΚΗ: ";
            this.label12.Visible = false;
            // 
            // textBoxALPHA
            // 
            this.textBoxALPHA.Location = new System.Drawing.Point(1171, 491);
            this.textBoxALPHA.Name = "textBoxALPHA";
            this.textBoxALPHA.ReadOnly = true;
            this.textBoxALPHA.Size = new System.Drawing.Size(39, 20);
            this.textBoxALPHA.TabIndex = 133;
            this.textBoxALPHA.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1129, 494);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 132;
            this.label13.Text = "ΑΛΦΑ: ";
            this.label13.Visible = false;
            // 
            // textBoxEBNK
            // 
            this.textBoxEBNK.Location = new System.Drawing.Point(1171, 517);
            this.textBoxEBNK.Name = "textBoxEBNK";
            this.textBoxEBNK.ReadOnly = true;
            this.textBoxEBNK.Size = new System.Drawing.Size(39, 20);
            this.textBoxEBNK.TabIndex = 135;
            this.textBoxEBNK.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1098, 519);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 134;
            this.label14.Text = "EUROBANK: ";
            this.label14.Visible = false;
            // 
            // listView_MIDs
            // 
            this.listView_MIDs.GridLines = true;
            this.listView_MIDs.Location = new System.Drawing.Point(11, 61);
            this.listView_MIDs.Name = "listView_MIDs";
            this.listView_MIDs.Size = new System.Drawing.Size(458, 119);
            this.listView_MIDs.TabIndex = 136;
            this.listView_MIDs.UseCompatibleStateImageBehavior = false;
            this.listView_MIDs.View = System.Windows.Forms.View.Details;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(11, 186);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(224, 24);
            this.button10.TabIndex = 137;
            this.button10.Text = "Clear Selections";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // listView_BINChk
            // 
            this.listView_BINChk.Location = new System.Drawing.Point(1045, 249);
            this.listView_BINChk.Name = "listView_BINChk";
            this.listView_BINChk.Size = new System.Drawing.Size(171, 139);
            this.listView_BINChk.TabIndex = 138;
            this.listView_BINChk.UseCompatibleStateImageBehavior = false;
            this.listView_BINChk.View = System.Windows.Forms.View.Details;
            this.listView_BINChk.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1120, 545);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 13);
            this.label15.TabIndex = 139;
            this.label15.Text = "ATTIKA: ";
            this.label15.Visible = false;
            // 
            // textBoxATTIKA
            // 
            this.textBoxATTIKA.Location = new System.Drawing.Point(1171, 543);
            this.textBoxATTIKA.Name = "textBoxATTIKA";
            this.textBoxATTIKA.ReadOnly = true;
            this.textBoxATTIKA.Size = new System.Drawing.Size(39, 20);
            this.textBoxATTIKA.TabIndex = 140;
            this.textBoxATTIKA.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(249, 21);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(265, 13);
            this.label16.TabIndex = 141;
            this.label16.Text = "Or Add Mult MIDs in the TextBox Below and press Add";
            // 
            // textBox_MultMid
            // 
            this.textBox_MultMid.Location = new System.Drawing.Point(245, 38);
            this.textBox_MultMid.Name = "textBox_MultMid";
            this.textBox_MultMid.Size = new System.Drawing.Size(224, 20);
            this.textBox_MultMid.TabIndex = 142;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(89, 13);
            this.label17.TabIndex = 143;
            this.label17.Text = "Pick MID from list";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(476, 37);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(43, 23);
            this.button12.TabIndex = 144;
            this.button12.Text = "Add";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(34, 293);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(756, 487);
            this.tabControl1.TabIndex = 145;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(748, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bin Ruling";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(748, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Load Balance";
            // 
            // textBox_hiddenCpy
            // 
            this.textBox_hiddenCpy.Location = new System.Drawing.Point(987, 15);
            this.textBox_hiddenCpy.Name = "textBox_hiddenCpy";
            this.textBox_hiddenCpy.Size = new System.Drawing.Size(100, 20);
            this.textBox_hiddenCpy.TabIndex = 146;
            this.textBox_hiddenCpy.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(34, 18);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(542, 258);
            this.tabControl2.TabIndex = 147;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(534, 232);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "MID";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.textBox_MultMid);
            this.groupBox3.Controls.Add(this.comboGroup);
            this.groupBox3.Controls.Add(this.button12);
            this.groupBox3.Controls.Add(this.listView_MIDs);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Location = new System.Drawing.Point(6, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(524, 223);
            this.groupBox3.TabIndex = 145;
            this.groupBox3.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(534, 232);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "TID";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView_TIDs);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.button14);
            this.groupBox4.Controls.Add(this.textBox_MultTid);
            this.groupBox4.Controls.Add(this.button13);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(522, 220);
            this.groupBox4.TabIndex = 148;
            this.groupBox4.TabStop = false;
            // 
            // listView_TIDs
            // 
            this.listView_TIDs.GridLines = true;
            this.listView_TIDs.Location = new System.Drawing.Point(10, 61);
            this.listView_TIDs.Name = "listView_TIDs";
            this.listView_TIDs.Size = new System.Drawing.Size(355, 107);
            this.listView_TIDs.TabIndex = 137;
            this.listView_TIDs.UseCompatibleStateImageBehavior = false;
            this.listView_TIDs.View = System.Windows.Forms.View.Details;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Paste copied TIDs";
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(233, 32);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(133, 23);
            this.button14.TabIndex = 3;
            this.button14.Text = "Add TID";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // textBox_MultTid
            // 
            this.textBox_MultTid.Location = new System.Drawing.Point(10, 34);
            this.textBox_MultTid.Name = "textBox_MultTid";
            this.textBox_MultTid.Size = new System.Drawing.Size(217, 20);
            this.textBox_MultTid.TabIndex = 0;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(9, 176);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(105, 23);
            this.button13.TabIndex = 2;
            this.button13.Text = "Clear Selections";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // tidDescr
            // 
            this.tidDescr.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.tidDescr.Location = new System.Drawing.Point(1002, 40);
            this.tidDescr.Name = "tidDescr";
            this.tidDescr.Size = new System.Drawing.Size(103, 20);
            this.tidDescr.TabIndex = 148;
            this.tidDescr.Visible = false;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(1052, 569);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(158, 25);
            this.button15.TabIndex = 149;
            this.button15.Text = "Hidden for Tids Verification";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Visible = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(1135, 634);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 41);
            this.button17.TabIndex = 150;
            this.button17.Text = "Progreess Test";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Visible = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(1052, 605);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(158, 23);
            this.button18.TabIndex = 151;
            this.button18.Text = "Hidden for Tids Upload";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Visible = false;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(39, 785);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 20);
            this.label8.TabIndex = 152;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(1052, 688);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(163, 23);
            this.button19.TabIndex = 153;
            this.button19.Text = "Hidden for TidsLB";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Visible = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1256, 817);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.tidDescr);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.pbar1);
            this.Controls.Add(this.textBox_hiddenCpy);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFr);
            this.Controls.Add(this.textBoxATTIKA);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.listView_BINChk);
            this.Controls.Add(this.comboBoxPrcntge);
            this.Controls.Add(this.textBoxEBNK);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBoxALPHA);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxNTBN);
            this.Controls.Add(this.textBoxHidden_Route);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBoxABC);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.midDescr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "Bin Ruling / Load Balancing ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboGroup;
        private System.Windows.Forms.TextBox midDescr;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ListBox listBoxOrigin;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox comboBnkRoute;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxHidden_Route;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox_InstalFr;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_InstalTo;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBoxFr;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxPrcntge;
        private System.Windows.Forms.ComboBox comboBoxLD;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxABC;
        private System.Windows.Forms.TextBox textBoxNTBN;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxALPHA;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxEBNK;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListView listView_MIDs;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox textBox_NEWBIN;
        private System.Windows.Forms.ListView listView_BINChk;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxATTIKA;
        private System.Windows.Forms.TextBox textBox_lbNbr;
        private System.Windows.Forms.RadioButton radioButton_Amnt;
        private System.Windows.Forms.RadioButton radioButton_Cnt;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_MultMid;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RadioButton radioButton_UpdBIN;
        private System.Windows.Forms.RadioButton radioButton_AddBin;
        private System.Windows.Forms.TextBox textBox_hiddenCpy;
        private System.Windows.Forms.TextBox textBox_CurrMaxInst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox__CurrMinInst;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_CurrRoute;
        private System.Windows.Forms.ProgressBar pbar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView_TIDs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox textBox_MultTid;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.TextBox tidDescr;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxAmntTo;
        private System.Windows.Forms.TextBox textBoxAmntFr;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxlb_other;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox textBoxlb_eu;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBoxlb_gr;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.RadioButton radioButton_Pri;
    }
}

