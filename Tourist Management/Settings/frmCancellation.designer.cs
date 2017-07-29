namespace Tourist_Management.Settings
{
    partial class frmCancellation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCancellation));
            this.grdCDate = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grdCDay = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdCDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCDay)).BeginInit();
            this.SuspendLayout();
            this.grdCDate.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdCDate.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdCDate.Location = new System.Drawing.Point(447, 49);
            this.grdCDate.Name = "grdCDate";
            this.grdCDate.Size = new System.Drawing.Size(396, 349);
            this.grdCDate.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCDate.Styles"));
            this.grdCDate.TabIndex = 311;
            this.grdCDate.LeaveCell += new System.EventHandler(this.grdCDate_LeaveCell);
            this.grdCDate.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdCDate_CellButtonClick);
            this.grdCDate.Click += new System.EventHandler(this.grdCDate_Click);
            this.grdCDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdCDate_KeyDown);
            this.grdCDay.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdCDay.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdCDay.Location = new System.Drawing.Point(32, 49);
            this.grdCDay.Name = "grdCDay";
            this.grdCDay.Size = new System.Drawing.Size(382, 349);
            this.grdCDay.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCDay.Styles"));
            this.grdCDay.TabIndex = 310;
            this.grdCDay.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.grdCDay_CellButtonClick);
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Blue;
            this.label26.Location = new System.Drawing.Point(444, 22);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(114, 15);
            this.label26.TabIndex = 313;
            this.label26.Text = "BY DATE RANGE";
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Blue;
            this.label25.Location = new System.Drawing.Point(29, 22);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(105, 15);
            this.label25.TabIndex = 312;
            this.label25.Text = "BY DAY RANGE";
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(764, 431);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 315;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(682, 431);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 314;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(874, 492);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.grdCDate);
            this.Controls.Add(this.grdCDay);
            this.Name = "frmCancellation";
            this.Text = "frmCancellation";
            this.Load += new System.EventHandler(this.frmCansellation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private C1.Win.C1FlexGrid.C1FlexGrid grdCDate;
        private C1.Win.C1FlexGrid.C1FlexGrid grdCDay;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}