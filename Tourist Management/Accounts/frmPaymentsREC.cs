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
    public partial class frmPaymentsREC : Form
    {
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public string SqlQry = "SELECT CP.ID,ISNULL(TBS.TourID,'X') TourID,Guest,DisplayName Company,VoucherID,APD.Name ReceivedFrom,PaidDate[Received Date],RefNo,Currency,(SELECT SUM(CPD.Debit) FROM dbo.act_CashPayment_Detail CPD WHERE CPD.CashPayID=CP.ID) Amount FROM dbo.act_CashPayment CP  LEFT JOIN vw_ALL_PERSON_DETAILS APD ON APD.ID=CP.PayableTo  LEFT JOIN trn_GroupAmendment TBS ON CP.TransID=TBS.ID LEFT JOIN  dbo.mst_CompanyGenaral CMP ON CP.CompID=CMP.ID LEFT OUTER JOIN dbo.mst_Currency CUR ON CP.CurrencyID=CUR.ID WHERE Type='REC' Order By ID DESC";
        public frmPaymentsREC(){InitializeComponent();}
        private void frmPaymentsREC_Load(object sender, EventArgs e)
        {
            ucPayments1.FormType = "REC".Trim();
            ucPayments1.Mode = Mode;
            ucPayments1.SystemCode = SystemCode;
            ucPayments1.form = this;
            ucPayments1.Intializer();
            ucPayments1.gbChk.Enabled = false;
        }
        private void ucPayments1_Load(object sender, EventArgs e)
        {
        }
    }
}
