namespace Tourist_Management.Other
{
    partial class frmList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmList));
            this.flxGroup = new C1FlexGroup.C1FlexGroup();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.BEdit = new System.Windows.Forms.ToolBarButton();
            this.BRefresh = new System.Windows.Forms.ToolBarButton();
            this.BSearch = new System.Windows.Forms.ToolBarButton();
            this.BPrint = new System.Windows.Forms.ToolBarButton();
            this.BExport = new System.Windows.Forms.ToolBarButton();
            this.BDelete = new System.Windows.Forms.ToolBarButton();
            this.ToolImages = new System.Windows.Forms.ImageList(this.components);
            this.tSearch = new System.Windows.Forms.TextBox();
            this.butSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // flxGroup
            // 
            this.flxGroup.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flxGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flxGroup.ContextMenuStrip = this.cms;
            this.flxGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flxGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            // 
            // 
            // 
            this.flxGroup.Grid.AllowEditing = false;
            this.flxGroup.Grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes;
            this.flxGroup.Grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.flxGroup.Grid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.flxGroup.Grid.ColumnInfo = "10,1,0,0,0,90,Columns:0{Width:17;}\t";
            this.flxGroup.Grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.flxGroup.Grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flxGroup.Grid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.flxGroup.Grid.EditOptions = ((C1.Win.C1FlexGrid.EditFlags)((C1.Win.C1FlexGrid.EditFlags.CycleOnDoubleClick | C1.Win.C1FlexGrid.EditFlags.MultiCheck)));
            this.flxGroup.Grid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flxGroup.Grid.Location = new System.Drawing.Point(0, 36);
            this.flxGroup.Grid.Name = "";
            this.flxGroup.Grid.ShowCursor = true;
            this.flxGroup.Grid.Size = new System.Drawing.Size(863, 398);
            this.flxGroup.Grid.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("flxGroup.Grid.Styles"));
            this.flxGroup.Grid.TabIndex = 1;
            this.flxGroup.Grid.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Symbols;
            this.flxGroup.Grid.DoubleClick += new System.EventHandler(this.flxGroup_Grid_DoubleClick);
            this.flxGroup.Image = null;
            this.flxGroup.Location = new System.Drawing.Point(0, 42);
            this.flxGroup.Name = "flxGroup";
            this.flxGroup.Size = new System.Drawing.Size(867, 438);
            this.flxGroup.TabIndex = 3;
            this.flxGroup.TabStop = false;
            this.flxGroup.AfterSelChange += new C1.Win.C1FlexGrid.RangeEventHandler(this.flxGroup_AfterSelChange);
            // 
            // cms
            // 
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(61, 4);
            this.cms.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cms_ItemClicked);
            // 
            // toolBar
            // 
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.BEdit,
            this.BRefresh,
            this.BSearch,
            this.BPrint,
            this.BExport,
            this.BDelete});
            this.toolBar.ButtonSize = new System.Drawing.Size(50, 50);
            this.toolBar.DropDownArrows = true;
            this.toolBar.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolBar.ImageList = this.ToolImages;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(867, 42);
            this.toolBar.TabIndex = 4;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Text = "Add";
            // 
            // BEdit
            // 
            this.BEdit.ImageIndex = 1;
            this.BEdit.Name = "BEdit";
            this.BEdit.Text = "Edit";
            // 
            // BRefresh
            // 
            this.BRefresh.ImageIndex = 2;
            this.BRefresh.Name = "BRefresh";
            this.BRefresh.Text = "Refresh";
            // 
            // BSearch
            // 
            this.BSearch.ImageIndex = 3;
            this.BSearch.Name = "BSearch";
            this.BSearch.Text = "Search";
            // 
            // BPrint
            // 
            this.BPrint.ImageIndex = 4;
            this.BPrint.Name = "BPrint";
            this.BPrint.Text = "Print";
            // 
            // BExport
            // 
            this.BExport.ImageIndex = 5;
            this.BExport.Name = "BExport";
            this.BExport.Text = "Export";
            // 
            // BDelete
            // 
            this.BDelete.ImageIndex = 6;
            this.BDelete.Name = "BDelete";
            this.BDelete.Text = "Delete";
            // 
            // ToolImages
            // 
            this.ToolImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolImages.ImageStream")));
            this.ToolImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ToolImages.Images.SetKeyName(0, "Add.png");
            this.ToolImages.Images.SetKeyName(1, "Modify.png");
            this.ToolImages.Images.SetKeyName(2, "Loading.png");
            this.ToolImages.Images.SetKeyName(3, "Search.png");
            this.ToolImages.Images.SetKeyName(4, "Print.png");
            this.ToolImages.Images.SetKeyName(5, "Next.png");
            this.ToolImages.Images.SetKeyName(6, "Delete.png");
            // 
            // tSearch
            // 
            this.tSearch.Location = new System.Drawing.Point(330, 13);
            this.tSearch.Name = "tSearch";
            this.tSearch.Size = new System.Drawing.Size(154, 20);
            this.tSearch.TabIndex = 0;
            this.tSearch.TextChanged += new System.EventHandler(this.tSearch_TextChanged);
            // 
            // butSearch
            // 
            this.butSearch.Location = new System.Drawing.Point(490, 11);
            this.butSearch.Name = "butSearch";
            this.butSearch.Size = new System.Drawing.Size(75, 23);
            this.butSearch.TabIndex = 6;
            this.butSearch.Text = "Search";
            this.butSearch.UseVisualStyleBackColor = true;
            this.butSearch.Click += new System.EventHandler(this.tSearch_TextChanged);
            // 
            // frmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(867, 480);
            this.Controls.Add(this.butSearch);
            this.Controls.Add(this.flxGroup);
            this.Controls.Add(this.tSearch);
            this.Controls.Add(this.toolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmList";
            this.Text = "FrmList";
            this.Activated += new System.EventHandler(this.frmList_Activated);
            this.Load += new System.EventHandler(this.frmList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flxGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private C1FlexGroup.C1FlexGroup flxGroup; 
        private System.Windows.Forms.ToolBar toolBar; 
        private System.Windows.Forms.ToolBarButton BEdit;
        private System.Windows.Forms.ToolBarButton BRefresh;
        private System.Windows.Forms.ToolBarButton BSearch;
        private System.Windows.Forms.ToolBarButton BPrint;
        private System.Windows.Forms.ToolBarButton BExport;
        private System.Windows.Forms.ToolBarButton BDelete;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ImageList ToolImages;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.TextBox tSearch;
        private System.Windows.Forms.Button butSearch;
    }
}