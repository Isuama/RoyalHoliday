﻿namespace Tourist_Management.Accounts
{
    partial class frmCashManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashManage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.grdCash = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.ucFilterByDate1 = new Tourist_Management.User_Controls.ucFilterByDate();
            this.label1 = new System.Windows.Forms.Label();
            this.ucFilterByOther1 = new Tourist_Management.User_Controls.ucFilterByOther();
            this.btnFilter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblIncome = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblExpense = new System.Windows.Forms.Label();
            this.lblBalanceError = new System.Windows.Forms.Label();
            this.rdbOriginal = new System.Windows.Forms.RadioButton();
            this.rdbDate = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpIssuedDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCash)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(544, 540);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 49);
            this.groupBox1.TabIndex = 335;
            this.groupBox1.TabStop = false;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(89, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 30);
            this.btnOk.TabIndex = 406;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(164, 12);
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
            this.btnPrint.Location = new System.Drawing.Point(6, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(77, 31);
            this.btnPrint.TabIndex = 344;
            this.btnPrint.Text = "&Preview";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.grdCash.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdCash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdCash.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdCash.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdCash.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCash.Location = new System.Drawing.Point(8, 120);
            this.grdCash.Name = "grdCash";
            this.grdCash.Size = new System.Drawing.Size(790, 418);
            this.grdCash.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCash.Styles"));
            this.grdCash.TabIndex = 334;
            this.grdCash.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdCash_AfterEdit);
            this.ucFilterByDate1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByDate1.Location = new System.Drawing.Point(5, 3);
            this.ucFilterByDate1.Name = "ucFilterByDate1";
            this.ucFilterByDate1.Size = new System.Drawing.Size(453, 66);
            this.ucFilterByDate1.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(463, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 372;
            this.label1.Text = "Currency";
            this.ucFilterByOther1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByOther1.Location = new System.Drawing.Point(458, 11);
            this.ucFilterByOther1.Name = "ucFilterByOther1";
            this.ucFilterByOther1.Query = null;
            this.ucFilterByOther1.Size = new System.Drawing.Size(185, 50);
            this.ucFilterByOther1.TabIndex = 373;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Image = global::Tourist_Management.Properties.Resources.filter;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(649, 23);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(64, 37);
            this.btnFilter.TabIndex = 383;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(9, 545);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 384;
            this.label2.Text = "Total Income  ";
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(9, 561);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 385;
            this.label3.Text = "Total Expense";
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(9, 577);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 386;
            this.label4.Text = "Balance";
            this.lblIncome.AutoSize = true;
            this.lblIncome.BackColor = System.Drawing.Color.Transparent;
            this.lblIncome.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblIncome.Location = new System.Drawing.Point(112, 545);
            this.lblIncome.Name = "lblIncome";
            this.lblIncome.Size = new System.Drawing.Size(35, 13);
            this.lblIncome.TabIndex = 387;
            this.lblIncome.Text = "0.00";
            this.lblBalance.AutoSize = true;
            this.lblBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblBalance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblBalance.Location = new System.Drawing.Point(112, 577);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(35, 13);
            this.lblBalance.TabIndex = 388;
            this.lblBalance.Text = "0.00";
            this.lblExpense.AutoSize = true;
            this.lblExpense.BackColor = System.Drawing.Color.Transparent;
            this.lblExpense.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblExpense.Location = new System.Drawing.Point(112, 561);
            this.lblExpense.Name = "lblExpense";
            this.lblExpense.Size = new System.Drawing.Size(35, 13);
            this.lblExpense.TabIndex = 389;
            this.lblExpense.Text = "0.00";
            this.lblBalanceError.AutoSize = true;
            this.lblBalanceError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblBalanceError.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceError.ForeColor = System.Drawing.Color.Red;
            this.lblBalanceError.Location = new System.Drawing.Point(185, 559);
            this.lblBalanceError.Name = "lblBalanceError";
            this.lblBalanceError.Size = new System.Drawing.Size(296, 16);
            this.lblBalanceError.TabIndex = 390;
            this.lblBalanceError.Text = "Balance is less than zero. Please check.";
            this.lblBalanceError.Visible = false;
            this.rdbOriginal.AutoSize = true;
            this.rdbOriginal.BackColor = System.Drawing.Color.Transparent;
            this.rdbOriginal.Checked = true;
            this.rdbOriginal.Location = new System.Drawing.Point(725, 21);
            this.rdbOriginal.Name = "rdbOriginal";
            this.rdbOriginal.Size = new System.Drawing.Size(60, 17);
            this.rdbOriginal.TabIndex = 391;
            this.rdbOriginal.TabStop = true;
            this.rdbOriginal.Text = "Original";
            this.rdbOriginal.UseVisualStyleBackColor = false;
            this.rdbOriginal.Visible = false;
            this.rdbDate.AutoSize = true;
            this.rdbDate.BackColor = System.Drawing.Color.Transparent;
            this.rdbDate.Location = new System.Drawing.Point(725, 41);
            this.rdbDate.Name = "rdbDate";
            this.rdbDate.Size = new System.Drawing.Size(48, 17);
            this.rdbDate.TabIndex = 392;
            this.rdbDate.Text = "Date";
            this.rdbDate.UseVisualStyleBackColor = false;
            this.rdbDate.Visible = false;
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dtpIssuedDate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(8, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 49);
            this.groupBox2.TabIndex = 393;
            this.groupBox2.TabStop = false;
            this.dtpIssuedDate.Location = new System.Drawing.Point(78, 20);
            this.dtpIssuedDate.Name = "dtpIssuedDate";
            this.dtpIssuedDate.Size = new System.Drawing.Size(200, 20);
            this.dtpIssuedDate.TabIndex = 1;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Issued Date";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(805, 594);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.rdbDate);
            this.Controls.Add(this.rdbOriginal);
            this.Controls.Add(this.lblBalanceError);
            this.Controls.Add(this.lblExpense);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblIncome);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ucFilterByOther1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grdCash);
            this.Controls.Add(this.ucFilterByDate1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmCashManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmCashManage_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCash)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private Tourist_Management.User_Controls.ucFilterByDate ucFilterByDate1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        private C1.Win.C1FlexGrid.C1FlexGrid grdCash;
        private System.Windows.Forms.Label label1;
        private Tourist_Management.User_Controls.ucFilterByOther ucFilterByOther1;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblIncome;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblExpense;
        private System.Windows.Forms.Label lblBalanceError;
        private System.Windows.Forms.RadioButton rdbOriginal;
        private System.Windows.Forms.RadioButton rdbDate;
        public System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpIssuedDate;
    }
}