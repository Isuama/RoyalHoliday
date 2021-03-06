﻿namespace Tourist_Management.Transaction
{
    partial class frmChangeRooms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeRooms));
            this.grdCI = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.txtGuest = new System.Windows.Forms.TextBox();
            this.btnTour = new System.Windows.Forms.Button();
            this.txtTourNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdAll = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdCI)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAll)).BeginInit();
            this.SuspendLayout();
            this.grdCI.BackColor = System.Drawing.Color.Transparent;
            this.grdCI.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdCI.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdCI.Location = new System.Drawing.Point(20, 295);
            this.grdCI.Name = "grdCI";
            this.grdCI.Size = new System.Drawing.Size(592, 117);
            this.grdCI.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCI.Styles"));
            this.grdCI.TabIndex = 314;
            this.txtGuest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGuest.Enabled = false;
            this.txtGuest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGuest.Location = new System.Drawing.Point(7, 51);
            this.txtGuest.Name = "txtGuest";
            this.txtGuest.ReadOnly = true;
            this.txtGuest.Size = new System.Drawing.Size(362, 21);
            this.txtGuest.TabIndex = 256;
            this.txtGuest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnTour.Location = new System.Drawing.Point(332, 20);
            this.btnTour.Name = "btnTour";
            this.btnTour.Size = new System.Drawing.Size(37, 23);
            this.btnTour.TabIndex = 0;
            this.btnTour.Text = "...";
            this.btnTour.UseVisualStyleBackColor = true;
            this.txtTourNo.Enabled = false;
            this.txtTourNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTourNo.ForeColor = System.Drawing.Color.Blue;
            this.txtTourNo.Location = new System.Drawing.Point(62, 21);
            this.txtTourNo.Name = "txtTourNo";
            this.txtTourNo.ReadOnly = true;
            this.txtTourNo.Size = new System.Drawing.Size(264, 21);
            this.txtTourNo.TabIndex = 255;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 254;
            this.label1.Text = "Tour ID";
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(15, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 14);
            this.label2.TabIndex = 322;
            this.label2.Text = "SELECT ROOM TYPE/TYPES";
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.txtGuest);
            this.groupBox3.Controls.Add(this.btnTour);
            this.groupBox3.Controls.Add(this.txtTourNo);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(13, 61);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(387, 81);
            this.groupBox3.TabIndex = 321;
            this.groupBox3.TabStop = false;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(575, 428);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 27);
            this.btnCancel.TabIndex = 316;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Image = global::Tourist_Management.Properties.Resources.floppy;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(506, 428);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 27);
            this.btnOk.TabIndex = 315;
            this.btnOk.Text = "&Save";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOk.UseVisualStyleBackColor = true;
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label12.Location = new System.Drawing.Point(13, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 14);
            this.label12.TabIndex = 319;
            this.label12.Text = "SELECT TOUR";
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(7, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(288, 18);
            this.label3.TabIndex = 317;
            this.label3.Text = "Change All Room Types At Once";
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 2);
            this.groupBox1.TabIndex = 318;
            this.groupBox1.TabStop = false;
            this.grdAll.BackColor = System.Drawing.Color.Transparent;
            this.grdAll.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdAll.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdAll.Location = new System.Drawing.Point(13, 177);
            this.grdAll.Name = "grdAll";
            this.grdAll.Size = new System.Drawing.Size(451, 102);
            this.grdAll.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdAll.Styles"));
            this.grdAll.TabIndex = 323;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(669, 477);
            this.ControlBox = false;
            this.Controls.Add(this.grdAll);
            this.Controls.Add(this.grdCI);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmChangeRooms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmChangeRooms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCI)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private C1.Win.C1FlexGrid.C1FlexGrid grdCI;
        private System.Windows.Forms.TextBox txtGuest;
        private System.Windows.Forms.Button btnTour;
        private System.Windows.Forms.TextBox txtTourNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdAll;
    }
}