namespace Tourist_Management.Reports
{
    partial class frmSetReportCategory
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
            this.lblTrans = new System.Windows.Forms.Label();
            this.drpRptCat = new  Tourist_Management.User_Controls.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.drpRpt = new DropDowns.DropSelect();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblTrans.AutoSize = true;
            this.lblTrans.Location = new System.Drawing.Point(6, 28);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(84, 13);
            this.lblTrans.TabIndex = 217;
            this.lblTrans.Text = "Report Category";
            this.drpRptCat.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpRptCat.FormattingEnabled = true;
            this.drpRptCat.Location = new System.Drawing.Point(99, 26);
            this.drpRptCat.Name = "drpRptCat";
            this.drpRptCat.Size = new System.Drawing.Size(295, 21);
            this.drpRptCat.TabIndex = 215;
            this.drpRptCat.SelectedIndexChanged += new System.EventHandler(this.drpRptCat_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 214;
            this.label4.Text = "Reports";
            this.drpRpt.DataSource = null;
            this.drpRpt.Location = new System.Drawing.Point(98, 61);
            this.drpRpt.Name = "drpRpt";
            this.drpRpt.SelectedList = null;
            this.drpRpt.SetList = "";
            this.drpRpt.Size = new System.Drawing.Size(296, 27);
            this.drpRpt.TabIndex = 216;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(327, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 219;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(245, 108);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 218;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 152);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblTrans);
            this.Controls.Add(this.drpRptCat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.drpRpt);
            this.Name = "frmSetReportCategory";
            this.Text = "frmSetReportCategory";
            this.Load += new System.EventHandler(this.frmSetReportCategory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Label lblTrans;
        private  Tourist_Management.User_Controls.ComboBox drpRptCat;
        private System.Windows.Forms.Label label4;
        private DropDowns.DropSelect drpRpt;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}