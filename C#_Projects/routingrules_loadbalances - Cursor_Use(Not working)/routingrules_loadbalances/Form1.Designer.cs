﻿namespace routingrules_loadbalances
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
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboGroup = new System.Windows.Forms.ComboBox();
            this.midDescr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button9 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.comboBoxPrcntge = new System.Windows.Forms.ComboBox();
            this.comboBoxLD = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(60, 92);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Insert records to Backup";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "1.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "2.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "3.";
            // 
            // comboGroup
            // 
            this.comboGroup.FormattingEnabled = true;
            this.comboGroup.Location = new System.Drawing.Point(59, 53);
            this.comboGroup.Name = "comboGroup";
            this.comboGroup.Size = new System.Drawing.Size(154, 21);
            this.comboGroup.TabIndex = 5;
            this.comboGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // midDescr
            // 
            this.midDescr.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.midDescr.Location = new System.Drawing.Point(237, 54);
            this.midDescr.Name = "midDescr";
            this.midDescr.Size = new System.Drawing.Size(173, 20);
            this.midDescr.TabIndex = 7;
            this.midDescr.TextChanged += new System.EventHandler(this.midDescr_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "4.";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(59, 141);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Delete records for selected MID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(235, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "FOR PROCESS";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(453, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "TEST";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button4.Location = new System.Drawing.Point(418, 37);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 37);
            this.button4.TabIndex = 92;
            this.button4.Text = "Add selection";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(24, 224);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(16, 13);
            this.label39.TabIndex = 104;
            this.label39.Text = "5.";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(59, 183);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(155, 23);
            this.button16.TabIndex = 105;
            this.button16.Text = "Check TIDs";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(234, 188);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(169, 13);
            this.label40.TabIndex = 106;
            this.label40.Text = "Check if all TIDs exist for all banks";
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
            this.label41.Size = new System.Drawing.Size(145, 13);
            this.label41.TabIndex = 108;
            this.label41.Text = "Επιλογή κάρτας/Συναλλαγής";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(261, 22);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(105, 13);
            this.label23.TabIndex = 109;
            this.label23.Text = "Τράπεζα προορισμού";
            // 
            // comboBnkRoute
            // 
            this.comboBnkRoute.FormattingEnabled = true;
            this.comboBnkRoute.Location = new System.Drawing.Point(264, 43);
            this.comboBnkRoute.Name = "comboBnkRoute";
            this.comboBnkRoute.Size = new System.Drawing.Size(121, 21);
            this.comboBnkRoute.TabIndex = 110;
            this.comboBnkRoute.SelectedIndexChanged += new System.EventHandler(this.comboBnkRoute_SelectedIndexChanged);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(264, 123);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(360, 212);
            this.listView1.TabIndex = 111;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(263, 97);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 13);
            this.label24.TabIndex = 112;
            this.label24.Text = "Προβολή συνδυασμού";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(549, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 37);
            this.button1.TabIndex = 113;
            this.button1.Text = "Clear selections";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBoxHidden_Route
            // 
            this.textBoxHidden_Route.Location = new System.Drawing.Point(266, 71);
            this.textBoxHidden_Route.Name = "textBoxHidden_Route";
            this.textBoxHidden_Route.Size = new System.Drawing.Size(118, 21);
            this.textBoxHidden_Route.TabIndex = 114;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(418, 86);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 115;
            this.button5.Text = "Proceed ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(264, 342);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 116;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(379, 344);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 117;
            // 
            // comboBox_InstalFr
            // 
            this.comboBox_InstalFr.FormattingEnabled = true;
            this.comboBox_InstalFr.Location = new System.Drawing.Point(17, 314);
            this.comboBox_InstalFr.Name = "comboBox_InstalFr";
            this.comboBox_InstalFr.Size = new System.Drawing.Size(39, 21);
            this.comboBox_InstalFr.TabIndex = 118;
            this.comboBox_InstalFr.SelectedIndexChanged += new System.EventHandler(this.comboBox_InstalFr_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 119;
            this.label6.Text = "Installments 1 - 99";
            // 
            // comboBox_InstalTo
            // 
            this.comboBox_InstalTo.FormattingEnabled = true;
            this.comboBox_InstalTo.Location = new System.Drawing.Point(81, 314);
            this.comboBox_InstalTo.Name = "comboBox_InstalTo";
            this.comboBox_InstalTo.Size = new System.Drawing.Size(39, 21);
            this.comboBox_InstalTo.TabIndex = 120;
            this.comboBox_InstalTo.SelectedIndexChanged += new System.EventHandler(this.comboBox_InstalTo_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(15, 298);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(31, 13);
            this.label25.TabIndex = 121;
            this.label25.Text = "From";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(78, 298);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(19, 13);
            this.label26.TabIndex = 122;
            this.label26.Text = "To";
            // 
            // textBoxFr
            // 
            this.textBoxFr.Location = new System.Drawing.Point(17, 342);
            this.textBoxFr.Name = "textBoxFr";
            this.textBoxFr.Size = new System.Drawing.Size(39, 21);
            this.textBoxFr.TabIndex = 123;
            this.textBoxFr.Visible = false;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(81, 342);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(39, 21);
            this.textBoxTo.TabIndex = 124;
            this.textBoxTo.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(549, 79);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 37);
            this.button6.TabIndex = 125;
            this.button6.Text = "Delete last combination";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.listBoxOrigin);
            this.groupBox1.Controls.Add(this.textBoxTo);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.textBoxFr);
            this.groupBox1.Controls.Add(this.comboBnkRoute);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.comboBox_InstalTo);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxHidden_Route);
            this.groupBox1.Controls.Add(this.comboBox_InstalFr);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(59, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 391);
            this.groupBox1.TabIndex = 126;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Κανόνες Δρομολόγησης";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.listView2);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.comboBoxPrcntge);
            this.groupBox2.Controls.Add(this.comboBoxLD);
            this.groupBox2.Location = new System.Drawing.Point(719, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(431, 268);
            this.groupBox2.TabIndex = 127;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Balance";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(311, 144);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 39);
            this.button9.TabIndex = 131;
            this.button9.Text = "Proceed";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(311, 225);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 130;
            this.checkBox1.Text = "Total by Amounts";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(155, 22);
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
            this.listView2.Location = new System.Drawing.Point(14, 97);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(277, 145);
            this.listView2.TabIndex = 126;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(311, 45);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 44);
            this.button8.TabIndex = 4;
            this.button8.Text = "Add selections";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click_1);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(311, 97);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 41);
            this.button7.TabIndex = 3;
            this.button7.Text = "Clear selections";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // comboBoxPrcntge
            // 
            this.comboBoxPrcntge.FormattingEnabled = true;
            this.comboBoxPrcntge.Location = new System.Drawing.Point(158, 45);
            this.comboBoxPrcntge.Name = "comboBoxPrcntge";
            this.comboBoxPrcntge.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPrcntge.TabIndex = 1;
            // 
            // comboBoxLD
            // 
            this.comboBoxLD.FormattingEnabled = true;
            this.comboBoxLD.Location = new System.Drawing.Point(14, 46);
            this.comboBoxLD.Name = "comboBoxLD";
            this.comboBoxLD.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLD.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1176, 739);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.midDescr);
            this.Controls.Add(this.comboGroup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboGroup;
        private System.Windows.Forms.TextBox midDescr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button9;
    }
}

