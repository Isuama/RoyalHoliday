using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
namespace Tourist_Management.Main
{
    public partial class frmMDIMain : Form
    {
        public static frmMDIMain MDI;
        private const string msghd = "Sytem Main";
        private Boolean blnLoad = false;
        private int ImgIndex = -1;
        private Tourist_Management.Other.frmNavigator frmNG= new Other.frmNavigator ();
        ToolStripMenuItem ParentItem = new ToolStripMenuItem();
        private bool isLeft=true;
        public bool isTree=true;
        private bool isRecent = true;
        ToolStripMenuItem menuItemLeft;
        ToolStripMenuItem menuItemTree;
        ToolStripMenuItem menuItemRecent;
        CheckBox chkLeft;
        CheckBox chkTree;
        CheckBox chkRecent;
        CheckBox cb = new CheckBox();
        TreeNode selectedNode = new TreeNode();
        TreeNode ActiveNode = new TreeNode();
        public frmMDIMain()
        {
            InitializeComponent();
            Icon = Properties.Resources.iiApplicaton;
            MDI = this;
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void frmMDIMain_Load(object sender, EventArgs e)
        {
            try
            {
                blnLoad = true;
                if (Tourist_Management.Classes.clsGlobal.AllowLog == false)
                {
                    Application.Exit();
                }
                arrage_Controlls();
                intialize_Grd();
                Set_Status_Bar();
                blnLoad = false;
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        void Application_ApplicationExit(object sender, EventArgs e)
        {
        }
        public void Reminder_Thread()
        {
        }
        private void arrage_Controlls()
        {
            try
            {
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-38;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            tabMain.Visible = false;
            Make_tree();
            Make_menu();
            Toolbar();
            Set_Panels();
            Set_tree_Selected();
            pnlTime.Left = this.Width - pnlTime.Width;
            pbLock.Left = this.Width - pnlTime.Width;
            tmerMain.Start();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Make_tree()
        {
            try
            {
                DataTable dt;
                string ssql="";
                TreeNode trNode = new TreeNode("Tourist Management System");
                if (Classes.clsGlobal.Is_SuperUser == true)
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](0,1,-1) Where ISNULL(ParentID,0)=0 Order by SortOrder ";
                }
                else if (Classes.clsGlobal.Is_Admin == true)
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](0,0,0) Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=0  and ISNULL(IsCritical,0)=0  Order by SortOrder ";
                }
                else 
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](" + Classes.clsGlobal.UserID.ToString() + ",0,0) Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=0  and ISNULL(IsCritical,0)=0  and GroupID=" + Classes.clsGlobal.UserID.ToString() + " Order by SortOrder ";
                }
                dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                treeMain.Nodes.Add(trNode);
                treeMain.ImageList=imgTree;
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode trn = new TreeNode(dr["Desc"].ToString());
                    trn.Name = dr["Name"].ToString();
                    imgTree.Images.Add(Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])dr["Img"]));
                    ImgIndex += 1;
                    trn.ImageIndex = ImgIndex;
                    trn.SelectedImageIndex = ImgIndex;
                    trNode.Nodes.Add(trn);
                    Add_SubNodes(trn, System.Convert.ToInt16(dr["ModuleID"].ToString()));
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Add_SubNodes(TreeNode trn,int modID)
        {
            string ssql="";
            DataTable DT;
            try
            {
                if (Classes.clsGlobal.Is_SuperUser == true)
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](0,1,-1) Where ISNULL(ParentID,0)=" + modID + " Order by SortOrder ";
                }
                else if (Classes.clsGlobal.Is_Admin == true)
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](0,0,0) Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=" + modID + "  and ISNULL(IsCritical,0)=0  Order by SortOrder ";
                }
                else 
                {
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder FROM dbo.[Fun_ReturnUserModule](" + Classes.clsGlobal.UserID.ToString() + ",0,0) Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=" + modID + "  and ISNULL(IsCritical,0)=0  and Isnull([View],0)=1 and GroupID=" + Classes.clsGlobal.UserID.ToString() + " Order by SortOrder ";
                }
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                foreach (DataRow dr in DT.Rows)
                {
                    TreeNode TRN= new TreeNode(dr["Desc"].ToString());
                    TRN.Name = dr["Name"].ToString();
                    if ((dr[4].ToString() != null) && (dr[4].ToString() != ""))
                    {
                        imgTree.Images.Add(Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])dr["Img"]));
                        ImgIndex += 1;
                        TRN.ImageIndex = ImgIndex;
                        TRN.SelectedImageIndex = ImgIndex;
                    }
                    trn.Nodes.Add(TRN);
                    Add_SubNodes(TRN, System.Convert.ToInt16(dr["ModuleID"].ToString()));
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Make_menu()
        {
            int ItemNo;
            DataTable DT=null;
            try
            {
                if (Classes.clsGlobal.Is_SuperUser == true)
                {
                    DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ID,Module,[Desc],Image,IsTool,isActive,sort from  TouristManagementCommon.dbo.Fun_RtnUserModule(0,1,-1) where Isnull(IsActive,0)=1 order by sort");
                }
                else if (Classes.clsGlobal.Is_Admin == true)
                {
                    DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ID,Module,[Desc],Image,IsTool,isActive,sort from  TouristManagementCommon.dbo.Fun_RtnUserModule(0,0,0) where isCritical<>1 and Isnull(IsActive,0)=1 order by sort");
                }
                else
                {
                    DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ID,Module,[Desc],Image,IsTool,isActive,sort from  TouristManagementCommon.dbo.Fun_RtnUserModule(" + Classes.clsGlobal.UserID.ToString() + ",0,0) where isCritical<>1 and Isnull(IsActive,0)=1 order by sort");
                }
                foreach (DataRow DR in DT.Rows)
                {
                    ItemNo = System.Convert.ToInt16(DR[0].ToString());
                    ToolStripMenuItem child = new ToolStripMenuItem(); 
                    child.Name = DR[1].ToString();
                    child.Text = DR[2].ToString();
                    if ((DR[3].ToString() != null) && (DR[3].ToString() != ""))
                    {
                        child.Image = Classes.clsGlobal.byteArrayToImage((byte[])DR[3]);
                    }
                    menuStrip.Items.Add(child);
                    Addchild(child, ItemNo.ToString()); 
                }
            foreach (ToolStripMenuItem item in menuStrip.Items)
                Subscribe(item, ContextMenu_Click);
        }
        catch (Exception ex){db.MsgERR(ex);}
        }
        public void Addchild(ToolStripMenuItem child, string ItemNo) 
        {
            try
            {
                string ItemName2;
                string ItemNo2;
                string type;
                DataTable ds22 = new DataTable();
                if (Classes.clsGlobal.Is_SuperUser == true)
                {
                    ds22 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ModuleID,[Desc],ParentID,Img,IsActive,[Type],tool,IsActive,SortOrder,Mode,Name from  TouristManagementCommon.dbo.Fun_RtnUserMenuModule(0,1,-1) where PARENTID= '" + ItemNo + "'  order by ParentId,ModuleId");
                }
                if (Classes.clsGlobal.Is_Admin == true)
                {
                    ds22 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ModuleID,[Desc],ParentID,Img,IsActive,[Type],tool,IsActive,SortOrder,Mode,Name from  TouristManagementCommon.dbo.Fun_RtnUserMenuModule(0,0,0) where PARENTID= '" + ItemNo + "'  and IsCritical<>1 and Isnull(IsActive,0)=1 order by ParentId,ModuleId");
                }
                if (Classes.clsGlobal.Is_Admin == false && Classes.clsGlobal.Is_SuperUser == false)
                {
                    ds22 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ModuleID,[Desc],ParentID,Img,IsActive,[Type],tool,IsActive,SortOrder,Mode,Name from  TouristManagementCommon.dbo.Fun_RtnUserMenuModule(" + Classes.clsGlobal.UserID.ToString() + ",0,0) where PARENTID= '" + ItemNo + "'  and Isnull(IsActive,0)=1 order by ParentId,ModuleId");
                }
                foreach (DataRow DR2 in ds22.Rows)
                {
                    ItemNo2 = DR2[0].ToString();
                    ItemName2 = DR2[1].ToString();
                    type = DR2[5].ToString();
                    ParentItem = new ToolStripMenuItem(ItemName2);
                    ParentItem.Text = DR2[1].ToString();
                    ParentItem.Name = DR2[10].ToString();
                    if ((DR2[3].ToString() != null) && (DR2[3].ToString() != ""))
                    {
                        ParentItem.Image = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])DR2[3]);
                    }
                    if (type == "1")
                    {
                        ParentItem.Checked = true;
                    }
                    Set_MenuCheckBox(ParentItem);
                    child.DropDownItems.Add(ParentItem);
                    Addchild(ParentItem, ItemNo2);  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void Toolbar()
        {
            string menuName;
            int ID;
            string type;
            bool IsActive;
            try
            {
                type = Classes.clsGlobal.UserID.ToString() + ",0,0";
                if (Classes.clsGlobal.Is_SuperUser == true) { type = "0,1,-1"; }
                if (Classes.clsGlobal.Is_Admin == true) { type = "0,0,0"; }
                DataTable dt1 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select MM.ModuleID,MM.Name,MM.ParentID,MM.Img,MM.IsActive,MM.[Type],MM.tool,MM.IsActive" +
                            ",SortOrder,Mode from dbo.Fun_RtnUserMenuModule(" + type + ")MM LEFT OUTER JOIN dbo.Fun_RtnUserModule(" + type + ")UM ON MM.ModuleID=UM.ID WHERE ISNULL(tool,0) = 1  or ParentID=0 order by UM.Sort");
            type = "";                
            foreach (DataRow DR in dt1.Rows)
            {
                ToolStripSeparator seperator = new ToolStripSeparator();
                toolStrip.Items.Add(seperator);
                ID = System.Convert.ToInt16(DR[0].ToString());
                menuName = DR[1].ToString();
                IsActive=Convert.ToBoolean(DR[4].ToString());
                type = DR[5].ToString();
                if (type == "1")
                {
                        CheckBox cb1 = new CheckBox();
                        cb1.Name = menuName;
                        cb1.Text = menuName;
                        cb1.BackColor = Color.Transparent;
                        if (IsActive == true)
                        {
                            cb1.Checked = true;
                        }
                        else
                        {
                            cb1.Checked = false;
                        }
                        cb1.Enabled = true;
                        ToolStripControlHost host = new ToolStripControlHost(cb1);
                        toolStrip.Items.Add(host);
                        Set_CheckBox(cb1);
                        Subscribe2(cb1, ToolStripClick);
                }
                if ((DR[3].ToString() != null) && (DR[3].ToString() != ""))
                {
                    toolStrip.Items.Add(menuName, Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])DR[3]));
                }
            }
        }
        catch (Exception ex){db.MsgERR(ex);}
        }
        public void Toolbar_MainModules()
        {
            string ModuleName;
            int ModuleID;
            DataTable dt1 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT [ModuleID],[ModuleName],[Image],[IsActive] FROM [TouristManagement].[dbo].[mst_ModuleMaster] where [ParentID] ='0' and [IsActive] = '1' order by [ModuleID]");
            foreach (DataRow DR in dt1.Rows)
            {
                ToolStripSeparator seperator = new ToolStripSeparator();
                toolStrip.Items.Add(seperator);
                ModuleID = System.Convert.ToInt16(DR[0].ToString());
                ModuleName = DR[1].ToString();
                if ((DR[3].ToString() != null) && (DR[3].ToString() != ""))
                {
                    toolStrip.Items.Add(ModuleName, Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])DR[2]));
                }
            }
        }
        protected void chkDynamic_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void Set_tree_Selected()
        {
            try
            {
            if (treeMain.Nodes[0].Nodes.Count > 0)
            {
                treeMain.SelectedNode = treeMain.Nodes[0].Nodes[0];
                treeMain.Nodes[0].Nodes[0].Expand();
                open_Form_Navigator(treeMain.SelectedNode.Name, treeMain.SelectedNode.Text);
                return;
            }
            treeMain.Nodes[0].Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Set_Specific_Selected(string strName)
        {
            try
            {
                ActiveNode = selectedNode;
                foreach (TreeNode TN in selectedNode.Nodes)
                {
                    ActiveNode = TN;
                    if (visitChildNodes(TN, strName) == true)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private bool visitChildNodes(TreeNode node, string strName)
        {
            try
            {
            foreach (TreeNode TN in node.Nodes)
            {
                ActiveNode = TN;
                if (TN.Name == strName)
                {
                    treeMain.SelectedNode = ActiveNode;
                    treeMain.SelectedNode.Expand();
                    return true; ;
                }
                visitChildNodes(TN, strName);
            }
            return false;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void Set_Panels()
        {
            pnlTreeMain.Width = this.Width / 4-50;
            pnlTreeMain.Height = this.Height - (pnlTreeMain.Top + statusStrip.Height+37);
            pnlTree.Width = pnlTreeMain.Width;
            pnlTree.Height = (pnlTreeMain.Height / 4) * 3;
            pnlRecent.Location = new Point(pnlTree.Location.X, pnlTree.Location.Y + pnlTree.Height);
            pnlRecent.Height = (pnlTreeMain.Height / 4);
            pnlRecent.Width = pnlTreeMain.Width;
            treeMain.Location = pnlTree.Location;
            treeMain.Width = pnlTree.Width;
            treeMain.Height = pnlTree.Height;
            tabMain.Location = new Point(pnlTree.Location.X + pnlTree.Width + 4, pnlTree.Location.Y + tabMain.Height + toolStrip.Height + 4);
            tabMain.Width = this.Width - pnlTree.Width - 4;
            tabMain.Top = tabMain.Height + PnlLFTOP.Top+2;
            PnlLFTOP.Left = tabMain.Left;
            PnlLFTOP.Width = tabMain.Width;
            lblProduct.Left = (PnlLFTOP.Width - lblProduct.Width) / 2;
            btnTree.Location = new Point(pnlTreeMain.Location.X + (pnlTreeMain.Width - btnTree.Width), btnTree.Location.Y);
            btnRecent.Location = new Point(pnlRecent.Location.X + (pnlRecent.Width - btnRecent.Width), btnRecent.Location.Y);
            flxR.Width = pnlRecent.Width;
            grdResize();
        }
        private bool Add_Select_Control(string strKey, string strtext)
        {
            foreach (Form frm in this.MdiChildren)
                if (strKey == frm.Name && tabMain.TabPages[strKey]!=null)
                {
                    tabMain.SelectedTab = tabMain.TabPages[strKey];
                    tabMain.SelectedTab.Text = strtext;
                    tabMain.TabPages[strKey].Select();
                    return true;
                }
            tabMain.TabPages.Add(strKey, strtext);
            tabMain.Select();
            tabMain.SelectedTab = tabMain.TabPages[strKey];
            tabMain.Select();
            tabMain.Visible = true;
            return false;
        }
        private bool Find_OpenedForm(string strKey, string strtext)
        {
                foreach (Form frm in this.MdiChildren) 
                    if (strKey == frm.Name)
                    {
                        frm.Activate();
                        return true;
                    } 
                return false;
        }
        public void open_form(string frmName,string frmText)
        {
            Boolean hasList = false;
            try
            {
                Form frm;
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT formName FROM dbo.vwForms Where IsNull(ApplyList,0)=1 and formName='" + frmName + "'").Rows.Count > 0)
                {
                    frmName = "frmList_" + frmName;
                    hasList = true;
                }
                if (Add_Select_Control(frmName, frmText) == true)   {    }
                if (frmName != "frmNavigator")
                {
                    frm = Classes.clsForms.rtnForm(frmName, 0, 0);
                }
                else
                {
                    frm = frmNG;
                    frmNG.Fill_Records();
                    hasList = true;
                }
                if (hasList == true)
                {
                    frm.MdiParent = this;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(pnlTreeMain.Location.X + pnlTreeMain.Width, tabMain.Location.Y - (tabMain.Height + 10));
                    frm.Width = this.Width-(pnlTreeMain.Width + 22);
                    frm.Height = this.Height - (tabMain.Top + tabMain.Height + 60);
                    frm.Show();
                    Resize_Main();
                }
                else
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                AddRemoveRecent(frm, 0);
            }
            catch (Exception ex)     {  db.MsgERR(ex); }
        }
        private void tabMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (tabMain.SelectedTab.Name != "frmNavigator")  Add_Select_Control(tabMain.SelectedTab.Name, tabMain.SelectedTab.Text); 
            if (e.Button == MouseButtons.Right)
            {
                if (tabMain.TabCount == 1) { popUp.Items[1].Enabled = false; } else { popUp.Items[1].Enabled = true; }
                popUp.Show(e.Location.X+tabMain.Left,e.Location.Y+tabMain.Top+(popUp.Height/2));
            }
        }
        private void ExpandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Form frm in this.MdiChildren)
                    if (tabMain.SelectedTab.Name == frm.Name)
                    {
                        tabMain.TabPages.RemoveByKey(tabMain.SelectedTab.Name);
                        if (frm == frmNG) frm.Hide(); else frm.Close();
                        if (tabMain.TabCount == 0) tabMain.Visible = false;
                        return;
                    }
            }
            catch (Exception ex)  {   db.MsgERR(ex);  } 
        } 
        private void closeOtherFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string frmName="";
            try
            {
                frmName = tabMain.SelectedTab.Name;
                        foreach (Form frm in this.MdiChildren)
                        {
                            if (frm.Name != frmName)
                            {
                                tabMain.TabPages.RemoveByKey(frm.Name);
                                if (frm == frmNG) frm.Hide(); else frm.Close();
                            }
                         }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void intialize_Grd()
        {
            try
            {
                flxR.Rows.Count = 0;
                flxR.Rows.Fixed = 0;
                flxR.Cols.Count = 3;
                flxR.Cols[0].Visible = false;
                flxR.Cols[1].Width = flxR.Width-5;
                flxR.Cols[1].Width = flxR.Cols[1].Width-40;
                flxR.Cols[2].Width = 40;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void AddRemoveRecent(Form frm, int intMode)
        {
            try
            {
                if (frm.Name == "frmNavigator")
                {
                    return;
                }
                if (intMode == 0)
                {
                    if (flxR.Rows.Count == 7)
                    {
                        flxR.Rows.Remove(flxR.Rows.Count - 1);
                    }
                    for (int x = 0;  x < flxR.Rows.Count; x++)
                    {
                        if (flxR[x, 0].ToString() == frm.Name.ToString())
                        {
                            return;
                        }
                    }
                    flxR.Rows.Insert(0);
                    flxR[0, 0] = frm.Name;
                    flxR[0, 1] = frm.Text;
                    flxR.SetCellImage(0,2,RtnImage(frm.Name));
                    grdResize();
                }
                else
                {
                    for(int x=0;x<flxR.Rows.Count;x++)
                    {
                        if (flxR[x,0].ToString() == frm.Name.ToString())
                        {
                            flxR.Rows.Remove(x);
                            grdResize();
                            return;
                        }
                    }
                }
        }
        catch (Exception ex){db.MsgERR(ex);}
        }
        private void flxR_Click(object sender, EventArgs e)
        {
            Form frm;
            try
            {
                if (flxR.Rows.Count < 1)
                {
                    return;
                }
                if (flxR[flxR.Row,0].ToString().Trim() != "")
                { 
                    frm=Classes.clsForms.rtnForm(flxR[flxR.Row,0].ToString(),0,0);
                    open_form(frm.Name.Replace("frmList_", ""), frm.Text);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdResize()
        {
            int rwSize;
            if (flxR.Rows.Count < 1)
            {
                flxR.Height = 0;
                return;
            }
            rwSize = 20 * flxR.Rows.Count;
            flxR.Height = rwSize;
        }
        private void flxR_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        private void flxR_MouseMove_1(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void pnlRecent_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        private void pnlTree_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabMain.TabCount < 1)
                {
                    return;
                }
            foreach (Form frm in this.MdiChildren)
            {
                if ( tabMain.SelectedTab !=null &&  frm.Name  == tabMain.SelectedTab.Name + "")
                {
                    frm.Activate();
                    return;
                }
            }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void frmMDIMain_Activated(object sender, EventArgs e)
        {
            Boolean blnFound = false;
            try
            {
                if (this.MdiChildren.Count()==0)
                {
                    foreach (TabPage TP in tabMain.TabPages)
                    {
                        tabMain.TabPages.RemoveByKey(TP.Name);
                    }
                    return;
                }
                foreach (TabPage TP in tabMain.TabPages)
                {
                    foreach (Form frm in this.MdiChildren)
                    {
                        if (frm.Name == TP.Name)
                        {
                            blnFound = true;
                            break;
                        }
                    }
                    if (blnFound == false)
                    {
                        tabMain.TabPages.RemoveByKey(TP.Name);
                    }
                    else
                    {
                        blnFound = false;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
         public Image RtnImage(string strText)
        {
            return imgTree.Images[SearchImageIndex(treeMain.Nodes[0],strText)];
        }
         private int SearchImageIndex(TreeNode TR,string str)
         {
             int rtnVal=-1;
             try{
             foreach (TreeNode tr in TR.Nodes)
             {
                 if (str.Replace("frmList_", "") == tr.Name)
                 {
                     return tr.ImageIndex;
                 }
                 else
                 {
                     rtnVal=SearchImageIndex(tr, str);
                     if (rtnVal != -1) { return rtnVal; }
                 }
             }
             return rtnVal;
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return rtnVal;
             }
         }
         private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
         {
             try
             {
                 if (blnLoad == true)
                 {
                     blnLoad = false;
                     return;
                 }
                 if (e.Node.Name.ToString() == "")
                 {
                     return;
                 }
                 open_Form_Navigator(e.Node.Name, e.Node.Text);
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         public void open_Form_Navigator(string strName,string strText)
            {
              try
                {
                    if (Classes.clsGlobal.objComCon.Fill_Table("SELECT formID from dbo.mst_FormMaster Where formName='" + strName + "'").Rows.Count < 1)
                 { 
                     frmNG.ParentID = System.Convert.ToInt16(Classes.clsGlobal.objComCon.Fill_Table("SELECT ModuleID FROM dbo.mst_ModuleMaster Where ModuleName='" + strName + "'").Rows[0][0].ToString());
                     frmNG.ParentName = strName;
                     frmNG.ParentText = strText;
                     frmNG.ParentImage = (byte[])(Classes.clsGlobal.objComCon.Fill_Table("SELECT Image FROM dbo.mst_ModuleMaster Where ModuleID='" + frmNG.ParentID + "'").Rows[0][0]);
                     open_form(frmNG.Name, strText);
                     Set_Specific_Selected(strName);
                     return;
                 }
                    open_form(strName.ToString(), strText.ToString());
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         private void tmerMain_Tick(object sender, EventArgs e)
         {
             lblTime.Text = System.DateTime.Now.ToLongTimeString();
         }
         private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
         {
             TabPage CurrentTab = tabMain.TabPages[e.Index];
             Rectangle ItemRect = tabMain.GetTabRect(e.Index);
             SolidBrush FillBrush = new SolidBrush(Color.White);
             SolidBrush TextBrush = new SolidBrush(Color.Black);
             StringFormat sf = new StringFormat();
             sf.Alignment = StringAlignment.Center;
             sf.LineAlignment = StringAlignment.Center;
             if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
             {
                 FillBrush.Color = Color.White;
                 TextBrush.Color = Color.Blue;
                 ItemRect.Inflate(2, 2);
             }
             if (tabMain.Alignment == TabAlignment.Left || tabMain.Alignment == TabAlignment.Right)
             {
                 float RotateAngle = 90;
                 if (tabMain.Alignment == TabAlignment.Left)
                     RotateAngle = 270;
                 PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                 e.Graphics.TranslateTransform(cp.X, cp.Y);
                 e.Graphics.RotateTransform(RotateAngle);
                 ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
             }
             e.Graphics.FillRectangle(FillBrush, ItemRect);
             e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, ItemRect, sf);
             e.Graphics.ResetTransform();
             FillBrush.Dispose();
             TextBrush.Dispose();
         }
         private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
         {
             selectedNode = treeMain.SelectedNode;
         }
         private void treeMain_KeyDown(object sender, KeyEventArgs e)
         {
             try
             {
                 if (e.KeyCode != Keys.Enter) { return; }
                 if (blnLoad == true)
                 {
                     blnLoad = false;
                     return;
                 }
                 if (treeMain.SelectedNode.Name.ToString() == "")
                 {
                     return;
                 }
                 open_Form_Navigator(treeMain.SelectedNode.Name, treeMain.SelectedNode.Text);
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         private static void Subscribe(ToolStripMenuItem item, EventHandler eventHandler)
         {
             if (item.DropDownItems.Count == 0)
                 item.Click += eventHandler;
             else foreach (ToolStripMenuItem subItem in item.DropDownItems)
                     Subscribe(subItem, eventHandler);
         }
         private static void Subscribe2(CheckBox item, EventHandler eventHandler)
         {
                 item.Click += eventHandler;
         }
         void ContextMenu_Click(object sender, EventArgs e)
         {
             if (IsNot_treeItem((sender as ToolStripMenuItem).Name) == true) { return;}
             if ((sender as ToolStripMenuItem).OwnerItem == null) { return; }
             if (IsCheckBox((sender as ToolStripMenuItem))==true) { return; }
             open_Form_Navigator((sender as ToolStripMenuItem).OwnerItem.Name, (sender as ToolStripMenuItem).OwnerItem.Text);
             open_Form_Navigator((sender as ToolStripMenuItem).Name, (sender as ToolStripMenuItem).Text);
         }
         void ToolStripClick(object sender, EventArgs e)
         {
             IsToolCheckBox((CheckBox)sender);
         }
         private bool IsToolCheckBox(CheckBox ToolNode)
         {
             switch (ToolNode.Name)
             {
                 case "Tree":
                     IsCheckBox(menuItemTree);
                     return true;
                 case "Recent":
                     IsCheckBox(menuItemRecent);
                    return true;
                 case "Right":
                     IsCheckBox(menuItemLeft);
                     return true;
                 default:
                     return false;
             }
         }
         private bool IsCheckBox(ToolStripMenuItem ToolNode)
         {
             switch (ToolNode.Name)
             {
                 case "Tree":
                     if (ToolNode.Checked == true) { ToolNode.Checked = false; } else { ToolNode.Checked = true; }
                     if (ToolNode.Checked == true) { isTree = true; } else { isTree = false; }
                     chkTree.Checked = ToolNode.Checked;
                     Resize_Main();
                     return true;
                 case "Recent":
                     if (ToolNode.Checked == true) { ToolNode.Checked = false; } else { ToolNode.Checked = true; }
                     if (ToolNode.Checked == true) { isRecent = true; } else { isRecent = false; }
                     chkRecent.Checked = ToolNode.Checked;
                     Resize_Main();
                     return true;
                 case "Right":
                     if (ToolNode.Checked == true) { ToolNode.Checked = false; } else { ToolNode.Checked = true; }
                     if (ToolNode.Checked == true) { isLeft = false; } else { isLeft = true; }
                     chkLeft.Checked = ToolNode.Checked;
                     Resize_Main();
                     return true;
                 default:
                     return false;
             }
         }
         private void Set_CheckBox(CheckBox chkBox)
         {
             switch (chkBox.Name)
             {
                 case "Tree":
                     chkTree = chkBox;
                     break;
                 case "Recent":
                     chkRecent = chkBox;
                     break;
                 case "Right":
                     chkLeft = chkBox;
                     break;
             }
         }
         private void Set_MenuCheckBox(ToolStripMenuItem ToolNode)
         {
             switch (ToolNode.Name)
             {
                 case "Tree":
                     menuItemTree = ToolNode;
                     break;
                 case "Recent":
                     menuItemRecent = ToolNode;
                     break;
                 case "Right":
                     menuItemLeft = ToolNode;
                     break;
             }
         }
        public void Resize_Main()
        {
            try
            {
                if (blnLoad == true) { return; }
                if (isLeft == true)
                {
                    menuStrip.RightToLeft = RightToLeft.No;
                    toolStrip.RightToLeft = RightToLeft.No;
                    pnlTime.Visible = true;
                }
                else
                {
                    menuStrip.RightToLeft = RightToLeft.Yes;
                    toolStrip.RightToLeft = RightToLeft.Yes;
                    pnlTime.Visible = false;
                }
                if (isRecent == false)
                {
                    pnlTree.Height = pnlTreeMain.Height;
                    pnlRecent.Visible = false;
                }
                else
                {
                   pnlTree.Height = (pnlTreeMain.Height / 4) * 3; 
                   pnlRecent.Visible = true;
                }
                Set_Panels();
                if (isTree == false)
                {
                    pnlTreeMain.Visible = false;
                    tabMain.Width = tabMain.Width + pnlTreeMain.Width;
                    tabMain.Location = new Point(tabMain.Location.X - pnlTreeMain.Width, tabMain.Location.Y);
                    PnlLFTOP.Width = PnlLFTOP.Width + pnlTreeMain.Width;
                    PnlLFTOP.Location = new Point(PnlLFTOP.Location.X - pnlTreeMain.Width, PnlLFTOP.Location.Y);
                    this.ActiveMdiChild.Width = this.Width - (22);
                    this.ActiveMdiChild.Location = new Point(pnlTreeMain.Location.X, this.ActiveMdiChild.Location.Y);
                }
                else
                {
                    pnlTreeMain.Visible = true;
                    tabMain.Location = new Point(pnlTree.Location.X + pnlTree.Width + 4, tabMain.Location.Y);
                    tabMain.Width = this.Width - pnlTree.Width - 4;
                    PnlLFTOP.Left = tabMain.Left;
                    PnlLFTOP.Width = tabMain.Width;
                    if (this.ActiveMdiChild != null) this.ActiveMdiChild.SetBounds(pnlTreeMain.Location.X + pnlTreeMain.Width, this.ActiveMdiChild.Location.Y, this.Width - (pnlTreeMain.Width + 22), this.Width - (pnlTreeMain.Width + 22));
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private bool IsNot_treeItem(string strName)
        {
            try
            {
                if (Classes.clsGlobal.objComCon.Fill_Table("SELECT MenuName FROM dbo.mst_MenuItems Where Type<>1 and MenuName='" + strName + "'").Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return true;
            }
        }
        private void PnlLFTOP_SizeChanged(object sender, EventArgs e)
        {
            pbMain.Size = PnlLFTOP.Size;
        }
        private void Set_Status_Bar()
        {
            toolSCommon.Text="Common : TouristManagementCommon";
            toolSCompany.Text = "Company : TouristManagement";
            toolSDate.Text="Today : "+DateTime.Today.ToLongDateString();
            toolSLogIn.Text = "Last login : " + DateTime.Today.ToShortDateString();
            toolSLogOut.Text = "Last logout : " + DateTime.Today.ToShortDateString();
            toolSUser.Text = "User : Admin";
        }
        private void btnTree_Click(object sender, EventArgs e)
        {
            IsCheckBox(menuItemTree);
        }
        private void btnRecent_Click(object sender, EventArgs e)
        {
            IsCheckBox(menuItemRecent);
        }
        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        { 
                switch (e.ClickedItem.ToString().Trim())
                {
                    case "Log Off":
                        Application.Restart();
                        break;
                    case "Exit":
                        Application.Exit();
                        break;
                    case "Lock":
                        Settings.frmLock fl = new Tourist_Management.Settings.frmLock();
                        this.Opacity = 0.8;
                        fl.ShowDialog();
                        this.Opacity = 1;
                        if (!Classes.clsGlobal.PasswordOK)
                            Application.Exit();
                        break;
                }
        }
        private void pbLock_Click(object sender, EventArgs e)
        {
                Settings.frmLock fl = new Tourist_Management.Settings.frmLock();
                this.Opacity = 0.8;
                fl.ShowDialog();
                this.Opacity = 1;
                if (!Classes.clsGlobal.PasswordOK)
                    Application.Exit();
        }
    }
}
