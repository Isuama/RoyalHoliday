namespace Tourist_Management.User_Controls
{
    partial class ucNavComponents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucNavComponents));
            this.picParent = new System.Windows.Forms.PictureBox();
            this.lblParent = new System.Windows.Forms.Label();
            this.pnlEnd = new System.Windows.Forms.Panel();
            this.picExpnd = new System.Windows.Forms.PictureBox();
            this.lstvwTree = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.picParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picExpnd)).BeginInit();
            this.SuspendLayout();
            this.picParent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picParent.Location = new System.Drawing.Point(21, 25);
            this.picParent.Name = "picParent";
            this.picParent.Size = new System.Drawing.Size(54, 51);
            this.picParent.TabIndex = 1;
            this.picParent.TabStop = false;
            this.lblParent.AutoSize = true;
            this.lblParent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblParent.Location = new System.Drawing.Point(21, 87);
            this.lblParent.Name = "lblParent";
            this.lblParent.Size = new System.Drawing.Size(38, 13);
            this.lblParent.TabIndex = 2;
            this.lblParent.Text = "Status";
            this.pnlEnd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlEnd.BackgroundImage")));
            this.pnlEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlEnd.Location = new System.Drawing.Point(-163, 149);
            this.pnlEnd.Name = "pnlEnd";
            this.pnlEnd.Size = new System.Drawing.Size(799, 1);
            this.pnlEnd.TabIndex = 3;
            this.picExpnd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picExpnd.BackgroundImage")));
            this.picExpnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExpnd.Location = new System.Drawing.Point(97, 17);
            this.picExpnd.Name = "picExpnd";
            this.picExpnd.Size = new System.Drawing.Size(48, 72);
            this.picExpnd.TabIndex = 4;
            this.picExpnd.TabStop = false;
            this.lstvwTree.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvwTree.AllowColumnReorder = true;
            this.lstvwTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvwTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstvwTree.GridLines = true;
            this.lstvwTree.HoverSelection = true;
            this.lstvwTree.Location = new System.Drawing.Point(146, 22);
            this.lstvwTree.Name = "lstvwTree";
            this.lstvwTree.Size = new System.Drawing.Size(679, 123);
            this.lstvwTree.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstvwTree.TabIndex = 5;
            this.lstvwTree.UseCompatibleStateImageBehavior = false;
            this.lstvwTree.ItemActivate += new System.EventHandler(this.lstvwTree_ItemActivate);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lstvwTree);
            this.Controls.Add(this.picExpnd);
            this.Controls.Add(this.pnlEnd);
            this.Controls.Add(this.lblParent);
            this.Controls.Add(this.picParent);
            this.Name = "ucNavComponents";
            this.Size = new System.Drawing.Size(839, 156);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picExpnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.PictureBox picParent;
        private System.Windows.Forms.Label lblParent;
        private System.Windows.Forms.Panel pnlEnd;
        private System.Windows.Forms.PictureBox picExpnd;
        internal System.Windows.Forms.ListView lstvwTree;
    }
}
