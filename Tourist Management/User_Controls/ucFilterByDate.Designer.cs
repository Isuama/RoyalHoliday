namespace Tourist_Management.User_Controls
{
    partial class ucFilterByDate
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
            this.label39 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkIByDate = new System.Windows.Forms.CheckBox();
            this.dtpIToDate = new System.Windows.Forms.DateTimePicker();
            this.label33 = new System.Windows.Forms.Label();
            this.dtpIFromDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label37 = new System.Windows.Forms.Label();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label39.Location = new System.Drawing.Point(3, 7);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(116, 14);
            this.label39.TabIndex = 365;
            this.label39.Text = "FILTER BY DATE";
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label24.Location = new System.Drawing.Point(3, 6);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(116, 14);
            this.label24.TabIndex = 366;
            this.label24.Text = "FILTER BY DATE";
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.chkIByDate);
            this.groupBox7.Controls.Add(this.dtpIToDate);
            this.groupBox7.Controls.Add(this.label33);
            this.groupBox7.Controls.Add(this.dtpIFromDate);
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(3, 9);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(445, 51);
            this.groupBox7.TabIndex = 364;
            this.groupBox7.TabStop = false;
            this.chkIByDate.AutoSize = true;
            this.chkIByDate.Location = new System.Drawing.Point(116, 0);
            this.chkIByDate.Name = "chkIByDate";
            this.chkIByDate.Size = new System.Drawing.Size(15, 14);
            this.chkIByDate.TabIndex = 250;
            this.chkIByDate.UseVisualStyleBackColor = true;
            this.chkIByDate.CheckedChanged += new System.EventHandler(this.chkIByDate_CheckedChanged);
            this.dtpIToDate.Enabled = false;
            this.dtpIToDate.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIToDate.Location = new System.Drawing.Point(288, 20);
            this.dtpIToDate.Name = "dtpIToDate";
            this.dtpIToDate.Size = new System.Drawing.Size(144, 20);
            this.dtpIToDate.TabIndex = 249;
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(231, 23);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(52, 13);
            this.label33.TabIndex = 248;
            this.label33.Text = "Date To";
            this.dtpIFromDate.Enabled = false;
            this.dtpIFromDate.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIFromDate.Location = new System.Drawing.Point(76, 20);
            this.dtpIFromDate.Name = "dtpIFromDate";
            this.dtpIFromDate.Size = new System.Drawing.Size(140, 20);
            this.dtpIFromDate.TabIndex = 247;
            this.groupBox8.BackColor = System.Drawing.Color.Gray;
            this.groupBox8.Location = new System.Drawing.Point(132, 7);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(307, 2);
            this.groupBox8.TabIndex = 356;
            this.groupBox8.TabStop = false;
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(8, 23);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(67, 13);
            this.label37.TabIndex = 246;
            this.label37.Text = "Date From";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.groupBox7);
            this.Name = "ucFilterByDate";
            this.Size = new System.Drawing.Size(453, 65);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label37;
        public System.Windows.Forms.CheckBox chkIByDate;
        public System.Windows.Forms.DateTimePicker dtpIToDate;
        public System.Windows.Forms.DateTimePicker dtpIFromDate;
    }
}
