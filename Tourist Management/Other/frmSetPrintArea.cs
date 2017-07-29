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
    public partial class frmSetPrintArea : Form
    {
        public Tourist_Management.User_Controls.ucReportViewer RPViewer;
        public frmSetPrintArea(){InitializeComponent();}
        private void frmSetPrintArea_Load(object sender, EventArgs e)
        {
            numRw.Maximum = 1000;
            numRw.Minimum = 1;
            numCol.Maximum = RPViewer.fG1.Grid.Cols.Count ;
            numCol.Minimum = 1;
            numRw.Value = RPViewer.MyRW;
            numCol.Value = RPViewer.MyCOL;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            RPViewer.MyRW = (int) numRw.Value;
            RPViewer.MyCOL = (int) numCol.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCan_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
