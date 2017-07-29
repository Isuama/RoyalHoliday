namespace Tourist_Management.Accounts
{
    partial class frmPaymentsOIE
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
            this.ucPayments1 = new Tourist_Management.User_Controls.ucPayments();
            this.SuspendLayout();
            this.ucPayments1.Location = new System.Drawing.Point(3, 2);
            this.ucPayments1.Mode = 0;
            this.ucPayments1.Name = "ucPayments1";
            this.ucPayments1.Size = new System.Drawing.Size(826, 610);
            this.ucPayments1.SystemCode = 0;
            this.ucPayments1.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 616);
            this.ControlBox = false;
            this.Controls.Add(this.ucPayments1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmPaymentsOIE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPaymentsOIE_Load);
            this.ResumeLayout(false);
        }
        #endregion
        private Tourist_Management.User_Controls.ucPayments ucPayments1;
    }
}