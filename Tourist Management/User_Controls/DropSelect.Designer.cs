namespace Tourist_Management.DropDowns
{
    partial class DropSelect 
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cb = new System.Windows.Forms.TextBox();
            this.B1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.timer1.Interval = 40;
            this.cb.BackColor = System.Drawing.Color.White;
            this.cb.Location = new System.Drawing.Point(0, 0);
            this.cb.Name = "cb";
            this.cb.ReadOnly = true;
            this.cb.Size = new System.Drawing.Size(289, 20);
            this.cb.TabIndex = 4;
            this.B1.Location = new System.Drawing.Point(295, -1);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(21, 21);
            this.B1.TabIndex = 5;
            this.B1.UseVisualStyleBackColor = true;
            this.B1.Click += new System.EventHandler(this.B1_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.B1);
            this.Controls.Add(this.cb);
            this.Name = "DropSelect";
            this.Size = new System.Drawing.Size(332, 23); 
            this.Resize += new System.EventHandler(this.DropSelect_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox cb;
        private System.Windows.Forms.Button B1;
    }
}
