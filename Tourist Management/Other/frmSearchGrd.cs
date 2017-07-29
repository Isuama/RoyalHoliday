using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace Tourist_Management.Other
{
    public partial class frmSearchGrd : Form
    {
        int FMWidth=250;
        int FMHeight=250;
        string[] strList;
        int colID = 1;
        Form frm;
        Boolean bLoad = false;
        DataTable DTB;
        public int MyWidth
        {
            get { return FMWidth; }
            set { FMWidth = value; }
        }
        public int DropHeight
        {
            get { return FMHeight; }
            set { FMHeight = value; }
        }
        public Form SubForm
        {
            set { frm = value; }
        }
       public string[] SelectedList
        {
            get { return strList; }
        }
        public string DefaultText
        {
            set { txttext.Text = value; }
        }
        public DataTable DataSource
        {
            get { return DTB; }
            set { DTB = value; }
        }
        public frmSearchGrd(){InitializeComponent();}
        private void frmSearchGrd_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            Populate_Drop();
            txttext.Select();
        }
        private void Populate_Drop()
        {
            bLoad = true;
            if (DTB != null)
            {
                Set_data_Source();
            }
            else
            {
                intializer_Componenet();
            }
            bLoad = false;
        }
        private void Load_Data()
        {
            dgrd.DataSource = DTB;
        }
        private void Set_data_Source()
        {
            DataView DV;
            DV = new DataView(DTB);
                if (dgrd.Columns.Count < 1 && bLoad == false)
                {
                    return;
                }
                if (txttext.Text.Trim() == "")
                {
                    Load_Data();
                    dgrd.Columns[0].Visible = false;
                    if (bLoad == true)
                    {
                        intializer_Componenet();
                    }
                    dgrd.ReadOnly = true;
                    return;
                }
                if (DTB.Columns[colID].DataType == System.Type.GetType("System.String"))
                {
                    DV.RowFilter = dgrd.Columns[colID].HeaderText.ToString() + " like '%" + txttext.Text.Trim() + "%'";
                }
                else
                {
                    if (IsNumeric(txttext.Text.Trim()) == true)
                    {
                        DV.RowFilter = dgrd.Columns[colID].HeaderText.ToString() + " = " + txttext.Text.Trim() + "";
                    }
                    else
                    {
                        return;
                    }
                }
                dgrd.DataSource = DV;
                dgrd.Columns[0].Visible = false;
                dgrd.ReadOnly = true;
        }
        private void intializer_Componenet()
        {
            this.Width = MyWidth;
            resise_forms();
            resise_forms();
        }
        private void Set_controlSize()
        {
            this.Height = DropHeight;
            dgrd.Height = DropHeight - (pnlSrch.Height + txttext.Height);
            dgrd.Width = this.Width + 40;
            txttext.Width = MyWidth - (btnOpen.Width) - 2;
            btnOpen.Left =  txttext.Width;
            pnlSrch.Top = txttext.Height + dgrd.Height;
            pnlSrch.Width = this.Width;
            for (int x = 1; x < dgrd.ColumnCount; x++)
            {
                dgrd.Columns[x].Width = (this.Width - 20) / (dgrd.ColumnCount - 1);
            }
        }
        private void resise_forms()
        {
            Set_controlSize();
            txttext.Select();
            dgrd.Select();
            if (bLoad == false && dgrd.RowCount > 0) { chkSrch.Text = "Serching........" + dgrd.Columns[colID].HeaderText.ToString(); }
        }
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, "^\\d+$");
        }
        private void dgrd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; }
            if (dgrd.RowCount > 0)
            {
                if (dgrd.CurrentCell.RowIndex >= 0)
                {
                    strList = new String[dgrd.ColumnCount];
                    for (int x = 0; x < dgrd.ColumnCount; x++)
                    {
                        strList[x] = dgrd[x, dgrd.CurrentCell.RowIndex].Value.ToString();
                    }
                    chkSrch.Checked = false;
                    this.Close();
                    return;
                }
            }
            strList = null;
        }
        private void dgrd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                strList = null;
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dgrd.RowCount >= 0)
                {
                    strList = new String[dgrd.ColumnCount];
                    for (int x = 0; x < dgrd.ColumnCount; x++)
                    {
                        strList[x] = dgrd[x, dgrd.CurrentCell.RowIndex].Value.ToString();
                    }
                    chkSrch.Checked = false;
                    this.Close();
                    return;
                }
            }
            strList = null;
        }
        private void txttext_TextChanged(object sender, EventArgs e)
        {
            {
                chkSrch.Checked = true;
                if (chkSrch.Checked == true)
                {
                    Set_data_Source();
                }
                txttext.Select();
            }
        }
        private void txttext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                strList = null;
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dgrd.RowCount > 0)
                {
                    strList = new String[dgrd.ColumnCount];
                    for (int x = 0; x < dgrd.ColumnCount; x++)
                    {
                        strList[x] = dgrd[x, dgrd.CurrentCell.RowIndex].Value.ToString();
                    }
                    chkSrch.Checked = false;
                    this.Close();
                    return;
                }
            }
            else
            {
                strList = null;
            }
        }
        public string[] Open_Search()
        {
            this.ShowDialog();
            return strList;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (frm != null)
            {
                frm.ShowDialog();
                Populate_Drop();
                this.Close();
            }
        }
       private void frmSearchGrd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                strList = null;
                this.Close();
            }
        }
       private void dgrd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
       {
           if (dgrd.Rows.Count > 0)
           {
               dgrd.CurrentCell = dgrd.Rows[0].Cells[e.ColumnIndex];
               colID = dgrd.CurrentCell.ColumnIndex;
               chkSrch.Text = "Serching........" + dgrd.Columns[colID].HeaderText.ToString();
           }
       }
    }
}
