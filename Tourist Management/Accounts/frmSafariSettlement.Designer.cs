﻿namespace Tourist_Management.Accounts
{
    partial class frmSafariSettlement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSafariSettlement));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.drpSafariCompany = new Tourist_Management.DropDowns.DropSearch();
            this.txtChkNo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkConfirmAll = new System.Windows.Forms.CheckBox();
            this.chkTodayPay = new System.Windows.Forms.CheckBox();
            this.chkAllPaid = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpPaidDate = new System.Windows.Forms.DateTimePicker();
            this.gbDateRange = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.grdSafariPay = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCompany = new  Tourist_Management.User_Controls.ComboBox();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDefChkNo = new System.Windows.Forms.Button();
            this.chkCmpny = new System.Windows.Forms.CheckBox();
            this.grpPayMethod = new System.Windows.Forms.GroupBox();
            this.rdbCash = new System.Windows.Forms.RadioButton();
            this.rdbBank = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbDateRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSafariPay)).BeginInit();
            this.grpPayMethod.SuspendLayout();
            this.SuspendLayout();
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(1007, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Enabled = false;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(931, 415);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 30);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.drpSafariCompany.BackColor = System.Drawing.Color.Transparent;
            this.drpSafariCompany.DataSource = null;
            this.drpSafariCompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpSafariCompany.Location = new System.Drawing.Point(157, 80);
            this.drpSafariCompany.Name = "drpSafariCompany";
            this.drpSafariCompany.SelectedText = "";
            this.drpSafariCompany.SelectedValue = "";
            this.drpSafariCompany.Size = new System.Drawing.Size(208, 21);
            this.drpSafariCompany.TabIndex = 413;
            this.drpSafariCompany.Selected_TextChanged += new System.EventHandler(this.drpSafariCompany_Selected_TextChanged);
            this.txtChkNo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtChkNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChkNo.Location = new System.Drawing.Point(157, 109);
            this.txtChkNo.Name = "txtChkNo";
            this.txtChkNo.Size = new System.Drawing.Size(117, 21);
            this.txtChkNo.TabIndex = 415;
            this.txtChkNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(53, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 414;
            this.label10.Text = "Cheque No";
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.chkConfirmAll);
            this.groupBox4.Controls.Add(this.chkTodayPay);
            this.groupBox4.Controls.Add(this.chkAllPaid);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(721, 102);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(361, 33);
            this.groupBox4.TabIndex = 410;
            this.groupBox4.TabStop = false;
            this.chkConfirmAll.AutoSize = true;
            this.chkConfirmAll.BackColor = System.Drawing.Color.Transparent;
            this.chkConfirmAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConfirmAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkConfirmAll.Location = new System.Drawing.Point(216, 13);
            this.chkConfirmAll.Name = "chkConfirmAll";
            this.chkConfirmAll.Size = new System.Drawing.Size(140, 17);
            this.chkConfirmAll.TabIndex = 346;
            this.chkConfirmAll.Text = "Mark All As Confirm";
            this.chkConfirmAll.UseVisualStyleBackColor = false;
            this.chkConfirmAll.CheckedChanged += new System.EventHandler(this.chkConfirmAll_CheckedChanged);
            this.chkTodayPay.AutoSize = true;
            this.chkTodayPay.BackColor = System.Drawing.Color.Transparent;
            this.chkTodayPay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTodayPay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTodayPay.Location = new System.Drawing.Point(6, 13);
            this.chkTodayPay.Name = "chkTodayPay";
            this.chkTodayPay.Size = new System.Drawing.Size(80, 17);
            this.chkTodayPay.TabIndex = 345;
            this.chkTodayPay.Text = "Paid Only";
            this.chkTodayPay.UseVisualStyleBackColor = false;
            this.chkTodayPay.CheckedChanged += new System.EventHandler(this.chkTodayPay_CheckedChanged);
            this.chkAllPaid.AutoSize = true;
            this.chkAllPaid.BackColor = System.Drawing.Color.Transparent;
            this.chkAllPaid.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllPaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkAllPaid.Location = new System.Drawing.Point(92, 13);
            this.chkAllPaid.Name = "chkAllPaid";
            this.chkAllPaid.Size = new System.Drawing.Size(118, 17);
            this.chkAllPaid.TabIndex = 344;
            this.chkAllPaid.Text = "Mark All As Paid";
            this.chkAllPaid.UseVisualStyleBackColor = false;
            this.chkAllPaid.CheckedChanged += new System.EventHandler(this.chkAllPaid_CheckedChanged);
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label12.Location = new System.Drawing.Point(407, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 13);
            this.label12.TabIndex = 407;
            this.label12.Text = "FILTER BY DATE";
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label23.Location = new System.Drawing.Point(3, 2);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(229, 34);
            this.label23.TabIndex = 405;
            this.label23.Text = "SAFARI PAYMENTS";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpPaidDate);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(721, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 48);
            this.groupBox2.TabIndex = 408;
            this.groupBox2.TabStop = false;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 346;
            this.label2.Text = "Paid";
            this.dtpPaidDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaidDate.Location = new System.Drawing.Point(44, 14);
            this.dtpPaidDate.Name = "dtpPaidDate";
            this.dtpPaidDate.Size = new System.Drawing.Size(212, 21);
            this.dtpPaidDate.TabIndex = 347;
            this.dtpPaidDate.ValueChanged += new System.EventHandler(this.dtpPaidDate_ValueChanged);
            this.gbDateRange.BackColor = System.Drawing.Color.Transparent;
            this.gbDateRange.Controls.Add(this.dtpToDate);
            this.gbDateRange.Controls.Add(this.label1);
            this.gbDateRange.Controls.Add(this.dtpFromDate);
            this.gbDateRange.Controls.Add(this.label9);
            this.gbDateRange.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDateRange.Location = new System.Drawing.Point(410, 53);
            this.gbDateRange.Name = "gbDateRange";
            this.gbDateRange.Size = new System.Drawing.Size(304, 81);
            this.gbDateRange.TabIndex = 404;
            this.gbDateRange.TabStop = false;
            this.dtpToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Location = new System.Drawing.Point(80, 51);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(212, 21);
            this.dtpToDate.TabIndex = 249;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 248;
            this.label1.Text = "Date To";
            this.dtpFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Location = new System.Drawing.Point(80, 20);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(212, 21);
            this.dtpFromDate.TabIndex = 247;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 246;
            this.label9.Text = "Date From";
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(721, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 13);
            this.label4.TabIndex = 417;
            this.label4.Text = "SET PAID/CONFIRM DATES";
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 418;
            this.label3.Text = "Safari Company";
            this.groupBox9.BackColor = System.Drawing.Color.Blue;
            this.groupBox9.Location = new System.Drawing.Point(231, 26);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(721, 1);
            this.groupBox9.TabIndex = 419;
            this.groupBox9.TabStop = false;
            this.grdSafariPay.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdSafariPay.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.grdSafariPay.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSafariPay.Location = new System.Drawing.Point(12, 149);
            this.grdSafariPay.Name = "grdSafariPay";
            this.grdSafariPay.Size = new System.Drawing.Size(1067, 260);
            this.grdSafariPay.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdSafariPay.Styles"));
            this.grdSafariPay.TabIndex = 420;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(53, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 421;
            this.label5.Text = "Company";
            this.cmbCompany.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompany.Enabled = false;
            this.cmbCompany.FormattingEnabled = true;
            this.cmbCompany.Location = new System.Drawing.Point(157, 53);
            this.cmbCompany.Name = "cmbCompany";
            this.cmbCompany.Size = new System.Drawing.Size(204, 21);
            this.cmbCompany.TabIndex = 422;
            this.cmbCompany.SelectedValueChanged += new System.EventHandler(this.cmbCompany_SelectedValueChanged);
            this.chkPrint.AutoSize = true;
            this.chkPrint.BackColor = System.Drawing.Color.Transparent;
            this.chkPrint.Checked = true;
            this.chkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrint.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrint.Location = new System.Drawing.Point(702, 422);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(136, 17);
            this.chkPrint.TabIndex = 424;
            this.chkPrint.Text = "Preview After Save";
            this.chkPrint.UseVisualStyleBackColor = false;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Enabled = false;
            this.btnPrint.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(850, 415);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(79, 31);
            this.btnPrint.TabIndex = 423;
            this.btnPrint.Text = "&Preview";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnDefChkNo.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefChkNo.Location = new System.Drawing.Point(280, 108);
            this.btnDefChkNo.Name = "btnDefChkNo";
            this.btnDefChkNo.Size = new System.Drawing.Size(83, 22);
            this.btnDefChkNo.TabIndex = 425;
            this.btnDefChkNo.Text = "Add Default Cheque No";
            this.btnDefChkNo.UseVisualStyleBackColor = true;
            this.btnDefChkNo.Click += new System.EventHandler(this.btnDefChkNo_Click);
            this.chkCmpny.AutoSize = true;
            this.chkCmpny.Location = new System.Drawing.Point(367, 55);
            this.chkCmpny.Name = "chkCmpny";
            this.chkCmpny.Size = new System.Drawing.Size(15, 14);
            this.chkCmpny.TabIndex = 426;
            this.chkCmpny.UseVisualStyleBackColor = true;
            this.chkCmpny.CheckedChanged += new System.EventHandler(this.chkCmpny_CheckedChanged);
            this.grpPayMethod.BackColor = System.Drawing.Color.Transparent;
            this.grpPayMethod.Controls.Add(this.rdbCash);
            this.grpPayMethod.Controls.Add(this.rdbBank);
            this.grpPayMethod.Location = new System.Drawing.Point(985, 53);
            this.grpPayMethod.Name = "grpPayMethod";
            this.grpPayMethod.Size = new System.Drawing.Size(96, 51);
            this.grpPayMethod.TabIndex = 427;
            this.grpPayMethod.TabStop = false;
            this.grpPayMethod.Text = "Pay Method";
            this.rdbCash.AutoSize = true;
            this.rdbCash.Location = new System.Drawing.Point(6, 30);
            this.rdbCash.Name = "rdbCash";
            this.rdbCash.Size = new System.Drawing.Size(49, 17);
            this.rdbCash.TabIndex = 1;
            this.rdbCash.TabStop = true;
            this.rdbCash.Text = "Cash";
            this.rdbCash.UseVisualStyleBackColor = true;
            this.rdbBank.AutoSize = true;
            this.rdbBank.Location = new System.Drawing.Point(5, 13);
            this.rdbBank.Name = "rdbBank";
            this.rdbBank.Size = new System.Drawing.Size(50, 17);
            this.rdbBank.TabIndex = 0;
            this.rdbBank.TabStop = true;
            this.rdbBank.Text = "Bank";
            this.rdbBank.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1090, 452);
            this.ControlBox = false;
            this.Controls.Add(this.grpPayMethod);
            this.Controls.Add(this.drpSafariCompany);
            this.Controls.Add(this.chkCmpny);
            this.Controls.Add(this.btnDefChkNo);
            this.Controls.Add(this.chkPrint);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cmbCompany);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grdSafariPay);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtChkNo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.gbDateRange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmSafariSettlement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSafariSettlement_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbDateRange.ResumeLayout(false);
            this.gbDateRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSafariPay)).EndInit();
            this.grpPayMethod.ResumeLayout(false);
            this.grpPayMethod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private DropDowns.DropSearch drpSafariCompany;
        private System.Windows.Forms.TextBox txtChkNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkTodayPay;
        private System.Windows.Forms.CheckBox chkAllPaid;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpPaidDate;
        private System.Windows.Forms.GroupBox gbDateRange;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox9;
        private C1.Win.C1FlexGrid.C1FlexGrid grdSafariPay;
        private System.Windows.Forms.Label label5;
        private  Tourist_Management.User_Controls.ComboBox cmbCompany;
        private System.Windows.Forms.CheckBox chkPrint;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDefChkNo;
        private System.Windows.Forms.CheckBox chkConfirmAll;
        private System.Windows.Forms.CheckBox chkCmpny;
        private System.Windows.Forms.GroupBox grpPayMethod;
        private System.Windows.Forms.RadioButton rdbCash;
        private System.Windows.Forms.RadioButton rdbBank;
    }
}