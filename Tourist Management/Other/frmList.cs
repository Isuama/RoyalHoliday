using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using C1.Win.C1FlexGrid;
namespace Tourist_Management.Other
{
    public partial class frmList : Form
    {
        private const string msghd = "Opening List";
        bool isReport = false,isFinder = false;
        public string SqlQuery = "", ReportID = "", frmFormName;
        int SelectedRow = 1;
        public Form OpenForm;
        private void frmList_Load(object sender, EventArgs e)
        {
            try
            {
                flxGroup.Grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.MultiColumn;
                   Enable_Disable_Button();
                cms.Items.Clear(); foreach (ToolBarButton b in toolBar.Buttons) if (b.Visible && b.Enabled) cms.Items.Add(new ToolStripMenuItem(b.Text, ToolImages.Images[b.ImageIndex]));
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            try
            {
                switch (e.Button.Text.ToUpper().ToString())
                {
                    case "ADD":
                        OpenForm = Classes.clsForms.rtnForm(OpenForm.Name, 0, 0);
                        OpenForm.StartPosition = FormStartPosition.CenterScreen;
                        OpenForm.ShowDialog();                        
                        break;
                    case "EDIT":
                        if (flxGroup.Grid.Rows.Count < 1) return;
                        if (flxGroup.Grid[flxGroup.Grid.Row, 1].ToString() != "")
                        {
                            OpenForm = Classes.clsForms.rtnForm(OpenForm.Name, 1, System.Convert.ToDouble(flxGroup.Grid[flxGroup.Grid.Row, 1]));
                            OpenForm.StartPosition = FormStartPosition.CenterScreen;
                            OpenForm.ShowDialog();                            
                        }
                        break;
                    case "REFRESH": break;
                    case "SEARCH":
                        string strID;
                        frmSearch frm = new frmSearch();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        strID = frm.Load_Search(Rtn_SRH_Table(SqlQuery)).ToString();
                        Select_Row(strID);
                        return;
                    case "PRINT":
                        if (flxGroup.Grid.Rows.Count < 1) return;
                        if (flxGroup.Grid[flxGroup.Grid.Row, 1].ToString() != "")
                        {
                            OpenForm = Classes.clsForms.rtnForm(OpenForm.Name, 1, System.Convert.ToDouble(flxGroup.Grid[flxGroup.Grid.Row, 1]));
                            frmFormName = OpenForm.Name.ToString().Trim();
                            Preview_Report(frmFormName);
                        }
                        break;
                    case "EXPORT": Classes.clsGlobal.ExporttoExceL(flxGroup.Grid, 2, true); break;
                    case "DELETE":
                        if (flxGroup.Grid.Rows.Count < 1) return;
                        if (flxGroup.Grid[flxGroup.Grid.Row, 1].ToString() != "")
                        {
                            var result = MessageBox.Show("Are you sure you want to delete this item", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                if (Classes.clsForms.delete_Records(OpenForm.Name, System.Convert.ToInt16(flxGroup.Grid[flxGroup.Grid.Row, 1])) == true)
                                    MessageBox.Show("Record sucessfully deleted", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    MessageBox.Show("Record was not sucessfully deleted\nBecause this record is already using...", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        break;
                } refresh_Grd();
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Preview_Report(string FormName)
        {
            try
            {
                DataTable DT = new DataTable();
                switch (FormName)
                {
                    case "frmGroupAmend": DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,ReportType FROM mst_ReportTypes Where IsNull(IsActive,0)=1 ORDER BY ID"); break;
                    default: break;
                }
                Reports.frmReportType.DataTable = DT;
                Reports.frmReportType.TourID = System.Convert.ToDouble(flxGroup.Grid[flxGroup.Grid.Row, 1]);
                Reports.frmReportType frt = new Tourist_Management.Reports.frmReportType();
                frt.ShowDialog();
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void frmList_Activated(object sender, EventArgs e)
        {
            try
            {
                if (OpenForm.Name == "frmReportViewer" && isReport == false && isFinder == false)
                {
                    isReport = true;
                    isFinder = true;
                    SqlQuery = "SELECT ID,ReportName,Mode from dbo.vw_Report Order By CategoryName,MODE,ReportName";
                }
                if (isReport == false) { Resize_Form(); }
                else if (isFinder == true)
                {
                    toolBar.Hide();
                    flxGroup.Visible = true;
                    Resize_Form();
                }
                else
                {
                    toolBar.Hide();
                    flxGroup.Hide();
                    Load_Report(Rtn_Table());
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void refresh_Grd()
        {
           flxGroup.Grid.DataSource = null;
            flxGroup.Grid.Clear(ClearFlags.All);
            flxGroup.Grid.ShowSort = false;
            flxGroup.FilterRow = false; 
            DataTable DT;
            try
            {
                if (Classes.clsForms.Is_Common(OpenForm.Name.ToString()) == true)
                    DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                else
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                
                flxGroup.Grid.DataSource = DT; 
                flxGroup.Grid.Cols[0].Width = 18;
                flxGroup.Grid.Cols[1].Width = 0;
                for (int x = 2; x < flxGroup.Grid.Cols.Count; x++)
                {
                    flxGroup.Grid.Cols[x].Width = (flxGroup.Width - 38) / (flxGroup.Grid.Cols.Count - 2);
                    flxGroup.Grid[0, x] = DT.Columns[x - 1].Caption.ToString();
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Select_Row(string ID)
        {
            try { for (int x = 2; x < flxGroup.Grid.Rows.Count; x++) if (flxGroup.Grid[x, 1].ToString() == ID) flxGroup.Grid.Row = x; }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Resize_Form()
        {
            ((Main.frmMDIMain)MdiParent).Resize_Main();
            frmList_Load(null, null);
            refresh_Grd();
        }
        private void Enable_Disable_Button()
        {
            toolBar.Buttons[0].Enabled = Classes.clsGlobal.Is_Permited(OpenForm.Name, 2);
            toolBar.Buttons[1].Enabled = Classes.clsGlobal.Is_Permited(OpenForm.Name, 3);
            toolBar.Buttons[6].Enabled = Classes.clsGlobal.Is_Permited(OpenForm.Name, 4);
            toolBar.Buttons[4].Enabled = toolBar.Buttons[5].Enabled = Classes.clsGlobal.Is_Permited(OpenForm.Name, 5);
        }
        private DataTable Rtn_SRH_Table(string ssql)
        {
            try { return Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql); }
            catch (Exception) { return Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql); }
        }
        private void Load_Report(DataTable DT)
        {
            try
            {
                Tourist_Management.User_Controls.ucReportViewer UC = new Tourist_Management.User_Controls.ucReportViewer();
                UC.MyHeight = this.Size.Height;
                UC.MyWidth = this.Size.Width;
                UC.Location = new Point(0, 0);
                UC.ReportID = ReportID;
                UC.MySource = DT;
                UC.MyParent = this;
                UC.BringToFront();
                this.Controls.Add(UC);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private DataTable Rtn_Table()
        {
            string strID = "0";
            try
            {
                if (flxGroup.Grid.Rows[flxGroup.Grid.Row][1].ToString() != "0")
                {
                    strID = flxGroup.Grid.Rows[flxGroup.Grid.Row][1].ToString();
                    ReportID = strID;
                }
                return Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(" EXEC [spReturn_Reports] " + strID);
            }
            catch (Exception ex) { db.MsgERR(ex); }
            return null;
        }
        public void Remove_Control()
        {
            toolBar.Hide();
            flxGroup.Visible = true;
            Resize_Form();
            this.Controls.RemoveByKey("ucReportViewer");
            isReport = true;
            isFinder = true;
            frmList_Activated(null, null);
        }
        private void flxGroup_AfterSelChange(object sender, RangeEventArgs e)
        {
            try
            {
                if (flxGroup.Grid.Rows.Count > 1)
                {
                    C1.Win.C1FlexGrid.CellStyle DEF = flxGroup.Grid.Styles.Add("DEF");
                    DEF.BackColor = Color.White;
                    flxGroup.Grid.Rows[SelectedRow].Style = DEF;
                    C1.Win.C1FlexGrid.CellStyle LOSS = flxGroup.Grid.Styles.Add("LOSS");
                    LOSS.BackColor = Color.LightCyan;
                    if (flxGroup.Grid.Row > 0)
                    {
                        flxGroup.Grid.Rows[flxGroup.Grid.Row].Style = LOSS;
                        SelectedRow = flxGroup.Grid.Row;
                    }
                    else
                    {
                        flxGroup.Grid.Rows[1].Style = LOSS;
                        SelectedRow = 1;
                    }
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        public frmList() { InitializeComponent(); }  
        private void cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { toolBar_ButtonClick(toolBar, new ToolBarButtonClickEventArgs(new ToolBarButton(e.ClickedItem.Text))); }
        private void flxGroup_Grid_DoubleClick(object sender, EventArgs e) { if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Left) toolBar_ButtonClick(toolBar, new ToolBarButtonClickEventArgs(BEdit)); }
        private void tSearch_TextChanged(object sender, EventArgs e)
        {
            flxGroup.Grid.Visible = false;
            bool has=false;
            string S = tSearch.Text.Trim().ToUpper();
            string[] s = (" " + S).ToLower().Replace(" ", "~ ").Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (C1.Win.C1FlexGrid.Row r in flxGroup.Grid.Rows)if(!r.IsNew && r.Index>1)
            {
                foreach (C1.Win.C1FlexGrid.Column c in flxGroup.Grid.Cols)
                {
                    has = true;
                    string v = (flxGroup.Grid[r.Index, c.Index] + "\n").Replace(",", " ").Replace("(", " ").Replace(")", " ");
                    foreach (char C in ",.()~`|_+-'/\\\r\"".ToCharArray()) v = v.Replace(C, ' ');                    
                    v = " " + v.Substring(0, v.IndexOf("\n")).Trim().ToLower();
                    foreach (string f in s) has = has && v.Contains(f);
                    if (v.EndsWith(S)) has = true;
                    if (has) break;
                }
                r.Visible = has; 
            }
            flxGroup.Grid.Visible = true;
        }
    }
}
