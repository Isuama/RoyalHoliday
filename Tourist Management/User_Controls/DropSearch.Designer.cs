namespace Tourist_Management.DropDowns
{
    partial class DropSearch
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
            this.B2 = new System.Windows.Forms.Button();
            this.cb = new System.Windows.Forms.TextBox();
            this.B1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.B2.Location = new System.Drawing.Point(313, 0);
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(21, 21);
            this.B2.TabIndex = 2;
            this.B2.UseVisualStyleBackColor = true;
            this.B2.Click += new System.EventHandler(this.B2_Click);
            this.cb.BackColor = System.Drawing.Color.White;
            this.cb.Location = new System.Drawing.Point(0, 0);
            this.cb.Name = "cb";
            this.cb.ReadOnly = true;
            this.cb.Size = new System.Drawing.Size(289, 20);
            this.cb.TabIndex = 3;
            this.cb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cb_MouseDown);
            this.B1.Location = new System.Drawing.Point(291, 0);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(21, 21);
            this.B1.TabIndex = 4;
            this.B1.UseVisualStyleBackColor = true;
            this.B1.Click += new System.EventHandler(this.B1_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.B1);
            this.Controls.Add(this.cb);
            this.Controls.Add(this.B2);
            this.Name = "DropSearch";
            this.Size = new System.Drawing.Size(333, 21);
            this.Click += new System.EventHandler(this.B1_Click);
            this.Resize += new System.EventHandler(this.DropSearch_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Button B2;
        private System.Windows.Forms.TextBox cb;
        private System.Windows.Forms.Button B1;
    }
}
