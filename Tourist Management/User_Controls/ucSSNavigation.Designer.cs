namespace Tourist_Management.User_Controls
{
    partial class ucSSNavigation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSSNavigation));
            this.grdSE = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.pbSE = new System.Windows.Forms.ProgressBar();
            this.btnSEGenerate = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdSE)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            this.grdSE.AllowAddNew = true;
            this.grdSE.BackColor = System.Drawing.Color.Transparent;
            this.grdSE.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdSE.ColumnInfo = "0,0,0,0,0,90,Columns:";
            this.grdSE.Location = new System.Drawing.Point(12, 49);
            this.grdSE.Name = "grdSE";
            this.grdSE.Rows.Count = 51;
            this.grdSE.Size = new System.Drawing.Size(823, 310);
            this.grdSE.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdSE.Styles"));
            this.grdSE.TabIndex = 14;
            this.groupBox8.BackColor = System.Drawing.Color.Transparent;
            this.groupBox8.Controls.Add(this.pbSE);
            this.groupBox8.Controls.Add(this.btnSEGenerate);
            this.groupBox8.Location = new System.Drawing.Point(10, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(721, 39);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.pbSE.Location = new System.Drawing.Point(104, 11);
            this.pbSE.Name = "pbSE";
            this.pbSE.Size = new System.Drawing.Size(609, 22);
            this.pbSE.TabIndex = 247;
            this.btnSEGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSEGenerate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSEGenerate.Location = new System.Drawing.Point(8, 11);
            this.btnSEGenerate.Name = "btnSEGenerate";
            this.btnSEGenerate.Size = new System.Drawing.Size(87, 22);
            this.btnSEGenerate.TabIndex = 246;
            this.btnSEGenerate.Text = "&Generate";
            this.btnSEGenerate.UseVisualStyleBackColor = true;
            this.btnSEGenerate.Click += new System.EventHandler(this.btnSEGenerate_Click);
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectAll.Location = new System.Drawing.Point(755, 19);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(80, 17);
            this.chkSelectAll.TabIndex = 16;
            this.chkSelectAll.Text = "Check All";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tourist_Management.Properties.Resources.formbak1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.grdSE);
            this.Controls.Add(this.groupBox8);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucSSNavigation";
            this.Size = new System.Drawing.Size(850, 367);
            this.Load += new System.EventHandler(this.ucSSNavigation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSE)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        public C1.Win.C1FlexGrid.C1FlexGrid grdSE;
        public System.Windows.Forms.GroupBox groupBox8;
        public System.Windows.Forms.ProgressBar pbSE;
        public System.Windows.Forms.Button btnSEGenerate;
        public System.Windows.Forms.CheckBox chkSelectAll;
    }
}
