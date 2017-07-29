﻿namespace Tourist_Management.Main
{
    partial class frmUserGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserGroups));
            this.imgTree = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTreeView = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.treeMain = new System.Windows.Forms.TreeView();
            this.grpUser = new System.Windows.Forms.GroupBox();
            this.chkIsAdmin = new System.Windows.Forms.CheckBox();
            this.txtGroupDesc = new System.Windows.Forms.TextBox();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flx = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.lblUserDetails = new System.Windows.Forms.Label();
            this.btnTreeView = new System.Windows.Forms.Button();
            this.btnUserDetails = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.grpUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flx)).BeginInit();
            this.SuspendLayout();
            this.imgTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgTree.ImageSize = new System.Drawing.Size(20, 20);
            this.imgTree.TransparentColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(701, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 214;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(619, 373);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 213;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.chkActive.AutoSize = true;
            this.chkActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActive.Location = new System.Drawing.Point(539, 376);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(65, 17);
            this.chkActive.TabIndex = 215;
            this.chkActive.Text = "IsActive";
            this.chkActive.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.lblTreeView);
            this.groupBox1.Controls.Add(this.pnlMain);
            this.groupBox1.Controls.Add(this.flx);
            this.groupBox1.Controls.Add(this.lblUserDetails);
            this.groupBox1.Controls.Add(this.btnTreeView);
            this.groupBox1.Controls.Add(this.btnUserDetails);
            this.groupBox1.Location = new System.Drawing.Point(5, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(796, 364);
            this.groupBox1.TabIndex = 216;
            this.groupBox1.TabStop = false;
            this.lblTreeView.AutoSize = true;
            this.lblTreeView.BackColor = System.Drawing.Color.White;
            this.lblTreeView.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblTreeView.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTreeView.Location = new System.Drawing.Point(16, 47);
            this.lblTreeView.Name = "lblTreeView";
            this.lblTreeView.Size = new System.Drawing.Size(65, 13);
            this.lblTreeView.TabIndex = 13;
            this.lblTreeView.Text = "Access Tree";
            this.lblTreeView.Click += new System.EventHandler(this.lblTreeView_Click);
            this.pnlMain.Controls.Add(this.treeMain);
            this.pnlMain.Controls.Add(this.grpUser);
            this.pnlMain.Location = new System.Drawing.Point(8, 69);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(264, 281);
            this.pnlMain.TabIndex = 11;
            this.treeMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeMain.Location = new System.Drawing.Point(8, 143);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(254, 130);
            this.treeMain.TabIndex = 221;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.grpUser.BackColor = System.Drawing.Color.Transparent;
            this.grpUser.Controls.Add(this.chkIsAdmin);
            this.grpUser.Controls.Add(this.txtGroupDesc);
            this.grpUser.Controls.Add(this.txtGroupName);
            this.grpUser.Controls.Add(this.label3);
            this.grpUser.Controls.Add(this.label4);
            this.grpUser.Location = new System.Drawing.Point(6, 3);
            this.grpUser.Name = "grpUser";
            this.grpUser.Size = new System.Drawing.Size(251, 144);
            this.grpUser.TabIndex = 220;
            this.grpUser.TabStop = false;
            this.chkIsAdmin.AutoSize = true;
            this.chkIsAdmin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsAdmin.Location = new System.Drawing.Point(9, 118);
            this.chkIsAdmin.Name = "chkIsAdmin";
            this.chkIsAdmin.Size = new System.Drawing.Size(64, 17);
            this.chkIsAdmin.TabIndex = 223;
            this.chkIsAdmin.Text = "IsAdmin";
            this.chkIsAdmin.UseVisualStyleBackColor = true;
            this.txtGroupDesc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupDesc.Location = new System.Drawing.Point(7, 86);
            this.txtGroupDesc.MaxLength = 50;
            this.txtGroupDesc.Name = "txtGroupDesc";
            this.txtGroupDesc.Size = new System.Drawing.Size(239, 21);
            this.txtGroupDesc.TabIndex = 210;
            this.txtGroupName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupName.Location = new System.Drawing.Point(7, 33);
            this.txtGroupName.MaxLength = 100;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(239, 21);
            this.txtGroupName.TabIndex = 209;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 207;
            this.label3.Text = "Description";
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 206;
            this.label4.Text = "Name";
            this.flx.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.flx.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.flx.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.Light3D;
            this.flx.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.flx.Location = new System.Drawing.Point(276, 17);
            this.flx.Name = "flx";
            this.flx.Size = new System.Drawing.Size(507, 337);
            this.flx.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("flx.Styles"));
            this.flx.TabIndex = 8;
            this.flx.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.flx_AfterEdit);
            this.lblUserDetails.AutoSize = true;
            this.lblUserDetails.BackColor = System.Drawing.Color.White;
            this.lblUserDetails.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblUserDetails.ForeColor = System.Drawing.Color.DarkGray;
            this.lblUserDetails.Location = new System.Drawing.Point(14, 24);
            this.lblUserDetails.Name = "lblUserDetails";
            this.lblUserDetails.Size = new System.Drawing.Size(88, 13);
            this.lblUserDetails.TabIndex = 12;
            this.lblUserDetails.Text = "User Information";
            this.lblUserDetails.Click += new System.EventHandler(this.lblUserDetails_Click);
            this.btnTreeView.BackColor = System.Drawing.Color.White;
            this.btnTreeView.Font = new System.Drawing.Font("Webdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnTreeView.ForeColor = System.Drawing.Color.DarkGray;
            this.btnTreeView.Location = new System.Drawing.Point(12, 42);
            this.btnTreeView.Name = "btnTreeView";
            this.btnTreeView.Size = new System.Drawing.Size(259, 24);
            this.btnTreeView.TabIndex = 10;
            this.btnTreeView.Text = "";
            this.btnTreeView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTreeView.UseVisualStyleBackColor = false;
            this.btnTreeView.Click += new System.EventHandler(this.btnTreeView_Click);
            this.btnUserDetails.BackColor = System.Drawing.Color.White;
            this.btnUserDetails.Font = new System.Drawing.Font("Webdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUserDetails.ForeColor = System.Drawing.Color.DarkGray;
            this.btnUserDetails.Location = new System.Drawing.Point(12, 17);
            this.btnUserDetails.Name = "btnUserDetails";
            this.btnUserDetails.Size = new System.Drawing.Size(259, 24);
            this.btnUserDetails.TabIndex = 9;
            this.btnUserDetails.Text = "4";
            this.btnUserDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUserDetails.UseVisualStyleBackColor = false;
            this.btnUserDetails.Click += new System.EventHandler(this.btnUserDetails_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(810, 409);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmUserGroups";
            this.Text = "frmUserGroups";
            this.Load += new System.EventHandler(this.frmUserGroups_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.grpUser.ResumeLayout(false);
            this.grpUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.ImageList imgTree;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTreeView;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.GroupBox grpUser;
        private System.Windows.Forms.TextBox txtGroupDesc;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private C1.Win.C1FlexGrid.C1FlexGrid flx;
        private System.Windows.Forms.Label lblUserDetails;
        private System.Windows.Forms.Button btnTreeView;
        private System.Windows.Forms.Button btnUserDetails;
        private System.Windows.Forms.CheckBox chkIsAdmin;
    }
}