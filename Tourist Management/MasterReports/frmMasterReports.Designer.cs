namespace Tourist_Management.Reports
{
    partial class frmMasterReports
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
            this.pan = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.pg = new System.Windows.Forms.PropertyGrid();
            this.cbReports = new  Tourist_Management.User_Controls.ComboBox();
            this.pan.SuspendLayout();
            this.SuspendLayout();
            this.pan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pan.Controls.Add(this.button1);
            this.pan.Controls.Add(this.pg);
            this.pan.Controls.Add(this.cbReports);
            this.pan.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan.Location = new System.Drawing.Point(0, 0);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(228, 429);
            this.pan.TabIndex = 0;
            this.button1.Location = new System.Drawing.Point(12, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(206, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "View Report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.pg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pg.Location = new System.Drawing.Point(12, 68);
            this.pg.Name = "pg";
            this.pg.Size = new System.Drawing.Size(206, 349);
            this.pg.TabIndex = 1;
            this.cbReports.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReports.FormattingEnabled = true;
            this.cbReports.Location = new System.Drawing.Point(12, 12);
            this.cbReports.Name = "cbReports";
            this.cbReports.Size = new System.Drawing.Size(206, 21);
            this.cbReports.TabIndex = 0;
            this.cbReports.SelectedIndexChanged += new System.EventHandler(this.cbReports_SelectedIndexChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 429);
            this.Controls.Add(this.pan);
            this.Name = "frmMasterReports";
            this.Text = "frmMasterReports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMasterReports_Load);
            this.pan.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.PropertyGrid pg;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Panel pan;
        public  Tourist_Management.User_Controls.ComboBox cbReports;
    }
}