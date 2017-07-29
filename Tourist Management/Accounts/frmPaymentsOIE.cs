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
    public partial class frmPaymentsOIE : Form
    {
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public string SqlQry = "SELECT ID,DisplayName Company,VoucherID,PaidDate,RefNo From Fun_ReturnOIETot() Order By ID DESC";
        public frmPaymentsOIE(){InitializeComponent();}
        private void frmPaymentsOIE_Load(object sender, EventArgs e)
        {
            ucPayments1.FormType = "OIE".Trim();
            ucPayments1.Mode = Mode;
            ucPayments1.SystemCode = SystemCode;
            ucPayments1.form = this;
            ucPayments1.Intializer();
            ucPayments1.gbBasic.Enabled = false;
            ucPayments1.gbCurrency.Enabled = false;
            ucPayments1.gbChk.Enabled = false;
            #if PYTHON
            if (InsMode != 1)
            {
                int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccID FROM def_Account WHERE AccName='Bank'").Rows[0]["AccID"]);
                string SqlQuery = "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accTypeID + "";
                DataTable DTAll = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                ucPayments1.grdPay[1, 1] = DTAll.Rows[0]["AccountTypeID"];
                ucPayments1.grdPay[1, 2] = DTAll.Rows[0]["AccountType"];
                ucPayments1.grdPay[1, 9] = "0.00";
            }
            #endif
        }
    }
}
