﻿namespace Tourist_Management.Reports
{
    partial class frmCheckCancellation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckCancellation));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbType = new  Tourist_Management.User_Controls.ComboBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.rdbConf = new System.Windows.Forms.RadioButton();
            this.rdbNonConfr = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdCancel = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBox1 = new  Tourist_Management.User_Controls.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCancel)).BeginInit();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter";
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(73, 23);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(160, 24);
            this.cmbType.TabIndex = 1;
            this.txtValue.Location = new System.Drawing.Point(241, 25);
            this.txtValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(144, 22);
            this.txtValue.TabIndex = 2;
            this.rdbAll.AutoSize = true;
            this.rdbAll.BackColor = System.Drawing.Color.Transparent;
            this.rdbAll.Checked = true;
            this.rdbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAll.Location = new System.Drawing.Point(23, 22);
            this.rdbAll.Margin = new System.Windows.Forms.Padding(4);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(41, 20);
            this.rdbAll.TabIndex = 3;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All";
            this.rdbAll.UseVisualStyleBackColor = false;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            this.rdbConf.AutoSize = true;
            this.rdbConf.BackColor = System.Drawing.Color.Transparent;
            this.rdbConf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbConf.Location = new System.Drawing.Point(87, 22);
            this.rdbConf.Margin = new System.Windows.Forms.Padding(4);
            this.rdbConf.Name = "rdbConf";
            this.rdbConf.Size = new System.Drawing.Size(71, 20);
            this.rdbConf.TabIndex = 4;
            this.rdbConf.TabStop = true;
            this.rdbConf.Text = "Confirm";
            this.rdbConf.UseVisualStyleBackColor = false;
            this.rdbConf.CheckedChanged += new System.EventHandler(this.rdbConf_CheckedChanged);
            this.rdbNonConfr.AutoSize = true;
            this.rdbNonConfr.BackColor = System.Drawing.Color.Transparent;
            this.rdbNonConfr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbNonConfr.Location = new System.Drawing.Point(166, 22);
            this.rdbNonConfr.Margin = new System.Windows.Forms.Padding(4);
            this.rdbNonConfr.Name = "rdbNonConfr";
            this.rdbNonConfr.Size = new System.Drawing.Size(99, 20);
            this.rdbNonConfr.TabIndex = 5;
            this.rdbNonConfr.TabStop = true;
            this.rdbNonConfr.Text = "Non Confirm";
            this.rdbNonConfr.UseVisualStyleBackColor = false;
            this.rdbNonConfr.CheckedChanged += new System.EventHandler(this.rdbNonConfr_CheckedChanged);
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.rdbAll);
            this.groupBox1.Controls.Add(this.rdbNonConfr);
            this.groupBox1.Controls.Add(this.rdbConf);
            this.groupBox1.Location = new System.Drawing.Point(790, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 52);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.grdCancel.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdCancel.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.grdCancel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCancel.Location = new System.Drawing.Point(12, 74);
            this.grdCancel.Name = "grdCancel";
            this.grdCancel.Size = new System.Drawing.Size(1056, 396);
            this.grdCancel.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCancel.Styles"));
            this.grdCancel.TabIndex = 362;
            this.btnPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreview.BackgroundImage")));
            this.btnPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPreview.Location = new System.Drawing.Point(881, 515);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 33);
            this.btnPreview.TabIndex = 363;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.Location = new System.Drawing.Point(962, 515);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 364;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(589, 25);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 24);
            this.comboBox1.TabIndex = 365;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(520, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 366;
            this.label2.Text = "Company";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1078, 560);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.grdCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmCheckCanselation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCheckCanselation";
            this.Load += new System.EventHandler(this.frmCheckCanselation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Label label1;
        private  Tourist_Management.User_Controls.ComboBox cmbType;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbConf;
        private System.Windows.Forms.RadioButton rdbNonConfr;
        private System.Windows.Forms.GroupBox groupBox1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdCancel;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnCancel;
        private  Tourist_Management.User_Controls.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
    }
}