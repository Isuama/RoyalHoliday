using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmPaymentsIOU : Form
    {
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public string SqlQry = "SELECT ID,TourID,DisplayName Company,VoucherID,PayableTo,PaidDate,RefNo,Amount,Settled From Fun_ReturnIOUTot() Order By ID DESC";
        public frmPaymentsIOU(){InitializeComponent();}
        private void frmPaymentsIOU_Load(object sender, EventArgs e)
        {
            ucPayments1.FormType = "IOU".Trim();
            ucPayments1.Mode = Mode;
            ucPayments1.SystemCode = SystemCode;
            ucPayments1.form = this;
            ucPayments1.Intializer();
        }
        private void ucPayments1_Load(object sender, EventArgs e)
        {
        }
    }
}
