namespace Tourist_Management.Account_Reports
{
    partial class frmRptPaymentIOU
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
            this.ucPaymentsReport1 = new Tourist_Management.User_Controls.ucPaymentsReport();
            this.SuspendLayout();
            this.ucPaymentsReport1.Location = new System.Drawing.Point(1, 2);
            this.ucPaymentsReport1.Name = "ucPaymentsReport1";
            this.ucPaymentsReport1.Size = new System.Drawing.Size(987, 516);
            this.ucPaymentsReport1.TabIndex = 0;
            this.ucPaymentsReport1.Load += new System.EventHandler(this.ucPaymentsReport1_Load);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(995, 517);
            this.ControlBox = false;
            this.Controls.Add(this.ucPaymentsReport1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmRptPaymentIOU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmRptPaymentIOU_Load);
            this.ResumeLayout(false);
        }
        #endregion
        private Tourist_Management.User_Controls.ucPaymentsReport ucPaymentsReport1;
    }
}