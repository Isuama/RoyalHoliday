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
    public partial class ucPaymentsReport : UserControl
    { 
        enum GRD { ID,TransID,TourID,Guest,CompanyID,Company,VoucherID,RefNo,PayableToID,PayableTo,PaidDate,Amount,Settled,Balance};
        private const string msghd = "Payment";
        string fType = ""; 
        Form frm;
        public string FormType { set { fType = value; } } 
        public Form form { set { frm = value; } }
        public ucPaymentsReport() { InitializeComponent(); }
        private void ucPaymentsReport_Load(object sender, EventArgs e) { Intializer(); }
        public void Intializer()
        {
            try
            {
                Fill_Control();
                Grd_Initializer();
                Set_Form_Type();
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void Fill_Control()
        {
            try
            { 
                cmbCompany.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpPaid.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID, Name FROM vw_ALL_PERSON_DETAILS");
                drpHandled.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                dtpFromDate.Value = Classes.clsGlobal.CurDate();
                dtpToDate.Value = Classes.clsGlobal.CurDate();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdPay, 1000, GRD.ID, 0, "ID", GRD.TransID, 0, "TransID", GRD.TourID, 0, "Tour ID", GRD.Guest, 0, "Guest", GRD.CompanyID, 0, "Company ID", GRD.Company, 0, "Company", GRD.VoucherID, 0, "Voucher ID", GRD.RefNo, 0, "Ref No", GRD.PayableToID, 0, "PayableTo ID", GRD.PayableTo, 0, "PayableTo", GRD.PaidDate, 0, "Paid Date", GRD.Amount, 0, "Amount", "##.##", GRD.Settled, 0, "Settled", "##.##", GRD.Balance, 0, "Balance", "##.##"); 

            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Set_Form_Type()
        {
            try
            {
                lblPayOrReceive.Text = "Paid To";
                if (fType == "IOU")
                {
                    lblName.Text = "I.O.U. Settlement";
                }
                else if (fType == "CPY")
                {
                    lblName.Text = "Cash Settlement";
                }
                else if (fType == "REC")
                {
                    lblName.Text = "Receipt Settlement";
                    lblPayOrReceive.Text = "Received From";
                }
                else if (fType == "CHQ")
                {
                    lblName.Text = "Cheque Settlement";
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            frm.Close();
        }
        private void chkFilterByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterByDate.Checked)
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
            }
        }
        private void chkCmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCmpny.Checked)
                cmbCompany.Enabled = true;
            else
                cmbCompany.Enabled = false;
        }
        private void chkAllHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllHandled.Checked)
                drpHandled.Enabled = true;
            else
                drpHandled.Enabled = false;
        }
        private void chkPaidBy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaidBy.Checked)
                drpPaid.Enabled = true;
            else
                drpPaid.Enabled = false;
        }
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            rdbSettled.Checked = false;
            rdbUnsettled.Checked = false;
            chkCancel.Checked = false;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            Fill_Data();
        }
        private void Fill_Data()
        {
            try
            {
                string sql;
                DataTable DT;
                sql = "SELECT DisplayName Company,TourID,Guest,HandledBy," +
                      "VoucherID,PayableTo,PaidDate,Amount,Settled,Balance,RefNo" +
                      " FROM vw_rpt_IOU";
                if (chkCancel.Checked)
                    sql += " WHERE IsCancelled=1";               
                else
                    sql += " WHERE IsCancelled<>1";
                if (rdbSettled.Checked)
                    sql += " AND Amount=Settled";
                if (rdbUnsettled.Checked)
                    sql += " AND Amount<>Settled";
                 if (chkFilterByDate.Checked)
                     sql += " AND cast(PaidDate as date)>='" + dtpFromDate.Value.ToString("yyyy-MM-dd") + "'" +
                           " AND cast(PaidDate as date)<='" + dtpToDate.Value.ToString("yyyy-MM-dd") + "'";
                 if (chkCmpny.Checked)
                     sql += " AND CompID=" + cmbCompany.SelectedValue.ToString().Trim() + "";
                 if (chkAllHandled.Checked)
                     sql += " AND HandledByID='" + drpHandled.SelectedValue.Trim() + "'";
                if(chkPaidBy.Checked)
                    sql += " AND HandledByID='" + drpHandled.SelectedValue.Trim() + "'";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                grdPay.DataSource = DT;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
