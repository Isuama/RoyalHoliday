namespace Tourist_Management.Accounts
{
    partial class frmHotelSettlement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHotelSettlement));
            this.grdManageHotel = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.lblHotelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdManageHotel)).BeginInit();
            this.SuspendLayout();
            this.grdManageHotel.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.grdManageHotel.BackColor = System.Drawing.Color.Transparent;
            this.grdManageHotel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.grdManageHotel.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdManageHotel.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.grdManageHotel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdManageHotel.Location = new System.Drawing.Point(8, 37);
            this.grdManageHotel.Name = "grdManageHotel";
            this.grdManageHotel.ShowSort = false;
            this.grdManageHotel.Size = new System.Drawing.Size(752, 367);
            this.grdManageHotel.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdManageHotel.Styles"));
            this.grdManageHotel.TabIndex = 9;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::Tourist_Management.Properties.Resources.close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(689, 412);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 32);
            this.btnCancel.TabIndex = 255;
            this.btnCancel.Text = "&Close";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::Tourist_Management.Properties.Resources.ok;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(615, 412);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 32);
            this.btnSave.TabIndex = 256;
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.groupBox9.BackColor = System.Drawing.Color.Blue;
            this.groupBox9.Location = new System.Drawing.Point(206, 24);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(367, 1);
            this.groupBox9.TabIndex = 421;
            this.groupBox9.TabStop = false;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Lucida Bright", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label23.Location = new System.Drawing.Point(10, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(181, 34);
            this.label23.TabIndex = 420;
            this.label23.Text = "MANAGE PRE-PAYMENTS - ";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHotelName.BackColor = System.Drawing.Color.Transparent;
            this.lblHotelName.Font = new System.Drawing.Font("Lucida Bright", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHotelName.ForeColor = System.Drawing.Color.Blue;
            this.lblHotelName.Location = new System.Drawing.Point(195, 0);
            this.lblHotelName.Name = "lblHotelName";
            this.lblHotelName.Size = new System.Drawing.Size(377, 34);
            this.lblHotelName.TabIndex = 422;
            this.lblHotelName.Text = "\"\"";
            this.lblHotelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(766, 449);
            this.ControlBox = false;
            this.Controls.Add(this.lblHotelName);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grdManageHotel);
            this.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "frmHotelSettlement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmHotelSettlement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdManageHotel)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion
        private C1.Win.C1FlexGrid.C1FlexGrid grdManageHotel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblHotelName;
    }
}