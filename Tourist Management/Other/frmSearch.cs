using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Other
{
    public partial class frmSearch : Form
    {
        DataTable DT; 
        string Syscode;
        public frmSearch(){InitializeComponent();}
        private void frmSearch_Load(object sender, EventArgs e)        {            txtval.Select();        }
        public string Load_Search(DataTable DTB)
        { 
            try
            {
                DT = DTB;
                if (DT.Rows.Count > 0)
                {
                    flx.DataSource = DT;
                    flx.Cols[0].Visible = false;
                }
                else  return ""; 
                for (int x = 0; x < flx.Cols.Count - 1; x++) cmbFld.Items.Add(flx[0, x].ToString());
                cmbFld.SelectedIndex = 1;
                db.LoadSearch(cmbOp);
                this.DialogResult = this.ShowDialog();
                return (this.DialogResult == DialogResult.OK) ? Syscode : "";
            }
            catch (Exception ex) { throw (ex); }
        }
        private void Apply_Filter()
        {
            DataView DV=new DataView(DT); 
            if(txtval.Text.Trim()=="")  return; 
            switch (cmbOp.Text.Trim())
            {
                case "Contains":
                case "Begins with":
                case "Ends with":
                    DV.RowFilter = cmbFld.Text + " " + cmbOp.SelectedValue.ToString().Replace("##", txtval.Text.Trim()).ToString();
                    break;
                default:
                    DV.RowFilter = cmbFld.Text + " " +  cmbOp.SelectedValue.ToString()+ "'" + txtval.Text.Trim() + "'";
                    break;
            }
            flx.DataSource = DV;
            flx.Cols[0].Visible = false;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
        if(flx.Rows.Count <= 1){return;}
        Syscode = flx[flx.Row, 0].ToString();
        this.DialogResult = DialogResult.OK;
        }
        private void btncancel_Click(object sender, EventArgs e)
        {
             Syscode = "";
             this.DialogResult = DialogResult.Cancel;
        }
        private void btnclear_Click(object sender, EventArgs e)        {            txtval.Text = "";    }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            flx.DataSource = DT;
            flx.Cols[0].Visible = false;
            cmbFld.SelectedIndex = 1;
            cmbOp.SelectedIndex = 0;
            txtval.Text = "";
        }
        private void btnSearch_Click(object sender, EventArgs e)        {            Apply_Filter();        }
        private void flx_Click(object sender, EventArgs e)
        {
                if (flx.Rows.Count < 2) { return; }
                if (flx.Row == 0) { return; }
                cmbFld.Text = flx[0, flx.Col].ToString();
                txtval.Select();
        }
        private void txtval_KeyDown(object sender, KeyEventArgs e)        {            if (e.KeyCode == Keys.Enter) btnSelect_Click(null, null);         }
        private void txtval_TextChanged(object sender, EventArgs e)        { btnSearch_Click(null, null);  }
    }
}
