namespace Tourist_Management.User_Controls
{
    partial class ucFilterByOther
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
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.drpOther = new DropDowns.DropSearch();
            this.chkIByOther = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.drpOther);
            this.groupBox9.Controls.Add(this.chkIByOther);
            this.groupBox9.Controls.Add(this.groupBox11);
            this.groupBox9.Location = new System.Drawing.Point(3, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(179, 45);
            this.groupBox9.TabIndex = 353;
            this.groupBox9.TabStop = false;
            this.drpOther.BackColor = System.Drawing.Color.Transparent;
            this.drpOther.DataSource = null;
            this.drpOther.Enabled = false;
            this.drpOther.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpOther.FormName = "";
            this.drpOther.Location = new System.Drawing.Point(12, 19);
            this.drpOther.Name = "drpOther";
            this.drpOther.SelectedText = "";
            this.drpOther.SelectedValue = "";
            this.drpOther.Size = new System.Drawing.Size(155, 23);
            this.drpOther.TabIndex = 349;
            this.chkIByOther.AutoSize = true;
            this.chkIByOther.BackColor = System.Drawing.Color.White;
            this.chkIByOther.Location = new System.Drawing.Point(74, 0);
            this.chkIByOther.Name = "chkIByOther";
            this.chkIByOther.Size = new System.Drawing.Size(15, 14);
            this.chkIByOther.TabIndex = 354;
            this.chkIByOther.UseVisualStyleBackColor = false;
            this.chkIByOther.CheckedChanged += new System.EventHandler(this.chkIByOther_CheckedChanged);
            this.groupBox11.BackColor = System.Drawing.Color.Gray;
            this.groupBox11.Location = new System.Drawing.Point(92, 7);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(207, 2);
            this.groupBox11.TabIndex = 352;
            this.groupBox11.TabStop = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox9);
            this.Name = "ucFilterByOther";
            this.Size = new System.Drawing.Size(185, 50);
            this.Load += new System.EventHandler(this.ucFilterByOther_Load);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox11;
        public DropDowns.DropSearch drpOther;
        public System.Windows.Forms.CheckBox chkIByOther;
    }
}
