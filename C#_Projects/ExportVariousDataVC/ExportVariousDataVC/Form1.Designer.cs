namespace ExportVariousDataVC
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
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
            this.textBox_MultParm = new System.Windows.Forms.TextBox();
            this.listView_Parm = new System.Windows.Forms.ListView();
            this.Add_Btn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxClust = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_MultParm
            // 
            this.textBox_MultParm.Location = new System.Drawing.Point(26, 38);
            this.textBox_MultParm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_MultParm.Name = "textBox_MultParm";
            this.textBox_MultParm.Size = new System.Drawing.Size(365, 23);
            this.textBox_MultParm.TabIndex = 0;
            // 
            // listView_Parm
            // 
            this.listView_Parm.Location = new System.Drawing.Point(26, 107);
            this.listView_Parm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView_Parm.Name = "listView_Parm";
            this.listView_Parm.Size = new System.Drawing.Size(365, 356);
            this.listView_Parm.TabIndex = 1;
            this.listView_Parm.UseCompatibleStateImageBehavior = false;
            this.listView_Parm.View = System.Windows.Forms.View.Details;
            // 
            // Add_Btn
            // 
            this.Add_Btn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Add_Btn.Location = new System.Drawing.Point(411, 107);
            this.Add_Btn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Add_Btn.Name = "Add_Btn";
            this.Add_Btn.Size = new System.Drawing.Size(100, 28);
            this.Add_Btn.TabIndex = 2;
            this.Add_Btn.Text = "Add Param";
            this.Add_Btn.UseVisualStyleBackColor = false;
            this.Add_Btn.Click += new System.EventHandler(this.Add_Btn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.button1.Location = new System.Drawing.Point(411, 275);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 97);
            this.button1.TabIndex = 3;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.IndianRed;
            this.button2.Location = new System.Drawing.Point(411, 143);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "Delete Param";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Tan;
            this.button3.Location = new System.Drawing.Point(411, 179);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 47);
            this.button3.TabIndex = 5;
            this.button3.Text = "Clear  Param list";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Parameters Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Parameters List";
            // 
            // comboBoxClust
            // 
            this.comboBoxClust.FormattingEnabled = true;
            this.comboBoxClust.Location = new System.Drawing.Point(411, 38);
            this.comboBoxClust.Name = "comboBoxClust";
            this.comboBoxClust.Size = new System.Drawing.Size(121, 24);
            this.comboBoxClust.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Cluster";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 476);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxClust);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Add_Btn);
            this.Controls.Add(this.listView_Parm);
            this.Controls.Add(this.textBox_MultParm);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Export Parameter Data for VC TIDs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_MultParm;
        private System.Windows.Forms.ListView listView_Parm;
        private System.Windows.Forms.Button Add_Btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxClust;
        private System.Windows.Forms.Label label3;
    }
}

