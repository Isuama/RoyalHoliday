using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Account_Reports
{
    public partial class frmRptPaymentIOU : Form
    {
        public frmRptPaymentIOU(){InitializeComponent();}
        private void frmRptPaymentIOU_Load(object sender, EventArgs e)
        {
            ucPaymentsReport1.form = this;
            ucPaymentsReport1.FormType = "IOU".Trim();
        }
        private void ucPaymentsReport1_Load(object sender, EventArgs e)
        {
        }
    }
}
