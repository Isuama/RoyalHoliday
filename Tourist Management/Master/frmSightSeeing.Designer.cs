﻿namespace Tourist_Management.Master
{
    partial class frmSightSeeing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSightSeeing));
            this.tcSightSeeing = new System.Windows.Forms.TabControl();
            this.tpSightDetails = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.drpProvince = new DropDowns.DropSearch();
            this.drpDistrict = new DropDowns.DropSearch();
            this.drpCity = new DropDowns.DropSearch();
            this.drpSSCat = new DropDowns.DropSearch();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.tpDescription = new System.Windows.Forms.TabPage();
            this.txtRemarks = new System.Windows.Forms.RichTextBox();
            this.tpTicket = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.drpCurrency = new DropDowns.DropSearch();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNChild = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNAdult = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSChild = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSAdult = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tpAddPhotos = new System.Windows.Forms.TabPage();
            this.grdPhoto = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tpPhotoView = new System.Windows.Forms.TabPage();
            this.pbMultiPhotos = new System.Windows.Forms.PictureBox();
            this.tvPhotoDesc = new System.Windows.Forms.TreeView();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tcSightSeeing.SuspendLayout();
            this.tpSightDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpDescription.SuspendLayout();
            this.tpTicket.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tpAddPhotos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPhoto)).BeginInit();
            this.tpPhotoView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMultiPhotos)).BeginInit();
            this.SuspendLayout();
            this.tcSightSeeing.Controls.Add(this.tpSightDetails);
            this.tcSightSeeing.Controls.Add(this.tpDescription);
            this.tcSightSeeing.Controls.Add(this.tpTicket);
            this.tcSightSeeing.Controls.Add(this.tpAddPhotos);
            this.tcSightSeeing.Controls.Add(this.tpPhotoView);
            this.tcSightSeeing.Location = new System.Drawing.Point(3, 3);
            this.tcSightSeeing.Name = "tcSightSeeing";
            this.tcSightSeeing.SelectedIndex = 0;
            this.tcSightSeeing.Size = new System.Drawing.Size(363, 298);
            this.tcSightSeeing.TabIndex = 0;
            this.tcSightSeeing.Click += new System.EventHandler(this.tcSightSeeing_Click);
            this.tpSightDetails.Controls.Add(this.groupBox1);
            this.tpSightDetails.Location = new System.Drawing.Point(4, 22);
            this.tpSightDetails.Name = "tpSightDetails";
            this.tpSightDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpSightDetails.Size = new System.Drawing.Size(355, 272);
            this.tpSightDetails.TabIndex = 0;
            this.tpSightDetails.Text = "Detail View";
            this.tpSightDetails.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.drpSSCat);
            this.groupBox1.Controls.Add(this.drpCity);
            this.groupBox1.Controls.Add(this.drpDistrict);
            this.groupBox1.Controls.Add(this.drpProvince);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 275);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.drpProvince.BackColor = System.Drawing.Color.Transparent;
            this.drpProvince.DataSource = null;
            this.drpProvince.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpProvince.FormName = "";
            this.drpProvince.Location = new System.Drawing.Point(126, 19);
            this.drpProvince.Name = "drpProvince";
            this.drpProvince.SelectedText = "";
            this.drpProvince.SelectedValue = "";
            this.drpProvince.Size = new System.Drawing.Size(203, 23);
            this.drpProvince.TabIndex = 0;
            this.drpProvince.Validating += new System.ComponentModel.CancelEventHandler(this.drpProvince_Validating);
            this.drpDistrict.BackColor = System.Drawing.Color.Transparent;
            this.drpDistrict.DataSource = null;
            this.drpDistrict.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpDistrict.FormName = "";
            this.drpDistrict.Location = new System.Drawing.Point(126, 48);
            this.drpDistrict.Name = "drpDistrict";
            this.drpDistrict.SelectedText = "";
            this.drpDistrict.SelectedValue = "";
            this.drpDistrict.Size = new System.Drawing.Size(203, 23);
            this.drpDistrict.TabIndex = 1;
            this.drpDistrict.Validating += new System.ComponentModel.CancelEventHandler(this.drpDistrict_Validating);
            this.drpCity.BackColor = System.Drawing.Color.Transparent;
            this.drpCity.DataSource = null;
            this.drpCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpCity.FormName = "";
            this.drpCity.Location = new System.Drawing.Point(126, 77);
            this.drpCity.Name = "drpCity";
            this.drpCity.SelectedText = "";
            this.drpCity.SelectedValue = "";
            this.drpCity.Size = new System.Drawing.Size(203, 23);
            this.drpCity.TabIndex = 2;
            this.drpCity.Click_Open += new System.EventHandler(this.drpCity_Click_Open);
            this.drpCity.Validating += new System.ComponentModel.CancelEventHandler(this.drpCity_Validating);
            this.drpSSCat.BackColor = System.Drawing.Color.Transparent;
            this.drpSSCat.DataSource = null;
            this.drpSSCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpSSCat.FormName = "";
            this.drpSSCat.Location = new System.Drawing.Point(126, 119);
            this.drpSSCat.Name = "drpSSCat";
            this.drpSSCat.SelectedText = "";
            this.drpSSCat.SelectedValue = "";
            this.drpSSCat.Size = new System.Drawing.Size(203, 23);
            this.drpSSCat.TabIndex = 3;
            this.drpSSCat.Click_Open += new System.EventHandler(this.drpSSCat_Click_Open);
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 13);
            this.label7.TabIndex = 252;
            this.label7.Text = "SightSeeing Category";
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 249;
            this.label5.Text = "Province";
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 247;
            this.label2.Text = "District";
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 246;
            this.label1.Text = "City";
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 242;
            this.label4.Text = "Place Code";
            this.txtName.BackColor = System.Drawing.Color.LightYellow;
            this.txtName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(126, 180);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(203, 21);
            this.txtName.TabIndex = 5;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 209;
            this.label3.Text = "Place Name";
            this.txtCode.BackColor = System.Drawing.Color.LightYellow;
            this.txtCode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(126, 148);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(203, 21);
            this.txtCode.TabIndex = 4;
            this.tpDescription.Controls.Add(this.txtRemarks);
            this.tpDescription.Location = new System.Drawing.Point(4, 22);
            this.tpDescription.Name = "tpDescription";
            this.tpDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tpDescription.Size = new System.Drawing.Size(355, 272);
            this.tpDescription.TabIndex = 4;
            this.tpDescription.Text = "Description";
            this.tpDescription.UseVisualStyleBackColor = true;
            this.txtRemarks.Location = new System.Drawing.Point(7, 7);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(543, 246);
            this.txtRemarks.TabIndex = 0;
            this.txtRemarks.Text = "";
            this.tpTicket.Controls.Add(this.groupBox4);
            this.tpTicket.Controls.Add(this.groupBox3);
            this.tpTicket.Controls.Add(this.groupBox2);
            this.tpTicket.Location = new System.Drawing.Point(4, 22);
            this.tpTicket.Name = "tpTicket";
            this.tpTicket.Padding = new System.Windows.Forms.Padding(3);
            this.tpTicket.Size = new System.Drawing.Size(355, 272);
            this.tpTicket.TabIndex = 3;
            this.tpTicket.Text = "Ticket Cost";
            this.tpTicket.UseVisualStyleBackColor = true;
            this.groupBox4.Controls.Add(this.drpCurrency);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(27, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(294, 55);
            this.groupBox4.TabIndex = 255;
            this.groupBox4.TabStop = false;
            this.drpCurrency.BackColor = System.Drawing.Color.Transparent;
            this.drpCurrency.DataSource = null;
            this.drpCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpCurrency.FormName = "";
            this.drpCurrency.Location = new System.Drawing.Point(72, 18);
            this.drpCurrency.Name = "drpCurrency";
            this.drpCurrency.SelectedText = "";
            this.drpCurrency.SelectedValue = "";
            this.drpCurrency.Size = new System.Drawing.Size(203, 23);
            this.drpCurrency.TabIndex = 0;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 13);
            this.label14.TabIndex = 313;
            this.label14.Text = "Currency";
            this.groupBox3.Controls.Add(this.txtNChild);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtNAdult);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(27, 168);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(294, 91);
            this.groupBox3.TabIndex = 254;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cost For Non SAARC Countries";
            this.txtNChild.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNChild.Location = new System.Drawing.Point(72, 58);
            this.txtNChild.MaxLength = 50;
            this.txtNChild.Multiline = true;
            this.txtNChild.Name = "txtNChild";
            this.txtNChild.Size = new System.Drawing.Size(203, 23);
            this.txtNChild.TabIndex = 1;
            this.txtNChild.Text = "0.00";
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 252;
            this.label10.Text = "Child";
            this.txtNAdult.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNAdult.Location = new System.Drawing.Point(72, 29);
            this.txtNAdult.MaxLength = 50;
            this.txtNAdult.Multiline = true;
            this.txtNAdult.Name = "txtNAdult";
            this.txtNAdult.Size = new System.Drawing.Size(203, 23);
            this.txtNAdult.TabIndex = 0;
            this.txtNAdult.Text = "0.00";
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(13, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 250;
            this.label11.Text = "Adult";
            this.groupBox2.Controls.Add(this.txtSChild);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtSAdult);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(27, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 91);
            this.groupBox2.TabIndex = 218;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cost For SAARC Countries";
            this.txtSChild.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSChild.Location = new System.Drawing.Point(72, 58);
            this.txtSChild.MaxLength = 50;
            this.txtSChild.Multiline = true;
            this.txtSChild.Name = "txtSChild";
            this.txtSChild.Size = new System.Drawing.Size(203, 23);
            this.txtSChild.TabIndex = 1;
            this.txtSChild.Text = "0.00";
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 252;
            this.label8.Text = "Child";
            this.txtSAdult.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSAdult.Location = new System.Drawing.Point(72, 29);
            this.txtSAdult.MaxLength = 50;
            this.txtSAdult.Multiline = true;
            this.txtSAdult.Name = "txtSAdult";
            this.txtSAdult.Size = new System.Drawing.Size(203, 23);
            this.txtSAdult.TabIndex = 0;
            this.txtSAdult.Text = "0.00";
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 250;
            this.label9.Text = "Adult";
            this.tpAddPhotos.Controls.Add(this.grdPhoto);
            this.tpAddPhotos.Location = new System.Drawing.Point(4, 22);
            this.tpAddPhotos.Name = "tpAddPhotos";
            this.tpAddPhotos.Padding = new System.Windows.Forms.Padding(3);
            this.tpAddPhotos.Size = new System.Drawing.Size(355, 272);
            this.tpAddPhotos.TabIndex = 1;
            this.tpAddPhotos.Text = "Add Photos";
            this.tpAddPhotos.UseVisualStyleBackColor = true;
            this.tpAddPhotos.Click += new System.EventHandler(this.tpAddPhotos_Click);
            this.grdPhoto.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdPhoto.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdPhoto.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdPhoto.Location = new System.Drawing.Point(3, 3);
            this.grdPhoto.Name = "grdPhoto";
            this.grdPhoto.Size = new System.Drawing.Size(550, 258);
            this.grdPhoto.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdPhoto.Styles"));
            this.grdPhoto.TabIndex = 0;
            this.grdPhoto.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.ButtonBar;
            this.grdPhoto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdPhoto_KeyDown);
            this.grdPhoto.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdPhoto_CellButtonClick);
            this.grdPhoto.RowColChange += new System.EventHandler(this.grdPhoto_RowColChange);
            this.tpPhotoView.Controls.Add(this.pbMultiPhotos);
            this.tpPhotoView.Controls.Add(this.tvPhotoDesc);
            this.tpPhotoView.Location = new System.Drawing.Point(4, 22);
            this.tpPhotoView.Name = "tpPhotoView";
            this.tpPhotoView.Padding = new System.Windows.Forms.Padding(3);
            this.tpPhotoView.Size = new System.Drawing.Size(355, 272);
            this.tpPhotoView.TabIndex = 2;
            this.tpPhotoView.Text = "View Photos";
            this.tpPhotoView.UseVisualStyleBackColor = true;
            this.pbMultiPhotos.Location = new System.Drawing.Point(159, 6);
            this.pbMultiPhotos.Name = "pbMultiPhotos";
            this.pbMultiPhotos.Size = new System.Drawing.Size(393, 251);
            this.pbMultiPhotos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMultiPhotos.TabIndex = 5;
            this.pbMultiPhotos.TabStop = false;
            this.tvPhotoDesc.Location = new System.Drawing.Point(4, 6);
            this.tvPhotoDesc.Name = "tvPhotoDesc";
            this.tvPhotoDesc.Size = new System.Drawing.Size(149, 251);
            this.tvPhotoDesc.TabIndex = 0;
            this.tvPhotoDesc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPhotoDesc_AfterSelect);
            this.chkActive.AutoSize = true;
            this.chkActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActive.Location = new System.Drawing.Point(134, 316);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.Click += new System.EventHandler(this.chkActive_Click);
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(205, 313);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(287, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 347);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tcSightSeeing);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSightSeeing";
            this.Text = "SightSeeing Details";
            this.Load += new System.EventHandler(this.frmSightSeeing_Load);
            this.tcSightSeeing.ResumeLayout(false);
            this.tpSightDetails.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpDescription.ResumeLayout(false);
            this.tpTicket.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tpAddPhotos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPhoto)).EndInit();
            this.tpPhotoView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMultiPhotos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.TabControl tcSightSeeing;
        private System.Windows.Forms.TabPage tpSightDetails;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TabPage tpAddPhotos;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private DropDowns.DropSearch drpDistrict;
        private DropDowns.DropSearch drpProvince;
        private DropDowns.DropSearch drpCity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpPhotoView;
        private C1.Win.C1FlexGrid.C1FlexGrid grdPhoto;
        private System.Windows.Forms.PictureBox pbMultiPhotos;
        private System.Windows.Forms.TreeView tvPhotoDesc;
        private DropDowns.DropSearch drpSSCat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tpTicket;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSAdult;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSChild;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNChild;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNAdult;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tpDescription;
        private System.Windows.Forms.RichTextBox txtRemarks;
        private System.Windows.Forms.GroupBox groupBox4;
        private DropDowns.DropSearch drpCurrency;
        private System.Windows.Forms.Label label14;
    }
}