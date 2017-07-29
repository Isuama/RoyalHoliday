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
    public partial class frmPaymentsCHQ : Form
    {
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public string SqlQry = "SELECT ID,TourID,DisplayName Company,VoucherID,PayableTo,PaidDate,ChkNo From Fun_ReturnCHQTot() Order By ID DESC";
        public frmPaymentsCHQ(){InitializeComponent();}
        private void frmPaymentsCHQ_Load(object sender, EventArgs e)
        {
            ucPayments1.FormType = "CHQ".Trim();
            ucPayments1.Mode = Mode;
            ucPayments1.SystemCode = SystemCode;
            ucPayments1.form = this;
            ucPayments1.Intializer();
            if (Mode != 1)
            {
                int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccID FROM def_Account WHERE AccName='Bank'").Rows[0]["AccID"]);
                string SqlQuery = "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accTypeID + "";
                DataTable DTAll = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                ucPayments1.grdPay[1, 1] = DTAll.Rows[0]["AccountTypeID"];
                ucPayments1.grdPay[1, 2] = DTAll.Rows[0]["AccountType"];
                ucPayments1.grdPay[1, 9] = "0.00";
            }
        }
    }
}
