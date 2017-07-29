namespace Tourist_Management.Email
{
    partial class frmFilterEmail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilterEmail));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.grdContact = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cmbContType = new  Tourist_Management.User_Controls.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShowEmail = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdContact)).BeginInit();
            this.SuspendLayout();
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Location = new System.Drawing.Point(3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Select Contacts";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.grdContact.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes;
            this.grdContact.ColumnInfo = "0,0,0,0,0,85,Columns:";
            this.grdContact.Location = new System.Drawing.Point(3, 29);
            this.grdContact.Name = "grdContact";
            this.grdContact.Size = new System.Drawing.Size(463, 248);
            this.grdContact.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdContact.Styles"));
            this.grdContact.TabIndex = 310;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(388, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 312;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(306, 283);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 311;
            this.btnOk.Text = "&Select";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.cmbContType.DropDownStyle =  System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbContType.FormattingEnabled = true;
            this.cmbContType.Location = new System.Drawing.Point(264, 4);
            this.cmbContType.Name = "cmbContType";
            this.cmbContType.Size = new System.Drawing.Size(189, 21);
            this.cmbContType.TabIndex = 332;
            this.cmbContType.SelectedIndexChanged += new System.EventHandler(this.cmbContType_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(208, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 333;
            this.label4.Text = "Group By";
            this.chkShowEmail.AutoSize = true;
            this.chkShowEmail.Location = new System.Drawing.Point(3, 283);
            this.chkShowEmail.Name = "chkShowEmail";
            this.chkShowEmail.Size = new System.Drawing.Size(133, 17);
            this.chkShowEmail.TabIndex = 334;
            this.chkShowEmail.Text = "Show Email Addresses";
            this.chkShowEmail.UseVisualStyleBackColor = true;
            this.chkShowEmail.CheckedChanged += new System.EventHandler(this.chkShowEmail_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 313);
            this.Controls.Add(this.chkShowEmail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbContType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grdContact);
            this.Controls.Add(this.textBox1);
            this.Name = "frmFilterEmail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Email Addresses";
            this.Load += new System.EventHandler(this.frmFilterEmail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdContact)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdContact;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private  Tourist_Management.User_Controls.ComboBox cmbContType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkShowEmail;
    }
}