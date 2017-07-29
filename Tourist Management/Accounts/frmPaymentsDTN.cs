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
    public partial class frmPaymentsDTN : Form
    {
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public string SqlQry = "SELECT ID,TourID,Guest,DisplayName Company,VoucherID,PayableTo ReceivedFrom,PaidDate[Received Date],RefNo,Currency,Amount From Fun_ReturnPaySetTot() WHERE Type='DTN' Order By ID DESC";
        public frmPaymentsDTN(){InitializeComponent();}
        private void frmPaymentsREC_Load(object sender, EventArgs e)
        {
            ucPayments1.FormType = "DTN".Trim();
            ucPayments1.Mode = Mode;
            ucPayments1.SystemCode = SystemCode;
            ucPayments1.form = this;
            ucPayments1.Intializer();
            ucPayments1.gbChk.Enabled = false;
        }
    }
}
