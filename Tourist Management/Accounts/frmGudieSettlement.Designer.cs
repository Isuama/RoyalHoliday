namespace Tourist_Management.Accounts
{
    partial class frmGudieSettlement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGudieSettlement));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblGuideName = new System.Windows.Forms.Label();
            this.cmbGuide = new Tourist_Management.User_Controls.ComboBox();
            this.btnTour = new System.Windows.Forms.Button();
            this.txtTourNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDue = new System.Windows.Forms.Label();
            this.btnGetTot = new System.Windows.Forms.Button();
            this.lblTotPay = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtPaidAmt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grpBasics = new System.Windows.Forms.GroupBox();
            this.grpPayMethod = new System.Windows.Forms.GroupBox();
            this.txtChkNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.rdbCash = new System.Windows.Forms.RadioButton();
            this.rdbBank = new System.Windows.Forms.RadioButton();
            this.chkNotPaid = new System.Windows.Forms.CheckBox();
            this.lblPaidDate = new System.Windows.Forms.Label();
            this.dtpPaidDate = new System.Windows.Forms.DateTimePicker();
            this.txtGuideName = new System.Windows.Forms.TextBox();
            this.chkPaid = new System.Windows.Forms.CheckBox();
            this.pbCompLogo = new System.Windows.Forms.PictureBox();
            this.chkConfirm = new System.Windows.Forms.CheckBox();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.txtLicense = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.txtNIC = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grdTAdvance = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.grdTExpense = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.btnIPreview = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cachedcrTrialBalance1 = new Tourist_Management.MasterReports.CachedcrTrialBalance();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grpBasics.SuspendLayout();
            this.grpPayMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCompLogo)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTAdvance)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTExpense)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblGuideName);
            this.groupBox1.Controls.Add(this.cmbGuide);
            this.groupBox1.Controls.Add(this.btnTour);
            this.groupBox1.Controls.Add(this.txtTourNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 42);
            this.groupBox1.TabIndex = 347;
            this.groupBox1.TabStop = false;
            // 
            // lblGuideName
            // 
            this.lblGuideName.AutoSize = true;
            this.lblGuideName.Location = new System.Drawing.Point(332, 18);
            this.lblGuideName.Name = "lblGuideName";
            this.lblGuideName.Size = new System.Drawing.Size(77, 13);
            this.lblGuideName.TabIndex = 258;
            this.lblGuideName.Text = "Guide Name";
            // 
            // cmbGuide
            // 
            this.cmbGuide.DataSource = null;
            this.cmbGuide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGuide.Enabled = false;
            this.cmbGuide.FormattingEnabled = true;
            this.cmbGuide.Location = new System.Drawing.Point(415, 15);
            this.cmbGuide.Name = "cmbGuide";
            this.cmbGuide.Size = new System.Drawing.Size(154, 21);
            this.cmbGuide.TabIndex = 257;
            this.cmbGuide.SelectedIndexChanged += new System.EventHandler(this.cmbGuide_SelectedIndexChanged);
            // 
            // btnTour
            // 
            this.btnTour.Location = new System.Drawing.Point(276, 14);
            this.btnTour.Name = "btnTour";
            this.btnTour.Size = new System.Drawing.Size(37, 23);
            this.btnTour.TabIndex = 256;
            this.btnTour.Text = "...";
            this.btnTour.UseVisualStyleBackColor = true;
            this.btnTour.Click += new System.EventHandler(this.btnTour_Click);
            // 
            // txtTourNo
            // 
            this.txtTourNo.Enabled = false;
            this.txtTourNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTourNo.ForeColor = System.Drawing.Color.Blue;
            this.txtTourNo.Location = new System.Drawing.Point(111, 15);
            this.txtTourNo.Name = "txtTourNo";
            this.txtTourNo.ReadOnly = true;
            this.txtTourNo.Size = new System.Drawing.Size(159, 21);
            this.txtTourNo.TabIndex = 255;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 254;
            this.label1.Text = "Reference No";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(473, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 18);
            this.label3.TabIndex = 357;
            this.label3.Text = ":";
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.BackColor = System.Drawing.Color.Transparent;
            this.lblDue.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDue.ForeColor = System.Drawing.Color.Red;
            this.lblDue.Location = new System.Drawing.Point(488, 80);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(47, 18);
            this.lblDue.TabIndex = 358;
            this.lblDue.Text = "0.00";
            // 
            // btnGetTot
            // 
            this.btnGetTot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetTot.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetTot.Image = global::Tourist_Management.Properties.Resources.sum;
            this.btnGetTot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetTot.Location = new System.Drawing.Point(221, 405);
            this.btnGetTot.Name = "btnGetTot";
            this.btnGetTot.Size = new System.Drawing.Size(73, 33);
            this.btnGetTot.TabIndex = 355;
            this.btnGetTot.Text = "Total";
            this.btnGetTot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGetTot.UseVisualStyleBackColor = true;
            this.btnGetTot.Click += new System.EventHandler(this.btnGetTot_Click);
            // 
            // lblTotPay
            // 
            this.lblTotPay.AutoSize = true;
            this.lblTotPay.BackColor = System.Drawing.Color.Transparent;
            this.lblTotPay.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotPay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTotPay.Location = new System.Drawing.Point(156, 80);
            this.lblTotPay.Name = "lblTotPay";
            this.lblTotPay.Size = new System.Drawing.Size(47, 18);
            this.lblTotPay.TabIndex = 350;
            this.lblTotPay.Text = "0.00";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label26.Location = new System.Drawing.Point(138, 79);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 18);
            this.label26.TabIndex = 349;
            this.label26.Text = ":";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label23.Location = new System.Drawing.Point(19, 80);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(119, 18);
            this.label23.TabIndex = 348;
            this.label23.Text = "Paid Amount";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(521, 405);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 33);
            this.btnCancel.TabIndex = 352;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(357, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 18);
            this.label4.TabIndex = 356;
            this.label4.Text = "Due Amount";
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Enabled = false;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(371, 405);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(73, 33);
            this.btnOk.TabIndex = 351;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPaidAmt
            // 
            this.txtPaidAmt.Location = new System.Drawing.Point(111, 193);
            this.txtPaidAmt.Name = "txtPaidAmt";
            this.txtPaidAmt.Size = new System.Drawing.Size(168, 21);
            this.txtPaidAmt.TabIndex = 361;
            this.txtPaidAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 360;
            this.label2.Text = "Paid Amount";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(285, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 15);
            this.label5.TabIndex = 362;
            this.label5.Text = "LKR";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(14, 110);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 286);
            this.tabControl1.TabIndex = 363;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grpBasics);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 260);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grpBasics
            // 
            this.grpBasics.Controls.Add(this.grpPayMethod);
            this.grpBasics.Controls.Add(this.chkNotPaid);
            this.grpBasics.Controls.Add(this.lblPaidDate);
            this.grpBasics.Controls.Add(this.label5);
            this.grpBasics.Controls.Add(this.dtpPaidDate);
            this.grpBasics.Controls.Add(this.txtPaidAmt);
            this.grpBasics.Controls.Add(this.label2);
            this.grpBasics.Controls.Add(this.txtGuideName);
            this.grpBasics.Controls.Add(this.chkPaid);
            this.grpBasics.Controls.Add(this.pbCompLogo);
            this.grpBasics.Controls.Add(this.chkConfirm);
            this.grpBasics.Controls.Add(this.txtDays);
            this.grpBasics.Controls.Add(this.label47);
            this.grpBasics.Controls.Add(this.txtFee);
            this.grpBasics.Controls.Add(this.label46);
            this.grpBasics.Controls.Add(this.txtTelephone);
            this.grpBasics.Controls.Add(this.label45);
            this.grpBasics.Controls.Add(this.txtLicense);
            this.grpBasics.Controls.Add(this.label44);
            this.grpBasics.Controls.Add(this.txtNIC);
            this.grpBasics.Controls.Add(this.label42);
            this.grpBasics.Controls.Add(this.txtCompany);
            this.grpBasics.Controls.Add(this.label41);
            this.grpBasics.Controls.Add(this.label43);
            this.grpBasics.Location = new System.Drawing.Point(7, 2);
            this.grpBasics.Name = "grpBasics";
            this.grpBasics.Size = new System.Drawing.Size(569, 250);
            this.grpBasics.TabIndex = 360;
            this.grpBasics.TabStop = false;
            // 
            // grpPayMethod
            // 
            this.grpPayMethod.BackColor = System.Drawing.Color.Transparent;
            this.grpPayMethod.Controls.Add(this.txtChkNo);
            this.grpPayMethod.Controls.Add(this.label14);
            this.grpPayMethod.Controls.Add(this.rdbCash);
            this.grpPayMethod.Controls.Add(this.rdbBank);
            this.grpPayMethod.Location = new System.Drawing.Point(325, 165);
            this.grpPayMethod.Name = "grpPayMethod";
            this.grpPayMethod.Size = new System.Drawing.Size(233, 64);
            this.grpPayMethod.TabIndex = 423;
            this.grpPayMethod.TabStop = false;
            this.grpPayMethod.Text = "Pay Method";
            // 
            // txtChkNo
            // 
            this.txtChkNo.Enabled = false;
            this.txtChkNo.Location = new System.Drawing.Point(73, 32);
            this.txtChkNo.Name = "txtChkNo";
            this.txtChkNo.Size = new System.Drawing.Size(154, 21);
            this.txtChkNo.TabIndex = 279;
            this.txtChkNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(119, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 13);
            this.label14.TabIndex = 280;
            this.label14.Text = "Cheque No";
            // 
            // rdbCash
            // 
            this.rdbCash.AutoSize = true;
            this.rdbCash.Location = new System.Drawing.Point(5, 38);
            this.rdbCash.Name = "rdbCash";
            this.rdbCash.Size = new System.Drawing.Size(49, 17);
            this.rdbCash.TabIndex = 1;
            this.rdbCash.TabStop = true;
            this.rdbCash.Text = "Cash";
            this.rdbCash.UseVisualStyleBackColor = true;
            this.rdbCash.CheckedChanged += new System.EventHandler(this.rdbCash_CheckedChanged);
            // 
            // rdbBank
            // 
            this.rdbBank.AutoSize = true;
            this.rdbBank.Location = new System.Drawing.Point(5, 19);
            this.rdbBank.Name = "rdbBank";
            this.rdbBank.Size = new System.Drawing.Size(50, 17);
            this.rdbBank.TabIndex = 0;
            this.rdbBank.TabStop = true;
            this.rdbBank.Text = "Bank";
            this.rdbBank.UseVisualStyleBackColor = true;
            this.rdbBank.CheckedChanged += new System.EventHandler(this.rdbBank_CheckedChanged);
            // 
            // chkNotPaid
            // 
            this.chkNotPaid.AutoSize = true;
            this.chkNotPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNotPaid.ForeColor = System.Drawing.Color.Red;
            this.chkNotPaid.Location = new System.Drawing.Point(461, 41);
            this.chkNotPaid.Name = "chkNotPaid";
            this.chkNotPaid.Size = new System.Drawing.Size(110, 19);
            this.chkNotPaid.TabIndex = 380;
            this.chkNotPaid.Text = "No Payments";
            this.chkNotPaid.UseVisualStyleBackColor = true;
            this.chkNotPaid.CheckedChanged += new System.EventHandler(this.chkNotPaid_CheckedChanged);
            // 
            // lblPaidDate
            // 
            this.lblPaidDate.AutoSize = true;
            this.lblPaidDate.BackColor = System.Drawing.Color.Transparent;
            this.lblPaidDate.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaidDate.Location = new System.Drawing.Point(332, 142);
            this.lblPaidDate.Name = "lblPaidDate";
            this.lblPaidDate.Size = new System.Drawing.Size(62, 13);
            this.lblPaidDate.TabIndex = 378;
            this.lblPaidDate.Text = "Paid Date";
            // 
            // dtpPaidDate
            // 
            this.dtpPaidDate.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaidDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaidDate.Location = new System.Drawing.Point(404, 139);
            this.dtpPaidDate.Name = "dtpPaidDate";
            this.dtpPaidDate.Size = new System.Drawing.Size(154, 20);
            this.dtpPaidDate.TabIndex = 379;
            // 
            // txtGuideName
            // 
            this.txtGuideName.Enabled = false;
            this.txtGuideName.Location = new System.Drawing.Point(111, 15);
            this.txtGuideName.Name = "txtGuideName";
            this.txtGuideName.ReadOnly = true;
            this.txtGuideName.Size = new System.Drawing.Size(211, 21);
            this.txtGuideName.TabIndex = 342;
            this.txtGuideName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkPaid
            // 
            this.chkPaid.AutoSize = true;
            this.chkPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.chkPaid.Location = new System.Drawing.Point(461, 19);
            this.chkPaid.Name = "chkPaid";
            this.chkPaid.Size = new System.Drawing.Size(98, 19);
            this.chkPaid.TabIndex = 341;
            this.chkPaid.Text = "Mark As Paid";
            this.chkPaid.UseVisualStyleBackColor = true;
            this.chkPaid.CheckedChanged += new System.EventHandler(this.chkPaid_CheckedChanged_1);
            // 
            // pbCompLogo
            // 
            this.pbCompLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbCompLogo.Image = global::Tourist_Management.Properties.Resources.noimage;
            this.pbCompLogo.Location = new System.Drawing.Point(326, 15);
            this.pbCompLogo.Name = "pbCompLogo";
            this.pbCompLogo.Size = new System.Drawing.Size(131, 118);
            this.pbCompLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCompLogo.TabIndex = 286;
            this.pbCompLogo.TabStop = false;
            // 
            // chkConfirm
            // 
            this.chkConfirm.AutoSize = true;
            this.chkConfirm.BackColor = System.Drawing.Color.Transparent;
            this.chkConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.chkConfirm.Location = new System.Drawing.Point(461, 64);
            this.chkConfirm.Name = "chkConfirm";
            this.chkConfirm.Size = new System.Drawing.Size(96, 19);
            this.chkConfirm.TabIndex = 340;
            this.chkConfirm.Text = "Confirm All";
            this.chkConfirm.UseVisualStyleBackColor = false;
            this.chkConfirm.CheckedChanged += new System.EventHandler(this.chkConfirm_CheckedChanged);
            // 
            // txtDays
            // 
            this.txtDays.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDays.Location = new System.Drawing.Point(111, 169);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(211, 21);
            this.txtDays.TabIndex = 285;
            this.txtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDays.TextChanged += new System.EventHandler(this.txtDays_TextChanged);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(9, 172);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(72, 13);
            this.label47.TabIndex = 284;
            this.label47.Text = "No Of Days";
            // 
            // txtFee
            // 
            this.txtFee.BackColor = System.Drawing.SystemColors.Window;
            this.txtFee.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFee.Location = new System.Drawing.Point(111, 143);
            this.txtFee.Name = "txtFee";
            this.txtFee.Size = new System.Drawing.Size(211, 21);
            this.txtFee.TabIndex = 283;
            this.txtFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFee.TextChanged += new System.EventHandler(this.txtFee_TextChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(9, 146);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(27, 13);
            this.label46.TabIndex = 282;
            this.label46.Text = "Fee";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Enabled = false;
            this.txtTelephone.Location = new System.Drawing.Point(111, 117);
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.ReadOnly = true;
            this.txtTelephone.Size = new System.Drawing.Size(211, 21);
            this.txtTelephone.TabIndex = 281;
            this.txtTelephone.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(9, 120);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(66, 13);
            this.label45.TabIndex = 280;
            this.label45.Text = "Telephone";
            // 
            // txtLicense
            // 
            this.txtLicense.Enabled = false;
            this.txtLicense.Location = new System.Drawing.Point(111, 91);
            this.txtLicense.Name = "txtLicense";
            this.txtLicense.ReadOnly = true;
            this.txtLicense.Size = new System.Drawing.Size(211, 21);
            this.txtLicense.TabIndex = 279;
            this.txtLicense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(9, 94);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(68, 13);
            this.label44.TabIndex = 278;
            this.label44.Text = "License No";
            // 
            // txtNIC
            // 
            this.txtNIC.Enabled = false;
            this.txtNIC.Location = new System.Drawing.Point(111, 66);
            this.txtNIC.Name = "txtNIC";
            this.txtNIC.ReadOnly = true;
            this.txtNIC.Size = new System.Drawing.Size(211, 21);
            this.txtNIC.TabIndex = 277;
            this.txtNIC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(9, 70);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(48, 13);
            this.label42.TabIndex = 276;
            this.label42.Text = "NIC No";
            // 
            // txtCompany
            // 
            this.txtCompany.Enabled = false;
            this.txtCompany.Location = new System.Drawing.Point(111, 41);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.ReadOnly = true;
            this.txtCompany.Size = new System.Drawing.Size(211, 21);
            this.txtCompany.TabIndex = 275;
            this.txtCompany.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(9, 44);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(99, 13);
            this.label41.TabIndex = 274;
            this.label41.Text = "Company Name";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(9, 19);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(77, 13);
            this.label43.TabIndex = 272;
            this.label43.Text = "Guide Name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grdTAdvance);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(576, 260);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tour Advances";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grdTAdvance
            // 
            this.grdTAdvance.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdTAdvance.BackColor = System.Drawing.Color.Transparent;
            this.grdTAdvance.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdTAdvance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdTAdvance.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdTAdvance.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdTAdvance.Location = new System.Drawing.Point(0, 3);
            this.grdTAdvance.Name = "grdTAdvance";
            this.grdTAdvance.Size = new System.Drawing.Size(576, 254);
            this.grdTAdvance.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdTAdvance.Styles"));
            this.grdTAdvance.TabIndex = 317;
            this.grdTAdvance.LeaveCell += new System.EventHandler(this.grdTAdvance_LeaveCell);
            this.grdTAdvance.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdTAdvance_AfterEdit);
            this.grdTAdvance.Click += new System.EventHandler(this.grdTAdvance_Click);
            this.grdTAdvance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdTAdvance_KeyDown);
            this.grdTAdvance.Leave += new System.EventHandler(this.grdTAdvance_Leave);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grdTExpense);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(576, 260);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tour Expenses";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // grdTExpense
            // 
            this.grdTExpense.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdTExpense.BackColor = System.Drawing.Color.Transparent;
            this.grdTExpense.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdTExpense.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdTExpense.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdTExpense.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdTExpense.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdTExpense.Location = new System.Drawing.Point(3, 3);
            this.grdTExpense.Name = "grdTExpense";
            this.grdTExpense.Size = new System.Drawing.Size(570, 253);
            this.grdTExpense.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdTExpense.Styles"));
            this.grdTExpense.TabIndex = 325;
            this.grdTExpense.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdTExpense_CellButtonClick);
            this.grdTExpense.Click += new System.EventHandler(this.grdTExpense_Click);
            this.grdTExpense.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdTExpense_KeyDown);
            this.grdTExpense.Leave += new System.EventHandler(this.grdTExpense_Leave);
            // 
            // lblOriginal
            // 
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.BackColor = System.Drawing.Color.Transparent;
            this.lblOriginal.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginal.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblOriginal.Location = new System.Drawing.Point(494, 5);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(123, 18);
            this.lblOriginal.TabIndex = 377;
            this.lblOriginal.Text = "View Original";
            this.lblOriginal.Visible = false;
            this.lblOriginal.Click += new System.EventHandler(this.lblOriginal_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.BackColor = System.Drawing.Color.Gray;
            this.groupBox20.Location = new System.Drawing.Point(133, 16);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(499, 2);
            this.groupBox20.TabIndex = 379;
            this.groupBox20.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Gray;
            this.label18.Location = new System.Drawing.Point(8, 5);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(121, 14);
            this.label18.TabIndex = 378;
            this.label18.Text = "Guide Settlement";
            // 
            // btnIPreview
            // 
            this.btnIPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIPreview.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnIPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIPreview.Location = new System.Drawing.Point(296, 405);
            this.btnIPreview.Name = "btnIPreview";
            this.btnIPreview.Size = new System.Drawing.Size(73, 33);
            this.btnIPreview.TabIndex = 380;
            this.btnIPreview.Text = "&Preview";
            this.btnIPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIPreview.UseVisualStyleBackColor = true;
            this.btnIPreview.Click += new System.EventHandler(this.btnIPreview_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(446, 405);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(73, 33);
            this.btnDelete.TabIndex = 381;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmGudieSettlement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(617, 452);
            this.ControlBox = false;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnIPreview);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDue);
            this.Controls.Add(this.btnGetTot);
            this.Controls.Add(this.lblTotPay);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmGudieSettlement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmGudieSettlement_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.grpBasics.ResumeLayout(false);
            this.grpBasics.PerformLayout();
            this.grpPayMethod.ResumeLayout(false);
            this.grpPayMethod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCompLogo)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTAdvance)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTExpense)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblGuideName;
        private  Tourist_Management.User_Controls.ComboBox cmbGuide;
        private System.Windows.Forms.Button btnTour;
        private System.Windows.Forms.TextBox txtTourNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDue;
        private System.Windows.Forms.Button btnGetTot;
        private System.Windows.Forms.Label lblTotPay;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtPaidAmt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox grpBasics;
        private System.Windows.Forms.Label lblPaidDate;
        private System.Windows.Forms.DateTimePicker dtpPaidDate;
        private System.Windows.Forms.TextBox txtGuideName;
        private System.Windows.Forms.CheckBox chkPaid;
        private System.Windows.Forms.CheckBox chkConfirm;
        private System.Windows.Forms.PictureBox pbCompLogo;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtFee;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txtLicense;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txtNIC;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private C1.Win.C1FlexGrid.C1FlexGrid grdTExpense;
        private C1.Win.C1FlexGrid.C1FlexGrid grdTAdvance;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkNotPaid;
        private System.Windows.Forms.Button btnIPreview;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grpPayMethod;
        private System.Windows.Forms.TextBox txtChkNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton rdbCash;
        private System.Windows.Forms.RadioButton rdbBank;
        private MasterReports.CachedcrTrialBalance cachedcrTrialBalance1;
    }
}