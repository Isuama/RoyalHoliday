namespace Tourist_Management.Transaction
{
    partial class frmItinerary
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dtpDeparture = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpArrival = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.drpDriver = new DropDowns.DropSearch();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            this.groupBox4.Controls.Add(this.drpDriver);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.chkAll);
            this.groupBox4.Controls.Add(this.dtpDeparture);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.dtpArrival);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(5, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(514, 112);
            this.groupBox4.TabIndex = 328;
            this.groupBox4.TabStop = false;
            this.dtpDeparture.Location = new System.Drawing.Point(338, 22);
            this.dtpDeparture.Name = "dtpDeparture";
            this.dtpDeparture.Size = new System.Drawing.Size(157, 20);
            this.dtpDeparture.TabIndex = 251;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(274, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 250;
            this.label10.Text = "Date To";
            this.dtpArrival.Location = new System.Drawing.Point(79, 23);
            this.dtpArrival.Name = "dtpArrival";
            this.dtpArrival.Size = new System.Drawing.Size(154, 20);
            this.dtpArrival.TabIndex = 249;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 246;
            this.label9.Text = "Date From";
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(444, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 343;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnPrint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(363, 124);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 342;
            this.btnPrint.Text = "&Preview";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.drpDriver.BackColor = System.Drawing.Color.Transparent;
            this.drpDriver.DataSource = null;
            this.drpDriver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpDriver.FormName = "";
            this.drpDriver.Location = new System.Drawing.Point(79, 56);
            this.drpDriver.Name = "drpDriver";
            this.drpDriver.SelectedText = "";
            this.drpDriver.SelectedValue = "";
            this.drpDriver.Size = new System.Drawing.Size(416, 23);
            this.drpDriver.TabIndex = 348;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 347;
            this.label2.Text = "Hotel Name";
            this.chkAll.AutoSize = true;
            this.chkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkAll.Location = new System.Drawing.Point(83, 85);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(80, 19);
            this.chkAll.TabIndex = 349;
            this.chkAll.Text = "All Drivers";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 155);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox4);
            this.Name = "frmItinerary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itinerary Report";
            this.Load += new System.EventHandler(this.frmItinerary_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DateTimePicker dtpDeparture;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpArrival;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        private DropDowns.DropSearch drpDriver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAll;
    }
}