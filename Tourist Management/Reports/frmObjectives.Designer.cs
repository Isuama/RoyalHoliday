namespace Tourist_Management.Reports
{
    partial class frmObjectives
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmObjectives));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.cmbECompany = new Tourist_Management.User_Controls.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.drpEHandled = new Tourist_Management.DropDowns.DropSearch();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEarnings = new System.Windows.Forms.TextBox();
            this.txtTOver = new System.Windows.Forms.TextBox();
            this.txtPax = new System.Windows.Forms.TextBox();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.grdEst = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dtpYear = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pbLoad = new System.Windows.Forms.PictureBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCompany = new System.Windows.Forms.CheckBox();
            this.cmbCCompany = new Tourist_Management.User_Controls.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpFilterByDate = new System.Windows.Forms.GroupBox();
            this.dtpMonthTo = new System.Windows.Forms.DateTimePicker();
            this.dtpMonthFrom = new System.Windows.Forms.DateTimePicker();
            this.lblDateCol1 = new System.Windows.Forms.Label();
            this.lblDateCol2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grdComp = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.drpCHandled = new Tourist_Management.DropDowns.DropSearch();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.cachedcrTrialBalance1 = new Tourist_Management.MasterReports.CachedcrTrialBalance();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox31.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEst)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.grpFilterByDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdComp)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 372);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage1.Controls.Add(this.groupBox31);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.grdEst);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(864, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Estimated";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox31
            // 
            this.groupBox31.BackColor = System.Drawing.Color.Transparent;
            this.groupBox31.Controls.Add(this.cmbECompany);
            this.groupBox31.Controls.Add(this.label36);
            this.groupBox31.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox31.Location = new System.Drawing.Point(431, 5);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(217, 51);
            this.groupBox31.TabIndex = 364;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "BY COMPANY";
            // 
            // cmbECompany
            // 
            this.cmbECompany.DataSource = null;
            this.cmbECompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbECompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbECompany.FormattingEnabled = true;
            this.cmbECompany.Location = new System.Drawing.Point(77, 18);
            this.cmbECompany.Name = "cmbECompany";
            this.cmbECompany.Size = new System.Drawing.Size(128, 21);
            this.cmbECompany.TabIndex = 380;
            this.cmbECompany.SelectedIndexChanged += new System.EventHandler(this.cmbECompany_SelectedIndexChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(7, 22);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(62, 13);
            this.label36.TabIndex = 379;
            this.label36.Text = "Company";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.drpEHandled);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox2.Location = new System.Drawing.Point(650, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 51);
            this.groupBox2.TabIndex = 363;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HANDLED BY";
            // 
            // drpEHandled
            // 
            this.drpEHandled.BackColor = System.Drawing.Color.Transparent;
            this.drpEHandled.DataSource = null;
            this.drpEHandled.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpEHandled.Location = new System.Drawing.Point(15, 20);
            this.drpEHandled.Name = "drpEHandled";
            this.drpEHandled.SelectedText = "";
            this.drpEHandled.SelectedValue = "";
            this.drpEHandled.Size = new System.Drawing.Size(180, 21);
            this.drpEHandled.TabIndex = 380;
            this.drpEHandled.Selected_TextChanged += new System.EventHandler(this.drpEHandled_Selected_TextChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.txtEarnings);
            this.groupBox7.Controls.Add(this.txtTOver);
            this.groupBox7.Controls.Add(this.txtPax);
            this.groupBox7.Controls.Add(this.chkDefault);
            this.groupBox7.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox7.Location = new System.Drawing.Point(7, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(419, 51);
            this.groupBox7.TabIndex = 362;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "DEFAULT VALUES";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(250, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 12);
            this.label3.TabIndex = 251;
            this.label3.Text = "Earnings (Rs)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(90, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 251;
            this.label2.Text = "T/Over (Rs)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 251;
            this.label1.Text = "Pax";
            // 
            // txtEarnings
            // 
            this.txtEarnings.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEarnings.Location = new System.Drawing.Point(329, 21);
            this.txtEarnings.Name = "txtEarnings";
            this.txtEarnings.Size = new System.Drawing.Size(84, 18);
            this.txtEarnings.TabIndex = 250;
            this.txtEarnings.TextChanged += new System.EventHandler(this.txtEarnings_TextChanged);
            // 
            // txtTOver
            // 
            this.txtTOver.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTOver.Location = new System.Drawing.Point(161, 21);
            this.txtTOver.Name = "txtTOver";
            this.txtTOver.Size = new System.Drawing.Size(78, 18);
            this.txtTOver.TabIndex = 250;
            this.txtTOver.TextChanged += new System.EventHandler(this.txtTOver_TextChanged);
            // 
            // txtPax
            // 
            this.txtPax.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPax.Location = new System.Drawing.Point(35, 21);
            this.txtPax.Name = "txtPax";
            this.txtPax.Size = new System.Drawing.Size(42, 18);
            this.txtPax.TabIndex = 250;
            this.txtPax.TextChanged += new System.EventHandler(this.txtPax_TextChanged);
            // 
            // chkDefault
            // 
            this.chkDefault.AutoSize = true;
            this.chkDefault.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDefault.Location = new System.Drawing.Point(130, 0);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(15, 14);
            this.chkDefault.TabIndex = 249;
            this.chkDefault.UseVisualStyleBackColor = true;
            this.chkDefault.CheckedChanged += new System.EventHandler(this.chkDefault_CheckedChanged);
            // 
            // grdEst
            // 
            this.grdEst.BackColor = System.Drawing.Color.Transparent;
            this.grdEst.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdEst.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdEst.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdEst.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdEst.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdEst.Location = new System.Drawing.Point(83, 100);
            this.grdEst.Name = "grdEst";
            this.grdEst.Size = new System.Drawing.Size(708, 238);
            this.grdEst.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdEst.Styles"));
            this.grdEst.TabIndex = 361;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dtpYear);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(82, 54);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(165, 40);
            this.groupBox4.TabIndex = 366;
            this.groupBox4.TabStop = false;
            // 
            // dtpYear
            // 
            this.dtpYear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYear.Location = new System.Drawing.Point(48, 13);
            this.dtpYear.Name = "dtpYear";
            this.dtpYear.Size = new System.Drawing.Size(94, 21);
            this.dtpYear.TabIndex = 365;
            this.dtpYear.ValueChanged += new System.EventHandler(this.dtpYear_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(9, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 251;
            this.label6.Text = "Year";
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.tabPage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage2.Controls.Add(this.pbLoad);
            this.tabPage2.Controls.Add(this.btnShow);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.grpFilterByDate);
            this.tabPage2.Controls.Add(this.grdComp);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(864, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Comparison";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pbLoad
            // 
            this.pbLoad.Image = global::Tourist_Management.Properties.Resources.Processing;
            this.pbLoad.Location = new System.Drawing.Point(330, 128);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(214, 138);
            this.pbLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLoad.TabIndex = 375;
            this.pbLoad.TabStop = false;
            this.pbLoad.Visible = false;
            // 
            // btnShow
            // 
            this.btnShow.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.btnShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShow.Image = global::Tourist_Management.Properties.Resources.filter;
            this.btnShow.Location = new System.Drawing.Point(811, 18);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(51, 36);
            this.btnShow.TabIndex = 374;
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.chkCompany);
            this.groupBox3.Controls.Add(this.cmbCCompany);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox3.Location = new System.Drawing.Point(385, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 51);
            this.groupBox3.TabIndex = 373;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BY COMPANY";
            // 
            // chkCompany
            // 
            this.chkCompany.AutoSize = true;
            this.chkCompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCompany.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkCompany.Location = new System.Drawing.Point(105, 0);
            this.chkCompany.Name = "chkCompany";
            this.chkCompany.Size = new System.Drawing.Size(15, 14);
            this.chkCompany.TabIndex = 376;
            this.chkCompany.UseVisualStyleBackColor = true;
            this.chkCompany.CheckedChanged += new System.EventHandler(this.chkCompany_CheckedChanged);
            // 
            // cmbCCompany
            // 
            this.cmbCCompany.DataSource = null;
            this.cmbCCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCCompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCCompany.FormattingEnabled = true;
            this.cmbCCompany.Location = new System.Drawing.Point(77, 18);
            this.cmbCCompany.Name = "cmbCCompany";
            this.cmbCCompany.Size = new System.Drawing.Size(125, 21);
            this.cmbCCompany.TabIndex = 380;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 379;
            this.label5.Text = "Company";
            // 
            // grpFilterByDate
            // 
            this.grpFilterByDate.Controls.Add(this.dtpMonthTo);
            this.grpFilterByDate.Controls.Add(this.dtpMonthFrom);
            this.grpFilterByDate.Controls.Add(this.lblDateCol1);
            this.grpFilterByDate.Controls.Add(this.lblDateCol2);
            this.grpFilterByDate.Controls.Add(this.label4);
            this.grpFilterByDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFilterByDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.grpFilterByDate.Location = new System.Drawing.Point(7, 6);
            this.grpFilterByDate.Name = "grpFilterByDate";
            this.grpFilterByDate.Size = new System.Drawing.Size(374, 51);
            this.grpFilterByDate.TabIndex = 372;
            this.grpFilterByDate.TabStop = false;
            this.grpFilterByDate.Text = "FILTER BY MONTH";
            // 
            // dtpMonthTo
            // 
            this.dtpMonthTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpMonthTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonthTo.Location = new System.Drawing.Point(262, 21);
            this.dtpMonthTo.Name = "dtpMonthTo";
            this.dtpMonthTo.Size = new System.Drawing.Size(103, 21);
            this.dtpMonthTo.TabIndex = 368;
            // 
            // dtpMonthFrom
            // 
            this.dtpMonthFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpMonthFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonthFrom.Location = new System.Drawing.Point(83, 21);
            this.dtpMonthFrom.Name = "dtpMonthFrom";
            this.dtpMonthFrom.Size = new System.Drawing.Size(106, 21);
            this.dtpMonthFrom.TabIndex = 0;
            // 
            // lblDateCol1
            // 
            this.lblDateCol1.AutoSize = true;
            this.lblDateCol1.BackColor = System.Drawing.Color.Transparent;
            this.lblDateCol1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblDateCol1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateCol1.Location = new System.Drawing.Point(8, 27);
            this.lblDateCol1.Name = "lblDateCol1";
            this.lblDateCol1.Size = new System.Drawing.Size(74, 13);
            this.lblDateCol1.TabIndex = 367;
            this.lblDateCol1.Text = "Month From";
            // 
            // lblDateCol2
            // 
            this.lblDateCol2.AutoSize = true;
            this.lblDateCol2.BackColor = System.Drawing.Color.Transparent;
            this.lblDateCol2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblDateCol2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateCol2.Location = new System.Drawing.Point(203, 26);
            this.lblDateCol2.Name = "lblDateCol2";
            this.lblDateCol2.Size = new System.Drawing.Size(59, 13);
            this.lblDateCol2.TabIndex = 367;
            this.lblDateCol2.Text = "Month To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(17, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 367;
            // 
            // grdComp
            // 
            this.grdComp.BackColor = System.Drawing.Color.Transparent;
            this.grdComp.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdComp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdComp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdComp.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdComp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdComp.Location = new System.Drawing.Point(16, 87);
            this.grdComp.Name = "grdComp";
            this.grdComp.Size = new System.Drawing.Size(826, 241);
            this.grdComp.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdComp.Styles"));
            this.grdComp.TabIndex = 365;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.drpCHandled);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(601, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 51);
            this.groupBox1.TabIndex = 364;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HANDLED BY";
            // 
            // drpCHandled
            // 
            this.drpCHandled.BackColor = System.Drawing.Color.Transparent;
            this.drpCHandled.DataSource = null;
            this.drpCHandled.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpCHandled.Location = new System.Drawing.Point(15, 20);
            this.drpCHandled.Name = "drpCHandled";
            this.drpCHandled.SelectedText = "";
            this.drpCHandled.SelectedValue = "";
            this.drpCHandled.Size = new System.Drawing.Size(180, 21);
            this.drpCHandled.TabIndex = 380;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(767, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(676, 394);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.btnPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreview.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPreview.Location = new System.Drawing.Point(582, 394);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 30);
            this.btnPreview.TabIndex = 366;
            this.btnPreview.Text = "Preview";
            this.btnPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // frmObjectives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(920, 449);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmObjectives";
            this.Text = "frmObjectives";
            this.Load += new System.EventHandler(this.frmObjectives_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEst)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpFilterByDate.ResumeLayout(false);
            this.grpFilterByDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdComp)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private C1.Win.C1FlexGrid.C1FlexGrid grdEst;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox2;
        private DropDowns.DropSearch drpEHandled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPax;
        private System.Windows.Forms.CheckBox chkDefault;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEarnings;
        private System.Windows.Forms.TextBox txtTOver;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private DropDowns.DropSearch drpCHandled;
        private C1.Win.C1FlexGrid.C1FlexGrid grdComp;
        private System.Windows.Forms.GroupBox grpFilterByDate;
        private System.Windows.Forms.DateTimePicker dtpMonthTo;
        private System.Windows.Forms.DateTimePicker dtpMonthFrom;
        private System.Windows.Forms.Label lblDateCol1;
        private System.Windows.Forms.Label lblDateCol2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox groupBox31;
        private  Tourist_Management.User_Controls.ComboBox cmbECompany;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.GroupBox groupBox3;
        private  Tourist_Management.User_Controls.ComboBox cmbCCompany;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.PictureBox pbLoad;
        private System.Windows.Forms.CheckBox chkCompany;
        private System.Windows.Forms.DateTimePicker dtpYear;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private MasterReports.CachedcrTrialBalance cachedcrTrialBalance1;
    }
}