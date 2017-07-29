namespace Tourist_Management.Other
{
    partial class frmSearchGrd
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOpen = new System.Windows.Forms.Button();
            this.dgrd = new System.Windows.Forms.DataGridView();
            this.txttext = new System.Windows.Forms.TextBox();
            this.pnlSrch = new System.Windows.Forms.Panel();
            this.chkSrch = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgrd)).BeginInit();
            this.pnlSrch.SuspendLayout();
            this.SuspendLayout();
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpen.Font = new System.Drawing.Font("Wingdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnOpen.Location = new System.Drawing.Point(273, 1);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "1";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            this.dgrd.AllowUserToAddRows = false;
            this.dgrd.AllowUserToDeleteRows = false;
            this.dgrd.AllowUserToOrderColumns = true;
            this.dgrd.AllowUserToResizeRows = false;
            this.dgrd.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgrd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrd.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgrd.Location = new System.Drawing.Point(-41, 23);
            this.dgrd.Name = "dgrd";
            this.dgrd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgrd.Size = new System.Drawing.Size(337, 165);
            this.dgrd.TabIndex = 8;
            this.dgrd.TabStop = false;
            this.dgrd.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgrd_ColumnHeaderMouseClick);
            this.dgrd.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrd_CellClick);
            this.dgrd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgrd_KeyDown);
            this.txttext.BackColor = System.Drawing.Color.White;
            this.txttext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txttext.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttext.Location = new System.Drawing.Point(1, 1);
            this.txttext.Name = "txttext";
            this.txttext.Size = new System.Drawing.Size(272, 21);
            this.txttext.TabIndex = 5;
            this.txttext.TextChanged += new System.EventHandler(this.txttext_TextChanged);
            this.txttext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txttext_KeyDown);
            this.pnlSrch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSrch.Controls.Add(this.chkSrch);
            this.pnlSrch.Location = new System.Drawing.Point(-2, 189);
            this.pnlSrch.Name = "pnlSrch";
            this.pnlSrch.Size = new System.Drawing.Size(297, 25);
            this.pnlSrch.TabIndex = 9;
            this.chkSrch.AutoSize = true;
            this.chkSrch.Enabled = false;
            this.chkSrch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSrch.Location = new System.Drawing.Point(16, 3);
            this.chkSrch.Name = "chkSrch";
            this.chkSrch.Size = new System.Drawing.Size(117, 17);
            this.chkSrch.TabIndex = 0;
            this.chkSrch.Text = "Searching...........";
            this.chkSrch.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 217);
            this.Controls.Add(this.pnlSrch);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.dgrd);
            this.Controls.Add(this.txttext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSearchGrd";
            this.Text = "frmSearchGrd";
            this.Load += new System.EventHandler(this.frmSearchGrd_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearchGrd_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgrd)).EndInit();
            this.pnlSrch.ResumeLayout(false);
            this.pnlSrch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.DataGridView dgrd;
        private System.Windows.Forms.TextBox txttext;
        private System.Windows.Forms.Panel pnlSrch;
        private System.Windows.Forms.CheckBox chkSrch;
    }
}