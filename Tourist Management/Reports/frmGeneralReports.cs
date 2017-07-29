using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
namespace Tourist_Management.Reports
{
    public partial class frmGeneralReports : Form
    {
        DataTable[] DTB;
        DataTable DT1;
        DataTable DT2;
        String ssql;
        String sql2;
        String sql3;
        string msghd = "General Reports";
        public frmGeneralReports(){InitializeComponent();}
        private void frmGeneralReports_Load(object sender, EventArgs e)
        {
            fillControl();
            cmbFilter.Enabled = false;
        }
        public void fillData()
        {
        }
        public void fillControl()
        {
            try
            {
                DTB = new DataTable[1];
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[Name]"+
                    "FROM [TouristManagementCommon].[dbo].[mst_GeneralQuery] Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpDriver.DataSource = DTB[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            cmbFilter.Items.Clear();
            cmbFilter.Text = "";
            txtFilter.Text = "";
            cmbFilter.Enabled = true;
            fillGrid();
            fillFilter();
            grdReport.DefaultCellStyle.ForeColor = Color.DarkCyan;
            grdReport.DefaultCellStyle.Font = new Font(grdReport.DefaultCellStyle.Font,FontStyle.Regular);
        }
        public void fillGrid()
        {
            try
            {
                ssql = "SELECT [ID],[Name],Query,Filter,IsFilterByDate,DateCols FROM [TouristManagementCommon].[dbo].[mst_GeneralQuery] " +
                    "where [ID]=" + drpDriver.SelectedValue.ToString();
                DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                sql2 = "";
                sql2 = DT1.Rows[0][2].ToString();
                sql3 = "";
                if (Convert.ToBoolean(DT1.Rows[0]["IsFilterByDate"].ToString()))
                {
                    grpFilterByDate.Visible = true;
                    string filter = DT1.Rows[0]["DateCols"].ToString();
                    string[] fil = filter.Split(',');
                    lblDateCol1.Text = fil[0];
                    lblDateCol2.Text = fil[1];
                    if (lblDateCol2.Text == "")
                    {
                        dtpDate2.Visible = false;
                    }
                    if (!sql2.Contains("WHERE"))
                    {
                        sql2 = sql2 + " WHERE " + lblDateCol1.Text + ">='" + dtpDate1.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                        if (lblDateCol2.Text != "")
                            sql2 = sql2 + " AND " + lblDateCol2.Text + "<='" + dtpDate2.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
                    }
                    else
                    {
                        sql2 = sql2 + " AND " + lblDateCol1.Text + ">='" + dtpDate1.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                        if (lblDateCol2.Text != "")
                            sql2 = sql2 + " AND " + lblDateCol2.Text + "<='" + dtpDate2.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
                    }
                }
                if (txtFilter.Text.Trim() != "" && cmbFilter.SelectedItem.ToString() != "")
                {
                    Filter_Columns();
                }
                else
                {
                    DT2 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql2);
                }
                grdReport.DataSource = DT2;
                grdReport.DataSource = DT2;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void fillFilter()
        {
             String filter = DT1.Rows[0][3].ToString();
            string[] fil=filter.Split(',');
            foreach (string fils in fil)
            {
                cmbFilter.Items.Add(fils);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "export.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ToCsV(grdReport, sfd.FileName); // Here dataGridview1 is your grid view name 
            }  
        }
        private void ToCsV(DataGridView dGV, string filename)
        {
            string stOutput = "";
            string sHeaders = "";
            for (int j = 0; j < dGV.Columns.Count; j++)
            {
                if(j==0)
                sHeaders = "                          "+drpDriver .SelectedText.ToString()+"                          "+ "\n\n";
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            }
            stOutput += sHeaders + "\r\n";
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            Filter_Columns();
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try{this.Close();}
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Filter_Columns()
        {
            try
            {
                if (cmbFilter.SelectedItem.ToString() != "")
                {
                    if (!sql2.Contains("WHERE"))
                    {
                        sql3 = sql2 + " WHERE " + cmbFilter.SelectedItem.ToString() + " Like '" + txtFilter.Text.Trim() + "%'";
                    }
                    else
                    {
                        sql3 = sql2 + " AND " + cmbFilter.SelectedItem.ToString() + " Like '" + txtFilter.Text.Trim() + "%'";
                    }
                    DT2 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql3);
                    grdReport.DataSource = DT2;
                }
            }
            catch (Exception  )
            {
                if(txtFilter.Text.Length>0)
                MessageBox.Show("Please Select Valid Filter Column",msghd,MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtFilter.Text = "";
            }
        }
        private void dtpDate1_ValueChanged(object sender, EventArgs e)
        {
        }
        private void dtpDate2_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
