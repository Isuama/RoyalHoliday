namespace Tourist_Management.User_Controls
{
    partial class ucReportViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportViewer));
            this.flxGroup = new C1FlexGroup.C1FlexGroup();
            this.ToolImages = new System.Windows.Forms.ImageList(this.components);
            this.toolBarRP = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.BEdit = new System.Windows.Forms.ToolBarButton();
            this.BRefresh = new System.Windows.Forms.ToolBarButton();
            this.BSearch = new System.Windows.Forms.ToolBarButton();
            this.BPrint = new System.Windows.Forms.ToolBarButton();
            this.BExport = new System.Windows.Forms.ToolBarButton();
            this.BDelete = new System.Windows.Forms.ToolBarButton();
            this.tbSetPrint = new System.Windows.Forms.ToolBarButton();
            this.tbMemo = new System.Windows.Forms.ToolBarButton();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup.Grid)).BeginInit();
            this.SuspendLayout();
            this.flxGroup.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flxGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flxGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.flxGroup.Grid.AllowEditing = false;
            this.flxGroup.Grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes;
            this.flxGroup.Grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.flxGroup.Grid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.flxGroup.Grid.ColumnInfo = "10,1,0,0,0,90,Columns:0{Width:17;}\t";
            this.flxGroup.Grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.flxGroup.Grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flxGroup.Grid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.flxGroup.Grid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flxGroup.Grid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.flxGroup.Grid.Location = new System.Drawing.Point(0, 36);
            this.flxGroup.Grid.Name = "";
            this.flxGroup.Grid.Rows.Fixed = 2;
            this.flxGroup.Grid.ShowCursor = true;
            this.flxGroup.Grid.Size = new System.Drawing.Size(567, 175);
            this.flxGroup.Grid.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("flxGroup.Grid.Styles"));
            this.flxGroup.Grid.TabIndex = 1;
            this.flxGroup.Grid.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Symbols;
            this.flxGroup.Image = null;
            this.flxGroup.Location = new System.Drawing.Point(3, 49);
            this.flxGroup.Name = "flxGroup";
            this.flxGroup.Size = new System.Drawing.Size(571, 215);
            this.flxGroup.TabIndex = 5;
            this.flxGroup.TabStop = false;
            this.ToolImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolImages.ImageStream")));
            this.ToolImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ToolImages.Images.SetKeyName(0, "delete.png");
            this.ToolImages.Images.SetKeyName(1, "export.png");
            this.ToolImages.Images.SetKeyName(2, "memorize report.png");
            this.ToolImages.Images.SetKeyName(3, "page setup.png");
            this.ToolImages.Images.SetKeyName(4, "print areat.png");
            this.ToolImages.Images.SetKeyName(5, "print priveew.png");
            this.ToolImages.Images.SetKeyName(6, "print.png");
            this.ToolImages.Images.SetKeyName(7, "refresh.png");
            this.ToolImages.Images.SetKeyName(8, "search.png");
            this.toolBarRP.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarRP.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.BEdit,
            this.BRefresh,
            this.BSearch,
            this.BPrint,
            this.BExport,
            this.BDelete,
            this.tbSetPrint,
            this.tbMemo});
            this.toolBarRP.ButtonSize = new System.Drawing.Size(50, 50);
            this.toolBarRP.DropDownArrows = true;
            this.toolBarRP.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolBarRP.ImageList = this.ToolImages;
            this.toolBarRP.Location = new System.Drawing.Point(0, 0);
            this.toolBarRP.Name = "toolBarRP";
            this.toolBarRP.ShowToolTips = true;
            this.toolBarRP.Size = new System.Drawing.Size(1144, 42);
            this.toolBarRP.TabIndex = 6;
            this.toolBarRP.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarRP_ButtonClick);
            this.toolBarButton1.ImageKey = "page setup.png";
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Text = "Setup";
            this.BEdit.ImageKey = "print priveew.png";
            this.BEdit.Name = "BEdit";
            this.BEdit.Text = "Preview";
            this.BRefresh.ImageKey = "refresh.png";
            this.BRefresh.Name = "BRefresh";
            this.BRefresh.Text = "Refresh";
            this.BSearch.ImageKey = "print.png";
            this.BSearch.Name = "BSearch";
            this.BSearch.Text = "Printer";
            this.BPrint.ImageKey = "print.png";
            this.BPrint.Name = "BPrint";
            this.BPrint.Text = "Format";
            this.BExport.ImageKey = "export.png";
            this.BExport.Name = "BExport";
            this.BExport.Text = "Export";
            this.BDelete.ImageKey = "delete.png";
            this.BDelete.Name = "BDelete";
            this.BDelete.Text = "Close";
            this.tbSetPrint.ImageKey = "print areat.png";
            this.tbSetPrint.Name = "tbSetPrint";
            this.tbSetPrint.Text = "SetArea";
            this.tbMemo.ImageKey = "memorize report.png";
            this.tbMemo.Name = "tbMemo";
            this.tbMemo.Text = "Memo";
            this.fontDialog1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolBarRP);
            this.Controls.Add(this.flxGroup);
            this.Name = "ucReportViewer";
            this.Size = new System.Drawing.Size(1144, 629);
            this.Load += new System.EventHandler(this.ucReportViewer_Load);
            this.Resize += new System.EventHandler(this.ucReportViewer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private C1FlexGroup.C1FlexGroup flxGroup;
        internal System.Windows.Forms.ImageList ToolImages;
        private System.Windows.Forms.ToolBar toolBarRP; 
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ToolBarButton tbSetPrint;
        private System.Windows.Forms.ToolBarButton tbMemo; 
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton BEdit;
        private System.Windows.Forms.ToolBarButton BRefresh;
        private System.Windows.Forms.ToolBarButton BSearch;
        private System.Windows.Forms.ToolBarButton BPrint;
        private System.Windows.Forms.ToolBarButton BExport;
        private System.Windows.Forms.ToolBarButton BDelete;
    }
}
