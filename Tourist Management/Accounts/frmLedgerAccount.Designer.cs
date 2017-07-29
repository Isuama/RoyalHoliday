namespace Tourist_Management.Accounts
{
    partial class frmLedgerAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLedgerAccount));
            this.btnICancel = new System.Windows.Forms.Button();
            this.btnIPreview = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCheck = new System.Windows.Forms.CheckBox();
            this.grdAccount = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.ucFilterByDate1 = new Tourist_Management.User_Controls.ucFilterByDate();
            this.ucFilterByCompany1 = new Tourist_Management.User_Controls.ucFilterByCompany();
            this.ucFilterByOther1 = new Tourist_Management.User_Controls.ucFilterByOther();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbSummary = new System.Windows.Forms.RadioButton();
            this.rdbDetail = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbAccountType = new  Tourist_Management.User_Controls.ComboBox();
            this.cmbOp = new  Tourist_Management.User_Controls.ComboBox();
            this.cmbFld = new  Tourist_Management.User_Controls.ComboBox();
            this.bPrintTotals = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnICancel
            // 
            this.btnICancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnICancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnICancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnICancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnICancel.Location = new System.Drawing.Point(916, 485);
            this.btnICancel.Name = "btnICancel";
            this.btnICancel.Size = new System.Drawing.Size(72, 30);
            this.btnICancel.TabIndex = 366;
            this.btnICancel.Text = "&Cancel";
            this.btnICancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnICancel.UseVisualStyleBackColor = true;
            this.btnICancel.Click += new System.EventHandler(this.btnICancel_Click);
            // 
            // btnIPreview
            // 
            this.btnIPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIPreview.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnIPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIPreview.Location = new System.Drawing.Point(838, 485);
            this.btnIPreview.Name = "btnIPreview";
            this.btnIPreview.Size = new System.Drawing.Size(72, 30);
            this.btnIPreview.TabIndex = 365;
            this.btnIPreview.Text = "&Preview";
            this.btnIPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIPreview.UseVisualStyleBackColor = true;
            this.btnIPreview.Click += new System.EventHandler(this.btnIPreview_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkCheck);
            this.groupBox1.Location = new System.Drawing.Point(868, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 49);
            this.groupBox1.TabIndex = 367;
            this.groupBox1.TabStop = false;
            // 
            // chkCheck
            // 
            this.chkCheck.AutoSize = true;
            this.chkCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheck.ForeColor = System.Drawing.Color.Green;
            this.chkCheck.Location = new System.Drawing.Point(10, 20);
            this.chkCheck.Name = "chkCheck";
            this.chkCheck.Size = new System.Drawing.Size(80, 17);
            this.chkCheck.TabIndex = 0;
            this.chkCheck.Text = "Check All";
            this.chkCheck.UseVisualStyleBackColor = true;
            this.chkCheck.CheckedChanged += new System.EventHandler(this.chkCheck_CheckedChanged);
            // 
            // grdAccount
            // 
            this.grdAccount.BackColor = System.Drawing.Color.Transparent;
            this.grdAccount.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdAccount.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdAccount.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdAccount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdAccount.Location = new System.Drawing.Point(8, 114);
            this.grdAccount.Name = "grdAccount";
            this.grdAccount.Size = new System.Drawing.Size(979, 364);
            this.grdAccount.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdAccount.Styles"));
            this.grdAccount.TabIndex = 364;
            // 
            // ucFilterByDate1
            // 
            this.ucFilterByDate1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByDate1.Location = new System.Drawing.Point(5, 3);
            this.ucFilterByDate1.Name = "ucFilterByDate1";
            this.ucFilterByDate1.Size = new System.Drawing.Size(453, 65);
            this.ucFilterByDate1.TabIndex = 368;
            // 
            // ucFilterByCompany1
            // 
            this.ucFilterByCompany1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByCompany1.Location = new System.Drawing.Point(457, 12);
            this.ucFilterByCompany1.Name = "ucFilterByCompany1";
            this.ucFilterByCompany1.Size = new System.Drawing.Size(231, 60);
            this.ucFilterByCompany1.TabIndex = 369;
            // 
            // ucFilterByOther1
            // 
            this.ucFilterByOther1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByOther1.Location = new System.Drawing.Point(681, 11);
            this.ucFilterByOther1.Name = "ucFilterByOther1";
            this.ucFilterByOther1.Query = null;
            this.ucFilterByOther1.Size = new System.Drawing.Size(188, 52);
            this.ucFilterByOther1.TabIndex = 370;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(687, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 14);
            this.label1.TabIndex = 371;
            this.label1.Text = "Paid/Rec";
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearSearch.Image = global::Tourist_Management.Properties.Resources.cancel;
            this.btnClearSearch.Location = new System.Drawing.Point(963, 81);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(24, 22);
            this.btnClearSearch.TabIndex = 374;
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // txtAccountName
            // 
            this.txtAccountName.BackColor = System.Drawing.Color.LightYellow;
            this.txtAccountName.Location = new System.Drawing.Point(841, 82);
            this.txtAccountName.MaxLength = 0;
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(120, 20);
            this.txtAccountName.TabIndex = 373;
            this.txtAccountName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccountName.TextChanged += new System.EventHandler(this.txtAccountName_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.rdbSummary);
            this.groupBox2.Controls.Add(this.rdbDetail);
            this.groupBox2.Location = new System.Drawing.Point(8, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(145, 43);
            this.groupBox2.TabIndex = 375;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Type";
            // 
            // rdbSummary
            // 
            this.rdbSummary.AutoSize = true;
            this.rdbSummary.Location = new System.Drawing.Point(72, 16);
            this.rdbSummary.Name = "rdbSummary";
            this.rdbSummary.Size = new System.Drawing.Size(68, 17);
            this.rdbSummary.TabIndex = 1;
            this.rdbSummary.Text = "Summary";
            this.rdbSummary.UseVisualStyleBackColor = true;
            this.rdbSummary.CheckedChanged += new System.EventHandler(this.rdbSummary_CheckedChanged);
            // 
            // rdbDetail
            // 
            this.rdbDetail.AutoSize = true;
            this.rdbDetail.Checked = true;
            this.rdbDetail.Location = new System.Drawing.Point(9, 16);
            this.rdbDetail.Name = "rdbDetail";
            this.rdbDetail.Size = new System.Drawing.Size(52, 17);
            this.rdbDetail.TabIndex = 0;
            this.rdbDetail.TabStop = true;
            this.rdbDetail.Text = "Detail";
            this.rdbDetail.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.cmbAccountType);
            this.groupBox3.Location = new System.Drawing.Point(159, 65);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(196, 43);
            this.groupBox3.TabIndex = 376;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Account Type";
            // 
            // cmbAccountType
            // 
            this.cmbAccountType.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.Enabled = false;
            this.cmbAccountType.FormattingEnabled = true;
            this.cmbAccountType.Location = new System.Drawing.Point(7, 15);
            this.cmbAccountType.Name = "cmbAccountType";
            this.cmbAccountType.Size = new System.Drawing.Size(183, 21);
            this.cmbAccountType.TabIndex = 0;
            this.cmbAccountType.SelectedIndexChanged += new System.EventHandler(this.cmbAccountType_SelectedIndexChanged);
            // 
            // cmbOp
            // 
            this.cmbOp.BackColor = System.Drawing.Color.White;
            this.cmbOp.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOp.ForeColor = System.Drawing.Color.Black;
            this.cmbOp.FormattingEnabled = true;
            this.cmbOp.Location = new System.Drawing.Point(677, 82);
            this.cmbOp.Name = "cmbOp";
            this.cmbOp.Size = new System.Drawing.Size(158, 21);
            this.cmbOp.TabIndex = 378;
            // 
            // cmbFld
            // 
            this.cmbFld.BackColor = System.Drawing.Color.White;
            this.cmbFld.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFld.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFld.ForeColor = System.Drawing.Color.Black;
            this.cmbFld.FormattingEnabled = true;
            this.cmbFld.Location = new System.Drawing.Point(518, 82);
            this.cmbFld.Name = "cmbFld";
            this.cmbFld.Size = new System.Drawing.Size(153, 21);
            this.cmbFld.TabIndex = 377;
            // 
            // bPrintTotals
            // 
            this.bPrintTotals.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bPrintTotals.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bPrintTotals.Image = global::Tourist_Management.Properties.Resources.search;
            this.bPrintTotals.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bPrintTotals.Location = new System.Drawing.Point(760, 485);
            this.bPrintTotals.Name = "bPrintTotals";
            this.bPrintTotals.Size = new System.Drawing.Size(72, 30);
            this.bPrintTotals.TabIndex = 379;
            this.bPrintTotals.Text = "&Totals";
            this.bPrintTotals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bPrintTotals.UseVisualStyleBackColor = true;
            this.bPrintTotals.Click += new System.EventHandler(this.bPrintTotals_Click);
            // 
            // frmLedgerAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(993, 523);
            this.ControlBox = false;
            this.Controls.Add(this.bPrintTotals);
            this.Controls.Add(this.cmbOp);
            this.Controls.Add(this.cmbFld);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.txtAccountName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ucFilterByOther1);
            this.Controls.Add(this.ucFilterByCompany1);
            this.Controls.Add(this.ucFilterByDate1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnICancel);
            this.Controls.Add(this.btnIPreview);
            this.Controls.Add(this.grdAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "frmLedgerAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmLedgerAccount_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Button btnICancel;
        private System.Windows.Forms.Button btnIPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCheck;
        private C1.Win.C1FlexGrid.C1FlexGrid grdAccount;
        private Tourist_Management.User_Controls.ucFilterByDate ucFilterByDate1;
        private Tourist_Management.User_Controls.ucFilterByCompany ucFilterByCompany1;
        private Tourist_Management.User_Controls.ucFilterByOther ucFilterByOther1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbDetail;
        private System.Windows.Forms.RadioButton rdbSummary;
        private System.Windows.Forms.GroupBox groupBox3;
        private  Tourist_Management.User_Controls.ComboBox cmbAccountType;
        internal  Tourist_Management.User_Controls.ComboBox cmbOp;
        internal  Tourist_Management.User_Controls.ComboBox cmbFld;
        private System.Windows.Forms.Button bPrintTotals;
    }
}