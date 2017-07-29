﻿namespace Tourist_Management.Accounts
{
    partial class frmJournalEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJournalEntry));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grdPay = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.gbBasic1 = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.drpPayableTo = new Tourist_Management.DropDowns.DropSearch();
            this.dtpPaidDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.lblPayRec = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCompany = new  Tourist_Management.User_Controls.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDifCredit = new System.Windows.Forms.Label();
            this.lblDifDebit = new System.Windows.Forms.Label();
            this.lblCredit = new System.Windows.Forms.Label();
            this.lblDebit = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rdbCancel = new System.Windows.Forms.RadioButton();
            this.lblGuest = new System.Windows.Forms.Label();
            this.btnTour = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTourNo = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPay)).BeginInit();
            this.gbBasic1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(525, 446);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 409;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(447, 446);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 30);
            this.btnOk.TabIndex = 408;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.grdPay.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdPay.BackColor = System.Drawing.Color.Transparent;
            this.grdPay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdPay.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdPay.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.grdPay.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPay.Location = new System.Drawing.Point(12, 156);
            this.grdPay.Name = "grdPay";
            this.grdPay.Size = new System.Drawing.Size(597, 229);
            this.grdPay.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdPay.Styles"));
            this.grdPay.TabIndex = 411;
            this.grdPay.RowColChange += new System.EventHandler(this.grdPay_RowColChange);
            this.grdPay.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdPay_AfterEdit);
            this.grdPay.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdPay_CellButtonClick);
            this.grdPay.Click += new System.EventHandler(this.grdPay_Click);
            this.grdPay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdPay_KeyDown);
            this.gbBasic1.BackColor = System.Drawing.Color.Transparent;
            this.gbBasic1.Controls.Add(this.txtDescription);
            this.gbBasic1.Controls.Add(this.label5);
            this.gbBasic1.Controls.Add(this.drpPayableTo);
            this.gbBasic1.Controls.Add(this.dtpPaidDate);
            this.gbBasic1.Controls.Add(this.label2);
            this.gbBasic1.Controls.Add(this.txtRefNo);
            this.gbBasic1.Controls.Add(this.lblPayRec);
            this.gbBasic1.Controls.Add(this.label1);
            this.gbBasic1.Controls.Add(this.cmbCompany);
            this.gbBasic1.Controls.Add(this.label6);
            this.gbBasic1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBasic1.Location = new System.Drawing.Point(12, 46);
            this.gbBasic1.Name = "gbBasic1";
            this.gbBasic1.Size = new System.Drawing.Size(597, 104);
            this.gbBasic1.TabIndex = 412;
            this.gbBasic1.TabStop = false;
            this.txtDescription.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(76, 61);
            this.txtDescription.MaxLength = 200;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(509, 37);
            this.txtDescription.TabIndex = 393;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 12);
            this.label5.TabIndex = 392;
            this.label5.Text = "Description";
            this.drpPayableTo.BackColor = System.Drawing.Color.Transparent;
            this.drpPayableTo.DataSource = null;
            this.drpPayableTo.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpPayableTo.Location = new System.Drawing.Point(77, 35);
            this.drpPayableTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.drpPayableTo.Name = "drpPayableTo";
            this.drpPayableTo.SelectedText = "";
            this.drpPayableTo.SelectedValue = "";
            this.drpPayableTo.Size = new System.Drawing.Size(179, 18);
            this.drpPayableTo.TabIndex = 382;
            this.dtpPaidDate.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaidDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaidDate.Location = new System.Drawing.Point(406, 14);
            this.dtpPaidDate.Name = "dtpPaidDate";
            this.dtpPaidDate.Size = new System.Drawing.Size(179, 18);
            this.dtpPaidDate.TabIndex = 383;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(338, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 387;
            this.label2.Text = "Date";
            this.txtRefNo.BackColor = System.Drawing.Color.OldLace;
            this.txtRefNo.Enabled = false;
            this.txtRefNo.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefNo.ForeColor = System.Drawing.Color.Blue;
            this.txtRefNo.Location = new System.Drawing.Point(406, 36);
            this.txtRefNo.MaxLength = 50;
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(179, 20);
            this.txtRefNo.TabIndex = 375;
            this.txtRefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblPayRec.BackColor = System.Drawing.Color.Transparent;
            this.lblPayRec.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayRec.Location = new System.Drawing.Point(9, 37);
            this.lblPayRec.Name = "lblPayRec";
            this.lblPayRec.Size = new System.Drawing.Size(61, 23);
            this.lblPayRec.TabIndex = 376;
            this.lblPayRec.Text = "Payable To";
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(338, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 386;
            this.label1.Text = "Ref No";
            this.cmbCompany.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompany.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCompany.FormattingEnabled = true;
            this.cmbCompany.Location = new System.Drawing.Point(77, 12);
            this.cmbCompany.Name = "cmbCompany";
            this.cmbCompany.Size = new System.Drawing.Size(179, 20);
            this.cmbCompany.TabIndex = 384;
            this.cmbCompany.SelectedIndexChanged += new System.EventHandler(this.cmbCompany_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 12);
            this.label6.TabIndex = 388;
            this.label6.Text = "Company";
            this.lblDifCredit.BackColor = System.Drawing.Color.Transparent;
            this.lblDifCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDifCredit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifCredit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblDifCredit.Location = new System.Drawing.Point(420, 416);
            this.lblDifCredit.Name = "lblDifCredit";
            this.lblDifCredit.Size = new System.Drawing.Size(85, 23);
            this.lblDifCredit.TabIndex = 426;
            this.lblDifCredit.Text = "Difference";
            this.lblDifCredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDifCredit.Visible = false;
            this.lblDifDebit.BackColor = System.Drawing.Color.Transparent;
            this.lblDifDebit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDifDebit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifDebit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblDifDebit.Location = new System.Drawing.Point(508, 416);
            this.lblDifDebit.Name = "lblDifDebit";
            this.lblDifDebit.Size = new System.Drawing.Size(85, 23);
            this.lblDifDebit.TabIndex = 425;
            this.lblDifDebit.Text = "Difference";
            this.lblDifDebit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDifDebit.Visible = false;
            this.lblCredit.BackColor = System.Drawing.Color.Transparent;
            this.lblCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCredit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredit.Location = new System.Drawing.Point(508, 391);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(85, 23);
            this.lblCredit.TabIndex = 424;
            this.lblCredit.Text = "Credit";
            this.lblCredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDebit.BackColor = System.Drawing.Color.Transparent;
            this.lblDebit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDebit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebit.Location = new System.Drawing.Point(420, 391);
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Size = new System.Drawing.Size(85, 23);
            this.lblDebit.TabIndex = 423;
            this.lblDebit.Text = "Debit";
            this.lblDebit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.rdbCancel);
            this.groupBox7.Controls.Add(this.lblGuest);
            this.groupBox7.Controls.Add(this.btnTour);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.txtTourNo);
            this.groupBox7.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(12, 2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(597, 42);
            this.groupBox7.TabIndex = 427;
            this.groupBox7.TabStop = false;
            this.rdbCancel.AutoSize = true;
            this.rdbCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCancel.ForeColor = System.Drawing.Color.Red;
            this.rdbCancel.Location = new System.Drawing.Point(484, 14);
            this.rdbCancel.Name = "rdbCancel";
            this.rdbCancel.Size = new System.Drawing.Size(107, 17);
            this.rdbCancel.TabIndex = 394;
            this.rdbCancel.TabStop = true;
            this.rdbCancel.Text = "Cancel Entry";
            this.rdbCancel.UseVisualStyleBackColor = true;
            this.lblGuest.AutoSize = true;
            this.lblGuest.BackColor = System.Drawing.Color.Transparent;
            this.lblGuest.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblGuest.Location = new System.Drawing.Point(275, 17);
            this.lblGuest.Name = "lblGuest";
            this.lblGuest.Size = new System.Drawing.Size(15, 12);
            this.lblGuest.TabIndex = 402;
            this.lblGuest.Text = "\"\"";
            this.btnTour.Location = new System.Drawing.Point(235, 12);
            this.btnTour.Name = "btnTour";
            this.btnTour.Size = new System.Drawing.Size(34, 23);
            this.btnTour.TabIndex = 399;
            this.btnTour.Text = "...";
            this.btnTour.UseVisualStyleBackColor = true;
            this.btnTour.Click += new System.EventHandler(this.btnTour_Click);
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 12);
            this.label9.TabIndex = 401;
            this.label9.Text = "Tour ID";
            this.txtTourNo.Enabled = false;
            this.txtTourNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTourNo.ForeColor = System.Drawing.Color.Blue;
            this.txtTourNo.Location = new System.Drawing.Point(90, 13);
            this.txtTourNo.Name = "txtTourNo";
            this.txtTourNo.ReadOnly = true;
            this.txtTourNo.Size = new System.Drawing.Size(139, 21);
            this.txtTourNo.TabIndex = 400;
            this.txtTourNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(16, 454);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(22, 16);
            this.lblStatus.TabIndex = 428;
            this.lblStatus.Text = "\"\"";
            this.lblStatus.Visible = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(613, 479);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.lblDifCredit);
            this.Controls.Add(this.lblDifDebit);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.lblDebit);
            this.Controls.Add(this.gbBasic1);
            this.Controls.Add(this.grdPay);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmJournalEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmJournalEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPay)).EndInit();
            this.gbBasic1.ResumeLayout(false);
            this.gbBasic1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnOk;
        public C1.Win.C1FlexGrid.C1FlexGrid grdPay;
        public System.Windows.Forms.GroupBox gbBasic1;
        public DropDowns.DropSearch drpPayableTo;
        public System.Windows.Forms.DateTimePicker dtpPaidDate;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtRefNo;
        public System.Windows.Forms.Label lblPayRec;
        public System.Windows.Forms.Label label1;
        public  Tourist_Management.User_Controls.ComboBox cmbCompany;
        public System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDifCredit;
        private System.Windows.Forms.Label lblDifDebit;
        private System.Windows.Forms.Label lblCredit;
        private System.Windows.Forms.Label lblDebit;
        public System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.Label lblGuest;
        public System.Windows.Forms.Button btnTour;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox txtTourNo;
        public System.Windows.Forms.TextBox txtDescription;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.RadioButton rdbCancel;
        public System.Windows.Forms.Label lblStatus;
    }
}