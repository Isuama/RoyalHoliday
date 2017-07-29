﻿namespace Tourist_Management.Account_Reports
{
    partial class frmDebtorAgents
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebtorAgents));
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.drpHandled = new Tourist_Management.DropDowns.DropSearch();
            this.chkAllHandled = new System.Windows.Forms.CheckBox();
            this.label40 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label39 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label33 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label37 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.drpAgent = new Tourist_Management.DropDowns.DropSearch();
            this.label30 = new System.Windows.Forms.Label();
            this.chkAllAgent = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbTW = new System.Windows.Forms.RadioButton();
            this.rdbAW = new System.Windows.Forms.RadioButton();
            this.grdDAgent = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbSupple = new System.Windows.Forms.RadioButton();
            this.rdbInvoice = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnDirect = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblUnIFrom = new System.Windows.Forms.Label();
            this.dtpUnIFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.rdb_A_All = new System.Windows.Forms.RadioButton();
            this.rdb_A_Invoiced = new System.Windows.Forms.RadioButton();
            this.groupBox10.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDAgent)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            this.groupBox10.BackColor = System.Drawing.Color.Transparent;
            this.groupBox10.Controls.Add(this.drpHandled);
            this.groupBox10.Controls.Add(this.chkAllHandled);
            this.groupBox10.Controls.Add(this.label40);
            this.groupBox10.Controls.Add(this.groupBox12);
            this.groupBox10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox10.Location = new System.Drawing.Point(600, 32);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(164, 53);
            this.groupBox10.TabIndex = 355;
            this.groupBox10.TabStop = false;
            this.drpHandled.BackColor = System.Drawing.Color.Transparent;
            this.drpHandled.DataSource = null;
            this.drpHandled.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpHandled.Location = new System.Drawing.Point(12, 22);
            this.drpHandled.Name = "drpHandled";
            this.drpHandled.SelectedText = "";
            this.drpHandled.SelectedValue = "";
            this.drpHandled.Size = new System.Drawing.Size(145, 21);
            this.drpHandled.TabIndex = 352;
            this.drpHandled.Selected_TextChanged += new System.EventHandler(this.drpHandled_Selected_TextChanged);
            this.chkAllHandled.AutoSize = true;
            this.chkAllHandled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chkAllHandled.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllHandled.Location = new System.Drawing.Point(88, 1);
            this.chkAllHandled.Name = "chkAllHandled";
            this.chkAllHandled.Size = new System.Drawing.Size(15, 14);
            this.chkAllHandled.TabIndex = 355;
            this.chkAllHandled.UseVisualStyleBackColor = false;
            this.chkAllHandled.CheckedChanged += new System.EventHandler(this.chkAllHandled_CheckedChanged);
            this.label40.AutoSize = true;
            this.label40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label40.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label40.Location = new System.Drawing.Point(0, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(87, 13);
            this.label40.TabIndex = 353;
            this.label40.Text = "BY HANDLED";
            this.groupBox12.BackColor = System.Drawing.Color.Gray;
            this.groupBox12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.Location = new System.Drawing.Point(98, 7);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(120, 2);
            this.groupBox12.TabIndex = 352;
            this.groupBox12.TabStop = false;
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label39.Location = new System.Drawing.Point(14, 28);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(110, 13);
            this.label39.TabIndex = 353;
            this.label39.Text = "FILTER BY DATE";
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.dtpToDate);
            this.groupBox7.Controls.Add(this.label33);
            this.groupBox7.Controls.Add(this.dtpFromDate);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(12, 33);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(354, 51);
            this.groupBox7.TabIndex = 352;
            this.groupBox7.TabStop = false;
            this.groupBox3.BackColor = System.Drawing.Color.Gray;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(115, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(236, 2);
            this.groupBox3.TabIndex = 353;
            this.groupBox3.TabStop = false;
            this.dtpToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(252, 18);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(95, 21);
            this.dtpToDate.TabIndex = 249;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(194, 21);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(52, 13);
            this.label33.TabIndex = 248;
            this.label33.Text = "Date To";
            this.dtpFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(87, 18);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(96, 21);
            this.dtpFromDate.TabIndex = 247;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(14, 21);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(67, 13);
            this.label37.TabIndex = 246;
            this.label37.Text = "Date From";
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.drpAgent);
            this.groupBox9.Controls.Add(this.label30);
            this.groupBox9.Controls.Add(this.chkAllAgent);
            this.groupBox9.Controls.Add(this.groupBox11);
            this.groupBox9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.Location = new System.Drawing.Point(372, 33);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(224, 53);
            this.groupBox9.TabIndex = 354;
            this.groupBox9.TabStop = false;
            this.drpAgent.BackColor = System.Drawing.Color.Transparent;
            this.drpAgent.DataSource = null;
            this.drpAgent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpAgent.Location = new System.Drawing.Point(12, 21);
            this.drpAgent.Name = "drpAgent";
            this.drpAgent.SelectedText = "";
            this.drpAgent.SelectedValue = "";
            this.drpAgent.Size = new System.Drawing.Size(201, 21);
            this.drpAgent.TabIndex = 349;
            this.drpAgent.Selected_TextChanged += new System.EventHandler(this.drpAgent_Selected_TextChanged);
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label30.Location = new System.Drawing.Point(0, 1);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(70, 13);
            this.label30.TabIndex = 377;
            this.label30.Text = "BY AGENT";
            this.chkAllAgent.AutoSize = true;
            this.chkAllAgent.BackColor = System.Drawing.Color.White;
            this.chkAllAgent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllAgent.Location = new System.Drawing.Point(73, 1);
            this.chkAllAgent.Name = "chkAllAgent";
            this.chkAllAgent.Size = new System.Drawing.Size(15, 14);
            this.chkAllAgent.TabIndex = 354;
            this.chkAllAgent.UseVisualStyleBackColor = false;
            this.chkAllAgent.CheckedChanged += new System.EventHandler(this.chkAllAgent_CheckedChanged);
            this.groupBox11.BackColor = System.Drawing.Color.Gray;
            this.groupBox11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.Location = new System.Drawing.Point(87, 7);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(132, 2);
            this.groupBox11.TabIndex = 352;
            this.groupBox11.TabStop = false;
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rdbTW);
            this.groupBox2.Controls.Add(this.rdbAW);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(493, 519);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 49);
            this.groupBox2.TabIndex = 335;
            this.groupBox2.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(0, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 354;
            this.label1.Text = "Report Type";
            this.rdbTW.AutoSize = true;
            this.rdbTW.Checked = true;
            this.rdbTW.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbTW.Location = new System.Drawing.Point(166, 20);
            this.rdbTW.Name = "rdbTW";
            this.rdbTW.Size = new System.Drawing.Size(82, 17);
            this.rdbTW.TabIndex = 1;
            this.rdbTW.TabStop = true;
            this.rdbTW.Text = "Tour Wise";
            this.rdbTW.UseVisualStyleBackColor = true;
            this.rdbAW.AutoSize = true;
            this.rdbAW.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAW.Location = new System.Drawing.Point(8, 20);
            this.rdbAW.Name = "rdbAW";
            this.rdbAW.Size = new System.Drawing.Size(149, 17);
            this.rdbAW.TabIndex = 0;
            this.rdbAW.Text = "Handled  Person Wise";
            this.rdbAW.UseVisualStyleBackColor = true;
            this.grdDAgent.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdDAgent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdDAgent.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdDAgent.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdDAgent.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDAgent.Location = new System.Drawing.Point(12, 96);
            this.grdDAgent.Name = "grdDAgent";
            this.grdDAgent.Size = new System.Drawing.Size(971, 417);
            this.grdDAgent.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdDAgent.Styles"));
            this.grdDAgent.TabIndex = 332;
            this.grdDAgent.AfterSelChange += new C1.Win.C1FlexGrid.RangeEventHandler(this.grdDAgent_AfterSelChange);
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(747, 519);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 49);
            this.groupBox1.TabIndex = 333;
            this.groupBox1.TabStop = false;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(81, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(77, 31);
            this.btnOk.TabIndex = 346;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(158, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 31);
            this.btnCancel.TabIndex = 345;
            this.btnCancel.Text = "&Close";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(3, 13);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(77, 31);
            this.btnPrint.TabIndex = 344;
            this.btnPrint.Text = "&Preview";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.groupBox20.BackColor = System.Drawing.Color.Gray;
            this.groupBox20.Location = new System.Drawing.Point(105, 18);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(876, 2);
            this.groupBox20.TabIndex = 376;
            this.groupBox20.TabStop = false;
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Gray;
            this.label18.Location = new System.Drawing.Point(9, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(94, 14);
            this.label18.TabIndex = 375;
            this.label18.Text = "Debtor Agent";
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.rdbSupple);
            this.groupBox4.Controls.Add(this.rdbInvoice);
            this.groupBox4.Controls.Add(this.rdbAll);
            this.groupBox4.Location = new System.Drawing.Point(770, 31);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(142, 56);
            this.groupBox4.TabIndex = 377;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Type";
            this.rdbSupple.AutoSize = true;
            this.rdbSupple.Location = new System.Drawing.Point(7, 34);
            this.rdbSupple.Name = "rdbSupple";
            this.rdbSupple.Size = new System.Drawing.Size(95, 17);
            this.rdbSupple.TabIndex = 2;
            this.rdbSupple.Text = "Supplementary";
            this.rdbSupple.UseVisualStyleBackColor = true;
            this.rdbSupple.CheckedChanged += new System.EventHandler(this.rdbSupple_CheckedChanged);
            this.rdbInvoice.AutoSize = true;
            this.rdbInvoice.Location = new System.Drawing.Point(52, 15);
            this.rdbInvoice.Name = "rdbInvoice";
            this.rdbInvoice.Size = new System.Drawing.Size(60, 17);
            this.rdbInvoice.TabIndex = 1;
            this.rdbInvoice.Text = "Invoice";
            this.rdbInvoice.UseVisualStyleBackColor = true;
            this.rdbInvoice.CheckedChanged += new System.EventHandler(this.rdbInvoice_CheckedChanged);
            this.rdbAll.AutoSize = true;
            this.rdbAll.Checked = true;
            this.rdbAll.Location = new System.Drawing.Point(7, 15);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(36, 17);
            this.rdbAll.TabIndex = 0;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Image = global::Tourist_Management.Properties.Resources.filter;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(918, 58);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(64, 37);
            this.btnFilter.TabIndex = 383;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            this.btnDirect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDirect.Location = new System.Drawing.Point(917, 29);
            this.btnDirect.Name = "btnDirect";
            this.btnDirect.Size = new System.Drawing.Size(65, 23);
            this.btnDirect.TabIndex = 384;
            this.btnDirect.Text = "Directs";
            this.btnDirect.UseVisualStyleBackColor = false;
            this.btnDirect.Click += new System.EventHandler(this.btnDirect_Click);
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.lblUnIFrom);
            this.groupBox5.Controls.Add(this.dtpUnIFrom);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.rdb_A_All);
            this.groupBox5.Controls.Add(this.rdb_A_Invoiced);
            this.groupBox5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(67, 519);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(421, 49);
            this.groupBox5.TabIndex = 385;
            this.groupBox5.TabStop = false;
            this.lblUnIFrom.AutoSize = true;
            this.lblUnIFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnIFrom.Location = new System.Drawing.Point(208, 21);
            this.lblUnIFrom.Name = "lblUnIFrom";
            this.lblUnIFrom.Size = new System.Drawing.Size(108, 13);
            this.lblUnIFrom.TabIndex = 387;
            this.lblUnIFrom.Text = "Un Invoiced From";
            this.dtpUnIFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpUnIFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUnIFrom.Location = new System.Drawing.Point(320, 17);
            this.dtpUnIFrom.Name = "dtpUnIFrom";
            this.dtpUnIFrom.Size = new System.Drawing.Size(95, 21);
            this.dtpUnIFrom.TabIndex = 386;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(0, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 354;
            this.label2.Text = "Advance Options";
            this.rdb_A_All.AutoSize = true;
            this.rdb_A_All.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_A_All.Location = new System.Drawing.Point(118, 20);
            this.rdb_A_All.Name = "rdb_A_All";
            this.rdb_A_All.Size = new System.Drawing.Size(75, 17);
            this.rdb_A_All.TabIndex = 1;
            this.rdb_A_All.Text = "All Tours";
            this.rdb_A_All.UseVisualStyleBackColor = true;
            this.rdb_A_All.CheckedChanged += new System.EventHandler(this.rdb_A_All_CheckedChanged);
            this.rdb_A_Invoiced.AutoSize = true;
            this.rdb_A_Invoiced.Checked = true;
            this.rdb_A_Invoiced.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_A_Invoiced.Location = new System.Drawing.Point(8, 20);
            this.rdb_A_Invoiced.Name = "rdb_A_Invoiced";
            this.rdb_A_Invoiced.Size = new System.Drawing.Size(104, 17);
            this.rdb_A_Invoiced.TabIndex = 0;
            this.rdb_A_Invoiced.TabStop = true;
            this.rdb_A_Invoiced.Text = "Invoiced Only";
            this.rdb_A_Invoiced.UseVisualStyleBackColor = true;
            this.rdb_A_Invoiced.CheckedChanged += new System.EventHandler(this.rdb_A_Invoiced_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(994, 580);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnDirect);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grdDAgent);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox9);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmDebtorAgents";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmDebtorAgents_Load);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDAgent)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private C1.Win.C1FlexGrid.C1FlexGrid grdDAgent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbTW;
        private System.Windows.Forms.RadioButton rdbAW;
        private System.Windows.Forms.GroupBox groupBox10;
        private DropDowns.DropSearch drpHandled;
        private System.Windows.Forms.CheckBox chkAllHandled;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.GroupBox groupBox9;
        private DropDowns.DropSearch drpAgent;
        private System.Windows.Forms.CheckBox chkAllAgent;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbSupple;
        private System.Windows.Forms.RadioButton rdbInvoice;
        private System.Windows.Forms.Button btnDirect;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdb_A_All;
        private System.Windows.Forms.RadioButton rdb_A_Invoiced;
        private System.Windows.Forms.Label lblUnIFrom;
        private System.Windows.Forms.DateTimePicker dtpUnIFrom;
    }
}