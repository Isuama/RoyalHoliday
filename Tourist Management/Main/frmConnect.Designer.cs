namespace Tourist_Management.Main
{
    partial class frmConnect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnect));
            this.tv = new System.Windows.Forms.TreeView();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtserver = new System.Windows.Forms.TextBox();
            this.optUS = new System.Windows.Forms.RadioButton();
            this.optWN = new System.Windows.Forms.RadioButton();
            this.Label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCon = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.chkManual = new System.Windows.Forms.CheckBox();
            this.txtuserID = new System.Windows.Forms.TextBox();
            this.cmbDB = new Tourist_Management.User_Controls.ComboBox();
            this.txtpw = new System.Windows.Forms.TextBox();
            this.imgLst = new System.Windows.Forms.ImageList(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.cachedcrTrialBalance1 = new Tourist_Management.MasterReports.CachedcrTrialBalance();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tv
            // 
            this.tv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tv.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tv.ForeColor = System.Drawing.Color.Black;
            this.tv.Location = new System.Drawing.Point(3, 56);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(204, 193);
            this.tv.TabIndex = 0;
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.btnOk);
            this.GroupBox2.Controls.Add(this.btnCancel);
            this.GroupBox2.Location = new System.Drawing.Point(0, 149);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(340, 50);
            this.GroupBox2.TabIndex = 88;
            this.GroupBox2.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.ok;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(175, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 26);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&OK";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(246, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtserver
            // 
            this.txtserver.Location = new System.Drawing.Point(130, 12);
            this.txtserver.Name = "txtserver";
            this.txtserver.Size = new System.Drawing.Size(183, 20);
            this.txtserver.TabIndex = 1;
            // 
            // optUS
            // 
            this.optUS.AutoSize = true;
            this.optUS.Checked = true;
            this.optUS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.optUS.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optUS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.optUS.Location = new System.Drawing.Point(199, 42);
            this.optUS.Name = "optUS";
            this.optUS.Size = new System.Drawing.Size(56, 19);
            this.optUS.TabIndex = 3;
            this.optUS.TabStop = true;
            this.optUS.Text = "User";
            this.optUS.UseVisualStyleBackColor = true;
            // 
            // optWN
            // 
            this.optWN.AutoSize = true;
            this.optWN.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.optWN.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optWN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.optWN.Location = new System.Drawing.Point(112, 42);
            this.optWN.Name = "optWN";
            this.optWN.Size = new System.Drawing.Size(79, 19);
            this.optWN.TabIndex = 2;
            this.optWN.Text = "Windows";
            this.optWN.UseVisualStyleBackColor = true;
            this.optWN.CheckedChanged += new System.EventHandler(this.optWN_CheckedChanged);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Label6.Location = new System.Drawing.Point(24, 41);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(79, 14);
            this.Label6.TabIndex = 14;
            this.Label6.Text = "Auth... Mode ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 22);
            this.label1.TabIndex = 96;
            this.label1.Text = "INFOSURV LANKA";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Label5.Location = new System.Drawing.Point(24, 124);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(58, 14);
            this.Label5.TabIndex = 13;
            this.Label5.Text = "Database";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Label4.Location = new System.Drawing.Point(24, 95);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(58, 14);
            this.Label4.TabIndex = 12;
            this.Label4.Text = "Password";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Label3.Location = new System.Drawing.Point(24, 66);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(47, 14);
            this.Label3.TabIndex = 11;
            this.Label3.Text = "User ID";
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.btnCon);
            this.GroupBox1.Controls.Add(this.optUS);
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.optWN);
            this.GroupBox1.Controls.Add(this.txtserver);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.chkManual);
            this.GroupBox1.Controls.Add(this.txtuserID);
            this.GroupBox1.Controls.Add(this.cmbDB);
            this.GroupBox1.Controls.Add(this.txtpw);
            this.GroupBox1.Location = new System.Drawing.Point(190, 50);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(325, 200);
            this.GroupBox1.TabIndex = 94;
            this.GroupBox1.TabStop = false;
            // 
            // btnCon
            // 
            this.btnCon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCon.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCon.ForeColor = System.Drawing.Color.Black;
            this.btnCon.Image = global::Tourist_Management.Properties.Resources.config;
            this.btnCon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCon.Location = new System.Drawing.Point(238, 92);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(81, 26);
            this.btnCon.TabIndex = 6;
            this.btnCon.Text = "Co&nnect";
            this.btnCon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCon.Click += new System.EventHandler(this.btnCon_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Label2.Location = new System.Drawing.Point(24, 16);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(82, 14);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "Server Manual";
            // 
            // chkManual
            // 
            this.chkManual.AutoSize = true;
            this.chkManual.Location = new System.Drawing.Point(112, 16);
            this.chkManual.Name = "chkManual";
            this.chkManual.Size = new System.Drawing.Size(15, 14);
            this.chkManual.TabIndex = 0;
            this.chkManual.UseVisualStyleBackColor = true;
            this.chkManual.CheckedChanged += new System.EventHandler(this.chkManual_CheckedChanged);
            // 
            // txtuserID
            // 
            this.txtuserID.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtuserID.Location = new System.Drawing.Point(112, 66);
            this.txtuserID.Name = "txtuserID";
            this.txtuserID.Size = new System.Drawing.Size(202, 20);
            this.txtuserID.TabIndex = 4;
            // 
            // cmbDB
            // 
            this.cmbDB.DataSource = null;
            this.cmbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbDB.FormattingEnabled = true;
            this.cmbDB.Location = new System.Drawing.Point(112, 120);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new System.Drawing.Size(202, 21);
            this.cmbDB.TabIndex = 7;
            // 
            // txtpw
            // 
            this.txtpw.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpw.Location = new System.Drawing.Point(112, 93);
            this.txtpw.Name = "txtpw";
            this.txtpw.PasswordChar = '*';
            this.txtpw.Size = new System.Drawing.Size(120, 20);
            this.txtpw.TabIndex = 5;
            // 
            // imgLst
            // 
            this.imgLst.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLst.ImageStream")));
            this.imgLst.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLst.Images.SetKeyName(0, "TreeMain2.bmp");
            this.imgLst.Images.SetKeyName(1, "SVR2.bmp");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(5, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 14);
            this.label7.TabIndex = 97;
            this.label7.Text = "Everything is nearby";
            // 
            // frmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(516, 251);
            this.ControlBox = false;
            this.Controls.Add(this.tv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.label7);
            this.Name = "frmConnect";
            this.Text = "Connect To The Database";
            this.Load += new System.EventHandler(this.frmConnect_Load);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TreeView tv;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox txtserver;
        internal System.Windows.Forms.RadioButton optUS;
        internal System.Windows.Forms.RadioButton optWN;
        internal System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.CheckBox chkManual;
        internal System.Windows.Forms.TextBox txtuserID;
        internal  Tourist_Management.User_Controls.ComboBox cmbDB;
        internal System.Windows.Forms.TextBox txtpw;
        private System.Windows.Forms.ImageList imgLst;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button btnCon;
        private MasterReports.CachedcrTrialBalance cachedcrTrialBalance1;
    }
}