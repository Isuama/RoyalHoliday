using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tourist_Management.User_Controls;
namespace Tourist_Management.Other
{
    public partial class frmFilterRecords : Form
    {
        DataTable DT, DTall; 
        string Syscode = "";
    Tourist_Management.User_Controls.ComboBox[] cbar, cbop;
        public string Load_search(DataTable DTB)
        {
            cbar = new Tourist_Management.User_Controls.ComboBox[] { cmbFld1, cmbFld2, cmbFld3, cmbFld4 };
            cbop = new Tourist_Management.User_Controls.ComboBox[] { cmbOp1, cmbOp2, cmbOp3, cmbOp4 };
            DTall = DTB; 
            try
            { 
                DT = DTB;
                if (DT.Rows.Count == 0) return "";
                flx.DataSource = DT;
                flx.Cols[0].Visible = false;
                for (int x = 0; x <= flx.Cols.Count - 1; x++) foreach (Tourist_Management.User_Controls.ComboBox b in cbar) b.Items.Add(flx[0, x].ToString());
                foreach (Tourist_Management.User_Controls.ComboBox b in cbar) b.SelectedIndex = Math.Min(cmbFld1.Items.Count-1, 1);
                  foreach (Tourist_Management.User_Controls.ComboBox b in cbop)  db.LoadSearch(b); 
                this.DialogResult = this.ShowDialog();
                if (this.DialogResult == DialogResult.OK) return Syscode; else return "";
            }
            catch (Exception) { return ""; }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (flx.Rows.Count <= 1 || flx[flx.Row, 0] + "".Trim() == "") return;
            Syscode = flx[flx.Row, 0].ToString();
            this.DialogResult = DialogResult.OK;
        }
        private void Apply_Filter()
        {
            DataView DV = new DataView(DT);
            if (txtval1.Text.Trim() == "") return;
            switch (cmbOp1.Text.Trim())
            {
                case "Contains":
                case "Begins with":
                case "Ends with": DV.RowFilter = cmbFld1.Text + " " + cmbOp1.SelectedValue.ToString().Replace("##", txtval1.Text.Trim()).ToString(); break;
                default: DV.RowFilter = cmbFld1.Text + " " + cmbOp1.SelectedValue.ToString() + "'" + txtval1.Text.Trim() + "'"; break;
            }
            flx.DataSource = DV;
            flx.Cols[0].Visible = false;
        }
        private void flx_Click_1(object sender, EventArgs e)
        {
            if (flx.Rows.Count < 2 || flx.Row == 0) { return; }
            cmbFld1.Text = flx[0, flx.Col].ToString();
            txtval1.Select();
        }
        private void flx_Click(object sender, EventArgs e) { if (flx.Rows.Count < 2 || flx.Row == 0) return; else foreach (Tourist_Management.User_Controls.ComboBox b in cbar) b.Text = flx[0, flx.Col].ToString(); }
        private void btnShowAll_Click(object sender, EventArgs e) { if (DTall.Rows.Count <= 0) return; flx.DataSource = DTall; flx.Cols[0].Visible = false; }
        private void flx_DoubleClick(object sender, EventArgs e) { btnSelect_Click(null, null); }
        private void flxGroup_Grid_DoubleClick(object sender, EventArgs e) { }
        private void frmFilterRecords_Load(object sender, EventArgs e) { txtval1.Select(); }
        private void txtval1_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { btnSelect_Click(null, null); } }
        private void txtval1_TextChanged(object sender, EventArgs e) { Apply_Filter(); }
        private void btncancel_Click(object sender, EventArgs e) { Syscode = ""; this.DialogResult = DialogResult.Cancel; }
        private void btnclear_Click(object sender, EventArgs e) { txtval1.Text = txtval2.Text = txtval3.Text = txtval4.Text = ""; }
        private void btnSearch_Click(object sender, EventArgs e) { Apply_Filter(); }
        public frmFilterRecords() { InitializeComponent(); }
    }
}