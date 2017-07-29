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
    public partial class ucTransOther : UserControl
    {
        private const string msghd = "Other Instructions";
        public bool NotEnable = true;
        public ucTransOther()    {  InitializeComponent();     } 
        private void ucTransOther_Load(object sender, EventArgs e)
        {
            bool EnableOK = true;
            if (NotEnable)  EnableOK = false;
            rtbBillingIns.Enabled = EnableOK;
            btnBilling.Enabled = EnableOK;
            rtbOtherInstructions.Enabled = EnableOK;
            btnOther.Enabled = EnableOK;
            rtbArrangement.Enabled = EnableOK;
            rtbReferance.Enabled = EnableOK;
            rtbNotice.Enabled = EnableOK;
        }
        private void btnBilling_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under Construction !!!", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnOther_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under Construction !!!", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
