namespace Tourist_Management.Accounts
{
    partial class frmTDL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTDL));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkByHotel = new System.Windows.Forms.CheckBox();
            this.drpsHotel = new Tourist_Management.DropDowns.DropSelect();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.grdViewer = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnICancel = new System.Windows.Forms.Button();
            this.btnIPreview = new System.Windows.Forms.Button();
            this.ucFilterByCompany1 = new Tourist_Management.User_Controls.ucFilterByCompany();
            this.ucFilterByDate1 = new Tourist_Management.User_Controls.ucFilterByDate();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewer)).BeginInit();
            this.SuspendLayout();
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(4, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(676, 2);
            this.groupBox1.TabIndex = 393;
            this.groupBox1.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 18);
            this.label1.TabIndex = 392;
            this.label1.Text = "TDL REPORT - HOTEL";
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Image = global::Tourist_Management.Properties.Resources.filter;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(429, 117);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(140, 37);
            this.btnFilter.TabIndex = 396;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.chkByHotel);
            this.groupBox2.Controls.Add(this.drpsHotel);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox2.Location = new System.Drawing.Point(112, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 47);
            this.groupBox2.TabIndex = 397;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "BY HOTEL";
            this.chkByHotel.AutoSize = true;
            this.chkByHotel.BackColor = System.Drawing.Color.White;
            this.chkByHotel.Location = new System.Drawing.Point(80, 1);
            this.chkByHotel.Name = "chkByHotel";
            this.chkByHotel.Size = new System.Drawing.Size(15, 14);
            this.chkByHotel.TabIndex = 355;
            this.chkByHotel.UseVisualStyleBackColor = false;
            this.drpsHotel.DataSource = null;
            this.drpsHotel.DropHeight = 332;
            this.drpsHotel.Enabled = false;
            this.drpsHotel.Location = new System.Drawing.Point(19, 18);
            this.drpsHotel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.drpsHotel.Name = "drpsHotel";
            this.drpsHotel.SelectedList = new string[0];
            this.drpsHotel.SetList = "";
            this.drpsHotel.Size = new System.Drawing.Size(277, 20);
            this.drpsHotel.TabIndex = 383;
            this.groupBox6.BackColor = System.Drawing.Color.Gray;
            this.groupBox6.Location = new System.Drawing.Point(93, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 2);
            this.groupBox6.TabIndex = 352;
            this.groupBox6.TabStop = false;
            this.grdViewer.BackColor = System.Drawing.Color.Transparent;
            this.grdViewer.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.grdViewer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdViewer.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdViewer.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdViewer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdViewer.Location = new System.Drawing.Point(12, 160);
            this.grdViewer.Name = "grdViewer";
            this.grdViewer.Size = new System.Drawing.Size(671, 323);
            this.grdViewer.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdViewer.Styles"));
            this.grdViewer.TabIndex = 400;
            this.btnICancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnICancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnICancel.Image = global::Tourist_Management.Properties.Resources.delete;
            this.btnICancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnICancel.Location = new System.Drawing.Point(612, 492);
            this.btnICancel.Name = "btnICancel";
            this.btnICancel.Size = new System.Drawing.Size(72, 30);
            this.btnICancel.TabIndex = 399;
            this.btnICancel.Text = "&Cancel";
            this.btnICancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnICancel.UseVisualStyleBackColor = true;
            this.btnICancel.Click += new System.EventHandler(this.btnICancel_Click);
            this.btnIPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIPreview.Image = global::Tourist_Management.Properties.Resources.search;
            this.btnIPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIPreview.Location = new System.Drawing.Point(534, 492);
            this.btnIPreview.Name = "btnIPreview";
            this.btnIPreview.Size = new System.Drawing.Size(72, 30);
            this.btnIPreview.TabIndex = 398;
            this.btnIPreview.Text = "&Preview";
            this.btnIPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIPreview.UseVisualStyleBackColor = true; 
            this.ucFilterByCompany1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByCompany1.Location = new System.Drawing.Point(459, 51);
            this.ucFilterByCompany1.Name = "ucFilterByCompany1";
            this.ucFilterByCompany1.Size = new System.Drawing.Size(231, 60);
            this.ucFilterByCompany1.TabIndex = 395;
            this.ucFilterByDate1.BackColor = System.Drawing.Color.Transparent;
            this.ucFilterByDate1.Location = new System.Drawing.Point(10, 42);
            this.ucFilterByDate1.Name = "ucFilterByDate1";
            this.ucFilterByDate1.Size = new System.Drawing.Size(453, 65);
            this.ucFilterByDate1.TabIndex = 394;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(688, 526);
            this.ControlBox = false;
            this.Controls.Add(this.grdViewer);
            this.Controls.Add(this.btnICancel);
            this.Controls.Add(this.btnIPreview);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.ucFilterByCompany1);
            this.Controls.Add(this.ucFilterByDate1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTDL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmTDL_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFilter;
        private Tourist_Management.User_Controls.ucFilterByCompany ucFilterByCompany1;
        private Tourist_Management.User_Controls.ucFilterByDate ucFilterByDate1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkByHotel;
        private DropDowns.DropSelect drpsHotel;
        private System.Windows.Forms.GroupBox groupBox6;
        private C1.Win.C1FlexGrid.C1FlexGrid grdViewer;
        private System.Windows.Forms.Button btnICancel;
        private System.Windows.Forms.Button btnIPreview;
    }
}