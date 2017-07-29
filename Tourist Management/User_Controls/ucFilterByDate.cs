using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.User_Controls
{
    public partial class ucFilterByDate : UserControl
    {
        public ucFilterByDate() { InitializeComponent(); }
        private void chkIByDate_CheckedChanged(object sender, EventArgs e)        {            dtpIFromDate.Enabled = dtpIToDate.Enabled = true;        }
    }
}
