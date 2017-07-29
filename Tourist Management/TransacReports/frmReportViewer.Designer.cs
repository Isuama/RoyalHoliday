namespace Tourist_Management.TransacReports
{
    partial class frmReportViewer
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
            this.CRViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbEXType = new  Tourist_Management.User_Controls.ComboBox();
            this.SuspendLayout();
            this.CRViewer.ActiveViewIndex = -1;
            this.CRViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CRViewer.Location = new System.Drawing.Point(0, 0);
            this.CRViewer.Name = "CRViewer";
            this.CRViewer.SelectionFormula = "";
            this.CRViewer.Size = new System.Drawing.Size(739, 361);
            this.CRViewer.TabIndex = 0;
            this.CRViewer.ViewTimeSelectionFormula = "";
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(486, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(61, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.cmbEXType.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEXType.FormattingEnabled = true;
            this.cmbEXType.Location = new System.Drawing.Point(359, 3);
            this.cmbEXType.Name = "cmbEXType";
            this.cmbEXType.Size = new System.Drawing.Size(121, 21);
            this.cmbEXType.TabIndex = 2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 361);
            this.Controls.Add(this.cmbEXType);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.CRViewer);
            this.Name = "frmReportViewer";
            this.Text = "Report Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.ResumeLayout(false);
        }
        #endregion
        public CrystalDecisions.Windows.Forms.CrystalReportViewer CRViewer;
        private System.Windows.Forms.Button btnExport;
        private  Tourist_Management.User_Controls.ComboBox cmbEXType;
    }
}