namespace Tourist_Management.User_Controls
{
    partial class ucFilterByCompany
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
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.chkICmpny = new System.Windows.Forms.CheckBox();
            this.cmbICompany = new  Tourist_Management.User_Controls.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox31.SuspendLayout();
            this.SuspendLayout();
            this.groupBox31.BackColor = System.Drawing.Color.Transparent;
            this.groupBox31.Controls.Add(this.chkICmpny);
            this.groupBox31.Controls.Add(this.cmbICompany);
            this.groupBox31.Controls.Add(this.label36);
            this.groupBox31.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox31.Location = new System.Drawing.Point(3, 0);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(222, 51);
            this.groupBox31.TabIndex = 361;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "BY COMPANY";
            this.chkICmpny.AutoSize = true;
            this.chkICmpny.Location = new System.Drawing.Point(105, 0);
            this.chkICmpny.Name = "chkICmpny";
            this.chkICmpny.Size = new System.Drawing.Size(15, 14);
            this.chkICmpny.TabIndex = 381;
            this.chkICmpny.UseVisualStyleBackColor = true;
            this.chkICmpny.CheckedChanged += new System.EventHandler(this.chkICmpny_CheckedChanged);
            this.cmbICompany.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbICompany.Enabled = false;
            this.cmbICompany.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbICompany.FormattingEnabled = true;
            this.cmbICompany.Location = new System.Drawing.Point(77, 18);
            this.cmbICompany.Name = "cmbICompany";
            this.cmbICompany.Size = new System.Drawing.Size(135, 21);
            this.cmbICompany.TabIndex = 380;
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(7, 22);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(62, 13);
            this.label36.TabIndex = 379;
            this.label36.Text = "Company";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox31);
            this.Name = "ucFilterByCompany";
            this.Size = new System.Drawing.Size(231, 60);
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.Label label36;
        public System.Windows.Forms.CheckBox chkICmpny;
        public  Tourist_Management.User_Controls.ComboBox cmbICompany;
    }
}
